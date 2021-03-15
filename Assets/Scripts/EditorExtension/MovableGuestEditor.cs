using UnityEngine;
using Adventure;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EditorExtension
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MovableGuest))]
    public class MovableGuestEditor : Editor {

        public void OnSceneGUI () {
            serializedObject.Update();
            SerializedProperty topRightCornerProp = serializedObject.FindProperty("topRightCorner");
            SerializedProperty lowerLeftCornerProp = serializedObject.FindProperty("lowerLeftCorner");
            MovableGuest movableGuest = (MovableGuest) target;
            
            EditorGUI.BeginChangeCheck();
            Vector3 topRightCorner = Handles.PositionHandle(topRightCornerProp.vector2Value, Quaternion.identity);
            Vector3 lowerLeftCorner = Handles.PositionHandle(lowerLeftCornerProp.vector2Value, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                topRightCornerProp.vector2Value = topRightCorner;
                lowerLeftCornerProp.vector2Value = lowerLeftCorner;
                serializedObject.ApplyModifiedProperties();
            }

            Vector3 currentPosition = movableGuest.transform.position;
            
            Vector3[] bounds = {
                new Vector3(currentPosition.x, currentPosition.x),
                new Vector3(currentPosition.y, currentPosition.y)
            };
            
            bounds[0].x = Mathf.Min(bounds[0].x, topRightCorner.x);
            bounds[0].y = Mathf.Max(bounds[0].y, topRightCorner.x);
            bounds[1].x = Mathf.Min(bounds[1].x, topRightCorner.y);
            bounds[1].y = Mathf.Max(bounds[1].y, topRightCorner.y);
            
            bounds[0].x = Mathf.Min(bounds[0].x, lowerLeftCorner.x);
            bounds[0].y = Mathf.Max(bounds[0].y, lowerLeftCorner.x);
            bounds[1].x = Mathf.Min(bounds[1].x, lowerLeftCorner.y);
            bounds[1].y = Mathf.Max(bounds[1].y, lowerLeftCorner.y);
            
            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(bounds[0].x, bounds[1].x, 0), new Vector3(bounds[0].x, bounds[1].y, 0));
            Handles.DrawLine(new Vector3(bounds[0].y, bounds[1].x, 0), new Vector3(bounds[0].y, bounds[1].y, 0));
            Handles.DrawLine(new Vector3(bounds[0].x, bounds[1].x, 0), new Vector3(bounds[0].y, bounds[1].x, 0));
            Handles.DrawLine(new Vector3(bounds[0].x, bounds[1].y, 0), new Vector3(bounds[0].y, bounds[1].y, 0));
        }
    }
#endif
}