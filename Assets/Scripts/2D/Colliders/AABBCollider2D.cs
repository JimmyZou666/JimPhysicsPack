using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimDevPack.Physics
{
    public class AABBCollider2D : BoxCollider2D
    {
        public Vector2 min => (Vector2)transform.position - size / 2;
        public Vector2 max => (Vector2)transform.position + size / 2;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        private void OnDrawGizmos()
        {
            // 绘制AABB边界
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}