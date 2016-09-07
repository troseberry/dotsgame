using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private GameObject playerLine;
	private Vector3 lineGridScale;
	
	void Start () 
	{
		playerLine = (GameObject) Resources.Load("PlayerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
	}
	
	
	void Update () 
	{
	
	}

	public void PlaceLine()
	{
		if(GameManager.isPlayerTurn && !GameManager.RoundOver())
		{
			Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
			Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();


			if (playerChoice.isOpen)
			{
				GameObject newLine = (GameObject) Instantiate(playerLine, playerChoice.linePosition, playerChoice.lineRotation);
				newLine.transform.localScale = lineGridScale;
				newLine.name = "PlayerLine";
				playerChoice.isOpen = false;

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

				if (playerChoice.boxParentOne.IsComplete()) playerChoice.boxParentOne.SetOwner("Player");
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("Player");

				GameManager.isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
			}
			//newLine.transform.SetParent(gameSpaceCanvas.transform.Find("LineGrid"), false);	
		}
	}
}
