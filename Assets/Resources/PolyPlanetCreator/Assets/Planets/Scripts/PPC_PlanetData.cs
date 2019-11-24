using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlanetData", menuName = "PPC/Planet Data")]
public class PPC_PlanetData : ScriptableObject
{
    [HideInInspector]
    public Vector3[] meshVertices;
    [HideInInspector]
    public int[] meshTriangles;
    [HideInInspector]
    public Color[] meshColors;
    
    public enum TerrainColoring { Average, Min, Max }
    [Header("Terrain")]
    public TerrainColoring terrainColoring = TerrainColoring.Average;
    [Range(0f, 1f)]
    public float darkSide = 0.666f;

    [Header("Liquid")]
    public Color liquidColor = new Color(0, 0.5f, 1f);
    [Range(0.99f, 1.51f)]
    public float liquidHeight = 0.99f;
    public Color specularColor = new Color(0.6f, 0.9f, 1f);
    [Range(1f, 6f)]
    public float specularHighlight = 3f;

    [Header("Core")]
    public Color coreColor = new Color(1, 0.8f, 0.4f);

    [Header("Rim")]
    public Color rimColor = new Color(0.6f, 0.9f, 1f);
    [Range(1f, 4f)]
    public float rimPower = 2f;
    [Range(0f, 1f)]
    public float rimOpacity = 0f;
    public bool outerRim = false;
    public Color outerRimColor = new Color(0.6f, 0.9f, 1f);
    [Range(0f, 2f)]
    public float outerRimRadius = 0.5f;
    [Range(-0.5f, 0.5f)]
    public float outerRimOffset = 0f;
    [Range(0f, 1f)]
    public float outerRimOpacity = 1f;

    public void SaveMeshData(Mesh _mesh)
    {
        meshVertices = _mesh.vertices;
        meshTriangles = _mesh.triangles;
        meshColors = _mesh.colors;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    /// <summary>
    /// Returns a mesh with the asset's saved data. If data isn't set, it returns PPC_IcosahedronGeneratorNoUV.Generate(10).
    /// </summary>
    /// <param name="_recalculateNecessaryData">Recalculates mesh bounds, normals and tangents.</param>
    public Mesh LoadMesh(bool _recalculateNecessaryData)
    {
        Mesh mesh;
        if (meshVertices == null || meshVertices.Length < 10)
        {
            Debug.Log(GetType().ToString() + ": Generating new mesh data...");
            mesh = PPC_IcosahedronGeneratorNoUV.Generate(10);
            SaveMeshData(mesh);
        }
        else
        {
            mesh = new Mesh
            {
                name = name + " Mesh",
                vertices = meshVertices,
                triangles = meshTriangles,
                colors = meshColors
            };
        }

        if (_recalculateNecessaryData)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        return mesh;
    }

    /// <summary>
    /// Returns a copy of the provided material with assets data.
    /// </summary>
    /// <param name="_mat">Material to set data to.</param>
    public Material LoadShaderData(Material _mat)
    {
        if (outerRim)
        {
            if (_mat == null || _mat.shader != PPC_PlanetManager.Instance.planetWRimMaterial.shader)
                _mat = PPC_PlanetManager.Instance.planetWRimMaterial;
        }
        else
        {
            if (_mat == null || _mat.shader != PPC_PlanetManager.Instance.planetMaterial.shader)
                _mat = PPC_PlanetManager.Instance.planetMaterial;
        }

        SetKeyword(_mat, "_TERRAINCOLORING_AVERAGE", terrainColoring == TerrainColoring.Average);
        SetKeyword(_mat, "_TERRAINCOLORING_MIN", terrainColoring == TerrainColoring.Min);
        SetKeyword(_mat, "_TERRAINCOLORING_MAX", terrainColoring == TerrainColoring.Max);

        _mat.SetFloat("_DarkSide", darkSide);

        _mat.SetColor("_LiquidColor", liquidColor);
        _mat.SetFloat("_LiquidHeight", liquidHeight);
        _mat.SetColor("_SpecularColor", specularColor);
        _mat.SetFloat("_SpecularHighlight", specularHighlight);

        _mat.SetColor("_CoreColor", coreColor);

        _mat.SetColor("_RimColor", rimColor);
        _mat.SetFloat("_RimPower", rimPower);
        _mat.SetFloat("_RimOpacity", rimOpacity);
        _mat.SetColor("_OuterRimColor", outerRimColor);
        _mat.SetFloat("_OuterRimRadius", outerRimRadius);
        _mat.SetFloat("_OuterRimOffset", outerRimOffset);
        _mat.SetFloat("_OuterRimOpacity", outerRimOpacity);

        return _mat;
    }

    protected void SetKeyword(Material _mat, string _keyword, bool _state)
    {
        if (_state)
            _mat.EnableKeyword(_keyword);
        else
            _mat.DisableKeyword(_keyword);
    }
}
