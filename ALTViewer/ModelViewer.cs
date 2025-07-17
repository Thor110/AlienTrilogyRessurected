using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

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
            List<BndSection> uvSections = null!;
            List<BndSection> modelSections = null!;
            // check which model is selected
            switch (listBox1.SelectedItem)
            {
                case "OBJ3D":
                    fileDirectory = gfxDirectory + "\\" + "OBJ3D.BND";
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
                    modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "M0");
                    ExportModel("OBJ3D", uvSections, modelSections);
                    break;
                case "OPTOBJ":
                    fileDirectory = gfxDirectory + "\\" + "OPTOBJ.BND";
                    textureDirectory = gfxDirectory + "\\" + "OPTGFX.BND";
                    if (!File.Exists(textureDirectory))
                    {
                        MessageBox.Show("Associated graphics file OPTGFX.BND does not exist!");
                        return;
                    }
                    uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
                    //List<BndSection> textureSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "TP");
                    modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "M0");
                    ExportModel("OPTOBJ", uvSections, modelSections);
                    break;
            }
        }
        private void ExportModel(string modelName, List<BndSection> uvSections, List<BndSection> modelSections)
        {
            foreach (var modelSection in modelSections)
            {
                using var ms = new MemoryStream(modelSection.Data);
                using var br = new BinaryReader(ms);
                // Skip until "OBJ1"
                while (ms.Position < ms.Length - 4)
                {
                    string chunk = Encoding.ASCII.GetString(br.ReadBytes(4));
                    if (chunk == "OBJ1")
                        break;
                    ms.Seek(-3, SeekOrigin.Current); // Slide window
                }

                br.ReadBytes(8); // Unknown 8 bytes

                int quadCount = br.ReadInt32();
                int vertexCount = br.ReadInt32();

                var quads = new List<(int A, int B, int C, int D)>();
                var vertices = new List<(short X, short Y, short Z)>();

                for (int i = 0; i < quadCount; i++)
                {
                    int a = br.ReadInt32();
                    int b = br.ReadInt32();
                    int c = br.ReadInt32();
                    int d = br.ReadInt32();
                    ushort texIndex = br.ReadUInt16(); // not used yet
                    ushort flags = br.ReadUInt16();    // not used yet

                    quads.Add((a, b, c, d));
                }

                for (int i = 0; i < vertexCount; i++)
                {
                    short x = br.ReadInt16();
                    short y = br.ReadInt16();
                    short z = br.ReadInt16();
                    br.ReadUInt16(); // padding or unused
                    vertices.Add((x, y, z));
                }

                // Write OBJ
                string objPath = Path.Combine(outputPath, $"{modelName}_{modelSection.Name}.obj");
                using var sw = new StreamWriter(objPath);

                sw.WriteLine($"# OBJ exported from Alien Trilogy {modelSection.Name}");

                // Write vertices
                foreach (var v in vertices)
                {
                    sw.WriteLine($"v {v.X / 100.0f:F4} {v.Y / 100.0f:F4} {v.Z / 100.0f:F4}");
                }

                // Write faces
                foreach (var q in quads)
                {
                    if (q.D == -1)
                    {
                        // Triangle
                        sw.WriteLine($"f {q.A + 1} {q.B + 1} {q.C + 1}");
                    }
                    else
                    {
                        // Quad
                        sw.WriteLine($"f {q.A + 1} {q.B + 1} {q.C + 1} {q.D + 1}");
                    }
                }
            }
            MessageBox.Show("Exported OBJ!");
        }
    }
}
