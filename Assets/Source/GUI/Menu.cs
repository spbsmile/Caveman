using Caveman.GUI;
using UnityEngine;

public class Menu : MonoBehaviour 
{
    public void LoadSingleGame()
    {
        LoadingScreen.instance.ProgressTo(1);
    }

	public void LoadMultiplayGame()
	{
		Application.LoadLevel(4);
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
}
