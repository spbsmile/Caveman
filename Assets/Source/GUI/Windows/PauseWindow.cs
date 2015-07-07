using UnityEngine;

namespace Caveman.UI.Windows
{
    public class PauseWindow : MonoBehaviour
    {
        public Transform window;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            window.gameObject.SetActive(false);
        }

        public void PauseGame()
        {
            Time.timeScale = 0.000001f;
            window.gameObject.SetActive(true);
            //TODO stop all animations
        }

        public void ExitGame()
        {
            Application.LoadLevel(0);
            Time.timeScale = 1;
        }
    }
}
