using UnityEngine;

namespace JimDevPack.Physics
{
    public class BoxCollider : Collider
    {
        public Vector3 size = Vector3.one;

        public Vector3 center => transform.position;
        public Vector3 halfExtents => size / 2;
    }
}