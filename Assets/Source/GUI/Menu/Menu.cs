using System.Collections;
using Caveman.UI.Common;
using Caveman.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Caveman.UI.Menu
{
    public class Menu : MonoBehaviour
    {
        public CanvasGroup tooltipNickname;
        public InputField inputNickname;

        public void Start()
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }

        public void LoadSingleGame()
        {
            if (string.IsNullOrEmpty(inputNickname.text))
            {
                ShowNoNameAlert();
            } 
            else
            {
                LoadingScreen.instance.ProgressTo(1);
            }
        }

        public void LoadMultiplayGame()
        {
            if (string.IsNullOrEmpty(inputNickname.text))
            {
                StartCoroutine(FadeOutTooltip());
            }
            else
            {
                LoadingScreen.instance.ProgressTo(4);
            }
        }

        public void LoadObserverGame()
        {
            //LoadingScreen.instance.ProgressTo(4);
        }

        public void LoadMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Settings()
        {
            SceneManager.LoadScene(2);
        }

        public void LoadProfile()
        {
            SceneManager.LoadScene(6);
        }

        private void ShowNoNameAlert()
        {
            StartCoroutine(FadeOutTooltip());
        }

        private IEnumerator FadeOutTooltip()
        {
            tooltipNickname.alpha = 1;
            yield return new WaitForSeconds(2f);
            StartCoroutine(tooltipNickname.FadeOut());
        }
    }
}
