using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JimDevPack.Geometry
{
    public enum AngleOrientation
    {
        Collinear, // 共线
        Clockwise, // 顺时针
        Counterclockwise // 逆时针
    }

    public static class ConvexHullHelper
    {
        /// <summary>
        /// 基于Graham的凸包计算方法
        /// </summary>
        /// <param name="points">分散的点集</param>
        /// <returns>包围points的最小凸包的点集</returns>
        public static List<Vector2> CalculateConvexHullByGraham(List<Vector2> points)
        {
            if (points.Count < 3)
            {
                Debug.LogError("点的数量必须大于等于3");
                return null;
            }


            // 找到最下面的点作为初始基点，如果有多个点具有相同的最小 y 坐标，则选择最左边的点
            Vector2 startPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].y < startPoint.y || (points[i].y == startPoint.y && points[i].x < startPoint.x))
                {
                    startPoint = points[i];
                }
            }

            // 将起始点移动到点集的第一个位置
            points.Remove(startPoint);
            points.Insert(0, startPoint);

            List<Vector2> sortedPoints = new List<Vector2>(points);

            // 根据起始点到其他点的极角进行排序
            sortedPoints.Sort((a, b) =>
            {
                float angleA = GetPolarAngle(startPoint, a);
                float angleB = GetPolarAngle(startPoint, b);
                // 比较极角
                if (angleA < angleB)
                    return -1;
                if (angleA > angleB)
                    return 1;

                // 极角相同则按距离近的来
                if (angleA == angleB)
                {
                    float distA = Vector2.Distance(a, startPoint);
                    float distB = Vector2.Distance(b, startPoint);
                    if (distA < distB)
                        return -1;
                    if (distA > distB)
                        return 1;
                }

                return 0;
            });

            // 创建一个栈，用来存储凸包上的点
            Stack<Vector2> hull = new Stack<Vector2>();
            hull.Push(sortedPoints[0]);
            hull.Push(sortedPoints[1]);

            // Graham扫描算法
            for (int i = 2; i < sortedPoints.Count; i++)
            {
                Vector2 top = hull.Pop();
                // 如果当前点不在上一个点和栈顶点的左侧（逆时针侧），那么上一个点存在凹陷或是共线，将其从栈中移除
                while (hull.Count != 0 && GetOrientation(hull.Peek(), top, sortedPoints[i]) != AngleOrientation.Counterclockwise)
                {
                    // 删除凹陷点，继续进入下个循环往前判断
                    top = hull.Pop();
                }
                // 将上一个点和当前点都添加到栈中
                hull.Push(top);
                hull.Push(sortedPoints[i]);
            }

            // 返回包含凸包上所有点的列表
            return hull.ToList();
        }

        /// <summary>
        /// 计算给定的多边形边缘上的点，判断该多边形是否为凸多边形
        /// </summary>
        public static bool IsConvex(List<Vector2> points)
        {
            // 点的数量必须大于等于3
            if (points.Count < 3)
            {
                return false;
            }

            int n = points.Count;
            AngleOrientation prevOrientation = AngleOrientation.Collinear;

            for (int i = 0; i < n; i++)
            {
                // 计算当前点、下一个点和下下个点构成的角的旋转方向
                AngleOrientation orientation = GetOrientation(points[i], points[(i + 1) % n], points[(i + 2) % n]);

                // 如果三个点共线，跳过当前循环，不用记录到prevOrientation中
                if (orientation == AngleOrientation.Collinear)
                    continue;

                // 如果之前的方向是共线，更新之前的方向为当前方向
                if (prevOrientation == AngleOrientation.Collinear)
                    prevOrientation = orientation;
                // 如果之前的方向和当前方向不一致，返回false
                else if (prevOrientation != orientation)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 计算三个点形成的夹角的类型
        /// a-->b-->c</summary>
        private static AngleOrientation GetOrientation(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;
            float val = ab.x * bc.y - ab.y * bc.x;

            if (val == 0)
                return AngleOrientation.Collinear; // 共线
            else if (val > 0)
                return AngleOrientation.Counterclockwise; // 逆时针
            else
                return AngleOrientation.Clockwise; // 顺时针
        }

        /// <summary>
        /// 计算point相对于origin的极角
        /// </summary>
        private static float GetPolarAngle(Vector2 origin, Vector2 point)
        {
            return Mathf.Atan2(point.y - origin.y, point.x - origin.x);
        }
    }

}