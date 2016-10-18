using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

	private bool playerStartsRound;
	private Text startingTurnText;
	private bool turnTextVisible;
	private Color originalTextColor; 

	private int roundWins;
	private Text roundWinsText;
	private Text currentPointsText;

	private bool randomizedBoard;

	private bool paused;

	private Text roundTimerText;
	private string formattedTimer;

	private float currentTimer;
	private float roundDuration;
	private float timeBonus;
	private float overallTotalTime;

	private Text roundDurationText;
	private Text pointGoalText;
	private Text timeBonusText;

	private string sceneName;

	void Start () 
    {
		Instance = this;

		_Dynamic = GameObject.Find("_Dynamic").transform;
		boxes = GameObject.FindGameObjectsWithTag("Box");

		sceneName = SceneManager.GetActiveScene().name;
		int pointGoalNumber = 0;

		if (sceneName.Contains("3x3"))
		{
			boardSize = ((int) Mathf.Sqrt(boxes.Length));
			roundDuration = 45.0f;
			timeBonus = 5.0f;
			pointGoalNumber = 5;		//2 Star Rating Equivalent
		}
		else if (sceneName.Contains("4x4"))
		{
			boardSize = ((int) Mathf.Sqrt(boxes.Length)) * 2;
			roundDuration = 60.0f;
			timeBonus = 10.0f;
			pointGoalNumber = 10;		//2 Star Rating Equivalent
		}
		else if (sceneName.Contains("5x5"))
		{
			boardSize = ((int) Mathf.Sqrt(boxes.Length)) * 3;
			roundDuration = 75.0f;
			timeBonus = 15.0f;
			pointGoalNumber = 16;		//2 Star Rating Equivalent
		}	

		usedLines = new List<int>();

		scoreObjects = GameObject.Find("ScoreObjects");


		playerStartsRound = true;
		startingTurnText = GameObject.Find("StartingTurn").GetComponent<Text>();
		turnTextVisible = true;
		originalTextColor = startingTurnText.color;

		roundWins = 0;
		roundWinsText = GameObject.Find("CurrentRoundTotal").GetComponent<Text>();
		roundWinsText.text = string.Empty + roundWins;

		currentPointsText = GameObject.Find("CurrentPointTotal").GetComponent<Text>();
		currentPointsText.text = string.Empty;

		randomizedBoard = false;

		roundTimerText = GameObject.Find("TimerText").GetComponent<Text>();
		currentTimer = roundDuration;
		overallTotalTime = roundDuration;


		roundDurationText = GameObject.Find("TimeAmount").GetComponent<Text>();
		roundDurationText.text = roundDuration + " Seconds";

		pointGoalText = GameObject.Find("PointsNeeded").GetComponent<Text>();
		pointGoalText.text = pointGoalNumber + "/Round";

		timeBonusText = GameObject.Find("BonusTimeAmount").GetComponent<Text>();
		timeBonusText.text = timeBonus + " Seconds";
		
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
			if (!randomizedBoard)
			{
				Invoke("RandomizeBoardLayout",0.5f);
				Invoke("SwitchTurnText", 0.5f);
				randomizedBoard = true;

				playerStartsRound = !playerStartsRound;
				CampaignGameManager.Instance.isPlayerTurn = playerStartsRound;
			}	
		}

		roundWinsText.text = string.Empty + roundWins;
		currentPointsText.text = string.Empty + CampaignGameManager.Instance.GetPlayerPoints();
		
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
		ClearBoard();

		if (CampaignGameManager.Instance.PlayerWon() == "S02" || CampaignGameManager.Instance.PlayerWon() == "S03")
		{
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

			while (!CampaignGameManager.Instance.lineObjects[randomLine].GetOpen() 
					&& CampaignGameManager.Instance.lineObjects[randomLine].boxParentOne.SidesLeftOpen() > 1 
					&& CampaignGameManager.Instance.lineObjects[randomLine].boxParentTwo.SidesLeftOpen() > 1)
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
		

		for (int i = 0; i < CampaignGameManager.Instance.lineObjects.Count; i++)
		{
			CampaignGameManager.Instance.lineObjects[i].ResetLine();
		}

		for (int i = 0; i < boxes.Length; i++)
		{
			boxes[i].GetComponent<Box>().ResetBox();
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
		return (roundTimerText.text == "0:00.00");
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