using System.Diagnostics;

namespace ALTViewer
{
    public static class ModelRenderer
    {
        public const float texSize = 256f;
        public static void ExportLevel(string levelName, List<BndSection> uvSections, byte[] levelSection, string textureName, string outputPath)
        {
            using var br = new BinaryReader(new MemoryStream(levelSection));
            short vertCount = br.ReadInt16();         // Number of vertices
            short quadCount = br.ReadInt16();         // Number of quads
            ushort mapLength = br.ReadUInt16();         // Length of the map section
            ushort mapWidth = br.ReadUInt16();          // Width of the map section
            ushort playerStartX = br.ReadUInt16();      // Player start X coordinate
            ushort playerStartY = br.ReadUInt16();      // Player start Y coordinate
            br.ReadBytes(2);                            // unknown 2 bytes
            ushort monster = br.ReadUInt16();           // Number of monsters
            ushort pickups = br.ReadUInt16();           // Number of pickups
            ushort boxes = br.ReadUInt16();             // Number of boxes
            ushort doors = br.ReadUInt16();             // Number of doors
            br.ReadBytes(2);                            // unknown 2 bytes
            ushort playerStartAngle = br.ReadUInt16();  // Player start angle
            br.ReadBytes(10);                           // unknown 6 and 4 bytes
            List<(ushort X, ushort Y, ushort Z)> vertices = new();
            for (int i = 0; i < vertCount; i++) // Count Vertices
            {
                ushort x = br.ReadUInt16();
                ushort y = br.ReadUInt16();
                ushort z = br.ReadUInt16();
                br.ReadBytes(2); // unknown bytes
                vertices.Add((x, y, z));
            }
            List<(int A, int B, int C, int D, ushort TexIndex, byte Flags)> quads = new();
            for (int i = 0; i < quadCount; i++) // Count Quads
            {
                int a = br.ReadInt32();
                int b = br.ReadInt32();
                int c = br.ReadInt32();
                int d = br.ReadInt32();
                ushort texIndex = br.ReadUInt16(); // signed or unsigned?
                byte flags = br.ReadByte();
                br.ReadByte(); // unknown byte

                quads.Add((a, b, c, d, texIndex, flags));
            }
            // Read UV rectangles BX00-BX04
            var uvRects = new List<(int X, int Y, int Width, int Height)>[5];
            for (int i = 0; i < 5; i++)
            {
                uvRects[i] = ParseBxRectangles(uvSections[i].Data);
            }
            string objPath = outputPath + $"\\{levelName}.obj";
            using var sw = new StreamWriter(objPath);

            using var mtlWriter = new StreamWriter(Path.Combine(outputPath, $"{levelName}.mtl"));

            sw.WriteLine($"# OBJ exported from Alien Trilogy {levelName}");

            sw.WriteLine($"mtllib {levelName}.mtl");
            
            for (int t = 0; t < 5; t++)
            {
                mtlWriter.WriteLine($"newmtl Texture{t:D2}");
                mtlWriter.WriteLine($"map_Kd {textureName}_TP{t:D2}.png");
            }
            // Write vertex positions
            foreach (var v in vertices)
            {
                sw.WriteLine($"v {v.X:F4} {v.Y:F4} {v.Z:F4}");
            }

            // Store unique UVs and their indices
            var uvDict = new Dictionary<(float, float), int>();
            var uvList = new List<(float, float)>();
            // Ensure at least one dummy UV exists (for fallback cases using index 1)
            if (uvList.Count == 0)
            {
                uvDict[(0f, 0f)] = 1;
                uvList.Add((0f, 0f));
            }
            // Map of per-face vertex UV indices
            var faceUvs = new List<int[]>();

            for (int i = 0; i < quads.Count; i++)
            {
                var q = quads[i];
                var uvIndices = new int[4];

                // Resolve texture group + local UV rect index
                bool found = false;
                int texGroup = 0;
                int localIndex = q.TexIndex;

                for (int t = 0; t < 5; t++)
                {
                    int count = uvRects[t].Count;
                    if (localIndex < count)
                    {
                        texGroup = t;
                        found = true;
                        break;
                    }
                    localIndex -= count;
                }

                if (!found || localIndex >= uvRects[texGroup].Count)
                {
                    // Fallback rectangle or skip invalid quad
                    faceUvs.Add(new int[] { 1, 1, 1, 1 }); // or log + continue
                    continue;
                }

                var rect = uvRects[texGroup][localIndex];
                float x0 = rect.X / texSize;
                float y0 = rect.Y / texSize;
                float x1 = (rect.X + rect.Width) / texSize;
                float y1 = (rect.Y + rect.Height) / texSize;

                var baseUvs = new (float, float)[]
                {
                    (x0, y1), // top-left
                    (x1, y1), // bottom-left
                    (x1, y0), // bottom-right
                    (x0, y0), // top-right
                };

                var uvs = baseUvs;

                switch (q.Flags)
                {
                    case 2:
                        uvs = new[] { baseUvs[0], baseUvs[2], baseUvs[3], baseUvs[3] };
                        break;
                    case 11:
                        uvs = new[] { baseUvs[1], baseUvs[0], baseUvs[3], baseUvs[2] };
                        break;
                }

                for (int j = 0; j < 4; j++)
                {
                    if (!uvDict.TryGetValue(uvs[j], out int idx))
                    {
                        idx = uvList.Count + 1;
                        uvDict[uvs[j]] = idx;
                        uvList.Add(uvs[j]);
                    }
                    uvIndices[j] = idx;
                }

                faceUvs.Add(uvIndices);
            }

            // Write UVs
            foreach (var uv in uvList)
            {
                sw.WriteLine($"vt {uv.Item1:F6} {1 - uv.Item2:F6}"); // Flip Y for OBJ
            }

            // Write faces with material switching
            string currentMtl = null;
            for (int i = 0; i < quads.Count; i++)
            {
                var q = quads[i];
                var uv = faceUvs[i];

                // Resolve which BX section this texIndex belongs to
                int texGroup = 0;
                int localIndex = q.TexIndex;
                for (int t = 0; t < 5; t++)
                {
                    int count = uvRects[t].Count;
                    if (localIndex < count)
                    {
                        texGroup = t;
                        break;
                    }
                    localIndex -= count;
                }

                string matName = $"Texture{texGroup:D2}";
                if (matName != currentMtl)
                {
                    currentMtl = matName;
                    sw.WriteLine($"usemtl {matName}");
                }
                // Validate vertex indices
                if (q.A >= vertices.Count || q.B >= vertices.Count || q.C >= vertices.Count || (q.D != -1 && q.D >= vertices.Count))
                {
                    Debug.WriteLine($"Skipping invalid face at index {i}");
                    continue;
                }
                // Faces
                if ((uint)q.D == 0xFFFFFFFF)
                {
                    sw.WriteLine($"f {q.A + 1}/{uv[0]} {q.B + 1}/{uv[1]} {q.C + 1}/{uv[2]}");
                }
                else
                {
                    sw.WriteLine($"f {q.A + 1}/{uv[0]} {q.B + 1}/{uv[1]} {q.C + 1}/{uv[2]} {q.D + 1}/{uv[3]}");
                }
            }

            // Export to OBJ
            MessageBox.Show($"Exported {levelName} with UVs!");
        }
        public static void ExportModel(string modelName, List<BndSection> uvSections, List<BndSection> modelSections, string textureName, string outputPath)
        {
            bool special = false;
            List<BndSection> altSections = uvSections; // for OBJ3D special case
            string backupName = textureName; // for OBJ3D special case
            if (modelName == "OBJ3D")
            {
                special = true; // OBJ3D has special handling
            }
            for (int m = 0; m < modelSections.Count; m++)
            {
                using var br = new BinaryReader(new MemoryStream(modelSections[m].Data));
                var uvRects = ParseBxRectangles(uvSections[0].Data); // PICKGFX / OBJ3D case
                if (uvSections.Count != 1 && !special) { uvRects = ParseBxRectangles(uvSections[m].Data); } // PICKGFX case
                string textureFileName = $"{textureName}";
                // 0 / 1 / 2 are fine // TODO reduce duplicate code when all cases are resolved
                if (special && m >= 3 && m <= 18) // OBJ3D LOCKERS
                {
                    string fileDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL0GFXE.16";
                    textureName = "PNL0GFXE";
                    textureFileName = textureName;
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "BX");
                    uvRects = ParseBxRectangles(uvSections[0].Data); // OBJ3D special case
                }
                else if (special && m >=19 && m <= 34) // OBJ3D BONESHIP SWITCHES
                {
                    string fileDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL1GFXE.16";
                    textureName = "PNL1GFXE";
                    textureFileName = textureName;
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "BX");
                    uvRects = ParseBxRectangles(uvSections[0].Data); // OBJ3D special case
                }
                else if (special && m == 35) // OBJ3D COIL OBSTACLE
                {
                    string fileDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL0GFXE.16";
                    textureName = "PNL0GFXE";
                    textureFileName = textureName;
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "BX");
                    uvRects = ParseBxRectangles(uvSections[0].Data); // OBJ3D special case
                }
                else if (special && m == 36) // OBJ3D special case
                {
                    // unknown
                }
                else if (special && m >= 37 && m <= 38) // OBJ3D PYLON AND COMPUTER
                {
                    uvSections = altSections; // restore previous BX sections
                }
                else if (special && m == 39 || special && m == 41) // OBJ3D special case // 39 is unknown, 41 is egg husk
                {
                    string fileDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL1GFXE.16";
                    textureName = "PNL1GFXE";
                    textureFileName = textureName;
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "BX");
                    uvRects = ParseBxRectangles(uvSections[0].Data); // OBJ3D special case
                }
                else if (special && m == 40) // OBJ3D POD COVER
                {
                    // OBJ3D POD COVER
                }


                br.ReadBytes(12); // OBJ1 + unknown

                int quadCount = br.ReadInt32();
                int vertexCount = br.ReadInt32();

                var quads = new List<(int A, int B, int C, int D, ushort TexIndex, ushort Flags)>();
                var vertices = new List<(short X, short Y, short Z)>();

                for (int i = 0; i < quadCount; i++)
                {
                    int a = br.ReadInt32();
                    int b = br.ReadInt32();
                    int c = br.ReadInt32();
                    int d = br.ReadInt32();
                    ushort texIndex = br.ReadUInt16();
                    ushort flags = br.ReadUInt16();

                    quads.Add((a, b, c, d, texIndex, flags));
                }

                for (int i = 0; i < vertexCount; i++)
                {
                    short x = br.ReadInt16();
                    short y = br.ReadInt16();
                    short z = br.ReadInt16();
                    br.ReadUInt16(); // padding
                    vertices.Add((x, y, z));
                }

                string nameAndNumber = $"{modelName}_{modelSections[m].Name}";

                string objPath = outputPath + $"\\{nameAndNumber}.obj";
                using var sw = new StreamWriter(objPath);

                sw.WriteLine($"# OBJ exported from Alien Trilogy {nameAndNumber}");

                sw.WriteLine($"mtllib {nameAndNumber}.mtl");
                sw.WriteLine("usemtl Texture01");

                if (uvSections.Count != 1 && !special) // PICKGFX / OBJ3D case
                {
                    textureFileName = $"{textureName}_TP{m:D2}";
                }
                File.WriteAllText(outputPath + $"\\{nameAndNumber}.mtl", $"newmtl Texture01\nmap_Kd {textureFileName}.png\n");

                // Write vertex positions
                foreach (var v in vertices)
                {
                    sw.WriteLine($"v {v.X:F4} {v.Y:F4} {v.Z:F4}");
                }

                // Store unique UVs and their indices
                var uvDict = new Dictionary<(float, float), int>();
                var uvList = new List<(float, float)>();

                // Map of per-face vertex UV indices
                var faceUvs = new List<int[]>();

                foreach (var q in quads)
                {
                    var uvIndices = new int[4];

                    if (q.TexIndex >= uvRects.Count)
                    {
                        Array.Fill(uvIndices, 1);
                        faceUvs.Add(uvIndices);
                        continue;
                    }

                    var rect = uvRects[q.TexIndex];
                    float x0 = rect.X / texSize;
                    float y0 = rect.Y / texSize;
                    float x1 = (rect.X + rect.Width) / texSize;
                    float y1 = (rect.Y + rect.Height) / texSize;

                    var baseUvs = new (float, float)[]
                    {
                        (x0, y1), // A → top-left
                        (x1, y1), // B → bottom-left
                        (x1, y0), // C → bottom-right
                        (x0, y0), // D → top-right
                    };

                    var uvs = baseUvs;

                    switch (q.Flags)
                    {
                        case 2:
                            // Triangle with special order: A → 0, C → 2, D → 3
                            uvs = new[] { baseUvs[0], baseUvs[2], baseUvs[3], baseUvs[3] };
                            break;
                        case 11:
                            // Flip texture 180
                            uvs = new[] { baseUvs[1], baseUvs[0], baseUvs[3], baseUvs[2] };
                            break;
                        default:
                            // Standard quad order
                            uvs = baseUvs;
                            break;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (!uvDict.TryGetValue(uvs[i], out int idx))
                        {
                            idx = uvList.Count + 1;
                            uvDict[uvs[i]] = idx;
                            uvList.Add(uvs[i]);
                        }
                        uvIndices[i] = idx;
                    }

                    faceUvs.Add(uvIndices);
                }

                // Write UVs
                foreach (var uv in uvList)
                {
                    sw.WriteLine($"vt {uv.Item1:F6} {1 - uv.Item2:F6}"); // Flip Y for OBJ
                }

                // Write faces
                for (int i = 0; i < quads.Count; i++)
                {
                    var q = quads[i];
                    var uv = faceUvs[i];

                    if ((uint)q.D == 0xFFFFFFFF)
                    {
                        sw.WriteLine($"f {q.A + 1}/{uv[0]} {q.B + 1}/{uv[1]} {q.C + 1}/{uv[2]}");
                    }
                    else
                    {
                        sw.WriteLine($"f {q.A + 1}/{uv[0]} {q.B + 1}/{uv[1]} {q.C + 1}/{uv[2]} {q.D + 1}/{uv[3]}");
                    }
                }
            }
            MessageBox.Show("Exported OBJ with UVs!");
        }
        // Parse BX rectangles from the BX section data
        public static List<(int X, int Y, int Width, int Height)> ParseBxRectangles(byte[] bxData)
        {
            var rectangles = new List<(int X, int Y, int Width, int Height)>();
            using var ms = new MemoryStream(bxData);
            using var br = new BinaryReader(ms);

            int rectCount = br.ReadInt16();

            for (int i = 0; i < rectCount; i++)
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte width = br.ReadByte();
                byte height = br.ReadByte();
                br.ReadBytes(2); // unknown bytes

                rectangles.Add((x, y, width + 1, height + 1));
            }
            return rectangles;
        }
    }
}
