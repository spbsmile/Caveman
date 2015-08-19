using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using System.Collections;
using System.Linq;
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
            //for (var i = 0; i < players.Count(); i++)
            //{
            //    players[i].
            //}
          
        }

        //private void Write(string value, Transform parent)
        //{
        //    goName.transform.SetParent(parent);
        //    goName.transform.localPosition = new Vector2(-2, axisY);
        //    goName.transform.rotation = Quaternion.identity;
        //    goName.GetComponent<Text>().text = value;
        //}
    }
}
