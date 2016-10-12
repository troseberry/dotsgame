using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroBoardManager : MonoBehaviour 
{
	private Transform _Dynamic;

	public Transform possiblePlayerLines;
	public Transform possibleComputerLines;
	public Transform possiblePlayerChips;
	public Transform possibleComputerChips;
	//public GameObject possiblePowerUps;

	private GameObject[] boxes;

	private int boardSize;
	private List<int> usedLines;
	//private List<Line> openLines;

	void Start () 
    {
		_Dynamic = GameObject.Find("_Dynamic").transform;
		boxes = GameObject.FindGameObjectsWithTag("Box");

		boardSize = ((int) Mathf.Sqrt(boxes.Length)) + 1;
		usedLines = new List<int>();
		//openLines = new List<Line>();
	}
	
	void Update () 
    {
	
	}

	public void RandomizeBoardLayout ()
	{
		ClearBoard();

		//Currently, every time the board resets, it should be the player's turn regardless of if they won or loss
		CampaignGameManager.Instance.isPlayerTurn = true;
		
		Line lineToStatic = null;
		//openLines = CampaignGameManager.Instance.lineObjects;

		for (int i = 0; i < boardSize; i++)
		{
			int randomLine = Random.Range(0, CampaignGameManager.Instance.lineObjects.Count);

			// while (usedLines.Contains(randomLine))
			// {
			// 	randomLine = Random.Range(0, CampaignGameManager.Instance.lineObjects.Count);
			// }

			while (!CampaignGameManager.Instance.lineObjects[randomLine].GetOpen())
			{
				randomLine = Random.Range(0, CampaignGameManager.Instance.lineObjects.Count); 
			}
			
			lineToStatic = CampaignGameManager.Instance.lineObjects[randomLine];
			Debug.Log(lineToStatic.transform.parent.name);

			lineToStatic.transform.parent.GetComponent<SpriteRenderer>().enabled = true;
			lineToStatic.SetLineStatic(true);
			lineToStatic.SetOpen(false);	

			//openLines.Remove(lineToStatic);

			
			Debug.Log(i);
		}
	}

	void ClearBoard ()
	{
		usedLines.Clear();
		

		foreach (Line line in CampaignGameManager.Instance.lineObjects)
		{
			line.ResetLine();
		}

		foreach (GameObject box in boxes)
		{
			box.GetComponent<Box>().ResetBox();
		}

		//Debug.Log("Dynamic Object Count: " + _Dynamic.childCount);
		//since editing _Dynamic children and iterating thru it at the same time, must start from end
		for (int i = _Dynamic.childCount-1; i > -1 ; i--)
		{
			Transform obj = _Dynamic.GetChild(i);
			switch(obj.name)
			{
				case "ComputerLine":
					//Debug.Log("Found Computer Line");
					obj.SetParent(possibleComputerLines, false);
					break;
				case "PlayerLine":
					obj.SetParent(possiblePlayerLines, false);
					break;
				case "PlayerChip":
					obj.SetParent(possiblePlayerChips, false);
					break;
				case "ComputerChip":
					obj.SetParent(possibleComputerChips, false);
					break;
			}	
			obj.position = Vector3.zero;
			obj.gameObject.SetActive(false);
			//Debug.Log("Completly Reset Object: " + obj.name);	
		}
		//openLines = CampaignGameManager.Instance.lineObjects;
	}
}
