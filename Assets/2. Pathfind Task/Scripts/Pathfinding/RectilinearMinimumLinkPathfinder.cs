using PathfindTask.Structures;
using System.Collections.Generic;
using System.Linq;
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
            var path = ConvertPointsToEdgesPath(pointA, pointC, edges).ToList();
            var rectilinearPath = FindRectilinearPath(pointA, pointC, path);

            return rectilinearPath;
        }

        private IEnumerable<Edge> ConvertPointsToEdgesPath(Vector2 pointA, Vector2 pointC, IEnumerable<Edge> edges)
        {
            var path = _pathfinder
                .Find(pointA, pointC)
                .ToArray();

            var edgesList = edges.ToList();
            for (var i = 0; i < path.Length - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];

                var edge = edgesList.First(e => e.First.Center == current && e.Second.Center == next);
                yield return edge;
            }
        }

        private static IEnumerable<Vector2> FindRectilinearPath(Vector2 pointA, Vector2 pointC, IReadOnlyList<Edge> path)
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
                    var lastEdge = path.LastOrDefault();
                    if (path.Count != 0 && MathUtilities.IsLinesIntersects(lastPoint, pointC, lastEdge.Start, lastEdge.End, out _) == false)
                        resultPath.Add(edgePoint);

                    resultPath.Add(pointC);
                    return resultPath;
                }

                lastPoint = edgePoint;
                startIndex = currentEdgeIndex;
                resultPath.Add(lastPoint);
            }
        }

        private static bool IsRayIntersectEdge(Vector2 rayStartPoint, IReadOnlyList<Edge> path, int startIndex, int currentEdgeIndex, out Vector2 edgePoint)
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

        private static bool IsRayIntersectEdgesToIndex(Vector2 rayStartPoint, IReadOnlyList<Edge> path, int startIndex, int endIndex, out Vector2 edgePoint)
        {
            var lastEdge = path[endIndex];
            var firstLastPoint = lastEdge.Start;
            var secondLastPoint = lastEdge.End;
            edgePoint = default;

            for (var i = startIndex; i <= endIndex; i++)
            {
                if (MathUtilities.IsLinesIntersects(rayStartPoint, firstLastPoint, path[i].Start, path[i].End, out _) == false)
                {
                    if (MathUtilities.IsLinesIntersects(rayStartPoint, secondLastPoint, path[i].Start, path[i].End, out _) == false)
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
    }
}