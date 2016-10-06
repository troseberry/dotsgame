using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignData : MonoBehaviour
{
	private static bool finishedTutorial;
	private static string lastScene;
	
	
	private static Dictionary<string, LevelStats> allBoardLevels = new Dictionary<string, LevelStats>();
	private static List<string> allBoardLevelNames = new List<string>();
	
	private static Dictionary<string, bool> heroesUnlocked = new Dictionary<string, bool>();
	public static HeroManager.Hero currentHero;

	void Start ()
	{
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
				//allBoardLevels.Add(lvlName, false);
				allBoardLevels.Add(lvlName, new LevelStats(false, 0, 0));
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
		//Can't iterate over dictionary and change values at same time
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
		//return allBoardLevels[levelName];
		return allBoardLevels[levelName].isComplete;
	}

	public static LevelStats GetFullLevelStatus (string levelName)
	{
		return allBoardLevels[levelName];
	}


	public static void SetLevelStatus (string levelName, bool status)
	{
		//Debug.Log("Input Level Name: " + levelName);

		allBoardLevels[levelName].isComplete = status;
	}

	public static void UpdateLevelStatus (string levelName, bool status, int rating, int score)
	{
		allBoardLevels[levelName].UpdateStats(status, rating, score);
	}



	/*public static Dictionary<string, bool> GetAllLevelsDictionary ()
	{
		return allBoardLevels;
	}*/

	public static Dictionary<string, LevelStats> GetAllLevelsDictionary ()
	{
		return allBoardLevels;
	}





	/*public static void SetAllLevelsDictionary (Dictionary<string, bool> toSet)
	{
		allBoardLevels = toSet;
	}*/

	public static void SetAllLevelsDictionary (Dictionary<string, LevelStats> toSet)
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
