using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerAI : MonoBehaviour 
{
	private GameObject computerLine;
	private Vector3 lineGridScale;

	private GameObject placedLineGroup;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;

	private bool placing;

	private List<Box> boxObjects = new List<Box>();

	private static System.Random rng = new System.Random();

	private string mode;

	//private PowerUp currentPowerUp;
	
	void Start () 
	{
		computerLine = (GameObject) Resources.Load("ComputerLine");
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


		if (!GameObject.Find("PlacedLineGroup"))
		{
			placedLineGroup = new GameObject();
			placedLineGroup.name = "PlacedLineGroup";
		}
		else
		{
			placedLineGroup = GameObject.Find("PlacedLineGroup");
		}
		
	}
	
	
	void Update () 
	{
		bool versusConditions = !GameManager.isPlayerTurn && !placing && !GameManager.RoundOver();
		bool campaignConditions = !CampaignGameManager.isPlayerTurn && !placing && !CampaignGameManager.RoundOver();
		bool tutorialConditions = !TutorialGameManager.isPlayerTurn && !placing && !TutorialGameManager.RoundOver();

		//if ((!GameManager.isPlayerTurn && !placing && !GameManager.RoundOver()) || (!CampaignGameManager.isPlayerTurn && !placing && !CampaignGameManager.RoundOver()))
		if (versusConditions || campaignConditions || tutorialConditions)
		{
			//Invoke("ComputerPlaceLine", 2.0f);

			//StartCoroutine(ComputerPlaceLine( DetermineLineToPlace() ));
			StartCoroutine( ComputerDrawLine( DetermineLineToPlace() ));
			placing = true;
		}


		if (canDraw)
		{
			if (drawingTime < 2.0f) drawingTime += Time.deltaTime/drawDuration;
			lineToDraw.transform.localScale = Vector3.Lerp(lineToDraw.transform.localScale, lineGridScale, drawingTime);
			lineToDraw.transform.position =  Vector3.Lerp(lineToDraw.transform.position, endDrawPosition, drawingTime);
		}

		if (drawingTime >= 0.5f)
		{
			drawingTime = 0f;
			canDraw = false;
			if (lineToDraw) lineToDraw = null;
		}
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
					if (line.isOpen) 
					{
						toPlace = line;
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
					if (line.isOpen)
					{
						//look at that line's box parents. 
						Box parentOne = line.boxParentOne;
						Box parentTwo = line.boxParentTwo;

						//If both have 3 sides left open, then return that line
						if (parentOne.SidesLeftOpen() >= 3 && parentTwo.SidesLeftOpen() >= 3)
						{
							toPlace = line;
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
			int randomPlace = UnityEngine.Random.Range(0, GameManager.lineObjects.Count);
			toPlace = GameManager.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "campaign")
		{
			int randomPlace = UnityEngine.Random.Range(0, CampaignGameManager.lineObjects.Count);
			toPlace = CampaignGameManager.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "tutorial")
		{
			int randomPlace = UnityEngine.Random.Range(0, TutorialGameManager.lineObjects.Count);
			toPlace = TutorialGameManager.lineObjects.ElementAt(randomPlace);
		}


		if (toPlace.isOpen)
		{
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
		yield return new WaitForSeconds(1.5f);
		computerChoice.isOpen = false;
		computerChoice.owner = "Computer";

		computerChoice.boxParentOne.UpdateSideCount(1);
		if (computerChoice.boxParentOne != computerChoice.boxParentTwo) computerChoice.boxParentTwo.UpdateSideCount(1);

		if (computerChoice.boxParentOne.IsComplete()) computerChoice.boxParentOne.SetOwner("Computer");
		if (computerChoice.boxParentTwo.IsComplete()) computerChoice.boxParentTwo.SetOwner("Computer");

		//Debug.Log("Computer Placed Line");
		if(mode == "versus")
		{
			GameManager.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		else if (mode == "campaign")
		{
			CampaignGameManager.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		else if (mode == "tutorial")
		{
			TutorialGameManager.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
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

		GameObject newLine = (GameObject) Instantiate(computerLine, startPosition, computerChoice.lineRotation);
		newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
		newLine.transform.SetParent(placedLineGroup.transform, false);
		newLine.name = "ComputerLine";
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
