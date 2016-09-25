﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour 
{
	public GameObject mainMenuButtons;

	public GameObject campaignMainMenu;
	public GameObject boardSelectMenu;

	private GameObject levelsGroup;
	private GameObject boardOne;
	private GameObject boardTwo;
	private GameObject boardThree;

	public GameObject versusMainMenu;
	public GameObject versusClassicMenu;
	public GameObject versusBattleMenu;
	public GameObject versusTwoPlayerMenu;

	public GameObject developerOptionsMenu;

	//private bool deleteSave;
	private int devOptionTapCount;



	//Vars for Detecting Swipe
	private float startTime;
	private Vector2 startPos;
	private bool couldBeSwipe;
	private float comfortZone;
	private float minSwipeDist;
	private float maxSwipeTime;

	private GameObject boardToSlide;
	private int totalSlidePositions;
	private int currentSlidePosition;
	private bool canSlideBoard;
	

	void Start () 
	{
		levelsGroup = GameObject.Find("Levels");
		boardOne = levelsGroup.transform.Find("BoardOne").gameObject;
		boardTwo = levelsGroup.transform.Find("BoardTwo").gameObject;
		boardThree = levelsGroup.transform.Find("BoardThree").gameObject;


		HideMenus();
		mainMenuButtons.SetActive(true);

		SaveLoad.Load();
		Debug.Log(Application.persistentDataPath);

		if (CampaignData.GetLastScene() == "Campaign3x3_Tutorial03")
		{
			//HideMenus();
			ShowCampaignMenu();
		}

		devOptionTapCount = 0;

		comfortZone = 5.0f;
		minSwipeDist = 90.0f;
		maxSwipeTime = 2.0f;
	}



	void Update ()
	{
		//Detect Swipe

		//Only detect swipe if a board menu is active
		if (campaignMainMenu.activeSelf && !boardSelectMenu.activeSelf)
		{
			//Debug.Log("Can Swipe");

			if (Input.touchCount > 0) {
		        Touch touch = Input.touches[0];
		       
		        switch (touch.phase) {
		            case TouchPhase.Began:
		            	//Debug.Log("Began Touch");
		                couldBeSwipe = true;
		                startPos = touch.position;
		                startTime = Time.time;
		                break;
		           
		            case TouchPhase.Moved:
		            	//Debug.Log("Change From Horizontal: " + Mathf.Abs(touch.position.x - startPos.x));

		                if (Mathf.Abs(touch.position.x - startPos.x) > comfortZone) {
		                    couldBeSwipe = false;
		                }
		                break;
		           
		            case TouchPhase.Stationary:
		                couldBeSwipe = false;
		                break;
		           
		            case TouchPhase.Ended:

		                float swipeTime = Time.time - startTime;
		                float swipeDist = (touch.position - startPos).magnitude;

		               // Debug.Log("End Swipe Time: " + swipeTime);
		                //Debug.Log("End Swipe Distance: " + swipeDist);
		               
		                if (couldBeSwipe || (swipeTime < maxSwipeTime) || (swipeDist > minSwipeDist)) {
		                    // It's a swiiiiiiiiiiiipe!
		                    float swipeDirection = Mathf.Sign(touch.position.x - startPos.x);
		                   
		                    // Do something here in reaction to the swipe.
		                    MoveLevelSlider(swipeDirection);
		                }

		                //after first swipe, set canSlideBoard true
		                //doing this because a swipe is detected when pressing board select button. maybe change comfortZone val
		            	if(!canSlideBoard) canSlideBoard = true;

		                break;
		        }
		    }
		}
	}
	
	public void ShowDeveloperMenu ()
	{
		devOptionTapCount++;
		if (devOptionTapCount < 5)
		{
			
			//Debug.Log("Tap Count: " + devOptionTapCount);
		}
		else
		{
			HideMenus();
			developerOptionsMenu.SetActive(true);
			devOptionTapCount = 0;
		}
	}

	public void DeleteSave ()
	{
		Debug.Log("Deleting Save...");
		SaveLoad.Delete();

		Debug.Log("Finished Tutorial? " + CampaignData.GetFinishedTutorial());

		CampaignData.ClearLevelsDictionary();

		/*Debug.Log(CampaignData.GetBoardOneDictionary());
		foreach (KeyValuePair<string, bool> pair in CampaignData.GetBoardOneDictionary())
		{
		    Debug.Log(pair.Key + pair.Value);
		}*/
		//SaveLoad.Load();
		//SceneManager.LoadScene(0);
		
	}

	public void SkipTutorial ()
	{
		Debug.Log("Skipping Tutorial");
		CampaignData.SetFinishedTutorial(true);
		SaveLoad.Save();
		SaveLoad.Load();
	}


	void HideMenus ()
	{
		mainMenuButtons.SetActive(false);

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
			HideMenus();
			campaignMainMenu.SetActive(true);
			boardSelectMenu.SetActive(true);

			
		}
	}

	public void ShowCampaignBoard ()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		string boardToShow = buttonName.Substring(0, buttonName.Length - 6);

		//boardToShow = char.ToLower(boardToShow[0]) + boardToShow.Substring(1);

		//Debug.Log(boardToShow);

		GameObject currentBoard = levelsGroup.transform.Find(boardToShow).gameObject;
		Debug.Log("Current Board: " + currentBoard);
		currentBoard.SetActive(true);

		boardToSlide = currentBoard.transform.Find("LevelSlider").gameObject;
		totalSlidePositions = boardToSlide.transform.childCount - 2;		//-2 so first and last never move into borders
		currentSlidePosition = 0;

		List<GameObject> levelButtons = new List<GameObject>();
		foreach (Transform child in boardToSlide.transform)
		{
			levelButtons.Add(child.gameObject);
		}

		//GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");
		foreach (GameObject btn in levelButtons)
		{
			string lvlNum = btn.name.Substring(6, btn.name.Length - 6);
			int prevLevel = (int.Parse(lvlNum.Substring(2, lvlNum.Length - 2))) - 1;
			string prevLevelName = lvlNum[0] + "-" + prevLevel.ToString(); 
			//Debug.Log(prevLevelName);

			if (CampaignData.GetLevelStatus(lvlNum))
			{
				//Debug.Log("Level Button Stuff:" + lvlNum);
				btn.transform.Find("CheckMark").gameObject.SetActive(true);
			}

			if (prevLevel != 0 && !CampaignData.GetLevelStatus(prevLevelName))/*!CampaignData.GetLevelStatus("1-" + prevLevel))*/
			{
				btn.GetComponent<Button>().enabled = false;
				Color temp = btn.GetComponent<RawImage>().color;
				temp.a = 0.5f;
				btn.GetComponent<RawImage>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 0.5f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
			else if (prevLevel != 0 && !CampaignData.GetLevelStatus(prevLevelName))/*!CampaignData.GetLevelStatus("1-" + prevLevel))*/
			{
				btn.GetComponent<Button>().enabled = true;
				Color temp = btn.GetComponent<RawImage>().color;
				temp.a = 1f;
				btn.GetComponent<RawImage>().color = temp;

				Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
				textTemp.a = 1f;
				btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
			}
		}


		boardSelectMenu.SetActive(false);
	}

	void MoveLevelSlider (float slideDirection)
	{
		float boardXPosition = boardToSlide.transform.localPosition.x;
		if( (boardXPosition == -375 && slideDirection > 0) || ( (boardXPosition == (-375 * totalSlidePositions)) && slideDirection < 0))
		{
			canSlideBoard = false;
		}

		if (boardToSlide && canSlideBoard)
		{
			
			//Debug.Log("Board X Pos: " + boardXPosition);
			//if((currentSlidePosition >= 0 && slideDirection < 0) || currentSlidePosition <= totalSlidePositions && slideDirection > 0)
			float newPositionX = (slideDirection > 0) ? boardToSlide.transform.localPosition.x + 375f : boardToSlide.transform.localPosition.x - 375f;
			boardToSlide.transform.localPosition = new Vector2(newPositionX, boardToSlide.transform.localPosition.y);

			currentSlidePosition = (slideDirection > 0) ? currentSlidePosition + 1 : currentSlidePosition - 1;
		}
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
		string boardName = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name;

		//Debug.Log("Board Name: " + boardName);
		//Debug.Log("Level Number: " + levelToLoad);

		if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/" + boardName + "/Campaign3x3_" + levelToLoad))
		{
			Debug.Log("Found 3x3 Scene");
			SceneManager.LoadScene("Levels/Campaign/" + boardName + "/Campaign3x3_" + levelToLoad);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/" + boardName + "/Campaign4x4_" + levelToLoad))
		{
			Debug.Log("Found 4x4 Scene");
			SceneManager.LoadScene("Levels/Campaign/" + boardName + "/Campaign4x4_" + levelToLoad);
		}
		else if (Application.CanStreamedLevelBeLoaded("Levels/Campaign/" + boardName + "/Campaign5x5_" + levelToLoad))
		{
			Debug.Log("Found 5x5 Scene");
			SceneManager.LoadScene("Levels/Campaign/" + boardName + "/Campaign5x5_" + levelToLoad);
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

		if (campaignMainMenu.activeSelf)
		{
			if (boardSelectMenu.activeSelf)
			{
				HideMenus();
				mainMenuButtons.SetActive(true);
				devOptionTapCount = 0;
			}
			else
			{
				HideBoards();
				boardToSlide = null;
				totalSlidePositions = 0;
				canSlideBoard = false;
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









