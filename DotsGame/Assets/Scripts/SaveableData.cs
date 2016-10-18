using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveableData
{
	public bool finishedTutorial;

	//public Dictionary<string, bool> allBoardLevels;
	public Dictionary<string, LevelStats> allBoardLevels;
    public Dictionary<HeroManager.Hero, LevelStats> heroesUnlocked;
	public Dictionary<string, int> boardStarCounts;
}
