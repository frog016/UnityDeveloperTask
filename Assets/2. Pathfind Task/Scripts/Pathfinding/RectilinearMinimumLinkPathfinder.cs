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

        private IEnumerable<List<Edge>> FindPaths(Vector2 pointA, Vector2 pointC, List<Edge> edges) // TODO: Need fix this. Finds extra paths.
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
                    var lastEdge = path.LastOrDefault();
                    if (path.Count != 0 && IsLinesIntersects(lastPoint, pointC, lastEdge.Start, lastEdge.End, out _) == false)
                        resultPath.Add(edgePoint);

                    resultPath.Add(pointC);
                    return resultPath;
                }

                lastPoint = edgePoint;
                startIndex = currentEdgeIndex;
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
                if (IsLinesIntersects(rayStartPoint, firstLastPoint, path[i].Start, path[i].End, out _) == false)
                {
                    if (IsLinesIntersects(rayStartPoint, secondLastPoint, path[i].Start, path[i].End, out _) == false)
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

        //private static bool IsLinesIntersects(Vector2 firstLineStart, Vector2 firstLineEnd, Vector2 secondLineStart, Vector2 secondLineEnd)
        //{
        //    var line1 = new Line() { StartPoint = firstLineStart, EndPoint = firstLineEnd };
        //    var line2 = new Line() { StartPoint = secondLineStart, EndPoint = secondLineEnd };

        //    return line1.Intersects(line2);
        //}

        public static bool Approximately(float a, float b, float tolerance = 1e-5f)
        {
            return Mathf.Abs(a - b) <= tolerance;
        }

        public static float CrossProduct2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool IsLinesIntersects(Vector2 p1start, Vector2 p1end, Vector2 p2start, Vector2 p2end,
        out Vector2 intersection)
        {
            // Consider:
            //   p1start = p
            //   p1end = p + r
            //   p2start = q
            //   p2end = q + s
            // We want to find the intersection point where :
            //  p + t*r == q + u*s
            // So we need to solve for t and u
            var p = p1start;
            var r = p1end - p1start;
            var q = p2start;
            var s = p2end - p2start;
            var qminusp = q - p;

            float cross_rs = CrossProduct2D(r, s);

            if (Approximately(cross_rs, 0f))
            {
                // Parallel lines
                if (Approximately(CrossProduct2D(qminusp, r), 0f))
                {
                    // Co-linear lines, could overlap
                    float rdotr = Vector2.Dot(r, r);
                    float sdotr = Vector2.Dot(s, r);
                    // this means lines are co-linear
                    // they may or may not be overlapping
                    float t0 = Vector2.Dot(qminusp, r / rdotr);
                    float t1 = t0 + sdotr / rdotr;
                    if (sdotr < 0)
                    {
                        // lines were facing in different directions so t1 > t0, swap to simplify check
                        Swap(ref t0, ref t1);
                    }

                    if (t0 <= 1 && t1 >= 0)
                    {
                        // Nice half-way point intersection
                        float t = Mathf.Lerp(Mathf.Max(0, t0), Mathf.Min(1, t1), 0.5f);
                        intersection = p + t * r;
                        return true;
                    }
                    else
                    {
                        // Co-linear but disjoint
                        intersection = Vector2.zero;
                        return false;
                    }
                }
                else
                {
                    // Just parallel in different places, cannot intersect
                    intersection = Vector2.zero;
                    return false;
                }
            }
            else
            {
                // Not parallel, calculate t and u
                float t = CrossProduct2D(qminusp, s) / cross_rs;
                float u = CrossProduct2D(qminusp, r) / cross_rs;
                if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
                {
                    intersection = p + t * r;
                    return true;
                }
                else
                {
                    // Lines only cross outside segment range
                    intersection = Vector2.zero;
                    return false;
                }
            }
        }

        public class Line
        {
            public Vector2 StartPoint { get; set; }
            public Vector2 EndPoint { get; set; }

            public bool Intersects(Line otherLine)
            {
                var direction1 = EndPoint - StartPoint;
                var direction2 = otherLine.EndPoint - otherLine.StartPoint;

                var crossProduct1 = CrossProduct(direction2, StartPoint - otherLine.StartPoint);
                var crossProduct2 = CrossProduct(direction2, EndPoint - otherLine.StartPoint);

                var dotProduct1 = DotProduct(crossProduct1, crossProduct2);

                crossProduct1 = CrossProduct(direction1, otherLine.StartPoint - StartPoint);
                crossProduct2 = CrossProduct(direction1, otherLine.EndPoint - StartPoint);

                var dotProduct2 = DotProduct(crossProduct1, crossProduct2);

                return dotProduct1 < 0 && dotProduct2 < 0;
            }

            private static float DotProduct(Vector2 first, Vector2 second)
            {
                return Vector2.Dot(first, second);
            }

            private static Vector3 CrossProduct(Vector2 first, Vector2 second)
            {
                return Vector3.Cross(first, second);
                //var crossVector = Vector3.Cross(first, second);
                //Debug.Log("Unity formula: " + crossVector.z);
                //Debug.Log("Other formula: " + (first.x * second.y - first.y * second.x));
                //return first.x * second.y - first.y * second.x;
            }
        }
    }
}