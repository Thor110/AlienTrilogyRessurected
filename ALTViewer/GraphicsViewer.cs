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
        private bool transparency;
        private bool palfile; // true if no .PAL file is used ( level files )
        public static string[] removal = new string[] { "DEMO111", "DEMO211", "DEMO311", "PICKMOD", "OPTOBJ", "OBJ3D", // demo files and models
        "EXPLGFX", "FLAME", "MM9", "OPTGFX", "PULSE", "SHOTGUN", "SMART"}; // remove duplicate entries
        // TODO : only add .B16 entries for these files, rather than removing them as if the unused files are removed, that will result in no entries for these files
        public GraphicsViewer()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox) });
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*" + ".PAL"); // Load palettes from the palette directory
            foreach (string palFile in palFiles) { if(!palFile.Contains("LEV")) { listBox2.Items.Add(Path.GetFileNameWithoutExtension(palFile)); } } // exclude level palettes
            ListFiles(gfxDirectory); // Load graphics files by default on startup
        }
        // graphics GFX
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = true;
            comboBox1.Enabled = false;
            ListFiles(gfxDirectory); // TODO : exclude .BND files that are not used in the game such as the weapons etc
        }
        // enemies NME
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = true;
            comboBox1.Enabled = false;
            ListFiles(enemyDirectory, ".B16", ".NOPE");
        }
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = false;
            comboBox1.Enabled = false;
            foreach (string level in levels) { ListFiles(level, ".NOPE"); }
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = true;
            comboBox1.Enabled = false;
            ListFiles(languageDirectory, ".NOPE", ".NOPE"); // .NOPE ignores the four .BIN files in the LANGUAGE folder which are not image data
        }
        // list files in directory
        public void ListFiles(string path, string type1 = ".BND", string type2 = ".B16")
        {
            string[] files = DiscoverFiles(path, type1, type2);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                listBox1.Items.Add(fileName);
            }
            if (radioButton1.Checked)
            {
                foreach (string file in removal) // TODO : needs updating to exclude .BND weapon entries IE : to prevent duplicates for users who do not purge unused files
                {
                    if (listBox1.Items.Contains(file))
                    {
                        listBox1.Items.Remove(file);
                    }
                }
            } // remove known unusable files
            else if (radioButton2.Checked) { listBox1.Items.Remove("SPRCLUT"); }
        }
        // discover files in directory
        private string[] DiscoverFiles(string path, string type1 = ".BND", string type2 = ".B16", string type3 = ".16")
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(type1) || file.EndsWith(type2) || file.EndsWith(type3)).ToArray();
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
                        palfile = true;
                        GetFile(level);
                        return; // exit after finding the first matching level
                    }
                }
            }
            else if (radioButton4.Checked) { GetFile(languageDirectory); }
        }
        // get the file from the selected directory then render it
        private void GetFile(string path)
        {
            string selected = listBox1.SelectedItem!.ToString()!; // get selected item
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(selected, ".PAL")); // detect palette for the selected item
            string palettePath = Path.Combine(paletteDirectory, chosen + ".PAL"); // actual palette path
            string filePath = "";
            foreach (string ext in new[] { ".BND", ".B16", ".16" })
            {
                string candidate = Path.Combine(path, selected + ext);
                if (File.Exists(candidate)) { filePath = candidate; break; }
            }
            if (File.Exists(filePath + ".BAK")) { button6.Enabled = true; }
            else { button6.Enabled = false; }
            if (string.IsNullOrEmpty(filePath)) { MessageBox.Show("No usable graphics file found for: " + selected); return; }
            RenderImage(filePath, palettePath, chosen);
        }
        // detect palette and hard coded palette lookups
        private string DetectPalette(string filename, string extension)
        {
            string palette = Path.Combine(paletteDirectory, filename + extension);
            string[] hardcodedPalettes = new string[] { "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART" };
            if (filename == "EXPLGFX" || filename == "PICKGFX") { return Path.Combine(paletteDirectory, "WSELECT" + ".PAL"); }
            else if (filename == "FONT1GFX") { return Path.Combine(paletteDirectory, "NEWFONT" + ".PAL"); }
            else if (filename == "OPTGFX") { return Path.Combine(paletteDirectory, "BONESHIP" + ".PAL"); }
            else if (hardcodedPalettes.Contains(filename)) { return Path.Combine(paletteDirectory, "GUNPALS" + ".PAL"); }
            else if (radioButton2.Checked) { return Path.Combine(paletteDirectory, "SPRITES" + ".PAL"); }
            else if (radioButton4.Checked || filename.Contains("PANEL")) { return Path.Combine(paletteDirectory, "PANEL" + ".PAL"); }
            else if (!File.Exists(palette)) { return ""; }
            else { return Path.Combine(paletteDirectory, filename + ".PAL"); }
        }
        // render the selected image
        private void RenderImage(string binbnd, string pal, string select)
        {
            pictureBox1.Image = null; // clear previous image
            byte[] levelPalette = null!;
            listBox2.SelectedIndexChanged -= listBox2_SelectedIndexChanged!; // event handler removal to prevent rendering the image twice
            if (listBox2.Items.Contains(select)) { listBox2.SelectedItem = select; } // select the detected palette if it exists
            else if (palfile) // load palette from levelfile
            {
                levelPalette = TileRenderer.Convert16BitPaletteToRGB(
                    ExtractLevelPalette(binbnd, $"CL0{(comboBox1.SelectedIndex == -1 ? "0" : comboBox1.SelectedIndex.ToString())}"));
            }
            else if (!File.Exists(pal)) { MessageBox.Show("Palette not found: " + select); return; } // bin bnd already checked
            else { MessageBox.Show("Palette not found: " + select); } // TODO : might not need this else
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged!;
            lastSelectedFilePath = binbnd;
            byte[] bndBytes = File.ReadAllBytes(binbnd); // TODO : replace binbnd with lastSelectedFile
            if (!palfile) { levelPalette = File.ReadAllBytes(pal); } // read .PAL file if not reading from .B16 palettes
            if (levelPalette != null) { currentPalette = levelPalette; } // Store palette for reuse on selection change
            currentSections = TileRenderer.ParseBndFormSections(bndBytes); // Parse all sections (TP00, TP01, etc.)
            palfile = false; // reset palfile to false for next file
            comboBox1.Enabled = true; // enable section selection combo box
            comboBox1.Items.Clear(); // Populate ComboBox with section names
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            if (comboBox1.Items.Count > 0) { comboBox1.SelectedIndex = 0; } // trigger rendering
            else { MessageBox.Show("No image sections found in BND file."); }
        }
        // re-detect image palette and refresh the image
        private void button1_Click(object sender, EventArgs e)
        {
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(listBox1.SelectedItem!.ToString()!, ".PAL")); // detect palette for the selected item
            RenderImage(lastSelectedFilePath, Path.Combine(paletteDirectory, chosen + ".PAL"), chosen); // TODO : fix palette detection in these odd places
        }
        // palette changed
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox2.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedPalette) { return; } // do not reselect same file
            lastSelectedPalette = selected; // store last selected file
            string palettePath = Path.Combine(paletteDirectory, selected + ".PAL"); // TODO : fix palette detection in these odd places
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
            Bitmap image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h, transparency);
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
        // render the image when a section is selected
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var section = currentSections[comboBox1.SelectedIndex];
            try
            {
                var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h, transparency);
                //MessageBox.Show($"Height : {w} Height : {h}"); // TODO : remove once dimensions are confirmed for weapon files
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
            int length = filename.Length;
            if (length == 1) { ReplaceFrame(comboBox1.SelectedIndex, "Texture frame replaced successfully.", true); } // replace single frame
            else if (length == currentSections.Count) // replace all frames
            {
                for (int i = 0; i < length; i++) { ReplaceFrame(i, "All texture frames replaced successfully.", false); }
                // CONSIDER : building a list of frames to replace : MICRO OPTIMISATION
            }
            else { MessageBox.Show($"Please select exactly {currentSections.Count} images to replace all frames."); return; }
            void ReplaceFrame(int frame, string message, bool single)
            {
                int framestore = frame; // frame is the frame to be replaced
                if (single) { framestore = 0; } // get only one frame if single is true
                Bitmap frameImage;
                try { frameImage = new Bitmap(filename[framestore]); } // safety first...
                catch (Exception ex) { MessageBox.Show("Failed to load image:\n" + ex.Message); return; }
                if (!IsIndexed8bpp(frameImage.PixelFormat)) { MessageBox.Show("Image must be 8bpp indexed PNG."); return; }
                else if (!CheckDimensions(frameImage)) { return; }
                if (TryGetTargetPath(out string selectedFile, out string backupFile) && !File.Exists(backupFile) && checkBox1.Checked) { File.Copy(selectedFile, backupFile); }
                byte[] indexedData = TileRenderer.Extract8bppData(frameImage);
                currentSections[frame].Data = indexedData;
                var section = currentSections[frame];
                string sectionName = $"TP0{frame}";
                long dataOffset = FindSectionDataOffset(selectedFile, sectionName, 8);
                List<Tuple<long, byte[]>> list = new() { Tuple.Create(dataOffset, indexedData) };
                BinaryUtility.ReplaceBytes(list, selectedFile);
                if (frame + 1 == currentSections.Count || single) // account for zero based indexing
                {
                    MessageBox.Show(message);
                    comboBox1_SelectedIndexChanged(null!, null!); // re-render the image
                }
            }
        }
        // find the offset of the section data in the file
        public static long FindSectionDataOffset(string filePath, string sectionName, int length)
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
                if (match) { return i + length; } // label (4) + size (4) = data starts here
            }
            throw new Exception("Section not found in file.");
        }
        // check if the image is indexed 8bpp
        private bool IsIndexed8bpp(PixelFormat format) { return format == PixelFormat.Format8bppIndexed; }
        // check the image dimensions match the expected size
        private bool CheckDimensions(Bitmap frameImage)
        {
            var section = currentSections[comboBox1.SelectedIndex];
            var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
            if (frameImage.Width == w && frameImage.Height == h) { return true; }
            MessageBox.Show($"Image dimensions do not match the expected size of {w}x{h} pixels.");
            return false;
        }
        // get the target path for the selected file
        private bool TryGetTargetPath(out string fullPath, out string backupPath)
        {
            fullPath = "";
            backupPath = "";
            string directory = "";
            string filetype = "";
            if (radioButton1.Checked)
            {
                directory = "GFX";
                filetype = lastSelectedFile switch { "EXPLGFX" or "FLAME" or "MM9" or "OPTGFX" or "PULSE" or "SHOTGUN" or "SMART" => ".B16", _ => ".BND" };
            }
            else if (radioButton2.Checked) { directory = "NME"; filetype = ".B16"; }
            else if (radioButton3.Checked)
            {
                directory = lastSelectedFile.Substring(0, 2) switch
                {
                    "11" or "12" or "13" => "SECT11",
                    "14" or "15" or "16" => "SECT12",
                    "21" or "22" or "23" => "SECT21",
                    "24" or "26" => "SECT22",
                    "31" or "32" or "33" => "SECT31",
                    "35" or "36" or "37" or "38" or "39" => "SECT32",
                    "90" => "SECT90",
                    _ => throw new Exception("Unknown section selected!")
                }; // TODO : change switch expression to search every directory for lastSelectedFile instead when level editing is implemented
                filetype = ".B16";
            }
            else if (radioButton4.Checked) { directory = "LANGUAGE"; filetype = ".16"; }
            fullPath = $"{gameDirectory}\\{directory}\\{listBox1.SelectedItem}{filetype}";
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
            comboBox1_SelectedIndexChanged(sender, e); // refresh the image after restoring
            MessageBox.Show("Backup successfully restored!");
        }
        // colour correction transparency setting
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            transparency = checkBox2.Checked;
            comboBox1_SelectedIndexChanged(sender, e); // re-render the image with the new transparency setting
        }
        // replace palette button click
        private void button7_Click(object sender, EventArgs e)
        {
            // replace the palette byte when it is known // TODO : implement palette editing
            //BinaryUtility.ReplaceByte(0x1A, 0x00, lastSelectedFilePath);
        }
        public static byte[] ExtractLevelPalette(string filePath, string clSectionName)
        {
            
            byte[] fileBytes = File.ReadAllBytes(filePath); // Read entire file once
            long paletteStart = FindSectionDataOffset(filePath, clSectionName, 8); // CL section starts 8 bytes after the header
            if (paletteStart + 512 > fileBytes.Length) { throw new Exception("Palette data exceeds file bounds."); }
            return fileBytes.Skip((int)paletteStart).Take(512).ToArray();
        }
    }
}