using System.Collections.Generic;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class PlayerPool : MonoBehaviour
    {
        private List<PlayerModelBase> players = new List<PlayerModelBase>();

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

        public List<PlayerModelBase> Players
        {
            get { return players; }
        }
    }
}
