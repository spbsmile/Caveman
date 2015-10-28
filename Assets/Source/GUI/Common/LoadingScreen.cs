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

            yield return StartCoroutine(FateTestImage());

            root.gameObject.SetActive(false);
        }

        private IEnumerator FateTestImage()
        {
            // todo color.a = Mathf.Lerp(0,128,timer); coroutine 
            // todo CrossFadeAlpha no work !(
            blackscreen.CrossFadeAlpha(1, 0.2f, false);
            yield return new WaitForSeconds(0.2f);
        }

        //public void Update()
        //{
        //    print(blackscreen.color.a + " alfa");
        //}
    }
}
