using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace JimPhysicsPack
{
    [CustomEditor(typeof(PolygonCollider2D))]
    public class PolygonCollider2DEditor : Editor
    {
        private PolygonCollider2D polygonCollider;

        private void OnEnable()
        {
            polygonCollider = (PolygonCollider2D)target;
            if (polygonCollider.points == null)
            {
                polygonCollider.points = new List<Vector2>();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Point"))
            {
                polygonCollider.points.Add(new Vector2());
            }
        }

        private void OnSceneGUI()
        {
            Event guiEvent = Event.current;

            // 如果按住ctrl并且点击了左键，会删除点击位置最近的点
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
            {
                float closestDistance = float.MaxValue;
                int closestIndex = -1;

                // 寻找最近的点
                for (int i = 0; i < polygonCollider.points.Count; i++)
                {
                    float distance = Vector2.Distance(polygonCollider.points[i], HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }

                if (closestIndex != -1)
                {
                    Undo.RecordObject(polygonCollider, "Remove Point");
                    polygonCollider.points.RemoveAt(closestIndex);
                    guiEvent.Use();
                }
            }

            for (int i = 0; i < polygonCollider.points.Count; i++)
            {
                Handles.color = new Color(0, 0, 1, 1f);
                EditorGUI.BeginChangeCheck();
                // 绘制一个可自由移动的Handle
                Vector2 newTargetPosition = Handles.FreeMoveHandle(polygonCollider.points[i], Quaternion.identity, 0.02f, new Vector2(0.5f, 0.5f), Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    // 记录信息，为了响应撤销
                    Undo.RecordObject(polygonCollider, "Move Point");
                    // 赋值新位置
                    polygonCollider.points[i] = newTargetPosition;
                }
            }
        }
    }
}