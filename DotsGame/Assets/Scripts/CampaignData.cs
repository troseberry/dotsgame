using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignData : MonoBehaviour
{
	private static bool finishedTutorial;
	private static string lastScene;
	
	
	private static Dictionary<string, LevelStats> allBoardLevels = new Dictionary<string, LevelStats>();
	private static List<string> allBoardLevelNames = new List<string>();
	
	private static Dictionary<HeroManager.Hero, LevelStats> heroesUnlocked = new Dictionary<HeroManager.Hero, LevelStats>();
	private static List<HeroManager.Hero> allHeroNames = new List<HeroManager.Hero>();
	public static HeroManager.Hero currentHero = HeroManager.Hero.None;

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

		
		allHeroNames.Add(HeroManager.Hero.Multiplier);
		allHeroNames.Add(HeroManager.Hero.Demolition);
		allHeroNames.Add(HeroManager.Hero.Thief);

		//Debug.Log("Levels Dictionary Exists? " + allBoardLevels);
		//Debug.Log("Heroes Unlocked Exists? " + heroesUnlocked);
		//currentHero =  HeroManager.Hero.None;

		SaveLoad.Load();

		//If for some reason the loaded dictionary doesn't have the level, add it and make its completion status false
		//foreach(string lvlName in allBoardLevelNames)
		for (int i = 0; i < allBoardLevelNames.Count; i++)
		{
			if (!allBoardLevels.ContainsKey(allBoardLevelNames[i]))
			{
				allBoardLevels.Add(allBoardLevelNames[i], new LevelStats(false, 0, 0));
			}
		}

		//Same for unlocked heroes (boss levels completed)
		//foreach (HeroManager.Hero hero in allHeroNames)
		for (int i = 0; i < allHeroNames.Count; i++)
		{
			if (!heroesUnlocked.ContainsKey(allHeroNames[i]))
			{
				heroesUnlocked.Add(allHeroNames[i], new LevelStats(false, 0, 0));
			}
		}




		//Debug.Log(boardOneLevels);
		/*foreach (KeyValuePair<string, bool> pair in allBoardLevels)
		{
		    Debug.Log(pair.Key + pair.Value);
		}*/

		//SaveLoad.Save();

		//Debug.Log("Last Scene Visited: " + lastScene);
	}


	public static bool GetFinishedTutorial ()
	{
		return finishedTutorial;
	}

	public static void SetFinishedTutorial (bool status)
	{
		finishedTutorial = status;
	}




	public static void ClearLevelsDictionary ()
	{
		//Can't iterate over dictionary and change values at same time
		//Iterate thru names list. Should contain the same strings as dictionary keys
		//foreach (string levelName in allBoardLevelNames)
		for (int i = 0; i < allBoardLevelNames.Count; i++)
		{
			//Setting allBoardLevels values no allBoardLeveNames
			//SetLevelStatus(levelName, false);	//Should be UpdateLevelStatus?
			UpdateLevelStatus(allBoardLevelNames[i], false, 0, 0);
		}
	}

	public static bool GetLevelStatus (string levelName)
	{
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

	public static Dictionary<string, LevelStats> GetAllLevelsDictionary ()
	{
		return allBoardLevels;
	}

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





	public static void ClearHeroesUnlockedDictionary () 
	{
		//foreach (HeroManager.Hero hero in allHeroNames)
		for (int i = 0; i < allHeroNames.Count; i++)
		{
			UpdateHeroBoardStatus(allHeroNames[i], false, 0, 0);
		}
	}

	public static bool GetHeroBoardStatus (HeroManager.Hero heroBoardName)
	{
		return heroesUnlocked[heroBoardName].isComplete;
	}

	public static LevelStats GetFullHeroBoardStatus (HeroManager.Hero heroBoardName)
	{
		return heroesUnlocked[heroBoardName];
	}

	public static void SetHeroBoardStatus (HeroManager.Hero heroBoardName, bool status)
	{
		heroesUnlocked[heroBoardName].isComplete = status;
	}

	public static void UpdateHeroBoardStatus (HeroManager.Hero heroBoardName, bool status, int rating, int score)
	{
		heroesUnlocked[heroBoardName].UpdateStats(status, rating, score);
	}

	public static Dictionary<HeroManager.Hero, LevelStats> GetAllHeroBoardsDictionary ()
	{
		return heroesUnlocked;
	}

	public static void SetAllHeroBoardsDictionary (Dictionary<HeroManager.Hero, LevelStats> toSet)
	{
		heroesUnlocked = toSet;
	}
}
