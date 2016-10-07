using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CampaignGameManager : MonoBehaviour 
{
	public static CampaignGameManager Instance;

	public List<Line> lineObjects = new List<Line>();

	//public Canvas gameSpaceCanvas;

	public GameObject possiblePlayerChips;
	public GameObject possibleComputerChips;

	public bool isPlayerTurn;

	private int playerPoints;
	private Text playerPointsText;

	private int minWinPoints;
	private int totalPossiblePoints;
	private int oneStarScore;
	private int twoStarScore;
	private int threeStarScore;

	private Text neededPointsText;
	
	void Start () 
	{
		Instance = this;
		//Debug.Log(SceneManager.GetActiveScene().path);

		GameObject[] linePlacementholder = GameObject.FindGameObjectsWithTag("LinePlacement");
		int staticLineCount = 0;
		foreach (GameObject linePlacement in linePlacementholder)
		{
			Line currentLine = linePlacement.GetComponent<Line>();

			lineObjects.Add(currentLine);

			SpriteRenderer lineSprite = linePlacement.transform.parent.transform.parent.gameObject.GetComponent<SpriteRenderer>();
			if (lineSprite.enabled)
			{
				currentLine.SetLineStatic(true);
				//Debug.Log(currentLine.lineName + " is Static: " + currentLine.GetLineStatic());
				staticLineCount++;

				currentLine.SetOpen(false);
			}
			else 
			{
				currentLine.SetLineStatic(false);
				currentLine.SetOpen(true);
			}

			/*if(linePlacement.GetComponent<Line>().isStatic)
			{
				staticLineCount++;
			}*/
		}

		isPlayerTurn = true;

		playerPoints = 0;
		playerPointsText = GameObject.Find("CurrentBoxesText").GetComponent<Text>();
		playerPointsText.text = "" + playerPoints;


		GameObject[] powerUpHolder = GameObject.FindGameObjectsWithTag("PowerUp");
		int x2Count = 0;
		int bombCount = 0;
		int thiefCount = 0;
		foreach (GameObject powerUp in powerUpHolder)
		{
			if (powerUp.name.Contains("x2"))
			{
				x2Count++;
			}
			else if (powerUp.name.Contains("Bomb"))
			{
				bombCount++;
			}
			else if (powerUp.name.Contains("Thief"))
			{
				thiefCount++;
			}
		}

		int totalLinesCount = GameObject.Find("LineGrid").transform.childCount;
		totalPossiblePoints =  totalLinesCount - staticLineCount + bombCount + (2 * x2Count) + (3 * thiefCount);

		oneStarScore = (int) Mathf.Ceil(totalPossiblePoints * 0.3f);
		twoStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.6f);
		threeStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.85f);

		neededPointsText = GameObject.Find("TotalBoxesText").GetComponent<Text>();
		neededPointsText.text = "" + oneStarScore;


		//Debug.Log("Current Selected Hero: " + CampaignData.currentHero.ToString());
	}

	
	
	void Update () 
	{
		//DebugPanel.Log("Is Player's Turn? ", isPlayerTurn);
		//DebugPanel.Log("Player Points: ", playerPoints);

		playerPointsText.text = "" + playerPoints;

		UpdateNeededPointsText();

		if (Input.GetKey(KeyCode.Escape))
		{
			//go back to campaign menu
		}

		foreach(Line line in lineObjects)
		{
			//DebugPanel.Log(line.lineName + "is Open: ", GameObject.Find(line.lineName).GetComponentInChildren<Line>().GetOpen());
		}
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

	public string PlayerWon ()
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


	public void PlayerGoesFirst ()
	{

	}
}
