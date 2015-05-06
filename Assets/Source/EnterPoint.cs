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
        private const int BoundaryRandom = 7;

        public SmoothCamera smoothCamera;
        public Transform containerWeapons;
        public Transform containerPlayers;
        public Text time;
        public Text deaths;
        public Text weapons;
        public Text killed;

        public int bots = 4 ;
        public int weaponsCount = 5;

        private float timeRespawnWeapon = 100;
        private string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik"};
        private Player player;
        private Random random;

        public void Start()
        {
            random = new Random();

            CreatePlayer(new Player("Zabiyakin", 37));
            CreateAiPlayers();
            CreateWeapons();
        }

        public void Update()
        {
            time.text = "Round Time " + Time.time;
            weapons.text = player.weapons.ToString();
            killed.text = player.killed.ToString();
            if (timeRespawnWeapon-- < 0)
            {
                CreateWeapon();
                timeRespawnWeapon = 100;
            }
        }

        private void CreateAiPlayers()
        {
            for (var i = 0; i < bots; i++)
            {
                CreateAiPlayer(new Player(names[i], i));
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
