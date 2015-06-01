using System;
using System.Collections;
using Caveman.Animation;
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
        
        public Transform prefabStoneIns;
        public Transform prefabStoneFlagmentInc;
        public Transform prefabLyingWeapon;
        public Transform prefabDeathImage;

        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        public SmoothCamera smoothCamera;
        public Transform containerWeaponsLyingPool;
        public Transform containerStoneSplashPool;
        public Transform containerWeaponsHandPool;
        public Transform containerDeathImagesPool;
        public Transform containerPlayerPool;
        public Transform result;
        public Text roundTime;
        public Text weapons;
        public Text killed;
       
        private float timeCurrentRespawnWeapon;

        private Player humanPlayer;
        private Random r;
        private bool flagEnd;
        private int countRespawnWeappons = 1;

        private ObjectPool poolWeaponsLying;
        private ObjectPool poolWeaponsHand;
        private ObjectPool poolstoneSplash;
        private ObjectPool poolDeathImage;
        private PlayerPool poolPlayers;
        private Transform[] arrayLyingWeapons;

        public void Start()
        {
            r = new Random();
            Player.idCounter = 0;

            poolWeaponsLying = CreatePool(30, containerWeaponsLyingPool, prefabLyingWeapon);
            poolWeaponsHand = CreatePool(30, containerWeaponsHandPool, prefabStoneIns);
            poolstoneSplash = CreatePool(30, containerStoneSplashPool, prefabStoneFlagmentInc);
            poolDeathImage = CreatePool(8, containerDeathImagesPool, prefabDeathImage);
            arrayLyingWeapons = poolWeaponsLying.ToArray();

            poolPlayers = containerPlayerPool.GetComponent<PlayerPool>();
            poolPlayers.Init(Settings.BotsCount + 1);
            CreatePlayer(new Player("Zabiyakin"), false);
            for (var i = 0; i < Settings.BotsCount; i++)
            {
                CreatePlayer(new Player(names[i]), true);
            }
            CreateAllLyingWeapons();

            humanPlayer.WeaponsCountChanged += WeaponsCountChanged;
            humanPlayer.KillsCountChanged += KillsCountChanged;
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

            timeCurrentRespawnWeapon = countRespawnWeappons * Settings.TimeRespawnWeapon - Time.timeSinceLevelLoad;
            if (timeCurrentRespawnWeapon-- < 0)
            {
                CreateAllLyingWeapons();
                countRespawnWeappons++;
                timeCurrentRespawnWeapon = Settings.TimeRespawnWeapon;
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

        private void CreateAllLyingWeapons()
        {
            for (var i = 0; i < Settings.WeaponsCount; i++)
            {
                CreateLyingWeapon();
            }
        }

        private void CreatePlayer(Player player, bool isAiPlayer)
        {
            BasePlayerModel playerModel;
            if (isAiPlayer)
            {
                var prefab = Instantiate(Resources.Load(PrefabBot, typeof(GameObject))) as GameObject;
                playerModel = prefab.GetComponent<AiPlayerModel>();
                var ai = (AiPlayerModel) playerModel;
                ai.SetWeapons(arrayLyingWeapons);
                playerModel.Init(player,
                new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)), r, poolPlayers);
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
            playerModel.ThrowStone += ThrowStone;
        }

        private IEnumerator RespawnPlayer(Player player)
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            poolPlayers.New(player.id).RandomPosition();
        }

        // todo метод перенести в файл игрока
        private void ThrowStone(Player owner, Vector2 start, Vector2 target)
        {
            var stone = poolWeaponsHand.New();
            var weaponModel = stone.GetComponent<StoneModel>();
            if (weaponModel.PoolIsEmty)
            {
                weaponModel.SetPool(poolWeaponsHand);
            }
            if (weaponModel.Splash == null)
            {
                weaponModel.Splash += CreateStoneFlagment;    
            }
            weaponModel.Move(owner, start, target);
        }
         

        private void CreateLyingWeapon()
        {
            var prefabWeapons = poolWeaponsLying.New();
            StartCoroutine(FadeIn(prefabWeapons.GetComponent<SpriteRenderer>()));
            var weaponModel = prefabWeapons.GetComponent<BaseWeaponModel>();
            if (weaponModel.PoolIsEmty)
            {
                weaponModel.SetPool(poolWeaponsLying);
                weaponModel.transform.SetParent(containerWeaponsLyingPool);
            }
            prefabWeapons.transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br),
                r.Next(-Settings.Br, Settings.Br));
        }

        private void CreateStoneFlagment(Vector2 position)
        {
            for (var i = 0; i < 4; i++)
            {
                var flagment = poolstoneSplash.New();
                flagment.GetComponent<StoneSplash>().Init(i, position, poolstoneSplash);
            }
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
