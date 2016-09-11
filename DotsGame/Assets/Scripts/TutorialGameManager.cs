﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TutorialGameManager : MonoBehaviour 
{
	public static List<Line> lineObjects = new List<Line>();

	private GameObject playerLine;
	private GameObject computerLine;
	private Vector3 lineGridScale;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;
	private Vector3 endDrawPosition;

	public static bool isPlayerTurn;
	private bool placing;

	private int tutorialStep;
	private static int tutorialPath;
	public static bool isPassiveInstruction;

	public static float passiveDismissDelay;
	
	void Start () 
	{
		GameObject[] holder = GameObject.FindGameObjectsWithTag("LinePlacement");
		foreach (GameObject child in holder)
		{
			lineObjects.Add(child.GetComponent<Line>());
		}

		playerLine = (GameObject) Resources.Load("PlayerLine");
		computerLine = (GameObject) Resources.Load("ComputerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;

		isPlayerTurn = false;
		//placing = false;

		tutorialStep = 1;
		isPassiveInstruction = false;
		passiveDismissDelay = 0;



	}
	
	
	void Update () 
	{
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

		if (isPassiveInstruction)
		{
			passiveDismissDelay += Time.deltaTime;
			//get tap or mouse down?
			if(Input.GetMouseButtonDown(0))
			{
				if(tutorialStep == 2 && passiveDismissDelay > 2.5f)
				{
					Debug.Log("Next step");
					tutorialStep = 3;
					StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0f));
				}
				else if (tutorialStep == 4 && passiveDismissDelay > 1.0f)
				{
					tutorialStep = 5;
					StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0f));
				}
				else if (tutorialStep == 5 && passiveDismissDelay > 1.0f)
				{
					tutorialStep = 6;
					StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0f));
				}
				else if (tutorialStep == 6 && passiveDismissDelay > 1.0f)
				{
					tutorialStep = 7;
					StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0f));
				}
				/*else if (tutorialStep == 8 && passiveDismissDelay > 1.5f)
				{
					//load next level
					SceneManager.LoadScene("Campaign5x5_1-1");
				}*/
			}
		}

		if (RoundOver())
		{
			tutorialStep = 8;
			StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0.75f));
			isPassiveInstruction = true;
		}
	}

	public static bool RoundOver ()
	{
		foreach (Line obj in lineObjects)
		{
			if (obj.isOpen) return false;
		}
		return true;
	}


	public void SetTutorialStep (int step)
	{
		tutorialStep = step;
	}

	public static int GetTutorialPath ()
	{
		return tutorialPath;
	}

	public void DoTutorialStep ()
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
						StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 2.0f));
						isPassiveInstruction = true;
					}
					else if (tutorialButton.name == "StepOne_02")
					{
						tutorialPath = 2;
						StartCoroutine(ComputerDrawLine(GameObject.Find("Line_22").GetComponentInChildren<Line>()));
						StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 2.0f));
						isPassiveInstruction = true;
					}
				}
				else if (tutorialButton.name.Contains("StepThree") && tutorialStep == 3)
				{
					PlayerDrawLine();
					tutorialStep = 4;

					StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 2.0f));
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
					if (line.isOpen) computerDrawChoice = line;
				}
				Debug.Log("Computer Drawing");
				StartCoroutine(ComputerDrawLine(computerDrawChoice));
			}
		}
	}

	/*public void DismissPassiveInstruction ()
	{
		if (isPassiveInstruction)
		{
			if(tutorialStep == 2)
			{
				Debug.Log("Next step");
				tutorialStep = 3;
				Debug.Log("got here");
				StartCoroutine(TutorialCanvasUI.Instance.DisplayNextSteps(tutorialStep, tutorialPath, 0f));
			}
		}
	}*/

	void PlayerDrawLine () 
	{
		Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();

		if (playerChoice.isOpen)
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


			GameObject newLine = (GameObject) Instantiate(playerLine, startPosition, playerChoice.lineRotation);
			newLine.name = "PlayerLine";
			newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
			//newLine.transform.SetParent(placedLineGroup.transform, false);
			lineToDraw = newLine;
			canDraw = true;

			
			playerChoice.owner = "Player";

			playerChoice.boxParentOne.UpdateSideCount(1);
			if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

			
			if (playerChoice.boxParentOne.IsComplete()) 
			{
				playerChoice.boxParentOne.SetOwner("CampaignPlayer");
			}
			if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("CampaignPlayer");

			
			playerChoice.isOpen = false;		
		}	
	}


	IEnumerator ComputerDrawLine (Line computerChoice)
	{
		if (!RoundOver())
		{
			yield return new WaitForSeconds(1.5f);
			computerChoice.isOpen = false;
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

			GameObject newLine = (GameObject) Instantiate(computerLine, startPosition, computerChoice.lineRotation);
			newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
			//newLine.transform.SetParent(placedLineGroup.transform, false);
			newLine.name = "ComputerLine";
			lineToDraw = newLine;
			canDraw = true;

			if(tutorialStep >= 7) isPlayerTurn = true;
		}
	}
}