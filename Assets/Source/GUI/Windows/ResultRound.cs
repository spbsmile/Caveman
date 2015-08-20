using System.Collections;
using Caveman.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class ResultRound : MonoBehaviour
    {
        public Transform names;
        public Transform kills;
        public Transform deaths;

        private PlayerPool poolPlayers;

        public void OnEnable()
        {
            StartCoroutine(DisplayResult());
        }

        public void SetPlayerPool(PlayerPool pool)
        {
            poolPlayers = pool;
        }

        private IEnumerator DisplayResult()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0.00001f;
            var players = poolPlayers.GetCurrentPlayers();
            var index = 0;
            foreach (var playerModelBase in players)
            {
                Write(playerModelBase.player.name, names, index);
                Write(playerModelBase.player.deaths.ToString(), deaths, index);
                Write(playerModelBase.player.Kills.ToString(), kills, index);
                index++;
            }
        }

        private void Write(string value, Transform parent, int index)
        {
            var item = parent.GetChild(index);
            item.GetComponent<Text>().text = value;
            item.gameObject.SetActive(true);
        }
    }
}
