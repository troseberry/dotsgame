using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveableData
{
	public bool finishedTutorial;

	//List of campaign levels. True == finished
	public bool boardOneLevelOne;
	public bool boardOneLevelTwo;
	public bool boardOneLevelThree;
	public bool boardOneLevelFour;
	public bool boardOneLevelFive;
	public bool boardOneLevelSix;
	public bool boardOneLevelSeven;
	public bool boardOneLevelEight;
	public bool boardOneLevelNine;
	public bool boardOneLevelTen;
	public bool boardOneLevelEleven;
	public bool boardOneLevelTwelve;

	public Dictionary<string, bool> boardOneLevels;
}
