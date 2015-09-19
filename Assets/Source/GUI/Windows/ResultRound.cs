using System.Collections;
using UnityEngine;

namespace Caveman.UI.Windows
{
    public class ResultRound : Result
    {
        public void OnEnable()
        {
            if (!Setting.Settings.multiplayerMode)
            {
                StartCoroutine(DisplayResult());    
            }
        }

        protected override IEnumerator DisplayResult()
        {
            Time.timeScale = 0.00001f;
            yield return StartCoroutine(base.DisplayResult());
        }
    }
}
