using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float deletedRowBonus;

    [Header("Components")]
    public UIManager UIManager;
    public Movement movement;
    public TileController tileController;

    public static bool gameOver { private set; get; }

    private static GameManager instance;
    private static int points = 0;

    private void Awake()
    {
        instance = this;
        gameOver = false;
        AddPoints(0);
    }

    public static void GameOver()
    {
        instance.UIManager.DisplayGameOverPanel(points);
        instance.DeactivateScripts();
        gameOver = true;
    }

    public static void FullRow(int index)
    {
        GridController.instance.DeleteRow(index);
        Movement.CheckSpeedUp();
        AddPoints( Mathf.FloorToInt( instance.deletedRowBonus * GridController.instance.size.x * GridController.instance.size.y) );
    }

    public static void AddPoints(int p)
    {
        points += p;
        instance.UIManager.DisplayPoints(points);
    }

    public void PlayAgain()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);   
    }

    private void DeactivateScripts()
    {
        tileController.enabled = false;
        movement.enabled = false;
    }
}
