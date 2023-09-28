using PathfindTask.Structures;
using System.Collections.Generic;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public interface IPathfinder
    {
        IEnumerable<Vector2> GetPath(Vector2 pointA, Vector2 pointC, IEnumerable<Edge> edges);
    }
}
