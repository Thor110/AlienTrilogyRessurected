namespace ALTViewer
{
    public partial class GraphicsViewer : Form
    {
        // default directories
        public static string gameDirectory = "HDD\\TRILOGY\\CD";
        public static string gfxDirectory = Path.Combine(gameDirectory, "GFX"); // BND / B16 / BIN
        public static string paletteDirectory = Path.Combine(gameDirectory, "PALS"); // TNT / DPQ / PAL
        public static string enemyDirectory = Path.Combine(gameDirectory, "NME"); // BND / B16
        public static string languageDirectory = Path.Combine(gameDirectory, "LANGUAGE"); // BND / 16
        public static string levelPath1 = Path.Combine(gameDirectory, "SECT11"); // BIN / B16
        public static string levelPath2 = Path.Combine(gameDirectory, "SECT12"); // BIN / B16
        public static string levelPath3 = Path.Combine(gameDirectory, "SECT21"); // BIN / B16
        public static string levelPath4 = Path.Combine(gameDirectory, "SECT22"); // BIN / B16
        public static string levelPath5 = Path.Combine(gameDirectory, "SECT31"); // BIN / B16
        public static string levelPath6 = Path.Combine(gameDirectory, "SECT32"); // BIN / B16
        public static string levelPath7 = Path.Combine(gameDirectory, "SECT90"); // BIN / B16
        public static string[] levels = new string[] { levelPath1, levelPath2, levelPath3, levelPath4, levelPath5, levelPath6, levelPath7 };
        private string lastSelectedFile = "";
        private string lastSelectedPalette = "";
        public GraphicsViewer()
        {
            InitializeComponent();
            GetPalettes(); // Load palettes from the palette directory
            ListFiles(gfxDirectory); // Load graphics files by default on startup
            //RenderImage("LEGAL.TNT", "LEGAL.BND", "LEGAL.PAL"); // Clear the image on startup
        }
        // Load palettes from the palette directory
        private void GetPalettes()
        {
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*.PAL");
            foreach (string palFile in palFiles)
            {
                listBox2.Items.Add(Path.GetFileNameWithoutExtension(palFile));
            }
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
        }
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string level in levels) { ListFiles(level); }
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListFiles(languageDirectory, ".BND", ".NOPE", ".16");
        }
        // list files in directory
        public void ListFiles(string path, string type1 = ".BND", string type2 = ".BIN", string type3 = ".B16")
        {
            string[] files = DiscoverFiles(path, type1, type2, type3);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (!listBox1.Items.Contains(fileName)) { listBox1.Items.Add(fileName); }
            }
        }
        // discover files in directory
        private string[] DiscoverFiles(string path, string type1, string type2, string type3)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(type1) || file.EndsWith(type2) || file.EndsWith(type3))
                .ToArray();
        }
        // display selected file
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox1.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedFile) { return; } // do not reselect same file
            lastSelectedFile = selected; // store last selected file
            // show palette controls
            label1.Visible = true; // show label
            listBox2.Visible = true; // show palette list
            button1.Visible = true; // show re-detect palette button
            // determine which directory to use based on selected radio button
            if (radioButton1.Checked) { GetFile(gfxDirectory); }
            else if (radioButton2.Checked) { GetFile(enemyDirectory); }
            else if (radioButton3.Checked)
            {
                foreach (string level in levels) // determine level folder based on selected item
                {
                    if (File.Exists(listBox1.SelectedItem!.ToString()! + ".BIN"))
                    {
                        GetFile(level);
                        return; // exit after finding the first matching level
                    }
                }
            }
            else if (radioButton4.Checked) { GetFile(languageDirectory); }
        }
        private void GetFile(string path)
        {
            string selected = listBox1.SelectedItem!.ToString()!; // get selected item
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(selected)); // detect palette for the selected item
            string filePathA = Path.Combine(path, selected + ".BND");
            string filePathB = Path.Combine(path, selected + ".BIN");
            string fileLookup = "";
            if (File.Exists(filePathA))
            {
                byte[] bndBytes = File.ReadAllBytes(filePathA);
                // Process the BND file as needed
                // For example, display its contents or render it
                // get palette and associated files
                //MessageBox.Show(selected);
                //MessageBox.Show(chosen);
                SelectPalette(chosen); // select the detected palette
                RenderImage("", filePathA, chosen);
                return;
            }
            fileLookup = filePathA;
            if (File.Exists(filePathB))
            {
                byte[] bndBytes = File.ReadAllBytes(filePathB);
                // Process the BND file as needed
                // For example, display its contents or render it
                // get palette and associated files
                SelectPalette(chosen); // select the detected palette
                RenderImage("", filePathB, chosen);
                return;
            }
            fileLookup = filePathB;
            MessageBox.Show("Selected file does not exist: " + fileLookup);
        }
        private string DetectPalette(string filename)
        {
            string palette = Path.Combine(paletteDirectory, filename + ".PAL");
            MessageBox.Show(filename);
            //
            // discover the palettes for the following files
            //DEMO111
            //DEMO211
            //DEMO311
            //EXPLGFX ( Possibly GUNPALS.PAL )
            //OBJ3D
            //OPTGFX
            //OPTOBJ
            //PICKGFX
            //PICKMOD
            //
            // hard coded palette lookups
            if (filename == "FONT1GFX")
            {
                return Path.Combine(paletteDirectory, "NEWFONT.PAL");
            }
            string[] hardcodedPalettes = new string[]
            {
                "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART"
            };
            if(hardcodedPalettes.Contains(filename))
            {
                return Path.Combine(paletteDirectory, "GUNPALS.PAL");
            }
            if (radioButton2.Checked)
            {
                return Path.Combine(paletteDirectory, "SPRITES.PAL");
            }
            else if (radioButton4.Checked || filename.Contains("PANEL"))
            {
                return Path.Combine(paletteDirectory, "PANEL.PAL");
            }
            if (!File.Exists(palette))
            {
                MessageBox.Show("No palette found for " + filename);
                return ""; // no palette found
            }
            else
            {
                return Path.Combine(paletteDirectory, filename + ".PAL");
            }
        }
        private void RenderImage(string tnt, string binbnd, string pal)
        {
            pictureBox1.Image = null; // clear previous image

            //testing
            tnt = "LEGAL.TNT";
            binbnd = "LEGAL.BND";
            pal = "LEGAL.PAL";

            if (pal == "") { return; } // do not render without palette
            if (tnt == "") { return; } // do not render without tnt? b16 16 or DPQ?
            // test render
            byte[] tntBytes = File.ReadAllBytes(tnt);
            byte[] bndBytes = File.ReadAllBytes(binbnd);
            byte[] palBytes = File.ReadAllBytes(pal);
            pictureBox1.Image = TileRenderer.RenderTiledImage(tntBytes, bndBytes, palBytes);
        }
        // re-detect image palette and refresh the image
        private void button1_Click(object sender, EventArgs e)
        {
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(listBox1.SelectedItem!.ToString()!)); // detect palette for the selected item
            SelectPalette(chosen); // select the detected palette
            RenderImage("", "", chosen);
        }
        private void SelectPalette(string chosen)
        {
            if (listBox2.Items.Contains(chosen))
            {
                listBox2.SelectedItem = chosen; // select the detected palette
            }
            else
            {
                MessageBox.Show("Palette not found: " + chosen);
            }
        }
        // palette changed
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox2.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedPalette) { return; } // do not reselect same file
            lastSelectedPalette = selected; // store last selected file
            // use the selected palette to render the image
            RenderImage("", "", selected + ".PAL");
        }
    }
}
