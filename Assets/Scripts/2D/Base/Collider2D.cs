using UnityEngine;

namespace JimDevPack.Physics
{
    public class Collider2D : MonoBehaviour
    {
        public virtual (Vector2 min, Vector2 max) GetAABB()
        {
            return (Vector2.zero, Vector2.zero);
        }

        protected void OnDrawGizmos()
        {
            // 绘制AABB边界
            var (min,max) = GetAABB();
            Gizmos.color = Color.yellow;
            Vector2 pos = (max + min) / 2;
            Vector2 size = max - min;
            Gizmos.DrawWireCube(pos, size);
        }
    }
}