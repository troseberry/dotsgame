using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LevelStats
{
    public bool isComplete {get; set;}
    public int starRating {get; set;}
    public int bestScore {get; set;}

    public LevelStats (bool complete, int rating, int score)
    {
        this.isComplete = complete;
        this.starRating = rating;
        this.bestScore = score;
    }

    public void UpdateStats (bool complete, int rating, int score)
    {
        this.isComplete = complete;
        this.starRating = rating;
        this.bestScore = score;
    }

}
