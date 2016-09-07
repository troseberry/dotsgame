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

	public void TogglePauseMenu ()
	{
		pauseMenu.enabled = !pauseMenu.enabled;
	}

	public void ResetLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadMainMenu ()
	{
		SceneManager.LoadScene(0);
	}
}
