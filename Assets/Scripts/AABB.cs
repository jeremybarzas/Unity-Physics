using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AABB_A
{
    public Vector2 pos;
    public Vector2 min;
    public Vector2 max;

    public float left;
    public float right;
    public float top;
    public float bottom;

    public bool TestOverlap(AABB_A a, AABB_A b)
    {
        // Matthew's Overlap
        float d1x = b.min.x - a.max.x;
        float d1y = b.min.y - a.max.y;

        float d2x = a.min.x - b.max.x;
        float d2y = a.min.y - b.max.y;

        if(d1x > 0 || d1y > 0)
            return false;

        if (d2x > 0 || d2y > 0)
            return false;

        return true;
    }
    
    public AABB_A(Vector2 minimum, Vector2 maximum)
    {
        min = minimum;
        max = maximum;
        pos = new Vector2( min.x, min.y);

        right = max.x;
        left = min.x;
        top = max.y;
        bottom = min.y;
    }

    public AABB_A(Vector2 position, float xSize, float ySize)
    {
        pos = position;

        min.x = pos.x - xSize;
        max.x = pos.x + xSize;

        min.y = pos.y - ySize;
        max.y = pos.y + ySize;

        right = max.x;
        left = min.x;
        top = max.y;
        bottom = min.y;
    }
}
