using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public static TileController instance = null;

    [SerializeField]
    private GameObject[] shapes;
    [SerializeField]
    private Color[] colors;

    public float timeInterval; // debug
    private float timer;    //debug

    private Tile currentTile = null;

    private void Start() 
    {
        if(instance == null)
            instance = this;

        //timer = timeInterval;
        SpawnShape();
    }

    private void Update()
    {   
        // timer += Time.deltaTime;
        // if(timer > timeInterval)
        // {
        //     SpawnShape();
        //     timer = 0;
        // }
    }
    
    private void SpawnShape()
    {
        GameObject shape = shapes[Random.Range(0, shapes.Length)];
        Color color = colors[Random.Range(0, colors.Length)];

        GameObject shapeObject = Instantiate(shape, transform);
        Tile tile = shapeObject.GetComponent<Tile>();
        shapeObject.name = "Tile";

        tile.ChangeColor(color);

        currentTile = tile;
    }

    public static void TileLanded()
    {
        GridController.FillTileSpace(instance.currentTile);
        instance.currentTile.Deactivate();
        instance.SpawnShape();
    }
}