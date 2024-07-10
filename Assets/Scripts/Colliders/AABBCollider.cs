using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBCollider : MonoBehaviour, ICollider
{
    public Vector3 min = Vector3.one * -1;
    public Vector3 max = Vector3.one;

    private void OnDrawGizmos()
    {
        // 绘制AABB边界
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, max - min);
    }
}
