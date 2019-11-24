using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPC_IcosahedronGeneratorNoUV
{
    /// <summary>
    /// Returns an icosahedron based mesh. Does not have UV data.
    /// </summary>
    public static Mesh Generate(int _detail)
    {
        float t = (1f + Mathf.Sqrt(5f)) / 2f;
        Vector3[] verts = new Vector3[]
        {
            new Vector3(-1, t, 0),
            new Vector3(1, t, 0),
            new Vector3(-1, -t, 0),
            new Vector3(1, -t, 0),

            new Vector3(0, -1, t),
            new Vector3(0, 1, t),
            new Vector3(0, -1, -t),
            new Vector3(0, 1, -t),

            new Vector3(t, 0, -1),
            new Vector3(t, 0, 1),
            new Vector3(-t, 0, -1),
            new Vector3(-t, 0, 1)
        };

        int[] triangles = new int[]
        {
            // 5 faces around point 0
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,
            // 5 adjacent faces
            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,
            // 5 faces around point 3
            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,
            // 5 adjacent faces
            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1
        };

        List<Vector3> realVertices = new List<Vector3>();
        List<int> realTriangles = new List<int>();
        for (int i = 0; i < triangles.Length; i += 3)
            CreateDetail(_detail, i, verts, triangles, ref realVertices, ref realTriangles);

        for (int i = 0; i < realVertices.Count; i++)
            realVertices[i] = realVertices[i].normalized;

        //Debug.Log("vertices: " + realVertices.Count + "   triangles: " + (realTriangles.Count / 3f));
        return MakeMesh(realVertices.ToArray(), realTriangles.ToArray());
    }

    private static void CreateDetail(int _detail, int _i, Vector3[] _verts, int[] _triangles, ref List<Vector3> realVertices, ref List<int> realTriangles)
    {
        int firstT = _triangles[_i];
        int secondT = _triangles[_i + 1];
        int thirdT = _triangles[_i + 2];

        Vector3 a = (_verts[secondT] - _verts[firstT]) / _detail;
        Vector3 b = (_verts[thirdT] - _verts[firstT]) / _detail;

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        Dictionary<Vector3, int> vertDict = new Dictionary<Vector3, int>();
        Vector3 vertex;
        int indexInc = 0;
        for (int j = 0; j <= _detail; j++)
        {
            for (int i = 0; i <= _detail - j; i++)
            {
                vertex = _verts[firstT] + a * i + b * j;
                if (i == 0 || j == 0 || j == _detail || i == _detail - j)
                {
                    for (int k = 0; k < realVertices.Count; k++)
                    {
                        if (Vector3.Distance(vertex, realVertices[k]) < 1f / _detail)
                        {
                            vertDict.Add(vertex, k);
                            break;
                        }
                    }

                    if (!vertDict.ContainsKey(vertex))
                    {
                        vertDict.Add(vertex, realVertices.Count + indexInc);
                        verts.Add(vertex);
                        indexInc++;
                    }
                }
                else
                {
                    vertDict.Add(vertex, realVertices.Count + indexInc);
                    verts.Add(vertex);
                    indexInc++;
                }
            }
        }

        for (int j = 0; j < _detail; j++)
        {
            for (int i = 0; i < _detail - j; i++)
            {
                tris.Add(vertDict[_verts[firstT] + a * i + b * j]);
                tris.Add(vertDict[_verts[firstT] + a * (i + 1) + b * j]);
                tris.Add(vertDict[_verts[firstT] + a * i + b * (j + 1)]);
                if (j < _detail - 1 && i < _detail - j - 1)
                {
                    tris.Add(vertDict[_verts[firstT] + a * (i + 1) + b * j]);
                    tris.Add(vertDict[_verts[firstT] + a * (i + 1) + b * (j + 1)]);
                    tris.Add(vertDict[_verts[firstT] + a * i + b * (j + 1)]);
                }
            }
        }

        realVertices.AddRange(verts);
        realTriangles.AddRange(tris);
    }

    private static Mesh MakeMesh(Vector3[] _verts, int[] _triangles)
    {
        Color[] cols = new Color[_verts.Length];
        for (int i = 0; i < cols.Length; i++)
            cols[i] = Color.white;

        Mesh mesh = new Mesh
        {
            name = "Icosahedron" + _verts.Length,
            vertices = _verts,
            normals = _verts,
            triangles = _triangles,
            colors = cols
        };

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;

    }

    public static void ApplyMesh(MeshFilter _meshFilter, Mesh _mesh)
    {
        if (!Application.isPlaying)
            _meshFilter.sharedMesh = _mesh;
        else
            _meshFilter.mesh = _mesh;

        if (_meshFilter.GetComponent<MeshCollider>() != null)
            _meshFilter.GetComponent<MeshCollider>().sharedMesh = _mesh;

        if (_meshFilter.GetComponent<PPC_PlanetMeshBuilder>() != null)
            _meshFilter.GetComponent<PPC_PlanetMeshBuilder>().InitiateBuilder();
    }
}
