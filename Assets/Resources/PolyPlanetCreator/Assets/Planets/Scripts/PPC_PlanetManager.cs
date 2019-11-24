using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class PPC_PlanetManager : PPC_AScriptableObjectInstance<PPC_PlanetManager>
{
#if UNITY_EDITOR
    private readonly string planetMaterialName = "PPC_Planet.mat";
    private readonly string planetWRimMaterialName = "PPC_PlanetWRim.mat";

    private readonly string planetShaderPath = "_TS/PPC/Planet";
    private readonly string planetWRimShaderName = "_TS/PPC/Planet Outer Rim";

    private void OnEnable()
    {
        if (planetMaterial == null || planetWRimMaterial == null)
            SetReferences();
    }

    /// <summary>
    /// Creates and sets up materials for planets with rim and without.
    /// </summary>
    public void SetReferences()
    {
        SetMaterialReference(planetMaterialName, out m_planetMaterial, planetShaderPath);
        SetMaterialReference(planetWRimMaterialName, out m_planetWRimMaterial, planetWRimShaderName);
    }

    private void SetMaterialReference(string _materialName, out Material _material, string _shaderPath)
    {
        Debug.Log("Searching for '" + _materialName + "' material...");
        _material = null;

        int extensionLength = ".mat".Length;

        string[] assetCandidates = AssetDatabase.FindAssets(_materialName.Substring(0, _materialName.Length - extensionLength));
        if (assetCandidates.Length != 0)
        {
            foreach (string stringItem in assetCandidates)
            {
                if (AssetDatabase.GUIDToAssetPath(stringItem).Contains(_materialName))
                {
                    _material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(stringItem));
                    Debug.Log("-> Material found!");
                    break;
                }
            }
        }

        if (_material == null)
        {
            string path = "Assets/PolyPlanetCreator/" + _materialName;
            Debug.Log("-> Material not found. Creating new material asset at: " + path);
            AssetDatabase.CreateAsset(new Material(Shader.Find(_shaderPath)), path);
            AssetDatabase.Refresh();

            assetCandidates = AssetDatabase.FindAssets(_materialName.Substring(0, _materialName.Length - extensionLength));
            if (assetCandidates.Length != 0)
            {
                foreach (string stringItem in assetCandidates)
                {
                    if (AssetDatabase.GUIDToAssetPath(stringItem).Contains(_materialName))
                    {
                        _material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(stringItem));
                        break;
                    }
                }
            }
        }
        else
            _material.shader = Shader.Find(_shaderPath);

        if (_material == null)
        {
            Debug.LogWarning("-> Can't find '" + _materialName + "'... This should not happen...");
            return;
        }
    }

#pragma warning disable CS0414 // Never used
    private enum CameraProjection { Perspective, Orthogrpahic }
    [SerializeField]
    private CameraProjection cameraProjection = CameraProjection.Perspective;
    public bool camertaProjectionIs2D { get { return cameraProjection == CameraProjection.Orthogrpahic; } }

    private enum LightingMode { Unity, Central, Custom }
    [SerializeField]
    private LightingMode lightingMode = LightingMode.Unity;
    public bool lightModeIsCustom { get { return lightingMode == LightingMode.Custom; } }
    public bool lightModeIsNotUnity { get { return lightingMode != LightingMode.Unity; } }

    [SerializeField]
    private bool polyLiquid;

    //private enum RimColoring { Plain, Bright }
    //[SerializeField]
    //private RimColoring rimColoring = RimColoring.Plain;
#pragma warning restore CS0414 // Never used
    
    /// <summary>
    /// Sets shader keywords for needed materials.
    /// </summary>
    public void SetShaderKeywords()
    {
        SetShaderKeywords(planetMaterial);
        SetShaderKeywords(planetWRimMaterial);
    }

    private void SetShaderKeywords(Material _mat)
    {
        SetKeyword(_mat, "_CAMERA_ORTHOGRAPHIC", cameraProjection == CameraProjection.Orthogrpahic);
        SetKeyword(_mat, "_CAMERA_PERSPECTIVE", cameraProjection == CameraProjection.Perspective);

        SetKeyword(_mat, "_LIGHTING_UNITY", lightingMode == LightingMode.Unity);
        SetKeyword(_mat, "_LIGHTING_CENTRAL", lightingMode == LightingMode.Central);
        SetKeyword(_mat, "_LIGHTING_CUSTOM", lightingMode == LightingMode.Custom);

        SetKeyword(_mat, "_POLYLIQUID_ON", polyLiquid);

        //SetKeyword(_mat, "_RIMCOLORING_PLAIN", rimColoring == RimColoring.Plain);
        //SetKeyword(_mat, "_RIMCOLORING_BRIGHT", rimColoring == RimColoring.Bright);
    }

    private void SetKeyword(Material _mat, string _keyword, bool _state)
    {
        if (_state)
            _mat.EnableKeyword(_keyword);
        else
            _mat.DisableKeyword(_keyword);
    }
#endif

    [SerializeField]
    private Material m_planetMaterial;
    public Material planetMaterial
    {
        set { m_planetMaterial = value; }
        get
        {
#if UNITY_EDITOR
            if (m_planetMaterial == null)
                SetMaterialReference(planetMaterialName, out m_planetMaterial, planetShaderPath);
#endif
            return m_planetMaterial;
        }
    }

    [SerializeField]
    private Material m_planetWRimMaterial;
    public Material planetWRimMaterial
    {
        set { m_planetWRimMaterial = value; }
        get
        {
#if UNITY_EDITOR
            if (m_planetWRimMaterial == null)
                SetMaterialReference(planetWRimMaterialName, out m_planetWRimMaterial, planetWRimShaderName);
#endif
            return m_planetWRimMaterial;
        }
    }
}
