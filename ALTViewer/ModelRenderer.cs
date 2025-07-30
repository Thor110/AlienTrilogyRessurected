using System.Diagnostics;
using System.Drawing.Imaging;

namespace ALTViewer
{
    public static class ModelRenderer
    {
        public const float texSize = 256f;
        public static int[] unknownValues = new int[]
        {
            3, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
            22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 38,
            40, 43, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 59, 61,
            62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 75, 77, 78, 79,
            80, 81, 82, 83, 84, 85, 87, 88, 89, 90, 91, 93, 94, 95, 96, 97,
            98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
            111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123,
            124, 125, 126, 127, 128, 133, 134, 135, 136, 137, 138, 139, 140,
            141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153,
            154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 166, 168,
            171, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184,
            187, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200,
            201, 203, 205, 206, 207, 208, 209, 210, 211, 212, 213, 215, 216,
            217, 218, 219, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231,
            232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244,
            245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255
        };
        public static int[] textureFlags = new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 10, 12, 13, 14, 26, 28, 30, 32, 34, 255 };
        public static void ExportLevel(string levelName, List<BndSection> uvSections, byte[] levelSection, string textureName, string outputPath, bool debug, bool unknown)
        {
            using var br = new BinaryReader(new MemoryStream(levelSection)); // skip first 20 bytes + 36 below = 56
            ushort vertCount = br.ReadUInt16();         // Number of vertices
            ushort quadCount = br.ReadUInt16();         // Number of quads
            /* // uncomment if you want to read the level header
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
            */ // comment out the next line if you want to read the level header
            br.BaseStream.Seek(32, SeekOrigin.Current); // Skip 36 bytes to reach vertex and quad data
            List<(short X, short Y, short Z)> vertices = new();
            for (int i = 0; i < vertCount; i++) // Count Vertices
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short z = br.ReadInt16();
                br.ReadBytes(2); // unknown bytes
                vertices.Add((x, y, z));
            }
            List<(int A, int B, int C, int D, ushort TexIndex, byte Flags, byte Other)> quads = new();
            for (int i = 0; i < quadCount; i++) // Count Quads
            {
                int a = br.ReadInt32();
                int b = br.ReadInt32();
                int c = br.ReadInt32();
                int d = br.ReadInt32();
                ushort texIndex = br.ReadUInt16(); // signed or unsigned?
                byte flags = br.ReadByte();
                byte other = br.ReadByte(); // unknown byte

                quads.Add((a, b, c, d, texIndex, flags, other));
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


            if (debug)
            {
                foreach (int f in textureFlags)
                {
                    if (f == 255)
                    {
                        mtlWriter.WriteLine($"newmtl Texture{f}");
                        mtlWriter.WriteLine($"map_Kd FLAGS{f}.png");
                    }
                    else
                    {
                        mtlWriter.WriteLine($"newmtl Texture{f:D2}");
                        mtlWriter.WriteLine($"map_Kd FLAGS{f}.png");
                    }
                }
            }
            else if (unknown)
            {
                for (int t = 0; t < 222; t++)
                {
                    mtlWriter.WriteLine($"newmtl UnkByte_{unknownValues[t]}");
                    mtlWriter.WriteLine($"map_Kd UnkByte_{unknownValues[t]}.png");
                }
            }
            else
            {
                for (int t = 0; t < 5; t++)
                {
                    mtlWriter.WriteLine($"newmtl Texture{t:D2}");
                    mtlWriter.WriteLine($"map_Kd {textureName}_TP{t:D2}.png");
                }
            }
            // Write vertex positions
            foreach (var v in vertices)
            {
                sw.WriteLine($"v {v.X:F4} {v.Y:F4} {v.Z:F4}");
            }

            //using var unkWriter = new StreamWriter(Path.Combine(outputPath, $"{levelName}.unk"));
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
                        // Flip texture 180
                        uvs = new[] { baseUvs[1], baseUvs[0], baseUvs[3], baseUvs[2] };
                        break;
                    case 11:
                        // Triangle with special order: A → 0, C → 2, D → 3
                        uvs = new[] { baseUvs[0], baseUvs[2], baseUvs[3], baseUvs[3] };
                        break;
                    default:
                        // Standard quad order
                        uvs = baseUvs;
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
            string currentMtl = null!;
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
                //
                string matName = "";
                if (debug)
                {
                    if(q.Flags == 255) { matName = $"usemtl UnkByte_{q.Flags:D3}"; }
                    else { matName = $"usemtl UnkByte_{q.Flags:D2}"; }
                }
                else if (unknown) { matName = $"usemtl UnkByte_{q.Other}"; }
                else { matName = $"Texture{texGroup:D2}"; }

                if (matName != currentMtl)
                {
                    currentMtl = matName;
                    if(debug)
                    {
                        if (q.Flags == 255) { sw.WriteLine($"usemtl Texture{q.Flags:D3}"); }
                        else { sw.WriteLine($"usemtl Texture{q.Flags:D2}"); }
                    }
                    else if (unknown)
                    {
                        sw.WriteLine($"usemtl UnkByte_{q.Other}");
                    }
                    else
                    {
                        sw.WriteLine($"usemtl {matName}");
                    }   
                }
                // Validate vertex indices
                if (q.A < 0 || q.B < 0 || q.C < 0 || q.A >= vertices.Count || q.B >= vertices.Count || q.C >= vertices.Count)
                {
                    Debug.WriteLine($"Skipping invalid triangle at face {i}");
                    continue;
                }
                // Faces
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
        public static void GenerateFlagTextures(string outputDir, string levelName)
        {
            for (int i = 0; i < 18; i++) // textureFlags.Count = 18
            {
                using var bmp = new Bitmap(256, 256);
                using var g = Graphics.FromImage(bmp);

                switch (textureFlags[i])
                {
                    case 0: g.Clear(Color.Black); break;
                    case 1: g.Clear(Color.DarkGray); break;
                    case 2: g.Clear(Color.DarkRed); break;
                    case 3: g.Clear(Color.Red); break;
                    case 4: g.Clear(Color.Orange); break;
                    case 5: g.Clear(Color.Yellow); break;
                    case 6: g.Clear(Color.Green); break;
                    case 8: g.Clear(Color.Blue); break;
                    case 10: g.Clear(Color.DarkBlue); break;
                    case 12: g.Clear(Color.Purple); break;
                    case 13: g.Clear(Color.White); break;
                    case 14: g.Clear(Color.LightGray); break;
                    case 26: g.Clear(Color.Brown); break;
                    case 28: g.Clear(Color.Pink); break;
                    case 30: g.Clear(Color.Gold); break;
                    case 32: g.Clear(Color.Tan); break;
                    case 34: g.Clear(Color.LimeGreen); break;
                    case 255: g.Clear(Color.SkyBlue); break;
                }
                
                bmp.Save(Path.Combine(outputDir, $"FLAGS{textureFlags[i]}.png"), ImageFormat.Png);
            }
        }
        public static void GenerateUnknownTextures(string outputDir, string levelName)
        {
            for (int i = 0; i < 222; i++) // unknownValues.Count = 222
            {
                using var bmp = new Bitmap(256, 256);
                using var g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);

                string number = $"{unknownValues[i]}";
                using var font = new Font("Arial", 8, FontStyle.Bold);
                using var brush = new SolidBrush(Color.White);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        int tileIndex = y * 8 + x;
                        if (tileIndex >= 64) break;

                        Rectangle tileRect = new Rectangle(x * 32, y * 32, 32, 32);
                        g.DrawRectangle(Pens.Gray, tileRect);
                        g.DrawString(number, font, brush, tileRect, sf);
                    }
                }

