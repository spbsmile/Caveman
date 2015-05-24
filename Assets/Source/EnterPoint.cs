using System;
using System.Collections;
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
        private readonly Player[] players = new Player[Settings.MaxCountPlayers];

        public SmoothCamera smoothCamera;
        public Transform containerWeaponsLyingPool;
        public Transform containerStoneSplashPool;
        public Transform containerWeaponsHandPool;
        public Transform containerDeathImagesPool;
        public Transform containerPlayers;
        public Transform result;
        public Text roundTime;
        public Text weapons;
        public Text killed;
       
        private float timeCurrentRespawnWeapon;

        private Player player;
        private Random r;
        private bool flagEnd;
        private int countRespawnWeappons = 1;

        private ObjectPool poolWeaponsLying;
        private ObjectPool poolWeaponsHand;
        private ObjectPool poolstoneSplash;
        private ObjectPool poolDeathImage;

        public void Start()
        {
            r = new Random();

            poolWeaponsLying = containerWeaponsLyingPool.GetComponent<ObjectPool>();
            poolWeaponsLying.CreatePool(prefabLyingWeapon, 30);
            for (var i = 0; i < 30; i++)
            {
                var weaponGo = Instantiate(prefabLyingWeapon);
                weaponGo.SetParent(poolWeaponsLying.transform);
                var weaponModel = weaponGo.GetComponent<BaseWeaponModel>();
                weaponModel.SetPool(poolWeaponsLying);
                poolWeaponsLying.Store(weaponModel.transform);
            }

            poolWeaponsHand = containerWeaponsHandPool.GetComponent<ObjectPool>();
            poolWeaponsHand.CreatePool(prefabStoneIns, 30);
            for (var i = 0; i < 30; i++)
            {
                var weaponGo = Instantiate(prefabStoneIns);
                weaponGo.SetParent(poolWeaponsHand.transform);
                var weaponModel = weaponGo.GetComponent<WeaponModel>();
                weaponModel.SetPool(poolWeaponsHand);
                poolWeaponsHand.Store(weaponModel.transform);
            }

            poolstoneSplash = containerStoneSplashPool.GetComponent<ObjectPool>();
            poolstoneSplash.CreatePool(prefabStoneFlagmentInc, 30);
            for (var i = 0; i < 30; i++)
            {
                var splashGo = Instantiate(prefabStoneFlagmentInc);
                splashGo.SetParent(poolstoneSplash.transform);
                var splash = splashGo.GetComponent<StoneSplash>();
                poolstoneSplash.Store(splash.transform);
            }

            poolDeathImage = containerDeathImagesPool.GetComponent<ObjectPool>();
            poolDeathImage.CreatePool(prefabDeathImage, 8);
            for (int i = 0; i < 8; i++)
            {
                var deathImageGo = Instantiate(prefabDeathImage);
                deathImageGo.SetParent(containerDeathImagesPool);
                poolDeathImage.Store(deathImageGo.transform);
            }

            CreatePlayer(new Player("Zabiyakin"));
            CreateAiPlayers();
            CreateAllLyingWeapons();
        }

        public void Update()
        {
            // todo use events!
            var remainTime = Settings.RoundTime - Math.Floor(Time.timeSinceLevelLoad);
            var displayTime = remainTime > 60 ? "1 : " + (remainTime - 60) : remainTime.ToString();
            roundTime.text = "Round Time " + displayTime;
            if (remainTime < 0 && !flagEnd)
            {
                flagEnd = true; 
                result.gameObject.SetActive(true);
                StartCoroutine(DisplayResult());
            }

            weapons.text = player.weapons.ToString();
            killed.text = player.kills.ToString();
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
                Write(players[i].name, namePlayer, axisY);
                Write(players[i].kills.ToString(), kills, axisY);
                Write(players[i].deaths.ToString(), deaths, axisY);
                axisY -= deltaY;
            }
        }

        private void Write(string value, Transform parent, int axisY)
        {
            var goName = Instantiate(Resources.Load(PrefabText, typeof (GameObject))) as GameObject;
            goName.transform.SetParent(parent);
            goName.transform.localPosition = new Vector2(-2, axisY);
            goName.transform.rotation = Quaternion.identity;
            goName.GetComponent<Text>().text = value;
        }

        private void CreateAiPlayers()
        {
            for (var i = 0; i < Settings.BotsCount; i++)
            {
                players[i + 1] = new Player(names[i]);
                CreateAiPlayer(players[i + 1]);
            }
        }

        private void CreateAllLyingWeapons()
        {
            for (var i = 0; i < Settings.WeaponsCount; i++)
            {
                CreateLyingWeapon();
            }
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
            for (float i = 1f; i > 0; i -= 0.1f)
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

        private void CreatePlayer(Player player)
        {
            players[0] = player;
            var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof(GameObject))) as GameObject;
            var playerModel = prefabPlayer.GetComponent<PlayerModel>();
            playerModel.transform.SetParent(containerPlayers);
            this.player = player;
            playerModel.Init(player, Vector2.zero, r);
            playerModel.Respawn += player1 => StartCoroutine(RespawnPlayer(player1));
            playerModel.Death += DeathAnimate;
            playerModel.ThrowStone += CreateWeapon;
            smoothCamera.target = prefabPlayer.transform;
        }

        private void CreateAiPlayer(Player player)
        {
            var prefabPlayer = Instantiate(Resources.Load(PrefabBot, typeof(GameObject))) as GameObject;
            var modelAiPlayer = prefabPlayer.GetComponent<AiPlayerModel>();
            modelAiPlayer.transform.SetParent(containerPlayers);
            modelAiPlayer.Init(player,
                new Vector2(r.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom), r.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom)), r);
            modelAiPlayer.SetWeapons(containerWeaponsLyingPool);
            modelAiPlayer.Respawn += player1 => StartCoroutine(RespawnAiPlayer(player1));
            modelAiPlayer.Death += DeathAnimate;
            modelAiPlayer.ThrowStone += CreateWeapon;
        }

        private IEnumerator RespawnAiPlayer(Player player)
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            CreateAiPlayer(player);
        }

        private IEnumerator RespawnPlayer(Player player)
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            CreatePlayer(player);
        }

        private void CreateWeapon(Player owner, Vector2 start, Vector2 target)
        {
            var stone = poolWeaponsHand.New();
            var weaponModel = stone.GetComponent<WeaponModel>();
            if (weaponModel.PoolIsEmty)
            {
                weaponModel.SetPool(poolWeaponsHand);
            }
            weaponModel.Splash += CreateStoneFlagment;
            weaponModel.Move(owner, start, target);
        }

        private void CreateLyingWeapon()
        {
            var prefabWeapons = poolWeaponsLying.New();
            prefabWeapons.SetParent(poolWeaponsLying.transform);
            StartCoroutine(FadeIn(prefabWeapons.GetComponent<SpriteRenderer>()));
            var weaponModel = prefabWeapons.GetComponent<BaseWeaponModel>();
            if (weaponModel.PoolIsEmty)
            {
                weaponModel.SetPool(poolWeaponsLying);
            }
            prefabWeapons.transform.position = new Vector2(r.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom),
                r.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom));
        }

        private void CreateStoneFlagment(Vector2 position)
        {
            for (var i = 0; i < 4; i++)
            {
                var flagment = poolstoneSplash.New();
                flagment.GetComponent<StoneSplash>().Init(i, position, poolstoneSplash);
            }
        }
    }
}
