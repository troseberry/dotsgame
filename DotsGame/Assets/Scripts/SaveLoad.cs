using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
		 dataToSave.Add("boardOneLevelOne", CampaignData.GetLevelStatus("boardOneLevelOne"));
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

			CampaignData.SetLevelStatus("boardOneLevelOne", (bool) saveData["boardOneLevelOne"]);
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
}























