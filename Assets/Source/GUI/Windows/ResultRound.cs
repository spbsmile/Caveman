using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class ResultRound : MonoBehaviour
    {
        public Transform prefabText;

        private PlayerPool poolPlayers;

        public void OnEnable()
        {
            StartCoroutine(DisplayResult());
        }

        //Todo gui не должен ,наверно, пор пул игроков. ему должно передваться все , что надо . но не пул
        public void SetPlayerPool(PlayerPool pool)
        {
            poolPlayers = pool;
        }

        private IEnumerator DisplayResult()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0.00001f;

            var namePlayer = transform.GetChild(1);
            var kills = transform.GetChild(2);
            var deaths = transform.GetChild(3);
            var axisY = -20;
            const int deltaY = 30;
            for (var i = 0; i < Settings.BotsCount + 1; i++)
            {
                //Write(poolPlayers.GetName(i), namePlayer, axisY);
                //Write(poolPlayers.GetKills(i), kills, axisY);
                //Write(poolPlayers.GetDeaths(i), deaths, axisY);
                axisY -= deltaY;
            }
        }

        private void Write(string value, Transform parent, int axisY)
        {
            var goName = Instantiate(prefabText);
            goName.transform.SetParent(parent);
            goName.transform.localPosition = new Vector2(-2, axisY);
            goName.transform.rotation = Quaternion.identity;
            goName.GetComponent<Text>().text = value;
        }
    }
}
