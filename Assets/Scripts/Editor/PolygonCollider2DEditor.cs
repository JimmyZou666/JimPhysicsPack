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

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
            {
                float closestDistance = float.MaxValue;
                int closestIndex = -1;

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
                Vector2 newTargetPosition = Handles.FreeMoveHandle(polygonCollider.points[i], Quaternion.identity, 0.03f, new Vector2(0.5f, 0.5f), Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(polygonCollider, "Move Point");
                    polygonCollider.points[i] = newTargetPosition;
                }
            }
        }
    }
}