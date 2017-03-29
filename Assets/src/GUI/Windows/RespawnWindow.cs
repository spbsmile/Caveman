using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Windows
{
    public class RespawnWindow : Result
    {
        [SerializeField] private Button buttonRespawn;
        [SerializeField] private Slider progress;
        [SerializeField] private CNJoystick joystick;

        public Button ButtonRespawn
        {
            get { return buttonRespawn; }
        }

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


