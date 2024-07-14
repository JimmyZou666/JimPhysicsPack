using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimPhysicsPack
{
    public class BoxCollider : MonoBehaviour, ICollider
    {
        public Vector3 size = Vector3.one;

        public Vector3 center => transform.position;
        public Vector3[] axis { get; private set; } // OBB的轴
        public Vector3 halfExtents => size / 2;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}