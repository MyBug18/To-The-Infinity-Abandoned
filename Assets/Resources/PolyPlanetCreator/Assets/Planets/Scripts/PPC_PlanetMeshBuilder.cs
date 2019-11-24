using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PPC_Planet))]
[RequireComponent(typeof(MeshCollider))]
public class PPC_PlanetMeshBuilder : MonoBehaviour
{
    [Tooltip("Automatically saves data.")]
    public bool autoSaveData;

    private Camera m_cam;
    private Camera cam
    {
        get
        {
            if (m_cam == null)
                m_cam = FindObjectOfType<Camera>();

            return m_cam;
        }
    }

    private MeshFilter m_meshF;
    public MeshFilter MeshF
    {
        get
        {
            if (m_meshF == null)
                m_meshF = GetComponent<MeshFilter>();
            return m_meshF;
        }
    }

    private PPC_Planet m_polyPlanet;
    public PPC_Planet PolyPlanet
    {
        get
        {
            if (m_polyPlanet == null)
                m_polyPlanet = GetComponent<PPC_Planet>();

            return m_polyPlanet;
        }
    }

    private enum BrushMode { Modify, Smooth, Level, Color }
    [SerializeField]
    [HideInInspector]
    private BrushMode m_brushMode = BrushMode.Modify;
    private BrushMode brushMode
    {
        get { return m_brushMode; }

        set
        {
            m_brushMode = value;

            SetBrushModeUIText();

#if UNITY_EDITOR
            if (value == BrushMode.Modify)
            {
                brushColor = brushColor2 = Color.red;
            }
            else if (value == BrushMode.Smooth)
            {
                brushColor = Color.green;
                brushColor2 = Color.white;
            }
            else if (value == BrushMode.Level)
            {
                brushColor = brushColor2 = new Color(0, 0.75f, 1, 1);
                SetLevelBrushUIText();
            }
#endif
        }
    }

    [SerializeField]
    [HideInInspector]
    [Tooltip("You can also use middle mouse button.")]
    private KeyCode brushModeKey = KeyCode.LeftControl;
    private bool m_singularMode;
    private bool singularMode
    {
        get { return m_singularMode; }

        set
        {
            m_singularMode = value;
            SetBrushModeUIText();
        }
    }
    
    [SerializeField]
    private float brushPower = 0.004f;
    [Range(1f, 1.5f)]
    [SerializeField]
    private float maxHeight = 1.2f;
    [SerializeField]
    private bool limitDepth1 = true;
    private RaycastHit hit;

    private bool m_editing = true;
    private bool editing
    {
        get { return m_editing; }
        set
        {
            m_editing = value;
            if (value)
                StartCoroutine("RecalculateNormalsCouroutine");
            else
                StopCoroutine("RecalculateNormalsCouroutine");

        }
    }

    private Vector3 point;
    private List<int> indexes = new List<int>();
    private Vector3[] vertices;
    private Color[] colors;

    [SerializeField]
    [Range(0f, 1f)]
    private float colorNoise = 0.1f;
    [SerializeField]
    private bool monotone = true;
    [SerializeField]
    private Color m_selectedColor = Color.white;
    public Color SelectedColor
    {
        get { return m_selectedColor; }
        set
        {
            m_selectedColor = value;
            SetColorPickerColor();
        }
    }
    
    /// <summary>
    /// Use for UnityEvents.
    /// </summary>
    public void SetSelectedColor(Color _color) { m_selectedColor = _color; }

    private void SetColorPickerColor()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            if (colorPicker != null)
                colorPicker.RGB2HSV(SelectedColor);
    }

    private float level = 1;

    private void Start()
    {
        brushMode = m_brushMode;
    }

    public void SaveCurrentMeshDataToScriptableObject()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            if (PolyPlanet.data != null)
                PolyPlanet.data.SaveMeshData(GetComponent<MeshFilter>().mesh);
    }

    private void SetColorArray()
    {
        if (MeshF.sharedMesh.colors.Length != MeshF.sharedMesh.vertices.Length)
        {
            Debug.Log("set new colors");
            colors = new Color[MeshF.sharedMesh.vertices.Length];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.white;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                MeshF.sharedMesh.colors = colors;
            else
#endif
                MeshF.mesh.colors = colors;
        }
        else
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                colors = MeshF.sharedMesh.colors;
            else
#endif
                colors = MeshF.mesh.colors;
        }

    }

