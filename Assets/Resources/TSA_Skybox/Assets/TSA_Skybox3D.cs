using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TSA_Skybox3D : MonoBehaviour
{
    public Texture2D front, back, left, right, up, down;

    private readonly string shader = "_TS/TSA/Skybox/3D 6 Sided";

    private void Awake()
    {
        if (GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();
        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (GetComponent<MeshFilter>().sharedMesh == null)
            CreateMesh();
    }

    public void ApplyTextures() { ApplyTextures(front, back, left, right, up, down); }
    public void ApplyTextures(Texture2D _front, Texture2D _back, Texture2D _left, Texture2D _right, Texture2D _up, Texture2D _down)
    {
        SetTextureReferences(_front, _back, _left, _right, _up, _down);

        MeshRenderer meshR = GetComponent<MeshRenderer>();            
        meshR.sharedMaterials = new Material[]
        {
            new Material(Shader.Find(shader)),
            new Material(Shader.Find(shader)),
            new Material(Shader.Find(shader)),
            new Material(Shader.Find(shader)),
            new Material(Shader.Find(shader)),
            new Material(Shader.Find(shader))
        };

        string mainTex = "_MainTex";
        if (_front != null)
            meshR.sharedMaterials[0].SetTexture(mainTex, _front);
        if (_left != null)
            meshR.sharedMaterials[1].SetTexture(mainTex, _left);
        if (_back != null)
            meshR.sharedMaterials[2].SetTexture(mainTex, _back);
        if (_right != null)
            meshR.sharedMaterials[3].SetTexture(mainTex, _right);
        if (_down != null)
            meshR.sharedMaterials[4].SetTexture(mainTex, _down);
        if (_up != null)
            meshR.sharedMaterials[5].SetTexture(mainTex, _up);
    }

    private void SetTextureReferences(Texture2D _front, Texture2D _back, Texture2D _left, Texture2D _right, Texture2D _up, Texture2D _down)
    {
        front = _front;
        back = _back;
        left = _left;
        right = _right;
        up = _up;
        down = _down;
    }

    /// <summary>
    /// Gets called on Start if mesh is null.
    /// </summary>
    public void CreateMesh()
    {
        float a = 1f;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-a, -a, a),
            new Vector3(a, -a, a),
            new Vector3(a, -a, -a),
            new Vector3(-a, -a, -a),

            new Vector3(-a, a, a),
            new Vector3(a, a, a),
            new Vector3(a, a, -a),
            new Vector3(-a, a, -a)
        };

        Vector3[] realVertices = new Vector3[24];
        for (int i = 0; i < 4; i++)
        {
            realVertices[i * 4] = vertices[i];
            realVertices[i * 4 + 1] = vertices[(i + 1) % 4];
            realVertices[i * 4 + 2] = vertices[i + 4];
            realVertices[i * 4 + 3] = vertices[(i + 1) % 4 + 4];
        }

        realVertices[16] = vertices[3];
        realVertices[17] = vertices[2];
        realVertices[18] = vertices[0];
        realVertices[19] = vertices[1];

        realVertices[20] = vertices[4];
        realVertices[21] = vertices[5];
        realVertices[22] = vertices[7];
        realVertices[23] = vertices[6];

        Vector2[] uvs = new Vector2[24];
        for (int i = 0; i < 6; i++)
        {
            uvs[i * 4] = Vector2.zero;
            uvs[i * 4 + 1] = new Vector2(1, 0);
            uvs[i * 4 + 2] = new Vector2(0, 1);
            uvs[i * 4 + 3] = Vector2.one;
        }

        Mesh mesh = new Mesh
        {
            name = "Skybox 3D",
            vertices = realVertices,
            uv = uvs
        };

        int k = 0;
        mesh.subMeshCount = 6;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            k = i * 4;
            mesh.SetTriangles(new int[] { k, k + 2, k + 1, k + 1, k + 2, k + 3 }, i);
        }

        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * (float.MaxValue * 0.00000000000000000001f));
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
