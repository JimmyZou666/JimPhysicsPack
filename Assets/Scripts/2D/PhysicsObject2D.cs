using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimPhysicsPack
{
    public class PhysicsObject2D : MonoBehaviour
    {
        [SerializeField] public new Collider2D collider;

        public bool isColliding = false;

        private void Start()
        {
            collider = GetComponent<Collider2D>();

            PhysicsWorld2D.allPhysicsObjects.Add(this);
        }

        private void OnDestroy()
        {
            PhysicsWorld2D.allPhysicsObjects.Remove(this);
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
            foreach (PhysicsObject2D otherObj in PhysicsWorld2D.allPhysicsObjects)
            {
                CollisionAlgorithm2D collisionAlgorithm = null;
                // 切换算法

                // 如果都是方形碰撞盒
                if (collider is BoxCollider2D && otherObj.collider is BoxCollider2D)
                {
                    // 如果都是轴对齐盒子，那就用最简单的AABB检测算法
                    if (collider is AABBCollider2D && otherObj.collider is AABBCollider2D)
                        collisionAlgorithm = new AABBCollisionAlgorithm2D();
                    // 如果存在有一个不是轴对齐盒子，则用OBB检测算法
                    //else
                    //collisionAlgorithm = new OBBCollisionAlgorithm();
                }

                if (collider is PolygonCollider2D && otherObj.collider is PolygonCollider2D)
                {
                    collisionAlgorithm = new GJKCollisionAlgorithm2D();
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
                if (collider is AABBCollider2D)
                {
                    AABBCollider2D c = (AABBCollider2D)collider;
                    Gizmos.DrawWireCube(c.transform.position, c.size);
                }
                else if (collider is PolygonCollider2D)
                {
                    PolygonCollider2D c = (PolygonCollider2D)collider;
                    List<Vector2> points = c.points;
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
    }
}