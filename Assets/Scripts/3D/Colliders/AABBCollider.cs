﻿using UnityEngine;

namespace JimDevPack.Physics
{
    public class AABBCollider : BoxCollider
    {
        public Vector3 min => transform.position - size / 2;
        public Vector3 max => transform.position + size / 2;

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