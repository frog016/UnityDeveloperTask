using System;
using UnityEngine;

namespace PathfindTask.Structures
{
    [Serializable]
    public struct Rectangle
    {
        public Vector2 Min;
        public Vector2 Max;

        public Vector2 Center => (Min + Max) / 2f;
        public Vector2 Size => Max - Min;

        public Rectangle(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= Min.x && point.x <= Max.x &&
                   point.y >= Min.y && point.y <= Max.y;
        }
    }
}
