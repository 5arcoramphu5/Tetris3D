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
    [SerializeField]
    private Transform nextShapeDisplay;
    [SerializeField]
    private float nextShapeSpinningSpeed;

    private GameObject previousNextShape;

    public void DisplayPoints(int points)
    {
        pointsDisplay.text = "Points: " + points;
    }

    public void DisplayGameOverPanel(int score)
    {
        gameOverPanel.SetActive(true);
        scoreDisplay.text = "Score: " + score;
    }

    public void DisplayNextShape(GameObject shape)
    {
        if(previousNextShape != null)
            Destroy(previousNextShape);

        GameObject gObject = Instantiate(shape, nextShapeDisplay);
        gObject.AddComponent<RectTransform>();
        Spinning spinning = gObject.AddComponent<Spinning>();
        spinning.speed = nextShapeSpinningSpeed;

        previousNextShape = gObject;
    }
}
