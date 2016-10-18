using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerAI : MonoBehaviour 
{
	private List<GameObject> allBoxObjects = new List<GameObject>();
	private List<Box> cornerBoxes = new List<Box>();

	private GameObject computerLine;
	public GameObject possibleComputerLines;

	private Vector3 lineGridScale;

	private GameObject _Dynamic;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration;// = 2.0f;			//if hero board, make this shorter

	private Vector3 endDrawPosition;

	private bool placing;

	private List<Box> boxObjects = new List<Box>();

	private static System.Random rng = new System.Random();

	private string mode;


	private bool versusConditions;
	private bool campaignConditions;
	private bool tutorialConditions;

	//private PowerUp currentPowerUp;

	private int currentPlayerChain = 0;
	
	void Start () 
	{
		var holder = from element in GameObject.FindGameObjectsWithTag("Box")
					orderby element.name
					select element;

		foreach (GameObject box in holder)
		{
			allBoxObjects.Add(box);
			//Debug.Log(box.name);
		}

		cornerBoxes.Add(allBoxObjects[0].GetComponent<Box>());
		cornerBoxes.Add(allBoxObjects[(int)Mathf.Sqrt(allBoxObjects.Count) - 1].GetComponent<Box>());
		cornerBoxes.Add(allBoxObjects[allBoxObjects.Count - (int)Mathf.Sqrt(allBoxObjects.Count)].GetComponent<Box>());
		cornerBoxes.Add(allBoxObjects[allBoxObjects.Count - 1].GetComponent<Box>());

		// foreach(Box corner in cornerBoxes)
		// {
		// 	Debug.Log(corner.GetBoxNumber());
		// }
		

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
		else if (SceneManager.GetActiveScene().name.Contains("HeroBoard"))
		{
			mode = "hero";
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

		if (mode != "hero")
		{
			drawDuration = 2.0f;
		}
		else
		{
			drawDuration = 0.5f;
		}
		
	}
	
	
	void Update () 
	{
		if (mode == "versus") versusConditions = !GameManager.Instance.isPlayerTurn && !placing && !GameManager.Instance.RoundOver();
		if (mode == "campaign" || mode == "hero") campaignConditions = !CampaignGameManager.Instance.isPlayerTurn && !placing && !CampaignGameManager.Instance.RoundOver();
		if (mode== "tutorial") tutorialConditions = !TutorialGameManager.Instance.isPlayerTurn && !placing && !TutorialGameManager.Instance.RoundOver();


		if (versusConditions || campaignConditions)
		{
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
			if (drawingTime < drawDuration) drawingTime += Time.deltaTime/drawDuration;
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
	}

	private Line DetermineLineToPlace ()
	{
		Line toPlace = null;
		List<Box> boxObjectsClone = boxObjects;
		Shuffle(boxObjectsClone);

		

		foreach (Box box in boxObjectsClone)
		{
			//Look for boxes with only 1 open side first
			if (box.SidesLeftOpen() == 1)
			{
				
				for (int i = 0; i < box.boxLineObjects.Count; i++)
				{
					if (box.boxLineObjects[i].GetOpen()) 
					{
						//Debug.Log("Found 1 Open Sided Box");
						toPlace = box.boxLineObjects[i];
						return toPlace;
					}
				}
			}
			//Look for boxes with 3 open sides
			else if (box.SidesLeftOpen() >= 3)
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
							//Debug.Log("Found 3 or 4 Open Sided Box");
							toPlace = line;
							return toPlace;
						}
					}
				}
			}
		}

		//check if corner boxes have 2 outside open lines
		int randomLine = UnityEngine.Random.Range(0, 2);

		if (cornerBoxes[0].SidesLeftOpen() == 2 && cornerBoxes[0].boxLineObjects[0].GetOpen() && cornerBoxes[0].boxLineObjects[1].GetOpen())
		{
			//Debug.Log("Open Upper Left Corner Box");
			toPlace = randomLine == 0 ? cornerBoxes[0].boxLineObjects[0] : cornerBoxes[0].boxLineObjects[0];
			return toPlace;
		}
		else if (cornerBoxes[1].SidesLeftOpen() == 2 && cornerBoxes[1].boxLineObjects[0].GetOpen() && cornerBoxes[1].boxLineObjects[2].GetOpen())
		{
			//Debug.Log("Open Upper Right Corner Box");
			toPlace = randomLine == 0 ? cornerBoxes[1].boxLineObjects[0] : cornerBoxes[1].boxLineObjects[2];
			return toPlace;
		}
		else if (cornerBoxes[2].SidesLeftOpen() == 2 && cornerBoxes[2].boxLineObjects[1].GetOpen() && cornerBoxes[2].boxLineObjects[3].GetOpen())
		{
			//Debug.Log("Open Lower Left Corner Box");
			toPlace = randomLine == 0 ? cornerBoxes[2].boxLineObjects[1] : cornerBoxes[2].boxLineObjects[3];
			return toPlace;
		}
		else if (cornerBoxes[3].SidesLeftOpen() == 2 && cornerBoxes[3].boxLineObjects[2].GetOpen() && cornerBoxes[3].boxLineObjects[3].GetOpen())
		{
			//Debug.Log("Open Lower Right Corner Box");
			toPlace = randomLine == 0 ? cornerBoxes[3].boxLineObjects[2] : cornerBoxes[3].boxLineObjects[3];
			return toPlace;
		}



		
		foreach (Box box in boxObjectsClone)
		{
			//Would only get here if theres a line straddling 3 open and 2 open sided box
			if (box.SidesLeftOpen() >= 3)
			{
				//Debug.Log("Found 3 Open Sided Box and 2 Open Sided Box");
				//if 0 or 3 is not open, then outnumbered line is odd
				//if 1 or 2 is not open, then outnumbered line is even 
				int currentBoxNumber = box.GetBoxNumber();
				Box verifyBox = null;

				if ( !box.boxLineObjects[0].GetOpen() || !box.boxLineObjects[3].GetOpen() )
				{
					if (currentBoxNumber % 10 != 0)
					{
						//went to lower box
						verifyBox = GameObject.Find("Box_" + (currentBoxNumber - 1)).GetComponent<Box>();

						if (box.boxLineObjects[0].GetOpen())			//outnumbered line is top
						{
							if (verifyBox.boxLineObjects[3].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[1];		//place line at lower ever
							}
							else
							{
								toPlace = box.boxLineObjects[2];
							}
						}
						else											//outnumbered line is bottom
						{
							if (verifyBox.boxLineObjects[0].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[2];		//place line at higer even
							}
							else
							{
								toPlace = box.boxLineObjects[1];
							}
						}
					}
					else
					{
						//Went to a higher box
						verifyBox = GameObject.Find("Box_" + (currentBoxNumber + 1)).GetComponent<Box>();

						if (box.boxLineObjects[0].GetOpen())			//outnumbered line is top
						{
							if (verifyBox.boxLineObjects[3].GetOpen())	//opposite side is open
							{
								toPlace = box.boxLineObjects[2];		//place line at higher even
							}
							else										//opposite side is closed
							{
								toPlace = box.boxLineObjects[1];		//place line at lower ever
							}
						}
						else											//outnumbered line is bottom
						{
							if (verifyBox.boxLineObjects[0].GetOpen())	//opposite side is open
							{
								toPlace = box.boxLineObjects[1];		//place line at lower even
							}
							else										//opposite side is closed
							{
								toPlace = box.boxLineObjects[2];		//place line at higher even
							}
						}
					}
				}
				else
				{
					if (currentBoxNumber - 10 >= 10)
					{
						//went to lower box
						verifyBox = GameObject.Find("Box_" + (currentBoxNumber - 10)).GetComponent<Box>();

						if (box.boxLineObjects[1].GetOpen())			//outnumbered line is left
						{
							if (verifyBox.boxLineObjects[2].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[0];		//place line at lower odd
							}
							else
							{
								toPlace = box.boxLineObjects[3];
							}
						}
						else											//outnumbered line is right
						{
							if (verifyBox.boxLineObjects[1].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[3];		//place line at higher odd
							}
							else
							{
								toPlace = box.boxLineObjects[0];
							}
						}
					}
					else
					{
						//went to higher box
						verifyBox = GameObject.Find("Box_" + (currentBoxNumber + 10)).GetComponent<Box>();

						if (box.boxLineObjects[1].GetOpen())			//outnumbered line is left
						{
							if (verifyBox.boxLineObjects[2].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[3];		//place line at higher odd
							}
							else
							{
								toPlace = box.boxLineObjects[0];
							}
						}
						else											//outnumbered line is right
						{
							if (verifyBox.boxLineObjects[1].GetOpen())	//opposite side check
							{
								toPlace = box.boxLineObjects[0];		//place line at lower odd
							}
							else
							{
								toPlace = box.boxLineObjects[3];
							}
						}
					}
				}

				return toPlace;
			}
		}
	

		int optimalPlayerChain = 0;
		Line optimalLineChoice = null;

		Line currentLineChoice = null;
		Box currentBox = null;
		List<Line> checkedLines = new List<Line>();

		List<Line> perimeterLinesRef = new List<Line>();
		if (campaignConditions)
		{
			perimeterLinesRef = CampaignGameManager.Instance.perimeterLines;
		}
		else if (versusConditions)
		{
			perimeterLinesRef = GameManager.Instance.perimeterLines;
		}


		foreach (Line line in perimeterLinesRef)
		{
			if (line.GetOpen() && line.boxParentOne.SidesLeftOpen() == 2 && !checkedLines.Contains(line))
			{
				currentPlayerChain = 0;

				currentLineChoice = line;
				checkedLines.Add(currentLineChoice);
				//Debug.Log("Current Line Choice: " + currentLineChoice.lineName);
				currentBox = line.boxParentOne;			//box parents should be the same

				do
				{
					currentBox = currentLineChoice.GetOtherBoxParent(currentBox);
					currentPlayerChain++;
					currentLineChoice = currentBox.GetOtherOpenLine(currentLineChoice);
				} while (currentLineChoice.BoxParentsDiffer());
				checkedLines.Add(currentLineChoice);
				// Debug.Log("Current Line Choice Post-Loop: " + currentLineChoice.lineName);
				// Debug.Log("Recent Chain Search: " + currentPlayerChain);
				// Debug.Log("Optimial Player Chain: " + optimalPlayerChain);


				if (optimalPlayerChain == 0)
				{
					optimalPlayerChain = currentPlayerChain;
					optimalLineChoice = line;
				}
				else if (currentPlayerChain < optimalPlayerChain)
				{
					optimalPlayerChain = currentPlayerChain;
					optimalLineChoice = line;
				}
			}
		}

		//If found shortest chain, place there
		if (optimalLineChoice)
		{
			toPlace = optimalLineChoice;
			//Debug.Log("To Place: "+ toPlace.lineName);
			return toPlace;
		}


		//otherwise, place randomly
		if (mode == "versus")
		{
			int randomPlace = UnityEngine.Random.Range(0, GameManager.Instance.lineObjects.Count);
			toPlace = GameManager.Instance.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "campaign" || mode == "hero")
		{
			int randomPlace = UnityEngine.Random.Range(0, CampaignGameManager.Instance.lineObjects.Count);
			toPlace = CampaignGameManager.Instance.lineObjects.ElementAt(randomPlace);
		}
		else if (mode == "tutorial")
		{
			int randomPlace = UnityEngine.Random.Range(0, TutorialGameManager.Instance.lineObjects.Count);
			toPlace = TutorialGameManager.Instance.lineObjects.ElementAt(randomPlace);
		}

		//Debug.Log("Randomly Placing");


		if (toPlace.GetOpen())
		{
			return toPlace;
		}
		else
		{
			return DetermineLineToPlace();
		}
	}

	IEnumerator ComputerDrawLine (Line computerChoice) 
	{
		yield return new WaitForSeconds(0.75f);
		computerChoice.SetOpen(false);
		computerChoice.owner = "Computer";

		string boxOwner = (mode == "campaign" || mode == "hero") ? "CampaignComputer" : "Computer";



		computerChoice.boxParentOne.UpdateSideCount(1);
		if (computerChoice.boxParentOne != computerChoice.boxParentTwo) computerChoice.boxParentTwo.UpdateSideCount(1);

		if (computerChoice.boxParentOne.IsComplete()) computerChoice.boxParentOne.SetOwner(boxOwner);
		if (computerChoice.boxParentTwo.IsComplete()) computerChoice.boxParentTwo.SetOwner(boxOwner);

		//Debug.Log("Computer Placed Line");
		if(mode == "versus")
		{
			GameManager.Instance.isPlayerTurn = (computerChoice.boxParentOne.IsComplete() || computerChoice.boxParentTwo.IsComplete()) ? false : true;
		}
		else if (mode == "campaign" || mode == "hero")
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

		GameObject newLine = possibleComputerLines.transform.GetChild(0).gameObject;
		
		newLine.transform.position = startPosition;
		newLine.transform.rotation = computerChoice.lineRotation;
		newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
		newLine.transform.SetParent(_Dynamic.transform, false);

		newLine.SetActive(true);
		lineToDraw = newLine;
		canDraw = true;
	}

	

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
