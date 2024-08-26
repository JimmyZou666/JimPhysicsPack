using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimDevPack.Physics
{
    public class QuadTree
    {
        public QuadTreeNode rootNode { get; private set; }

        private int maxDepth;   // 四叉树的最大深度

        public QuadTree(Vector2 center, float size,int maxDepth = 5)
        {
            this.maxDepth = maxDepth;
            rootNode = new QuadTreeNode(center, size, this.maxDepth, 0);
        }

        public void Insert(Collider2D obj)
        {
            rootNode.Insert(obj);
        }

        public Collider2D[] FindArea(Rect rect)
        {
            return null;
        }
    }

    public class QuadTreeNode
    {
        public Vector2 center;
        public float size;
        public List<Collider2D> objects;
        public QuadTreeNode[] children; // 顺序 左上、左下、右下、右上

        public bool isDivided => children != null;
        private int depth;
        private int maxDepth;

        public QuadTreeNode(Vector2 center, float size, int maxDepth, int depth)
        {
            this.center = center;
            this.size = size;

            this.maxDepth = maxDepth;
            this.depth = depth;

            objects = new List<Collider2D>();
        }

        public void Insert(Collider2D obj)
        {
            // 如果物体不处于该区域中，则返回
            if (!Overlaps(obj))
            {
                return;
            }

            // 如果处于该区域中，则执行插入

            // 如果未划分，且当前节点的层级未到最大划分层级，则划分
            if (!isDivided && depth <= maxDepth)
            {
                children = new QuadTreeNode[4];
                float len = size / 2; // 子区域的边长

                children[0] = new QuadTreeNode(center + new Vector2(-1, 1) * len / 2, len, maxDepth, depth + 1);
                children[1] = new QuadTreeNode(center + new Vector2(-1, -1) * len / 2, len, maxDepth, depth + 1);
                children[2] = new QuadTreeNode(center + new Vector2(1, -1) * len / 2, len, maxDepth, depth + 1);
                children[3] = new QuadTreeNode(center + new Vector2(1, 1) * len / 2, len, maxDepth, depth + 1);
            }


            // 以下两种情况会让对象存到自身维护的对象列表中 （1）如果压了子区域的分割线 （2）深度到达最深的一层，不能再往下划分了

            // 存入自身维护的对象列表中
            if (CrossSplitLine(obj) || depth == maxDepth)
            {
                if(!objects.Contains(obj))
                    objects.Add(obj);
            }
            // 存入子区域维护的对象列表中
            else
            {
                foreach (QuadTreeNode child in children)
                {
                    child.Insert(obj);
                }
            }
        }

        /// <summary>
        /// 判断物体obj和当前区域是否有重叠
        /// </summary>
        public bool Overlaps(Collider2D obj)
        {
            var (objMin, objMax) = obj.GetAABB();
            var (boundMin, boundMax) = (center - Vector2.one * size / 2, center + Vector2.one * size / 2);

            if (objMin.x <= boundMax.x && objMax.x >= boundMin.x &&
                   objMin.y <= boundMax.y && objMax.y >= boundMin.y)
                return true;

            return false;
        }

        /// <summary>
        /// 判断物体obj和子区域的十字分割线中的任意一根是否有交叉
        /// </summary>
        public bool CrossSplitLine(Collider2D obj)
        {
            // AABB的x和y的任意一个轴上，如果跨过了0点，则代表压线
            var (min, max) = obj.GetAABB();
            if((min.x < center.x && max.x> center.x) || (min.y< center.y && max.y> center.y))
                return true;

            return false;
        }
    }
}