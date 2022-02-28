using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector3Int size;
    public float spacing;
    public GameObject linePrefab;
    
    [HideInInspector]
    public Vector3Int topCenter;
    public static GridController instance;
    public Vector3 coordStart;

    private Row[] rows;
    private VisualGrid visualGrid;

    private void Awake() 
    {
        if(instance == null)
            instance = this;

        coordStart = new Vector3(-size.x/2, -size.y/2, 0) * spacing;
        topCenter = new Vector3Int(size.x/2, size.y-1 , size.z/2);

        rows = new Row[size.y];
        for(int i = 0; i < size.y; ++i)
            rows[i] = new Row(this, i);

        visualGrid = new VisualGrid(this);
    }

    public static Vector3 gridToWorldSpace(Vector3Int coords) 
    {
        return instance.coordStart + ((Vector3)coords + Vector3.one/2)* instance.spacing;
    }

    public static int worldSpaceYToGrid(float y)
    {
        // y = coordStart.y + (coords.y + 1/2) * spacing;
        // (y - coordStart.y)/spacing = coords.y + 1/2
        return Mathf.FloorToInt((y - instance.coordStart.y)/instance.spacing - 1/2);
    }

    public static Vector3 gridToSpaced(Vector3Int coords)
    {
        return (Vector3)coords * instance.spacing;
    }
    
    public static bool IsInsideHorizontally(Vector3Int coords)
    {
        return coords.x >= 0 && coords.z >= 0 && coords.x < instance.size.x && coords.z < instance.size.z;
    }

    public static bool IsColliding(Vector3Int coords)
    {
       if(coords.y >= instance.size.y) 
            return false;

        return coords.y < 0 || instance.rows[coords.y].isFilled[coords.x, coords.z];
    }

    public void FilledRowAction(Row row)
    {
        Debug.Log("filled");
    }

    public static void FillTileSpace(Tile tile)
    {
        foreach(TileSegment segment in tile.segments)
        {
            Vector3Int position = tile.centerGridPosition + segment.localPosition;
            if(position.y >= 0 && position.y < instance.size.y)
                instance.rows[position.y].Fill(position.x, position.z);
        }

    }
}