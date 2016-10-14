using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	private Canvas pauseMenu;
	private float softBackDelay;
	private string mode;

	void Start () 
	{
		pauseMenu = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
		pauseMenu.enabled = false;

		softBackDelay = 0f;

		mode = (SceneManager.GetActiveScene().name.Contains("HeroBoard")) ? "hero" : string.Empty;
	}

	void Update ()
	{
		softBackDelay = (softBackDelay > 0) ? (softBackDelay - Time.deltaTime) : 0;

		//Android Soft Back Button Handling
		if (Input.GetKey(KeyCode.Escape) && softBackDelay == 0)
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

		if (mode == "hero") HeroBoardManager.Instance.TogglePause();
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