#if UNITY_EDITOR
    private void SetGizmoSize()
    {
        gizmoSize = Vector3.Distance(vertices[0], vertices[1]) * gizmoScale;
    }
#endif

    /// <summary>
    /// Sets arrays.
    /// </summary>
    public void InitiateBuilder()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            vertices = MeshF.sharedMesh.vertices;
        else
#endif
            vertices = MeshF.mesh.vertices;
        SetColorArray();
#if UNITY_EDITOR
        SetGizmoSize();
#endif
    }

    private bool meshChanged = false;
    private void Update()
    {
        BrushModeInput();

        if (Input.GetKeyUp(brushModeKey) || Input.GetMouseButtonUp(2))
            singularMode = !singularMode;

        FindNearestVertexAndEdit();
        if (editing && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            editing = false;
            if (meshChanged)
                PolyPlanet.RecalculateMeshInfo();
            if (autoSaveData)
                SaveCurrentMeshDataToScriptableObject();
            meshChanged = false;
        }

        BrushModeUI();
        LevelBrushUI();
    }

    private void BrushModeInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            brushMode = 0;
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            brushMode = (BrushMode)1;
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            brushMode = (BrushMode)2;
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            brushMode = (BrushMode)3;

        // mouseWheel alternative
        //if (Input.mouseScrollDelta.y != 0)
        //{
        //    brushMode += (int)Mathf.Sign(-Input.mouseScrollDelta.y);
        //    if (brushMode < 0)
        //        brushMode = BrushMode.Color;
        //    else if (brushMode > BrushMode.Color)
        //        brushMode = BrushMode.Modify;
        //}
    }

    private void FindNearestVertexAndEdit()
    {
        if (indexes.Count > 0)
            indexes.Clear();

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.gameObject == gameObject)
                point = hit.point;
            else
                return;
        }
        else
            return;

        vertices = MeshF.mesh.vertices;
        int[] triangles = MeshF.mesh.triangles;

        float itemDist;
        float dist = float.MaxValue;
        int vertexIndex = 0;
        for (int i = hit.triangleIndex * 3; i < hit.triangleIndex * 3 + 3; i++)
        {
            itemDist = Vector3.Magnitude(point.normalized - vertices[triangles[i]].normalized);
            if (itemDist < dist)
            {
                dist = itemDist;
                vertexIndex = triangles[i];
            }
        }

        indexes.Add(vertexIndex);
        if (!singularMode || brushMode == BrushMode.Smooth || brushMode == BrushMode.Color)
        {
            for (int i = 0; i < triangles.Length; i += 3)
                if (triangles[i] == vertexIndex || triangles[i + 1] == vertexIndex || triangles[i + 2] == vertexIndex)
                    for (int j = 0; j < 3; j++)
                        if (!indexes.Contains(triangles[i + j]))
                            indexes.Add(triangles[i + j]);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            EditVertices(vertexIndex);
    }

    /// <summary>
    /// This function contains everything for mesh editing.
    /// </summary>
    private void EditVertices(int _vertexIndex)
    {
        if (!editing)
            editing = true;

        if (brushMode == BrushMode.Modify)
        {
            float extrude = brushPower * (Input.GetMouseButton(0) ? 1 : -1);
            for (int i = 0; i < indexes.Count; i++)
            {
                vertices[indexes[i]] = vertices[indexes[i]].normalized * Mathf.Clamp(Vector3.Magnitude(vertices[indexes[i]]) + extrude, limitDepth1 ? 1 : 0.5f, maxHeight);
                if (i == 0)
                    extrude *= 0.75f;
            }
            meshChanged = true;
        }
        else if (brushMode == BrushMode.Smooth)
        {
            float intermediateLength = 0;
            for (int i = 1; i < indexes.Count; i++)
                intermediateLength += Vector3.Magnitude(vertices[indexes[i]]);
            intermediateLength /= indexes.Count - 1;

            if (Input.GetMouseButton(0))
                intermediateLength = Vector3.Magnitude(vertices[_vertexIndex]) + (intermediateLength - Vector3.Magnitude(vertices[_vertexIndex])) * 0.1f;
            
            vertices[_vertexIndex] = vertices[_vertexIndex].normalized * intermediateLength;
            meshChanged = true;
        }
        else if (brushMode == BrushMode.Level)
        {
            if (Input.GetMouseButton(1))
            {
                level = Vector3.Magnitude(vertices[_vertexIndex]);
                SetLevelBrushUIText();
            }
            else
            {
                foreach (var index in indexes)
                    vertices[index] = vertices[index].normalized * level;
                meshChanged = true;
            }
        }
        else if (brushMode == BrushMode.Color)
        {
            if (Input.GetMouseButton(0))
            {
                colors = MeshF.mesh.colors;
                if (singularMode)
                {
                    if (monotone)
                    {
                        float variation = Random.Range(-colorNoise, colorNoise);
                        colors[_vertexIndex] = new Color(
                            Mathf.Clamp01(SelectedColor.r + variation),
                            Mathf.Clamp01(SelectedColor.g + variation),
                            Mathf.Clamp01(SelectedColor.b + variation),
                            1);
                    }
                    else
                    {
                        colors[_vertexIndex] = new Color(
                            Mathf.Clamp01(SelectedColor.r + Random.Range(-colorNoise, colorNoise)),
                            Mathf.Clamp01(SelectedColor.g + Random.Range(-colorNoise, colorNoise)),
                            Mathf.Clamp01(SelectedColor.b + Random.Range(-colorNoise, colorNoise)),
                            1);
                    }
                }
                else
                {
                    if (monotone)
                    {
                        float variation = Random.Range(-colorNoise, colorNoise);
                        foreach (var index in indexes)
                            colors[index] = new Color(
                                Mathf.Clamp01(SelectedColor.r + variation),
                                Mathf.Clamp01(SelectedColor.g + variation),
                                Mathf.Clamp01(SelectedColor.b + variation),
                                1);
                    }
                    else
                    {
                        foreach (var index in indexes)
                            colors[index] = new Color(
                                Mathf.Clamp01(SelectedColor.r + Random.Range(-colorNoise, colorNoise)),
                                Mathf.Clamp01(SelectedColor.g + Random.Range(-colorNoise, colorNoise)),
                                Mathf.Clamp01(SelectedColor.b + Random.Range(-colorNoise, colorNoise)),
                                1);
                    }
                }
                MeshF.mesh.colors = colors;
            }
            else if (Input.GetMouseButton(1))
                SelectedColor = colors[_vertexIndex];
        }

        if (meshChanged)
            MeshF.mesh.vertices = vertices;
    }

    private readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    private IEnumerator RecalculateNormalsCouroutine()
    {
        while (true)
        {
            yield return wait;
            yield return wait;
            yield return wait;
            yield return wait;
            yield return wait;
            MeshF.mesh.RecalculateNormals();
        }
    }

    /// <summary>
    /// Finds vertex with max distance and sets maxHeight to that distance.
    /// </summary>
    public void GetMaxCurrentMaxHeight()
    {
        if (vertices == null || vertices.Length == 0)
            return;

        float currentMax = 1;
        foreach (var item in vertices)
        {
            float vertexRadius = Vector3.Magnitude(item);
            if (vertexRadius > currentMax)
                currentMax = vertexRadius;
        }

        maxHeight = currentMax;
    }

    /// <summary>
    /// Sets distance of every vertex to maxHeight.
    /// </summary>
    public void SetHeightToMaxHeight()
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = vertices[i].normalized * maxHeight;

        MeshF.mesh.vertices = vertices;
        PolyPlanet.RecalculateMeshInfo();
        if (autoSaveData)
            SaveCurrentMeshDataToScriptableObject();
    }

    /// <summary>
    /// Colors whole planet with selectedColor (noisy color).
    /// </summary>
    public void ColorPlanetWithCurrentColor()
    {
        if (monotone)
        {
            float variation = 0;
            for (int i = 0; i < colors.Length; i++)
            {
                variation = Random.Range(-colorNoise, colorNoise);
                colors[i] = new Color(
                    Mathf.Clamp01(SelectedColor.r + variation),
                    Mathf.Clamp01(SelectedColor.g + variation),
                    Mathf.Clamp01(SelectedColor.b + variation),
                    1);
            }
        }
        else
        {
            for (int i = 0; i < colors.Length; i++)
                colors[i] = new Color(
                    Mathf.Clamp01(SelectedColor.r + Random.Range(-colorNoise, colorNoise)), 
                    Mathf.Clamp01(SelectedColor.g + Random.Range(-colorNoise, colorNoise)), 
                    Mathf.Clamp01(SelectedColor.b + Random.Range(-colorNoise, colorNoise)), 
                    1);
        }

        MeshF.mesh.colors = colors;

        if (autoSaveData)
            SaveCurrentMeshDataToScriptableObject();
    }
    
    /// <summary>
    /// This property calls a private function SampleTexture(...). This property always returns null.
    /// </summary>
    public Texture2D sampleTexture { set { SampleTexture(value); } get { return null; } }
    private void SampleTexture(Texture2D _tex)
    {
        if (_tex == null)
            return;

        Color[] height = _tex.GetPixels();

        List<Vector3> normalizedVertices = new List<Vector3>(vertices);
        for (int i = 0; i < normalizedVertices.Count; i++)
            normalizedVertices[i] = normalizedVertices[i].normalized;

        List<Vector2> uvs = new List<Vector2>();
        foreach (var item in normalizedVertices)
            uvs.Add(
                new Vector2(
                    (Mathf.Atan2(item.z, item.x) / (2 * Mathf.PI) + 0.5f) * (_tex.width - 1), 
                    ((Mathf.Asin(item.y) / Mathf.PI) + 0.5f) * (_tex.height - 1)
                    ));
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * Mathf.Lerp(1, maxHeight,
                height[Mathf.FloorToInt(uvs[i].y) * _tex.width + Mathf.FloorToInt(uvs[i].x)].r
                );
        }

        MeshF.sharedMesh.vertices = vertices;
        PolyPlanet.RecalculateMeshInfo();
        if (autoSaveData)
            SaveCurrentMeshDataToScriptableObject();
    }

