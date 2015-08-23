using System.Collections;
using Caveman.Setting;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class WaitForResp : MonoBehaviour
    {
        public Button buttonRespawn;
        public Slider progress;

        public void OnEnable()
        {
            var timeRespawn = Settings.TimeRespawnPlayer;
            StartCoroutine(WithProgress(timeRespawn));
        }

        public void OnDisable()
        {
            progress.value = 0;
        }

        private IEnumerator WithProgress(int timeRespawn)
        {
            var startTime = Time.time;
            while (progress.value < 0.99)
            {
                progress.value = (Time.time - startTime)/timeRespawn;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}


