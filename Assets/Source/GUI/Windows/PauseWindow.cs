using UnityEngine;

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
        //TODO stop scale time
        //TODO stop all animations
        //TODO show pause window
    }
}
