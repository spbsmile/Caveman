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
        private CanvasGroup canvasGroup;
        public Button ButtonRespawn => buttonRespawn;

        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void OnDisable()
        {
            canvasGroup.alpha = 0;
            progress.value = 0;
            joystick.Enable();
        }

        public void Activate(int timeRespawn)
        {
            joystick.Disable();
            gameObject.SetActive(true);
            StartCoroutine(Delay());
            StartCoroutine(DisplayRespawnTimeOut(timeRespawn));
            StartCoroutine(DisplayResult());
        }

        private IEnumerator Delay()
        {
            const float intervalAlpha = 0.25f;
            canvasGroup.alpha += intervalAlpha;
            while(canvasGroup.alpha < 1){
                yield return new WaitForSeconds(0.1f);
                canvasGroup.alpha += intervalAlpha;
            }
        }

        protected override IEnumerator DisplayResult()
        {
            yield return StartCoroutine(base.DisplayResult());
            // for real-time update display data
            yield return StartCoroutine(DisplayResult());
        }

        private IEnumerator DisplayRespawnTimeOut(int timeRespawn)
        {
            var startTime = Time.time;
            while (progress.value < 0.99)
            {
                progress.value = (Time.time - startTime) / timeRespawn;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}


