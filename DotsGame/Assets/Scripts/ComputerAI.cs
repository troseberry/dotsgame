using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerAI : MonoBehaviour 
{
	private GameObject computerLine;
	private Vector3 lineGridScale;

	private bool placing;

	private List<Box> boxObjects = new List<Box>();

	private static System.Random rng = new System.Random();
	
	void Start () 
	{
		computerLine = (GameObject) Resources.Load("ComputerLine");
		lineGridScale = GameObject.Find("LineGrid").transform.localScale;

		placing = false;

		GameObject[] boxHolder = GameObject.FindGameObjectsWithTag("Box");
		foreach (GameObject child in boxHolder)
		{
			boxObjects.Add(child.GetComponent<Box>());
		}
		
	}
	
	
	void Update () 
	{
		if (!GameManager.isPlayerTurn && !placing && !GameManager.RoundOver())
		{
			//Invoke("ComputerPlaceLine", 2.0f);
			StartCoroutine(ComputerPlaceLine( DetermineLineToPlace() ));
			placing = true;
		}
	}

	private Line DetermineLineToPlace ()
	{
		Line toPlace;
		List<Box> boxObjectsClone = boxObjects;
		Shuffle(boxObjectsClone);

		//Look for boxes with only 1 open side first
		foreach (Box box in boxObjectsClone)
		{
			if (box.SidesLeftOpen() == 1)
			{
				foreach (Line line in box.boxLineObjects)
				{
					if (line.isOpen) 
					{
						toPlace = line;
						return toPlace;
					}
				}
			}
		}

		//Look for boxes with 3 open sides
		foreach (Box box in boxObjectsClone)
		{
			if (box.SidesLeftOpen() >= 3)
			{
				//foreach open line
				foreach (Line line in box.boxLineObjects)
				{
					if (line.isOpen)
					{
						//look at that line's box parents. 
						Box parentOne = line.boxParentOne;
						Box parentTwo = line.boxParentTwo;

						//If both have 3 sides left open, then return that line
						if (parentOne.SidesLeftOpen() >= 3 && parentTwo.SidesLeftOpen() >= 3)
						{
							toPlace = line;
							return toPlace;
						}
					}
				}
			}
		}

		//if no boxes with 3 open sides, or boxes with only one open side, then place randomly
		//(should later come back and change this so that it places to limit the player's chain length)

		int randomPlace = UnityEngine.Random.Range(0, GameManager.lineObjects.Count);
		toPlace = GameManager.lineObjects.ElementAt(randomPlace);

		if (toPlace.isOpen)
		{
			return toPlace;
		}
		else
		{
			return DetermineLineToPlace();
		}
	}


	IEnumerator ComputerPlaceLine(Line toPlace)
	{
		yield return new WaitForSeconds(2.0f);

		GameObject computerChoice = (GameObject) Instantiate(computerLine, toPlace.linePosition, toPlace.lineRotation);
		computerChoice.transform.localScale = lineGridScale;
		computerChoice.name = "ComputerLine";
		toPlace.isOpen = false;

		toPlace.boxParentOne.UpdateSideCount(1);
		if (toPlace.boxParentOne != toPlace.boxParentTwo) toPlace.boxParentTwo.UpdateSideCount(1);

		if (toPlace.boxParentOne.IsComplete()) toPlace.boxParentOne.SetOwner("Computer");
		if (toPlace.boxParentTwo.IsComplete()) toPlace.boxParentTwo.SetOwner("Computer");

		//Debug.Log("Computer Placed Line");
		GameManager.isPlayerTurn = (toPlace.boxParentOne.IsComplete() || toPlace.boxParentTwo.IsComplete()) ? false : true;
		placing = false;
	}


	/*void ComputerPlaceLine()
	{
		int randomPlace = Random.Range(0, GameManager.lineObjects.Count);
		Line toPlace = GameManager.lineObjects.ElementAt(randomPlace);

		if(toPlace.isOpen)
		{
			GameObject computerChoice = (GameObject) Instantiate(computerLine, toPlace.linePosition, toPlace.lineRotation);
			computerChoice.name = "ComputerLine";
			toPlace.isOpen = false;

			toPlace.boxParentOne.UpdateSideCount(1);
			if(toPlace.boxParentOne != toPlace.boxParentTwo) toPlace.boxParentTwo.UpdateSideCount(1);

			//Debug.Log("Computer Placed Line");
			GameManager.isPlayerTurn = (toPlace.boxParentOne.IsComplete() || toPlace.boxParentTwo.IsComplete()) ? false : true;
			placing = false;
		}
		else
		{
			ComputerPlaceLine();
		}
	}*/

	public static void Shuffle (List<Box> list)
	{
		int n = list.Count;

		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Box value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
