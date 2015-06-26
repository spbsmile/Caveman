using UnityEngine;

public class PauseWindow : MonoBehaviour 
{
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
