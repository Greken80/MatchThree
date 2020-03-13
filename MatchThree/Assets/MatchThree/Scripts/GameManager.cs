using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public int currentLevel { get; private set; } = 0;


    public int[] levelScores;


    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        BoardManager.Instance.CreateBoard();
    }


    public void ResetGame()
    {
        BoardManager.Instance.ResetBoard();
        ScoreManager.Instance.ScoreReset();
    }
 
    public int GetCurrentLevelInfo()
    {

        int levelPoints = levelScores[currentLevel];

        return levelPoints;
    }

    public void IncreaseLevel()
    {
        currentLevel++;

        if(currentLevel > levelScores.Length)
        {
            //Show gameover
            //Give the user the option to start again or quit
            currentLevel = 0;
        }
        else
        {
            //show new level screen;
        }

    }

}
