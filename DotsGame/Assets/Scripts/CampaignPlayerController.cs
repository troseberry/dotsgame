using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
	private float drawDuration;// = 2.0f;		//if hero board, make this shorter

	private Vector3 endDrawPosition;

	//private static string currentPowerUp;

	//private GameObject bombButton;
	private bool canUseBomb;
	private Toggle bombToggle;

	//private GameObject thiefTokenButton;
	private bool canUseThiefToken;
	private Toggle thiefTokenToggle;

	private ColorBlock holderColorBlock = ColorBlock.defaultColorBlock;
	private Color32 redBombColor = new Color32 (0xFD, 0x7C, 0x7C, 0xFF);
	private Color32 yellowThiefColor = new Color32 (0xED, 0xFF, 0x7D, 0xFF);
	private Color resetColor = new Color (1, 1, 1);

	private string mode;

	
	void Start () 
	{
		Instance = this;

		mode = (SceneManager.GetActiveScene().name.Contains("HeroBoard")) ? "hero" : string.Empty;

		//playerLine = (GameObject) Resources.Load("PlayerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;
		canDraw = false;
		drawingTime = 0f;

		_Dynamic = GameObject.Find("_Dynamic");

		//currentPowerUp = "";

		if (mode != "hero")
		{
			canUseBomb = false;
			bombToggle = GameObject.Find("BombToggle").GetComponent<Toggle>();
			bombToggle.gameObject.SetActive(false);
			bombToggle.onValueChanged.AddListener((isOn) => ToggleBomb() );


			canUseThiefToken = false;
			thiefTokenToggle = GameObject.Find("ThiefTokenToggle").GetComponent<Toggle>();
			thiefTokenToggle.gameObject.SetActive(false);
			thiefTokenToggle.onValueChanged.AddListener((isOn) => ToggleThiefToken() );

			drawDuration = 2.0f;
		}
		else
		{
			drawDuration = 0.5f;
		}
	}
	
	
	void Update () 
	{
		//DebugPanel.Log("Drawing Time: ", drawingTime);
		if (canDraw)
		{
			if (drawingTime < drawDuration) drawingTime += Time.deltaTime/drawDuration;
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
				//Update side counts and dole out points if need be
				playerChoice.owner = "Player";
				playerChoice.SetOpen(false);

				playerChoice.boxParentOne.UpdateSideCount(1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(1);
				
				if (playerChoice.boxParentOne.IsComplete()) playerChoice.boxParentOne.SetOwner("CampaignPlayer");
				if (playerChoice.boxParentTwo.IsComplete()) playerChoice.boxParentTwo.SetOwner("CampaignPlayer");

				
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


				GameObject newLine = possiblePlayerLines.transform.GetChild(0).gameObject;

				newLine.transform.position = startPosition;
				newLine.transform.rotation = playerChoice.lineRotation;
				newLine.transform.localScale = new Vector3(0, lineGridScale.y, lineGridScale.z);
				newLine.transform.SetParent(_Dynamic.transform, false);

				newLine.SetActive(true);
				lineToDraw = newLine;
				canDraw = true;


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
		//bombButton.SetActive(true);
		bombToggle.gameObject.SetActive(true);
	}

	public void ToggleBomb ()			//attach to powerup button
	{
	
		if (bombToggle.isOn)
		{
			holderColorBlock.pressedColor = redBombColor;
			holderColorBlock.normalColor = redBombColor;
			holderColorBlock.highlightedColor = redBombColor;

			bombToggle.colors = holderColorBlock;

			if (CampaignGameManager.Instance.isPlayerTurn) canUseBomb = true;
			Debug.Log("Can Use Bomb: " + canUseBomb);
		}
		else
		{
			holderColorBlock.pressedColor = resetColor;
			holderColorBlock.normalColor = resetColor;
			holderColorBlock.highlightedColor = resetColor;

			bombToggle.colors = holderColorBlock;

			if (CampaignGameManager.Instance.isPlayerTurn) canUseBomb = false;
		}
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
				Color change = playerChoice.gameObject.transform.parent.GetComponent<SpriteRenderer>().color;
				change.a = 0f;
				playerChoice.gameObject.transform.parent.GetComponent<SpriteRenderer>().color = change;

				playerChoice.SetLineStatic(false);
				playerChoice.SetOpen(true);

				playerChoice.boxParentOne.UpdateSideCount(-1);
				if (playerChoice.boxParentOne != playerChoice.boxParentTwo) playerChoice.boxParentTwo.UpdateSideCount(-1);

				//PlayerDrawLine();
				//CampaignGameManager.isPlayerTurn = true;		//Seems to work to not make next turn the player's unless they complete a NEW box
																//Bomb power up is being held, instead of consumed on immediate next place

				//currentPowerUp = "";
				canUseBomb = false;
				//bombButton.SetActive(false);
				bombToggle.gameObject.SetActive(false);

				//Reset bomb colors 
				holderColorBlock.pressedColor = resetColor;
				holderColorBlock.normalColor = resetColor;
				holderColorBlock.highlightedColor = resetColor;

				bombToggle.colors = holderColorBlock;

				//if either box parent was complete, list it as incomplete/unowned
				//if either belonged to the player, subtract the correct amount of points 
				//then when player draw a lines, it wll complete the box and give them the updated points
			}
		}
	}

	public void PickedUpThiefToken ()
	{
		//thiefTokenButton.SetActive(true);
		thiefTokenToggle.gameObject.SetActive(true);
	}

	//Thief Token PowerUp	
	public void ToggleThiefToken ()			//attach to powerup button
	{
		if (thiefTokenToggle.isOn)
		{
			holderColorBlock.pressedColor = yellowThiefColor;
			holderColorBlock.normalColor = yellowThiefColor;
			holderColorBlock.highlightedColor = yellowThiefColor;

			thiefTokenToggle.colors = holderColorBlock;

			if (CampaignGameManager.Instance.isPlayerTurn) canUseThiefToken = true;
		}
		else
		{
			holderColorBlock.pressedColor = resetColor;
			holderColorBlock.normalColor = resetColor;
			holderColorBlock.highlightedColor = resetColor;

			thiefTokenToggle.colors = holderColorBlock;

			if (CampaignGameManager.Instance.isPlayerTurn) canUseThiefToken = false;
		}
	}
	public void UseThiefToken ()				//attach to boxObject. (give box objects button components)
	{
		//Box chosenBox = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<Box>();
		Box chosenBox = EventSystem.current.currentSelectedGameObject.GetComponent<Box>();

		if (canUseThiefToken && chosenBox.IsComplete())
		{
			Debug.Log("Used thief token");
			chosenBox.ChangeOwnership();
			canUseThiefToken = false;
			//hide thief token button, or gray it out
			//thiefTokenButton.SetActive(false);

			holderColorBlock.pressedColor = resetColor;
			holderColorBlock.normalColor = resetColor;
			holderColorBlock.highlightedColor = resetColor;

			thiefTokenToggle.colors = holderColorBlock;

			thiefTokenToggle.gameObject.SetActive(false);
		}
	}
}