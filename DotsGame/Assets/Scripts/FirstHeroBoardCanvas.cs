using UnityEngine;
using System.Collections;

public class FirstHeroBoardCanvas : MonoBehaviour 
{
	public GameObject step1;
	public GameObject step2;
	public GameObject step3;
	public GameObject step4;
	public GameObject step5;
	public GameObject step6;
	public GameObject step7;
	public GameObject step8;

	private int currentStep;
	private float passiveDismissDelay;

	void Start () 
    {
		SaveLoad.Load();
		if (CampaignData.ViewedMatchAbilityInstructions())
		{
			gameObject.SetActive(false);
		}
		else
		{
			//Debug.Log("Viewed Instructions Already: " + CampaignData.ViewedMatchAbilityInstructions());
			step1.SetActive(true);
			step2.SetActive(false);
			step3.SetActive(false);
			step4.SetActive(false);
			step5.SetActive(false);
			step6.SetActive(false);
			step7.SetActive(false);
			step8.SetActive(false);

			currentStep = 1;
			passiveDismissDelay = 0f;
		}
	}
	
	void Update () 
    {
		DebugPanel.Log("Dimiss Delay: ", passiveDismissDelay);
		passiveDismissDelay += Time.deltaTime;

		if (Input.GetMouseButtonDown(0) && passiveDismissDelay >= 1.5f)
		{
			currentStep++;
			DisplayNextInstruction(currentStep);
		}
	}

	void DisplayNextInstruction (int currentStep)
	{
		switch (currentStep)
		{
			case 2:
				step1.SetActive(false);
				step2.SetActive(true);
				break;
			case 3:
				step2.SetActive(false);
				step3.SetActive(true);
				break;
			case 4:
				step3.SetActive(false);
				step4.SetActive(true);
				break;
			case 5:
				step4.SetActive(false);
				step5.SetActive(true);
				break;
			case 6:
				step5.SetActive(false);
				step6.SetActive(true);
				break;
			case 7:
				step6.SetActive(false);
				step7.SetActive(true);
				break;
			case 8:
				step7.SetActive(false);
				step8.SetActive(true);
				break;
			case 9:
				gameObject.SetActive(false);
				CampaignData.SetAbilityInstructionsState(true);
				SaveLoad.Save();
				break;
		}
		passiveDismissDelay = 0f;
	}
}
