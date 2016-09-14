using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour 
{
	public GameObject mainMenuButtons;

	public GameObject campaignMainMenu;

	public GameObject versusMainMenu;
	public GameObject versusClassicMenu;
	public GameObject versusBattleMenu;
	public GameObject versusTwoPlayerMenu;

	public GameObject developerOptionsMenu;

	//private bool deleteSave;
	private int devOptionTapCount;
	
	void Start () 
	{
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
	}
	
	public void ShowDeveloperMenu ()
	{
		devOptionTapCount++;
		if (devOptionTapCount < 5)
		{
			
			Debug.Log("Tap Count: " + devOptionTapCount);
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
		SaveLoad.Delete();

		Debug.Log(CampaignData.GetFinishedTutorial());

		CampaignData.ClearBoardOneDictionary();

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
		CampaignData.SetFinishedTutorial(true);
		SaveLoad.Save();
		SaveLoad.Load();
	}


	void HideMenus ()
	{
		mainMenuButtons.SetActive(false);
		campaignMainMenu.SetActive(false);
		versusMainMenu.SetActive(false);
		versusClassicMenu.SetActive(false);
		versusBattleMenu.SetActive(false);
		versusTwoPlayerMenu.SetActive(false);

		developerOptionsMenu.SetActive(false);
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

			GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");
			foreach (GameObject btn in levelButtons)
			{
				string lvlNum = btn.name.Substring(6, btn.name.Length - 6);
				int prevLevel = (int.Parse(lvlNum.Substring(2, lvlNum.Length - 2))) - 1;
				Debug.Log(prevLevel);

				if (CampaignData.GetLevelStatus(lvlNum))
				{
					Debug.Log("Level Button Stuff:" + lvlNum);
					btn.transform.Find("CheckMark").gameObject.SetActive(true);
				}

				if (prevLevel != 0 && !CampaignData.GetLevelStatus("1-" + prevLevel))
				{
					btn.GetComponent<Button>().enabled = false;
					Color temp = btn.GetComponent<RawImage>().color;
					temp.a = 0.5f;
					btn.GetComponent<RawImage>().color = temp;

					Color textTemp = btn.transform.Find("LevelText").GetComponent<Text>().color;
					textTemp.a = 0.5f;
					btn.transform.Find("LevelText").GetComponent<Text>().color = textTemp;
				}
				else if (prevLevel != 0 && CampaignData.GetLevelStatus("1-" + prevLevel))
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

		SceneManager.LoadScene("Campaign5x5_" + levelToLoad);
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

		if (versusMainMenu.activeSelf || campaignMainMenu.activeSelf || developerOptionsMenu.activeSelf)
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
