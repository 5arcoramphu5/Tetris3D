using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row
{
    private GridController gc;
    private int index;
    public Cell[,] cells { private set; get; }
    private int leftToFill;

    public Row(GridController _gc, int _index)
    {
        gc = _gc;
        index = _index;

        cells = new Cell[gc.size.x, gc.size.z];
        for(int x = 0; x < gc.size.x; ++x)
            for(int y = 0; y < gc.size.z; ++y)
                cells[x, y] = new Cell();

        leftToFill = gc.size.x * gc.size.z;
    }

    public void Fill(int x, int y, Transform cube)
    {
        if(cells[x, y].isFilled)
        {
            cells[x, y].Fill(cube, new Vector3Int(x, index, y));
            return;
        }

        cells[x, y].Fill(cube, new Vector3Int(x, index, y));
        leftToFill--;

        if(leftToFill == 0)
            gc.FilledRowAction(index);
    }

    public static void Move(Row from, Row to)
    {
        for(int x = 0; x < from.gc.size.x; ++x)
            for(int y = 0; y < from.gc.size.z; ++y)
            {
                Cell.Move(from.cells[x, y], to.cells[x, y], new Vector3Int(x, to.index, y));
            }
    }
}