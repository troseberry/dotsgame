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
	private Text p1ScoreNumber;
	private Text p2ScoreNumber;
	//private Text timeNumber;

	private bool didSave;
	
	void Start () 
	{

		//SaveLoad.Load();
		winLoseMenuCanvas = GameObject.FindGameObjectWithTag("WinLoseMenu").GetComponent<Canvas>();
		winLoseMenuCanvas.enabled = false;

		
		//Titles
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


		


		gameManagerObj = GameObject.Find("GameManager");
		if (gameManagerObj.GetComponent<CampaignGameManager>())
		{
			gameManagerCampaignRef = gameManagerObj.GetComponent<CampaignGameManager>();
			mode = "campaign";

			//2nd child is awarded star
			star01 = transform.Find("Star_01").gameObject;
			star01.transform.GetChild(2).gameObject.SetActive(false);

			star02 = transform.Find("Star_02").gameObject;
			star02.transform.GetChild(2).gameObject.SetActive(false);

			star03 = transform.Find("Star_03").gameObject;
			star03.transform.GetChild(2).gameObject.SetActive(false);

			scoreNumber = transform.Find("ScoreNumber").GetComponent<Text>();
			scoreNumber.text = "";

		}
		if(gameManagerObj.GetComponent<GameManager>())
		{
			gameManagerRef =  gameManagerObj.GetComponent<GameManager>();
			mode = "classic";

			scoreNumber = transform.Find("ScoreNumber").GetComponent<Text>();
			scoreNumber.text = "";
		}
		else if (gameManagerObj.GetComponent<GameManagerTwoPlayer>())
		{
			gameManagerTwoPlayerRef = gameManagerObj.GetComponent<GameManagerTwoPlayer>();
			mode = "2player";

			p1ScoreNumber = transform.Find("P1ScoreNumber").GetComponent<Text>();
			p1ScoreNumber.text = "";

			p2ScoreNumber = transform.Find("P2ScoreNumber").GetComponent<Text>();
			p2ScoreNumber.text = "";
		}



		


		



		Debug.Log(CampaignData.GetAllLevelsDictionary());
		didSave = false;
		/*foreach (KeyValuePair<string, bool> pair in CampaignData.GetAllLevelsDictionary())
		{
		    Debug.Log(pair.Key + pair.Value);
		}*/
	}
	
	
	void Update () 
	{
		/*if (gameManagerObj && (gameManagerRef.RoundOver() || gameManagerTwoPlayerRef.RoundOver()))
		{
			ShowWinLoseMenu();
			ShowOutcomeText(mode);
		}*/

		if ((gameManagerRef && GameManager.RoundOver()) || (gameManagerTwoPlayerRef && GameManagerTwoPlayer.RoundOver()) || (gameManagerCampaignRef && CampaignGameManager.Instance.RoundOver()) )
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

	public void LoadNextLevel ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}


	void ShowOutcomeText (string mode)
	{
		if (mode == "classic")
		{
			scoreNumber.text = GameManager.GetPlayerPoints() + " / " + GameManager.GetTotalPoints();

			if (GameManager.PlayerWon() == "W")
			{
				//Debug.Log("Game Won");
				gameWonImage.SetActive(true);

				gameLostImage.SetActive(false);
				drawImage.SetActive(false);	
			}
			else if (GameManager.PlayerWon() == "L")
			{
				//Debug.Log("Game Lost");
				gameLostImage.SetActive(true);

				gameWonImage.SetActive(false);
				drawImage.SetActive(false);
			}
			else
			{
				//Debug.Log("Draw");
				drawImage.SetActive(true);

				gameWonImage.SetActive(false);
				gameLostImage.SetActive(false);
			}
		}
		else if (mode == "2player")
		{
			if (GameManagerTwoPlayer.PlayerWon() == "W1")
			{
				//Debug.Log("P1 Wins");
				playerOneWinsImage.SetActive(true);

				playerTwoWinsImage.SetActive(false);
				drawImage.SetActive(false);
			}
			else if (GameManagerTwoPlayer.PlayerWon() == "W2")
			{
				//Debug.Log("P2 Wins");
				playerTwoWinsImage.SetActive(true);

				playerOneWinsImage.SetActive(false);
				drawImage.SetActive(false);
			}
			else
			{
				//Debug.Log("Draw");
				drawImage.SetActive(true);
				
				playerOneWinsImage.SetActive(false);
				playerTwoWinsImage.SetActive(false);
			}

			p1ScoreNumber.text = GameManagerTwoPlayer.GetPlayerPoints("One") + " / " + GameManagerTwoPlayer.GetTotalPoints();
			p2ScoreNumber.text = GameManagerTwoPlayer.GetPlayerPoints("Two") + " / " + GameManagerTwoPlayer.GetTotalPoints();
		}
		else if (mode == "campaign")
		{
			if (CampaignGameManager.Instance.PlayerWon() == "L")
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
				int newStarRating = 0;

				if (CampaignGameManager.Instance.PlayerWon() == "S01")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);
					
					newStarRating = 1;
				}
				else if (CampaignGameManager.Instance.PlayerWon() == "S02")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);	
					star02.transform.GetChild(2).gameObject.SetActive(true);
					
					newStarRating = 2;
				}
				else if (CampaignGameManager.Instance.PlayerWon() == "S03")
				{
					star01.transform.GetChild(2).gameObject.SetActive(true);
					star02.transform.GetChild(2).gameObject.SetActive(true);
					star03.transform.GetChild(2).gameObject.SetActive(true);	

					newStarRating = 3;
				}

				string sceneName = SceneManager.GetActiveScene().name;
				string levelName = sceneName.Split('_')[1];

				//Debug.Log("Scene Name: " + sceneName);
				//Debug.Log("Level Name: " + levelName);

				//if level has never been completed before, update all stats
				if (!CampaignData.GetLevelStatus(levelName))
				{
					CampaignData.UpdateLevelStatus(levelName, true, newStarRating, CampaignGameManager.Instance.GetPlayerPoints());
				}
				else
				{
					LevelStats currentLevelStats = CampaignData.GetFullLevelStatus(levelName);
					if (currentLevelStats.starRating < newStarRating)
					{
						CampaignData.UpdateLevelStatus(levelName, currentLevelStats.isComplete, newStarRating, currentLevelStats.bestScore);
					}

					if (currentLevelStats.bestScore < CampaignGameManager.Instance.GetPlayerPoints())
					{
						CampaignData.UpdateLevelStatus(levelName, currentLevelStats.isComplete, currentLevelStats.starRating, CampaignGameManager.Instance.GetPlayerPoints());
					}
				}

				if(!didSave)
				{
					SaveLoad.Save();
					didSave = true;
				}
			}
			scoreNumber.text = "" + CampaignGameManager.Instance.GetPlayerPoints();
		}
		
	}
}
