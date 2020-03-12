using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

    public int CurrentScore { get; private set; }

    [SerializeField]private GuiManager guiManager;

    [SerializeField] private int tileValue;

    public void AddPoints(int numberOfTiles)
    {
        CurrentScore += tileValue * numberOfTiles;
        UpdateScoreBoard(CurrentScore);
    }

    public void ScoreReset()
    {
        UpdateScoreBoard(0);
    }

    private void UpdateScoreBoard(int points)
    {
        guiManager.AddScore(points);
    }




}
