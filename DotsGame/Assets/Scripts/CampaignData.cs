using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignData : MonoBehaviour
{
	private static bool finishedTutorial;
	private static string lastScene;
	
	//private static bool boardOneLevelOne;

	
	//private static Dictionary<string, bool> boardOneLevels = new Dictionary<string, bool>();
	private static Dictionary<string, bool> allBoardLevels = new Dictionary<string, bool>();


	//private List<string> boardOneLevelNames = new List<string>();
	private static List<string> allBoardLevelNames = new List<string>();
	

	void Start ()
	{
		//can also iterate through gameObjects tagged with LevelButton - this currently might only work for first group of board buttons

		/*GameObject parent = levelsGroup;
		while (parent.getChild[0].gameObject.tag != "LevelButton")
		{
			parent = parent.getChild[0].gameobject;
		}*/

		/*foreach (Transform child in parent)
		{
			if (child.gameObject.tag == "LevelButton")
			{
				add to dictionary
			}
		}*/


		allBoardLevelNames.Add("1-1");
		allBoardLevelNames.Add("1-2");
		allBoardLevelNames.Add("1-3");
		allBoardLevelNames.Add("1-4");
		allBoardLevelNames.Add("1-5");
		allBoardLevelNames.Add("1-6");
		allBoardLevelNames.Add("1-7");
		allBoardLevelNames.Add("1-8");
		allBoardLevelNames.Add("1-9");


		allBoardLevelNames.Add("2-1");
		allBoardLevelNames.Add("2-2");
		allBoardLevelNames.Add("2-3");
		allBoardLevelNames.Add("2-4");
		allBoardLevelNames.Add("2-5");
		allBoardLevelNames.Add("2-6");
		allBoardLevelNames.Add("2-7");
		allBoardLevelNames.Add("2-8");
		allBoardLevelNames.Add("2-9");
		allBoardLevelNames.Add("2-10");
		allBoardLevelNames.Add("2-11");
		allBoardLevelNames.Add("2-12");


		allBoardLevelNames.Add("3-1");
		allBoardLevelNames.Add("3-2");
		allBoardLevelNames.Add("3-3");
		allBoardLevelNames.Add("3-4");
		allBoardLevelNames.Add("3-5");
		allBoardLevelNames.Add("3-6");
		allBoardLevelNames.Add("3-7");
		allBoardLevelNames.Add("3-8");
		allBoardLevelNames.Add("3-9");
		allBoardLevelNames.Add("3-10");
		allBoardLevelNames.Add("3-11");
		allBoardLevelNames.Add("3-12");

		//Debug.Log("not attached");

		//check if the save data has all dictionary entries
		/*if (!SaveLoad.DoesSaveExist())
		{
			boardOneLevels.Add("1-1", false);
			boardOneLevels.Add("1-2", false);
			boardOneLevels.Add("1-3", false);
			boardOneLevels.Add("1-4", false);
			boardOneLevels.Add("1-5", false);
			boardOneLevels.Add("1-6", false);
			boardOneLevels.Add("1-7", false);
			boardOneLevels.Add("1-8", false);
			boardOneLevels.Add("1-9", false);
		}
		else
		{
			//load dictionary?
			SaveLoad.Load();
		}*/

		SaveLoad.Load();

		//If for some reason the loaded dictionary doesn't have the level, add it and make its completion status false
		foreach(string lvlName in allBoardLevelNames)
		{
			if (!allBoardLevels.ContainsKey(lvlName))
			{
				allBoardLevels.Add(lvlName, false);
			}
		}
		

		//Debug.Log(boardOneLevels);
		/*foreach (KeyValuePair<string, bool> pair in allBoardLevels)
		{
		    Debug.Log(pair.Key + pair.Value);
		}*/



		//SaveLoad.Save();
	}



	public static void ClearLevelsDictionary ()
	{
		/*allBoardLevels["1-1"] = false;
		allBoardLevels["1-2"] = false;
		allBoardLevels["1-3"] = false;
		allBoardLevels["1-4"] = false;
		allBoardLevels["1-5"] = false;
		allBoardLevels["1-6"] = false;
		allBoardLevels["1-7"] = false;
		allBoardLevels["1-8"] = false;
		allBoardLevels["1-9"] = false;*/


		//Can't iterate over dictionary and change values at same time
		/*foreach (KeyValuePair<string, bool> pair in allBoardLevels)
		{
			SetLevelStatus(pair.Key, false);
		}*/

		//Iterate thru names list. Should contain the same strings as dictionary keys
		foreach (string levelName in allBoardLevelNames)
		{
			//Setting allBoardLevels values no allBoardLeveNames
			SetLevelStatus(levelName, false);
		}
	}



	public static bool GetFinishedTutorial ()
	{
		return finishedTutorial;
	}

	public static void SetFinishedTutorial (bool status)
	{
		finishedTutorial = status;
	}

	public static bool GetLevelStatus (string levelName)
	{
		/*if (levelName == "boardOneLevelOne") 
		{
			return boardOneLevelOne;
		}
		else
		{
			return false;
		}*/

		return allBoardLevels[levelName];
	}

	public static void SetLevelStatus (string levelName, bool status)
	{
		/*if (levelName == "boardOneLevelOne")
		{
			boardOneLevelOne = status;
		}*/

		//Debug.Log("Input Level Name: " + levelName);

		allBoardLevels[levelName] = status;
	}

	public static Dictionary<string, bool> GetAllLevelsDictionary ()
	{
		return allBoardLevels;
	}

	public static void SetAllLevelsDictionary (Dictionary<string, bool> toSet)
	{
		allBoardLevels = toSet;
	}


	public static void SetLastScene (string name)
	{
		lastScene = name;
	}

	public static string GetLastScene ()
	{
		return lastScene;
	}
}
