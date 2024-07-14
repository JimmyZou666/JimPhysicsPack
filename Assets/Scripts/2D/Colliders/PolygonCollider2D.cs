using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimPhysicsPack
{
    public class PolygonCollider2D : Collider2D
    {
        public List<Vector2> points;

        private void OnDrawGizmos()
        {
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