using System.Collections;
using System.Collections.Generic;
using Caveman.UI.Common;
using Caveman.Utils;
using UnityEngine;
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

        public void LoadMenu()
        {
            Time.timeScale = 1;
            Application.LoadLevel(0);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Settings()
        {
            Application.LoadLevel(2);
        }

        public void LoadProfile()
        {
            Application.LoadLevel(6);
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
