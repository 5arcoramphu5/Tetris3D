using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text pointsDisplay;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Text scoreDisplay;

    public void DisplayPoints(int points)
    {
        pointsDisplay.text = "Points: " + points;
    }

    public void DisplayGameOverPanel(int score)
    {
        gameOverPanel.SetActive(true);
        scoreDisplay.text = "Score: " + score;
    }
}
