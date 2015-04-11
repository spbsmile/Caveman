using Caveman.Players;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        private const string PrefabPlayer = "skin_1";
        private const string PrefabWeapon = "weapon";

        public Transform ContainerWeapons;
        public Transform ContainerPlayers;
        public Text time;
        public Text deaths;
        public Text weapons;
        public Text killed;

        public int bots = 4 ;
        public int weaponsCount = 5;
        
        private string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik"};
        private Player player;
        private Random random;

        public void Start()
        {
            random = new Random();

            var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof(GameObject))) as GameObject;
            var playerModel = prefabPlayer.AddComponent<ModelPlayer>();
            playerModel.transform.SetParent(ContainerPlayers);
            player = new Player("Zabiyakin", 37);
            playerModel.Init(player, Vector2.zero);

            InitAIPlayers();
            InitWeapons();
        }

        public void Update()
        {
            time.text = "Time " + Time.time;
            weapons.text = "Weapons count : " + player.weapons;
            //deaths.text = player.death.ToString();
            killed.text = "Killed : " + player.killed;
        }

        private void InitAIPlayers()
        {
            const int boundaryRandom = 6;
            for (var i = 0; i < bots; i++)
            {
                var prefabPlayer = Instantiate(Resources.Load(PrefabPlayer, typeof(GameObject))) as GameObject;
                var modelAiPlayer = prefabPlayer.AddComponent<ModelAIPlayer>();
                modelAiPlayer.transform.SetParent(ContainerPlayers);
                modelAiPlayer.Init(new Player(names[i], i), new Vector2(random.Next(-boundaryRandom, boundaryRandom), random.Next(-boundaryRandom, boundaryRandom)));
                modelAiPlayer.SetWeapons(ContainerWeapons);
            }
        }

        private void InitWeapons()
        {
            const int boundaryRandom = 6;
            for (var i = 0; i < weaponsCount; i++)
            {
                var prefabWeapons = Instantiate(Resources.Load(PrefabWeapon, typeof (GameObject))) as GameObject;
                var modelWeapon = prefabWeapons.AddComponent<WeaponModel>();
                modelWeapon.transform.SetParent(ContainerWeapons);
                modelWeapon.transform.position = new Vector2(random.Next(-boundaryRandom, boundaryRandom), random.Next(-boundaryRandom, boundaryRandom));
            }
        }
    }
}
