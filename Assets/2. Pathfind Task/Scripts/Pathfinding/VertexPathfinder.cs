using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public class VertexPathfinder
    {
        private readonly VertexGraph _graph;
        private static readonly Vector2 EndPathMark = Vector2.one * -10e5f;

        public VertexPathfinder(VertexGraph graph)
        {
            _graph = graph;
        }

        public IEnumerable<Vector3> Find(Vector2 start, Vector2 end)
        {
            start = _graph.ToVertex(start);
            end = _graph.ToVertex(end);

            var routs = new Dictionary<Vector2, Vector2> { [start] = EndPathMark };
            var queue = new Queue<Vector2>();
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in _graph.GetNeighbors(current))
                {
                    if (routs.ContainsKey(neighbor))
                        continue;

                    routs[neighbor] = current;
                    queue.Enqueue(neighbor);
                }

                if (routs.ContainsKey(end))
                    break;
            }

            if (routs.ContainsKey(end) == false)
                yield break;

            foreach (var point in CreatePath(routs, end).Reverse())
                yield return point;
        }

        public List<List<Vector2>> FindAllRecursive(Vector2 currentPoint, Vector2 endPoint, List<List<Vector2>> paths = null, List<Vector2> path = null)
        {
            currentPoint = _graph.ToVertex(currentPoint);
            endPoint = _graph.ToVertex(endPoint);

            path ??= new List<Vector2>();
            paths ??= new List<List<Vector2>>();

            path.Add(currentPoint);

            if (currentPoint == endPoint)
            {
                paths.Add(path);
                return paths;
            }

            foreach (var neighbor in _graph.GetNeighbors(currentPoint))
            {
                if (path.Contains(neighbor))
                    continue;

                var newPath = new List<Vector2>(path);
                var newPaths = FindAllRecursive(neighbor, endPoint, paths, newPath);
                paths.AddRange(newPaths);
            }

            return paths;
        }

        private static IEnumerable<Vector3> CreatePath(IReadOnlyDictionary<Vector2, Vector2> routs, Vector2 end)
        {
            var pathItem = end;
            while (pathItem != EndPathMark)
            {
                yield return pathItem;
                pathItem = routs[pathItem];
            }
        }
    }
}