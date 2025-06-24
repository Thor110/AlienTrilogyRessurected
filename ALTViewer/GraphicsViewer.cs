using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

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
        private string lastSelectedFilePath = "";
        private string lastSelectedTilePath = "";
        private string outputPath = "";
        public GraphicsViewer()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox) });
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*" + ".PAL"); // Load palettes from the palette directory
            foreach (string palFile in palFiles) { listBox2.Items.Add(Path.GetFileNameWithoutExtension(palFile)); }
            ListFiles(gfxDirectory); // Load graphics files by default on startup
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
            ListFiles(languageDirectory, ".BND", ".NOPE"); // .NOPE ignores the four .BIN files in the LANGUAGE folder which are not image data
        }
        // list files in directory
        public void ListFiles(string path, string type1 = ".BND", string type2 = ".BIN")
        {
            string[] files = DiscoverFiles(path, type1, type2);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                listBox1.Items.Add(fileName);
            }
        }
        // discover files in directory
        private string[] DiscoverFiles(string path, string type1, string type2)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(type1) || file.EndsWith(type2)).ToArray();
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
                    if (File.Exists(Path.Combine(level, listBox1.SelectedItem!.ToString()! + ".BIN")))
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
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(selected, ".PAL")); // detect palette for the selected item
            string palettePath = Path.Combine(paletteDirectory, chosen + ".PAL"); // actual palette path
            string tilePath = Path.Combine(paletteDirectory, Path.GetFileNameWithoutExtension(DetectPalette(selected, ".TNT")) + ".TNT");
            string filePath = "";
            foreach (string ext in new[] { ".BND", ".BIN", ".B16", ".16" })
            {
                string candidate = Path.Combine(path, selected + ext);
                if (File.Exists(candidate))
                {
                    filePath = candidate;
                    break;
                }
            }
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No usable graphics file found for: " + selected);
                return;
            }
            RenderImage(tilePath, filePath, palettePath, chosen);
        }
        private string DetectPalette(string filename, string extension)
        {
            string palette = Path.Combine(paletteDirectory, filename + extension);
            //
            // discover the palettes for the following files
            //DEMO111   ( Try LEV111.PAL )
            //DEMO211   ( Try LEV211.PAL )
            //DEMO311   ( Try LEV311.PAL )
            //EXPLGFX   ( Try GUNPALS.PAL )
            //OBJ3D     ( UNKNOWN )
            //OPTGFX    ( UNKNOWN )
            //OPTOBJ    ( UNKNOWN )
            //PICKGFX   ( UNKNOWN )
            //PICKMOD   ( UNKNOWN )
            //
            // unused palettes currently
            // MBRF_PAL.PAL ( Possibly LANGUAGE folder panels )
            // WSELECT.PAL  ( UNKNOWN )
            //
            // hard coded palette lookups
            string[] hardcodedPalettes = new string[] { "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART" };
            if (filename == "FONT1GFX") { return Path.Combine(paletteDirectory, "NEWFONT" + ".PAL"); }
            else if (hardcodedPalettes.Contains(filename)) { return Path.Combine(paletteDirectory, "GUNPALS" + ".PAL"); }
            else if (radioButton2.Checked) { return Path.Combine(paletteDirectory, "SPRITES" + ".PAL"); }
            else if (radioButton3.Checked) { return "LEV" + listBox1.SelectedItem!.ToString()!.Substring(0, 3) + ".PAL"; }
            else if (radioButton4.Checked || filename.Contains("PANEL")) { return Path.Combine(paletteDirectory, "PANEL" + ".PAL"); }
            else if (!File.Exists(palette)) { return ""; }
            else { return Path.Combine(paletteDirectory, filename + ".PAL"); }
        }
        // render the selected image
        private void RenderImage(string tnt, string binbnd, string pal, string select)
        {
            pictureBox1.Image = null; // clear previous image
            // event handler removal to prevent rendering the image twice
            listBox2.SelectedIndexChanged -= listBox2_SelectedIndexChanged!;
            // select palette for the chosen file in the palette listbox
            if (listBox2.Items.Contains(select)) { listBox2.SelectedItem = select; } // select the detected palette
            else { MessageBox.Show("Palette not found: " + select); }
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged!;
            lastSelectedTilePath = tnt;
            lastSelectedFilePath = binbnd;
            if (!File.Exists(pal)) { return; } // bin bnd already checked
            // test render
            byte[]? tntBytes = File.Exists(tnt) ? File.ReadAllBytes(tnt) : null;
            // Palettes without TNT files
            // GUNPALS.PAL
            // MBRF_PAL.PAL
            // NEWFONT.PAL
            // PANEL.PAL
            // SPRITES.PAL
            // WSELECT.PAL
            //
            byte[] bndBytes = File.ReadAllBytes(binbnd);
            byte[] palBytes = File.ReadAllBytes(pal);
            pictureBox1.Image = TileRenderer.RenderTiledImage(tntBytes!, bndBytes, palBytes);
        }
        // re-detect image palette and refresh the image
        private void button1_Click(object sender, EventArgs e)
        {
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(listBox1.SelectedItem!.ToString()!, ".PAL")); // detect palette for the selected item
            RenderImage(lastSelectedTilePath, lastSelectedFilePath, Path.Combine(paletteDirectory, chosen + ".PAL"), chosen);
        }
        // palette changed
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox2.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedPalette) { return; } // do not reselect same file
            lastSelectedPalette = selected; // store last selected file
            string palettePath = Path.Combine(paletteDirectory, selected + ".PAL");
            RenderImage(lastSelectedTilePath, lastSelectedFilePath, palettePath, selected); // use the selected palette to render the image
        }
        // export selected button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = lastSelectedFile;
                string filepath = Path.Combine(outputPath, filename + ".png");
                pictureBox1.Image.Save(filepath, ImageFormat.Png);
                MessageBox.Show($"Image saved to:\n{filepath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image:\n" + ex.Message);
            }
        }
        // export all button
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Batch export not supported yet!"); // until the rendering pipeline actually works
        }
        // select output path
        private void button4_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Select output folder to save the WAV file.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputPath = fbd.SelectedPath;
                textBox1.Text = outputPath; // update text box with selected path
                button2.Enabled = true; // enable extract button
                button3.Enabled = true; // enable extract all button
            }
        }
    }
}