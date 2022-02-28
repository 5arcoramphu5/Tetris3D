using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private bool rotate;
    private bool flip;
    private bool fastFall;
    private int Xtransl;
    private int Ztransl;

    private Move(bool r, bool f, bool q, int x, int z)
    {
        rotate = r;
        flip = f;
        fastFall = q;
        Xtransl = x;
        Ztransl = z;
    }

    public Move() : this(false, false, false, 0, 0) {}

    public void AddRotatation()
    { rotate = true; }

    public void AddFlip()
    { flip = true; }

    public void FastFall()
    { fastFall = true; }

    public void Add(int x, int z)
    {
        Xtransl += x;
        Ztransl += z;
    }

    public bool IsIdentity()
    {
        return Xtransl == 0 && Ztransl == 0 && !rotate && !flip;
    }

    public bool isTranslation()
    {
        return Xtransl != 0 || Ztransl != 0;
    }
    public void ClearGridMovement()
    {
        rotate = false;
        flip = false;
        ClearTranslation();
    }

    public void ClearTranslation()
    {
        Xtransl = 0;
        Ztransl = 0;
    }

    public Vector3Int TranslatedOnGrid(Vector3Int vector)
    {
        return vector + new Vector3Int(Xtransl, 0, Ztransl);
    }
    
    public Vector3Int[] movedGridPositions(Tile tile)
    {
        Vector3Int center = tile.centerGridPosition;
        Vector3Int translation = GetGridTranslation(tile);
        Vector3Int[] positions = new Vector3Int[4];
        Vector3Int localPosition;

        for(int i = 0; i<4; ++i)
        {
            localPosition = tile.segments[i].localPosition;

            if(flip)
                localPosition = Flip(localPosition);
            
            if(rotate)
                localPosition = Rotate(localPosition);

            positions[i] = center + localPosition + translation;
        }

        return positions;
    }

    private Vector3Int GetGridTranslation(Tile tile) 
    {
        Vector3Int translation = new Vector3Int(Xtransl, 0, Ztransl);

        float y = tile.transform.position.y;
        y -= (fastFall ? Movement.instance.fastFallingSpeed : Movement.instance.fallingSpeed) * Time.deltaTime;

        if(GridController.worldSpaceYToGrid(y) < tile.centerGridPosition.y)
            translation.y--;
        
        return translation;
    }

    public void Apply(Tile tile)
    {
        Vector3Int gridTranslation = GetGridTranslation(tile);
        tile.centerGridPosition += gridTranslation;

        Vector3 tileTranslation = GridController.gridToSpaced(gridTranslation);
        tileTranslation.y = -(fastFall ? Movement.instance.fastFallingSpeed : Movement.instance.fallingSpeed) * Time.deltaTime;
        tile.transform.Translate(tileTranslation);

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