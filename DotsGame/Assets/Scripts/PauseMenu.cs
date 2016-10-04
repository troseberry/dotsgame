using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	private Canvas pauseMenu;
	private float softBackDelay;
	

	void Start () 
	{
		pauseMenu = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
		pauseMenu.enabled = false;

		softBackDelay = 0f;
	}

	void Update ()
	{
		softBackDelay = (softBackDelay > 0) ? (softBackDelay - Time.deltaTime) : 0;

		//Android Soft Back Button Handling
		if (Input.GetKey(KeyCode.Escape))
		{
			if(pauseMenu.enabled)
			{
				TogglePauseMenu();
			}
			else
			{
				LoadMainMenu();
			}
		}
	}

	public void TogglePauseMenu ()
	{
		softBackDelay = 0.5f;
		pauseMenu.enabled = !pauseMenu.enabled;
	}

	public void ResetLevel ()
	{
		//single means the previous (i.e. current) scene is completely unloaded before reloading
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void LoadMainMenu ()
	{
		softBackDelay = 0.5f;
		CampaignData.SetLastScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene(0);
	}
}
