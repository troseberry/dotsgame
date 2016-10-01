using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public static List<Line> lineObjects = new List<Line>();

	public Canvas gameSpaceCanvas;

	public static bool isPlayerTurn;

	private static int playerPoints;
	private Text playerPointsText;

	private static int totalPoints;
	
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

		totalPoints = int.Parse(GameObject.Find("TotalBoxesText").GetComponent<Text>().text);
	}
	
	
	void Update () 
	{
		DebugPanel.Log("Is Player's Turn? ", isPlayerTurn);
		DebugPanel.Log("Player Points: ", playerPoints);


		/*if (RoundOver()) 
		{
			Debug.Log("Round over. No Open boxes.");
		}*/


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

	public static int GetPlayerPoints ()
	{
		return playerPoints;
	}

	public static int GetTotalPoints()
	{
		return totalPoints;
	}

	public static string PlayerWon ()
	{
		//Debug.Log("Half Total Points: " + Mathf.Ceil(totalPoints / 2));
		//Debug.Log("Player Points: " + playerPoints);

		if (playerPoints > Mathf.Ceil(totalPoints / 2))
		{
			return "W";
		}
		else if (playerPoints < Mathf.Ceil(totalPoints / 2))
		{
			return "L";
		}
		else
		{
			return "D";
		}

	}
}
