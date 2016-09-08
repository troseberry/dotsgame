using UnityEngine;
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

	public Vector3 chipPlacement;
	private GameObject playerChip;
	private GameObject computerChip;
	private GameObject playerOneChip;
	private GameObject playerTwoChip;

	private Vector3 ownerChipScale;
	
	void Start () 
	{
		boxNumber = int.Parse(name.Substring(4, 2));
		sideCount = 0;
		owner = "";
		claimed = false;

		//Don't think these are being used
		upperLeft = GameObject.Find("Dot_" + boxNumber).GetComponent<Dot>();
		upperRight = GameObject.Find("Dot_" + (boxNumber + 1)).GetComponent<Dot>();
		lowerLeft = GameObject.Find("Dot_" + (boxNumber + 10)).GetComponent<Dot>();
		lowerRight = GameObject.Find("Dot_" + (boxNumber + 11)).GetComponent<Dot>();


		playerChip = (GameObject) Resources.Load("PlayerChip");
		computerChip = (GameObject) Resources.Load("ComputerChip");
		playerOneChip = (GameObject) Resources.Load("PlayerOneChip");
		playerTwoChip = (GameObject) Resources.Load("PlayerTwoChip");

		//chipPlacement = transform.position;
		ownerChipScale = GameObject.Find("BoxGroup").transform.localScale;
	}
	
	
	void Update () 
	{
		DebugPanel.Log(name + " Side Count: ", sideCount);

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
		if(!boxLineObjects.Contains(toAdd)) boxLineObjects.Add(toAdd);
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
		if (owner == "Player")
		{
			GameManager.UpdatePlayerPoints(1);
			GameObject chip = (GameObject) Instantiate(playerChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "Computer")
		{

			GameObject chip = (GameObject) Instantiate(computerChip, transform.position, computerChip.transform.rotation);
			chip.name = "ComputerChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "PlayerOne")
		{
			GameManagerTwoPlayer.UpdatePlayerPoints("One", 1);
			GameObject chip = (GameObject) Instantiate(playerOneChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerOneChip";
			chip.transform.localScale = ownerChipScale;
		}
		else if (owner == "PlayerTwo")
		{
			GameManagerTwoPlayer.UpdatePlayerPoints("Two", 1);
			GameObject chip = (GameObject) Instantiate(playerTwoChip, transform.position, playerChip.transform.rotation);
			chip.name = "PlayerTwoChip";
			chip.transform.localScale = ownerChipScale;
		}
		claimed = true;
	}
}
