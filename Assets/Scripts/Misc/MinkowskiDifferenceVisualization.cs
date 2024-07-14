using System.Collections.Generic;
using UnityEngine;

namespace JimPhysicsPack
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [ExecuteInEditMode]
    public class MinkowskiDifferenceVisualization : MonoBehaviour
    {
        public PolygonCollider2D polygon1, polygon2;
        public PolygonCollider2D resultPolygon;

        public void Awake()
        {
            resultPolygon = GetComponent<PolygonCollider2D>();

            if (resultPolygon.points == null)
                resultPolygon.points = new List<Vector2>();
        }
        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (polygon1 != null && polygon2 != null && polygon1.points != null && polygon2.points != null)
                    resultPolygon.points = MinkowskiDifference(polygon1.points, polygon2.points);
            }
        }

        private List<Vector2> MinkowskiDifference(List<Vector2> polygon1, List<Vector2> polygon2)
        {
            List<Vector2> result = new List<Vector2>();

            foreach (Vector2 point1 in polygon1)
            {
                foreach (Vector2 point2 in polygon2)
                {
                    result.Add(point1 - point2);
                }
            }

            return result;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(Vector3.zero, 0.02f);
        }
    }
}