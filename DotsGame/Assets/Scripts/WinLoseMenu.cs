using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinLoseMenu : MonoBehaviour 
{

	private Canvas winLoseMenuCanvas;
	private Text winLoseText;
	private GameObject gameManagerObj;
	private GameManager gameManagerRef;
	private GameManagerTwoPlayer gameManagerTwoPlayerRef;
	private CampaignGameManager gameManagerCampaignRef;
	private string mode;
	
	void Start () 
	{
		if (SceneManager.GetActiveScene().buildIndex > 0)
		{
			winLoseMenuCanvas = GameObject.Find("WinLoseMenuCanvas").GetComponent<Canvas>();
			winLoseMenuCanvas.enabled = false;

			winLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();


			gameManagerObj = GameObject.Find("GameManager");
			if (gameManagerObj.GetComponent<CampaignGameManager>())
			{
				gameManagerCampaignRef = gameManagerObj.GetComponent<CampaignGameManager>();
				mode = "campaign";
			}
			if(gameManagerObj.GetComponent<GameManager>())
			{
				gameManagerRef =  gameManagerObj.GetComponent<GameManager>();
				mode = "classic";
			}
			else if (gameManagerObj.GetComponent<GameManagerTwoPlayer>())
			{
				gameManagerTwoPlayerRef = gameManagerObj.GetComponent<GameManagerTwoPlayer>();
				mode = "2player";
			}

		}
	}
	
	
	void Update () 
	{
		/*if (gameManagerObj && (gameManagerRef.RoundOver() || gameManagerTwoPlayerRef.RoundOver()))
		{
			ShowWinLoseMenu();
			ShowOutcomeText(mode);
		}*/

		if ((gameManagerRef && GameManager.RoundOver()) || (gameManagerTwoPlayerRef && GameManagerTwoPlayer.RoundOver()) || (gameManagerCampaignRef && CampaignGameManager.RoundOver()) )
		{
			ShowWinLoseMenu();
			ShowOutcomeText(mode);
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

	void ShowOutcomeText (string mode)
	{
		if (GameManager.PlayerWon() == "D" || GameManagerTwoPlayer.PlayerWon() == "D")
		{
			winLoseText.text = "Draw";
		}

		if (mode == "classic")
		{
			if (GameManager.PlayerWon() == "W")
			{
				winLoseText.text = "Game Won";
			}
			else if (GameManager.PlayerWon() == "L")
			{
				winLoseText.text = "Game Lost";
			}
		}
		else if (mode == "2player")
		{
			if (GameManagerTwoPlayer.PlayerWon() == "W1")
			{
				winLoseText.text = "Player One Wins";
			}
			else if (GameManagerTwoPlayer.PlayerWon() == "W2")
			{
				winLoseText.text = "Player Two Wins";
			}
		}
		else if (mode == "campaign")
		{
			if (CampaignGameManager.PlayerWon() == "W")
			{
				winLoseText.text = "Board Complete";
			}
			else if (CampaignGameManager.PlayerWon() == "L")
			{
				winLoseText.text = "Board Failed";
			}
		}
		
	}
}
