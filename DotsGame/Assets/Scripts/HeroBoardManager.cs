using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeroBoardManager : MonoBehaviour 
{
	public static HeroBoardManager Instance;

	private Transform _Dynamic;

	public Transform possiblePlayerLines;
	public Transform possibleComputerLines;
	public Transform possiblePlayerChips;
	public Transform possibleComputerChips;

	private GameObject[] boxes;

	private int boardSize;
	private List<int> usedLines;

	private GameObject scoreObjects;

	private Text startingTurnText;
	private bool turnTextVisible;
	private Color originalTextColor; 

	private int roundWins;
	private Text roundWinsText;
	private bool randomizedBoard;

	private bool paused;

	private Text roundTimerText;
	private string formattedTimer;

	private float currentTimer;
	private float roundDuration = 60.0f;
	private float timeBonus = 10.0f;
	private float overallTotalTime;

	private Text roundDurationText;
	private Text pointGoalText;
	private Text timeBonusText;

	void Start () 
    {
		Instance = this;

		_Dynamic = GameObject.Find("_Dynamic").transform;
		boxes = GameObject.FindGameObjectsWithTag("Box");

		boardSize = ((int) Mathf.Sqrt(boxes.Length));// + 1;
		usedLines = new List<int>();

		scoreObjects = GameObject.Find("ScoreObjects");
		//scoreObjects.SetActive(false);

		startingTurnText = GameObject.Find("StartingTurn").GetComponent<Text>();
		turnTextVisible = true;
		originalTextColor = startingTurnText.color;

		roundWins = 0;
		roundWinsText = GameObject.Find("CurrentRoundTotal").GetComponent<Text>();
		roundWinsText.text = string.Empty + roundWins;

		randomizedBoard = false;

		roundTimerText = GameObject.Find("TimerText").GetComponent<Text>();
		currentTimer = roundDuration;
		overallTotalTime = roundDuration;

		// roundDurationText = GameObject.Find("TimeAmount").GetComponent<Text>();
		// roundDurationText.text = roundDuration + " Seconds";

		// pointGoalText = GameObject.Find("PointsNeeded").GetComponent<Text>();
		// timeBonusText = GameObject.Find("BonusTimeAmount").GetComponent<Text>();
		
		paused = true;
	}
	
	void Update () 
    {
		DebugPanel.Log("Round Over: ", CampaignGameManager.Instance.RoundOver());
		DebugPanel.Log("Round Time Left: ", currentTimer);

		if (!paused)
		{
			if (currentTimer > 0)
			{
				currentTimer -= Time.deltaTime;
			}
			else
			{
				currentTimer = 0;
			}

			formattedTimer = string.Format("{0}:{1:00.00}", (int)currentTimer / 60, currentTimer % 60);
			DebugPanel.Log("Timer: ", formattedTimer);
			roundTimerText.text = formattedTimer;			
		}


		if (CampaignGameManager.Instance.RoundOver())
		{
			//This visually looks cool. Can tell it's randomizing lines
			//But it also calls RandomizeBoardLayout about 30 times each invoke
			//RandomizeBoardLayout();
			
			if (!randomizedBoard)
			{
				Invoke("RandomizeBoardLayout",0.5f);
				Invoke("SwitchTurnText", 0.5f);
				randomizedBoard = true;

				//For 3x3, if the number of generated static lines (or boardSize) is 2, have to manually switch starting turn here
				CampaignGameManager.Instance.isPlayerTurn = !CampaignGameManager.Instance.isPlayerTurn;
			}	
		}

		roundWinsText.text = string.Empty + roundWins;
		if (turnTextVisible) 
		{
			Invoke("HideTurnText", 3.0f);
			turnTextVisible = false;
		}

		if (BoardWon())
		{
			paused = true;
		}
	}

	void InternalClearPoints ()
	{
		CampaignGameManager.Instance.ClearPlayerPoints();
	}

	public void RandomizeBoardLayout ()
	{
		//Debug.Log("Randomizing Board");
		Debug.Log("Total Points Before Clear: " + CampaignGameManager.Instance.GetPlayerPoints());
		ClearBoard();

		Debug.Log("Round Rating: " + CampaignGameManager.Instance.PlayerWon());

		if (CampaignGameManager.Instance.PlayerWon() == "S02" || CampaignGameManager.Instance.PlayerWon() == "S03")
		{
			Debug.Log("Won Round");
			IncrementRoundWins();

			if (!BoardWon())
			{
				currentTimer += timeBonus;
				overallTotalTime += timeBonus;
			}
		}
		CampaignGameManager.Instance.ClearPlayerPoints();
		


		Line lineToStatic = null;

		for (int i = 0; i < boardSize; i++)
		{
			int randomLine = Random.Range(0, CampaignGameManager.Instance.lineObjects.Count);

			while (!CampaignGameManager.Instance.lineObjects[randomLine].GetOpen())
			{
				randomLine = Random.Range(0, CampaignGameManager.Instance.lineObjects.Count); 
			}
			
			lineToStatic = CampaignGameManager.Instance.lineObjects[randomLine];

			lineToStatic.transform.parent.GetComponent<SpriteRenderer>().enabled = true;
			lineToStatic.SetLineStatic(true);
			lineToStatic.SetOpen(false);
		}
		randomizedBoard = false;
	}

	void ClearBoard ()
	{
		usedLines.Clear();
		

		foreach (Line line in CampaignGameManager.Instance.lineObjects)
		{
			line.ResetLine();
		}

		foreach (GameObject box in boxes)
		{
			box.GetComponent<Box>().ResetBox();
		}

		for (int i = _Dynamic.childCount-1; i > -1 ; i--)
		{
			Transform obj = _Dynamic.GetChild(i);
			switch(obj.name)
			{
				case "PlayerLine":
					obj.SetParent(possiblePlayerLines, false);
					break;
				case "ComputerLine":
					obj.SetParent(possibleComputerLines, false);
					break;
				case "PlayerChip":
					obj.SetParent(possiblePlayerChips, false);
					break;
				case "ComputerChip":
					obj.SetParent(possibleComputerChips, false);
					break;
			}	
			obj.position = Vector3.zero;
			obj.gameObject.SetActive(false);
		}
	}

	public void IncrementRoundWins ()
	{
		roundWins++;
	}

	public bool BoardWon ()
	{
		return (roundWins >= 3);
	}

	public bool BoardFailed ()
	{
		return (currentTimer <= 0);
	}

	public void StartBoard ()
	{	
		GameObject.Find("StartPrompt").SetActive(false);
		RandomizeBoardLayout();
		CampaignGameManager.Instance.CalculateHeroScoring();
		paused = false;
	}

	public void TogglePause () 
	{
		paused = !paused;
	}

	void SwitchTurnText ()
	{
		
		startingTurnText.color = originalTextColor;
		turnTextVisible = true;
		startingTurnText.text = (startingTurnText.text == "Player Start") ? "Computer Start" : "Player Start";
	}

	void HideTurnText ()
	{
		startingTurnText.GetComponent<Animation>().Play("TextFade");
	}

	public float CalculateFinalTime ()
	{
		return (overallTotalTime - currentTimer);
	}
}
