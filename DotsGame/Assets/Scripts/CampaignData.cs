using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignData : MonoBehaviour
{
	private static bool finishedTutorial;
	private static string lastScene;
	
	private static bool boardOneLevelOne;

	
	private static Dictionary<string, bool> boardOneLevels = new Dictionary<string, bool>();

	private List<string> boardOneLevelNames = new List<string>();
	

	void Start ()
	{
		boardOneLevelNames.Add("1-1");
		boardOneLevelNames.Add("1-2");
		boardOneLevelNames.Add("1-3");
		boardOneLevelNames.Add("1-4");
		boardOneLevelNames.Add("1-5");
		boardOneLevelNames.Add("1-6");
		boardOneLevelNames.Add("1-7");
		boardOneLevelNames.Add("1-8");
		boardOneLevelNames.Add("1-9");
		boardOneLevelNames.Add("1-10");
		boardOneLevelNames.Add("1-11");
		boardOneLevelNames.Add("1-12");


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
			boardOneLevels.Add("1-10", false);
			boardOneLevels.Add("1-11", false);
			boardOneLevels.Add("1-12", false);
		}
		else
		{
			//load dictionary?
			SaveLoad.Load();
		}*/

		SaveLoad.Load();

		foreach(string lvlName in boardOneLevelNames)
		{
			if (!boardOneLevels.ContainsKey(lvlName))
			{
				boardOneLevels.Add(lvlName, false);
			}
		}
		

		//Debug.Log(boardOneLevels);
		foreach (KeyValuePair<string, bool> pair in boardOneLevels)
		{
		    //Debug.Log(pair.Key + pair.Value);
		}



		//SaveLoad.Save();
	}



	public static void ClearBoardOneDictionary ()
	{
		boardOneLevels["1-1"] = false;
		boardOneLevels["1-2"] = false;
		boardOneLevels["1-3"] = false;
		boardOneLevels["1-4"] = false;
		boardOneLevels["1-5"] = false;
		boardOneLevels["1-6"] = false;
		boardOneLevels["1-7"] = false;
		boardOneLevels["1-8"] = false;
		boardOneLevels["1-9"] = false;
		boardOneLevels["1-10"] = false;
		boardOneLevels["1-11"] = false;
		boardOneLevels["1-12"] = false;
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

		return boardOneLevels[levelName];
	}

	public static void SetLevelStatus (string levelName, bool status)
	{
		/*if (levelName == "boardOneLevelOne")
		{
			boardOneLevelOne = status;
		}*/

		Debug.Log("Input Level Name: " + levelName);

		boardOneLevels[levelName] = status;
	}

	public static Dictionary<string, bool> GetBoardOneDictionary ()
	{
		return boardOneLevels;
	}

	public static void SetBoardOneDictionary (Dictionary<string, bool> toSet)
	{
		boardOneLevels = toSet;
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
