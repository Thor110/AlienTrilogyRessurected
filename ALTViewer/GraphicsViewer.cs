using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms.VisualStyles;
using static System.Collections.Specialized.BitVector32;

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
        private string outputPath = "";
        private List<BndSection> currentSections = new();
        private byte[]? currentPalette;
        public static string[] removal = new string[] { "DEMO111", "DEMO211", "DEMO311", "PICKMOD", "OPTOBJ", "OBJ3D" }; // demo files and models
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
            comboBox1.Enabled = false;
            ListFiles(gfxDirectory);
        }
        // enemies NME
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            comboBox1.Enabled = false;
            ListFiles(enemyDirectory);
        }
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            comboBox1.Enabled = false;
            foreach (string level in levels) { ListFiles(level); } // BIN and B16 files look identical?
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            comboBox1.Enabled = false;
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
            if (radioButton1.Checked) { foreach (string file in removal) { if (listBox1.Items.Contains(file)) { listBox1.Items.Remove(file); } } } // remove known unusable files
            else if (radioButton2.Checked) { listBox1.Items.Remove("SPRCLUT"); }
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
            string filePath = "";
            foreach (string ext in new[] { ".BND", ".BIN", ".B16", ".16" })
            {
                string candidate = Path.Combine(path, selected + ext);
                if (File.Exists(candidate)) { filePath = candidate; break; }
            }
            if(File.Exists(filePath + ".BAK")) { button6.Enabled = true; }
            else { button6.Enabled = false; }
            if (string.IsNullOrEmpty(filePath)) { MessageBox.Show("No usable graphics file found for: " + selected); return; }
            RenderImage(filePath, palettePath, chosen);
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
            if (filename == "EXPLGFX" || filename == "PICKGFX") { return Path.Combine(paletteDirectory, "WSELECT" + ".PAL"); }
            else if (filename == "FONT1GFX") { return Path.Combine(paletteDirectory, "NEWFONT" + ".PAL"); }
            else if (filename == "OPTGFX") { return Path.Combine(paletteDirectory, "BONESHIP" + ".PAL"); }
            else if (hardcodedPalettes.Contains(filename)) { return Path.Combine(paletteDirectory, "GUNPALS" + ".PAL"); }
            else if (radioButton2.Checked) { return Path.Combine(paletteDirectory, "SPRITES" + ".PAL"); }
            else if (radioButton3.Checked) { return "LEV" + listBox1.SelectedItem!.ToString()!.Substring(0, 3) + ".PAL"; }
            else if (radioButton4.Checked || filename.Contains("PANEL")) { return Path.Combine(paletteDirectory, "PANEL" + ".PAL"); }
            else if (!File.Exists(palette)) { return ""; }
            else { return Path.Combine(paletteDirectory, filename + ".PAL"); }
        }
        // render the selected image
        private void RenderImage(string binbnd, string pal, string select)
        {
            pictureBox1.Image = null; // clear previous image
            // event handler removal to prevent rendering the image twice
            listBox2.SelectedIndexChanged -= listBox2_SelectedIndexChanged!;
            // select palette for the chosen file in the palette listbox
            if (listBox2.Items.Contains(select)) { listBox2.SelectedItem = select; } // select the detected palette
            else { MessageBox.Show("Palette not found: " + select); }
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged!;
            //lastSelectedTilePath = tnt;
            lastSelectedFilePath = binbnd;
            if (!File.Exists(pal)) { return; } // bin bnd already checked
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
            // Store palette for reuse on selection change
            currentPalette = palBytes;
            // Parse all sections (TP00, TP01, etc.)
            currentSections = TileRenderer.ParseBndFormSections(bndBytes);
            comboBox1.Enabled = true; // enable section selection combo box
            // Populate ComboBox with section names
            comboBox1.Items.Clear();
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            if (comboBox1.Items.Count > 0) { comboBox1.SelectedIndex = 0; } // trigger rendering
            else { MessageBox.Show("No image sections found in BND file."); }
        }
        // re-detect image palette and refresh the image
        private void button1_Click(object sender, EventArgs e)
        {
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(listBox1.SelectedItem!.ToString()!, ".PAL")); // detect palette for the selected item
            RenderImage(lastSelectedFilePath, Path.Combine(paletteDirectory, chosen + ".PAL"), chosen);
        }
        // palette changed
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox2.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedPalette) { return; } // do not reselect same file
            lastSelectedPalette = selected; // store last selected file
            string palettePath = Path.Combine(paletteDirectory, selected + ".PAL");
            RenderImage(lastSelectedFilePath, palettePath, selected); // use the selected palette to render the image
        }
        // export selected button
        private void button2_Click(object sender, EventArgs e)
        {
            var section = currentSections[comboBox1.SelectedIndex];
            try { MessageBox.Show($"Image saved to:\n{ExportFile(section, comboBox1.SelectedItem!.ToString()!)}"); }
            catch (Exception ex) { MessageBox.Show("Error saving image:\n" + ex.Message); }
        }
        // export file
        private string ExportFile(BndSection section, string sectionName)
        {
            var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
            string filepath = Path.Combine(outputPath, $"{lastSelectedFile}_{sectionName}.png");
            Bitmap image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h);
            image.Save(filepath, ImageFormat.Png);
            return filepath;
        }
        // export all button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    var section = currentSections[i];
                    ExportFile(section, comboBox1.Items[i]!.ToString()!);
                }
                MessageBox.Show($"Images saved to:\n{outputPath}");
            }
            catch (Exception ex) { MessageBox.Show("Error saving images:\n" + ex.Message); }
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var section = currentSections[comboBox1.SelectedIndex];
            try
            {
                var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h);
                //MessageBox.Show($"Height : {w} Height : {h}");
            }
            catch (Exception ex) { MessageBox.Show("Render failed: " + ex.Message); }
        }
        // replace button click event
        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select an image (.png) file";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) { ReplaceTexture(openFileDialog.FileNames); }
            }
        }
        // replace texture
        private void ReplaceTexture(string[] filename)
        {
            if (TryGetTargetPath(out string selectedFile, out string backupFile) && !File.Exists(backupFile) && checkBox1.Checked) { File.Copy(selectedFile, backupFile); }
            int length = filename.Length;
            if (length == 1) { ReplaceFrame(comboBox1.SelectedIndex); MessageBox.Show("Texture frame replaced successfully."); } // replace single frame
            else if (length == currentSections.Count) // replace all frames
            {
                for (int i = 0; i < length; i++) { ReplaceFrame(i); } // CONSIDER : building a list of frames to replace : MICRO OPTIMISATION
                MessageBox.Show("All texture frames replaced successfully.");
            }
            else { MessageBox.Show($"Please select exactly {currentSections.Count} images to replace all frames."); return; }
            void ReplaceFrame(int frame)
            {
                Bitmap frameImage = new Bitmap(filename[frame]);
                if (!IsIndexed8bpp(frameImage.PixelFormat)) { MessageBox.Show("Image must be 8bpp indexed PNG."); return; }
                if (!CheckDimensions(frameImage)) { return; }
                byte[] indexedData = TileRenderer.Extract8bppData(frameImage);
                currentSections[frame].Data = indexedData;
                var section = currentSections[frame];
                string sectionName = $"TP0{frame}";
                long dataOffset = FindSectionDataOffset(selectedFile, sectionName);
                List<Tuple<long, byte[]>> list = new() { Tuple.Create(dataOffset, indexedData) };
                BinaryUtility.ReplaceBytes(list, selectedFile);
            }
        }
        public static long FindSectionDataOffset(string filePath, string sectionName)
        {
            byte[] label = Encoding.ASCII.GetBytes(sectionName);
            byte[] fileBytes = File.ReadAllBytes(filePath);

            for (int i = 0; i < fileBytes.Length - label.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < label.Length; j++)
                {
                    if (fileBytes[i + j] != label[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i + 8; // label (4) + size (4) = data starts here
                }
            }
            throw new Exception("Section not found in file.");
        }
        private bool IsIndexed8bpp(PixelFormat format) { return format == PixelFormat.Format8bppIndexed; }
        private bool CheckDimensions (Bitmap frameImage)
        {
            var section = currentSections[comboBox1.SelectedIndex];
            var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
            if(frameImage.Width == w && frameImage.Height == h) { return true; }
            MessageBox.Show($"Image dimensions do not match the expected size of {w}x{h} pixels.");
            return false;
        }
        private bool TryGetTargetPath(out string fullPath, out string backupPath)
        {
            fullPath = "";
            backupPath = "";
            string directory = listBox1.SelectedItem!.ToString()!.Substring(0, 2);
            string filetype = "";
            if (radioButton1.Checked) { directory = "GFX"; filetype = "BND"; }
            else if (radioButton2.Checked) { directory = "NME"; filetype = "B16"; }
            else if (radioButton3.Checked)
            {
                switch (directory)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                        directory = "SECT11"; break;
                    case "21":
                    case "22":
                    case "23":
                        directory = "SECT21"; break;
                    case "24":
                    case "26":
                        directory = "SECT22"; break;
                    case "31":
                    case "32":
                    case "33":
                        directory = "SECT31"; break;
                    case "35":
                    case "36":
                    case "37":
                    case "38":
                    case "39":
                        directory = "SECT32"; break;
                    case "90":
                        directory = "SECT90"; break;
                    default:
                        MessageBox.Show("Unknown section selected!");
                        return false;
                }
                filetype = "B16";
            }
            else if (radioButton4.Checked) { directory = "LANGUAGE"; filetype = "16"; }
            fullPath = $"HDD\\TRILOGY\\CD\\{directory}\\{listBox1.SelectedItem}.{filetype}";
            backupPath = fullPath + ".BAK";
            return true;
        }
        // restore backup click event
        private void button6_Click(object sender, EventArgs e)
        {
            if (!TryGetTargetPath(out string selectedFile, out string backupFile)) { return; }
            File.Copy($"{backupFile}", selectedFile, true);
            File.Delete($"{backupFile}");
            button6.Enabled = false;
            // refresh the image after restoring
            MessageBox.Show("Backup successfully restored!");
        }
    }
}