using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB
{
    public float xSize;
    public float ySize;

    public Vector2 position;
    public Vector2 min;
    public Vector2 max;

    public AABB(float x, float y)
    {
        xSize = x;
        ySize = y;
    }

    public void UpdatePosition(Vector3 pos)
    {
        position = new Vector2(pos.x, pos.y);

        min.x = position.x - xSize;
        max.x = position.x + xSize;

        min.y = position.y - ySize;
        max.y = position.y + ySize;
    }

    public bool TestOverlap(AABB a, AABB b)
    {
        // a on left of b
        if (a.max.x >= b.min.x && a.min.x <= b.min.x)
            if (a.max.y >= b.min.y && a.min.y <= b.min.y)
                return true;
        
        // a on right of b
        if (a.max.x <= b.min.x && a.min.x >= b.min.x)
            if (a.max.y <= b.min.y && a.min.y >= b.min.y)
                return true;
        

        return false;
    }
}
