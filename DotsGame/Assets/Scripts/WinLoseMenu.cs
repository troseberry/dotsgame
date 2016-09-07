using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinLoseMenu : MonoBehaviour 
{

	private Canvas winLoseMenuCanvas;
	private Text winLoseText;
	private GameObject gameManagerObj;
	
	void Start () 
	{
		if (SceneManager.GetActiveScene().buildIndex > 0)
		{
			winLoseMenuCanvas = GameObject.Find("WinLoseMenuCanvas").GetComponent<Canvas>();
			winLoseMenuCanvas.enabled = false;

			winLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();

			gameManagerObj = GameObject.Find("GameManager");
		}
	}
	
	
	void Update () 
	{
		if (gameManagerObj && GameManager.RoundOver())
		{
			ShowWinLoseMenu();
			ShowOutcomeText();
		}
	}

	public void LoadMainMenu ()
	{
		SceneManager.LoadScene(0);
	}

	void ShowWinLoseMenu ()
	{
		winLoseMenuCanvas.enabled = true;
	}
	

	public void ResetLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	void ShowOutcomeText ()
	{
		if (GameManager.PlayerWon() == "W")
		{
			winLoseText.text = "Game Won";
		}
		else if (GameManager.PlayerWon() == "L")
		{
			winLoseText.text = "Game Lost";
		}
		else if (GameManager.PlayerWon() == "D")
		{
			winLoseText.text = "Draw";
		}
	}
}
