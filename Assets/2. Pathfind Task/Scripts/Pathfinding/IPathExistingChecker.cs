using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public interface IPathExistingChecker
    {
        bool IsPathExist(Vector2 pointA, Vector2 pointC);
    }
}