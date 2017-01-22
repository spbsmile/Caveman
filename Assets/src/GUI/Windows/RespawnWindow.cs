using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class RespawnWindow : Result
    {
        public Button buttonRespawn;
        public Slider progress;
        public CNJoystick joystick;

        //public void OnEnable()
        //{
        //    var timeRespawn = Settings.PlayerTimeRespawn;
        //    StartCoroutine(WithProgress(timeRespawn));
        //    StartCoroutine(DisplayResult());
        //}

        public void OnDisable()
        {
            progress.value = 0;
            joystick.Enable();
        }

        public void Activate(int timeRespawn)
        {
            joystick.Disable();
            gameObject.SetActive(true);
            StartCoroutine(WithProgress(timeRespawn));
            StartCoroutine(DisplayResult());
        }

        protected override IEnumerator DisplayResult()
        {
            yield return StartCoroutine(base.DisplayResult());
            // for real-time update display data
            yield return StartCoroutine(DisplayResult());
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


