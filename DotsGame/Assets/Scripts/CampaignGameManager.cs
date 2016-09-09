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

		minWinPoints = int.Parse(GameObject.Find("TotalBoxesText").GetComponent<Text>().text);
	}
	
	
	void Update () 
	{
		DebugPanel.Log("Is Player's Turn? ", isPlayerTurn);
		DebugPanel.Log("Player Points: ", playerPoints);

		playerPointsText.text = "" + playerPoints;
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

	public static string PlayerWon ()
	{
		if (playerPoints >= minWinPoints)
		{
			return "W";
		}
		else
		{
			return "L";
		}
	}
}
