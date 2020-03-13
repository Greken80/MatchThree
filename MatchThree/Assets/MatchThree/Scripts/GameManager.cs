using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public int currentLevel { get; private set; } = 0;


    public int levelScores = 600;


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
 


}
