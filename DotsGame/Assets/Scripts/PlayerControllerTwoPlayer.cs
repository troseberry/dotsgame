using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerControllerTwoPlayer : MonoBehaviour 
{
	private GameObject _Dynamic;

	private GameObject playerOneLine;
	private GameObject playerTwoLine;
	private Vector3 lineGridScale;

	private bool canDraw;
	private bool canPlayerOneDraw;
	private bool canPlayerTwoDraw;

	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;
	
	void Start () 
	{
		_Dynamic = GameObject.Find("_Dynamic");
		
		playerOneLine = (GameObject) Resources.Load("PlayerLine");
		playerTwoLine = (GameObject) Resources.Load("ComputerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;

		canDraw = false;
		//canPlayerOneDraw = false;
		//canPlayerTwoDraw = false;
		drawingTime = 0f;
	}
	
	
	void Update () 
	{
		DebugPanel.Log("Drawing Time: ", drawingTime);
		if (canDraw)
		{
			if (drawingTime < 2.0f) drawingTime += Time.deltaTime/drawDuration;
			lineToDraw.transform.localScale = Vector3.Lerp(lineToDraw.transform.localScale, lineGridScale, drawingTime);
			lineToDraw.transform.position =  Vector3.Lerp(lineToDraw.transform.position, endDrawPosition, drawingTime);
		
			if (lineToDraw.transform.localScale.x >= 0.9f)
			{
				lineToDraw.transform.localScale = new Vector3(1.0f, lineToDraw.transform.localScale.y, lineToDraw.transform.localScale.z);
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

		
	}


	public void PlayerDrawLine () 
	{
		if(!GameManagerTwoPlayer.Instance.RoundOver())
		{
			//Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
			Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();


			if (playerChoice.isOpen)
			{
				//DrawLine(playerChoice);
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



				GameObject newLine = null;
				if (GameManagerTwoPlayer.Instance.isPlayerOneTurn) 
				{
					newLine = (GameObject) Instantiate(playerOneLine, startPosition, playerChoice.lineRotation);
					newLine.name = "PlayerOneLine";
				}
				else if (GameManagerTwoPlayer.Instance.isPlayerTwoTurn) 
				{
					newLine = (GameObject) Instantiate(playerTwoLine, startPosition, playerChoice.lineRotation);
					newLine.name = "PlayerTwoLine";
				}

				newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
				newLine.transform.SetParent(_Dynamic.transform, false);
				lineToDraw = newLine;
				canDraw = true;


				
				//Update side counts and dole out points if need be
				string boxOwner = GameManagerTwoPlayer.Instance.isPlayerOneTurn ? "PlayerOne" : "PlayerTwo";
				playerChoice.isOpen = false;

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

				
				if (playerChoice.boxParentOne.IsComplete()) playerChoice.boxParentOne.SetOwner(boxOwner);
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner(boxOwner);

				
				//Determine whose turn is next
				if (GameManagerTwoPlayer.Instance.isPlayerOneTurn)
				{
					GameManagerTwoPlayer.Instance.isPlayerOneTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
					GameManagerTwoPlayer.Instance.isPlayerTwoTurn = !GameManagerTwoPlayer.Instance.isPlayerOneTurn;
				}
				else if (GameManagerTwoPlayer.Instance.isPlayerTwoTurn)
				{
					GameManagerTwoPlayer.Instance.isPlayerTwoTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
					GameManagerTwoPlayer.Instance.isPlayerOneTurn = !GameManagerTwoPlayer.Instance.isPlayerTwoTurn;
				}
			}
		}		
	}
}
