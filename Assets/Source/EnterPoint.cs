using System;
using System.Collections;
using Caveman.Animation;
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
    public class EnterPoint : MonoBehaviour
    {
        protected const string IdHostPlayer = "0";

        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;

        public SkullModel prefabSkull;
        public StoneSplash prefabStoneFlagmentInc;
        public StoneModel prefabStone;
        public Transform prefabDeathImage;
        public SpeedBonus prefabBonusSpeed;

        public SmoothCamera smoothCamera;
        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerDeathImages;
        public Transform containerPlayers;
        public Transform containerBonusesSpeed;
        public Transform containerBonusesForce;
        public Transform containerBonusesShield;

        protected Random r;
        protected ServerConnection serverConnection;
        protected PlayerPool poolPlayers;
        protected ObjectPool<WeaponModelBase> poolStones;
        protected ObjectPool<WeaponModelBase> poolSkulls;
        protected ObjectPool<BonusBase> poolBonusesSpeed;
        protected ObjectPool<StoneSplash> poolStonesSplash;
        //protected ObjectPool<Transform> poolDeathImage;
        
        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        public virtual void Start()
        {
            r = new Random();

            poolStonesSplash = CreatePool<StoneSplash>(Settings.PoolCountSplashStones, containerSplashStones, prefabStoneFlagmentInc, null);
            //poolDeathImage = CreatePool<Transform>(Settings.PoolCountDeathImages, containerDeathImages, prefabDeathImage, null);
            poolStones = CreatePool<WeaponModelBase>(Settings.PoolCountStones, containerStones, prefabStone, InitStoneModel);
            poolSkulls = CreatePool<WeaponModelBase>(Settings.PoolCountSkulls, containerSkulls, prefabSkull, InitSkullModel);
            poolBonusesSpeed = CreatePool<BonusBase>(Settings.PoolCountBonusesSpeed, containerBonusesSpeed, prefabBonusSpeed, InitBonusModel);

            poolStones.RelatedPool += () => poolStonesSplash;

            poolPlayers = containerPlayers.GetComponent<PlayerPool>();
          //  poolPlayers.Init();
            var humanPlayer = new Player("Zabiyakin");
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
            BattleGui.instance.resultRound.SetPlayerPool(poolPlayers);
            CreatePlayer(humanPlayer, false, IdHostPlayer);
            
            if (serverConnection == null)
            {
                for (var i = 0; i < Settings.BotsCount; i++)
                {
                    CreatePlayer(new Player(names[i]), true, i.ToString());
                }
                StartCoroutine(PutWeapons());
                StartCoroutine(PutBonuses());
            }
        }

        private void InitBonusModel(GameObject item, ObjectPool<BonusBase> pool)
        {
            var bonusModel = item.GetComponent<BonusBase>();
            bonusModel.Init(pool, r, Settings.DurationBonusSpeed);
        }

        private void InitSkullModel(GameObject item, ObjectPool<WeaponModelBase> pool) 
        {
            item.GetComponent<SkullModel>().SetPool(pool);
        }

        private void InitStoneModel(GameObject item, ObjectPool<WeaponModelBase> pool)
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

        private void PutItem<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(r.Next(Settings.WidthMap),
                r.Next(Settings.HeightMap));
        }

        private ObjectPool<T> CreatePool<T>(int initialBufferSize, Transform container, T prefab, Action<GameObject, ObjectPool<T>> init) where T : MonoBehaviour
        {
            var pool = container.GetComponent<ObjectPool<T>>();
            pool.CreatePool(prefab, initialBufferSize, serverConnection != null);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                if (init != null)
                {
                    init(item.gameObject, pool);
                }
                item.transform.SetParent(container);
                pool.Store(item);
            }
            return pool;
        }

        private void CreatePlayer(Player player, bool isAiPlayer, string id)
        {
            PlayerModelBase playerModel;
            if (isAiPlayer)
            {
                var prefab = Instantiate(prefabAiPlayer);
                playerModel = prefab.GetComponent<AiPlayerModel>();
                (playerModel as AiPlayerModel).InitAi(player,
                new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)), r, poolPlayers, containerStones);
            }
            else
            {
                var prefab = Instantiate(prefabHumanPlayer);
                playerModel = prefab.GetComponent<HumanPlayerModel>();
                smoothCamera.target = prefab.transform;
                playerModel.Init(player, new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)), r, poolPlayers, serverConnection);
            }
            poolPlayers.Add(id, playerModel);
            playerModel.transform.SetParent(containerPlayers);
            //playerModel.Death += position => StartCoroutine(DeathAnimate(position));
            playerModel.ChangedWeapons += ChangedWeapons;
        }

        private ObjectPool<WeaponModelBase> ChangedWeapons(WeaponType weaponType)
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

        ////todo вынести два метода и есть бага у этого метода
        //private IEnumerator DeathAnimate(Vector2 position)
        //{
        //    var deathImage = poolDeathImage.New();
        //    deathImage.transform.position = position;
        //    var spriteRenderer = deathImage.GetComponent<SpriteRenderer>();
        //    if (spriteRenderer)
        //    {
        //        yield return UnityExtensions.FadeOut(spriteRenderer);
        //    }
        //    poolDeathImage.Store(spriteRenderer.transform);
        //}
    }
}
