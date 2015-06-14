using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class PlayerPool : MonoBehaviour
    {
        private BasePlayerModel[] players;

        public void Store(BasePlayerModel player)
        {
            //todo при смерти скидывать ли камни ?
            player.InMotion = false;
            player.gameObject.SetActive(false);
        }

        public BasePlayerModel New(int id)
        {
            players[id].gameObject.SetActive(true);
            return players[id];
        }

        //TODO подумать над разумностью 
        public void Init(int count)
        {
            players = new BasePlayerModel[count];
        }

        public void Add(int id, BasePlayerModel model)
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

        public BasePlayerModel[] Players
        {
            get { return players; }
        }
    }
}
