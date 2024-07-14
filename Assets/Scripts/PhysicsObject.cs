﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimPhysicsPack
{
    public class PhysicsObject : MonoBehaviour
    {
        [SerializeField] public new ICollider collider;

        public bool isColliding = false;

        private void Start()
        {
            collider = GetComponent<ICollider>();

            PhysicsWorld.allPhysicsObjects.Add(this);
        }

        private void OnDestroy()
        {
            PhysicsWorld.allPhysicsObjects.Remove(this);
        }

        private void FixedUpdate()
        {
            if (IsColliding())
            {
                isColliding = true;
            }
            else
            {
                isColliding = false;
            }
        }

        // 检测这个物体是否与任何其他物体发生碰撞
        public bool IsColliding()
        {
            // @improve:目前为直接遍历，需要进行空间优化
            foreach (PhysicsObject otherObj in PhysicsWorld.allPhysicsObjects)
            {
                ICollisionAlgorithm collisionAlgorithm = null;
                // 切换算法

                // 如果都是方形碰撞盒
                if (collider is BoxCollider && otherObj.collider is BoxCollider)
                {
                    // 如果都是轴对齐盒子，那就用最简单的AABB检测算法
                    if (collider is AABBCollider && otherObj.collider is AABBCollider)
                        collisionAlgorithm = new AABBCollisionAlgorithm();
                    // 如果存在有一个不是轴对齐盒子，则用OBB检测算法
                    //else
                    //collisionAlgorithm = new OBBCollisionAlgorithm();
                }

                // 检测碰撞
                if (otherObj != this && collisionAlgorithm != null && collisionAlgorithm.IsColliding(collider, otherObj.collider))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (isColliding)
            {
                Gizmos.color = Color.red;
                if (collider is AABBCollider)
                {
                    AABBCollider c = (AABBCollider)collider;
                    Gizmos.DrawWireCube(c.transform.position, c.size);
                }
            }
        }
    }
}