using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance;

	public List<Line> lineObjects = new List<Line>();

	public Canvas gameSpaceCanvas;

	public bool isPlayerTurn;

	private int playerPoints;
	private Text playerPointsText;

	private int totalPoints;
	
	void Start () 
	{
		Instance = this;

		GameObject[] holder = GameObject.FindGameObjectsWithTag("LinePlacement");
		foreach (GameObject line in holder)
		{
			lineObjects.Add(line.GetComponent<Line>());

			//since classic would never have static lines, set all lines open
			line.GetComponent<Line>().SetOpen(true);
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

	public bool RoundOver ()
	{
		foreach (Line obj in lineObjects)
		{
			if (obj.GetOpen()) return false;
		}
		return true;
	}

	public void UpdatePlayerPoints (int amount)
	{
		playerPoints += amount;
	}

	public int GetPlayerPoints ()
	{
		return playerPoints;
	}

	public int GetTotalPoints()
	{
		return totalPoints;
	}

	public string PlayerWon ()
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
