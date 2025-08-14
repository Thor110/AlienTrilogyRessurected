using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class TimerExample : MonoBehaviour
{
    private Stopwatch stopwatch;

    void Start()
    {
        stopwatch = new Stopwatch();

        // Start timer
        stopwatch.Start();
        UnityEngine.Debug.Log("Timer started.");
    }

    void Update()
    {
        // Example: stop timer when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log($"Timer stopped. Elapsed: {stopwatch.ElapsedMilliseconds} ms");

            // If you want to restart for another run:
            stopwatch.Reset();
            stopwatch.Start();
        }
    }
}

/*
	Alien Trilogy Data Loader
	Load data directly from original Alien Trilogy files to use it in Unity
*/
public class AlienTrilogyMapLoader : MonoBehaviour
{
    //[Header("PATHS")]
    private string levelPath = ""; // path to the .MAP file
    private string texturePath = "C:\\Program Files (x86)\\Collection Chamber\\Alien Trilogy\\HDD\\TRILOGY\\CD\\L111LEV.B16"; // path to the .B16 file

    [Header("SETTINGS")]
	// TODO : Adjust this dynamically
    public int textureSize = 256; // pixel dimensions
    private float scalingFactor = 1/512f; // scaling corrections
    private Material baseMaterial;

    // These store the mesh data for Unity
    private List<Vector3> meshVertices = new();
    private List<Vector2> meshUVs = new();
    private Dictionary<int, List<int>> meshTriangles = new();

    // Original vertex data from MAP0 before duplication
    private List<Vector3> originalVertices = new();

    // UV rectangles for each texture group
    private List<List<(int X, int Y, int Width, int Height)>> uvRects = new();

    // Texture image data list
    private List<Texture2D> imgData = new();

    public static AlienTrilogyMapLoader loader;

