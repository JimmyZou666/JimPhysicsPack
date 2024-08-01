using System.Collections.Generic;
using UnityEngine;

namespace JimDevPack.Physics
{
    public class GJKCollisionAlgorithm2D : CollisionAlgorithm2D
    {
        public override bool IsColliding(Collider2D thisCollider, Collider2D otherCollider)
        {
            PolygonCollider2D collider1 = thisCollider as PolygonCollider2D;
            PolygonCollider2D collider2 = otherCollider as PolygonCollider2D;

            // 初始化方向为任意值
            Vector2 direction = new Vector2(1, 0);

            // 计算初始支撑点
            Vector2 support = Support(collider1, collider2, direction);

            // 初始化单纯形列表并添加初始支持点
            List<Vector2> simplex = new List<Vector2> { support };

            // 反转搜索方向（当然也可以朝着原点方向）
            direction = -support;

            while (true)
            {
                // 计算新的支撑点
                support = Support(collider1, collider2, direction);

                // 如果新的支撑点在搜索方向上没有超过原点，那么两个形状不相交
                // 因为支撑点一定是在搜索方向上最远的点，这个最远的点都没有超过原点，肯定整体也不会超过
                // 在某方向d下点1超过点2的意思：点1在d方向上的投影位置大于点2的投影位置
                // 这里的Vector2.Dot(support, direction)实际是Vector2.Dot(support - origin, direction)，因为原点origin是零向量，所以省略掉了。目的就是判断direction方向下，支撑点是在原点的哪一侧。
                if (Vector2.Dot(support, direction) < 0)
                {
                    return false;
                }

                simplex.Add(support);

                // 检查单纯形是否包含原点，同时更新下一个搜索的方向
                if (ContainsOrigin(simplex, ref direction))
                {
                    // 如果包含，那么两个形状相交
                    return true;
                }
            }
        }

        // 计算支持点
        private Vector2 Support(PolygonCollider2D collider1, PolygonCollider2D collider2, Vector2 direction)
        {
            Vector2 p1 = FarthestPointInDirection(collider1, direction);
            Vector2 p2 = FarthestPointInDirection(collider2, -direction);
            // 图形1在方向上最远的点减去图形2在反方向上最远的点
            return p1 - p2;
        }

        // 计算在给定方向上最远的点
        private Vector2 FarthestPointInDirection(PolygonCollider2D collider, Vector2 direction)
        {
            float maxDot = float.NegativeInfinity;
            Vector2 farthestPoint = Vector2.zero;

            foreach (Vector2 point in collider.points)
            {
                float dot = Vector2.Dot(point, direction);

                // direciton方向上最远的点，必然是和direciton点乘后值最大的点。
                if (dot > maxDot)
                {
                    maxDot = dot;
                    farthestPoint = point;
                }
            }

            return farthestPoint;
        }

        /// <summary>
        /// 目的：
        /// <br>（1）检查单纯形是否包含原点</br>
        /// <br>（2）更新搜索方向</br>
        /// <br></br>
        /// <br>注：每次在这个函数执行完之后，要么就是找到目标，终止整个上层的循环，结束。要么就是会删除一个点，继续从下一个方向开始寻找。所以始终会让simplex维持在三个点（除了最开始寻找的时候，只有两个点）。</br>
        /// </summary>
        /// <param name="simplex">每次在这个函数执行完之后，要么就是找到目标，终止整个上层的循环，结束。要么就是会删除一个点，继续从下一个方向开始寻找。所以始终会让simplex维持在三个点（除了最开始寻找的时候，只有两个点）。</param>
        /// <param name="direction">传入时是当前正在准备搜索的方向，传出时是下一个要搜索的方向</param>
        /// <returns></returns>
        private bool ContainsOrigin(List<Vector2> simplex, ref Vector2 direction)
        {
            // 获取单纯形的最后一个点，这是刚刚添加的点，记为a
            Vector2 a = simplex[simplex.Count - 1];

            // 计算从最后添加的点到原点的向量
            Vector2 ao = -a;

            // 单纯形是一个线段，我们需要检查原点是否在线段的右侧或左侧
            if (simplex.Count == 2)
            {
                // 获取单纯形的另一个点
                Vector2 b = simplex[0];

                // 计算从b到a的向量
                Vector2 ab = b - a;

                // ab × ao × ab，可以直接得到垂直ab，且朝向o这一侧的向量，三重积的好处是也可以让这个垂直关系推广到三维
                direction = TripleProduct(ab, ao, ab);

                return false;
            }
            // 单纯形是一个三角形，我们需要检查原点是否在三角形内
            else if (simplex.Count == 3)
            {
                // 获取单纯形的其他两个点
                Vector2 b = simplex[1];
                Vector2 c = simplex[0];

                // 计算从a到b和从a到c的向量
                Vector2 ab = b - a;
                Vector2 ac = c - a;

                // 计算垂直于ab和ac的向量
                Vector2 abPerp = TripleProduct(ac, ab, ab);
                Vector2 acPerp = TripleProduct(ab, ac, ac);

                // 如果原点在三角形外，且是在ab那边的外侧，下次就需要在ab的方向上搜索
                if (Vector2.Dot(abPerp, ao) > 0)
                {
                    // 就把c移除，从ab方向探索，重新构建三角形
                    simplex.Remove(c);
                    direction = abPerp;
                    return false;
                }
                // 如果原点在三角形外，且是在ac那边的外侧，下次就需要在ac的方向上搜索
                else if (Vector2.Dot(acPerp, ao) > 0)
                {
                    // 就把b移除，从ac方向探索，重新构建三角形
                    simplex.Remove(b);
                    direction = acPerp;
                    return false;
                }
                // 如果都没命中，那么一定在三角形内
                else
                {
                    return true;
                }
            }
            return false;
        }

        // 计算三重积，这是一个方便的函数，用于计算垂直于两个向量的向量
        private Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            // 等价于a × b × c
            return b * Vector2.Dot(a, c) - a * Vector2.Dot(b, c);
        }
    }
}