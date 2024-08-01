using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimDevPack.Physics
{
    public class BoxCollider2D : Collider2D
    {
        public Vector2 size = Vector3.one;

        public Vector2 center => transform.position;
        public Vector2 halfExtents => size / 2;

    }
}