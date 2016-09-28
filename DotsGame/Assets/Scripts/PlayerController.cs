using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private GameObject playerLine;
	private Vector3 lineGridScale;

	private GameObject _Dynamic;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;
	
	void Start () 
	{
		playerLine = (GameObject) Resources.Load("PlayerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;

		_Dynamic = GameObject.Find("_Dynamic");
	}

	void Update ()
	{
		DebugPanel.Log("Drawing Time: ", drawingTime);
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
	

	public void PlaceLine()
	{
		if(GameManager.isPlayerTurn && !GameManager.RoundOver())
		{
			//Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
			Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();


			if (playerChoice.isOpen)
			{
				//GameObject newLine = (GameObject) Instantiate(playerLine, playerChoice.linePosition, playerChoice.lineRotation);
				//newLine.transform.localScale = lineGridScale;
				DrawLine(playerChoice);
				
				playerChoice.isOpen = false;
				playerChoice.owner = "Player";

				//string newBoxOwner = SceneManager.GetActiveScene().name.Contains("Campaign") ? "CampaignPlayer" : "Player";

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

				if (playerChoice.boxParentOne.IsComplete()) playerChoice.boxParentOne.SetOwner("Player");
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("Player");

				GameManager.isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
			}
			//newLine.transform.SetParent(gameSpaceCanvas.transform.Find("LineGrid"), false);	
		}
	}


	void DrawLine (Line playerChoice) 
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
		newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
		newLine.name = "PlayerLine";
		newLine.transform.SetParent(_Dynamic.transform, false);
		lineToDraw = newLine;
		canDraw = true;
	}
}
