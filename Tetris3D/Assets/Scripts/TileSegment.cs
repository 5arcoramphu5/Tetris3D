using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSegment : MonoBehaviour
{
    public Vector3Int localPosition;

    private void Awake() 
    {
        UpdatePosition();
    } 

    public void UpdatePosition(Vector3Int newLocalPosition)
    {
        localPosition = newLocalPosition;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.localPosition = GridController.gridToSpaced(localPosition);
    }
}

