using Caveman.Players;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        private const string PrefabName = "skin_1";

        public Transform ContainerWeapons;
        public Text time;
        public Text deaths;
        public Text weapons;
        public int bots = 1 ;
        private string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik"};

        private Player player;
        private Random random;

        public void Start()
        {
            random = new System.Random();

            var prefabPlayer = Instantiate(Resources.Load(PrefabName, typeof(GameObject))) as GameObject;
            var playerModel = prefabPlayer.AddComponent<ModelPlayer>();
            player = new Player("Zabiyakin", 37);
            playerModel.Ini(player, Vector2.zero);

            IniAIPlayers();
        }

        public void Update()
        {
            time.text = "Time " + Time.time.ToString();
            weapons.text = "Weapons count : " + player.weapons.ToString();
            //deaths.text = player.death.ToString();
        }

        private void IniAIPlayers()
        {
            for (var i = 0; i < bots; i++)
            {
                var prefabPlayer = Instantiate(Resources.Load(PrefabName, typeof(GameObject))) as GameObject;
                var modelAiPlayer = prefabPlayer.AddComponent<ModelAIPlayer>();
                modelAiPlayer.Ini(new Player(names[i], i),new Vector2(random.Next(0,6),random.Next(0,6)));
                modelAiPlayer.SetWeapons(ContainerWeapons);
            }
        }
    }
}
