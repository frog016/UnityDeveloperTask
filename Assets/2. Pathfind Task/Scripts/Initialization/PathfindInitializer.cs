using PathfindTask.Pathfinding;
using PathfindTask.Structures;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private RectangleInitializer[] _monoRectangles;
        [SerializeField] private EdgeInitializer[] _monoEdges;

        private IEnumerable<Rectangle> _rectangles;
        private IEnumerable<Edge> _edges;

        private IPathfinder _pathfinder;
        private IPathExistingChecker _pathExistingChecker;

        private void Awake()
        {
            _rectangles = _monoRectangles.Select(rect => rect.CreateRectangle());
            _edges = _monoEdges.Select(edge => edge.CreateEdge());

            var vertexGraph = new VertexGraph(_rectangles, _edges);
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
    }
}