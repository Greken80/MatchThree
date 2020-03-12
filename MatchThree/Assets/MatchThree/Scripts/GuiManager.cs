using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiManager : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI scoreText;

    public void AddScore(int score)
    {
        scoreText.text = score.ToString();      
    }



}
