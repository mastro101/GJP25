using Eastermaster.Handle;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Eastermaster
{
    [CustomEditor(typeof(ViewTrigger))]
    public class ViewTriggerEditor : Editor
    {
        SerializedProperty radius;
        SerializedProperty viewAngle;

        ViewTrigger c;
        Vector3 handlePos;

        private void OnEnable()
        {
            c = (ViewTrigger)target;

            radius = serializedObject.FindProperty("radius");
            viewAngle = serializedObject.FindProperty("viewAngle");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            radius.floatValue = EditorGUILayout.FloatField(radius.displayName, radius.floatValue);
            viewAngle.floatValue = EditorGUILayout.Slider(viewAngle.displayName, viewAngle.floatValue, 0f, 360f);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            Vector3 arcSideDir = Quaternion.AngleAxis(-viewAngle.floatValue / 2f, c.transform.up) * c.transform.forward;
            Vector3 newHandlePos = c.transform.position + arcSideDir * radius.floatValue;

            handlePos = Handles.PositionHandle(newHandlePos, c.transform.rotation);
            Vector3 localHandlePos = handlePos - c.transform.position;


            radius.floatValue = localHandlePos.magnitude;
            viewAngle.floatValue = Vector3.Angle(c.transform.forward, localHandlePos) * 2f;

            serializedObject.ApplyModifiedProperties();

            Handles.color = Color.white;

            HandlesUtility.DrawArc3D(viewAngle.floatValue, radius.floatValue, c.transform.position, c.transform.up, c.transform.forward);

            IDetectable[] detectables = c.GetDetectedObjects();
            foreach ( IDetectable detectable in detectables)
            {
                Handles.DrawLine(c.transform.position, detectable.transform.position);
            }
        }
    } 
}