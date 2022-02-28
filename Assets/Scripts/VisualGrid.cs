using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualGrid
{
    GridController gc;

    public VisualGrid(GridController _gc)
    {
        gc = _gc;

        GenerateVerticalGrid(Vector3.right, Vector3.forward, gc.size.x, gc.size.z);
        GenerateVerticalGrid(Vector3.forward, Vector3.right, gc.size.z, gc.size.x);

        for(int i = 0; i <= gc.size.y; ++i)
        {
            LineRenderer lineRend = GameObject.Instantiate(gc.linePrefab, gc.transform).GetComponent<LineRenderer>();
            lineRend.transform.localPosition = gc.coordStart;
            
            Vector3[] positions = new Vector3[4];
            positions[0] = new Vector3(0, i, 0);
            positions[1] = new Vector3(gc.size.x, i, 0);
            positions[2] = new Vector3(gc.size.x, i, gc.size.z);
            positions[3] = new Vector3(0, i, gc.size.z);

            for(int j = 0; j < 4; ++j) 
                positions[j] *= gc.spacing;

            lineRend.positionCount = 4;
            lineRend.SetPositions(positions);
            lineRend.loop = true;
        }
    }

    private void GenerateVerticalGrid(Vector3 direction, Vector3 direction2,  int _size, int _size2)
    {
        for(int i = 0; i < _size; ++i)
        {
            LineRenderer[] lineRends = new LineRenderer[2];

            for(int j = 0; j<2; ++j)
            {
                lineRends[j] = GameObject.Instantiate(gc.linePrefab, gc.transform).GetComponent<LineRenderer>();
                lineRends[j].transform.localPosition = gc.coordStart;
            }
            
            Vector3[] positions = { direction * i * gc.spacing, (Vector3.up * gc.size.y + direction*i) * gc.spacing};
            lineRends[0].SetPositions(positions);

            for(int j = 0; j<2; ++j) 
                positions[j] += direction2 * _size2 * gc.spacing;
                
            lineRends[1].SetPositions(positions);
        }
    }

    public void Hightlight(Vector3Int coord) 
    {
        // TODO
    }
}
