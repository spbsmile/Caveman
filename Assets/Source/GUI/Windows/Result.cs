using UnityEngine;
using System.Collections;
using Caveman.Pools;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class Result : MonoBehaviour
    {
        public Transform names;
        public Transform kills;
        public Transform deaths;

        protected  virtual IEnumerator DisplayResult()
        {
            var players = PlayerPool.instance.GetCurrentPlayers();
            var lineIndex = 0;
            foreach (var playerModelBase in players)
            {
                Write(playerModelBase.PlayerCore.Name, names, lineIndex);
                Write(playerModelBase.PlayerCore.Deaths.ToString(), deaths, lineIndex);
                Write(playerModelBase.PlayerCore.Kills.ToString(), kills, lineIndex);
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
