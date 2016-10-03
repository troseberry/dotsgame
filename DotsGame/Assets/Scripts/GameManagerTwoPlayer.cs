using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManagerTwoPlayer : MonoBehaviour 
{
	public static GameManagerTwoPlayer Instance;

	public List<Line> lineObjects = new List<Line>();

	public Canvas gameSpaceCanvas;

	public bool isPlayerOneTurn;
	public bool isPlayerTwoTurn;

	private int playerOnePoints;
	private int playerTwoPoints;
	//private Text playerPointsText;

	private int totalPoints;
	
	void Start () 
	{
		Instance = this;

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

	public bool RoundOver ()
	{
		foreach (Line obj in lineObjects)
		{
			if (obj.isOpen) return false;
		}
		return true;
	}

	public void UpdatePlayerPoints (string player, int amount)
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

	public int GetTotalPoints ()
	{
		return totalPoints;
	}

	public string PlayerWon ()
	{
		//Debug.Log("Half Total Points: " + Mathf.Ceil(totalPoints / 2));
		//Debug.Log("Player One Points: " + playerOnePoints);
		//Debug.Log("Player Two Points: " + playerTwoPoints);

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
