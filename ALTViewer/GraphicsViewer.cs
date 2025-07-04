using System.Drawing.Imaging;
using static System.Collections.Specialized.BitVector32;

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
        private int lastSelectedSubFrame = -1;
        private string outputPath = "";
        private List<BndSection> currentSections = new();
        private byte[]? currentPalette;
        private byte[]? currentFrame;
        private bool palfile = true; // true if .PAL file is used ( no palette file for level files, enemies and weapons )
        private bool compressed;
        private bool refresh; // set to true when entering the palette editor
        private bool exporting; // set to true when exporting everything
        public static string[] removal = new string[] { "DEMO111", "DEMO211", "DEMO311", "PICKMOD", "OPTOBJ", "OBJ3D" }; // unused demo files and models
        public static string[] duplicate = new string[] { "EXPLGFX", "FLAME", "MM9", "OPTGFX", "PULSE", "SHOTGUN", "SMART" }; // remove duplicate entries & check for weapons
        public static string[] weapons = new string[] { "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART" }; // check for weapons
        public static string[] excluded = { "LEV", "GUNPALS", "SPRITES", "WSELECT", "PANEL", "NEWFONT" }; // excluded palettes
        public int w = 0; // WIDTH
        public int h = 0; // HEIGHT
        private bool trimmed; // trim 96 bytes from the beginning of the palette for some files (e.g. PRISHOLD, COLONY, BONESHIP)
        public GraphicsViewer()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox), typeof(NumericUpDown) });
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*" + ".PAL"); // Load palettes from the palette directory
            foreach (string palFile in palFiles)
            {
                string name = Path.GetFileNameWithoutExtension(palFile);
                if (!excluded.Any(e => name.Contains(e))) // exclude unused palettes
                {
                    listBox2.Items.Add(name);
                }
            }
            ListFiles(gfxDirectory); // Load graphics files by default on startup
        }
        // graphics GFX
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ListBoxes(true);
            ListFiles(gfxDirectory); // .BND and .B16 files exist in the GFX folder which are used
        }
        // enemies NME
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ListBoxes(false);
            ListFiles(enemyDirectory, ".B16", ".NOPE"); // enemies are all compressed .B16 files
        }
        // levels SECT##
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ListBoxes(false);
            foreach (string level in levels) { ListFiles(level); } // levels are all .B16 files
        }
        // panels LANGUAGE
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ListBoxes(true);
            ListFiles(languageDirectory, ".NOPE", ".16"); // .NOPE ignores the unused .BND files in the LANGUAGE folder
        }
        // clear listBox1 then enable or disable the palette list box based on the selected radio button
        private void ListBoxes(bool enabled)
        {
            listBox1.Items.Clear();
            listBox2.Enabled = enabled;
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
                var toRemove = new List<string>(); // Items to remove
                foreach (string item in listBox1.Items) // Count occurrences
                {
                    if (!counts.ContainsKey(item)) { counts[item] = 0; }
                    counts[item]++;
                }
                foreach (var file in removal) { if (listBox1.Items.Contains(file)) { toRemove.Add(file); } } // Add always-remove items
                foreach (var file in duplicate) // Add duplicate-only items
                {
                    if (counts.TryGetValue(file, out int count) && count > 1) { toRemove.Add(file); }
                }
                foreach (var file in toRemove) { listBox1.Items.Remove(file); } // Remove items
            }
        }
        // discover files in directory
        private string[] DiscoverFiles(string path, string type1 = ".BND", string type2 = ".B16")
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(type1) || file.EndsWith(type2)).ToArray();
        }
        // display selected file
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox1.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedFile && !refresh) { return; } // do not reselect same file
            lastSelectedFile = selected; // store last selected file
            // show palette controls
            label1.Visible = true; // show palette selection label
            label2.Visible = true; // show palette note 1
            label3.Visible = true; // show palette note 2
            label4.Visible = true; // show palette note 3
            listBox2.Visible = true; // show palette list
            button7.Visible = true; // show palette editor button
            button5.Enabled = true; // enable replace texture button
            checkBox1.Enabled = true; // enable backup checkbox
            // determine which directory to use based on selected radio button
            if (radioButton1.Checked) { GetFile(gfxDirectory); }
            else if (radioButton2.Checked) { GetFile(enemyDirectory); }
            else if (radioButton3.Checked)
            {
                foreach (string level in levels) // determine level folder based on selected item
                {
                    if (File.Exists(Path.Combine(level, listBox1.SelectedItem!.ToString()! + ".B16")))
                    {
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
            string chosen = Path.GetFileNameWithoutExtension(DetectPalette(selected + ".PAL")); // detect palette for the selected item
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
        private string DetectPalette(string filename)
        {
            string palette = Path.Combine(paletteDirectory, filename);
            if (!File.Exists(palette)) { return ""; }
            else { return palette; }
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
                    compressed = false;
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
                    counter++;
                    decompressedF0Sections.Add(new BndSection { Name = section.Name, Data = decompressedData }); // Store for UI
                }
                currentSections = decompressedF0Sections;
                lastSelectedFilePath = binbnd;
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
                if (binbnd.Contains("PRISHOLD") || binbnd.Contains("COLONY") || binbnd.Contains("BONESHIP")) // these also use embedded palettes
                {
                    byte[] loaded = File.ReadAllBytes(pal);
                    currentPalette = new byte[768];
                    trimmed = true; // set trimmed to true for these files
                    Array.Copy(loaded, 0, currentPalette, 96, 672); // 96 padded bytes at the beginning for these palettes
                }
                else if (binbnd.Contains("LOGOSGFX"))
                {
                    byte[] loaded = File.ReadAllBytes(pal);
                    currentPalette = new byte[768];
                    trimmed = false; // set trimmed to false for these files
                    Array.Copy(loaded, 0, currentPalette, 0, 576);
                }
                else // LEGAL.PAL
                {
                    currentPalette = File.ReadAllBytes(pal);
                }
            }
            else
            {
                if (binbnd.Contains("PANEL")) // TODO : figure out PANEL3GF and PANELGFX palettes and usecase
                {
                    //MessageBox.Show("Viewing these files is not properly implemented yet. ( PANEL3GF & PANELGFX )");
                }
                else
                {
                    trimmed = false; // set trimmed to false for these files
                }
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
        // export selected frame button
        private void button2_Click(object sender, EventArgs e)
        {
            try { MessageBox.Show($"Image saved to:\n{ExportFile(currentSections[comboBox1.SelectedIndex], comboBox1.SelectedItem!.ToString()!)}"); }
            catch (Exception ex) { MessageBox.Show("Error saving image:\n" + ex.Message); }
        }
        // export file
        private string ExportFile(BndSection section, string sectionName)
        {
            string filepath = "";
            byte[] saving = null!;
            if (!compressed)
            {
                filepath = Path.Combine(outputPath, $"{lastSelectedFile}_{sectionName}.png");
                saving = section.Data; // use section data for non-compressed files
            }
            else
            {
                filepath = Path.Combine(outputPath, $"{lastSelectedFile}_{sectionName}_FRAME{comboBox2.SelectedIndex:D2}.png");
                saving = currentFrame!; // use current frame data for compressed files
                (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(lastSelectedFilePath), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            }
            TileRenderer.Save8bppPng(filepath, saving, TileRenderer.ConvertPalette(currentPalette!), w, h);
            return filepath;
        }
        // export everything button click
        private void button1_Click(object sender, EventArgs e)
        {
            int previouslySelected = 0;
            if (listBox1.SelectedIndex != -1) { previouslySelected = listBox1.SelectedIndex; } // store previously selected index
            exporting = true;
            RadioButton[] buttons = { radioButton1, radioButton2, radioButton3, radioButton4 };
            int selectedIndex = Array.FindIndex(buttons, b => b.Checked);
            foreach (var button in buttons)
            {
                button.Checked = true;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SelectedIndex = i; // select each item in the list box
                    button3_Click(null!, null!); // call the export all button click event
                }
            }
            buttons[selectedIndex].Checked = true;
            exporting = false;
            listBox1.SelectedIndex = previouslySelected; // restore previously selected index
            MessageBox.Show($"All images saved to:\n{outputPath}");
        }
        // export all frames button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    comboBox1.SelectedIndex = i; // select each section so that each sub frame is detected, selected and exported
                    if (!compressed && !palfile) // update embedded palette for each frame
                    {
                        currentPalette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(lastSelectedFilePath, $"CL{comboBox1.SelectedIndex:D2}", 12));
                    }
                    else if (compressed)
                    {
                        for (int f = 0; f < comboBox2.Items.Count; f++)
                        {
                            comboBox2.SelectedIndex = f; // select each sub frame
                            ExportFile(null!, comboBox1.Items[i]!.ToString()!);
                        }
                    }
                    if (palfile || !compressed && !palfile) // export embedded palette images and external palette images
                    {
                        ExportFile(currentSections[i], comboBox1.Items[i]!.ToString()!);
                    }
                }
                if (!exporting)
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
            fbd.Description = "Select output folder to save exported files.";
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
            var section = currentSections[comboBox1.SelectedIndex];
            try
            {
                if (!compressed)
                {
                    comboBox2.Visible = false;
                    label5.Visible = false;
                    if (!palfile) // update embedded palette to match selected frame
                    {
                        currentPalette = TileRenderer.Convert16BitPaletteToRGB(
                        TileRenderer.ExtractEmbeddedPalette(lastSelectedFilePath, $"CL{comboBox1.SelectedIndex:D2}", 12));
                    }
                    (w, h) = TileRenderer.AutoDetectDimensions(section.Data); // TODO : remove when compressed file dimensions are detected
                    pictureBox1.Width = w;
                    pictureBox1.Height = h;
                    pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, currentPalette!, w, h);
                }
                else
                {
                    lastSelectedSubFrame = -1; // reset last selected sub frame index
                    comboBox2.Visible = true;
                    label5.Visible = true;
                    DetectFrames.ListFrames(lastSelectedFilePath, comboBox1, comboBox2);
                }
            }
            catch (Exception ex) { MessageBox.Show("Render failed A: " + ex.Message); }
        }
        // sub frame combo box index changed
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == lastSelectedSubFrame) { return; } // still happens twice on keyboard up / down
            lastSelectedSubFrame = comboBox2.SelectedIndex; // store last selected sub frame index
            currentFrame = DetectFrames.RenderSubFrame(lastSelectedFilePath, comboBox1, comboBox2, pictureBox1, currentPalette!); // render the sub frame
            //DetectAfterRender(); // TODO : Keep this for future use
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
            // TODO : update for compressed images and sub frames
            // TODO : backup original file when replacing textures
            int length = filename.Length;
            if (compressed)
            {
                if(length !=1)
                {
                    MessageBox.Show("Please select only one image when replacing a sub frame.");
                    return;
                }
                DetectFrames.ReplaceSubFrame(lastSelectedFilePath, comboBox1, comboBox2, pictureBox1, filename[0]); // replace sub frame
                //MessageBox.Show("Replacing compressed images is not supported yet.");
                //return;
            }
            else
            {
                if (length == 1) { ReplaceFrame(comboBox1.SelectedIndex, "Texture frame replaced successfully.", true); } // replace single frame
                else if (length == currentSections.Count) // replace all frames
                {
                    for (int i = 0; i < length; i++) { ReplaceFrame(i, "All texture frames replaced successfully.", false); }
                    // CONSIDER : building a list of frames to replace : MICRO OPTIMISATION
                }
                else { MessageBox.Show($"Please select exactly {currentSections.Count} images to replace all frames."); return; }
            }
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
                string sectionName = $"TP{frame:D2}";
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
        public static bool IsIndexed8bpp(PixelFormat format) { return format == PixelFormat.Format8bppIndexed; }
        // check the image dimensions match the expected size
        private bool CheckDimensions(Bitmap frameImage)
        {
            if(palfile || !palfile)
            {
                (w, h) = TileRenderer.AutoDetectDimensions(currentSections[comboBox1.SelectedIndex].Data);
            }
            else if(compressed)
            {
                (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(lastSelectedFilePath), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            }
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
        // testing functions for numeric up down controls used to help determine the frame sizes
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { ReRender(); }
        private void ReRender()
        {
            int width = (int)numericUpDown1.Value; // get width from numeric up down control
            (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(lastSelectedFilePath), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            pictureBox1.Image = TileRenderer.RenderRaw8bppImage(currentFrame!, currentPalette!, width, h);
            pictureBox1.Width = width; // set picture box width
        }
        private void DetectAfterRender() // TEST Code commented out
        {
            numericUpDown1.Value = pictureBox1.Width;
            numericUpDown2.Value = pictureBox1.Height;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 300; i++)
            {
                if(pictureBox1.Width * i == currentFrame!.Length)
                {
                    numericUpDown2.Value = i;
                    pictureBox1.Height = i;
                    break;
                }
            }
        }
    }
}