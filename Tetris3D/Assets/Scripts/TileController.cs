using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public static TileController instance = null;

    public UIManager uiManager;

    [SerializeField]
    private GameObject[] shapes;
    [SerializeField]
    private Color[] colors;

    private GameObject nextShape;
    private Tile currentTile = null;

    private void Start() 
    {
        instance = this;

        nextShape = shapes[Random.Range(0, shapes.Length)];
        SpawnShape();
    }
    
    private void SpawnShape()
    {
        GameObject shapeObject = Instantiate(nextShape, transform);
        Color color = colors[Random.Range(0, colors.Length)];
        Tile tile = shapeObject.AddComponent<Tile>();

        shapeObject.name = "Tile";
        tile.ChangeColor(color);
        currentTile = tile;

        nextShape = shapes[Random.Range(0, shapes.Length)];
        uiManager.DisplayNextShape(nextShape);
    }

    public static void TileLanded()
    {
        GridController.FillTileSpace(instance.currentTile);
        GameManager.AddPoints(4);
        instance.currentTile.Deactivate();
        
        if(!GameManager.gameOver) 
            instance.SpawnShape();
    }
}
