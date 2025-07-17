namespace ALTViewer
{
    public static class ModelRenderer
    {
        public static void ExportLevel(string levelName, List<BndSection> uvSections, byte[] levelSection, string textureName, string outputPath)
        {

        }
        public static void ExportModel(string modelName, List<BndSection> uvSections, List<BndSection> modelSections, string textureName, string outputPath)
        {
            const float texSize = 256f;
            for (int m = 0; m < modelSections.Count; m++)
            {
                using var ms = new MemoryStream(modelSections[m].Data);
                using var br = new BinaryReader(ms);
                var uvRects = ParseBxRectangles(uvSections[0].Data);

                if (uvSections.Count != 1) // PICKGFX / OBJ3D case
                {
                    uvRects = ParseBxRectangles(uvSections[m].Data);
                }
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
                    sw.WriteLine($"v {v.X / 100.0f:F4} {v.Y / 100.0f:F4} {v.Z / 100.0f:F4}");
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
                        (x0, y0), // Top-left
                        (x1, y0), // Top-right
                        (x1, y1), // Bottom-right
                        (x0, y1), // Bottom-left
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
                byte xOffset = br.ReadByte();
                byte yOffset = br.ReadByte();

                int x = 256 - xOffset - width; // Correct X calculation from right edge
                int y = 256 - yOffset - height; // Correct Y calculation from bottom edge

                rectangles.Add((x, y, width + 1, height + 1));
            }
            return rectangles;
        }
    }
}
