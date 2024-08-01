using System.Collections.Generic;
using UnityEngine;
using JimDevPack.Geometry;
using System.Linq;

namespace JimDevPack.Physics
{
    public class PolygonCollider2D : Collider2D
    {
        public List<Vector2> points
        {
            get
            {
                Vector2 transformPosition = transform.position;
                return localPoints.Select(p => p + transformPosition).ToList();
            }
            set
            {
                Vector2 transformPosition = transform.position;
                localPoints = value.Select(p => p - transformPosition).ToList();
            }
        }

        public List<Vector2> localPoints;

        private void OnDrawGizmos()
        {
            if (!ConvexHullHelper.IsConvex(points))
                Gizmos.color = Color.magenta;
            else
                Gizmos.color = Color.green;

            if (points.Count > 1)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }
                Gizmos.DrawLine(points[points.Count - 1], points[0]);
            }
        }
    }

}