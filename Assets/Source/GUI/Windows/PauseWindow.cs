using UnityEngine;
using System.Collections;

public class PauseWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
