namespace ALTViewer
{
    public partial class ModelViewer : Form
    {
        private string gameDirectory = ""; // default directories
        private string gfxDirectory = "";
        private string outputPath = ""; // output path for exported files
        public ModelViewer()
        {
            InitializeComponent();
            gameDirectory = Utilities.CheckDirectory();
            gfxDirectory = gameDirectory + "GFX";
            ListModels();
        }
        private void ListModels()
        {
            string[] models = { "OBJ3D", "OPTOBJ" }; // known model files
            foreach (string model in models)
            {
                if (File.Exists(gfxDirectory + "\\" + model + ".BND"))
                {
                    listBox1.Items.Add(model); // add model to list box
                }
            }
        }
        // select output path
        private void button1_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Select output folder to save exported files.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputPath = fbd.SelectedPath;
                textBox1.Text = outputPath; // update text box with selected path
                button2.Enabled = true; // enable export button
            }
        }
        // export selected model
        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a model to export.");
                return;
            }
            string fileDirectory = "";
            string textureDirectory = gfxDirectory + "\\" + "OPTGFX.BND";
            string textureName = "";
            string caseName = "";
            List<BndSection> uvSections = null!;
            List<BndSection> modelSections = null!;
            switch (listBox1.SelectedItem) // check which model is selected
            {
                case "OBJ3D":
                    fileDirectory = gfxDirectory + "\\" + "OBJ3D.BND";
                    textureDirectory = gfxDirectory + "\\" + "OPTGFX.BND"; // currently unknown
                    textureName = "OPTGFX"; // temporary assignment while the texture is unknown
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
                    modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "M0");
                    caseName = "OBJ3D";
                    break;
                case "OPTOBJ":
                    fileDirectory = gfxDirectory + "\\" + "OPTOBJ.BND";
                    textureDirectory = gfxDirectory + "\\" + "OPTGFX.BND";
                    textureName = "OPTGFX";
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
                    //List<BndSection> textureSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "TP");
                    modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "M0");
                    caseName = "OPTOBJ";
                    break;
            }
            if (!File.Exists(textureDirectory))
            {
                MessageBox.Show($"Associated graphics file {caseName}.BND does not exist!");
                return;
            }
            ExportModel(caseName, uvSections, modelSections, textureName);
        }
        private void ExportModel(string modelName, List<BndSection> uvSections, List<BndSection> modelSections, string textureName)
        {
            const float texSize = 256f;

            for(int m = 0; m < modelSections.Count; m++)
            {
                using var ms = new MemoryStream(modelSections[m].Data);
                using var br = new BinaryReader(ms);
                var uvRects = ParseBxRectangles(uvSections[m].Data);

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
                
                File.WriteAllText(outputPath + $"\\{nameAndNumber}.mtl", $"newmtl Texture01\nmap_Kd {textureName}_TP{m:D2}.png\n");

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
        private List<(int X, int Y, int Width, int Height)> ParseBxRectangles(byte[] bxData)
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
