using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManagerTwoPlayer : MonoBehaviour 
{
	public static List<Line> lineObjects = new List<Line>();

	public Canvas gameSpaceCanvas;

	public static bool isPlayerOneTurn;
	public static bool isPlayerTwoTurn;

	private static int playerOnePoints;
	private static int playerTwoPoints;
	//private Text playerPointsText;

	private static int totalPoints;
	
	void Start () 
	{
		GameObject[] holder = GameObject.FindGameObjectsWithTag("LinePlacement");
		foreach (GameObject child in holder)
		{
			lineObjects.Add(child.GetComponent<Line>());
		}


		isPlayerOneTurn = true;
		playerOnePoints = 0;

		isPlayerTwoTurn = false;
		playerTwoPoints = 0;

		//playerPointsText = GameObject.Find("CurrentBoxesText").GetComponent<Text>();
		//playerPointsText.text = "" + playerPoints;

		totalPoints = int.Parse(GameObject.Find("TotalBoxesText").GetComponent<Text>().text);
	}
	
	
	void Update () 
	{
		DebugPanel.Log("Player One's Turn: ", isPlayerOneTurn);
		DebugPanel.Log("Player Two's Turn: ", isPlayerTwoTurn);

		DebugPanel.Log("Player One's Points: ", GetPlayerPoints("One"));
		DebugPanel.Log("Player Two's Points: ", GetPlayerPoints("Two"));
	}

	public static bool RoundOver ()
	{
		foreach (Line obj in lineObjects)
		{
			if (obj.isOpen) return false;
		}
		return true;
	}

	public static void UpdatePlayerPoints (string player, int amount)
	{
		if(player == "One")
		{
			playerOnePoints += amount;
		}
		else if (player == "Two")
		{
			playerTwoPoints += amount;
		}
	}

	public int GetPlayerPoints (string player)
	{
		if(player == "One")
		{
			return playerOnePoints;
		}
		else if (player == "Two")
		{
			return playerTwoPoints;
		}
		else
		{
			return -1;
		}
		
	}

	public static string PlayerWon ()
	{
		if (playerOnePoints > Mathf.Ceil(totalPoints / 2))
		{
			return "W1";
		}
		else if (playerOnePoints < Mathf.Ceil(totalPoints / 2))
		{
			return "W2";
		}
		else
		{
			return "D";
		}
	}
}
