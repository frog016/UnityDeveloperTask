using PathfindTask.Structures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathfindTask.Pathfinding
{
    public class VertexGraph
    {
        private readonly Dictionary<Vector2, Vector2[]> _neighborsTable;
        private readonly Rectangle[] _rectangles;

        public VertexGraph(IEnumerable<Rectangle> rectangles, IEnumerable<Edge> edges)
        {
            _rectangles = rectangles.ToArray();
            _neighborsTable = CreateTable(_rectangles, edges);
        }

        public Vector2[] GetNeighbors(Vector2 point)
        {
            var vertex = ToVertex(point);
            return _neighborsTable[vertex];
        }

        public Vector2 ToVertex(Vector2 point)
        {
            return _rectangles.First(rect => rect.Contains(point)).Center;
        }

        private static Dictionary<Vector2, Vector2[]> CreateTable(IEnumerable<Rectangle> rectangles, IEnumerable<Edge> edges)
        {
            var rectTable = rectangles.ToDictionary(key => key, _ => new List<Rectangle>());
            foreach (var edge in edges)
            {
                rectTable[edge.First].Add(edge.Second);
                rectTable[edge.Second].Add(edge.First);
            }

            var vertexTable = rectTable.ToDictionary(
                pair => pair.Key.Center,
                pair => pair.Value
                    .Select(rect => rect.Center)
                    .ToArray());

            return vertexTable;
        }
    }
}