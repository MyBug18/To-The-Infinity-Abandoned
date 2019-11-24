using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PPC_PlanetShaderGUI : PPC_AShaderGUI
{
    protected enum CameraProjection { Perspective, Orthographic }

    protected CameraProjection CurrentProjection(Material _targetMat)
    {
        CameraProjection projection = CameraProjection.Perspective;
        if (_targetMat.IsKeywordEnabled("_CAMERA_ORTHOGRAPHIC"))
            projection = CameraProjection.Orthographic;

        return projection;
    }

    protected enum LightingType { Unity, Central, Custom }
    protected LightingType CurrentLightingType(Material _targetMat)
    {
        LightingType projection = LightingType.Custom;
        if (_targetMat.IsKeywordEnabled("_LIGHTING_UNITY"))
            projection = LightingType.Unity;
        else if (_targetMat.IsKeywordEnabled("_LIGHTING_CENTRAL"))
            projection = LightingType.Central;

        return projection;
    }

    public override void OnGUI(MaterialEditor _editor, MaterialProperty[] _properties)
    {
        Material targetMat = _editor.target as Material;
        DoSettingsArea(targetMat, _editor, _properties);
        DoMainArea(_editor, _properties);
        DoRimArea(_editor, _properties);
    }

    protected virtual void DoSettingsArea(Material _targetMat, MaterialEditor _editor, MaterialProperty[] _properties)
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);

        CameraProjection projection = CurrentProjection(_targetMat);
        EditorGUI.BeginChangeCheck();
        projection = (CameraProjection)EditorGUILayout.EnumPopup(new GUIContent("Projection", "_Camera"), projection);
        if (EditorGUI.EndChangeCheck())
        {
            _editor.RegisterPropertyChangeUndo("Projection");
            SetKeyword(_targetMat, "_CAMERA_PERSPECTIVE", projection == CameraProjection.Perspective);
            SetKeyword(_targetMat, "_CAMERA_ORTHOGRAPHIC", projection == CameraProjection.Orthographic);
        }

        LightingType lightingType = CurrentLightingType(_targetMat);
        EditorGUI.BeginChangeCheck();
        lightingType = (LightingType)EditorGUILayout.EnumPopup(new GUIContent("Lighting", "_Lighting"), lightingType);
        if (EditorGUI.EndChangeCheck())
        {
            _editor.RegisterPropertyChangeUndo("Lighting");
            SetKeyword(_targetMat, "_LIGHTING_UNITY", lightingType == LightingType.Unity);
            SetKeyword(_targetMat, "_LIGHTING_CENTRAL", lightingType == LightingType.Central);
            SetKeyword(_targetMat, "_LIGHTING_CUSTOM", lightingType == LightingType.Custom);
        }

        if (lightingType == LightingType.Custom)
        {
            EditorGUI.indentLevel++;
            ShowShaderProperty(_editor, _properties, "Light Direction", "_LightDirection", "");
            EditorGUI.indentLevel--;
        }

        bool toggle;
        ShowToggle(_targetMat, _editor, out toggle, "Poly Liquid", "_POLYLIQUID_ON", "");
    }

    protected virtual void DoMainArea(MaterialEditor _editor, MaterialProperty[] _properties)
    {
        GUILayout.Label("Terrain", EditorStyles.boldLabel);
        ShowShaderProperty(_editor, _properties, "Terrain Coloring", "_TerrainColoring", "");
        ShowShaderProperty(_editor, _properties, "Dark Side", "_DarkSide", "");

        GUILayout.Label("Liquid", EditorStyles.boldLabel);
        ShowShaderProperty(_editor, _properties, "Color", "_LiquidColor", "");
        ShowShaderProperty(_editor, _properties, "Height", "_LiquidHeight", "");
        ShowShaderProperty(_editor, _properties, "Specular Color", "_SpecularColor", "");
        ShowShaderProperty(_editor, _properties, "Specular Highlight", "_SpecularHighlight", "");
        
        GUILayout.Label("Core", EditorStyles.boldLabel);
        ShowShaderProperty(_editor, _properties, "Color", "_CoreColor", "");
    }

    protected virtual void DoRimArea(MaterialEditor _editor, MaterialProperty[] _properties)
    {
        GUILayout.Label("Rim", EditorStyles.boldLabel);
        ShowShaderProperty(_editor, _properties, "Color", "_RimColor", "");
        ShowShaderProperty(_editor, _properties, "Power", "_RimPower", "");
        ShowShaderProperty(_editor, _properties, "Opacity", "_RimOpacity", "");
    }
}
