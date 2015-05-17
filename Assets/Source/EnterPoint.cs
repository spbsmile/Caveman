using System;
using System.Collections;
using Caveman.Players;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        private const string PrefabPlayer = "player";
        private const string PrefabBot = "bot";
        private const string PrefabLyingWeapon = "stone_bunch";
        private const string PrefabText = "Text";
        private const string PrefabDeath = "dead";
        private const string PrefabStoneFlagment = "stone";

        private const string PrefabStone = "weapon";
        public Transform prefabStoneIns;
        public Transform prefabStoneFlagmentInc;
        public Transform prefabLyingWeapon;

        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };
        private readonly Player[] players = new Player[Settings.MaxCountPlayers];

        public SmoothCamera smoothCamera;
        public Transform containerWeapons;
        public Transform containerPlayers;
        public Transform result;
        public Text roundTime;
        public Text weapons;
        public Text killed;
       
        private float timeCurrentRespawnWeapon;

        private Player player;
        private Random random;
        private bool flagEnd;
        private int countRespawnWeappons = 1;

        public void Start()
        {
            random = new Random();
            CreatePlayer(new Player("Zabiyakin"));
            CreateAiPlayers();
            CreateAllWeapons();
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
                CreateAllWeapons();
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

        private void CreateAllWeapons()
        {
            for (var i = 0; i < Settings.WeaponsCount; i++)
            {
                CreateLyingWeapon();
            }
        }

        private void DeathAnimate(Vector2 position)
        {
            var deathImage = Instantiate(Resources.Load(PrefabDeath, typeof(GameObject)) as GameObject, position, Quaternion.identity) as GameObject;
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
            Destroy(spriteRenderer.gameObject);
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
            var playerModel = prefabPlayer.GetComponent<ModelPlayer>();
            playerModel.transform.SetParent(containerPlayers);
            this.player = player;
            playerModel.Init(player, Vector2.zero, random);
            playerModel.Respawn += player1 => StartCoroutine(RespawnPlayer(player1));
            playerModel.Death += DeathAnimate;
            playerModel.ThrowStone += CreateStone;
            smoothCamera.target = prefabPlayer.transform;
        }

        private void CreateAiPlayer(Player player)
        {
            var prefabPlayer = Instantiate(Resources.Load(PrefabBot, typeof(GameObject))) as GameObject;
            var modelAiPlayer = prefabPlayer.GetComponent<ModelAIPlayer>();
            modelAiPlayer.transform.SetParent(containerPlayers);
            modelAiPlayer.Init(player,
                new Vector2(random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom), random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom)), random);
            modelAiPlayer.SetWeapons(containerWeapons);
            modelAiPlayer.Respawn += player1 => StartCoroutine(RespawnAiPlayer(player1));
            modelAiPlayer.Death += DeathAnimate;
            modelAiPlayer.ThrowStone += CreateStone;
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

        // todo use Object pool pattern
        private void CreateStone(Player owner, Vector2 start, Vector2 target)
        {
            var stone = Instantiate(prefabStoneIns);
            var weaponModel = stone.GetComponent<WeaponModel>();
            weaponModel.Splash += CreateStoneFlagment;
            weaponModel.Move(owner, start, target);
        }

        private void CreateLyingWeapon()
        {
            var prefabWeapons = Instantiate(prefabLyingWeapon);
            var sprite = prefabWeapons.GetComponent<SpriteRenderer>();
            StartCoroutine(FadeIn(sprite));
            var modelWeapon = prefabWeapons.gameObject.AddComponent<WeaponModel>();
            modelWeapon.transform.SetParent(containerWeapons);
            modelWeapon.transform.position = new Vector2(random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom),
                random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom));
        }

        private void CreateStoneFlagment(Vector2 position)
        {
            for (int i = 0; i < 2; i++)
            {
                var flagment =
                    Instantiate(prefabStoneFlagmentInc);
                flagment.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                flagment.GetComponent<StoneSplash>().Init(i, position);
            }
        }
    }
}
