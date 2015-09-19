using UnityEngine;
using System.Collections;
using Caveman.Utils;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class Result : MonoBehaviour
    {
        public Transform names;
        public Transform kills;
        public Transform deaths;

        protected PlayerPool poolPlayers;

        public void SetPlayerPool(PlayerPool pool)
        {
            poolPlayers = pool;
        }

        protected  virtual IEnumerator DisplayResult()
        {
            var players = poolPlayers.GetCurrentPlayers();
            var lineIndex = 0;
            foreach (var playerModelBase in players)
            {
                Write(playerModelBase.player.name, names, lineIndex);
                Write(playerModelBase.player.deaths.ToString(), deaths, lineIndex);
                Write(playerModelBase.player.Kills.ToString(), kills, lineIndex);
                lineIndex++;
            }
            yield return new WaitForSeconds(1f);
        }

        public void Write(string value, Transform parent, int lineIndex)
        {
            var item = parent.GetChild(lineIndex);
            item.GetComponent<Text>().text = value;
            item.gameObject.SetActive(true);
        }
    }
}
