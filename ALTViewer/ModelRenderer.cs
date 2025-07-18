namespace ALTViewer
{
    public static class ModelRenderer
    {
        public static void ExportLevel(string levelName, List<BndSection> uvSections, byte[] levelSection, string textureName, string outputPath)
        {
            /*
            using var br = new BinaryReader(new MemoryStream(levelSection));
            ushort vertCount = br.ReadUInt16();
            ushort quadCount = br.ReadUInt16();
            ushort mapLength = br.ReadUInt16();
            ushort mapWidth = br.ReadUInt16();
            ushort playerStartX = br.ReadUInt16();
            ushort playerStartY = br.ReadUInt16();
            br.ReadBytes(2); // unknown 1
            ushort monster = br.ReadUInt16();
            ushort pickups = br.ReadUInt16();
            ushort boxes = br.ReadUInt16();
            ushort doors = br.ReadUInt16();
            br.ReadBytes(2); // unknown 2
            ushort playerStartAngle = br.ReadUInt16();
            br.ReadBytes(10); // unknown 3 & 4
            */
            // Count Vertices and Quads
            /*List<Vector2> vertices = new();
            for (int i = 0; i < vertCount; i++)
            {
                ushort x = br.ReadUInt16();
                ushort y = br.ReadUInt16();
                vertices.Add(new Vector2(x, y));
            }*/
            /*List<(ushort A, ushort B, ushort C, ushort D)> quads = new();
            for (int i = 0; i < quadCount; i++)
            {
                ushort a = br.ReadUInt16();
                ushort b = br.ReadUInt16();
                ushort c = br.ReadUInt16();
                ushort d = br.ReadUInt16();
                quads.Add((a, b, c, d));
            }*/
            // Read UV rectangles BX00-BX04
            // List Textures TP00-TP04
            // Parse other objects in the level section

            // Export to OBJ
            MessageBox.Show("Exported OBJ with UVs!");
        }
        public static void ExportModel(string modelName, List<BndSection> uvSections, List<BndSection> modelSections, string textureName, string outputPath)
        {
            const float texSize = 256f;
            for (int m = 0; m < modelSections.Count; m++)
            {
                using var br = new BinaryReader(new MemoryStream(modelSections[m].Data));
                var uvRects = ParseBxRectangles(uvSections[0].Data); // PICKGFX / OBJ3D case
                if (uvSections.Count != 1) { uvRects = ParseBxRectangles(uvSections[m].Data); } // else PICKGFX / OBJ3D case

                br.ReadBytes(12); // OBJ1 + unknown

                int quadCount = br.ReadInt32();
                int vertexCount = br.ReadInt32();

                var quads = new List<(int A, int B, int C, int D, ushort TexIndex)>();
                var vertices = new List<(short X, short Y, short Z)>();

                for (int i = 0; i < quadCount; i++)
                {
                    int a = br.ReadInt32();
                    int b = br.ReadInt32();
                    int c = br.ReadInt32();
                    int d = br.ReadInt32();
                    ushort texIndex = br.ReadUInt16();
                    ushort flags = br.ReadUInt16(); // unused for now

                    quads.Add((a, b, c, d, texIndex));
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
                string textureFileName = $"{textureName}_TP00";

                if (uvSections.Count != 1) // PICKGFX / OBJ3D case
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

                    var uvs = new (float, float)[]
                    {
                        (x0, y0), // A → top-left
                        (x0, y1), // B → bottom-left
                        (x1, y1), // C → bottom-right
                        (x1, y0), // D → top-right
                    };

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

                    if (q.D == -1)
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
                byte width = br.ReadByte();
                byte height = br.ReadByte();
                byte unk1 = br.ReadByte();
                byte unk2 = br.ReadByte();
                byte x = br.ReadByte();
                byte y = br.ReadByte();

                rectangles.Add((x, y, width, height));
            }
            return rectangles;
        }
    }
}
