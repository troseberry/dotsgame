using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialTwoCanvasUI : MonoBehaviour 
{
	public static TutorialTwoCanvasUI Instance;

	private RectTransform step1Arrow;
	private RectTransform step2Arrow;
	private RectTransform step3Arrow;
	private RectTransform step5LeftArrow;
	private RectTransform step5RightArrow;
	private RectTransform step6LeftArrow;
	private RectTransform step6RightArrow;

	public GameObject step1;
	public GameObject step2;
	public GameObject step3;
	public GameObject step4;
	public GameObject step5;
	public GameObject step6;
	public GameObject step7;
	//step 8 turns off other ui. player free plays board
	public GameObject step9;

	public GameObject left_x2;
	public GameObject right_x2;

	private GameObject star01;
	private GameObject star02;
	private GameObject star03;

	private Text scoreNumber;

	void Start () 
	{
		Instance = this;

		step1Arrow = step1.transform.Find("IndicatorArrow").GetComponent<RectTransform>();
		step2Arrow = step2.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step3Arrow = step3.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();
		step5LeftArrow = step5.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step5RightArrow = step5.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();
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

		left_x2.SetActive(false);
		right_x2.SetActive(false);

		star01 = step9.transform.Find("Star_01").gameObject;
		star01.transform.GetChild(2).gameObject.SetActive(false);

		star02 = step9.transform.Find("Star_02").gameObject;
		star02.transform.GetChild(2).gameObject.SetActive(false);

		star03 = step9.transform.Find("Star_03").gameObject;
		star03.transform.GetChild(2).gameObject.SetActive(false);

		scoreNumber = step9.transform.Find("ScoreNumber").GetComponent<Text>();
	}
	
	
	void Update () 
	{
		HandleTutorialTwoArrows();
	}


	void HandleTutorialTwoArrows ()
	{
		if(step1.activeSelf)
		{	
			step1Arrow.transform.localPosition = new Vector3 (step1Arrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 500, step1Arrow.transform.localPosition.z);
		}
		else if (step2.activeSelf)
		{
			step2Arrow.transform.localPosition = new Vector3 (step2Arrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 500, step2Arrow.transform.localPosition.z);
		}
		else if (step3.activeSelf)
		{
			step3Arrow.transform.localPosition = new Vector3 (step3Arrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 500, step3Arrow.transform.localPosition.z);
		}
		else if (step5.activeSelf)
		{
			step5LeftArrow.transform.localPosition = new Vector3 (step5LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 350, step5LeftArrow.transform.localPosition.z);
			step5RightArrow.transform.localPosition = new Vector3 (step5RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 50, step5RightArrow.transform.localPosition.z);
		}
		else if (step6.activeSelf)
		{
			step6LeftArrow.transform.localPosition = new Vector3 (step6LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 350, step6LeftArrow.transform.localPosition.z);
			step6RightArrow.transform.localPosition = new Vector3 (step6RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 50, step6RightArrow.transform.localPosition.z);
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
		}
		else if (tutorialStep == 3)
		{
			step2.SetActive(false);
			step3.SetActive(true);
			
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 4)
		{
			step3.SetActive(false);
			step4.SetActive(true);
			
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
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
			
		}
		else if (tutorialStep == 8)
		{
			step7.SetActive(false);
			TutorialGameManager.Instance.isPassiveInstruction = false;
			TutorialGameManager.Instance.passiveDismissDelay = 0f;
			TutorialGameManager.Instance.isPlayerTurn = true;

			left_x2.SetActive(true);
			right_x2.SetActive(true);
		}
		else if (tutorialStep == 9)
		{
			step9.SetActive(true);
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
		else
		{
			//If player fails tutorial board, can reset and make the current step 7
		}
		scoreNumber.text = string.Empty + TutorialGameManager.Instance.GetPlayerPoints();
	}
}
