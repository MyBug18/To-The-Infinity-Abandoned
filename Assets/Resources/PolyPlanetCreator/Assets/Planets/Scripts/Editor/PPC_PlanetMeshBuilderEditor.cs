using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PPC_PlanetMeshBuilder))]
public class PPC_PlanetMeshBuilderEditor : Editor
{
    SerializedProperty autoSaveData;

    SerializedProperty brushModeKey;
    SerializedProperty brushPower;
    SerializedProperty maxHeight;
    SerializedProperty limitDepth1;

    SerializedProperty colorNoise;
    SerializedProperty monotone;
    
    SerializedProperty scale;
    SerializedProperty seed;

    SerializedProperty gizmoScale;

    SerializedProperty brushModeTextUI;
    SerializedProperty levelBrushTextUI;
    SerializedProperty colorPicker;

    SerializedProperty detail;

    private PPC_PlanetMeshBuilder builder;

    private static bool terrainFoldout, noiseFoldout, sampleFoldout;
    private static bool settingsFoldout;
    private static bool UIFoldout;
    private static bool sphereGenerationFoldout;
    private static bool coloringFoldout;
    private static bool brushInfoFoldout;

    private bool NotPlayingOrNoMesh { get { return !EditorApplication.isPlaying || builder.MeshF.mesh == null; } }

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }
        
        autoSaveData = serializedObject.FindProperty("autoSaveData");

        brushModeKey = serializedObject.FindProperty("brushModeKey");
        brushPower = serializedObject.FindProperty("brushPower");
        maxHeight = serializedObject.FindProperty("maxHeight");
        limitDepth1 = serializedObject.FindProperty("limitDepth1");

        colorNoise = serializedObject.FindProperty("colorNoise");
        monotone = serializedObject.FindProperty("monotone");
        
        scale = serializedObject.FindProperty("scale");
        seed = serializedObject.FindProperty("seed");

        gizmoScale = serializedObject.FindProperty("gizmoScale");
        
        brushModeTextUI = serializedObject.FindProperty("brushModeTextUI");
        levelBrushTextUI = serializedObject.FindProperty("levelBrushTextUI");
        colorPicker = serializedObject.FindProperty("colorPicker");

        detail = serializedObject.FindProperty("detail");

        builder = (PPC_PlanetMeshBuilder)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Auto Save Data");
        EditorGUILayout.PropertyField(autoSaveData, GUIContent.none, GUILayout.MaxWidth(14));
        EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh || builder.PolyPlanet.data == null);
        if (GUILayout.Button(new GUIContent("Save Mesh Data", "Available in play-mode."), EditorStyles.miniButton))
            builder.SaveCurrentMeshDataToScriptableObject();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        terrainFoldout = EditorGUILayout.Foldout(terrainFoldout, "Terrain", true);
        if (terrainFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(maxHeight);
            EditorGUILayout.PropertyField(limitDepth1);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            if (GUILayout.Button(new GUIContent("Get Mesh Max Height", "Sets 'maxHeight' to the current mesh's maximum vertex distance.")))
                builder.GetMaxCurrentMaxHeight();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh);
            EditorGUILayout.PrefixLabel(" ");
            if (GUILayout.Button(new GUIContent("Set Height To Max Height", "Sets all vertex distances to 'maxHeight'. Available in play-mode.")))
                builder.SetHeightToMaxHeight();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            //perlin
            noiseFoldout = EditorGUILayout.Foldout(noiseFoldout, "Perlin Noise", true);
            if (noiseFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(scale);
                EditorGUILayout.PropertyField(seed);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(" ");
                EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh);
                if (GUILayout.Button(new GUIContent("Generate Terrain", "Available in play-mode."), EditorStyles.miniButton))
                    (target as PPC_PlanetMeshBuilder).ApplyNoiseToMesh();
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            // sample texture
            sampleFoldout = EditorGUILayout.Foldout(sampleFoldout, "Sample Texture", true);
            if (sampleFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh);
                builder.sampleTexture = EditorGUILayout.ObjectField(builder.sampleTexture, typeof(Texture2D), false) as Texture2D;
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.HelpBox("Make sure the texture is readable.", MessageType.Info);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        coloringFoldout = EditorGUILayout.Foldout(coloringFoldout, "Coloring", true);
        if (coloringFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            Color selectedColor = EditorGUILayout.ColorField(builder.SelectedColor);
            EditorGUILayout.PropertyField(colorNoise);
            EditorGUILayout.PropertyField(monotone);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Selected Color Change");
                builder.SelectedColor = selectedColor;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh);
            if (GUILayout.Button(new GUIContent("Color Terrain", "Available in play-mode."), EditorStyles.miniButton))
                builder.ColorPlanetWithCurrentColor();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }
        
        sphereGenerationFoldout = EditorGUILayout.Foldout(sphereGenerationFoldout, "Sphere Generation", true);
        if (sphereGenerationFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(detail);
            EditorGUILayout.HelpBox("Mesh triangles on generation: " + (detail.intValue * detail.intValue * 20).ToString(), MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EditorGUI.BeginDisabledGroup(NotPlayingOrNoMesh);
            if (GUILayout.Button(new GUIContent("Generate Sphere", "Generates an Icosahedron-based sphere. Available in play-mode."), EditorStyles.miniButton))
            {
                PPC_IcosahedronGeneratorNoUV.ApplyMesh(builder.GetComponent<PPC_Planet>().GetComponent<MeshFilter>(), PPC_IcosahedronGeneratorNoUV.Generate(detail.intValue));
                if (autoSaveData.boolValue)
                    builder.SaveCurrentMeshDataToScriptableObject();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        settingsFoldout = EditorGUILayout.Foldout(settingsFoldout, "Settings", true);
        if (settingsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(brushPower);
            EditorGUILayout.PropertyField(brushModeKey);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        UIFoldout = EditorGUILayout.Foldout(UIFoldout, "UI / Editor", true);
        if (UIFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(brushModeTextUI);
            EditorGUILayout.PropertyField(levelBrushTextUI);
            EditorGUILayout.PropertyField(colorPicker);
            EditorGUILayout.PropertyField(gizmoScale);
            EditorGUI.indentLevel--;
        }

        brushInfoFoldout = EditorGUILayout.Foldout(brushInfoFoldout, "Controls & Brush Info", true);
        if (brushInfoFoldout)
        {
            EditorGUILayout.HelpBox(
                "KEYBOARD/MOUSE CONTROLS:\n" +
                "Left Mouse - Primary brush function\n" +
                "Right Mouse - Secondary brush function\n" +
                "Left Control - Toggle singleton brush\n" +
                "Alpha 1, 2, 3, 4 - Brush mode switching",
                MessageType.Info);
            EditorGUILayout.HelpBox(
                "MODIFY BRUSH\n" +
                "Primary - Increase vertex height (expand)\n" +
                "Secondary - Decrease vertex height (erode)", 
                MessageType.Info);
            EditorGUILayout.HelpBox(
                "SMOOTH BRUSH\n" +
                "Primary - Slowly smooth center vertex\n" +
                "Secondary - Fully smooth center vertex",
                MessageType.Info);
            EditorGUILayout.HelpBox(
                "LEVEL BRUSH\n" +
                "Primary - Level terrain (plateau)\n" +
                "Secondary - Set level height to center vertex height",
                MessageType.Info);
            EditorGUILayout.HelpBox(
                "COLOR BRUSH\n" +
                "Primary - Color vertex\n" +
                "Secondary - Get color of center vertex",
                MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
