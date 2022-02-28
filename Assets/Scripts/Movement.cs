using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float fallingSpeed;
    public float fastFallingSpeed;
    [SerializeField]
    private float minTimeBetweenMoves;

    public static Movement instance;
    private float timer;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        
        timer = minTimeBetweenMoves;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public static void TryToMove(Move move, Tile tile) 
    {
        instance._TryToMove(move, tile);
    }

    private void _TryToMove(Move move, Tile tile)
    {
        CheckGridMovement(move, tile);
        move.Apply(tile);
    }

    private void CheckGridMovement(Move move, Tile tile) 
    {
        if(move.isTranslation())
        {
            if(timer >= minTimeBetweenMoves)
                timer = 0;
            else
                move.ClearTranslation();
        }

        if(!FitsInGrid(move, tile))
            move.ClearGridMovement();
    }

    private bool FitsInGrid(Move move, Tile tile) 
    {
        Vector3Int[] movedPositions = move.movedGridPositions(tile);
        foreach(Vector3Int pos in movedPositions)
        {
            if(!GridController.IsInsideHorizontally(pos) || GridController.IsColliding(pos))
                return false;
        }
        return true;
    }
}