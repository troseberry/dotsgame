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

		//Don't think these are being used
		//upperLeft = GameObject.Find("Dot_" + boxNumber).GetComponent<Dot>();
		//upperRight = GameObject.Find("Dot_" + (boxNumber + 1)).GetComponent<Dot>();
		//lowerLeft = GameObject.Find("Dot_" + (boxNumber + 10)).GetComponent<Dot>();
		//lowerRight = GameObject.Find("Dot_" + (boxNumber + 11)).GetComponent<Dot>();


		playerChip = (GameObject) Resources.Load("PlayerChip");
		computerChip = (GameObject) Resources.Load("ComputerChip");
		playerOneChip = (GameObject) Resources.Load("PlayerOneChip");
		playerTwoChip = (GameObject) Resources.Load("PlayerTwoChip");

		_Dynamic = GameObject.Find("_Dynamic");

		//chipPlacement = transform.position;
		ownerChipScale = GameObject.Find("BoxGroup").transform.localScale;

		if (transform.childCount > 1/* && transform.GetChild(1).gameObject.activeSelf*/) //if there is a power up gameobject that is inactive, that can still trigger powerups
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
			if (toAdd.isStatic) UpdateSideCount(1);
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

	void AwardPoint()
	{
		if (heldPowerUp)
		{
			heldPowerUp.SetActive(false);
		}

		

		/*if (!GameObject.Find("ChipGroup"))
		{
			chipGroup = new GameObject();
			chipGroup.name = "ChipGroup";
		}*/

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
				else if (heldPowerUp.name == "PowerUp_Bomb")
				{
					if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
					{
						TutorialGameManager.PickedUpBomb();
					}
					else
					{	
						CampaignPlayerController.PickedUpBomb();
					}
				}
				else if (heldPowerUp.name == "PowerUp_ThiefToken")
				{
					CampaignPlayerController.PickedUpThiefToken();
				}
			}

			//Debug.Log("Points Awarded");
			if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
			{
				if (!SceneManager.GetActiveScene().name.Contains("01")) TutorialGameManager.UpdatePlayerPoints(pointsAwarded);
			}
			else
			{
				CampaignGameManager.Instance.UpdatePlayerPoints(pointsAwarded);
			}

			chip = (GameObject) Instantiate(playerChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerChip";
			chip.transform.localScale = ownerChipScale;

			//determine if box had a power up
			//if so, handle next step
		}
		else if (owner == "Player")
		{
			GameManager.Instance.UpdatePlayerPoints(1);
			chip = (GameObject) Instantiate(playerChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "Computer")
		{

			chip = (GameObject) Instantiate(computerChip, transform.position, computerChip.transform.rotation);
			chip.name = "ComputerChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "PlayerOne")
		{
			GameManagerTwoPlayer.Instance.UpdatePlayerPoints("One", 1);
			chip = (GameObject) Instantiate(playerOneChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerOneChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "PlayerTwo")
		{
			GameManagerTwoPlayer.Instance.UpdatePlayerPoints("Two", 1);
			chip = (GameObject) Instantiate(playerTwoChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerTwoChip";
			chip.transform.localScale = ownerChipScale;
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
		if (owner == "Computer")
		{
			//should check gameManagerObj, get mode from whichever GameManager, and make either CampaignPlayer or Player
			Destroy(chip);

			chip = (GameObject) Instantiate(playerChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerChip";
			chip.transform.localScale = ownerChipScale;

			owner = "CampaginPlayer";

			int pointsAwarded = 0;
			foreach (Line line in boxLineObjects)
			{
				if (line.owner == "Player") pointsAwarded++;

			}
			CampaignGameManager.Instance.UpdatePlayerPoints(pointsAwarded);
			//AwardPoint();
		}
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
