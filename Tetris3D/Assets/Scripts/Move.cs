using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    Tile tile;
    private bool rotate = false;
    private bool flip = false;
    private bool fastFall = false;
    private int Xtransl = 0;
    private int Ztransl = 0;

    public bool horizontal { private set; get; }
    public bool verticalOnGrid { private set; get; }
    private bool vertical;

    public Move(Tile t)
    {
        tile = t;

        float y = tile.transform.position.y;
        y -= Movement.instance.fallingSpeed * Time.deltaTime;
        verticalOnGrid =  GridController.worldSpaceYToGrid(y) < tile.centerGridPosition.y;

        horizontal = false;
        vertical = true;
    }

    public void AddRotatation()
    { 
        rotate = horizontal = true;
    }

    public void AddFlip()
    { 
        flip = horizontal = true; 
    }

    public void FastFall()
    { 
        fastFall = true; 

        float y = tile.transform.position.y;
        y -= Movement.instance.fastFallingSpeed * Time.deltaTime;
        verticalOnGrid =  GridController.worldSpaceYToGrid(y) < tile.centerGridPosition.y;
    }

    public void Add(Vector2Int translation)
    {
        Xtransl += translation.x;
        Ztransl += translation.y;
        horizontal = true;
    }

    public void ClearHorizontalMovement()
    {
        horizontal = false;
    }

    public void ClearVerticalMovement()
    {
        vertical = false;
    }

    public bool isTranslation()
    {
        return Xtransl != 0 || Ztransl != 0;
    }

    public void ClearTranslation()
    {
        Xtransl = 0;
        Ztransl = 0;
    }
    
    //zwraca null gdy pozycje się nie zmieniają
    public Vector3Int[] movedGridPositions(bool includeVerticalMovement)
    {
        if(!horizontal && !(vertical && includeVerticalMovement && verticalOnGrid))
            return null;

        Vector3Int center = tile.centerGridPosition;
        Vector3Int translation = Vector3Int.zero;

        Vector3Int[] positions = new Vector3Int[4];
        for(int i = 0; i<4; ++i)
            positions[i] = tile.segments[i].localPosition;
        
        if(horizontal)
        {
            translation.x = Xtransl;
            translation.z = Ztransl;
            
            if(flip)
                for(int i = 0; i<4; ++i)
                    positions[i] = Flip(positions[i]);
            
            if(rotate)
                for(int i = 0; i<4; ++i) 
                    positions[i] = Rotate(positions[i]);
        } 

        if(vertical && includeVerticalMovement && verticalOnGrid)
                translation.y--;

        for(int i = 0; i<4; ++i) 
            positions[i] += center + translation;

        return positions;
    }

    public void Apply()
    {
        if(!horizontal && !vertical)
            return;

        Vector3Int gridTranslation = Vector3Int.zero;

        if(horizontal)
        {
            gridTranslation.x = Xtransl;
            gridTranslation.z = Ztransl;

            if(rotate)
            {
                foreach(TileSegment segment in tile.segments)
                    segment.UpdatePosition( Rotate(segment.localPosition) );
            }

            if(flip)
            {
                foreach(TileSegment segment in tile.segments)
                    segment.UpdatePosition( Flip(segment.localPosition) );
            } 
        }

        if(vertical && verticalOnGrid)
            gridTranslation.y = -1;

        tile.centerGridPosition += gridTranslation;

        Vector3 tileTranslation = GridController.gridToSpaced(gridTranslation);

        if(vertical)
            tileTranslation.y = -(fastFall ? Movement.instance.fastFallingSpeed : Movement.instance.fallingSpeed) * Time.deltaTime;

        tile.transform.Translate(tileTranslation);
    }

    private static Vector3Int Rotate(Vector3Int vector)
    {
        Vector3 newVector = Quaternion.Euler(0, 90, 0) * vector;
        return new Vector3Int(Mathf.RoundToInt(newVector.x), Mathf.RoundToInt(newVector.y), Mathf.RoundToInt(newVector.z));
    }

    private static Vector3Int Flip(Vector3Int vector)
    {
        return new Vector3Int(vector.x, -vector.y, vector.z);
    }
}