    /*
		Called once as soon as this script is loaded
	*/
    void Start()
    {
        if (loader == null)
        {
            loader = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*
		Load the file from a given path and build the map in Unity
	*/
    public void Initiate(string levelToLoad, string texturesToLoad)
    {
		baseMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        levelPath = levelToLoad;
        texturePath = texturesToLoad;
        BuildMapTextures(); // Build map textures
        BuildMapGeometry(); // Build map geometry
        BuildMapMesh();     // Build map mesh
    }
    // Parse BND file sections from a byte array
    private List<BndSection> LoadSection(byte[] bnd, string section)
    {
        var sections = new List<BndSection>();
        using var br = new BinaryReader(new MemoryStream(bnd));
        string formTag = Encoding.ASCII.GetString(br.ReadBytes(4)); // Read FORM header
        if (formTag != "FORM") { throw new Exception("Invalid BND file: missing FORM header."); }
        int formSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
        string platform = Encoding.ASCII.GetString(br.ReadBytes(4)); // e.g., "PSXT"
        while (br.BaseStream.Position + 8 <= br.BaseStream.Length) // Parse chunks
        {
            string chunkName = Encoding.ASCII.GetString(br.ReadBytes(4));
            int chunkSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            if (br.BaseStream.Position + chunkSize > br.BaseStream.Length) { break; }
            byte[] chunkData = br.ReadBytes(chunkSize);
            if (chunkName.StartsWith(section)) { sections.Add(new BndSection { Name = chunkName, Data = chunkData }); }
            if ((chunkSize % 2) != 0) { br.BaseStream.Seek(1, SeekOrigin.Current); } // IFF padding to 2-byte alignment
        }
        return sections;
    }
    /*
		Build the map textures
	*/
    private void BuildMapTextures()
    {
        Texture2D texture = null!;
        int levelID = int.Parse(levelPath.Substring(levelPath.Length - 10, 3)); //Get Level ID from levelPath String
        List<(int X, int Y, int Width, int Height)> rectangles = new();
        using var br = new BinaryReader(File.OpenRead(texturePath));
        br.BaseStream.Seek(36, SeekOrigin.Current);         // skip base header (36)
        for (int i = 0; i < 5; i++)                         // perfect read order TP/CL/BX*5
        {
            br.BaseStream.Seek(8, SeekOrigin.Current);      // TP header
            byte[] TP = br.ReadBytes(65536);                // texture
            br.BaseStream.Seek(12, SeekOrigin.Current);     // CL header
            byte[] CL = br.ReadBytes(512);                  // palette
            br.BaseStream.Seek(8, SeekOrigin.Current);      // BX header
            rectangles = new();                             // renew rectangles list
            int rectCount = br.ReadInt16();                 // UV rectangle count
            for (int j = 0; j < rectCount; j++)
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte width = br.ReadByte();
                byte height = br.ReadByte();
                br.BaseStream.Seek(2, SeekOrigin.Current);  // unknown bytes
                rectangles.Add((x, y, width + 1, height + 1));
            }
            if (rectCount % 2 == 0) { br.BaseStream.Seek(2, SeekOrigin.Current); }    // if number of UVs is even, read forward two extra bytes
            uvRects.Add(rectangles);
            texture = RenderRaw8bppImageUnity(TP, Convert16BitPaletteToRGB(CL), textureSize, levelID, i);
            texture.name = $"Tex_{i:D2}";
            imgData.Add(texture);
        }
    }
    /*
		Create 16-bit RGB palette
	*/
    public byte[] Convert16BitPaletteToRGB(byte[] rawPalette)
    {
        if (rawPalette == null || rawPalette.Length < 2)
            throw new ArgumentException("Palette data is missing or too short.");

        int colorCount = rawPalette.Length / 2;
        byte[] rgbPalette = new byte[256 * 3]; // max 256 colors RGB

        for (int i = 0; i < colorCount && i < 256; i++)
        {
            // Read 16-bit color (little endian)
            ushort color = (ushort)((rawPalette[i * 2 + 1] << 8) | rawPalette[i * 2]);

            int r5 = color & 0x1F;
            int g5 = (color >> 5) & 0x1F;
            int b5 = (color >> 10) & 0x1F;

            // Convert 5-bit color to 8-bit using bit replication
            byte r8 = (byte)((r5 << 3) | (r5 >> 2));
            byte g8 = (byte)((g5 << 3) | (g5 >> 2));
            byte b8 = (byte)((b5 << 3) | (b5 >> 2));

            rgbPalette[i * 3 + 0] = r8;
            rgbPalette[i * 3 + 1] = g8;
            rgbPalette[i * 3 + 2] = b8;
        }
        return rgbPalette;
    }
    /*
		Returns transparent palette indices based on map id and image index
	*/
    private int[] GetTransparencyValues(int id, int index)
    {
        int[] values = null;

        switch (id)
        {
            case 111:
            case 113:
            case 114:
            case 115:
            case 121:
                switch (index)
                {
                    case 0:
                    case 2:
                    case 4:
                        values = new int[] { 255 };
                        break;
                    case 1:
                    case 3:
                        values = new int[] { 0 };
                        break;
                }
                break;

            case 112:
                switch (index)
                {
                    case 0:
                        return values;
                    case 1:
                    case 3:
                        values = new int[] { 0 };
                        break;
                    case 2:
                    case 4:
                        values = new int[] { 255 };
                        break;
                }
                break;

            case 122:
            case 213:
                switch (index)
                {
                    case 0:
                        values = new int[] { 255 };
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        return values;
                }
                break;

            case 131:
            case 211:
            case 212:
            case 231:
            case 232:
            case 242:
            case 243:
            case 262:
            case 331:
            case 361:
            case 391:
            case 901:
            case 906:
            case 907:
                return values;

            case 141:
            case 155:
            case 161:
            case 162:
            case 263:
            case 311:
            case 352:
            case 353:
            case 381:
            case 902:
            case 903:
            case 908:
            case 909:
                switch (index)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        return values;
                    case 4:
                        values = new int[] { 255 };
                        break;
                }
                break;

            case 154:
            case 321:
            case 322:
            case 323:
            case 324:
            case 325:
                switch (index)
                {
                    case 0:
                    case 4:
                        values = new int[] { 255 };
                        break;
                    case 1:
                    case 2:
                    case 3:
                        return values;
                }
                break;

            case 222:
                switch (index)
                {
                    case 0:
                    case 1:
                    case 3:
                    case 4:
                        return values;
                    case 2:
                        values = new int[] { 255 };
                        break;
                }
                break;

            case 351:
            case 371:
                switch (index)
                {
                    case 0:
                    case 1:
                    case 3:
                        return values;
                    case 2:
                    case 4:
                        values = new int[] { 255 };
                        break;
                }
                break;

            case 900:
                switch (index)
                {
                    case 0:
                        values = new int[] { 255 };
                        break;
                    case 1:
                    case 3:
                        values = new int[] { 0 };
                        break;
                    case 2:
                    case 4:
                        return values;
                }
                break;

            case 904:
            case 905:
                switch (index)
                {
                    case 0:
                        values = new int[] { 255 };
                        break;
                    case 1:
                    case 2:
                    case 3:
                        return values;
                    case 4:
                        values = new int[] { 255 };
                        break;
                }
                break;

            default:
                break;
        }

        return values;
    }
    /*
		Create 8-bit texture from 16-bit palette, with transparency based on palette indices for given map id and image index
	*/
    private Texture2D RenderRaw8bppImageUnity(
        byte[] pixelData,
        byte[] rgbPalette,
        int dimension,
        int mapId,
        int imageIndex)
    {
        // Get transparency for this image
        int[] transparentValues = GetTransparencyValues(mapId, imageIndex);

        // Number of colors in palette
        int numColors = rgbPalette.Length / 3;

        // Create color array
        Color32[] pixels = new Color32[dimension * dimension];

        // Create texture object
        Texture2D texture = new Texture2D(dimension, dimension, TextureFormat.RGBA32, false);

        // Write pixels to texture
        for (int y = 0; y < dimension; y++)
        {
            for (int x = 0; x < dimension; x++)
            {
                int srcIndex = y * dimension + x;
                if (srcIndex >= pixelData.Length)
                    continue;

                byte colorIndex = pixelData[srcIndex];
                Color32 color;

                // Defensive: if palette data is incomplete or index out of range, fallback magenta
                if (colorIndex < numColors && (colorIndex * 3 + 2) < rgbPalette.Length)
                {
                    byte r = rgbPalette[colorIndex * 3];
                    byte g = rgbPalette[colorIndex * 3 + 1];
                    byte b = rgbPalette[colorIndex * 3 + 2];
                    color = new Color32(r, g, b, 255);
                }
                else
                {
                    color = new Color32(255, 0, 255, 255); // Magenta fallback
                }

                // Handle transparency: if pixel color index is in transparentValues, make fully transparent
                if (transparentValues != null && transparentValues.Contains(colorIndex))
                {
                    color.a = 0;  // Fully transparent
                }

                // Vertical flip: write pixels from bottom to top
                int flippedY = dimension - 1 - y;
                int dstIndex = flippedY * dimension + x;
                pixels[dstIndex] = color;
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }
    /*
		Build the map geometry and prepare mesh data (vertices, uvs, triangles)
	*/
    private void BuildMapGeometry()
    {
        // Clear all lists to avoid artefacts
        meshVertices.Clear();
        meshUVs.Clear();
        meshTriangles.Clear();
        originalVertices.Clear();
        // load MAP0 chunk
        using var br = new BinaryReader(new MemoryStream(LoadSection(File.ReadAllBytes(levelPath), "MAP0")[0].Data));
        ushort vertCount = br.ReadUInt16(); // Read number of vertices
        UnityEngine.Debug.Log("Number of vertices: " + vertCount);
        ushort quadCount = br.ReadUInt16(); // Read number of quads
        UnityEngine.Debug.Log("Number of quads: " + quadCount);
        br.BaseStream.Seek(32, SeekOrigin.Current); // Skip unused bytes
        for (int i = 0; i < vertCount; i++)
        {
            try
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short z = br.ReadInt16();
                br.ReadBytes(2); // unknown bytes
                originalVertices.Add(new Vector3(x, y, z));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed at vertex index {i}: {e}");
                break;
            }
        }

        UnityEngine.Debug.Log("originalVertices.Count = " + originalVertices.Count);

        // Read and process quads, duplicating vertices & uvs per quad
        for (int i = 0; i < quadCount; i++)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            try
            {
                a = br.ReadInt32();
                b = br.ReadInt32();
                c = br.ReadInt32();
                d = br.ReadInt32();
                ushort texIndex = br.ReadUInt16();
                byte flags = br.ReadByte();
                byte other = br.ReadByte();

                // Determine texture group based on texIndex
                int localIndex = texIndex;
                int texGroup = 0;
                for (int t = 0; t < uvRects.Count; t++)
                {
                    int count = uvRects[t].Count;
                    if (localIndex < count)
                    {
                        texGroup = t;
                        break;
                    }
                    localIndex -= count;
                }

                Vector3 vA, vB, vC, vD;
                bool issueFound = false;

                // Validate vertex indices
                if (a < 0 || b < 0 || c < 0 || d < 0 ||
                    a >= originalVertices.Count || b >= originalVertices.Count ||
                    c >= originalVertices.Count || d >= originalVertices.Count)
                {
                    UnityEngine.Debug.LogWarning($"Invalid quad indices at quad {i}: {a}, {b}, {c}, {d}, {flags}");
                    d = a; // Triangle instead of quad
                    issueFound = true;
                }

                // Get quad vertices positions
                vA = originalVertices[a];
                vB = originalVertices[b];
                vC = originalVertices[c];
                vD = originalVertices[d];

                // Get UV rectangle for texture
                var rect = uvRects[texGroup][localIndex];

                // Calculate UV coords (Unity uses bottom-left origin for UV)
                float texSize = (float)textureSize;
                float uMin = rect.X / texSize;
                float vMin = 1f - (rect.Y + rect.Height) / texSize;
                float uMax = (rect.X + rect.Width) / texSize;
                float vMax = 1f - rect.Y / texSize;

                Vector2[] baseUvs = new Vector2[]
                {
                        new Vector2(uMin, vMin), // A
						new Vector2(uMax, vMin), // B
						new Vector2(uMax, vMax), // C
						new Vector2(uMin, vMax)  // D
                };

                Vector2[] quadUvs;

                // Adjust UVs based on quad flags
                switch (flags)
                {
                    case 2:
                        // Flip texture 180 degrees
                        quadUvs = new Vector2[] { baseUvs[1], baseUvs[0], baseUvs[3], baseUvs[2] };
                        break;
                    case 11:
                        // Special triangle case: repeat D's UV for the 4th vertex
                        quadUvs = new Vector2[] { baseUvs[0], baseUvs[2], baseUvs[3], baseUvs[3] };
                        break;
                    default:
                        // Default UV mapping
                        quadUvs = baseUvs;
                        break;
                }

                // Adjust UVs based on triangles
                if (issueFound)
                {
                    quadUvs = new Vector2[] { baseUvs[0], baseUvs[2], baseUvs[3], baseUvs[3] };
                }

                // Add duplicated vertices for this quad
                int baseIndex = meshVertices.Count;

                meshVertices.Add(vA);
                meshVertices.Add(vB);
                meshVertices.Add(vC);
                meshVertices.Add(vD);

                // Add UVs for each vertex
                meshUVs.AddRange(quadUvs);

                // Add triangles for this quad (two triangles)
                if (!meshTriangles.TryGetValue(texGroup, out List<int> tris))
                {
                    tris = new List<int>();
                    meshTriangles[texGroup] = tris;
                }

                tris.Add(baseIndex + 0);
                tris.Add(baseIndex + 1);
                tris.Add(baseIndex + 2);

                tris.Add(baseIndex + 2);
                tris.Add(baseIndex + 3);
                tris.Add(baseIndex + 0);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Failed at quad index {i}:  {a}, {b}, {c}, {d} - {e}");
                break;
            }
        }
    }
    /*
		Build the map mesh in Unity from duplicated vertices/uvs and triangles
	*/
    private void BuildMapMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Mesh";

        mesh.vertices = meshVertices.ToArray();
        mesh.uv = meshUVs.ToArray();

        mesh.subMeshCount = uvRects.Count;

        Material[] materials = new Material[mesh.subMeshCount];
        foreach (var sub in meshTriangles)
        {
            mesh.SetTriangles(sub.Value, sub.Key);

            Material mat = new Material(baseMaterial);
            mat.name = "Mat_" + sub.Key;
            mat.color = Color.white;
            mat.mainTexture = imgData[sub.Key];
            materials[sub.Key] = mat;
        }

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject child = new GameObject("Map");
        child.transform.parent = this.transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        var mf = child.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        var mr = child.AddComponent<MeshRenderer>();
        mr.materials = materials;

        // Un-mirror
        child.transform.localScale = new Vector3(-1f, 1f, 1f) * scalingFactor;
        child.transform.localPosition = new Vector3(-int.Parse(ObjDataPuller.objectPuller.mapLengthString) / 2, 2, int.Parse(ObjDataPuller.objectPuller.mapWidthString) / 2);
        child.AddComponent<MeshCollider>();

        UnityEngine.Debug.Log("mesh.subMeshCount = " + mesh.subMeshCount);
    }
}
