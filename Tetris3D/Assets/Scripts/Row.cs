using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    private GridController gc;
    private int index;
    public Cell[,] cells { private set; get; }
    private int leftToFill;

    public void Initialize(GridController _gc, int _index)
    {
        gc = _gc;
        transform.SetParent(_gc.transform);
        index = _index;
        transform.position = GridController.gridToWorldSpace(new Vector3Int(0, index, 0));

        cells = new Cell[gc.size.x, gc.size.z];
        for(int x = 0; x < gc.size.x; ++x)
            for(int y = 0; y < gc.size.z; ++y)
                cells[x, y] = new Cell();

        leftToFill = gc.size.x * gc.size.z;
    }

    public void Fill(int x, int y, Transform cube)
    {
        if(!cells[x, y].isFilled)
            leftToFill--;

        cube.SetParent(transform);
        cells[x, y].Fill(cube, new Vector3Int(x, 0, y));
    }

    public void CheckIfFilled()
    {
        if(leftToFill <= 0)
            gc.DeleteRow(index);
    }

    public void MoveDown()
    {
        index--;
        StartCoroutine(SmoothFall());
    }

    IEnumerator SmoothFall()
    {
        Vector3 target = GridController.gridToWorldSpace(new Vector3Int(0, index, 0));
        while(true)
        {
            float delta = -GridController.instance.rowsFallingSpeed * Time.deltaTime;
            if(transform.position.y + delta < target.y)
                break; 

            transform.Translate(new Vector3(0, delta, 0));
            yield return null;
        }
        transform.position = target;
    }
}