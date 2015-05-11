using UnityEngine;

public class Menu : MonoBehaviour 
{
	public void LoadSingleGame()
	{
		LoadLevel ();
	}

	public void LoadMultiGame()
	{
		LoadLevel();
	}

	public void LoadLevel() 
	{
		Application.LoadLevel (1);
	}
}
