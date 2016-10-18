using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour 
{
	public GameObject titleGroup;

	public GameObject mainMenuButtons;

	public GameObject campaignMainMenu;
	public GameObject boardSelectMenu;

	public GameObject levelsGroup;
	private GameObject levelsCommonAssets;
	private GameObject boardOne;
	private GameObject boardTwo;
	private GameObject boardThree;

	public GameObject versusMainMenu;
	public GameObject versusClassicMenu;
	public GameObject versusBattleMenu;
	public GameObject versusTwoPlayerMenu;

	public GameObject developerOptionsMenu;

	private int devOptionTapCount;

	private GameObject currentBoardLevels;

	private float softBackDelay;
	

	void Start () 
	{
		levelsCommonAssets = levelsGroup.transform.Find("_CommonAssets").gameObject;
		boardOne = levelsGroup.transform.Find("BoardOne").gameObject;
		boardTwo = levelsGroup.transform.Find("BoardTwo").gameObject;
		boardThree = levelsGroup.transform.Find("BoardThree").gameObject;

		HideMenus();
		mainMenuButtons.SetActive(true);
		titleGroup.SetActive(true);

		SaveLoad.Load();

		if (CampaignData.GetLastScene() == null)
		{
			CampaignData.SetLastScene("");
		}
		else
		{
			if (CampaignData.GetLastScene().Contains("Campaign"))
			{
				ShowCampaignMenu();
			}
			else if (CampaignData.GetLastScene().Contains("Classic"))
			{
				ShowVersusClassicMenu();
			}
			else if (CampaignData.GetLastScene().Contains("Battle"))
			{
				ShowVersusBattleMenu();
			}
			else if (CampaignData.GetLastScene().Contains("2Player"))
			{
				ShowVersusTwoPlayerMenu();
			}
		}

		devOptionTapCount = 0;
		softBackDelay = 0f;
	}



	void Update ()
	{
		//Using delay so Back() won't be executed twice in rapid succession
		softBackDelay = (softBackDelay > 0) ? (softBackDelay - Time.deltaTime) : 0;

		//Android Soft Back Button Handling
		if (Input.GetKey(KeyCode.Escape) && softBackDelay == 0)
		{
			Back();
			softBackDelay = 0.5f;
		}
	}
	
	public void ShowDeveloperMenu ()
	{
		devOptionTapCount++;
		if (devOptionTapCount >= 5)
		{
			HideMenus();
			developerOptionsMenu.SetActive(true);
			devOptionTapCount = 0;
		}
	}

	public void DeleteSave ()
	{
		SaveLoad.Delete();

		CampaignData.ClearLevelsDictionary();
		CampaignData.ClearHeroesUnlockedDictionary();
	}

	public void SkipTutorial ()
	{
		CampaignData.SetFinishedTutorial(true);
		SaveLoad.Save();
		SaveLoad.Load();
	}


	void HideMenus ()
	{
		mainMenuButtons.SetActive(false);
		titleGroup.SetActive(false);

		campaignMainMenu.SetActive(false);
		boardSelectMenu.SetActive(false);

		boardOne.SetActive(false);

		versusMainMenu.SetActive(false);
		versusClassicMenu.SetActive(false);
		versusBattleMenu.SetActive(false);
		versusTwoPlayerMenu.SetActive(false);

		developerOptionsMenu.SetActive(false);
	}

	void HideBoards ()
	{
		levelsCommonAssets.SetActive(false);

		boardOne.SetActive(false);
		boardTwo.SetActive(false);
		boardThree.SetActive(false);
	}

	public void ShowCampaignMenu ()
	{
		if (!CampaignData.GetFinishedTutorial())
		{
			//if haven't done tutorial, load first tutorial scene
			SceneManager.LoadScene("Campaign3x3_Tutorial01");
		}
		else
		{
			titleGroup.SetActive(false);
			HideMenus();
			campaignMainMenu.SetActive(true);
			CampaignData.currentHero = HeroManager.Hero.None;

			if (CampaignData.GetLastScene() == "")
			{
				boardSelectMenu.SetActive(true);
			}
			else
			{
				if (CampaignData.GetLastScene() == "Campaign3x3_Tutorial03")
				{
					boardSelectMenu.SetActive(true);
				}
				else if (CampaignData.GetLastScene().Contains("Campaign3x3"))
				{
					ShowCampaignBoard("BoardOne");
				}
				else if (CampaignData.GetLastScene().Contains("Campaign4x4"))
				{
					ShowCampaignBoard("BoardTwo");
				}
				else if (CampaignData.GetLastScene().Contains("Campaign5x5"))
				{
					ShowCampaignBoard("BoardThree");
				}

				CampaignData.SetLastScene("");
			}
			
		}
	}

	public void ShowCampaignBoard ()
	{
		levelsCommonAssets.SetActive(true);


		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		string boardToShow = buttonName.Substring(0, buttonName.Length - 6);

		
		GameObject currentBoard = levelsGroup.transform.Find(boardToShow).gameObject;
		Debug.Log("Current Board: " + currentBoard);
		currentBoard.SetActive(true);

		

		currentBoardLevels = currentBoard.transform.Find("LevelSlider").transform.GetChild(0).transform.GetChild(0).gameObject;


		List<GameObject> levelButtons = new List<GameObject>();
		foreach (Transform child in currentBoardLevels.transform)
		{
			levelButtons.Add(child.gameObject);
		}

		for (int i = 0; i < levelButtons.Count; i++)
		{
			GameObject btn = levelButtons[i];
			//Handle Transparency
			string lvlNum = btn.name.Substring(6, btn.name.Length - 6);
			int prevLevel = (int.Parse(lvlNum.Substring(2, lvlNum.Length - 2))) - 1;
			string prevLevelName = lvlNum.Substring(0, 1) + "-" + prevLevel;
			//Debug.Log(prevLevel);

			int levelStarRating = CampaignData.GetLevelStats(lvlNum).starRating;
			//Debug.Log(lvlNum + " Stars: " + levelStarRating);

			//If level completed
			if (CampaignData.GetLevelStats(lvlNum).isComplete)
			{
				//Debug.Log("Level Button Stuff:" + lvlNum);
				btn.transform.Find("CheckMark").gameObject.SetActive(true);

				if(levelStarRating == 1)
				{
					//Debug.Log(lvlNum + " Star Rating: " + levelStarRating);
					btn.transform.Find("1Star").gameObject.SetActive(true);
				}
				else if(levelStarRating == 2)
				{
					//Debug.Log(lvlNum + " Star Rating: " + levelStarRating);
					btn.transform.Find("2Stars").gameObject.SetActive(true);
				}
				else if(levelStarRating == 3)
				{
					//Debug.Log(lvlNum + " Star Rating: " + levelStarRating);
					btn.transform.Find("3Stars").gameObject.SetActive(true);
				}
				else
				{
					//Debug.Log(lvlNum + " Star Rating: " + levelStarRating);
					btn.transform.Find("1Star").gameObject.SetActive(false);
					btn.transform.Find("2Stars").gameObject.SetActive(false);
					btn.transform.Find("3Stars").gameObject.SetActive(false);
				}

			}

			if (prevLevel != 0 && !CampaignData.GetLevelStats(prevLevelName).isComplete)
			{
				btn.GetComponent<Button>().enabled = false;
				Color temp = btn.GetComponent<Image>().color;
				temp.a = 0.5f;
				btn.GetComponent<Image>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 0.5f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
			else if (prevLevel != 0 && CampaignData.GetLevelStats(prevLevelName).isComplete)
			{
				btn.GetComponent<Button>().enabled = true;
				Color temp = btn.GetComponent<Image>().color;
				temp.a = 1f;
				btn.GetComponent<Image>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 1f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
		}
		//If completed all previous level, unlocked hero board (later add check for # of stars required)
		string lastLevelName = levelButtons[levelButtons.Count - 1].name.Split('_')[1];
		if (CampaignData.GetLevelStats(lastLevelName).isComplete && CampaignData.EnoughBoardStars(currentBoard.name))
		{
			Debug.Log("Star Count: " + CampaignData.GetBoardStars(currentBoard.name));
			currentBoard.transform.Find("Locked").gameObject.SetActive(false);
		}
		Debug.Log("Star Count: " + CampaignData.GetBoardStars(currentBoard.name));
		boardSelectMenu.SetActive(false);

		//currentBoard.transform.Find("HeaderTitles").GetComponent<Animation>().Play("SwitchHeaders");
	}

	public void ShowCampaignBoard (string boardToShow)
	{
		levelsCommonAssets.SetActive(true);

		GameObject currentBoard = levelsGroup.transform.Find(boardToShow).gameObject;
		currentBoard.SetActive(true);

		currentBoardLevels = currentBoard.transform.Find("LevelSlider").transform.GetChild(0).transform.GetChild(0).gameObject;


		List<GameObject> levelButtons = new List<GameObject>();
		foreach (Transform child in currentBoardLevels.transform)
		{
			levelButtons.Add(child.gameObject);
		}

		for (int i = 0; i < levelButtons.Count; i++)
		{
			GameObject btn = levelButtons[i];

			//Handle Transparency
			string lvlNum = btn.name.Substring(6, btn.name.Length - 6);
			int prevLevel = (int.Parse(lvlNum.Substring(2, lvlNum.Length - 2))) - 1;
			string prevLevelName = lvlNum.Substring(0, 1) + "-" + prevLevel;

			int levelStarRating = CampaignData.GetLevelStats(lvlNum).starRating;
			//Debug.Log(lvlNum + " Stars: " + levelStarRating);


			//If level completed
			if (CampaignData.GetLevelStats(lvlNum).isComplete)
			{
				btn.transform.Find("CheckMark").gameObject.SetActive(true);

				if(levelStarRating == 1)
				{
					btn.transform.Find("1Star").gameObject.SetActive(true);
				}
				else if(levelStarRating == 2)
				{
					btn.transform.Find("2Stars").gameObject.SetActive(true);
				}
				else if(levelStarRating == 3)
				{
					btn.transform.Find("3Stars").gameObject.SetActive(true);
				}
				else
				{
					btn.transform.Find("1Star").gameObject.SetActive(false);
					btn.transform.Find("2Stars").gameObject.SetActive(false);
					btn.transform.Find("3Stars").gameObject.SetActive(false);
				}

			}

			if (prevLevel != 0 && !CampaignData.GetLevelStats(prevLevelName).isComplete)
			{
				btn.GetComponent<Button>().enabled = false;
				Color temp = btn.GetComponent<Image>().color;
				temp.a = 0.5f;
				btn.GetComponent<Image>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 0.5f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
			else if (prevLevel != 0 && CampaignData.GetLevelStats(prevLevelName).isComplete)
			{
				btn.GetComponent<Button>().enabled = true;
				Color temp = btn.GetComponent<Image>().color;
				temp.a = 1f;
				btn.GetComponent<Image>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 1f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
		}

		//If completed all previous level, unlocked hero board (later add check for # of stars required)
		string lastLevelName = levelButtons[levelButtons.Count - 1].name.Split('_')[1];
		if (CampaignData.GetLevelStats(lastLevelName).isComplete && CampaignData.EnoughBoardStars(currentBoard.name))
		{
			Debug.Log("Star Count: " + CampaignData.GetBoardStars(currentBoard.name));
			currentBoard.transform.Find("Locked").gameObject.SetActive(false);
		}
		Debug.Log("Star Count: " + CampaignData.GetBoardStars(currentBoard.name));
		boardSelectMenu.SetActive(false);
	}


	public void ShowVersusMenu ()
	{
		HideMenus();
		versusMainMenu.SetActive(true);
	}

	public void ShowVersusClassicMenu ()
	{
		HideMenus();
		versusClassicMenu.SetActive(true);
	}

	public void ShowVersusBattleMenu ()
	{
		HideMenus();
		versusBattleMenu.SetActive(true);
	}

	public void ShowVersusTwoPlayerMenu ()
	{
		HideMenus();
		versusTwoPlayerMenu.SetActive(true);
	}



	public void LoadCampaignBoard ()
	{
		SaveLoad.Save();
		
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		string levelToLoad = buttonName.Substring(6, buttonName.Length - 6);

		if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/BoardOne/Campaign3x3_" + levelToLoad))
		{
			Debug.Log("Found 3x3 scene with that name");
			SceneManager.LoadScene("Levels/Campaign/BoardOne/Campaign3x3_" + levelToLoad);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/BoardTwo/Campaign4x4_" + levelToLoad))
		{
			Debug.Log("Found 4x4 scene with that name");
			SceneManager.LoadScene("Levels/Campaign/BoardTwo/Campaign4x4_" + levelToLoad);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/BoardThree/Campaign5x5_" + levelToLoad))
		{
			Debug.Log("Found 5x5 scene with that name");
			SceneManager.LoadScene("Levels/Campaign/BoardThree/Campaign5x5_" + levelToLoad);
		}
	}

	public void LoadHeroBoard ()
	{
		SaveLoad.Save();

		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		string heroString = buttonName.Substring(0, buttonName.Length - 6);
		Debug.Log("Hero String: " + heroString);

		if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/HeroBoards/Campaign3x3HeroBoard_" + heroString))
		{
			Debug.Log("Found 3x3 hero board with that name");
			SceneManager.LoadScene("Levels/Campaign/HeroBoards/Campaign3x3HeroBoard_" + heroString);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/HeroBoards/Campaign4x4HeroBoard_" + heroString))
		{
			Debug.Log("Found 4x4 hero board with that name");
			SceneManager.LoadScene("Levels/Campaign/HeroBoards/Campaign4x4HeroBoard_" + heroString);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/HeroBoards/Campaign5x5HeroBoard_" + heroString))
		{
			Debug.Log("Found 5x5 hero board with that name");
			SceneManager.LoadScene("Levels/Campaign/HeroBoards/Campaign5x5HeroBoard_" + heroString);
		}
	}

	public void LoadClassicVersus ()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;

		if (buttonName == "3x3 Button")
		{
			SceneManager.LoadScene("Classic_3x3");
		}
		else if (buttonName == "4x4 Button")
		{
			SceneManager.LoadScene("Classic_4x4");
		}
		else if (buttonName == "5x5 Button")
		{
			SceneManager.LoadScene("Classic_5x5");
		}
	}

	public void LoadBattleVersus ()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;

		if (buttonName == "3x3 Button")
		{
			SceneManager.LoadScene("Battle_3x3");
		}
		else if (buttonName == "4x4 Button")
		{
			SceneManager.LoadScene("Battle_4x4");
		}
		else if (buttonName == "5x5 Button")
		{
			SceneManager.LoadScene("Battle_5x5");
		}
	}

	public void LoadTwoPlayerVersus ()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;

		if (buttonName == "3x3 Button")
		{
			SceneManager.LoadScene("2Player_3x3");
		}
		else if (buttonName == "4x4 Button")
		{
			SceneManager.LoadScene("2Player_4x4");
		}
		else if (buttonName == "5x5 Button")
		{
			SceneManager.LoadScene("2Player_5x5");
		}
	}




	public void Back ()
	{

		if (mainMenuButtons.activeSelf)
		{
			Application.Quit();
		}
		else if (campaignMainMenu.activeSelf)
		{
			if (boardSelectMenu.activeSelf)
			{
				HideMenus();
				titleGroup.SetActive(true);
				mainMenuButtons.SetActive(true);
				devOptionTapCount = 0;
			}
			else
			{
				HideBoards();
				currentBoardLevels = null;
				boardSelectMenu.SetActive(true);
			}
		}
		else if (versusMainMenu.activeSelf || developerOptionsMenu.activeSelf)
		{
			HideMenus();
			mainMenuButtons.SetActive(true);
			devOptionTapCount = 0;
		}
		else if (versusClassicMenu.activeSelf || versusBattleMenu.activeSelf || versusTwoPlayerMenu.activeSelf)
		{
			HideMenus();
			versusMainMenu.SetActive(true);
		}
	}
}