                bmp.Save(Path.Combine(outputDir, $"UnkByte_{unknownValues[i]}.png"), ImageFormat.Png);
            }
        }
        public static void ExportModel(string modelName, string textureDirectory, string modelDirectory, string textureName, string outputPath)
        {
            bool special = false;
            List<BndSection> modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(modelDirectory), "M0");
            List<BndSection> uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX"); // PICKMOD case
            List<(int X, int Y, int Width, int Height)> uvRects = ParseBxRectangles(uvSections[0].Data); // PICKMOD case
            string backupName = textureName; // for OBJ3D special case
            string backupDirectory = textureDirectory; // for OBJ3D special case
            if (modelName == "OBJ3D") { special = true; } // OBJ3D has special handling
            for (int m = 0; m < modelSections.Count; m++)
            {
                using var br = new BinaryReader(new MemoryStream(modelSections[m].Data));
                // 0 / 1 / 2 are fine to default // TODO : reduce duplicate code when all cases are resolved
                if (special && m >= 3 && m <= 18 || special && m == 35) // OBJ3D LOCKERS & COIL OBSTACLE
                {
                    textureDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL0GFXE.16";
                    textureName = "PNL0GFXE";
                }
                else if (special && m >=19 && m <= 34 || special && m == 41) // OBJ3D BONESHIP SWITCHES && EGGHUSK
                {
                    textureDirectory = Utilities.CheckDirectory() + "LANGUAGE\\PNL1GFXE.16";
                    textureName = "PNL1GFXE";
                }
                else if (special && m == 36) // OBJ3D special case -> defaults to PICKGFX for now // UNRESOLVED
                {
                    // unknown switch maybe // definitely not PNL0GFXE, PNL1GFXE or PICKGFX
                    textureDirectory = backupDirectory; // restore previous texture directory
                    textureName = backupName; // restore previous texture name
                    // unknown switch maybe
                }
                else if (special && m >= 37 && m <= 38) // OBJ3D PYLON AND COMPUTER -> uses PICKGFX
                {
                    textureDirectory = backupDirectory; // restore previous texture directory
                    textureName = backupName; // restore previous texture name
                }
                else if (special && m == 39) // OBJ3D special case -> defaults to PICKGFX for now // UNRESOLVED
                {
                    // 39 is unknown, same model as EGGHUSK // definitely not PNL0GFXE, PNL1GFXE or PICKGFX
                    textureDirectory = backupDirectory; // restore previous texture directory
                    textureName = backupName; // restore previous texture name
                    // 39 is unknown, same model as EGGHUSK
                }
                else if (special && m == 40) // OBJ3D POD COVER -> defaults to PICKGFX for now // UNRESOLVED
                {
                    // texture unknown // definitely not PNL0GFXE, PNL1GFXE or PICKGFX
                    textureDirectory = backupDirectory; // restore previous texture directory
                    textureName = backupName; // restore previous texture name
                    // texture unknown
                }
                if (uvSections.Count != 1 && !special) // OPTOBJ case
                {
                    textureName = $"{backupName}_TP{m:D2}";
                    uvRects = ParseBxRectangles(uvSections[m].Data);
                }
                else if (special) // OBJ3D case // loads the texture file once per model section, try to reduce maybe...
                {
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
                    uvRects = ParseBxRectangles(uvSections[0].Data);
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

                File.WriteAllText(outputPath + $"\\{nameAndNumber}.mtl", $"newmtl Texture01\nmap_Kd {textureName}.png\n");

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
        // export doors or lifts as OBJ
        public static void ExportDoorLift(string levelName, List<BndSection> uvSections, byte[] levelSection, string textureName, string outputPath)
        {
            using var br = new BinaryReader(new MemoryStream(levelSection));
            br.BaseStream.Seek(12, SeekOrigin.Current); // Skip 12 bytes to reach vertex and quad data

            int quadCount = br.ReadInt32();         // Number of quads
            int vertCount = br.ReadInt32();         // Number of vertices

            List<(int A, int B, int C, int D, ushort TexIndex, ushort Flags)> quads = new();
            List<(short X, short Y, short Z)> vertices = new();

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

            for (int i = 0; i < vertCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short z = br.ReadInt16();
                br.ReadUInt16(); // padding
                vertices.Add((x, y, z));
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

                // levels and lifts
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
            string currentMtl = null!;
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
                //
                string matName = $"Texture{texGroup:D2}";

                if (matName != currentMtl)
                {
                    currentMtl = matName;
                    sw.WriteLine($"usemtl {matName}");
                }
                // Validate vertex indices
                if (q.A < 0 || q.B < 0 || q.C < 0 || q.A >= vertices.Count || q.B >= vertices.Count || q.C >= vertices.Count)
                {
                    Debug.WriteLine($"Skipping invalid triangle at face {i}");
                    continue;
                }
                // Faces
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
    }
}
