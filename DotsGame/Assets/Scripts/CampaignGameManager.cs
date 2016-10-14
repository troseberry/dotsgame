using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CampaignGameManager : MonoBehaviour 
{
	public static CampaignGameManager Instance;

	public List<Line> lineObjects = new List<Line>();

	public GameObject possiblePlayerChips;
	public GameObject possibleComputerChips;

	public bool isPlayerTurn;

	private int playerPoints;
	private Text playerPointsText;

	private int minWinPoints;
	private int totalPossiblePoints;
	private int totalLinesCount;
	private int oneStarScore;
	private int twoStarScore;
	private int threeStarScore;

	private Text neededPointsText;

	private string mode;
	
	void Start () 
	{
		Instance = this;
		mode = (SceneManager.GetActiveScene().name.Contains("HeroBoard")) ? "hero" : string.Empty;

		GameObject[] linePlacementholder = GameObject.FindGameObjectsWithTag("LinePlacement");
		int staticLineCount = 0;
		foreach (GameObject linePlacement in linePlacementholder)
		{
			Line currentLine = linePlacement.GetComponent<Line>();
			lineObjects.Add(currentLine);

			SpriteRenderer lineSprite = linePlacement.transform.parent.gameObject.GetComponent<SpriteRenderer>();
			if (lineSprite.enabled)
			{
				currentLine.SetLineStatic(true);
				staticLineCount++;
				currentLine.SetOpen(false);
			}
			else 
			{
				currentLine.SetLineStatic(false);
				currentLine.SetOpen(true);
			}
		}

		isPlayerTurn = true;
		playerPoints = 0;

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

		totalLinesCount = GameObject.Find("LineGrid").transform.childCount;
		totalPossiblePoints =  totalLinesCount - staticLineCount + bombCount + (2 * x2Count) + (3 * thiefCount);

		oneStarScore = (int) Mathf.Ceil(totalPossiblePoints * 0.3f);
		twoStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.6f);
		threeStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.85f);

		if (mode != "hero")
		{
			playerPointsText = GameObject.Find("CurrentBoxesText").GetComponent<Text>();
			playerPointsText.text = string.Empty + playerPoints;

			neededPointsText = GameObject.Find("TotalBoxesText").GetComponent<Text>();
			neededPointsText.text = string.Empty + oneStarScore;
		}
	}

	
	void Update () 
	{
		DebugPanel.Log("Is Player's Turn? ", isPlayerTurn);
		DebugPanel.Log("Player Points: ", playerPoints);

		if (mode != "hero")
		{
			playerPointsText.text = "" + playerPoints;
			UpdateNeededPointsText();
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

	public void ClearPlayerPoints ()
	{
		playerPoints = 0;
		Debug.Log("Total Points After Clear: " + playerPoints);
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

	//Needed to calculate star ratings after start for Hero Boards
	public void CalculateHeroScoring ()
	{
		int staticLineCount = 0;
		foreach (Line line in lineObjects)
		{
			SpriteRenderer lineSprite = line.transform.parent.gameObject.GetComponent<SpriteRenderer>();
			
			if (lineSprite.enabled) staticLineCount++;
		}
		totalPossiblePoints =  totalLinesCount - staticLineCount;

		oneStarScore = (int) Mathf.Ceil(totalPossiblePoints * 0.3f);
		twoStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.6f);
		threeStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.85f);

		//Debug.Log("Points Goal: " + twoStarScore);
		//Debug.Log("3 Star: " + threeStarScore);
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
}