using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBCollider : MonoBehaviour, ICollider
{
    public Vector3 size = Vector3.one;
    public Vector3 min => transform.position - size / 2;
    public Vector3 max => transform.position + size / 2;


    private void OnDrawGizmos()
    {
        // 绘制AABB边界
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
