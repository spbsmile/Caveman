using UnityEngine;
using UnityEngine.SceneManagement;

namespace Caveman.UI.Windows
{
    public class PauseWindow : MonoBehaviour
    {
        public Transform window;
        public CNJoystick joystick;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            window.gameObject.SetActive(false);
            joystick.Enable();
        }

        public void PauseGame()
        {
            Time.timeScale = 0.000001f;
            window.gameObject.SetActive(true);
            joystick.Disable();
            //TODO stop all animations
        }

        public void ExitGame()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
    }
}
