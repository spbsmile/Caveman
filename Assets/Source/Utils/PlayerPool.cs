using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class PlayerPool : MonoBehaviour
    {
        private PlayerModelBase[] players;

        public void Store(PlayerModelBase player)
        {
            //todo при смерти скидывать ли камни ?
            player.InMotion = false;
            player.gameObject.SetActive(false);
        }

        public PlayerModelBase New(int id)
        {
            players[id].gameObject.SetActive(true);
            return players[id];
        }

        //TODO подумать над разумностью 
        public void Init(int count)
        {
            players = new PlayerModelBase[count];
        }

        public void Add(int id, PlayerModelBase model)
        {
            model.InMotion = true;
            players[id] = model;
        }

        // todo сомнительно
        public string GetName(int id)
        {
            return players[id].player.name;
        }

        public string GetKills(int id)
        {
            return players[id].player.Kills.ToString();
        }

        public string GetDeaths(int id)
        {
            return players[id].player.deaths.ToString();
        }

        public PlayerModelBase[] Players
        {
            get { return players; }
        }
    }
}
