using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class Menu : MonoBehaviour 
{
	public Text Name;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void LoadSingleGame()
	{
		if (Name != null) 
		{
			Debug.Log(Name.text);
			Settings.PLAYER_NAME = Name.text;
		}
		Debug.Log("welcome " + Settings.PLAYER_NAME.ToString());
		Settings.GAME_TYPE = 0;

		LoadLevel ();
	}

	public void LoadMultiGame()
	{
		if (Name != null) 
		{
			Debug.Log(Name.text);
			Settings.PLAYER_NAME = Name.text;
		}
		Debug.Log("welcome " + Settings.PLAYER_NAME.ToString());
		Settings.GAME_TYPE = 1;
		
		LoadLevel();
	}

	void LoadLevel() 
	{
		Application.LoadLevel (1);
	}
}
