using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PPC_Planet))]
public class PPC_PlanetEditor : Editor
{
    SerializedProperty autoLoadData;

    private PPC_Planet polyPlanet;

    protected PPC_PlanetData subEditorData;
    protected Editor cachedEditor;

    private static bool editData;

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        polyPlanet = (PPC_Planet)target;

        autoLoadData = serializedObject.FindProperty("autoLoadData");
    }
    
    public override void OnInspectorGUI()
    {
        // Check if we need to get our asset's editor
        if (cachedEditor == null || subEditorData != polyPlanet.data)
        {
            subEditorData = polyPlanet.data;
            cachedEditor = CreateEditor(subEditorData);
        }

        bool regenerateAtEnd = false;

        serializedObject.Update();

        if (PPC_PlanetManager.Instance.lightModeIsCustom)
        {
            EditorGUI.BeginChangeCheck();
            Vector4 customLightDirection = EditorGUILayout.Vector4Field("Light Direction", polyPlanet.customLightDirection);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Light Direction Change");
                polyPlanet.customLightDirection = customLightDirection;
            }
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Data");
        EditorGUI.BeginChangeCheck();
        PPC_PlanetData data = EditorGUILayout.ObjectField(polyPlanet.data, typeof(PPC_PlanetData), false) as PPC_PlanetData;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Poly Planet Data Change");
            polyPlanet.data = data;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Auto Load Data");
        EditorGUILayout.PropertyField(autoLoadData, GUIContent.none, GUILayout.Width(14));
        EditorGUI.BeginDisabledGroup(polyPlanet.data == null);
        if (GUILayout.Button("Load Mesh Data", EditorStyles.miniButton))
            polyPlanet.LoadMeshData();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(" ");
        EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(14));
        EditorGUI.BeginDisabledGroup(polyPlanet.data == null);
        if (GUILayout.Button("Load Shader Data", EditorStyles.miniButton))
            polyPlanet.LoadShaderData();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        if (PPC_PlanetManager.Instance.camertaProjectionIs2D)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(14));
            if (GUILayout.Button("Fix 2D Rim Scale", EditorStyles.miniButton))
                polyPlanet.SetOrthograhicRimScale();
            EditorGUILayout.EndHorizontal();
        }

        if (polyPlanet.data == null)
        {
            EditorGUI.BeginDisabledGroup(polyPlanet.data == null);
            EditorGUILayout.Foldout(false, "Shader Data");
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            editData = EditorGUILayout.Foldout(editData, "Shader Data");
            if (editData)
            {
                EditorGUILayout.BeginVertical(GUI.skin.textArea);
                EditorGUI.BeginChangeCheck();
                if (cachedEditor != null)
                    cachedEditor.OnInspectorGUI();
                regenerateAtEnd = EditorGUI.EndChangeCheck();
                EditorGUILayout.EndVertical();
                EditorGUILayout.HelpBox("Changes are also saved in play mode.", MessageType.Info);
            }
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(" ");
        EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(14));
        if (GUILayout.Button("Go To Shader Settings", EditorStyles.miniButton))
            Selection.objects = new Object[] { PPC_PlanetManager.Instance };
        EditorGUILayout.EndHorizontal();

        if (regenerateAtEnd)
            polyPlanet.LoadShaderData();
    }
}
