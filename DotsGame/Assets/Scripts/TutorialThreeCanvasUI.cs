﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialThreeCanvasUI : MonoBehaviour 
{
	public static TutorialThreeCanvasUI Instance;

	private RectTransform step1LeftArrow;
	private RectTransform step1RightArrow;
	private RectTransform step2LeftArrow;
	private RectTransform step2RightArrow;
	private RectTransform step3LeftArrow;
	private RectTransform step3RightArrow;
	private RectTransform step5Arrow;
	private RectTransform step6LeftArrow;
	private RectTransform step6RightArrow;

	public GameObject step1;
	public GameObject step2;
	public GameObject step3;
	public GameObject step4;
	public GameObject step5;
	public GameObject step6;
	public GameObject step7;
	//need 8 as gap step for player action
	public GameObject step9;
	public GameObject step10;
	public GameObject step11;
	public GameObject step13;

	public GameObject leftBomb;
	public GameObject rightBomb;

	public static bool completedBombBox;			//true after player finishes bomb box. Use for check in handle passive instructions to incrememnt tutorial step

	//public SpriteRenderer staticLineOne;
	//public SpriteRenderer staticLineTwo;

	private GameObject star01;
	private GameObject star02;
	private GameObject star03;

	private Text scoreNumber;

	void Start () 
	{
		Instance = this;

		step1LeftArrow = step1.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step1RightArrow = step1.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();

		step2LeftArrow = step2.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step2RightArrow = step2.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();

		step3LeftArrow = step3.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step3RightArrow = step3.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();

		step5Arrow = step5.transform.Find("IndicatorArrow").GetComponent<RectTransform>();

		step6LeftArrow = step6.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step6RightArrow = step6.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();


		step1.SetActive(true);
		step2.SetActive(false);
		step3.SetActive(false);
		step4.SetActive(false);
		step5.SetActive(false);
		step6.SetActive(false);
		step7.SetActive(false);

		step9.SetActive(false);
		step10.SetActive(false);
		step11.SetActive(false);
		step13.SetActive(false);

		leftBomb.SetActive(false);
		rightBomb.SetActive(false);

		star01 = step13.transform.Find("Star_01").gameObject;
		star01.transform.GetChild(2).gameObject.SetActive(false);

		star02 = step13.transform.Find("Star_02").gameObject;
		star02.transform.GetChild(2).gameObject.SetActive(false);

		star03 = step13.transform.Find("Star_03").gameObject;
		star03.transform.GetChild(2).gameObject.SetActive(false);


		scoreNumber = step13.transform.Find("ScoreNumber").GetComponent<Text>();
	}
	
	
	void Update () 
	{
		HandleTutorialThreeArrows();

		//DebugPanel.Log("Static Line Destroyed: ", StaticLineDestroyed());
		//DebugPanel.Log("Static Line One: ", staticLineOne.color);
		//DebugPanel.Log("Static Line Two: ", staticLineTwo.color);
	}


	void HandleTutorialThreeArrows ()
	{
		if(step1.activeSelf)
		{	
			step1LeftArrow.transform.localPosition = new Vector3 (step1LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step1LeftArrow.transform.localPosition.z);
			step1RightArrow.transform.localPosition = new Vector3 (step1RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step1RightArrow.transform.localPosition.z);
		}
		else if (step2.activeSelf)
		{
			step2LeftArrow.transform.localPosition = new Vector3 (step2LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step2LeftArrow.transform.localPosition.z);
			step2RightArrow.transform.localPosition = new Vector3 (step2RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step2RightArrow.transform.localPosition.z);
		}
		else if (step3.activeSelf)
		{
			step3LeftArrow.transform.localPosition = new Vector3 (step3LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 350, step3LeftArrow.transform.localPosition.z);
			step3RightArrow.transform.localPosition = new Vector3 (step3RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 50, step3RightArrow.transform.localPosition.z);
		}
		else if (step5.activeSelf)
		{
			step5Arrow.transform.localPosition = new Vector3 ((Mathf.Sin(Time.time * 3f) * 30) - 125, step5Arrow.transform.localPosition.y, step5Arrow.transform.localPosition.z);
		}
		else if (step6.activeSelf)
		{
			step6LeftArrow.transform.localPosition = new Vector3 (step6LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step6LeftArrow.transform.localPosition.z);
			step6RightArrow.transform.localPosition = new Vector3 (step6RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 275, step6RightArrow.transform.localPosition.z);
		}

	}


	public IEnumerator DisplayTutorialSteps (int tutorialStep, int tutorialPath, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		if (tutorialStep == 2)
		{
			step1.SetActive(false);
			step2.SetActive(true);

			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.SetTutorialPath(0);

		}
		else if (tutorialStep == 3)
		{
			step2.SetActive(false);
			step3.SetActive(true);

			leftBomb.SetActive(true);
			leftBomb.transform.parent.GetComponent<Box>().SetPowerUp(leftBomb);

			rightBomb.SetActive(true);
			rightBomb.transform.parent.GetComponent<Box>().SetPowerUp(rightBomb);
			
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 4)
		{
			step3.SetActive(false);
			step4.SetActive(true);
			
			TutorialGameManager.Instance.isPassiveInstruction = false;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.isPlayerTurn = true;

		}
		else if (tutorialStep == 5)
		{
			step4.SetActive(false);
			step5.SetActive(true);

			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 6)
		{
			step5.SetActive(false);
			step6.SetActive(true);

			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}

		else if (tutorialStep == 7)
		{
			step6.SetActive(false);
			step7.SetActive(true);

			TutorialGameManager.Instance.isPassiveInstruction = true;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			
		}
		else if (tutorialStep == 8)
		{
			//step7.SetActive(false);
			TutorialGameManager.Instance.isPassiveInstruction = false;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.isPlayerTurn = true;
		}
		else if (tutorialStep == 9)
		{
			step7.SetActive(false);
			step9.SetActive(true);

			TutorialGameManager.Instance.isPassiveInstruction = true;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 10)
		{
			step9.SetActive(false);
			step10.SetActive(true);

			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 11)
		{
			step10.SetActive(false);
			step11.SetActive(true);

			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.isPlayerTurn = true;
		}
		else if (tutorialStep == 12)
		{
			step11.SetActive(false);

			TutorialGameManager.Instance.isPassiveInstruction = false;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.isPlayerTurn = true;
		}
		else if (tutorialStep == 13)
		{
			step13.SetActive(true);

			PlayerWon();
		}
	}

	void PlayerWon ()
	{
		if (TutorialGameManager.Instance.GetPlayerPoints() >= TutorialGameManager.Instance.oneStarScore && TutorialGameManager.Instance.GetPlayerPoints() < TutorialGameManager.Instance.twoStarScore)
		{
			star01.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (TutorialGameManager.Instance.GetPlayerPoints() >= TutorialGameManager.Instance.twoStarScore && TutorialGameManager.Instance.GetPlayerPoints() < TutorialGameManager.Instance.threeStarScore)
		{
			star01.transform.GetChild(2).gameObject.SetActive(true);
			star02.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (TutorialGameManager.Instance.GetPlayerPoints() >= TutorialGameManager.Instance.threeStarScore)
		{
			star01.transform.GetChild(2).gameObject.SetActive(true);
			star02.transform.GetChild(2).gameObject.SetActive(true);
			star03.transform.GetChild(2).gameObject.SetActive(true);
		}

		scoreNumber.text = string.Empty + TutorialGameManager.Instance.GetPlayerPoints();
	}

	/*public bool StaticLineDestroyed ()
	{
		return (staticLineOne.color.a == 0 || staticLineTwo.color.a == 0);
	}*/
}
