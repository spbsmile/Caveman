using System.Collections;
using UnityEngine;

namespace Caveman.UI.Windows
{
    public class ResultRound : Result
    {
        public CNJoystick joystick;

        private bool isMultiplayer;

        public void Initialization(bool isMultiplayer)
        {
            this.isMultiplayer = isMultiplayer;
        }

        public void OnEnable()
        {
            if (!isMultiplayer)
            {
                StartCoroutine(DisplayResult());
            }
            else
            {
                joystick.Disable();
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
