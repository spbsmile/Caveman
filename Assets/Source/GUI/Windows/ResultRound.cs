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
            if (!Setting.Settings.multiplayerMode)
            {
                StartCoroutine(DisplayResult());    
            }
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
            var lineIndex = 0;
            foreach (var playerModelBase in players)
            {
                Write(playerModelBase.player.name, names, lineIndex);
                Write(playerModelBase.player.deaths.ToString(), deaths, lineIndex);
                Write(playerModelBase.player.Kills.ToString(), kills, lineIndex);
                lineIndex++;
            }
        }

        public void Write(string value, Transform parent, int lineIndex)
        {
            var item = parent.GetChild(lineIndex);
            item.GetComponent<Text>().text = value;
            item.gameObject.SetActive(true);
        }
    }
}
