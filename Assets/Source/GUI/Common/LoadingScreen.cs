using System;
using System.Collections;
using Caveman.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Common
{
    public class LoadingScreen : Permanent<LoadingScreen>
    {
        public Slider slider;
        public Transform root;
        public Image blackscreen;
        public float TimeEmergenceBlackScreen = 0.2f;
        public float TimeHidingBlackScreen = 0.5f;

        public event EventHandler FinishLoading;

        public override void Awake()
        {
            if (!instance)
            {
                root.gameObject.SetActive(false);
                blackscreen.gameObject.SetActive(false);
            }
            else
            {
                DestroyImmediate(root.gameObject);
            }
            base.Awake();
        }

        public void ProgressTo(string name)
        {
            ProgressTo(Application.LoadLevelAsync(name));
        }

        public void ProgressTo(int level)
        {
            ProgressTo(Application.LoadLevelAsync(level));
        }

        private void ProgressTo(AsyncOperation loadLevelAsync)
        {
            StopAllCoroutines();
            StartCoroutine(WithProgress(loadLevelAsync));
        }

        private IEnumerator WithProgress(AsyncOperation load)
        {
            root.gameObject.SetActive(true);
            blackscreen.gameObject.SetActive(true);

            while (!load.isDone)
            {
                slider.value = load.progress;
                yield return new WaitForFixedUpdate();
            }

            // Hide loading screen
            var color = blackscreen.color;
            color.a = 0;
            blackscreen.color = color;
            var startTime = Time.time;
            while (blackscreen.color.a < 1)
            {
                var c = blackscreen.color;
                c.a = Mathf.Lerp(0, 1, (Time.time - startTime) / TimeEmergenceBlackScreen);
                blackscreen.color = c;
                yield return null;
            }
            root.gameObject.SetActive(false);
            
            // Show game screen
            startTime = Time.time;
            while (blackscreen.color.a > 0)
            {
                var c = blackscreen.color;
                c.a = Mathf.Lerp(1, 0, (Time.time - startTime) / TimeHidingBlackScreen);
                blackscreen.color = c;
                yield return null;
            }

            blackscreen.gameObject.SetActive(false);
            if (FinishLoading != null)
                FinishLoading(this, EventArgs.Empty);
        }
    }
}
