using PathfindTask.Structures;
using UnityEngine;

namespace PathfindTask.Initialization
{
    public class RectangleInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField] private Color _debugColor;

        public Rectangle CreateRectangle()
        {
            var center = (Vector2)transform.position;
            var halfSize = _size / 2f;

            return new Rectangle(center - halfSize, center + halfSize);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _debugColor;

            var rectangle = CreateRectangle();

            Gizmos.DrawLine(rectangle.Min, rectangle.Min + Vector2.Scale(Vector2.right, _size));
            Gizmos.DrawLine(rectangle.Min + Vector2.Scale(Vector2.right, _size), rectangle.Max);
            Gizmos.DrawLine(rectangle.Max, rectangle.Max + Vector2.Scale(Vector2.left, _size));
            Gizmos.DrawLine(rectangle.Max + Vector2.Scale(Vector2.left, _size), rectangle.Min);
        }
    }
}