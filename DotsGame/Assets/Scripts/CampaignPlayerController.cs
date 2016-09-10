using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CampaignPlayerController : MonoBehaviour 
{
	private GameObject playerLine;
	private Vector3 lineGridScale;

	private GameObject placedLineGroup;

	private bool canDraw;
	private GameObject lineToDraw;
	private float drawingTime;
	private float drawDuration = 2.0f;

	private Vector3 endDrawPosition;

	private static string currentPowerUp;

	private static GameObject bombButton;
	private bool canUseBomb;

	private static GameObject thiefTokenButton;
	private bool canUseThiefToken;

	
	void Start () 
	{
		playerLine = (GameObject) Resources.Load("PlayerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;

		if (!GameObject.Find("PlacedLineGroup"))
		{
			placedLineGroup = new GameObject();
			placedLineGroup.name = "PlacedLineGroup";
		}
		else
		{
			placedLineGroup = GameObject.Find("PlacedLineGroup");
		}

		currentPowerUp = "";

		canUseBomb = false;
		bombButton = GameObject.Find("BombButton");
		bombButton.SetActive(false);

		canUseThiefToken = false;
		thiefTokenButton = GameObject.Find("ThiefTokenButton");
		thiefTokenButton.SetActive(false);
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

		if(!CampaignGameManager.isPlayerTurn) 
		{
			canUseBomb = false;
			canUseThiefToken = false;
		}

	}

	public void PlayerDrawLine () 
	{
		if(CampaignGameManager.isPlayerTurn && !CampaignGameManager.RoundOver())
		{
			Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
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


				GameObject newLine = (GameObject) Instantiate(playerLine, startPosition, playerChoice.lineRotation);
				newLine.name = "PlayerLine";
				newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
				newLine.transform.SetParent(placedLineGroup.transform, false);
				lineToDraw = newLine;
				canDraw = true;

				
				//Update side counts and dole out points if need be
				
				playerChoice.owner = "Player";

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);

				
				if (playerChoice.boxParentOne.IsComplete()) 
				{
					playerChoice.boxParentOne.SetOwner("CampaignPlayer");
					//if box had a power up determine the type:
					// 1) Immediate use, passive (player doesn't need to do anything).
					// 2) Immediate use, active (player will automatically use on their next line place).
					// 3) Delayed Use, can be used on any turn.
				}
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("CampaignPlayer");

				
				//Determine whose turn is next
				CampaignGameManager.isPlayerTurn = (playerChoice.boxParentOne.IsComplete() || playerChoice.boxParentTwo.IsComplete()) ? true : false;
				playerChoice.isOpen = false;		
			}
		}		
	}


	public static void SetCurrentPowerUp (GameObject powerUp)
	{
		currentPowerUp = powerUp.name;
	}
	//BEGINNING OF POWERUP METHODS (x2 powerup is in Box.cs)

	public static void PickedUpBomb ()
	{
		bombButton.SetActive(true);
	}

	public void ToggleBomb ()			//attach to powerup button
	{
		if (CampaignGameManager.isPlayerTurn) canUseBomb = !canUseBomb;
	}

	//Bomb PowerUp
	public void DestroyStaticLine ()			//attach to static line buttons
	{
		if(canUseBomb && !CampaignGameManager.RoundOver())
		{
			Transform buttonLocation = EventSystem.current.currentSelectedGameObject.transform;
			Line playerChoice = EventSystem.current.currentSelectedGameObject.GetComponent<Line>();

			if (playerChoice.isStatic)
			{
				//Debug.Log("Destroy Line");
				Color change = playerChoice.gameObject.transform.parent.transform.parent.GetComponent<SpriteRenderer>().color;
				change.a = 0f;
				playerChoice.gameObject.transform.parent.transform.parent.GetComponent<SpriteRenderer>().color = change;

				playerChoice.isStatic = false;
				playerChoice.isOpen = true;

				playerChoice.boxParentOne.UpdateSideCount(-1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(-1);

				PlayerDrawLine();
				CampaignGameManager.isPlayerTurn = true;		//Seems to work to not make next turn the player's unless they complete a NEW box
																//Bomb power up is being held, instead of consumed on immediate next place

				//currentPowerUp = "";
				canUseBomb = false;
				bombButton.SetActive(false);
			}
		}
	}

	public static void PickedUpThiefToken ()
	{
		thiefTokenButton.SetActive(true);
	}

	//Thief Token PowerUp	
	public void ToggleThiefToken ()			//attach to powerup button
	{
		if (CampaignGameManager.isPlayerTurn) canUseThiefToken = !canUseThiefToken;
	}
	public void UseThiefToken ()				//attach to boxObject. (give box objects button components)
	{
		Box chosenBox = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<Box>();

		if (canUseThiefToken && chosenBox.IsComplete())
		{
			chosenBox.ChangeOwnership();
			canUseThiefToken = false;
			//hide thief token button, or gray it out
			thiefTokenButton.SetActive(false);
		}
	}
}












