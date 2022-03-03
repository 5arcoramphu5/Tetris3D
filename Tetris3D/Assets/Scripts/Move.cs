using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    Tile tile;
    private Vector3 rotation = Vector3.zero;
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

    public void AddRotatation(Vector2Int r)
    { 
        rotation += new Vector3(r.x, 0, r.y) * 90;
        horizontal = true;
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
            
            if(rotation != Vector3.zero)
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

            if(rotation != Vector3.zero)
            {
                foreach(TileSegment segment in tile.segments)
                    segment.UpdatePosition( Rotate(segment.localPosition) );
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

    private Vector3Int Rotate(Vector3Int vector)
    {
        return GridController.RotateRoundToInt(Quaternion.Euler(rotation), vector);
    }
}