using System;
using System.Collections.Generic;
using System.Linq;
using PathfindTask.Pathfinding;
using PathfindTask.Structures;
using UnityEngine;

namespace PathfindTask.Initialization
{
    public class PathfindInitializer : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LineRenderer _pathDrawer;

        [Header("Data")]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Edge[] _edges;
        [SerializeField] private Rectangle[] _rectangles;

        private IPathfinder _pathfinder;
        private IPathExistingChecker _pathExistingChecker;

        private void Awake()
        {
            var vertexGraph = CreateGraph(_rectangles, _edges);
            var vertexPathfinder = new VertexPathfinder(vertexGraph);

            _pathfinder = new RectilinearMinimumLinkPathfinder(vertexPathfinder);
            _pathExistingChecker = new GraphPathExistingChecker(vertexPathfinder);
        }

        [ContextMenu(nameof(CreatePath))]
        public void CreatePath()
        {
            if (_pathExistingChecker.IsPathExist(_startPoint.position, _endPoint.position) == false)
            {
                Debug.LogWarning("Path is not exist.");
                return;
            }

            var points = _pathfinder
                .GetPath(_startPoint.position, _endPoint.position, _edges)
                .ToArray();

            _pathDrawer.positionCount = points.Length;
            _pathDrawer.SetPositions(points.Select(p => (Vector3)p).ToArray());

        }

        private static VertexGraph CreateGraph(IEnumerable<Rectangle> rectangles, IEnumerable<Edge> edges)
        {
            return new VertexGraph(rectangles, edges);
        }

        [SerializeField] private bool _debugDraw;

        private void OnDrawGizmos()
        {
            if (_debugDraw == false)
                return;

            Gizmos.color = Color.black;
            foreach (var rectangle in _rectangles)
            {
                var size = rectangle.Size;
                Gizmos.DrawLine(rectangle.Min, rectangle.Min + Vector2.Scale(Vector2.right, size));
                Gizmos.DrawLine(rectangle.Min + Vector2.Scale(Vector2.right, size), rectangle.Max);
                Gizmos.DrawLine(rectangle.Max, rectangle.Max + Vector2.Scale(Vector2.left, size));
                Gizmos.DrawLine(rectangle.Max + Vector2.Scale(Vector2.left, size), rectangle.Min);
            }

            Gizmos.color = Color.green;
            foreach (var edge in _edges)
            {
                Gizmos.DrawLine(edge.Start, edge.End);
            }
        }
    }
}