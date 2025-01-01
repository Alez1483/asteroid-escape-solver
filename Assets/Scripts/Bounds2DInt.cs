using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Bounds2DInt
{
    public Vector2Int min;
    public Vector2Int max;

    public Vector2Int size
    {
        get
        {
            return (max - min) + Vector2Int.one;
        }
    }
    public Vector2 center
    {
        get
        {
            return min + (Vector2)(max - min) / 2f;
        }
    }

    public Bounds2DInt(Vector2Int min, Vector2Int max)
    {
        this.min = min;
        this.max = max;
    }

    //extends the bounds in the direction of the vector by the magnitude of it
    public void SwipeBounds(Vector2Int vector)
    {
        if (vector.x > 0)
        {
            max.x += vector.x; //extend to right
        }
        else
        {
            min.x += vector.x; //extend to left
        }

        if (vector.y > 0)
        {
            max.y += vector.y; //extend upwards
        }
        else
        {
            min.y += vector.y; //extend downwards
        }
    }

    public bool Intersects(Bounds2DInt other)
    {
        return other.min.x <= max.x && other.max.x >= min.x &&
            other.min.y <= max.y && other.max.y >= min.y;
    }

    public static Bounds2DInt operator+(Bounds2DInt b, Vector2Int v)
    {
        return new Bounds2DInt(b.min + v, b.max + v);
    }
    public static Bounds2DInt operator-(Bounds2DInt b, Vector2Int v)
    {
        return new Bounds2DInt(b.min - v, b.max - v);
    }
}
