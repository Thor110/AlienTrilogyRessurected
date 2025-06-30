using System.Drawing.Imaging;

namespace ALTViewer
{
    public partial class GraphicsViewer : Form
    {
        public static string gameDirectory = "HDD\\TRILOGY\\CD"; // default directories
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
        private bool palfile = true; // true if .PAL file is used ( no palette file for level files, enemies and weapons )
        private bool compressed;
        private bool refresh; // set to true when entering the palette editor
        private bool exporting; // set to true when exporting everything
        public static string[] removal = new string[] { "DEMO111", "DEMO211", "DEMO311", "PICKMOD", "OPTOBJ", "OBJ3D" }; // unused demo files and models
        public static string[] duplicate = new string[] { "EXPLGFX", "FLAME", "MM9", "OPTGFX", "PULSE", "SHOTGUN", "SMART" }; // remove duplicate entries & check for weapons
        public static string[] weapons = new string[] { "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART" }; // check for weapons
        public static string[] excluded = { "LEV", "GUNPALS", "SPRITES", "WSELECT", "PANEL", "NEWFONT" }; // excluded palettes
        private bool trimmed; // trim 96 bytes from the beginning of the palette for some files (e.g. PRISHOLD, COLONY, BONESHIP)
        public GraphicsViewer()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox) });
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*" + ".PAL"); // Load palettes from the palette directory
            foreach (string palFile in palFiles)
            {
                string name = Path.GetFileNameWithoutExtension(palFile);
                if (!excluded.Any(e => name.Contains(e)))
                {
                    listBox2.Items.Add(name);
                }
            }
            ListFiles(gfxDirectory); // Load graphics files by default on startup
        }
        // graphics GFX
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = true;
            ListFiles(gfxDirectory);
        }
        // enemies NME
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = false;
            ListFiles(enemyDirectory, ".B16", ".NOPE"); // enemies are compressed
        }
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = false;
            foreach (string level in levels) { ListFiles(level, ".NOPE"); }
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = true;
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
            if (radioButton1.Checked) // remove known unusable files
            {
                var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (string item in listBox1.Items) // Count occurrences
                {
                    if (!counts.ContainsKey(item)) { counts[item] = 0; }
                    counts[item]++;
                }
                var toRemove = new List<string>(); // Items to remove
                foreach (var file in removal) { if (listBox1.Items.Contains(file)) { toRemove.Add(file); } } // Add always-remove items
                foreach (var file in duplicate) // Add duplicate-only items
                {
                    if (counts.TryGetValue(file, out int count) && count > 1) { toRemove.Add(file); }
                }
                foreach (var file in toRemove) { listBox1.Items.Remove(file); } // Remove items
            }
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
            if (selected == lastSelectedFile && !refresh) { return; } // do not reselect same file
            lastSelectedFile = selected; // store last selected file
            // show palette controls
            label1.Visible = true; // show label
            listBox2.Visible = true; // show palette list
            button7.Visible = true; // show palette editor button
            // determine which directory to use based on selected radio button
            if (radioButton1.Checked) { GetFile(gfxDirectory); }
            else if (radioButton2.Checked) { palfile = false; GetFile(enemyDirectory); }
            else if (radioButton3.Checked)
            {
                foreach (string level in levels) // determine level folder based on selected item
                {
                    if (File.Exists(Path.Combine(level, listBox1.SelectedItem!.ToString()! + ".B16")))
                    {
                        palfile = false;
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
            if (!File.Exists(palette)) { return ""; }
            else { return Path.Combine(paletteDirectory, filename + ".PAL"); }
        }
        // render the selected image
        private void RenderImage(string binbnd, string pal, string select)
        {
            pictureBox1.Image = null; // clear previous image
            if (radioButton1.Checked)
            {
                foreach (string weapon in weapons) // check if the selected file is a weapon
                {
                    if (lastSelectedFile.Contains(weapon))
                    {
                        binbnd = binbnd.Replace(".BND", ".B16");
                        listBox2.Enabled = false;
                        palfile = false; // set palfile to false for weapons
                        compressed = true; // set compressed to true for weapons
                        break;
                    }
                    else
                    {
                        listBox2.Enabled = true;
                        palfile = true; // reset palfile to true for next detection
                        compressed = false; // reset compressed to false for next detection
                    }
                }
                if (binbnd.Contains("EXPLGFX") || binbnd.Contains("OPTGFX")) // these also use embedded palettes
                {
                    binbnd = binbnd.Replace(".BND", ".B16"); // but these are B16 files
                    palfile = false;
                    listBox2.Enabled = false;
                    compressed = false; // reset compressed to false for next detection
                }
            }
            else if (radioButton2.Checked)
            {
                binbnd = binbnd.Replace(".BND", ".B16");
                palfile = false; // set palfile to false for weapons
                compressed = true; // set compressed to true for weapons
            }
            listBox2.SelectedIndexChanged -= listBox2_SelectedIndexChanged!; // event handler removal to prevent rendering the image twice
            if (palfile && listBox2.Items.Contains(select)) // select the detected palette if it exists
            {
                listBox2.SelectedItem = select;
                lastSelectedPalette = select; // store last selected file
            }
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged!; // re add the event handler
            if (radioButton4.Checked || radioButton3.Checked ||
                radioButton1.Checked && lastSelectedFile.Contains("GF") && !lastSelectedFile.Contains("LOGO")) // load embedded palettes
            {
                palfile = false; // palette is embedded
                compressed = false; // reset compressed to false for next detection
                listBox2.Enabled = false;
                currentPalette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(binbnd, "CL00", 12));
            }
            else if (compressed) // load palette from level file or enemies
            {
                refresh = false; // reset refresh to false before any possible returns
                trimmed = false; // set trimmed to false for these files
                binbnd = binbnd.Replace(".BND", ".B16"); // Ensure we are working with the B16 file variant
                byte[] fullFile = File.ReadAllBytes(binbnd);
                currentPalette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(binbnd, $"C000", 8));
                List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
                var f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList(); // Get only F0## sections
                if (f0Sections.Count == 0) { MessageBox.Show("No F0 sections found."); return; }
                List<BndSection> decompressedF0Sections = new();
                int counter = 0;
                foreach (var section in f0Sections)
                {
                    byte[] decompressedData;
                    try
                    {
                        decompressedData = TileRenderer.DecompressSpriteSection(section.Data); // Try decompressing individual F0 section
                        if (decompressedData.Length < 64) { throw new Exception("Data too small, likely not compressed"); } // Heuristic: If result is tiny, probably not valid
                    }
                    catch { decompressedData = section.Data; } // Fallback: Use raw data
                    //File.WriteAllBytes($"sprite_decompressed_F0_{counter:D2}.bin", decompressedData); // Optional: Write decompressed data for inspection
                    counter++;
                    decompressedF0Sections.Add(new BndSection { Name = section.Name, Data = decompressedData }); // Store for UI
                }
                lastSelectedFilePath = binbnd;
                currentSections = decompressedF0Sections;
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
                if (comboBox1.Items.Count > 0) { comboBox1.SelectedIndex = 0; } // trigger rendering
                else { MessageBox.Show("No image sections found in decompressed F0 blocks."); }
                return; // We handled everything; skip rest of RenderImage.
            }
            else if (!File.Exists(pal)) { MessageBox.Show("Palette not found: Error :" + select); return; } // bin bnd already checked
            lastSelectedFilePath = binbnd;
            byte[] bndBytes = File.ReadAllBytes(binbnd);
            if (palfile)  // read .PAL file if not reading from embedded palettes
            {
                byte[] loaded = File.ReadAllBytes(pal);
                currentPalette = new byte[768];
                if (binbnd.Contains("PRISHOLD") || binbnd.Contains("COLONY") || binbnd.Contains("BONESHIP")) // these also use embedded palettes
                {
                    trimmed = true; // set trimmed to true for these files
                    Array.Copy(loaded, 0, currentPalette, 96, Math.Min(loaded.Length, 672)); // 96 padded bytes at the beginning for these palettes
                }
                else
                {
                    trimmed = false; // set trimmed to false for these files
                    currentPalette = File.ReadAllBytes(pal);
                }
            }
            else
            {
                trimmed = false; // set trimmed to false for these files
            }
            currentSections = TileRenderer.ParseBndFormSections(bndBytes); // Parse all sections (TP00, TP01, etc.)
            comboBox1.Enabled = true; // enable section selection combo box
            comboBox1.Items.Clear(); // Populate ComboBox with section names
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            if (comboBox1.Items.Count > 0) { comboBox1.SelectedIndex = 0; } // trigger rendering
            else { MessageBox.Show("No image sections found in BND file."); }
            refresh = false;
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
            try { MessageBox.Show($"Image saved to:\n{ExportFile(currentSections[comboBox1.SelectedIndex], comboBox1.SelectedItem!.ToString()!)}"); }
            catch (Exception ex) { MessageBox.Show("Error saving image:\n" + ex.Message); }
        }
        // export file
        private string ExportFile(BndSection section, string sectionName)
        {
            var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
            string filepath = Path.Combine(outputPath, $"{lastSelectedFile}_{sectionName}.png");
            TileRenderer.Save8bppPng(filepath, section.Data, TileRenderer.ConvertPalette(currentPalette!), w, h);
            return filepath;
        }
        // export everything button click
        private void button1_Click(object sender, EventArgs e)
        {
            int previouslyselected = 0;
            if (listBox1.SelectedIndex != -1) { previouslyselected = listBox1.SelectedIndex; } // store previously selected index
            RadioButton selected = radioButton1.Checked ? radioButton1 :
                       radioButton2.Checked ? radioButton2 :
                       radioButton3.Checked ? radioButton3 :
                       radioButton4;
            //radioButton1.Checked = true;
            //Export();
            //radioButton2.Checked = true;
            //Export();
            exporting = true;
            radioButton3.Checked = true; // set radio button to GFX
            Export();
            radioButton4.Checked = true;
            Export();
            // use this code to export all radio buttons at once, when compressed images are working
            /*RadioButton[] buttons = { radioButton1, radioButton2, radioButton3, radioButton4 };
            foreach (var button in buttons)
            {
                button.Checked = true;
                Export();
            }*/
            //
            exporting = false;
            selected.Checked = true;
            listBox1.SelectedIndex = previouslyselected; // restore previously selected index
            MessageBox.Show($"All images saved to:\n{outputPath}");
            void Export()
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SelectedIndex = i; // select each item in the list box
                    button3_Click(null!, null!); // call the export all button click event
                }
            }
        }
        // export all button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (!compressed && !palfile)
                    {
                        currentPalette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(lastSelectedFilePath, $"CL0{i}", 12));
                    }
                    ExportFile(currentSections[i], comboBox1.Items[i]!.ToString()!);
                }
                if(!exporting)
                {
                    MessageBox.Show($"Images saved to:\n{outputPath}");
                }
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
                button1.Enabled = true; // enable export all button
            }
        }
        // render the image when a section is selected
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO : ExtractEmbeddedPalette runs twice because this is called to render
            var section = currentSections[comboBox1.SelectedIndex];
            try
            {
                if (compressed)
                {
                    //var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                    int w = 32; // BAMBI
                    int h = 77;
                    //int w = 84; // SHOTGUN
                    //int h = 77;
                    if (currentPalette == null) // TODO : remove this testing check when compressed image loading is working
                    {
                        MessageBox.Show("Palette is null");
                        return;
                    }
                    File.WriteAllBytes("debug_dump_palette.bin", currentPalette!);
                    pictureBox1.Image = TileRenderer.BuildIndexedBitmap(section.Data, currentPalette!, w, h);
                }
                else
                {
                    if (!palfile) // update embedded palette to match selected frame
                    {
                        currentPalette = TileRenderer.Convert16BitPaletteToRGB(
                        TileRenderer.ExtractEmbeddedPalette(lastSelectedFilePath, $"CL0{comboBox1.SelectedIndex.ToString()}", 12));
                    }
                    var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                    pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h, transparency); // TODO : remove transparency boolean?
                }
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
                long dataOffset = TileRenderer.FindSectionDataOffset(selectedFile, sectionName, 8); // TODO : update for compressed files
                List<Tuple<long, byte[]>> list = new() { Tuple.Create(dataOffset, indexedData) };
                BinaryUtility.ReplaceBytes(list, selectedFile);
                if (frame + 1 == currentSections.Count || single) // account for zero based indexing
                {
                    MessageBox.Show(message);
                }
            }
            comboBox1_SelectedIndexChanged(null!, null!); // re-render the image
            button6.Enabled = true; // enable restore backup button
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
            refresh = true;
            listBox1_SelectedIndexChanged(null!, null!); // re-render the image after restoring a backup
            MessageBox.Show("Backup successfully restored!");
        }
        // colour correction transparency setting
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            transparency = checkBox2.Checked;
            comboBox1_SelectedIndexChanged(null!, null!); // re-render the image with the new transparency setting
        }
        // palette editor button click
        private void button7_Click(object sender, EventArgs e)
        {
            refresh = true;
            string choice = palfile ? lastSelectedPalette : lastSelectedFilePath; // use the last selected file path for embedded palettes or use the last selected palette
            newForm(new PaletteEditor(choice, palfile, currentSections, compressed, trimmed));
        }
        // create new form method
        private void newForm(Form form)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = this.Location;
            form.Show();
            this.Hide();
            form.FormClosed += (s, args) =>
            {
                this.Show();
                listBox1_SelectedIndexChanged(null!, null!); // Re-run selected palette loading logic and re-render image
            };
            form.Move += (s, args) => { if (this.Location != form.Location) { this.Location = form.Location; } };
        }
    }
}