using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool isFilled { private set; get; }
    public Transform cube { private set; get; }

    public Cell()
    {
        isFilled = false;
        cube = null;
    }

    public void Fill(Transform c, Vector3Int coords) 
    {
        isFilled = true;
        cube = c;
        cube.transform.position = GridController.gridToWorldSpace(coords);
    }

    public static void Move(Cell from, Cell to, Vector3Int toCoords)
    {
        if(to.cube != null)
            GameObject.Destroy(to.cube.gameObject);
            
        to.isFilled = from.isFilled;
        to.cube = from.cube;

        if(to.cube != null)
            to.cube.position = GridController.gridToWorldSpace(toCoords);

        from.isFilled = false;
        from.cube = null;
    }
}
