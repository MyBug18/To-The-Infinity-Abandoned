using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PPC_PlanetData))]
[CanEditMultipleObjects]
public class PPC_PlanetDataEditor : Editor
{
    SerializedProperty terrainColoring;
    SerializedProperty darkSide;

    SerializedProperty liquidColor;
    SerializedProperty liquidHeight;
    SerializedProperty specularColor;
    SerializedProperty specularHighlight;

    SerializedProperty coreColor;

    SerializedProperty rimColor;
    SerializedProperty rimPower;
    SerializedProperty rimOpacity;
    SerializedProperty outerRim;
    SerializedProperty outerRimRadius;
    SerializedProperty outerRimColor;
    SerializedProperty outerRimOffset;
    SerializedProperty outerRimOpacity;

    private PPC_PlanetData planetData;

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        terrainColoring = serializedObject.FindProperty("terrainColoring");
        darkSide = serializedObject.FindProperty("darkSide");

        liquidColor = serializedObject.FindProperty("liquidColor");
        liquidHeight = serializedObject.FindProperty("liquidHeight");
        specularColor = serializedObject.FindProperty("specularColor");
        specularHighlight = serializedObject.FindProperty("specularHighlight");

        coreColor = serializedObject.FindProperty("coreColor");

        rimColor = serializedObject.FindProperty("rimColor");
        rimPower = serializedObject.FindProperty("rimPower");
        rimOpacity = serializedObject.FindProperty("rimOpacity");

        outerRim = serializedObject.FindProperty("outerRim");
        outerRimRadius = serializedObject.FindProperty("outerRimRadius");
        outerRimColor = serializedObject.FindProperty("outerRimColor");
        outerRimOffset = serializedObject.FindProperty("outerRimOffset");
        outerRimOpacity = serializedObject.FindProperty("outerRimOpacity");

        planetData = (PPC_PlanetData)target;
    }

    public override void OnInspectorGUI()
    {
        if (planetData.meshVertices == null || planetData.meshVertices.Length < 10)
            EditorGUILayout.HelpBox("No mesh data detected!\nCreate a mesh with " + typeof(PPC_Planet).ToString() +
                ".\nModify the mesh with " + typeof(PPC_PlanetMeshBuilder), MessageType.Warning);
        else
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(terrainColoring);
            EditorGUILayout.PropertyField(darkSide);

            EditorGUILayout.PropertyField(liquidColor);
            EditorGUILayout.PropertyField(liquidHeight);
            EditorGUILayout.PropertyField(specularColor);
            EditorGUILayout.PropertyField(specularHighlight);

            EditorGUILayout.PropertyField(coreColor);

            EditorGUILayout.PropertyField(rimColor);
            EditorGUILayout.PropertyField(rimPower);
            EditorGUILayout.PropertyField(rimOpacity);
            EditorGUILayout.PropertyField(outerRim);
            EditorGUILayout.PropertyField(outerRimColor);
            EditorGUILayout.PropertyField(outerRimRadius);
            EditorGUILayout.PropertyField(outerRimOffset);
            EditorGUILayout.PropertyField(outerRimOpacity);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