#region perlin
    [Range(0.001f, 1)]
    public float scale = 0.1f;
    [Range(1, 6)]
    private const int octaves = 4;
    [Range(0, 1)]
    private const float persistance = 0.6f;
    private const float lacunarity = 2;
    public int seed;

    private float Perlin3D(float x, float y, float z)
    {
        return (Mathf.PerlinNoise(x, y) + Mathf.PerlinNoise(y, z) + Mathf.PerlinNoise(x, z) + Mathf.PerlinNoise(y, x) + Mathf.PerlinNoise(z, x) + Mathf.PerlinNoise(z, y)) / 6f;
    }

    private Vector3[] PerlinMeshVerts(Vector3[] _verts, int seed, float scale, int octaves, float persistance, float lacunarity, float maxHeight)
    {
        for (int i = 0; i < _verts.Length; i++)
            _verts[i] = _verts[i].normalized;

        if (scale <= 0)
            scale = 0.001f;

        System.Random prng = new System.Random(seed);
        Vector3[] octaveOffsets = new Vector3[octaves];
        for (int i = 0; i < octaves; i++)
            octaveOffsets[i] = new Vector3(prng.Next(-100000, 100000), prng.Next(-100000, 100000), prng.Next(-100000, 100000));

        float mini = float.MaxValue;
        float maxi = float.MinValue;
        float[] noiseHeight = new float[_verts.Length];
        for (int i = 0; i < _verts.Length; i++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseH = 0;

            for (int k = 0; k < octaves; k++)
            {
                float sampleX = _verts[i].x / scale * frequency + octaveOffsets[k].x;
                float sampleY = _verts[i].y / scale * frequency + octaveOffsets[k].y;
                float sampleZ = _verts[i].z / scale * frequency + octaveOffsets[k].z;

                float perlinValue = Perlin3D(sampleX, sampleY, sampleZ);
                noiseH += perlinValue * amplitude;

                amplitude *= persistance;
                frequency *= lacunarity;
            }

            noiseHeight[i] = noiseH;

            if (noiseHeight[i] > maxi)
                maxi = noiseHeight[i];
            if (noiseHeight[i] < mini)
                mini = noiseHeight[i];
        }

        for (int i = 0; i < _verts.Length; i++)
            _verts[i] *= 1 + Mathf.InverseLerp(mini, maxi, noiseHeight[i]) * (maxHeight - 1);

        return _verts;
    }

    /// <summary>
    /// Applies perlin noise to mesh and saves it.
    /// </summary>
    public void ApplyNoiseToMesh()
    {
        vertices = PerlinMeshVerts(MeshF.mesh.vertices, seed, scale, octaves, persistance, lacunarity, maxHeight);
        MeshF.mesh.vertices = vertices;
        PolyPlanet.RecalculateMeshInfo();
        if (autoSaveData)
            SaveCurrentMeshDataToScriptableObject();
    }
