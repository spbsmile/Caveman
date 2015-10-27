using System.Collections;
using UnityEngine;

namespace Caveman.UI.Windows
{
    public class ResultRound : Result
    {
        public CNJoystick joystick;

        public void OnEnable()
        {
            if (!Setting.Settings.multiplayerMode)
            {
                StartCoroutine(DisplayResult());
            }
        }

        protected override IEnumerator DisplayResult()
        {
            joystick.Disable();
            Time.timeScale = 0.00001f;
            yield return StartCoroutine(base.DisplayResult());
        }
    }
}
