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
        private const string PrefabPlayer = "skin_1";
        private const string PrefabWeapon = "stone_bunch";
        private const string PrefabText = "Text";
        private const int BoundaryRandom = 7;
        private const int MaxCountPlayers = 10;
        private const int RoundTime = 10;
        private const int TimeRespawnWeapon = 2;
        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };
        private readonly Player[] players = new Player[MaxCountPlayers];

        public SmoothCamera smoothCamera;
        public Transform containerWeapons;
        public Transform containerPlayers;
        public Transform result;
        public Text roundTime;
        public Text weapons;
        public Text killed;
        public int botsCount = 4 ;
        public int weaponsCount = 5;
       
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
            CreateWeapons();
        }

        public void Update()
        {
            var remainTime = RoundTime - Math.Floor(Time.time);
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
            timeCurrentRespawnWeapon = countRespawnWeappons*TimeRespawnWeapon - Time.time;
            if (timeCurrentRespawnWeapon-- < 0)
            {
                CreateWeapon();
                countRespawnWeappons++;
                timeCurrentRespawnWeapon = TimeRespawnWeapon;
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
            for (var i = 0; i < botsCount + 1; i++)
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
            for (var i = 0; i < botsCount; i++)
            {
                players[i + 1] = new Player(names[i]);
                CreateAiPlayer(players[i + 1]);
            }
        }

        private void CreateWeapons()
        {
            for (var i = 0; i < weaponsCount; i++)
            {
                CreateWeapon();
            }
        }

        private void CreateAiPlayer(Player player)
        {
            var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof (GameObject))) as GameObject;
            var modelAiPlayer = prefabPlayer.AddComponent<ModelAIPlayer>();
            modelAiPlayer.transform.SetParent(containerPlayers);
            modelAiPlayer.Init(player,
                new Vector2(random.Next(-BoundaryRandom, BoundaryRandom), random.Next(-BoundaryRandom, BoundaryRandom)), random);
            modelAiPlayer.SetWeapons(containerWeapons);
            modelAiPlayer.Respawn += player1 => StartCoroutine(RespawnAiPlayer(player1));
        }

        IEnumerator RespawnAiPlayer(Player player)
        {
            yield return new WaitForSeconds(1);
            CreateAiPlayer(player);
        }

        private void CreatePlayer(Player player)
        {
            players[0] = player;
            var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof(GameObject))) as GameObject;
            var playerModel = prefabPlayer.AddComponent<ModelPlayer>();
            playerModel.transform.SetParent(containerPlayers);
            this.player = player;
            playerModel.Init(player, Vector2.zero, random);
            playerModel.Respawn += player1 => StartCoroutine(RespawnPlayer(player1));
            smoothCamera.target = prefabPlayer.transform;
        }

        IEnumerator RespawnPlayer(Player player)
        {
            yield return new WaitForSeconds(1);
            CreatePlayer(player);
        }

        private void CreateWeapon()
        {
            var prefabWeapons = Instantiate(Resources.Load(PrefabWeapon, typeof (GameObject))) as GameObject;
            var modelWeapon = prefabWeapons.AddComponent<WeaponModel>();
            modelWeapon.transform.SetParent(containerWeapons);
            modelWeapon.transform.position = new Vector2(random.Next(-BoundaryRandom, BoundaryRandom),
                random.Next(-BoundaryRandom, BoundaryRandom));
        }
    }
}
