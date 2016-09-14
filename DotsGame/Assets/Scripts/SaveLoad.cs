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

		/* dataToSave.Add("boardOneLevelOne", CampaignData.GetLevelStatus("boardOneLevelOne"));
		 dataToSave.Add("boardOneLevelTwo", CampaignData.GetLevelStatus("boardOneLevelTwo"));
		 dataToSave.Add("boardOneLevelThree", CampaignData.GetLevelStatus("boardOneLevelThree"));
		 dataToSave.Add("boardOneLevelFour", CampaignData.GetLevelStatus("boardOneLevelFour"));
		 dataToSave.Add("boardOneLevelFive", CampaignData.GetLevelStatus("boardOneLevelFive"));
		 dataToSave.Add("boardOneLevelSix", CampaignData.GetLevelStatus("boardOneLevelSix"));
		 dataToSave.Add("boardOneLevelSeven", CampaignData.GetLevelStatus("boardOneLevelSeven"));
		 dataToSave.Add("boardOneLevelEight", CampaignData.GetLevelStatus("boardOneLevelEight"));
		 dataToSave.Add("boardOneLevelNine", CampaignData.GetLevelStatus("boardOneLevelNine"));
		 dataToSave.Add("boardOneLevelTen", CampaignData.GetLevelStatus("boardOneLevelTen"));
		 dataToSave.Add("boardOneLevelEleven", CampaignData.GetLevelStatus("boardOneLevelEleven"));
		 dataToSave.Add("boardOneLevelTwelve", CampaignData.GetLevelStatus("boardOneLevelTwelve"));*/

		dataToSave.Add("boardOneLevels", CampaignData.GetBoardOneDictionary());
		 //-----------------------Done Saving---------------------------------------------
		 data.Serialize(file, dataToSave);
		 file.Close();
		 Debug.Log("Saved here: " + Application.persistentDataPath);
	}

	public static void Load ()
	{
		if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
		{
			Debug.Log("Loading...");

			BinaryFormatter data = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
			Hashtable saveData = (Hashtable) data.Deserialize(file);
			file.Close();

			//-----------------------Loading Stats---------------------------------
			CampaignData.SetFinishedTutorial((bool) saveData["finishedTutorial"]);

			//CampaignData.SetLevelStatus("boardOneLevelOne", (bool) saveData["boardOneLevelOne"]);

			CampaignData.SetBoardOneDictionary( (Dictionary<string, bool>) saveData["boardOneLevels"]);
			//-----------------------Done Loading----------------------------------
		}
		else {
			Save();
		}
	}

	public static void Delete ()
	{
		Debug.Log(Application.persistentDataPath);
		File.Delete (Application.persistentDataPath + "/gameSave.dat");
	}

	public static bool DoesSaveExist ()
	{
		return File.Exists(Application.persistentDataPath + "/gameSave.dat");
	}
}























