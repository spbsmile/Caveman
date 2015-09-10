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

        public void Start()
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
            yield return  new WaitForSeconds(1);
            var remainTime = roundTime - 1;
            var m = Convert.ToInt32(remainTime / 60);
            var s = Convert.ToInt32(remainTime % 60);
            if (m < 60)
            {
                m = 0;
            }
            value.text = m + ":" + s;
            if (remainTime < 0 && RoundEnded != null && !Settings.multiplayerMode)
            {
                RoundEnded();
                StopAllCoroutines();
            }
            StartCoroutine(UpdateTime(remainTime));
        }
    }
}
