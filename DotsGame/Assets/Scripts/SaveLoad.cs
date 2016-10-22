/*
Any time a change is made to save load that affects what is being saved, the gameSave.dat file need to be 
deleted so new variables will be loaded correctly. Otherwise the save data won't contain the information
and will through null ref exceptions when trying to retrieve it.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour 
{
	public static void Save ()
	{
		BinaryFormatter data = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");

		SaveableData saveData = new SaveableData();

		//-----------------------Saving Data---------------------------------------------
		Hashtable dataToSave = new Hashtable();

		dataToSave.Add("finishedTutorial", CampaignData.GetFinishedTutorial());
		dataToSave.Add("viewedAbilityMatchInstructions", CampaignData.ViewedMatchAbilityInstructions());
		dataToSave.Add("allBoardLevels", CampaignData.GetAllLevelsDictionary());
		dataToSave.Add("heroesUnlocked", CampaignData.GetAllHeroBoardsDictionary());
		dataToSave.Add("boardStarCounts", CampaignData.GetAllBoardStarCounts());

		//-----------------------Done Saving---------------------------------------------
		data.Serialize(file, dataToSave);
		file.Close();
		//Debug.Log("Saved here: " + Application.persistentDataPath);
	}



	public static void Load ()
	{
		if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
		{
			//Debug.Log("Loading...");

			BinaryFormatter data = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
			Hashtable saveData = (Hashtable) data.Deserialize(file);
			file.Close();

			//-----------------------Loading Stats---------------------------------
			CampaignData.SetFinishedTutorial((bool) saveData["finishedTutorial"]);
			CampaignData.SetAbilityInstructionsState((bool) saveData["viewedAbilityMatchInstructions"]);
			CampaignData.SetAllLevelsDictionary( (Dictionary<string, LevelStats>) saveData["allBoardLevels"]);
			CampaignData.SetAllHeroBoardsDictionary( (Dictionary<HeroManager.Hero, LevelStats>) saveData["heroesUnlocked"]);
			CampaignData.SetAllBoardStarCounts( (Dictionary<string, int>) saveData["boardStarCounts"]);
			//-----------------------Done Loading----------------------------------
		}
		else {
			Save();
		}
	}

	public static void Delete ()
	{
		//Debug.Log(Application.persistentDataPath);
		File.Delete (Application.persistentDataPath + "/gameSave.dat");
	}

	public static bool DoesSaveExist ()
	{
		return File.Exists(Application.persistentDataPath + "/gameSave.dat");
	}
}























