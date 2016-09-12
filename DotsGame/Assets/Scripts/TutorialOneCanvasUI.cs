using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialOneCanvasUI : MonoBehaviour 
{
	public static TutorialOneCanvasUI Instance;

	private RectTransform step1LeftArrow;
	private RectTransform step1RightArrow;

	private RectTransform step3LeftArrow;
	private RectTransform step3RightArrow;

	public GameObject step1;
	public GameObject step2;
	public GameObject step3;
	public GameObject step4;
	public GameObject step5;
	public GameObject step6;
	//step 7 has no needed Ui. player free plays rest of board
	public GameObject step8;

	void Start () 
	{
		Instance = this;

		step1LeftArrow = step1.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step1RightArrow = step1.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();

		step3LeftArrow = step3.transform.Find("IndicatorArrow_L").GetComponent<RectTransform>();
		step3RightArrow = step3.transform.Find("IndicatorArrow_R").GetComponent<RectTransform>();


		step1.SetActive(true);
		step2.SetActive(false);
		step3.SetActive(false);
		step4.SetActive(false);
		step5.SetActive(false);
		step6.SetActive(false);
		step8.SetActive(false);
	}
	
	
	void Update () 
	{
		HandleTutorialOneArrows();
	}


	void HandleTutorialOneArrows ()
	{
		if(step1.activeSelf)
		{	step1LeftArrow.transform.localPosition = new Vector3 (step1LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 115, step1LeftArrow.transform.localPosition.z);
			step1RightArrow.transform.localPosition = new Vector3 (step1RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) + 115, step1RightArrow.transform.localPosition.z);
		}
		else if (step3.activeSelf)
		{
			if (TutorialGameManager.GetTutorialPath() == 1)
			{
				step3LeftArrow.transform.localPosition = new Vector3 (step3LeftArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 515, step3LeftArrow.transform.localPosition.z);
			}
			else 
			{
				step3RightArrow.transform.localPosition = new Vector3 (step3RightArrow.transform.localPosition.x, (Mathf.Sin(Time.time * 3f) * 30) - 515, step3RightArrow.transform.localPosition.z);
			}
		}
	}


	public IEnumerator DisplayTutorialSteps (int tutorialStep, int tutorialPath, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		if (tutorialStep == 2)
		{
			step1.SetActive(false);
			step2.SetActive(true);
			if (tutorialPath == 1)
			{
				step2.transform.Find("IndicatorArrow_L").gameObject.SetActive(true);
				step2.transform.Find("IndicatorArrow_R").gameObject.SetActive(false);
			}
			else if (tutorialPath == 2)
			{
				step2.transform.Find("IndicatorArrow_L").gameObject.SetActive(false);
				step2.transform.Find("IndicatorArrow_R").gameObject.SetActive(true);
			}
		}
		else if (tutorialStep == 3)
		{
			step2.SetActive(false);
			step3.SetActive(true);
			if (tutorialPath == 1)
			{
				step3.transform.Find("IndicatorArrow_L").gameObject.SetActive(true);
				step3.transform.Find("IndicatorArrow_R").gameObject.SetActive(false);
			}
			else if (tutorialPath == 2)
			{
				step3.transform.Find("IndicatorArrow_L").gameObject.SetActive(false);
				step3.transform.Find("IndicatorArrow_R").gameObject.SetActive(true);
			}
			TutorialGameManager.isPassiveInstruction = false;
			TutorialGameManager.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 4)
		{
			step3.SetActive(false);
			step4.SetActive(true);
			if (tutorialPath == 1)
			{
				step4.transform.Find("Path_01").gameObject.SetActive(true);
				step4.transform.Find("Path_02").gameObject.SetActive(false);
			}
			else if (tutorialPath == 2)
			{
				step4.transform.Find("Path_01").gameObject.SetActive(false);
				step4.transform.Find("Path_02").gameObject.SetActive(true);
			}
		}
		else if (tutorialStep == 5)
		{
			step4.SetActive(false);
			step5.SetActive(true);
			TutorialGameManager.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 6)
		{
			step5.SetActive(false);
			step6.SetActive(true);
			TutorialGameManager.passiveDismissDelay = 0f;
		}
		else if (tutorialStep == 7)
		{
			step6.SetActive(false);
			TutorialGameManager.isPassiveInstruction = false;
			TutorialGameManager.passiveDismissDelay = 0f;
			TutorialGameManager.isPlayerTurn = true;
		}
		else if (tutorialStep == 8)
		{
			step8.SetActive(true);
		}
	}
}
