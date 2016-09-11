using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CampaignGameManager : MonoBehaviour 
{
	public static List<Line> lineObjects = new List<Line>();

	public Canvas gameSpaceCanvas;

	public static bool isPlayerTurn;

	private static int playerPoints;
	private Text playerPointsText;

	private static int minWinPoints;
	private static int totalPossiblePoints;
	private static int oneStarScore;
	private static int twoStarScore;
	private static int threeStarScore;

	private Text neededPointsText;
	
	void Start () 
	{
		GameObject[] holder = GameObject.FindGameObjectsWithTag("LinePlacement");
		foreach (GameObject child in holder)
		{
			lineObjects.Add(child.GetComponent<Line>());
		}

		isPlayerTurn = true;

		playerPoints = 0;
		playerPointsText = GameObject.Find("CurrentBoxesText").GetComponent<Text>();
		playerPointsText.text = "" + playerPoints;


		//minWinPoints = int.Parse(GameObject.Find("TotalBoxesText").GetComponent<Text>().text);
		totalPossiblePoints = GameObject.Find("LineGrid").transform.childCount;

		oneStarScore = (int) Mathf.Ceil(totalPossiblePoints * 0.3f);
		twoStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.6f);
		threeStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.85f);

		neededPointsText = GameObject.Find("TotalBoxesText").GetComponent<Text>();
		neededPointsText.text = "" + oneStarScore;
	}
	
	
	void Update () 
	{
		DebugPanel.Log("Is Player's Turn? ", isPlayerTurn);
		DebugPanel.Log("Player Points: ", playerPoints);

		playerPointsText.text = "" + playerPoints;

		UpdateNeededPointsText();
	}

	public static bool RoundOver ()
	{
		foreach (Line obj in lineObjects)
		{
			if (obj.isOpen) return false;
		}
		return true;
	}

	public static void UpdatePlayerPoints (int amount)
	{
		playerPoints += amount;
	}

	public int GetPlayerPoints ()
	{
		return playerPoints;
	}

	void UpdateNeededPointsText ()
	{
		if (playerPoints >= oneStarScore && playerPoints < twoStarScore)
		{
			neededPointsText.text = "" + twoStarScore;
		}
		else if (playerPoints >= twoStarScore)
		{
			neededPointsText.text = "" + threeStarScore;
		}
	}

	public static string PlayerWon ()
	{
		if (playerPoints >= oneStarScore && playerPoints < twoStarScore)
		{
			return "S01";
		}
		else if (playerPoints >= twoStarScore && playerPoints < threeStarScore)
		{
			return "S02";
		}
		else if (playerPoints >= threeStarScore)
		{
			return "S03";
		}
		else
		{
			return "L";
		}
	}


	public void ClearStaticVars ()
	{
		
	}
}
