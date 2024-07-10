using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] public new ICollider collider;
    public ICollisionAlgorithm collisionAlgorithm;

    public bool isColliding = false;

    private void Start()
    {
        collider = GetComponent<ICollider>();

        if (collider is AABBCollider)
        {
            collisionAlgorithm = new AABBCollisionAlgorithm();
        }
        /*else if (collider is XXCollider)
        {
            collisionAlgorithm = new XXCollisionAlgorithm():
        }*/

        PhysicsManager.allPhysicsObjects.Add(this);
    }

    private void OnDestroy()
    {
        PhysicsManager.allPhysicsObjects.Remove(this);
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
        // @improve:目前为直接遍历，需要优化
        foreach (PhysicsObject obj in PhysicsManager.allPhysicsObjects)
        {
            if (obj != this && collisionAlgorithm.IsColliding(collider, obj.collider))
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
