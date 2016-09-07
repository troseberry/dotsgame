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
		HideMenus();
		campaignMainMenu.SetActive(true);
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

		if (versusMainMenu.active || campaignMainMenu.active)
		{
			HideMenus();
			mainMenuButtons.SetActive(true);
		}
		else if (versusClassicMenu.active || versusBattleMenu.active || versusTwoPlayerMenu.active)
		{
			HideMenus();
			versusMainMenu.SetActive(true);
		}
	}
}
