using System;
using System.Collections;
using Caveman.Setting;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Battle
{
    public class MainGameTimer : MonoBehaviour
    {
        public Action RoundEnded;

        private Text value;

        public void Awake()
        {
            value = GetComponent<Text>();
        }

        public void OnEnable()
        {
            if (!Settings.multiplayerMode)
            {
                StartCoroutine(UpdateTime(Settings.RoundTime));
            }
        }

        public IEnumerator UpdateTime(int roundTime)
        {
            if (roundTime <= 0)
                yield break;

            do
            {
                var minutes = roundTime / 60;
                var seconds = roundTime % 60;
                value.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
                if (roundTime > 0)
                    yield return new WaitForSeconds(1);
            } while (--roundTime >= 0);

            if (RoundEnded != null && !Settings.multiplayerMode)
            {
                RoundEnded();
                StopAllCoroutines();
            }
        }
    }
}
