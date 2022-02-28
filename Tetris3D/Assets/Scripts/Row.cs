using System.Collections;
using System.Collections.Generic;

public class Row
{
    private GridController gc;
    private int index;
    public readonly bool[,] isFilled;
    private int leftToFill;

    public Row(GridController _gc, int _index)
    {
        gc = _gc;
        index = _index;
        isFilled = new bool[gc.size.x, gc.size.z];
        Clear();
    }

    public void Fill(int x, int y)
    {
        if(isFilled[x, y])
            return;

        isFilled[x, y] = true;
        leftToFill--;
        if(leftToFill == 0)
            gc.FilledRowAction(this);
    }

    public void Clear()
    {
        for(int x = 0; x < gc.size.x; ++x)
            for(int y = 0; y < gc.size.z; ++y)
                isFilled[x, y] = false;
        
        leftToFill = gc.size.x * gc.size.z;
    }
}