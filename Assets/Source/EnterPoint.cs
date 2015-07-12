using System;
using System.Collections;
using Caveman.Bonuses;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Setting;
using Caveman.UI;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour, IServerListener
    {
        public bool multiplayer;

        public Transform prefabSkull;
        public Transform prefabStoneFlagmentInc;
        public Transform prefabStone;
        public Transform prefabDeathImage;
        public Transform prefabPlayer;
        public Transform prefabBot;
        public Transform prefabBonusSpeed;

        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        public SmoothCamera smoothCamera;
        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerDeathImages;
        public Transform containerPlayers;
        public Transform containerBonusesSpeed;
        public Transform containerBonusesForce;
        public Transform containerBonusesShield;

        private Random r;
        private ObjectPool poolStones;
        private ObjectPool poolSkulls;
        private ObjectPool poolStonesSplash;
        private ObjectPool poolDeathImage;
        private PlayerPool poolPlayers;
        private ObjectPool poolBonusesSpeed;
        private ObjectPool poolBonusesForce;
        private ObjectPool poolBonusesShield;

        ServerConnection serverConnection;

        public void Start()
        {
            r = new Random();
            Player.idCounter = 0;


            if (multiplayer)
            {
                serverConnection = new ServerConnection();
                serverConnection.ServerListener = this;
                serverConnection.StartSession(SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName);
                //todo valid value!!
                serverConnection.SendRespawn(Vector2.zero);
            }

            poolDeathImage = CreatePool(Settings.PoolCountDeathImages, containerDeathImages, prefabDeathImage, null);
            poolStonesSplash = CreatePool(Settings.PoolCountSplashStones, containerSplashStones, prefabStoneFlagmentInc, null);
            poolStones = CreatePool(Settings.PoolCountStones, containerStones, prefabStone, InitStoneModel);
            poolSkulls = CreatePool(Settings.PoolCountSkulls, containerSkulls, prefabSkull, InitSkullModel);
            poolBonusesSpeed = CreatePool(Settings.PoolCountBonusesSpeed, containerBonusesSpeed, prefabBonusSpeed, InitBonusModel);

            poolStones.RelatedPool += () => poolStonesSplash;

            poolPlayers = containerPlayers.GetComponent<PlayerPool>();
            poolPlayers.Init(Settings.BotsCount + 1);
            var humanPlayer = new Player("Zabiyakin");
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
            CreatePlayer(humanPlayer, false);
            for (var i = 0; i < Settings.BotsCount; i++)
            {
                CreatePlayer(new Player(names[i]), true);
            }

            if (!multiplayer)
            {
                StartCoroutine(PutWeapons());
                StartCoroutine(PutBonuses());
            }
        }

        private void InitBonusModel(GameObject item, ObjectPool pool)
        {
            var bonusModel = item.GetComponent<BonusBase>();
            bonusModel.Init(pool, r, Settings.DurationBonusSpeed);
        }

        private void InitSkullModel(GameObject item, ObjectPool pool)
        {
            item.GetComponent<SkullModel>().SetPool(pool);
        }

        private void InitStoneModel(GameObject item, ObjectPool pool)
        {
            var model = item.GetComponent<StoneModel>();
            model.SetPool(pool);
            model.SetPoolSplash(poolStonesSplash);
        }

        private IEnumerator PutBonuses()
        {
            for (var i = 0; i < Settings.InitalCountBonusesSpeed; i++)
            {
                PutItem(poolBonusesSpeed);
            }
            yield return new WaitForSeconds(Settings.TimeRespawnBonuses);
            StartCoroutine(PutBonuses());
        }

        private IEnumerator PutWeapons()
        {
            for (var i = 0; i < Settings.InitialLyingWeapons; i++)
            {
                PutItem(poolStones);
            }
            for (var i = 0; i < Settings.CountLyingSkulls; i++)
            {
                PutItem(poolSkulls);
            }
            yield return new WaitForSeconds(Settings.TimeRespawnWeapon);
            StartCoroutine(PutWeapons());
        }

        private void PutItem(ObjectPool pool)
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br),
                r.Next(-Settings.Br, Settings.Br));
        }

        private ObjectPool CreatePool(int initialBufferSize, Transform container, Transform prefab, Action<GameObject, ObjectPool> init)
        {
            var pool = container.GetComponent<ObjectPool>();
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                if (init != null)
                {
                    init(item.gameObject, pool);
                }
                item.SetParent(container);
                pool.Store(item.transform);
            }
            return pool;
        }

        private void CreatePlayer(Player player, bool isAiPlayer)
        {
            PlayerModelBase playerModel;
            if (isAiPlayer)
            {
                var prefab = Instantiate(prefabBot);
                playerModel = prefab.GetComponent<AiPlayerModel>();
                (playerModel as AiPlayerModel).InitAi(player,
                new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)), r, poolPlayers, containerStones);
            }
            else
            {
                var prefab = Instantiate(prefabPlayer);
                playerModel = prefab.GetComponent<PlayerModel>();
                smoothCamera.target = prefab.transform;
                playerModel.Init(player, Vector2.zero, r, poolPlayers, serverConnection);
            }
            poolPlayers.Add(player.id, playerModel);
            playerModel.transform.SetParent(containerPlayers);
            playerModel.Respawn += player1 => StartCoroutine(RespawnPlayer(player));
            playerModel.Death += position => StartCoroutine(DeathAnimate(position));
            playerModel.ChangedWeapons += ChangedWeapons;
        }

        private ObjectPool ChangedWeapons(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Stone:
                    return poolStones;
                case WeaponType.Skull:
                    return poolSkulls;
            }
            return null;
        }


        // todo вынести из файла!
        private IEnumerator RespawnPlayer(Player player)
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            var pl = poolPlayers.New(player.id).GetComponent<PlayerModelBase>();
            pl.transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
            StartCoroutine(pl.ThrowOnTimer());
        }

        private IEnumerator DeathAnimate(Vector2 position)
        {
            var deathImage = poolDeathImage.New();
            deathImage.position = position;
            var spriteRenderer = deathImage.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                yield return UnityExtensions.FadeOut(spriteRenderer);
            }
            poolDeathImage.Store(spriteRenderer.transform);
        }

        // todo multiplayer
        public void WeaponAddedReceived(Vector2 point)
        {
            Debug.Log("stone added : " + point);
            //todo var value size map 
            poolStones.New().transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
        }

        public void BonusAddedReceived(Vector2 point)
        {
            throw new NotImplementedException();
        }

        public void PlayerDeadResceived(string playerId, Vector2 point)
        {
            throw new NotImplementedException();
        }

        public void WeaponRemovedReceived(Vector2 point)
        {
            print(string.Format("WeaponRemovedReceived {0}", point));
            //poolStones.Store(-);
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
            print(string.Format("MoveReceived {0} by playerId {1}", point, playerId));
        }

        public void LoginReceived(string playerId)
        {
            print(string.Format("LoginReceived {0} by playerId {1}", playerId));
        }

        public void PickWeaponReceived(string playerId, Vector2 point)
        {
            print(string.Format("PickWeaponReceived {0} by playerId {1}", point, playerId));
        }

        public void PickBonusReceived(string playerId, Vector2 point)
        {
            print(string.Format("PickBonusReceived {0} by playerId {1}", point, playerId));
        }

        public void UseWeaponReceived(string playerId, Vector2 point)
        {
            Debug.Log(string.Format("UseWeaponReceived {0} by playerId {1}", point, playerId));
        }

        public void RespawnReceived(string playerId, Vector2 point)
        {
            Debug.Log(string.Format("RespawnReceived {0} by playerId {1}", point, playerId));
        }
    }
}
