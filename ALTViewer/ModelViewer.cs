using System.Diagnostics;

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
            ToolTip tooltip = new ToolTip(); // no tooltips added yet
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(Label), typeof(ListBox) });
            ListModels();
        }
        private void ListModels()
        {
            string[] models = { "OBJ3D", "OPTOBJ", "PICKMOD" }; // known model files
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
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a model to export.");
                return;
            }
            string modelDirectory = "";
            string textureDirectory = "";
            string textureName = "";
            string caseName = "";
            switch (listBox1.SelectedItem) // check which model is selected
            {
                case "OBJ3D":
                    modelDirectory = gfxDirectory + "\\" + "OBJ3D.BND";
                    textureDirectory = gfxDirectory + "\\" + "PICKGFX.BND"; // currently unknown
                    textureName = "PICKGFX"; // temporary assignment while the texture is unknown
                    caseName = "OBJ3D"; // possibly PICKGFX.BND with only one BX section?
                    break;
                case "OPTOBJ":
                    modelDirectory = gfxDirectory + "\\" + "OPTOBJ.BND";
                    textureDirectory = gfxDirectory + "\\" + "OPTGFX.BND";
                    textureName = "OPTGFX";
                    caseName = "OPTOBJ";
                    break;
                case "PICKMOD":
                    modelDirectory = gfxDirectory + "\\" + "PICKMOD.BND";
                    textureDirectory = gfxDirectory + "\\" + "PICKGFX.BND";
                    textureName = "PICKGFX";
                    caseName = "PICKMOD";
                    break;
            }
            if (!File.Exists(textureDirectory)) // check texture file exists
            {
                MessageBox.Show($"Associated graphics file {caseName}.BND does not exist!");
                return;
            }
            List<BndSection> modelSections = null!;
            modelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(modelDirectory), "M0");
            ModelRenderer.ExportModel(caseName, textureDirectory, modelSections, textureName, outputPath);
        }
        // double click to open output path
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        { if (outputPath != "") { Process.Start(new ProcessStartInfo() { FileName = outputPath, UseShellExecute = true, Verb = "open" }); } }
    }
}