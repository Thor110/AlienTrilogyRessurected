using System.Drawing.Imaging;

namespace ALTViewer
{
    public partial class GraphicsViewer : Form
    {
        private string gameDirectory = ""; // default directories
        private string gfxDirectory = "";
        private string paletteDirectory = "";
        private string enemyDirectory = "";
        private string languageDirectory = "";
        private string levelPath1 = "";
        private string levelPath2 = "";
        private string levelPath3 = "";
        private string levelPath4 = "";
        private string levelPath5 = "";
        private string levelPath6 = "";
        private string levelPath7 = "";
        private string[] levels = null!;
        private string lastSelectedFile = "";
        private string lastSelectedPalette = "";
        private string lastSelectedFilePath = "";
        private int lastSelectedFrame = -1; // for reselecting the section after export
        private int lastSelectedSub = -1; // for reselecting the subframe after export
        private int lastSelectedSubFrame = -1; // to prevent selecting the same subframe and rendering twice
        private int lastSelectedSection = -1; // to prevent selecting the same section and rendering twice
        private string outputPath = "";
        private List<BndSection> currentSections = null!;
        private byte[]? currentPalette;
        private byte[]? currentFrame;
        private bool palfile = true; // true if .PAL file is used ( no palette file for level files, enemies and weapons )
        private bool compressed;
        private bool refresh; // set to true when entering the palette editor
        private bool exporting; // set to true when exporting everything
        private bool saved; // set to true when export is successful
        private string exception = "";
        private static string[] removal = new string[] { "DEMO111", "DEMO211", "DEMO311", "PICKMOD", "OPTOBJ", "OBJ3D" }; // unused demo files and models
        private static string[] duplicate = new string[] { "EXPLGFX", "FLAME", "MM9", "OPTGFX", "PULSE", "SHOTGUN", "SMART" }; // remove duplicate entries & check for weapons
        private static string[] weapons = new string[] { "FLAME", "MM9", "PULSE", "SHOTGUN", "SMART" }; // check for weapons
        private static string[] excluded = { "LEV", "GUNPALS", "SPRITES", "WSELECT", "PANEL", "NEWFONT", "MBRF_PAL" }; // excluded palettes
        private int w = 0; // WIDTH
        private int h = 0; // HEIGHT
        private bool trimmed; // trim 96 bytes from the beginning of the palette for some files (e.g. PRISHOLD, COLONY, BONESHIP)
        public GraphicsViewer()
        {
            InitializeComponent();
            SetupDirectories();
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox), typeof(NumericUpDown) });
            string[] palFiles = Directory.GetFiles(paletteDirectory, "*" + ".PAL"); // Load palettes from the palette directory
            foreach (string palFile in palFiles)
            {
                string name = Path.GetFileNameWithoutExtension(palFile);
                if (!excluded.Any(e => name.Contains(e))) { listBox2.Items.Add(name); } // exclude unused palettes
            }
            ListFiles(gfxDirectory); // Load graphics files by default on startup
            listBox1.SelectedIndex = 0; // Select the first item in the list box
            comboBox1.Enabled = true; // enable section selection combo box
        }
        public void SetupDirectories()
        {
            gameDirectory = Utilities.CheckDirectory();     // File types used
            gfxDirectory = gameDirectory + "GFX";           // .BND + .B16
            paletteDirectory = gameDirectory + "PALS";      // .PAL
            enemyDirectory = gameDirectory + "NME";         // .B16
            languageDirectory = gameDirectory + "LANGUAGE"; // .16
            levelPath1 = gameDirectory + "SECT11";          // .B16
            levelPath2 = gameDirectory + "SECT12";          // .B16
            levelPath3 = gameDirectory + "SECT21";          // .B16
            levelPath4 = gameDirectory + "SECT22";          // .B16
            levelPath5 = gameDirectory + "SECT31";          // .B16
            levelPath6 = gameDirectory + "SECT32";          // .B16
            levelPath7 = gameDirectory + "SECT90";          // .B16
            levels = new string[] { levelPath1, levelPath2, levelPath3, levelPath4, levelPath5, levelPath6, levelPath7 };
        }
        // graphics GFX // .BND and .B16 files exist in the GFX folder which are used
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { listBox1.Items.Clear(); ListFiles(gfxDirectory, ".BND", ".B16", true); }
        // enemies NME // enemies are all compressed .B16 files
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { listBox1.Items.Clear(); ListFiles(enemyDirectory, ".B16", ".NOPE"); }
        // levels SECT## // level graphics are all .B16 files
        private void radioButton3_CheckedChanged(object sender, EventArgs e) { listBox1.Items.Clear(); foreach (string level in levels) { ListFiles(level); } }
        // panels LANGUAGE // .NOPE ignores the unused .BND files in the LANGUAGE folder
        private void radioButton4_CheckedChanged(object sender, EventArgs e) { listBox1.Items.Clear(); ListFiles(languageDirectory, ".NOPE", ".16"); }
        // list files in directory
        public void ListFiles(string path, string type1 = ".BND", string type2 = ".B16", bool enabled = false)
        {
            listBox2.Enabled = enabled; // enable or disable the palette list box based on the selected radio button
            string[] files = DiscoverFiles(path, type1, type2);
            foreach (string file in files) { listBox1.Items.Add(Path.GetFileNameWithoutExtension(file)); }
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
            if(outputPath != "") // so I don't have to check if an item is selected or not
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            checkBox1.Enabled = true; // enable backup checkbox
            // determine which directory to use based on selected radio button
            if (radioButton1.Checked) { GetFile(gfxDirectory); }
            else if (radioButton2.Checked) { GetFile(enemyDirectory); }
            else if (radioButton3.Checked)
            {
                foreach (string level in levels) // determine level folder based on selected item
                {
                    if (File.Exists(level + "\\" + selected + ".B16"))
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
            // TODO : chosen and palettePath might be able to be one string
            string palettePath = DetectPalette(selected); // actual palette path
            // TODO : RenderImage might not need chosen / select string
            string filePath = "";
            foreach (string ext in new[] { ".BND", ".B16", ".16" })
            {
                string candidate = path + "\\" + selected + ext;
                if (File.Exists(candidate)) { filePath = candidate; break; }
            }
            if (File.Exists(filePath + ".BAK")) { button6.Enabled = true; } // check if a backup exists
            else { button6.Enabled = false; }
            if (string.IsNullOrEmpty(filePath)) { MessageBox.Show("No usable graphics file found for: " + selected); return; }
            RenderImage(filePath, palettePath);
        }
        // detect palette and hard coded palette lookups
        private string DetectPalette(string filename)
        {
            string palette = paletteDirectory + "\\" + filename + ".PAL";
            if (!File.Exists(palette)) { return ""; }
            else { return palette; }
        }
        private string UpdateExtension(string filename)
        {
            filename = filename.Replace(".BND", ".B16");
            palfile = false;
            return filename;
        }
        // render the selected image
        private void RenderImage(string binbnd, string palettePath)
        {
            pictureBox1.Image = null; // clear previous image
            lastSelectedSection = -1; // reset last selected section variable
            palfile = true; // this is not always true so it gets reset if it is not a file using a palette
            compressed = false; // reset compressed to false for next detection
            if (radioButton1.Checked)
            {
                if (weapons.Any(e => lastSelectedFile.Contains(e))) // exclude unused palettes
                {
                    binbnd = UpdateExtension(binbnd);
                    listBox2.Enabled = false;
                    compressed = true; // set compressed to true for weapons
                }
                if (binbnd.Contains("EXPLGFX") || binbnd.Contains("OPTGFX")) // these also use embedded palettes
                {
                    binbnd = UpdateExtension(binbnd);
                    listBox2.Enabled = false;
                }
                else if (palfile && !compressed) // select the detected palette if it exists
                {
                    listBox2.Enabled = true;
                    listBox2.SelectedIndexChanged -= listBox2_SelectedIndexChanged!; // event handler removal to prevent rendering the image twice
                    listBox2.SelectedItem = Path.GetFileNameWithoutExtension(palettePath);
                    listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged!; // re-add the event handler
                    lastSelectedPalette = palettePath; // store last selected file
                    if (palettePath.Contains("LOGOSGFX"))
                    {
                        byte[] loaded = File.ReadAllBytes(palettePath);
                        currentPalette = new byte[768];
                        trimmed = false; // set trimmed to false for these files
                        Array.Copy(loaded, 0, currentPalette, 0, 576);
                    }
                    else if (binbnd.Contains("PRISHOLD") || binbnd.Contains("COLONY") || binbnd.Contains("BONESHIP")) // these also use embedded palettes
                    {
                        byte[] loaded = File.ReadAllBytes(palettePath);
                        currentPalette = new byte[768];
                        trimmed = true; // set trimmed to true for these files
                        Array.Copy(loaded, 0, currentPalette, 96, 672); // 96 padded bytes at the beginning for these palettes
                    }
                    else if (palettePath.Contains("LEGAL"))
                    {
                        trimmed = false; // set trimmed to false for these files
                        currentPalette = File.ReadAllBytes(palettePath);
                    }
                    else
                    {
                        listBox2.Enabled = false;
                        palfile = false; // reset palfile if not a file that uses external palettes
                    }
                }
                else
                {
                    if (binbnd.Contains("PANEL")) // TODO : figure out PANEL3GF and PANELGFX palettes and usecase
                    {
                        //MessageBox.Show("Viewing these files is not properly implemented yet. ( PANEL3GF & PANELGFX )"); // message shown in palette editor
                    }
                    else // PANEL has a trimmed and or padded embedded palette
                    {
                        trimmed = false; // set trimmed to false for these files
                    }
                }
            }
            else if (radioButton2.Checked)
            {
                binbnd = UpdateExtension(binbnd);
                compressed = true; // set compressed to true for weapons
            }
            if (radioButton4.Checked || radioButton3.Checked ||
                radioButton1.Checked && lastSelectedFile.Contains("GF") && !lastSelectedFile.Contains("LOGO")) // embedded palettes [TODO : Is this necessary?]
            {
                palfile = false; // palette is embedded
                compressed = false; // reset compressed to false for next detection
            }
            lastSelectedFilePath = binbnd;
            byte[] bndBytes = File.ReadAllBytes(binbnd);
            if (compressed) // load palette from level file or enemies
            {
                trimmed = false; // set trimmed to false for these files
                binbnd = UpdateExtension(binbnd);
                currentPalette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(binbnd, $"C000", 8));
                List<BndSection> allSections = TileRenderer.ParseBndFormSections(bndBytes);
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
            }
            else { currentSections = TileRenderer.ParseBndFormSections(bndBytes); }// Parse all sections (TP00, TP01, etc.)
            comboBox1.Items.Clear(); // Populate ComboBox with section names
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            comboBox1.SelectedIndex = 0; // trigger rendering
            refresh = false; // reset refresh to false before any possible returns
        }
        // palette changed
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox2.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedPalette) { return; } // do not reselect same file
            lastSelectedPalette = selected; // store last selected file
            string palettePath = paletteDirectory + "\\" + selected + ".PAL";
            RenderImage(lastSelectedFilePath, palettePath); // use the selected palette to render the image
        }
        // export selected frame button
        private void button2_Click(object sender, EventArgs e)
        {
            ShowMessage($"Image saved to:\n{ExportFile(currentSections[comboBox1.SelectedIndex], comboBox1.SelectedItem!.ToString()!)}");
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
            try
            {
                TileRenderer.Save8bppPng(filepath, saving, TileRenderer.ConvertPalette(currentPalette!), w, h);
                saved = true;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                saved = false;
            }
            return filepath;
        }
        // export everything button click
        private void button1_Click(object sender, EventArgs e)
        {
            exporting = true;
            RadioButton[] buttons = { radioButton1, radioButton2, radioButton3, radioButton4 };
            int selectedIndex = Array.FindIndex(buttons, b => b.Checked);
            int previouslySelected = listBox1.SelectedIndex; // store previously selected index
            lastSelectedFrame = comboBox1.SelectedIndex;     // set previously selected index
            lastSelectedSub = comboBox2.SelectedIndex;       // set previously selected index
            foreach (var button in buttons)
            {
                button.Checked = true;                      // select each radio button
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SelectedIndex = i;             // select each item in the list box
                    button3_Click(null!, null!);            // call the export all button click event
                }
            }
            buttons[selectedIndex].Checked = true;
            listBox1.SelectedIndex = previouslySelected;    // restore previously selected index
            comboBox1.SelectedIndex = lastSelectedFrame;    // restore previously selected index
            comboBox2.SelectedIndex = lastSelectedSub;      // restore previously selected index
            ShowMessage($"All images saved to:\n{outputPath}");
            exporting = false;
        }
        // show message on successful export operation
        private void ShowMessage(string messageSuccess, string messageFail = "Failed to export : ")
        {
            if (saved) { MessageBox.Show(messageSuccess); }
            else { MessageBox.Show(messageFail + exception); }
        }
        // export all frames button
        private void button3_Click(object sender, EventArgs e)
        {
            if (!exporting) // set previously selected indexes on export all frames
            {
                lastSelectedFrame = comboBox1.SelectedIndex;
                lastSelectedSub = comboBox2.SelectedIndex;
            }
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
            if (!exporting) // restore previously selected index on export all frames
            {
                comboBox1.SelectedIndex = lastSelectedFrame;
                comboBox2.SelectedIndex = lastSelectedSub;
                ShowMessage($"Images saved to:\n{outputPath}");
            }
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
            if (comboBox1.SelectedIndex == lastSelectedSection) { return; }
            lastSelectedSection = comboBox1.SelectedIndex;
            var section = currentSections[comboBox1.SelectedIndex];
            try // TODO : remove try catch block here maybe
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
                    DetectFrames.ListSubFrames(lastSelectedFilePath, comboBox1, comboBox2);
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
                //DetectFrames.ReplaceSubFrame(lastSelectedFilePath, comboBox1, comboBox2, pictureBox1, filename[0]); // replace sub frame
                MessageBox.Show("Replacing compressed images is not supported yet.");
                return;
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
            if (compressed)
            {
                (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(lastSelectedFilePath), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            }
            else { (w, h) = TileRenderer.AutoDetectDimensions(currentSections[comboBox1.SelectedIndex].Data); }
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