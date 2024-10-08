using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class VectorExt
{
    public static Vector3Int ToVector3Int(this Vector2Int v)
    {
        return new Vector3Int(v.x, v.y);
    }

    public static Vector2Int ToVector2Int(this Vector3Int v)
    {
        return new Vector2Int(v.x, v.y);
    }
}