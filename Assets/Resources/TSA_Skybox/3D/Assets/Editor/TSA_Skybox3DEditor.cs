using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TSA_Skybox3D))]
public class TSA_Skybox3DEditor : Editor
{
    SerializedProperty front, back, left, right, up, down;

    private TSA_Skybox3D skybox;

    private readonly GUIContent meshGUIContent = new GUIContent("Create Mesh", "On Start: Mesh gets created automatically if it is null.");

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        front = serializedObject.FindProperty("front");
        back = serializedObject.FindProperty("back");
        left = serializedObject.FindProperty("left");
        right = serializedObject.FindProperty("right");
        up = serializedObject.FindProperty("up");
        down = serializedObject.FindProperty("down");

        skybox = (TSA_Skybox3D)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button(meshGUIContent, EditorStyles.miniButton))
            skybox.CreateMesh();
        if (GUILayout.Button("Apply Textures"))
            skybox.ApplyTextures();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(front);
        EditorGUILayout.PropertyField(back);
        EditorGUILayout.PropertyField(left);
        EditorGUILayout.PropertyField(right);
        EditorGUILayout.PropertyField(up);
        EditorGUILayout.PropertyField(down);

        serializedObject.ApplyModifiedProperties();
    }
}
