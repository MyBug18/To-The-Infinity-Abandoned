using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PPC_PlanetManager))]
public class PPC_PlanetManagerEditor : Editor
{
    SerializedProperty cameraProjection;
    SerializedProperty lightingMode;
    SerializedProperty polyLiquid;
    //SerializedProperty rimColoring;

    PPC_PlanetManager manager;

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        cameraProjection = serializedObject.FindProperty("cameraProjection");
        lightingMode = serializedObject.FindProperty("lightingMode");
        polyLiquid = serializedObject.FindProperty("polyLiquid");
        //rimColoring = serializedObject.FindProperty("rimColoring");

        manager = (PPC_PlanetManager)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

        EditorGUILayout.PropertyField(cameraProjection);
        EditorGUILayout.PropertyField(lightingMode);
        if (manager.lightModeIsCustom)
            EditorGUILayout.HelpBox("You can set the lighting direction on " + typeof(PPC_Planet).ToString(), MessageType.Info);
        if (manager.lightModeIsNotUnity)
            EditorGUILayout.HelpBox("Terrain shadows don't work correctly in this mode. Disable shadows in the MeshRenderer component.", MessageType.Info);
        EditorGUILayout.PropertyField(polyLiquid);
        //EditorGUILayout.PropertyField(rimColoring);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(" ");
        if (GUILayout.Button("Set Shader Keywords", EditorStyles.miniButton))
            manager.SetShaderKeywords();
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
        serializedObject.ApplyModifiedProperties();

        if (EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Exit play-mode to configure settings.", MessageType.Info);
        else
            EditorGUILayout.HelpBox("This asset needs to be in the base of a Resource folder.", MessageType.Info);
    }
}
