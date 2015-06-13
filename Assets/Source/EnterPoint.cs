using System;
using System.Collections;
using Caveman.Level;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        private const string PrefabPlayer = "player";
        private const string PrefabBot = "bot";
        private const string PrefabText = "Text";
        
        public Transform prefabSkull;
        public Transform prefabStoneFlagmentInc;
        public Transform prefabStone;
        public Transform prefabDeathImage;

        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        public SmoothCamera smoothCamera;
        public Transform containerStonesPool;
        public Transform containerStoneSplashPool;
        public Transform containerSkullsPool;
        public Transform containerDeathImagesPool;
        public Transform containerPlayerPool;
        public Transform result;
        public Text roundTime;
        public Text weapons;
        public Text killed;
       
        private Player humanPlayer;
        private Random r;
        private bool flagEnd;

        private ObjectPool poolStones;
        private ObjectPool poolSkulls;
        private ObjectPool poolStonesSplash;
        private ObjectPool poolDeathImage;
        private PlayerPool poolPlayers;
        private Transform[] arrayStones;

        public void Start()
        {
            r = new Random();
            Player.idCounter = 0;

            poolStones = CreatePool(Settings.PoolCountStones, containerStonesPool, prefabStone);
            poolSkulls = CreatePool(Settings.PoolCountSkulls, containerSkullsPool, prefabSkull);
            poolStonesSplash = CreatePool(Settings.PoolCountSplashStones, containerStoneSplashPool, prefabStoneFlagmentInc);
            poolDeathImage = CreatePool(Settings.PoolCountDeathImages, containerDeathImagesPool, prefabDeathImage);
            arrayStones = poolStones.ToArray();

            poolPlayers = containerPlayerPool.GetComponent<PlayerPool>();
            poolPlayers.Init(Settings.BotsCount + 1);
            CreatePlayer(new Player("Zabiyakin"), false);
            for (var i = 0; i < Settings.BotsCount; i++)
            {
                CreatePlayer(new Player(names[i]), true);
            }
            PutWeapons();

            humanPlayer.WeaponsCountChanged += WeaponsCountChanged;
            humanPlayer.KillsCountChanged += KillsCountChanged;

            Invoke("PutWeapons", Settings.TimeRespawnWeapon);

            //todo temp
            for (int i = 0; i < arrayStones.Length; i++)
            {
                arrayStones[i].GetComponent<StoneModel>().SetPoolSplash(poolStonesSplash);
            }
         
        }

        public void Update()
        {
            var remainTime = Settings.RoundTime - Math.Floor(Time.timeSinceLevelLoad);
            var displayTime = remainTime > 60 ? "1 : " + (remainTime - 60) : remainTime.ToString();
            roundTime.text = "Round Time " + displayTime;
            if (remainTime < 0 && !flagEnd)
            {
                flagEnd = true; 
                result.gameObject.SetActive(true);
                StartCoroutine(DisplayResult());
            }
        }

        private IEnumerator DisplayResult()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0.00001f;

            var namePlayer = result.GetChild(1);
            var kills = result.GetChild(2);
            var deaths = result.GetChild(3);
            var axisY = -20;
            const int deltaY = 30;
            for (var i = 0; i < Settings.BotsCount + 1; i++)
            {
                Write(poolPlayers.GetName(i), namePlayer, axisY);
                Write(poolPlayers.GetKills(i), kills, axisY);
                Write(poolPlayers.GetDeaths(i), deaths, axisY);
                axisY -= deltaY;
            }
        }

        private void WeaponsCountChanged(int count)
        {
            weapons.text = count.ToString();
        }

        private void KillsCountChanged(int count)
        {
            killed.text = count.ToString();
        }

        private void Write(string value, Transform parent, int axisY)
        {
            var goName = Instantiate(Resources.Load(PrefabText, typeof (GameObject))) as GameObject;
            goName.transform.SetParent(parent);
            goName.transform.localPosition = new Vector2(-2, axisY);
            goName.transform.rotation = Quaternion.identity;
            goName.GetComponent<Text>().text = value;
        }

        private void PutWeapons()
        {
            for (var i = 0; i < Settings.InitialLyingWeapons; i++)
            {
                PutWeapon(poolStones);
            }
            Invoke("PutWeapons", Settings.TimeRespawnWeapon);
        }

        private void CreatePlayer(Player player, bool isAiPlayer)
        {
            BasePlayerModel playerModel;
            if (isAiPlayer)
            {
                var prefab = Instantiate(Resources.Load(PrefabBot, typeof(GameObject))) as GameObject;
                playerModel = prefab.GetComponent<AiPlayerModel>();
                var ai = (AiPlayerModel) playerModel;
                ai.InitAi(player,
                new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)), r, poolPlayers, arrayStones);
            }
            else
            {
                var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof(GameObject))) as GameObject;
                humanPlayer = player;
                playerModel = prefabPlayer.GetComponent<PlayerModel>();
                smoothCamera.target = prefabPlayer.transform;
                playerModel.Init(player, Vector2.zero, r, poolPlayers);
            }
            poolPlayers.Add(player.id, playerModel);
            playerModel.transform.SetParent(containerPlayerPool);
            playerModel.Respawn += player1 => StartCoroutine(RespawnPlayer(player));
            playerModel.Death += DeathAnimate;
            playerModel.ChangedWeapons += ChangedWeapons;
        }

        private ObjectPool ChangedWeapons(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Stone:
                    return poolStones;
                case  WeaponType.Skull:
                    return poolSkulls;
            }
            return null;
        }

        private IEnumerator RespawnPlayer(Player player)
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            poolPlayers.New(player.id).RandomPosition();
        }

        private void PutWeapon(ObjectPool pool)
        {
            var stone = pool.New();
            StartCoroutine(FadeIn(stone.GetComponent<SpriteRenderer>()));
            var weaponModel = stone.GetComponent<BaseWeaponModel>();
            if (weaponModel.PoolIsEmty)
            {
                weaponModel.SetPool(poolStones);
                weaponModel.transform.SetParent(containerStonesPool);
            }
            stone.transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br),
                r.Next(-Settings.Br, Settings.Br));
        }

        private ObjectPool CreatePool(int initialBufferSize, Transform container, Transform prefab)
        {
            var pool = container.GetComponent<ObjectPool>();
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var gO = Instantiate(prefab);
                gO.SetParent(container);
                pool.Store(gO.transform);
            }
            return pool;
        }

        private void DeathAnimate(Vector2 position)
        {
            var deathImage = poolDeathImage.New();
            deathImage.position = position;
            var sprite = deathImage.GetComponent<SpriteRenderer>();
            if (sprite)
            {
                StartCoroutine(FadeOut(sprite));
            }
        }

        private IEnumerator FadeOut(SpriteRenderer spriteRenderer)
        {
            for (var i = 1f; i > 0; i -= 0.1f)
            {
                var c = spriteRenderer.color;
                c.a = i;
                spriteRenderer.color = c;
                yield return null;
            }
            poolDeathImage.Store(spriteRenderer.transform);
        }

        private IEnumerator FadeIn(SpriteRenderer spriteRenderer)
        {
            var color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;
            for (var i = 0f; i < 1; i += 0.1f)
            {
                if (spriteRenderer)
                {
                    var c = spriteRenderer.color;
                    c.a = i;
                    spriteRenderer.color = c;
                }
                yield return null;
            }
        }
    }
}
