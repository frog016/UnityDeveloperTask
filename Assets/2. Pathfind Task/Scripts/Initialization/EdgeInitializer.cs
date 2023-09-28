using PathfindTask.Structures;
using UnityEngine;

namespace PathfindTask.Initialization
{
    public class EdgeInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 _start;
        [SerializeField] private Vector2 _end;
        [SerializeField] private RectangleInitializer _first;
        [SerializeField] private RectangleInitializer _second;

        [SerializeField] private Color _debugColor;

        public Edge CreateEdge()
        {
            return new Edge(_first.CreateRectangle(), _second.CreateRectangle(), _start, _end);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _debugColor;
            Gizmos.DrawLine(_start, _end);
        }
    }
}