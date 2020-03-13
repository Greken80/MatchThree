using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiManager : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI scoreText;



    [SerializeField] private GuiManager guiManager;

    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas newLevelCanvas;

    public void AddScore(int score)
    {
        scoreText.text = score.ToString();      
    }


}
