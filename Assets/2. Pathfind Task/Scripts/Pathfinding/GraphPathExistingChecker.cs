using System.Linq;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public class GraphPathExistingChecker : IPathExistingChecker
    {
        private readonly VertexPathfinder _pathfinder;

        public GraphPathExistingChecker(VertexPathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public bool IsPathExist(Vector2 pointA, Vector2 pointC)
        {
            var path = _pathfinder.Find(pointA, pointC);
            return path.Any();
        }
    }
}