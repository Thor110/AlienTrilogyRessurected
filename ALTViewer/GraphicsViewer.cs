namespace ALTViewer
{
    public partial class GraphicsViewer : Form
    {
        //directories
        public static string gameDirectory = "HDD\\TRILOGY\\CD";
        public static string gfxDirectory = Path.Combine(gameDirectory, "GFX");
        public static string paletteDirectory = Path.Combine(gameDirectory, "PALS");
        public static string enemyDirectory = Path.Combine(gameDirectory, "NME");
        public static string languageDirectory = Path.Combine(gameDirectory, "LANGUAGE");
        public static string levelPath1 = Path.Combine(gameDirectory, "SECT11");
        public static string levelPath2 = Path.Combine(gameDirectory, "SECT12");
        public static string levelPath3 = Path.Combine(gameDirectory, "SECT21");
        public static string levelPath4 = Path.Combine(gameDirectory, "SECT22");
        public static string levelPath5 = Path.Combine(gameDirectory, "SECT31");
        public static string levelPath6 = Path.Combine(gameDirectory, "SECT32");
        public static string levelPath7 = Path.Combine(gameDirectory, "SECT90");
        public static string[] levels = new string[] { levelPath1, levelPath2, levelPath3, levelPath4, levelPath5, levelPath6, levelPath7 };
        public GraphicsViewer()
        {
            InitializeComponent();
            radioButton1_CheckedChanged(this, null!); // Load graphics files on startup
            // test render
            byte[] tntBytes = File.ReadAllBytes("LEGAL.TNT");
            byte[] bndBytes = File.ReadAllBytes("LEGAL.BND");
            byte[] palBytes = File.ReadAllBytes("LEGAL.PAL");
            pictureBox1.Image = TileRenderer.RenderTiledImage(tntBytes, bndBytes, palBytes);
        }
        // graphics GFX
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListFiles(gfxDirectory);
        }
        // enemies NME
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListFiles(enemyDirectory);
        } // no .BIN in NME folder
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string level in levels) { ListFiles(level); } // no .BND in SECT folders
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListFiles(languageDirectory, ".NOPE", ".16");
        }
        public void ListFiles(string path, string type1 = ".BND", string type2 = ".BIN", string type3 = ".B16")
        {
            string[] files = DiscoverFiles(path, type1, type2, type3);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (!listBox1.Items.Contains(fileName)) { listBox1.Items.Add(fileName); }
            }
        }
        private string[] DiscoverFiles(string path, string type1, string type2, string type3)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(type1) || file.EndsWith(type2) || file.EndsWith(type3))
                .ToArray();
        }
    }
}
