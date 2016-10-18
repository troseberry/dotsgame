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

	private static List<string> allBoardNames = new List<string>();
	private static Dictionary<string, int> boardStarCounts = new Dictionary<string, int>();

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

		allBoardNames.Add("BoardOne");
		allBoardNames.Add("BoardTwo");
		allBoardNames.Add("BoardThree");

		SaveLoad.Load();

		//If for some reason the loaded dictionary doesn't have the level, add it and make its completion status false
		for (int i = 0; i < allBoardLevelNames.Count; i++)
		{
			if (!allBoardLevels.ContainsKey(allBoardLevelNames[i]))
			{
				allBoardLevels.Add(allBoardLevelNames[i], new LevelStats(false, 0, 0));
			}
		}

		//Same for unlocked heroes (boss levels completed)
		for (int i = 0; i < allHeroNames.Count; i++)
		{
			if (!heroesUnlocked.ContainsKey(allHeroNames[i]))
			{
				heroesUnlocked.Add(allHeroNames[i], new LevelStats(false, 0, 0));
			}
		}

		for (int i = 0; i < allBoardNames.Count; i++)
		{
			if (!boardStarCounts.ContainsKey(allBoardNames[i]))
			{
				boardStarCounts.Add(allBoardNames[i], 0);
			}
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




	public static void ClearLevelsDictionary ()
	{
		for (int i = 0; i < allBoardLevelNames.Count; i++)
		{
			//Setting allBoardLevels values not allBoardLeveNames
			SetLevelStats(allBoardLevelNames[i], new LevelStats(false, 0, 0));
		}
	}

	public static LevelStats GetLevelStats (string levelName)
	{
		return allBoardLevels[levelName];
	}

	public static void SetLevelStats (string levelName, LevelStats stats)
	{
		allBoardLevels[levelName] = stats;
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
		for (int i = 0; i < allHeroNames.Count; i++)
		{
			SetHeroBoardStats(allHeroNames[i], new LevelStats(false, 0, 0));
		}
	}

	public static LevelStats GetHeroBoardStats (HeroManager.Hero heroBoardName)
	{
		return heroesUnlocked[heroBoardName];
	}

	public static void SetHeroBoardStats (HeroManager.Hero heroBoardName, LevelStats stats)
	{
		heroesUnlocked[heroBoardName] = stats;
	}

	public static Dictionary<HeroManager.Hero, LevelStats> GetAllHeroBoardsDictionary ()
	{
		return heroesUnlocked;
	}

	public static void SetAllHeroBoardsDictionary (Dictionary<HeroManager.Hero, LevelStats> toSet)
	{
		heroesUnlocked = toSet;
	}



	public static int GetBoardStars (string boardName)
	{
		return boardStarCounts[boardName];
	}

	public static void UpdateBoardStars (string boardName, int toAdd)
	{
		boardStarCounts[boardName] += toAdd;
	}

	public static bool EnoughBoardStars (string boardName)
	{
		if (boardName == "BoardOne")
		{
			return boardStarCounts[boardName] >= 15;
		}
		else if (boardName == "BoardTwo" || boardName == "BoardThree")
		{
			return boardStarCounts[boardName] >= 21;
		}

		return false;
	}

	public static Dictionary<string, int> GetAllBoardStarCounts ()
	{
		return boardStarCounts;
	}

	public static void SetAllBoardStarCounts (Dictionary<string, int> toSet)
	{
		boardStarCounts = toSet;
	}
}
