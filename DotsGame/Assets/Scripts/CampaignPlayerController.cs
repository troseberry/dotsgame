﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CampaignPlayerController : MonoBehaviour 
{
	public static CampaignPlayerController Instance;

	private GameObject playerLine;
	public GameObject possiblePlayerLines;

	private Vector3 lineGridScale;

	private GameObject _Dynamic;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;

	//private static string currentPowerUp;

	private GameObject bombButton;
	private bool canUseBomb;

	private GameObject thiefTokenButton;
	private bool canUseThiefToken;

	
	void Start () 
	{
		Instance = this;

		//playerLine = (GameObject) Resources.Load("PlayerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;

		_Dynamic = GameObject.Find("_Dynamic");

		//currentPowerUp = "";

		canUseBomb = false;
		bombButton = GameObject.Find("BombButton");
		bombButton.SetActive(false);

		canUseThiefToken = false;
		thiefTokenButton = GameObject.Find("ThiefTokenButton");
		thiefTokenButton.SetActive(false);
	}
	
	
	void Update () 
	{
		//DebugPanel.Log("Drawing Time: ", drawingTime);
		if (canDraw)
		{
			if (drawingTime < 2.0f) drawingTime += Time.deltaTime/drawDuration;
			lineToDraw.transform.localScale = Vector3.Lerp(lineToDraw.transform.localScale, lineGridScale, drawingTime);
			lineToDraw.transform.position = Vector3.Lerp(lineToDraw.transform.position, endDrawPosition, drawingTime);
		
			
			if (lineToDraw.transform.localScale.x >= (0.9f * lineGridScale.x))
			{
				lineToDraw.transform.localScale = new Vector3(lineGridScale.x, lineToDraw.transform.localScale.y, lineToDraw.transform.localScale.z);
				lineToDraw.transform.position = endDrawPosition;
				drawingTime = 0f;
				canDraw = false;
				if (lineToDraw) lineToDraw = null;
			}
		}

		if(!CampaignGameManager.Instance.isPlayerTurn) 
		{
			canUseBomb = false;
			canUseThiefToken = false;
		}

	}

	public void PlayerDrawLine () 
	{
		if(CampaignGameManager.Instance.isPlayerTurn && !CampaignGameManager.Instance.RoundOver())
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

				
				//Update side counts and dole out points if need be
				
				playerChoice.owner = "Player";

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

				
				if (playerChoice.boxParentOne.IsComplete()) 
				{
					playerChoice.boxParentOne.SetOwner("CampaignPlayer");
				}
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("CampaignPlayer");

				playerChoice.SetOpen(false);

				//Determine whose turn is next
				CampaignGameManager.Instance.isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
						
			}
		}		
	}


	/*public static void SetCurrentPowerUp (GameObject powerUp)
	{
		currentPowerUp = powerUp.name;
	}*/
	//BEGINNING OF POWERUP METHODS (x2 powerup is in Box.cs)

	public void PickedUpBomb ()
	{
		bombButton.SetActive(true);
	}

	public void ToggleBomb ()			//attach to powerup button
	{
		if (CampaignGameManager.Instance.isPlayerTurn) canUseBomb = !canUseBomb;
	}

	//Bomb PowerUp
	public void DestroyStaticLine ()			//attach to static line buttons
	{
		if(canUseBomb && !CampaignGameManager.Instance.RoundOver())
		{
			//Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
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

				//PlayerDrawLine();
				//CampaignGameManager.isPlayerTurn = true;		//Seems to work to not make next turn the player's unless they complete a NEW box
																//Bomb power up is being held, instead of consumed on immediate next place

				//currentPowerUp = "";
				canUseBomb = false;
				bombButton.SetActive(false);

				//if either box parent was complete, list it as incomplete/unowned
				//if either belonged to the player, subtract the correct amount of points 
				//then when player draw a lines, it wll complete the box and give them the updated points
			}
		}
	}

	public void PickedUpThiefToken ()
	{
		thiefTokenButton.SetActive(true);
	}

	//Thief Token PowerUp	
	public void ToggleThiefToken ()			//attach to powerup button
	{
		if (CampaignGameManager.Instance.isPlayerTurn) canUseThiefToken = !canUseThiefToken;
	}
	public void UseThiefToken ()				//attach to boxObject. (give box objects button components)
	{
		Box chosenBox = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<Box>();

		if (canUseThiefToken && chosenBox.IsComplete())
		{
			//Debug.Log("Used thief token");
			chosenBox.ChangeOwnership();
			canUseThiefToken = false;
			//hide thief token button, or gray it out
			thiefTokenButton.SetActive(false);
		}
	}
}












