using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TutorialGameManager : MonoBehaviour 
{
	public static TutorialGameManager Instance;

	public List<Line> lineObjects = new List<Line>();

	private GameObject playerLine;
	public GameObject possiblePlayerLines;

	private GameObject computerLine;
	public GameObject possibleComputerLines;

	public GameObject possiblePlayerChips;
	public GameObject possibleComputerChips;

	private Vector3 lineGridScale;

	private GameObject _Dynamic;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;
	private Vector3 endDrawPosition;

	public bool isPlayerTurn;
	private bool placing;

	private bool isTutorial01;
	private bool isTutorial02;
	private bool isTutorial03;


	private int tutorialStep;
	private int tutorialPath;
	public bool isPassiveInstruction;

	public float passiveDismissDelay;


	private static GameObject bombButton;
	private bool canUseBomb;



	private int playerPoints;
	private Text playerPointsText;

	private int minWinPoints;
	private int totalPossiblePoints;
	public int oneStarScore;
	public int twoStarScore;
	public int threeStarScore;

	private Text neededPointsText;
	
	void Start () 
	{
		Instance = this;

		GameObject[] holder = GameObject.FindGameObjectsWithTag("LinePlacement");
		foreach (GameObject line in holder)
		{
			Line currentLine = line.GetComponent<Line>();
			lineObjects.Add(currentLine);

			SpriteRenderer lineSprite = line.transform.parent.transform.parent.gameObject.GetComponent<SpriteRenderer>();

			if(lineSprite.enabled)
			{
				currentLine.SetLineStatic(true);
				currentLine.SetOpen(false);
			}
			else 
			{
				currentLine.SetLineStatic(false);
				currentLine.SetOpen(true);
			}

		}

		_Dynamic = GameObject.Find("_Dynamic");

		//playerLine = (GameObject) Resources.Load("PlayerLine");
		//computerLine = (GameObject) Resources.Load("ComputerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;


		isTutorial01 = SceneManager.GetActiveScene().name.Contains("Tutorial01");
		isTutorial02 = SceneManager.GetActiveScene().name.Contains("Tutorial02");
		isTutorial03 = SceneManager.GetActiveScene().name.Contains("Tutorial03");
		//placing = false;

		tutorialStep = 1;

		if (SceneManager.GetActiveScene().name.Contains("Tutorial01"))
		{
			isPassiveInstruction = false;
			isPlayerTurn = false;
		}
		else
		{
			isPassiveInstruction = true;
			isPlayerTurn = true;
		}
		passiveDismissDelay = 0;


		canUseBomb = false;
		bombButton = GameObject.Find("BombButton");
		if (bombButton) bombButton.SetActive(false);


		if (!isTutorial01)
		{
			playerPoints = 0;
			playerPointsText = GameObject.Find("CurrentBoxesText").GetComponent<Text>();
			Debug.Log("Found Player Points");
			playerPointsText.text = "" + playerPoints;


			//minWinPoints = int.Parse(GameObject.Find("TotalBoxesText").GetComponent<Text>().text);
			totalPossiblePoints = GameObject.Find("LineGrid").transform.childCount;

			oneStarScore = (int) Mathf.Ceil(totalPossiblePoints * 0.3f);
			twoStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.6f);
			threeStarScore = (int) Mathf.Floor(totalPossiblePoints * 0.85f);

			neededPointsText = GameObject.Find("TotalBoxesText").GetComponent<Text>();
			neededPointsText.text = "" + oneStarScore;
		}
	}
	
	
	void Update () 
	{
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

		if(!isPlayerTurn) 
		{
			canUseBomb = false;
		}


		if (isPassiveInstruction)
		{
			passiveDismissDelay += Time.deltaTime;
			
			if (isTutorial01)
			{
				HandleTutorialOnePassiveInstructions();
			}
			else if (SceneManager.GetActiveScene().name.Contains("Tutorial02"))
			{
				HandleTutorialTwoPassiveInstructions();
			}
			else if (SceneManager.GetActiveScene().name.Contains("Tutorial03"))
			{
				HandleTutorialThreePassiveInstructions();
			}	
		}



		if (!isTutorial01)
		{
			playerPointsText.text = "" + playerPoints;
			UpdateNeededPointsText();
		}

		if (RoundOver())
		{
			if (isTutorial01)
			{
				tutorialStep = 8;
				StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0.75f));
				isPassiveInstruction = true;
			}
			else if (SceneManager.GetActiveScene().name.Contains("Tutorial02"))
			{
				tutorialStep = 9;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0.75f));
				//isPassiveInstruction = true;
			}
			else if (SceneManager.GetActiveScene().name.Contains("Tutorial03"))
			{
				tutorialStep = 13;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0.75f));
			}
		}
	}

	

	public void ContinueToNextTutorial ()
	{
		//if current scene is the last tutorial, set lastScene as such, load main menu
		if (SceneManager.GetActiveScene().name == "Campaign3x3_Tutorial03")
		{
			CampaignData.SetFinishedTutorial(true);
			SaveLoad.Save();
			CampaignData.SetLastScene("Campaign3x3_Tutorial03");
			//SceneManager.LoadScene("Campaign5x5_1-1");
			SceneManager.LoadScene("Menu");
		}
		//otherwise, loading another tutorial, reset paths and steps
		else
		{
			CampaignData.SetLastScene(SceneManager.GetActiveScene().name);		//set current scene as lastScene
			SetTutorialPath(0);					//these next two lines aren't necessary. wil be reset on scene load
			SetTutorialStep(1);

			//If,Else: Look at currentscene, Load next tutorial
			if (SceneManager.GetActiveScene().name.Contains("01"))
			{
				SceneManager.LoadScene("Campaign3x3_Tutorial02");
			}
			else if (SceneManager.GetActiveScene().name.Contains("02"))
			{
				SceneManager.LoadScene("Campaign3x3_Tutorial03");
			}
			
		}
	}




	public int GetTutorialStep ()
	{
		return tutorialStep;
	}

	public void SetTutorialStep (int step)
	{
		tutorialStep = step;
	}

	public int GetTutorialPath ()
	{
		return tutorialPath;
	}

	public void SetTutorialPath (int path)
	{
		tutorialPath = path;
	}




	public void DoTutorialOneStep ()
	{
		if(isTutorial01)
		{
			if (tutorialStep < 7)
			{
				if (!isPassiveInstruction)
				{
					GameObject tutorialButton = EventSystem.current.currentSelectedGameObject;

					if (tutorialButton.name.Contains("StepOne") && tutorialStep == 1)
					{
						PlayerDrawLine();
						tutorialStep = 2;

						if (tutorialButton.name == "StepOne_01")
						{
							tutorialPath = 1;
							StartCoroutine(ComputerDrawLine(GameObject.Find("Line_20").GetComponentInChildren<Line>()));
							StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 2.0f));
							isPassiveInstruction = true;
						}
						else if (tutorialButton.name == "StepOne_02")
						{
							tutorialPath = 2;
							StartCoroutine(ComputerDrawLine(GameObject.Find("Line_22").GetComponentInChildren<Line>()));
							StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 2.0f));
							isPassiveInstruction = true;
						}
					}
					else if (tutorialButton.name.Contains("StepThree") && tutorialStep == 3)
					{
						PlayerDrawLine();
						tutorialStep = 4;

						StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 2.0f));
						isPassiveInstruction = true;
					}
				}
			}
			else
			{
				if (isPlayerTurn)
				{
					PlayerDrawLine();
					isPlayerTurn = false;


					Line computerDrawChoice = null;
					foreach (Line line in lineObjects)
					{
						if (line.GetOpen()) computerDrawChoice = line;
					}
					Debug.Log("Computer Drawing");
					StartCoroutine(ComputerDrawLine(computerDrawChoice));
				}
			}
		}
	}

	void HandleTutorialOnePassiveInstructions ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(tutorialStep == 2 && passiveDismissDelay > 2.5f)
			{
				Debug.Log("Next step");
				tutorialStep = 3;
				StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 4 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 5;
				StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 5 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 6;
				StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 6 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 7;
				StartCoroutine(TutorialOneCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			/*else if (tutorialStep == 8 && passiveDismissDelay > 1.5f)
			{
				//load next level
				SceneManager.LoadScene("Campaign5x5_1-1");
			}*/
		}
	}


	public void DoTutorialTwoStep ()
	{
		if (isPlayerTurn && !RoundOver() && tutorialStep >= 8)
		{
			PlayerDrawLine();
		}
	}

	void HandleTutorialTwoPassiveInstructions ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(tutorialStep == 1 && passiveDismissDelay > 0.75f)
			{
				Debug.Log("Next step");
				tutorialStep = 2;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if(tutorialStep == 2 && passiveDismissDelay > 0.75f)
			{
				tutorialStep = 3;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 3 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 4;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 4 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 5;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 5 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 6;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 6 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 7;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 7 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 8;
				StartCoroutine(TutorialTwoCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
		}
	}


	public void DoTutorialThreeStep ()
	{
		GameObject tutorialButton = EventSystem.current.currentSelectedGameObject;

		if (isPlayerTurn && !RoundOver())
		{
			if (tutorialButton.name.Contains("StepFour") && tutorialStep == 4)
			{
				PlayerDrawLine();
				isPlayerTurn = false;

				if (!TutorialThreeCanvasUI.completedBombBox)
				{
					Line computerDrawChoice = null;
					if (tutorialButton.name == "StepFour_01" || tutorialButton.name == "StepFour_02")
					{
						computerDrawChoice = GameObject.Find("StepFour_03").GetComponent<Line>();
					}
					else if (tutorialButton.name == "StepFour_03")
					{
						computerDrawChoice = GameObject.Find("StepFour_01").GetComponent<Line>();
					}
					else if (tutorialButton.name == "StepFour_04" || tutorialButton.name == "StepFour_05")
					{
						computerDrawChoice = GameObject.Find("StepFour_06").GetComponent<Line>();
					}
					else if (tutorialButton.name == "StepFour_06")
					{
						computerDrawChoice = GameObject.Find("StepFour_04").GetComponent<Line>();
					}

					StartCoroutine(ComputerDrawLine(computerDrawChoice));
				}
				else
				{
					isPlayerTurn = true;
				}
			}

			if (tutorialStep == 8)
			{
				PlayerDrawLine();

				tutorialStep = 9;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0.75f));
			}
			else if (tutorialStep == 12)
			{
				PlayerDrawLine();
				/*isPlayerTurn = false;


				Line computerDrawChoice = null;
				foreach (Line line in lineObjects)
				{
					if (line.isOpen) computerDrawChoice = line;
				}
				Debug.Log("Computer Drawing");
				StartCoroutine(ComputerDrawLine(computerDrawChoice));*/
			}
		}
	}

	void HandleTutorialThreePassiveInstructions ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(tutorialStep == 1 && passiveDismissDelay > 0.75f)
			{
				Debug.Log("Next step");
				tutorialStep = 2;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if(tutorialStep == 2 && passiveDismissDelay > 0.75f)
			{
				tutorialStep = 3;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 3 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 4;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 4 && TutorialThreeCanvasUI.completedBombBox)
			{
				//tutorialStep = 5;
				//StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 5 && passiveDismissDelay > 1.0f)
			{
				//tutorialStep = 6;
				//StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 6 && passiveDismissDelay > 1.0f)
			{
				//tutorialStep = 7;
				//StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 7 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 8;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 8 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 9;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 9 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 10;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 10 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 11;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
			else if (tutorialStep == 11 && passiveDismissDelay > 1.0f)
			{
				tutorialStep = 12;
				StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
			}
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

	void PlayerDrawLine () 
	{
		Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();

		if (playerChoice.GetOpen())
		{
			Vector3 startPosition = playerChoice.linePosition;
			endDrawPosition = playerChoice.linePosition;

			if (playerChoice.lineRotation.z == 0)
			{
				startPosition.x = playerChoice.linePosition.x - (120f * lineGridScale.x);
			}
			else
			{
				startPosition.y = playerChoice.linePosition.y + (120f * lineGridScale.y);
			}


			//GameObject newLine = (GameObject) Instantiate(playerLine, startPosition, playerChoice.lineRotation);
			//newLine.name = "PlayerLine";

			GameObject newLine = possiblePlayerLines.transform.GetChild(0).gameObject;

			newLine.transform.position = startPosition;
			newLine.transform.rotation = playerChoice.lineRotation;
			newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
			newLine.transform.SetParent(_Dynamic.transform, false);

			newLine.SetActive(true);
			lineToDraw = newLine;
			canDraw = true;

			
			playerChoice.owner = "Player";

			playerChoice.boxParentOne.UpdateSideCount(1);
			if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

			
			if (playerChoice.boxParentOne.IsComplete()) 
			{
				Debug.Log("Box One Complete");
				playerChoice.boxParentOne.SetOwner("CampaignPlayer");
			}
			if (playerChoice.boxParentTwo.IsComplete()) 
			{
				Debug.Log("Box Two Complete");
				playerChoice.boxParentTwo.SetOwner("CampaignPlayer");
			}

			
			playerChoice.SetOpen(false);	

			//!contains tutorial01
			if (SceneManager.GetActiveScene().name.Contains("Tutorial02")) 
			{
				isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
			}
			

			if (SceneManager.GetActiveScene().name.Contains("Tutorial03"))
			{
				if ((playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) && tutorialStep == 4)
				{
					TutorialThreeCanvasUI.completedBombBox = true;			//unnecessary?

					tutorialStep = 5;
					StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0.75f));
					//isPlayerTurn = true;
					//tutorialStep = 5;
				}

				if (tutorialStep >= 12)
				{
					isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
				}
			}

		}	
	}


	IEnumerator ComputerDrawLine (Line computerChoice)
	{
		if (!RoundOver())
		{
			yield return new WaitForSeconds(0.75f);
			computerChoice.SetOpen(false);
			computerChoice.owner = "Computer";

			computerChoice.boxParentOne.UpdateSideCount(1);
			if (computerChoice.boxParentOne != computerChoice.boxParentTwo) computerChoice.boxParentTwo.UpdateSideCount(1);

			if (computerChoice.boxParentOne.IsComplete()) computerChoice.boxParentOne.SetOwner("Computer");
			if (computerChoice.boxParentTwo.IsComplete()) computerChoice.boxParentTwo.SetOwner("Computer");


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

			if (tutorialStep >= 7 && isTutorial01) isPlayerTurn = true;

			if (isTutorial03 && (tutorialStep == 4 || tutorialStep >= 12 )) isPlayerTurn = true;


		}
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




	public void PickedUpBomb ()
	{
		bombButton.SetActive(true);
	}

	public void ToggleBomb ()			//attach to powerup button
	{
		if (isPlayerTurn) canUseBomb = !canUseBomb;

		if(tutorialStep == 5 && isTutorial03)
		{
			tutorialStep = 6;
			StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
		}
		Debug.Log("Can Use Bomb: " + canUseBomb);
	}

	//Bomb PowerUp
	public void DestroyStaticLine ()			//attach to static line buttons
	{
		if(canUseBomb && !RoundOver())
		{
			Debug.Log("Trying to use bomb");
			Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();

			if (playerChoice.GetLineStatic())
			{
				//Debug.Log("Destroy Line");
				Color change = playerChoice.gameObject.transform.parent.transform.parent.GetComponent<SpriteRenderer>().color;
				change.a = 0f;
				playerChoice.gameObject.transform.parent.transform.parent.GetComponent<SpriteRenderer>().color = change;

				playerChoice.SetLineStatic(false);
				playerChoice.SetOpen(true);

				playerChoice.boxParentOne.UpdateSideCount(-1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(-1);

				canUseBomb = false;
				bombButton.SetActive(false);
			}
		}

		if(tutorialStep == 6)
		{
			tutorialStep = 7;
			StartCoroutine(TutorialThreeCanvasUI.Instance.DisplayTutorialSteps(tutorialStep, tutorialPath, 0f));
		}
	}
}
