using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class WinLoseMenu : MonoBehaviour 
{

	private Canvas winLoseMenuCanvas;

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

	private bool showingOutcome;
	private bool didSave;

	private GameObject nextLevelButton;
	
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

			nextLevelButton =  transform.Find("NextLevelButton").gameObject;

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

		showingOutcome = false;
		didSave = false;
	}
	
	
	void Update () 
	{
		if ((gameManagerRef && GameManager.Instance.RoundOver()) || (gameManagerTwoPlayerRef && GameManagerTwoPlayer.Instance.RoundOver()) || (gameManagerCampaignRef && CampaignGameManager.Instance.RoundOver()) )
		{	
			if (!winLoseMenuCanvas.enabled) Invoke("ShowWinLoseMenu", 1.25f);
			if (!showingOutcome) StartCoroutine(ShowOutcomeText(mode, 1.25f));
		}
	}


	public void LoadMainMenu ()
	{
		CampaignData.SetLastScene(SceneManager.GetActiveScene().name);
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
		int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

		if (nextIndex != 22 && nextIndex != 35 && nextIndex != 48)
		{
			SceneManager.LoadScene(nextIndex);
		}
		else
		{
			LoadMainMenu();
		}
	}

	IEnumerator ShowOutcomeText (string mode, float waitDelay)
	{
		showingOutcome = true;
		yield return new WaitForSeconds(waitDelay);
		
		if (mode == "classic")
		{
			scoreNumber.text = GameManager.Instance.GetPlayerPoints() + " / " + GameManager.Instance.GetTotalPoints();

			if (GameManager.Instance.PlayerWon() == "W")
			{
				//Debug.Log("Game Won");
				gameWonImage.SetActive(true);

				gameLostImage.SetActive(false);
				drawImage.SetActive(false);	
			}
			else if (GameManager.Instance.PlayerWon() == "L")
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
			if (GameManagerTwoPlayer.Instance.PlayerWon() == "W1")
			{
				//Debug.Log("P1 Wins");
				playerOneWinsImage.SetActive(true);

				playerTwoWinsImage.SetActive(false);
				drawImage.SetActive(false);
			}
			else if (GameManagerTwoPlayer.Instance.PlayerWon() == "W2")
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

			p1ScoreNumber.text = GameManagerTwoPlayer.Instance.GetPlayerPoints("One") + " / " + GameManagerTwoPlayer.Instance.GetTotalPoints();
			p2ScoreNumber.text = GameManagerTwoPlayer.Instance.GetPlayerPoints("Two") + " / " + GameManagerTwoPlayer.Instance.GetTotalPoints();
		}
		else if (mode == "campaign")
		{
			string sceneName = SceneManager.GetActiveScene().name;
			string levelName = sceneName.Split('_')[1];
			


			if (CampaignGameManager.Instance.PlayerWon() == "L")
			{
				boardFailedImage.SetActive(true);
				boardCompleteImage.SetActive(false);
			}
			else
			{
				boardCompleteImage.SetActive(true);
				boardFailedImage.SetActive(false);
				Debug.Log("Won");

				int newStarRating = 0;
				Debug.Log(CampaignGameManager.Instance.PlayerWon());

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


				LevelStats currentStats = CampaignData.GetLevelStats(levelName);
				int starChange = 0;
				
				if (!currentStats.isComplete) currentStats.isComplete = true;
				if (currentStats.starRating < newStarRating) 
				{
					starChange = newStarRating - currentStats.starRating;
					currentStats.starRating = newStarRating;Debug.Log("New Star Rating: " + currentStats.starRating);
				}
				if (currentStats.bestScore < CampaignGameManager.Instance.GetPlayerPoints()) currentStats.bestScore = CampaignGameManager.Instance.GetPlayerPoints();

				CampaignData.SetLevelStats(levelName, currentStats);


				if(levelName[0] == '1')
				{
					CampaignData.UpdateBoardStars("BoardOne", starChange);
				}
				else if(levelName[0] == '2')
				{
					CampaignData.UpdateBoardStars("BoardTwo", starChange);
				}
				else if(levelName[0] == '3')
				{
					CampaignData.UpdateBoardStars("BoardThree", starChange);
				}


				if(!didSave)
				{
					SaveLoad.Save();
					didSave = true;
				}
			}
			scoreNumber.text = "" + CampaignGameManager.Instance.GetPlayerPoints();

			if (CampaignData.GetLevelStats(levelName).isComplete)
			{
				Color temp = nextLevelButton.GetComponent<Image>().color;
				temp.a = 1f;
				nextLevelButton.GetComponent<Image>().color = temp;

				nextLevelButton.GetComponent<Button>().enabled = true;
			}
			else 
			{
				Color temp = nextLevelButton.GetComponent<Image>().color;
				temp.a = 0.5f;
				nextLevelButton.GetComponent<Image>().color = temp;

				nextLevelButton.GetComponent<Button>().enabled = false;
			}
		}
	}
}