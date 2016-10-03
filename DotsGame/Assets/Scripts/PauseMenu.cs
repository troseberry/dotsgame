using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	private Canvas pauseMenu;
	

	void Start () 
	{
		pauseMenu = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
		pauseMenu.enabled = false;
	}

	void OnLevelWasLoaded () 
	{
		pauseMenu = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
		pauseMenu.enabled = false;
	}

	void Update ()
	{
		//Android Soft Back Button Handling
		if (pauseMenu.enabled && Input.GetKey(KeyCode.Escape))
		{
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu ()
	{
		pauseMenu.enabled = !pauseMenu.enabled;
	}

	public void ResetLevel ()
	{
		//single means the previous (i.e. current) scene is completely unloaded before reloading
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void LoadMainMenu ()
	{
		CampaignData.SetLastScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene(0);
	}
}
