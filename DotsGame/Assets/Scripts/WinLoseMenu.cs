using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class WinLoseMenu : MonoBehaviour 
{

	private Canvas winLoseMenuCanvas;
	//private Text winLoseText;

	public GameObject titleGroup;
	private GameObject boardCompleteImage;
	private GameObject boardFailedImage;
	private GameObject gameWonImage;
	private GameObject gameLostImage;
	private GameObject drawImage;
	private GameObject playerOneWinsImage;
	private GameObject playerTwoWinsImage;

	private GameObject gameManagerObj;
	private GameManager gameManagerRef;
	private GameManagerTwoPlayer gameManagerTwoPlayerRef;
	private CampaignGameManager gameManagerCampaignRef;

	private string mode;

	private GameObject star01;
	private GameObject star02;
	private GameObject star03;

	private Text scoreNumber;

	private bool didSave;
	
	void Start () 
	{
		if (SceneManager.GetActiveScene().buildIndex > 0)
		{
			//SaveLoad.Load();
			winLoseMenuCanvas = GameObject.Find("WinLoseMenuCanvas").GetComponent<Canvas>();
			winLoseMenuCanvas.enabled = false;

			//winLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();
			boardCompleteImage = titleGroup.transform.Find("BoardComplete").gameObject;
			boardCompleteImage.SetActive(false);

			boardFailedImage = titleGroup.transform.Find("BoardFailed").gameObject;
			boardFailedImage.SetActive(false);

			gameWonImage = titleGroup.transform.Find("GameWon").gameObject;
			gameWonImage.SetActive(false);

			gameLostImage = titleGroup.transform.Find("GameLost").gameObject;
			gameLostImage.SetActive(false);

			drawImage = titleGroup.transform.Find("Draw").gameObject;
			drawImage.SetActive(false);

			playerOneWinsImage = titleGroup.transform.Find("PlayerOneWins").gameObject;
			playerOneWinsImage.SetActive(false);

			playerTwoWinsImage = titleGroup.transform.Find("PlayerTwoWins").gameObject;
			playerTwoWinsImage.SetActive(false);


			scoreNumber =  transform.Find("ScoreNumber").GetComponent<Text>();
			scoreNumber.text = "";


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




			//2nd child is awarded star
			star01 = transform.Find("Star_01").gameObject;
			star01.transform.GetChild(2).gameObject.SetActive(false);

			star02 = transform.Find("Star_02").gameObject;
			star02.transform.GetChild(2).gameObject.SetActive(false);

			star03 = transform.Find("Star_03").gameObject;
			star03.transform.GetChild(2).gameObject.SetActive(false);




			Debug.Log(CampaignData.GetAllLevelsDictionary());
			didSave = false;
			/*foreach (KeyValuePair<string, bool> pair in CampaignData.GetAllLevelsDictionary())
			{
			    Debug.Log(pair.Key + pair.Value);
			}*/
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
			Invoke("ShowWinLoseMenu", 1.25f);
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
		if (mode != "campaign" && (GameManager.PlayerWon() == "D" || GameManagerTwoPlayer.PlayerWon() == "D"))
		{
			drawImage.SetActive(true);
			//winLoseText.text = "Draw";
			Debug.Log("Draw");
		}

		else if (mode == "classic")
		{
			if (GameManager.PlayerWon() == "W")
			{
				gameWonImage.SetActive(true);
				Debug.Log("Game Won");
				//winLoseText.text = "Game Won";
			}
			else if (GameManager.PlayerWon() == "L")
			{
				gameLostImage.SetActive(true);
				Debug.Log("Game Lost");
				//winLoseText.text = "Game Lost";
			}
		}
		else if (mode == "2player")
		{
			if (GameManagerTwoPlayer.PlayerWon() == "W1")
			{
				playerOneWinsImage.SetActive(true);
				//winLoseText.text = "Player One Wins";
			}
			else if (GameManagerTwoPlayer.PlayerWon() == "W2")
			{
				playerTwoWinsImage.SetActive(true);
				//winLoseText.text = "Player Two Wins";
			}
		}
		else if (mode == "campaign")
		{
			if (CampaignGameManager.PlayerWon() == "L")
			{
				//winLoseText.text = "Board Failed";
				boardFailedImage.SetActive(true);
				//Debug.Log("Lost");
			}
			else
			{
				//winLoseText.text = "Board Complete";
				boardCompleteImage.SetActive(true);
				//Debug.Log("Won");

				if (CampaignGameManager.PlayerWon() == "S01")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);
				}
				else if (CampaignGameManager.PlayerWon() == "S02")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);	
					star02.transform.GetChild(2).gameObject.SetActive(true);
				}
				else if (CampaignGameManager.PlayerWon() == "S03")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);
					star02.transform.GetChild(2).gameObject.SetActive(true);
					star03.transform.GetChild(2).gameObject.SetActive(true);	
				}

				//if playing from main menu, substring should start at 37. If directly from level, 12
				string sceneName = SceneManager.GetActiveScene().name;
				//Debug.Log("Current Scene Name: "+ SceneManager.GetActiveScene().name);
				//Debug.Log("Completed Level #: " + sceneName.Substring(37, sceneName.Length - 37));
				CampaignData.SetLevelStatus(sceneName.Substring(37, sceneName.Length - 37), true);
				
				if(!didSave)
				{
					SaveLoad.Save();
					didSave = true;
				}
			}
			scoreNumber.text = "" + CampaignGameManager.GetPlayerPoints();
		}
		
	}
}
