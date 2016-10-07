using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class Box : MonoBehaviour 
{
	private int boxNumber;

	private int sideCount;

	private Dot upperLeft;
	private Dot upperRight;
	private Dot lowerLeft;
	private Dot lowerRight;

	public List<Line> boxLineObjects = new List<Line>();
	//private List<GameObject> linePositions = new List<GameObject>();

	private string owner;
	private bool claimed;

	//private GameObject chipGroup;

	private GameObject chip;

	public Vector3 chipPlacement;
	private GameObject playerChip;
	private GameObject computerChip;
	private GameObject playerOneChip;
	private GameObject playerTwoChip;

	private GameObject _Dynamic;

	private Vector3 ownerChipScale;

	private GameObject heldPowerUp;
	
	void Start () 
	{
		boxNumber = int.Parse(name.Substring(4, 2));
		sideCount = 0;
		owner = "";
		claimed = false;
		chip = null;

		_Dynamic = GameObject.Find("_Dynamic");

		ownerChipScale = GameObject.Find("BoxGroup").transform.localScale;

		if (transform.childCount > 1 && transform.GetChild(1).gameObject.activeSelf)
		{
			heldPowerUp = transform.GetChild(1).gameObject;
		}
		else
		{
			heldPowerUp = null;
		}
	}
		
	void Update () 
	{
		//DebugPanel.Log(name + " Sides Left Open: ", SidesLeftOpen());

		//DebugPanel.Log(name + " Is Claimed: ", claimed);
		//DebugPanel.Log(name + " Owner: ", owner);
		//DebugPanel.Log(name + " Is Complete: ", IsComplete());

		if (IsComplete() && owner != "" && !claimed)
		{
			AwardPoint();
		}
	}

	public string GetOwner ()
	{
		return owner;
	}

	public void SetOwner (string newOwner)
	{
		owner = newOwner;
	}


	public bool IsComplete ()
	{
		return (sideCount >= 4);
	}

	public void UpdateSideCount (int amount)
	{
		sideCount += amount;
	}

	public void AddLineToBox (Line toAdd)
	{
		if(!boxLineObjects.Contains(toAdd)) 
		{
			boxLineObjects.Add(toAdd);
			if (toAdd.GetLineStatic()) UpdateSideCount(1);
		}

	}

	/*public void AddLinePositionToBox (GameObject toAdd)
	{
		if(!linePositions.Contains(toAdd)) linePositions.Add(toAdd);
	}*/

	public int SidesLeftOpen ()
	{
		return (sideCount >= 4 ? 0 : 4 - sideCount);
	}

	public void SetPowerUp (GameObject powerUp)
	{
		heldPowerUp = powerUp;
	}


	void AwardPoint()
	{
		if (heldPowerUp)
		{
			heldPowerUp.SetActive(false);
		}

		

		if (owner == "CampaignPlayer")
		{
			int pointsAwarded = 0;
			foreach (Line line in boxLineObjects)
			{
				if (line.owner == "Player") pointsAwarded++;

			}

			if (heldPowerUp)
			{
				if(heldPowerUp.name == "PowerUp_x2") 
				{
					pointsAwarded += pointsAwarded;
				}
				else if (heldPowerUp.name == "PowerUp_Minus")
				{
					pointsAwarded = -pointsAwarded;
				}
				else if (heldPowerUp.name == "PowerUp_Bomb")
				{
					if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
					{
						TutorialGameManager.Instance.PickedUpBomb();
					}
					else
					{	
						CampaignPlayerController.Instance.PickedUpBomb();
					}
				}
				else if (heldPowerUp.name == "PowerUp_ThiefToken")
				{
					CampaignPlayerController.Instance.PickedUpThiefToken();
				}
			}

			//Debug.Log("Points Awarded");
			if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
			{
				if (!SceneManager.GetActiveScene().name.Contains("01")) TutorialGameManager.Instance.UpdatePlayerPoints(pointsAwarded);

				chip = TutorialGameManager.Instance.possiblePlayerChips.transform.GetChild(0).gameObject;
				chip.transform.position = transform.position;
				chip.transform.localScale = ownerChipScale;
				chip.SetActive(true);
			}
			else
			{
				CampaignGameManager.Instance.UpdatePlayerPoints(pointsAwarded);

				chip = CampaignGameManager.Instance.possiblePlayerChips.transform.GetChild(0).gameObject;
				chip.transform.position = transform.position;
				chip.transform.localScale = ownerChipScale;
				chip.SetActive(true);
			}

			//chip = (GameObject) Instantiate(playerChip, transform.position, playerChip.transform.rotation);
			//chip.name = "PlayerChip";

			

			//determine if box had a power up
			//if so, handle next step
		}
		//Can get rid of distinc campaign computer block when all scenes no longer instantiate. Will still need to get specific gameManager type
		else if (owner == "CampaignComputer")
		{
			chip = CampaignGameManager.Instance.possibleComputerChips.transform.GetChild(0).gameObject;
			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);
		}

		else if (owner == "Player")
		{
			GameManager.Instance.UpdatePlayerPoints(1);
			
			chip = GameManager.Instance.possiblePlayerChips.transform.GetChild(0).gameObject;
			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);
		}
		else if (owner == "Computer")
		{
			if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
			{
				chip = TutorialGameManager.Instance.possibleComputerChips.transform.GetChild(0).gameObject;
			}
			else
			{
				chip = GameManager.Instance.possibleComputerChips.transform.GetChild(0).gameObject;
			}

			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);
		}
		else if (owner == "PlayerOne")
		{
			GameManagerTwoPlayer.Instance.UpdatePlayerPoints("One", 1);

			chip = GameManagerTwoPlayer.Instance.possiblePlayerOneChips.transform.GetChild(0).gameObject;
			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);
		}
		else if (owner == "PlayerTwo")
		{
			GameManagerTwoPlayer.Instance.UpdatePlayerPoints("Two", 1);
			
			chip = GameManagerTwoPlayer.Instance.possiblePlayerTwoChips.transform.GetChild(0).gameObject;
			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);
		}
		else 
		{
			chip = new GameObject();
		}
		chip.transform.SetParent(_Dynamic.transform, false);	
		claimed = true;
	}

	public void ChangeOwnership ()
	{
		Debug.Log("Previous Owner: " + owner);
		//get mode from whichever GameManager, and make check either CampaignComputer or Computer
		if (owner == "CampaignComputer")
		{
			//should check gameManagerObj, get mode from whichever GameManager, and make either CampaignPlayer or Player
			Destroy(chip);

			chip = CampaignGameManager.Instance.possiblePlayerChips.transform.GetChild(0).gameObject;
			chip.transform.position = transform.position;
			chip.transform.localScale = ownerChipScale;
			chip.SetActive(true);

			chip.transform.SetParent(_Dynamic.transform, false);

			owner = "CampaginPlayer";

			int pointsAwarded = 0;
			foreach (Line line in boxLineObjects)
			{
				if (line.owner == "Player") pointsAwarded++;

			}
			CampaignGameManager.Instance.UpdatePlayerPoints(pointsAwarded);
			//AwardPoint();
		}
		//If I want the ability for the computer to steal boxes from the player
		else if (owner == "CampaginPlayer" || owner == "Player")
		{
			int pointsSubtracted = 0;
			owner = "Computer";
			foreach (Line line in boxLineObjects)
			{
				if (line.owner == "Player") pointsSubtracted--;
			}
			CampaignGameManager.Instance.UpdatePlayerPoints(pointsSubtracted);
		}
	}

	public GameObject GetPowerUp ()
	{
		return heldPowerUp;
	}
}
