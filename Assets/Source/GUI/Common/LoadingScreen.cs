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

        public override void Awake()
        {
            if (!instance)
            {
                root.gameObject.SetActive(false);
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

            while (!load.isDone)
            {
                slider.value = load.progress;
                yield return new WaitForFixedUpdate();
            }

            yield return null;

            root.gameObject.SetActive(false);
        }


        public void FadeTo(string name)
        {
            WithFade(Application.LoadLevelAsync(name));
        }

        public void FadeTo(int level)
        {
            WithFade(Application.LoadLevelAsync(level));
        }

        private void WithFade(AsyncOperation loadLevelAsync)
        {

        }
    }
}
