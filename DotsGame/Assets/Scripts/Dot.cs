using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dot : MonoBehaviour 
{
	enum DotType {SingleNotch, DoubleNotch90, DoubleNotch180, TripleNotch, QuadNotch};

	private GameObject attachedDot;
	private DotType type;
	private int totalNotches;
	private int currentNotchCount;
	
	void Start () 
	{
		//attachedDot = gameObject;
		//string dotTextureName = GetComponent<SpriteRenderer>().sprite.name;
		currentNotchCount = 0;

		/*switch(dotTextureName)
		{
			case "Dot_SingleNotch":
				type = DotType.SingleNotch;
				totalNotches = 1;
				break;
			case "Dot_DoubleNotch90":
				type = DotType.DoubleNotch90;
				totalNotches = 2;
				break;
			case "Dot_DoubleNotch180":
				type = DotType.DoubleNotch180;
				totalNotches = 2;
				break;
			case "Dot_TripleNotch":
				type = DotType.TripleNotch;
				totalNotches = 3;
				break;
			case "Dot_QuadeNotch":
				type = DotType.QuadNotch;
				totalNotches = 4;
				break;
		}*/
	}
	
	
	void Update () 
	{
		//DebugPanel.Log(name + " Current Notch Count: ", currentNotchCount);
	}

	void OnTriggerEnter2D (Collider2D line)
	{
		
		
		if (line.gameObject.tag == "Line")
		{
			//Debug.Log("Line Entered");
			currentNotchCount++;
		}
	}

	void OnTriggerExit2D (Collider2D line)
	{
		//This won't fire if object is destroyed. Transform +1x and +1z will trigger exit. Then destroy
		if (line.gameObject.tag == "Line")
		{
			//Debug.Log("Line Exited");
			currentNotchCount--;
		}
	}
}