#endregion perlin

#region editor
#if UNITY_EDITOR
    [Range(0.1f, 1f)]
    public float gizmoScale = 0.333f;
    private float gizmoSize;

    private Color brushColor = Color.white;
    private Color brushColor2 = Color.white;
    private void OnDrawGizmos()
    {
        if (brushMode == BrushMode.Color)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                Gizmos.color = colors[indexes[i]];
                Gizmos.DrawSphere(transform.TransformPoint(vertices[indexes[i]]), (i != 0 && (singularMode || brushMode == BrushMode.Smooth) ? gizmoSize * 0.5f : gizmoSize) * transform.lossyScale.x);
            }
        }
        else
        {
            Gizmos.color = brushColor;
            for (int i = 0; i < indexes.Count; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertices[indexes[i]]), (i != 0 && (singularMode || brushMode == BrushMode.Smooth) ? gizmoSize * 0.5f : gizmoSize) * transform.lossyScale.x);
                if (i == 0)
                    Gizmos.color = brushColor2;
            }
        }

        Gizmos.color = Color.white;
    }
#endif
#endregion editor

#region UI

    public PPC_ColorPicker colorPicker;

    [Tooltip("Display brush mode as UI text.")]
    public RectTransform brushModeTextUI;
    private float brushModeTime;
    private float brushModeCD = 2;

    private void SetBrushModeUIText()
    {
        if (brushModeTextUI == null)
            return;

        brushModeTextUI.GetComponent<Text>().text = brushMode.ToString() + (singularMode && brushMode != BrushMode.Smooth ? " Singular" : "");
        brushModeTime = Time.time + brushModeCD;
    }

    private void BrushModeUI()
    {
        if (brushModeTextUI == null)
            return;

        if (Time.time < brushModeTime)
        {
            if (!brushModeTextUI.gameObject.activeSelf)
                brushModeTextUI.gameObject.SetActive(true);

            brushModeTextUI.anchoredPosition = Input.mousePosition + new Vector3(50, -brushModeTextUI.rect.height * 0.5f, 0);
        }
        else if (brushModeTextUI.gameObject.activeSelf)
            brushModeTextUI.gameObject.SetActive(false);
    }

    [Tooltip("Display level height as UI text.")]
    public RectTransform levelBrushTextUI;

    private void SetLevelBrushUIText()
    {
        if (levelBrushTextUI == null)
            return;

        levelBrushTextUI.GetComponent<Text>().text = level.ToString();
    }

    private void LevelBrushUI()
    {
        if (levelBrushTextUI == null)
            return;

        if (brushMode == BrushMode.Level && Time.time >= brushModeTime)
        {
            if (!levelBrushTextUI.gameObject.activeSelf)
                levelBrushTextUI.gameObject.SetActive(true);

            levelBrushTextUI.anchoredPosition = Input.mousePosition + new Vector3(50, -levelBrushTextUI.rect.height * 0.5f, 0);
        }
        else if (levelBrushTextUI.gameObject.activeSelf)
            levelBrushTextUI.gameObject.SetActive(false);
    }
    #endregion UI

#if UNITY_EDITOR
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0414 // Never used
    [Tooltip("Available in editor only.")]
    [Range(1, 80)]
    [SerializeField]
    private int detail = 20;
#pragma warning restore CS0414 // Never used
#pragma warning restore IDE0044 // Add readonly modifier
#endif
}
