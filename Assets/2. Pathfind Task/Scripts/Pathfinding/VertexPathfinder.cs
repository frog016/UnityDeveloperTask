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

        public IEnumerable<Vector2> Find(Vector2 start, Vector2 end)
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

        private static IEnumerable<Vector2> CreatePath(IReadOnlyDictionary<Vector2, Vector2> routs, Vector2 end)
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