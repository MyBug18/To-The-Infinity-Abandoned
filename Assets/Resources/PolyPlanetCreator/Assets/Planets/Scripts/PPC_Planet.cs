using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class PPC_Planet : MonoBehaviour
{
    [SerializeField]
    private PPC_PlanetData m_data;
    public PPC_PlanetData data
    {
        get { return m_data; }

        set
        {
            if (m_data == value)
                return;
            m_data = value;

            if (autoLoadData)
                LoadAllData();
        }
    }
    
    [Tooltip("Loads data on data change and on Start.")]
    public bool autoLoadData;
    
    [SerializeField]
    private Vector4 m_customLightDirection = new Vector4(-1, -1, 0, 1);
    public Vector4 customLightDirection
    {
        get { return m_customLightDirection; }
        set
        {
            m_customLightDirection = value;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                SetCustomLightDirection(GetComponent<MeshRenderer>().sharedMaterial);
            else
#endif
                SetCustomLightDirection(GetComponent<MeshRenderer>().material);
        }
    }

    private void Start()
    {
        if (autoLoadData)
            LoadAllData();
            
#if UNITY_EDITOR
        if (GetComponent<MeshRenderer>().sharedMaterial == null)
            GetComponent<MeshRenderer>().sharedMaterial = PPC_PlanetManager.Instance.planetMaterial;
#endif

        customLightDirection = m_customLightDirection;

        if (GetComponent<MeshRenderer>().sharedMaterial.IsKeywordEnabled("_CAMERA_ORTHOGRAPHIC"))
            SetOrthograhicRimScale();
    }

    public void SetOrthograhicRimScale()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_ObjectScale", transform.lossyScale.x);
        else
#endif
            GetComponent<MeshRenderer>().material.SetFloat("_ObjectScale", transform.lossyScale.x);
    }

    /// <summary>
    /// Loads mesh data and shader data.
    /// </summary>
    public void LoadAllData()
    {
        LoadMeshData();
        LoadShaderData();
    }

    /// <summary>
    /// Loads mesh data if data isn't null.
    /// </summary>
    public void LoadMeshData()
    {
        if (data != null)
        {
            PPC_IcosahedronGeneratorNoUV.ApplyMesh(GetComponent<MeshFilter>(), data.LoadMesh(false));
            RecalculateMeshInfo();
        }
    }

    /// <summary>
    /// Loads shader data if data isn't null.
    /// </summary>
    public void LoadShaderData()
    {
        if (data != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                GetComponent<MeshRenderer>().sharedMaterial = data.LoadShaderData(GetComponent<MeshRenderer>().sharedMaterial);
            else
#endif
                GetComponent<MeshRenderer>().material = data.LoadShaderData(GetComponent<MeshRenderer>().material);
        }

    }

    private void SetCustomLightDirection(Material _mat)
    {
        _mat.SetVector("_LightDirection", customLightDirection);
    }

    /// <summary>
    /// Recalculates mesh bounds, normals and tangents. Also sets the MeshCollider's mesh, if a MeshCollider exists.
    /// </summary>
    public void RecalculateMeshInfo()
    {
        MeshFilter meshF = GetComponent<MeshFilter>();
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            meshF.sharedMesh.RecalculateBounds();
            meshF.sharedMesh.RecalculateNormals();
            meshF.sharedMesh.RecalculateTangents();
            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().sharedMesh = meshF.sharedMesh;
        }
        else
#endif
        {
            meshF.mesh.RecalculateBounds();
            meshF.mesh.RecalculateNormals();
            meshF.mesh.RecalculateTangents();
            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().sharedMesh = meshF.mesh;
        }
    }
}
