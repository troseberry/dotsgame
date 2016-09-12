using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignData
{
	private static bool finishedTutorial;
	
	private static bool boardOneLevelOne;

	private static string lastScene;




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
		if (levelName == "boardOneLevelOne") 
		{
			return boardOneLevelOne;
		}
		else
		{
			return false;
		}
	}

	public static void SetLevelStatus (string levelName, bool status)
	{
		if (levelName == "boardOneLevelOne")
		{
			boardOneLevelOne = status;
		}
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
