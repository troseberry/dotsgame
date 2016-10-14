using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class HeroBoardCanvas : MonoBehaviour 
{
	private Canvas menuCavnas;
	public GameObject winGroup;
	public GameObject loseGroup;

	private Text finalTimeText;

	private bool didSave;

	void Start () 
    {
		menuCavnas = GetComponent<Canvas>();
		menuCavnas.enabled = false;

		finalTimeText = winGroup.transform.Find("TimeNumber").GetComponent<Text>();

		didSave = false;
	}
	
	void Update () 
    {
		if (HeroBoardManager.Instance.BoardWon())
		{
			menuCavnas.enabled = true;
			winGroup.SetActive(true);
			loseGroup.SetActive(false);

			DisplayFinalTime();
			UnlockPowerUp();
		}
		else if (HeroBoardManager.Instance.BoardFailed())
		{
			menuCavnas.enabled = true;
			winGroup.SetActive(false);
			loseGroup.SetActive(true);
		}
	}

	public void ResetLevel ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadLevelSelectMenu ()
	{
		CampaignData.SetLastScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene(0);
	}

	void DisplayFinalTime ()
	{
		float time = HeroBoardManager.Instance.CalculateFinalTime();
		string formattedTime = string.Format("{0}:{1:00.00}", (int)time / 60, time % 60);
		finalTimeText.text = formattedTime;
	}
	
	void UnlockPowerUp ()
	{
		string heroString = SceneManager.GetActiveScene().name.Split('_')[1];
		HeroManager.Hero hero = (HeroManager.Hero)Enum.Parse(typeof (HeroManager.Hero), heroString); 
		CampaignData.SetHeroBoardStatus(hero, true);

		if(!didSave)
		{
			SaveLoad.Save();
			didSave = true;
		}
	}
}