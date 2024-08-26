using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JimDevPack.Physics;

public class QuadTreeRenderer : MonoBehaviour
{
    public JimDevPack.Physics.Collider2D[] colliders;

    public int size;
    private QuadTree quadTree;
    void Start()
    {
        // 创建一棵树
        quadTree = new QuadTree(Vector2.zero,size,3);

        // 插入所有节点
        for(int i=0; i< colliders.Length; i++)
        {
            if(colliders[i].gameObject.activeSelf)
                quadTree.Insert(colliders[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size,size,0));

        if (Application.isPlaying)
            DrawRecursively(quadTree.rootNode);
    }

    public void DrawRecursively(QuadTreeNode node)
    {
        // 绘制当前节点的边界框
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(node.center, node.size * Vector2.one);

        // 如果当前节点被划分为子节点,递归绘制子节点
        if (node.isDivided)
        {
            foreach (var child in node.children)
            {
                DrawRecursively(child);
            }
        }
/*        // 否则绘制当前节点包含的碰撞体
        else
        {
            Gizmos.color = Color.yellow;
            foreach (var collider in node.objects)
            {
                Gizmos.DrawWireCube(collider.transform.position, collider.size);
            }
        }*/
    }
}
