using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public GameObject mainMenuButtons;

	public GameObject campaignMainMenu;

	public GameObject versusMainMenu;
	public GameObject versusClassicMenu;
	public GameObject versusBattleMenu;
	public GameObject versusTwoPlayerMenu;
	
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
	}
	
	


	void HideMenus ()
	{
		mainMenuButtons.SetActive(false);
		campaignMainMenu.SetActive(false);
		versusMainMenu.SetActive(false);
		versusClassicMenu.SetActive(false);
		versusBattleMenu.SetActive(false);
		versusTwoPlayerMenu.SetActive(false);
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
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		string levelToLoad = buttonName.Substring(6, 3);

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

		if (versusMainMenu.activeSelf || campaignMainMenu.activeSelf)
		{
			HideMenus();
			mainMenuButtons.SetActive(true);
		}
		else if (versusClassicMenu.activeSelf || versusBattleMenu.activeSelf || versusTwoPlayerMenu.activeSelf)
		{
			HideMenus();
			versusMainMenu.SetActive(true);
		}
	}
}
