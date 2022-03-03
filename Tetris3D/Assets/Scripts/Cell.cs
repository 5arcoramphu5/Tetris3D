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
        cube.transform.localPosition = GridController.gridToSpaced(coords);
    }
}
