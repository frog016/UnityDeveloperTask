using System.Collections.Generic;
using System.Linq;
using PathfindTask.Structures;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public class RectilinearMinimumLinkPathfinder : IPathfinder
    {
        private readonly VertexPathfinder _pathfinder;

        public RectilinearMinimumLinkPathfinder(VertexPathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public IEnumerable<Vector2> GetPath(Vector2 pointA, Vector2 pointC, IEnumerable<Edge> edges)
        {
            var pathsAcrossEdges = FindPaths(pointA, pointC, edges.ToList()).ToList();
            var minimalPath = new List<Vector2>();

            foreach (var path in pathsAcrossEdges)
            {
                var rectilinearPath = FindRectilinearPath(pointA, pointC, path);

                if (minimalPath.Count == 0)
                    minimalPath = rectilinearPath;
                else if (rectilinearPath.Count < minimalPath.Count)
                    minimalPath = rectilinearPath;
            }
            
            return minimalPath;
        }

        private IEnumerable<List<Edge>> FindPaths(Vector2 pointA, Vector2 pointC, List<Edge> edges)
        {
            var pointPath = _pathfinder.FindAllRecursive(pointA, pointC);
            foreach (var path in pointPath)
            {
                var edgePath = new List<Edge>();
                for (var i = 0; i < path.Count - 1; i++)
                {
                    var current = path[i];
                    var next = path[i + 1];

                    var edge = edges.First(e => e.First.Center == current && e.Second.Center == next);
                    edgePath.Add(edge);
                }

                yield return edgePath;
            }
        }

        private static List<Vector2> FindRectilinearPath(Vector2 pointA, Vector2 pointC, List<Edge> path)
        {
            var resultPath = new List<Vector2> { pointA };
            var lastPoint = pointA;

            var startIndex = 0;
            var currentEdgeIndex = 0;
            while (true)
            {
                Vector2 edgePoint = default;
                while (IsRayIntersectEdge(lastPoint, path, startIndex, currentEdgeIndex, out var point))
                {
                    edgePoint = point;
                    currentEdgeIndex++;
                }

                if (currentEdgeIndex == path.Count)
                {
                    resultPath.Add(pointC);
                    return resultPath;
                }

                lastPoint = edgePoint;
                startIndex = currentEdgeIndex - 1;
                resultPath.Add(lastPoint);
            }
        }

        private static bool IsRayIntersectEdge(Vector2 rayStartPoint, List<Edge> path, int startIndex, int currentEdgeIndex, out Vector2 edgePoint)
        {
            edgePoint = default;
            if (currentEdgeIndex >= path.Count)
                return false;

            for (var index = startIndex; index <= currentEdgeIndex; index++)
            {
                if (IsRayIntersectEdgesToIndex(rayStartPoint, path, startIndex, index, out edgePoint) == false)
                    return false;
            }

            return true;
        }

        private static bool IsRayIntersectEdgesToIndex(Vector2 rayStartPoint, List<Edge> path, int startIndex, int endIndex, out Vector2 edgePoint)
        {
            var lastEdge = path[endIndex];
            var firstLastPoint = lastEdge.Start;
            var secondLastPoint = lastEdge.End;
            edgePoint = default;

            for (var i = startIndex; i <= endIndex; i++)
            {
                if (IsLinesIntersects(rayStartPoint, firstLastPoint, path[i].Start, path[i].End) == false)
                {
                    if (IsLinesIntersects(rayStartPoint, secondLastPoint, path[i].Start, path[i].End) == false)
                    {
                        return false;
                    }

                    edgePoint = secondLastPoint;
                }
                else
                {
                    edgePoint = firstLastPoint;
                }
            }

            return true;
        }

        private static bool IsLinesIntersects(Vector2 firstLineStart, Vector2 firstLineEnd, Vector2 secondLineStart, Vector2 secondLineEnd)
        {
            var angle1 = CrossProduct(secondLineEnd - secondLineStart, firstLineStart - secondLineStart);
            var angle2 = CrossProduct(secondLineEnd - secondLineStart, firstLineEnd - secondLineStart);
            var angle3 = CrossProduct(firstLineEnd - firstLineStart, secondLineStart - firstLineStart);
            var angle4 = CrossProduct(firstLineEnd - firstLineStart, secondLineEnd - firstLineStart);
            return angle1 * angle2 <= 0 && angle3 * angle4 <= 0;
        }

        private static float CrossProduct(Vector2 first, Vector2 second)
        {
            return first.x * second.y - second.x * first.y;
        }
    }
}