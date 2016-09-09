using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Line : MonoBehaviour 
{
	private int lineNumber;
	private int farRightDigit;
	private int bottomTensNumber;
	private int lastDigit;

	public bool isOpen;
	public Vector3 linePosition;
	public Quaternion lineRotation;
	public Box boxParentOne;
	public Box boxParentTwo;
	public string owner;

	public bool isStatic;
	private bool calledAdd;
	
	void Start () 
	{

		isOpen = isStatic ? false : true;
		calledAdd = false;
		//linePosition = new Vector3(transform.localPosition.x / 100, transform.localPosition.y / 100, transform.localPosition.z / 100);
		linePosition = transform.position;// new Vector3(transform.localPosition.x / 100, transform.localPosition.y / 100, transform.localPosition.z / 100);
		lineRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

		owner = "";


		if(SceneManager.GetActiveScene().name.Contains("3x3"))
		{
			farRightDigit = 2;
			bottomTensNumber = 50;
		}
		else if(SceneManager.GetActiveScene().name.Contains("4x4"))
		{
			farRightDigit = 3;
			bottomTensNumber = 70;
		}
		else if(SceneManager.GetActiveScene().name.Contains("5x5"))
		{
			farRightDigit = 4;
			bottomTensNumber = 90;
		}
		

		
		string lineParent = transform.parent.transform.parent.name;
		//Debug.Log(lineParent.Substring(5, 2));

		lineNumber = int.Parse(lineParent.Substring(5, 2));
		lastDigit = int.Parse(lineNumber.ToString().Substring(1, 1));

		//Always the first row
		if (lineNumber > 9 && lineNumber < 20)
		{
			boxParentOne = GameObject.Find("Box_" + lineNumber).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + lineNumber).GetComponent<Box>();
		}
		//All left edge lines
		else if (lineNumber == 20 || lineNumber == 40 || lineNumber == 60 || lineNumber == 80)
		{
			boxParentOne = GameObject.Find("Box_" + (lineNumber/2)).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + (lineNumber/2)).GetComponent<Box>();
		}

		//check for right edge lines
		else if ((lineNumber == (20 + farRightDigit)) || (lineNumber == (40 + farRightDigit)) || (lineNumber == (60 + farRightDigit)) || (lineNumber == (80 + farRightDigit)))
		{
			int tensDigit = ((int) Mathf.Ceil(lineNumber/10) * 10) / 2;

			/*Debug.Log("Line Number: " + lineNumber);
			Debug.Log("Far Right Digit: " + farRightDigit);
			Debug.Log("Last Digit: " + lastDigit);
			Debug.Log(tensDigit + lastDigit);*/
			
			boxParentOne = GameObject.Find("Box_" + (tensDigit + lastDigit - 1)).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + (tensDigit + lastDigit - 1)).GetComponent<Box>();
		}
		//check  remaining vertical lines
		else if ((lineNumber > 19 && lineNumber < 30) || (lineNumber > 39 && lineNumber < 50) || (lineNumber > 59 && lineNumber < 70) || (lineNumber > 79 && lineNumber < 90))
		{
			//For 2 digit numbers
			int tensDigit = ((int) Mathf.Floor(lineNumber/10) * 10) / 2;

			boxParentOne = GameObject.Find("Box_" + (tensDigit + (lastDigit - 1))).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + (tensDigit + lastDigit)).GetComponent<Box>();
		} 
		//remaining horizontal lines
		else if (lineNumber >= bottomTensNumber && lineNumber < (bottomTensNumber + 10))
		{
			float tensNumber = (Mathf.Floor(lineNumber/10) * 10) / 2;

			int boxNumber = (int) (Mathf.Floor(tensNumber / 10) * 10) + lastDigit;			

			boxParentOne = GameObject.Find("Box_" + boxNumber).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + boxNumber).GetComponent<Box>();
		}
		else
		{
			//line number tens /2 ceil and line number tens/2 floor both plus last digit
			float tensNumber = (Mathf.Ceil(lineNumber/10) * 10) / 2;

			int firstBoxNumber = (int) (Mathf.Ceil(tensNumber / 10) * 10) + lastDigit;
			int secondBoxNumber = (int) (Mathf.Floor(tensNumber/10) * 10) + lastDigit;

			//Debug.Log("Line Number: " + lineNumber);
			//Debug.Log("Last Digit: " + lastDigit);
			//Debug.Log(firstBoxNumber);
			//Debug.Log(secondBoxNumber);

			boxParentOne = GameObject.Find("Box_" + firstBoxNumber).GetComponent<Box>();
			boxParentTwo = GameObject.Find("Box_" + secondBoxNumber).GetComponent<Box>();
		}
	}
	
	
	void Update () 
	{
		if(!calledAdd)
		{
			boxParentOne.AddLineToBox(this);
			boxParentTwo.AddLineToBox(this);
			calledAdd = true;
		}
	}

	/*public bool IsSpotOpen ()
	{
		return isOpen;
	}*/
}
