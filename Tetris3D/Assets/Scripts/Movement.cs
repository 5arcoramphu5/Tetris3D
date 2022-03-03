using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float fallingSpeed;
    public float fastFallingSpeed;
    public float smoothRotationSpeed;
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

    public static void TryToMove(Move move) 
    {
        instance.CheckPossibleMovement(move);
        move.Apply();
    }

    private void CheckPossibleMovement(Move move)
    {
        if(!CanMoveHorizontally(move))
            move.ClearHorizontalMovement();
        
        if(!CanMoveVertically(move))
        {
            move.ClearVerticalMovement();
            move.Apply();
            TileController.TileLanded();
            return;
        }
    }

    private bool CanMoveHorizontally(Move move) 
    {
        if(!move.horizontal)
           return true;

        if(move.isTranslation())
        {
            if(timer >= minTimeBetweenMoves)
                timer = 0;
            else
                return false;
        }

        return FitsInGrid(move, false);
    }

    private bool CanMoveVertically(Move move)
    {
        return !move.verticalOnGrid || FitsInGrid(move, true);
    }

    private bool FitsInGrid(Move move, bool includeVertical) 
    {
        Vector3Int[] movedPositions = move.movedGridPositions(includeVertical);

        if(movedPositions == null)
            return true;

        foreach(Vector3Int pos in movedPositions)
        {
            if(!GridController.IsInsideHorizontally(pos) || GridController.IsColliding(pos))
                return false;
        }
        return true;
    }
}