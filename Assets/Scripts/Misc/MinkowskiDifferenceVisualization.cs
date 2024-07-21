using JimDevPack.Geometry;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JimPhysicsPack
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [ExecuteInEditMode]
    public class MinkowskiDifferenceVisualization : MonoBehaviour
    {
        public PolygonCollider2D polygon1, polygon2;
        public List<Vector2> resultPointsSet;
        public PolygonCollider2D hullPolygon;

        public void Awake()
        {
            hullPolygon = GetComponent<PolygonCollider2D>();

            if (hullPolygon.points == null)
                hullPolygon.points = new List<Vector2>();
        }
        private void Update()
        {
            if (polygon1 != null && polygon2 != null && polygon1.points != null && polygon2.points != null)
            {
                resultPointsSet = MinkowskiDifference(polygon1.points, polygon2.points);
                hullPolygon.points = ConvexHullHelper.CalculateConvexHullByGraham(resultPointsSet);
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
            Handles.Label(new Vector2(0.02f, 0.02f), "原点");

            Gizmos.color = Color.yellow;
            foreach (Vector2 point in resultPointsSet)
            {
                Gizmos.DrawSphere(point, 0.015f);
            }

            Gizmos.color = Color.yellow;
            var hullPoints = hullPolygon.points;
            if (hullPoints.Count > 1)
            {
                for (int i = 0; i < hullPoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(hullPoints[i], hullPoints[i + 1]);
                }
                Gizmos.DrawLine(hullPoints.Last(), hullPoints[0]);
            }
        }
    }
}