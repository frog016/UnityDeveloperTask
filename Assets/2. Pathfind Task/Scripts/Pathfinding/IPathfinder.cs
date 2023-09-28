using System;
using System.Collections.Generic;
using System.Net;
using PathfindTask.Structures;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public interface IPathfinder
    {
        IEnumerable<Vector2> GetPath(Vector2 pointA, Vector2 pointC, IEnumerable<Edge> edges);
    }
}
