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
            if (roundTime <= 0) yield break;
            yield return  new WaitForSeconds(1);
            var remainTime = roundTime - 1;
            value.text = remainTime/60 + ":" +  remainTime%60;
            if (remainTime <= 0 && RoundEnded != null && !Settings.multiplayerMode)
            {
                RoundEnded();
                StopAllCoroutines();
            }
            StartCoroutine(UpdateTime(remainTime));
        }
    }
}
