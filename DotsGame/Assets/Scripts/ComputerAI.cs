using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerAI : MonoBehaviour 
{
	private GameObject computerLine;
	public GameObject possibleComputerLines;

	private Vector3 lineGridScale;

	private GameObject _Dynamic;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;

	private bool placing;

	private List<Box> boxObjects = new List<Box>();

	private static System.Random rng = new System.Random();

	private string mode;


	private bool versusConditions;
	private bool campaignConditions;
	private bool tutorialConditions;

	//private PowerUp currentPowerUp;
	
	void Start () 
	{
		//computerLine = (GameObject) Resources.Load("ComputerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;

		canDraw = false;
		drawingTime = 0f;

		placing = false;

		GameObject[] boxHolder = GameObject.FindGameObjectsWithTag("Box");
		foreach (GameObject child in boxHolder)
		{
			boxObjects.Add(child.GetComponent<Box>());
		}

		//mode = SceneManager.GetActiveScene().name.Contains("Campaign") ? "campaign" : "versus";

		if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
		{
			mode = "tutorial";
		}
		else
		{
			if (SceneManager.GetActiveScene().name.Contains("Campaign"))
			{
				mode = "campaign";
			}
			else
			{
				mode = "versus";
			}
		}

		_Dynamic = GameObject.Find("_Dynamic");

		versusConditions = false;
		campaignConditions = false;
		tutorialConditions = false;
		
	}
	
	
	void Update () 
	{
		if (mode == "versus") versusConditions = !GameManager.Instance.isPlayerTurn && !placing && !GameManager.Instance.RoundOver();
		if (mode == "campaign") campaignConditions = !CampaignGameManager.Instance.isPlayerTurn && !placing && !CampaignGameManager.Instance.RoundOver();
		if (mode== "tutorial") tutorialConditions = !TutorialGameManager.Instance.isPlayerTurn && !placing && !TutorialGameManager.Instance.RoundOver();


		//if ((!GameManager.isPlayerTurn && !placing && !GameManager.RoundOver()) || (!CampaignGameManager.isPlayerTurn && !placing && !CampaignGameManager.RoundOver()))
		if (versusConditions || campaignConditions/* || tutorialConditions*/)
		{
			//Invoke("ComputerPlaceLine", 2.0f);

			//StartCoroutine(ComputerPlaceLine( DetermineLineToPlace() ));
			StartCoroutine( ComputerDrawLine( DetermineLineToPlace() ));
			placing = true;
		}
		else if (tutorialConditions)
		{
			if (!SceneManager.GetActiveScene().name.Contains("03"))
			{
				StartCoroutine( ComputerDrawLine( DetermineLineToPlace() ));
				placing = true;
			}
			else
			{
				if (TutorialGameManager.Instance.GetTutorialStep() >= 12)
				{
					StartCoroutine( ComputerDrawLine( DetermineLineToPlace() ));
					placing = true;
				}
			}
		}


		if (canDraw)
		{
			if (drawingTime < 2.0f) drawingTime += Time.deltaTime/drawDuration;
			lineToDraw.transform.localScale = Vector3.Lerp(lineToDraw.transform.localScale, lineGridScale, drawingTime);
			lineToDraw.transform.position =  Vector3.Lerp(lineToDraw.transform.position, endDrawPosition, drawingTime);
		
			if (lineToDraw.transform.localScale.x >= (0.9f * lineGridScale.x))
			{
				lineToDraw.transform.localScale = new Vector3(lineGridScale.x, lineToDraw.transform.localScale.y, lineToDraw.transform.localScale.z);
				lineToDraw.transform.position = endDrawPosition;
				drawingTime = 0f;
				canDraw = false;
				if (lineToDraw) lineToDraw = null;
			}
		}

		/*if (drawingTime >= 0.5f)
		{
			drawingTime = 0f;
			canDraw = false;
			if (lineToDraw) lineToDraw = null;
		}*/
	}

	private Line DetermineLineToPlace ()
	{
		Line toPlace = null;
		List<Box> boxObjectsClone = boxObjects;
		Shuffle(boxObjectsClone);

		//Look for boxes with only 1 open side first
		foreach (Box box in boxObjectsClone)
		{
			if (box.SidesLeftOpen() == 1)
			{
				foreach (Line line in box.boxLineObjects)
				{
					if (line.GetOpen()) 
					{
						toPlace = line;
						//Debug.Log("Computer Placing Last Line at " + toPlace.lineName + " and line is open: " + toPlace.isOpen);
						return toPlace;
					}
				}
			}
		}

		//Look for boxes with 3 open sides
		foreach (Box box in boxObjectsClone)
		{
			if (box.SidesLeftOpen() >= 3)
			{
				//foreach open line
				foreach (Line line in box.boxLineObjects)
				{
					if (line.GetOpen())
					{
						//look at that line's box parents. 
						Box parentOne = line.boxParentOne;
						Box parentTwo = line.boxParentTwo;

						//If both have 3 sides left open, then return that line
						if (parentOne.SidesLeftOpen() >= 3 && parentTwo.SidesLeftOpen() >= 3)
						{
							toPlace = line;
							//Debug.Log("Computer Placing 1st or 2nd Line at " + toPlace.lineName + " and line is open: " + toPlace.isOpen);
							return toPlace;
						}
					}
				}
			}
		}

		//if no boxes with 3 open sides, or boxes with only one open side, then place randomly
		//(should later come back and change this so that it places to limit the player's chain length)

		if (mode == "versus")
		{
			int randomPlace = UnityEngine.Random.Range(0, GameManager.Instance.lineObjects.Count);
			toPlace = GameManager.Instance.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "campaign")
		{
			int randomPlace = UnityEngine.Random.Range(0, CampaignGameManager.Instance.lineObjects.Count);
			toPlace = CampaignGameManager.Instance.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "tutorial")
		{
			int randomPlace = UnityEngine.Random.Range(0, TutorialGameManager.Instance.lineObjects.Count);
			toPlace = TutorialGameManager.Instance.lineObjects.ElementAt(randomPlace);
		}


		if (toPlace.GetOpen())
		{
			//Debug.Log("Computer Placing 3rd Line at " + toPlace.lineName + " and line is open: " + toPlace.isOpen);
			return toPlace;
		}
		else
		{
			return DetermineLineToPlace();
		}
	}


	/*
	IEnumerator ComputerPlaceLine(Line toPlace)
	{
		yield return new WaitForSeconds(2.0f);

		//GameObject computerChoice = (GameObject) Instantiate(computerLine, toPlace.linePosition, toPlace.lineRotation);
		//computerChoice.transform.localScale = lineGridScale;
		//computerChoice.name = "ComputerLine";
		toPlace.isOpen = false;
		toPlace.owner = "Computer";

		toPlace.boxParentOne.UpdateSideCount(1);
		if (toPlace.boxParentOne != toPlace.boxParentTwo) toPlace.boxParentTwo.UpdateSideCount(1);

		if (toPlace.boxParentOne.IsComplete()) toPlace.boxParentOne.SetOwner("Computer");
		if (toPlace.boxParentTwo.IsComplete()) toPlace.boxParentTwo.SetOwner("Computer");

		//Debug.Log("Computer Placed Line");
		GameManager.isPlayerTurn = (toPlace.boxParentOne.IsComplete() || toPlace.boxParentTwo.IsComplete()) ? false : true;
		placing = false;
	}*/


	IEnumerator ComputerDrawLine (Line computerChoice) 
	{
		yield return new WaitForSeconds(0.75f);
		computerChoice.SetOpen(false);
		computerChoice.owner = "Computer";

		string boxOwner = (mode == "campaign") ? "CampaignComputer" : "Computer";



		computerChoice.boxParentOne.UpdateSideCount(1);
		if (computerChoice.boxParentOne != computerChoice.boxParentTwo) computerChoice.boxParentTwo.UpdateSideCount(1);

		if (computerChoice.boxParentOne.IsComplete()) computerChoice.boxParentOne.SetOwner(boxOwner);
		if (computerChoice.boxParentTwo.IsComplete()) computerChoice.boxParentTwo.SetOwner(boxOwner);

		//Debug.Log("Computer Placed Line");
		if(mode == "versus")
		{
			GameManager.Instance.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		else if (mode == "campaign")
		{
			CampaignGameManager.Instance.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		else if (mode == "tutorial")
		{
			TutorialGameManager.Instance.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		placing = false;


		Vector3 startPosition = computerChoice.linePosition;
		endDrawPosition = computerChoice.linePosition;

		if (computerChoice.lineRotation.z == 0)
		{
			startPosition.x = computerChoice.linePosition.x - (120f * lineGridScale.x);
		}
		else
		{
			startPosition.y = computerChoice.linePosition.y + (120f * lineGridScale.y);
		}

		//GameObject newLine = (GameObject) Instantiate(computerLine, startPosition, computerChoice.lineRotation);
		//newLine.name = "ComputerLine";


		GameObject newLine = possibleComputerLines.transform.GetChild(0).gameObject;
		
		newLine.transform.position = startPosition;
		newLine.transform.rotation = computerChoice.lineRotation;
		newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
		newLine.transform.SetParent(_Dynamic.transform, false);

		newLine.SetActive(true);
		lineToDraw = newLine;
		canDraw = true;
	}

	/*void ComputerPlaceLine()
	{
		int randomPlace = Random.Range(0, GameManager.lineObjects.Count);
		Line toPlace = GameManager.lineObjects.ElementAt(randomPlace);

		if(toPlace.isOpen)
		{
			GameObject computerChoice = (GameObject) Instantiate(computerLine, toPlace.linePosition, toPlace.lineRotation);
			computerChoice.name = "ComputerLine";
			toPlace.isOpen = false;

			toPlace.boxParentOne.UpdateSideCount(1);
			if(toPlace.boxParentOne != toPlace.boxParentTwo) toPlace.boxParentTwo.UpdateSideCount(1);

			//Debug.Log("Computer Placed Line");
			GameManager.isPlayerTurn = (toPlace.boxParentOne.IsComplete() || toPlace.boxParentTwo.IsComplete()) ? false : true;
			placing = false;
		}
		else
		{
			ComputerPlaceLine();
		}
	}*/

	public static void Shuffle (List<Box> list)
	{
		int n = list.Count;

		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Box value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
