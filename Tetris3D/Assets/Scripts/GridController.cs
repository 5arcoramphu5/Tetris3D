using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector3Int size;
    public float spacing;
    public float rowsFallingSpeed;
    
    public static GridController instance;
    [HideInInspector]
    public Vector3Int topCenter;
    [HideInInspector]
    public Vector3 coordStart;

    private Row[] rows;
    private VisualGrid visualGrid;

    private void Awake() 
    {
        if(instance == null)
            instance = this;

        coordStart = -(Vector3)size/2 * spacing;
        topCenter = new Vector3Int(size.x/2, size.y-1 , size.z/2);

        rows = new Row[size.y];
        for(int i = 0; i < size.y; ++i)
            CreateNewRowAtIndex(i);

        visualGrid = GetComponent<VisualGrid>();
        visualGrid.CreateGrid(this);
    }

    private void CreateNewRowAtIndex(int index)
    {
        GameObject rowObject = new GameObject("Row");
        rows[index] = rowObject.AddComponent<Row>();
        rows[index].Initialize(this, index);
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

        return coords.y < 0 || instance.rows[coords.y].cells[coords.x, coords.z].isFilled;
    }

    public void DeleteRow(int index)
    {
        Destroy(rows[index].gameObject);

        for(int i = index + 1; i < size.y; i++)
        {
            rows[i].MoveDown();
            rows[i-1] = rows[i];
        } 

        CreateNewRowAtIndex(size.y-1);
    }

    public static void FillTileSpace(Tile tile)
    {
        List<int> filledRows = new List<int>();
        foreach(TileSegment segment in tile.segments)
        {
            Vector3Int position = tile.centerGridPosition + segment.localPosition;
            if(position.y >= instance.size.y)
            {
                GameManager.GameOver();
                return;
            }
            
            instance.rows[position.y].Fill(position.x, position.z, segment.transform);
            filledRows.Add(position.y);
        }

        foreach(int index in filledRows)
            instance.rows[index].CheckIfFilled();
    }

    public static Vector3Int RotateRoundToInt(Quaternion rotation, Vector3Int vector)
    {
        Vector3 newVector = rotation * vector;
        return new Vector3Int(Mathf.RoundToInt(newVector.x), Mathf.RoundToInt(newVector.y), Mathf.RoundToInt(newVector.z));
    }

    public static Vector2Int RotateRoundToInt(Quaternion rotation, Vector2Int vector)
    {
        Vector2 newVector = rotation * (Vector2)vector;
        return new Vector2Int(Mathf.RoundToInt(newVector.x), Mathf.RoundToInt(newVector.y));
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, (Vector3)size*spacing);
    }
    #endif
}
