using Caveman.Players;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        private const string PrefabName = "skin_1";

        public Text time;
        public Text deaths;
        public Text weapons;
        public int bots = 3;
        private string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik"};

        private Player humanPlayer;
        private Random random;

        public void Start()
        {
            random = new System.Random();

            var prefabPlayer = Instantiate(Resources.Load(PrefabName, typeof(GameObject))) as GameObject;
            var playerModel = prefabPlayer.AddComponent<ModelPlayer>();
            humanPlayer = new Player("Zabiyakin", 37);
            playerModel.Ini(humanPlayer, Vector2.zero);

            IniPlayers();
        }

        public void Update()
        {
            time.text = "Time " + Time.time.ToString();
            weapons.text = "Weapons count : " + humanPlayer.weapons.ToString();
            //deaths.text = humanPlayer.death.ToString();
        }

        private void IniPlayers()
        {
            for (var i = 0; i < bots; i++)
            {
                var prefabPlayer = Instantiate(Resources.Load(PrefabName, typeof(GameObject))) as GameObject;
                var playerModel = prefabPlayer.AddComponent<ModelAIPlayer>();
                playerModel.Ini(new Player(names[i], i),new Vector2(random.Next(0,6),random.Next(0,6)));
            }
        }
    }
}
