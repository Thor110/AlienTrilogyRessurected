namespace ALTViewer
{
    public partial class PaletteEditor : Form
    {
        public static string paletteDirectory = "";
        public string backupDirectory = "";
        public string fileDirectory = "";
        public string selectedPalette = ""; // either the name of the .PAL file or the name and location of the source file for the embedded palette
        private byte[] palette = null!;
        private bool compressed;
        private bool usePAL;
        private bool trim;
        private int w = 0;
        private int h = 0;
        private int lastSelectedSubFrame = -1;
        private bool changesMade;
        private List<BndSection> currentSections = new();
        private HashSet<Color> usedColors = new HashSet<Color>();
        public PaletteEditor(string selected, bool palfile, List<BndSection> loadedSections, bool compression, bool trimmed)
        {
            InitializeComponent();
            if (File.Exists("Run.exe"))
            {
                paletteDirectory = "HDD\\TRILOGY\\CD\\PALS\\";
            }
            else if (File.Exists("TRILOGY.EXE"))
            {
                paletteDirectory = "CD\\PALS\\";
            }
            usePAL = palfile; // store boolean for later use
            compressed = compression; // is the file compressed or not
            trim = trimmed; // is the palette file trimmed or not (e.g. PRISHOLD, COLONY, BONESHIP)
            if (selected.Contains("PANEL"))
            {
                MessageBox.Show("Viewing and editing these palettes is not properly implemented yet. ( PANEL3GF & PANELGFX )");
                pictureBox1.Height = 128; // set picture box height for PANEL3GF & PANELGFX
            }
            if (!palfile)
            {
                fileDirectory = selected; // set selected filepath instead of palette path
                selectedPalette = Path.GetDirectoryName(fileDirectory) + "\\" + Path.GetFileNameWithoutExtension(fileDirectory);
                string extension = "";
                if (!compressed)
                {
                    extension = "_CL00.BAK"; // backup extension
                    palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(selected, "CL00", 12));
                }
                else
                {
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    extension = "_C000.BAK"; // backup extension
                    palette = File.ReadAllBytes(fileDirectory);
                    palette = TileRenderer.Convert16BitPaletteToRGB(palette.Skip(palette.Length - 512).Take(512).ToArray());
                }
                backupDirectory = selectedPalette + extension; // set backup directory
            }
            else
            {
                selectedPalette = selected; // set selected palette filename
                fileDirectory = paletteDirectory + selected + ".PAL"; // set selected palette filepath
                backupDirectory = fileDirectory + ".BAK"; // set backup directory
                palette = File.ReadAllBytes(fileDirectory); // store the selected palette
                if (trim) // trimmed palettes [672]
                {
                    byte[] loaded = palette;
                    palette = new byte[768];
                    Array.Copy(loaded, 0, palette, 96, 672); // 96 padded bytes at the beginning for these palettes
                }
                else { label2.Visible = false; } // hide note on trimmed colours
            }
            currentSections = loadedSections;
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            if (File.Exists(backupDirectory)) { button2.Enabled = true; } // backup exists
        }
        private void PaletteEditor_Shown(object sender, EventArgs e)
        {
            // Force layout and painting to complete
            this.Refresh();           // Forces repaint immediately
            Application.DoEvents();   // Processes pending paint messages
            // Now run your code after form and controls fully painted
            DetectUnusedColors();
        }
        private void DetectUnusedColors()
        {
            // build a list of unused colours from all sections and frames
            if (!compressed)
            {
                if (usePAL)
                {
                    for (int i = 0; i < comboBox1.Items.Count; i++)
                    {
                        comboBox1.SelectedIndex = i; // set the selected index to the current section
                        TestImageColours();
                    }
                }
            }
            else
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    comboBox1.SelectedIndex = i; // set the selected index to the current section
                    for (int f = 0; f < comboBox2.Items.Count; f++)
                    {
                        comboBox2.SelectedIndex = f; // set the selected index to the current section
                        TestImageColours();
                    }
                }
            }
            label4.Visible = false; // hide detecting unused colours label
            pictureBox2.Visible = false; // hide the loading background picture box
            comboBox1.SelectedIndex = 0; // reset to first frame
            Paint += PaletteEditorForm_Paint!;
            MouseClick += PaletteEditorForm_MouseClick!;
        }
        private void TestImageColours()
        {
            HashSet<Color> testColors = GetUsedColors((Bitmap)pictureBox1.Image); // check current section and frame
            usedColors.UnionWith(testColors); // add the used colors to the set
        }
        // draw palette
        private void PaletteEditorForm_Paint(object sender, PaintEventArgs e)
        {
            if (!compressed && !usePAL)
            {
                usedColors = GetUsedColors((Bitmap)pictureBox1.Image);
            }
            Pen crossPen = new Pen(Color.Red, 2);
            for (int i = 0; i < palette.Length / 3; i++)
            {
                int x = (i % 16) * 16 + 32; // + 32 for the initial offset
                int y = (i / 16) * 16 + 32;

                Color color = ScaleColour(i);
                using Brush brush = new SolidBrush(color);
                e.Graphics.FillRectangle(brush, x, y, 16, 16);

                if (trim && i < 32)
                {
                    e.Graphics.DrawLine(crossPen, x, y, x + 16, y + 16);
                    e.Graphics.DrawLine(crossPen, x + 16, y, x, y + 16);
                }
                else if (!usedColors.Contains(color)) // visual display for unused colours
                {
                    e.Graphics.DrawLine(crossPen, x + 16, y, x, y + 16);
                }
            }
        }
        // get used colors from the image
        private HashSet<Color> GetUsedColors(Bitmap bmp)
        {
            HashSet<Color> usedColors = new HashSet<Color>();
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color color = bmp.GetPixel(x, y);
                    usedColors.Add(color);
                }
            }
            return usedColors;
        }
        private Color ScaleColour(int index) { return Color.FromArgb(palette[index * 3] * 4, palette[index * 3 + 1] * 4, palette[index * 3 + 2] * 4); }
        // palette section mouse click event
        private void PaletteEditorForm_MouseClick(object sender, MouseEventArgs e)
        {
            int offset = 32;
            int cellSize = 16;
            int cols = 16;
            int totalColors = palette.Length / 3;

            // Calculate column and row
            int col = (e.X - offset) / cellSize;
            int row = (e.Y - offset) / cellSize;

            // Ignore clicks outside the palette grid area:
            if (col < 0 || col >= cols || row < 0) { return; }

            int index = row * cols + col;

            if (index >= totalColors) { return; }

            if (trim && index < 32) { return; } // ignore trimmed colours
            using ColorDialog dlg = new();
            // Show scaled color in the dialog
            dlg.Color = ScaleColour(index);
            usedColors.Remove(dlg.Color);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                palette[index * 3] = (byte)(dlg.Color.R / 4);
                palette[index * 3 + 1] = (byte)(dlg.Color.G / 4);
                palette[index * 3 + 2] = (byte)(dlg.Color.B / 4);
                usedColors.Add(Color.FromArgb(palette[index * 3] * 4, palette[index * 3 + 1] * 4, palette[index * 3 + 2] * 4)); // add the new color to the used colors set
                Invalidate();
                RenderImage();
                button3.Enabled = true; // enable undo button
                button1.Enabled = true; // enable save button
                changesMade = true;
            }
        }
        // save palette button clicked
        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(backupDirectory) && checkBox1.Checked) // make a backup of the original file if one doesn't already exist
            {
                File.Copy(fileDirectory, backupDirectory);
                //File.WriteAllBytes(backupDirectory, palette); // TODO : Backup just the palette, not the entire file.
                button2.Enabled = true; // enable restore backup button
            }
            if (!usePAL) // backup embedded palettes
            {
                if (!compressed) // embedded palettes [512]
                {
                    TileRenderer.OverwriteEmbeddedPalette(fileDirectory, $"CL{comboBox1.SelectedIndex.ToString():D2}", palette, 12);
                }
                else // compressed palettes [512]
                {
                    var replacements = new List<Tuple<long, byte[]>>();
                    replacements.Add(new Tuple<long, byte[]>(File.ReadAllBytes(fileDirectory).Length - 512, TileRenderer.ConvertRGBTripletsToEmbeddedPalette(palette)));
                    BinaryUtility.ReplaceBytes(replacements, fileDirectory);
                }
            }
            else // backup .PAL files
            {
                byte[] saving = palette; // palette store
                if (trim) // regular palettes [768]
                {
                    saving = new byte[672]; // resize to 672 bytes
                    Array.Copy(palette, 96, saving, 0, 672);
                    MessageBox.Show("Note: First 32 unused colors were trimmed from this palette.");
                }
                File.WriteAllBytes(fileDirectory, saving);
            }
            button1.Enabled = false; // disable save button
            MessageBox.Show("Palette saved successfully.");
        }
        // restore backup button clicked
        private void button2_Click(object sender, EventArgs e)
        {
            File.Move(backupDirectory, fileDirectory, true);
            if (!usePAL)
            {
                //File.Move(backupDirectory, fileDirectory, true);
                //palette = File.ReadAllBytes(backupDirectory);
                if (!compressed) // embedded palettes [512]
                {
                    // TODO : Fix restore backup for embedded palettes
                    //TileRenderer.OverwriteEmbeddedPalette(fileDirectory, $"CL{comboBox1.SelectedIndex.ToString():D2}", palette, 12);
                    palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(fileDirectory, "CL00", 12));
                }
                else // compressed palettes [512]
                {
                    // TODO : Fix restore backup for compressed palettes
                    //palette = File.ReadAllBytes(fileDirectory);
                    //byte[] bytes = TileRenderer.Convert16BitPaletteToRGB(palette.Skip(palette.Length - 512).Take(512).ToArray());
                    //var replacements = new List<Tuple<long, byte[]>>();
                    //byte[] bytes = TileRenderer.ConvertRGBTripletsToEmbeddedPalette(palette);
                    //replacements.Add(new Tuple<long, byte[]>(File.ReadAllBytes(fileDirectory).Length - 512, bytes));
                    //BinaryUtility.ReplaceBytes(replacements, fileDirectory);
                    palette = File.ReadAllBytes(fileDirectory);
                    palette = TileRenderer.Convert16BitPaletteToRGB(palette.Skip(palette.Length - 512).Take(512).ToArray());
                }
                //File.Delete(backupDirectory);
            }
            else // regular & trimmed palettes [768 & 672] + LOGOSGFX [576]
            {
                //File.Move(backupDirectory, fileDirectory, true);
                //File.Delete(backupDirectory);
                //palette = File.ReadAllBytes(fileDirectory);
                byte[] loaded = File.ReadAllBytes(fileDirectory);
                if (palette.Length == 672)
                {
                    palette = new byte[768];
                    Array.Copy(loaded, 0, palette, 96, 672);
                }
                else
                {
                    palette = loaded;
                }
                // TODO : If I add byte precision instructions
                /*byte[] loaded = File.ReadAllBytes(fileDirectory);
                if (!trim) // regular palettes [768]
                {
                    File.Move(backupDirectory, fileDirectory, true);
                    palette = loaded;
                }
                else
                {
                    palette = new byte[768];
                    if(fileDirectory.Contains("LOGOSGFX")) // [576]
                    {
                        Array.Copy(loaded, 0, palette, 0, 576); // LOGOSGFX Edge Case
                    }
                    else // trimmed palettes [672]
                    {
                        Array.Copy(loaded, 0, palette, 96, 672); // 96 padded bytes at the beginning for these palettes
                    }
                    File.Delete(backupDirectory);
                }*/
            }
            File.Delete(backupDirectory);
            button2.Enabled = false; // restore backup button
            button1.Enabled = false; // disable save button
            changesMade = false;
            Invalidate();
            RenderImage();
            MessageBox.Show("Palette restored from backup.");
        }
        // undo changes button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                if (!compressed) // embedded palettes [512]
                {
                    palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL{comboBox1.SelectedIndex.ToString():D2}", 12));
                }
                else // compressed palettes [512]
                {
                    palette = File.ReadAllBytes(fileDirectory);
                    palette = TileRenderer.Convert16BitPaletteToRGB(palette.Skip(palette.Length - 512).Take(512).ToArray());
                }
            }
            else
            {
                palette = File.ReadAllBytes(fileDirectory); // regular palettes [768]
                if (trim) // trimmed palettes [672]
                {
                    byte[] loaded = palette;
                    palette = new byte[768];
                    Array.Copy(loaded, 0, palette, 96, 672); // 96 padded bytes at the beginning for these palettes
                }
            }
            Invalidate();
            RenderImage();
            button3.Enabled = false; // disable undo button
            button1.Enabled = false; // disable save button
            changesMade = false;
        }
        // frame selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO : Add last selected check
            if (!usePAL && !compressed) // only embedded palettes need to switch here
            {
                int index = comboBox1.SelectedIndex;
                backupDirectory = selectedPalette + $"_CL{index:D2}.BAK";
                palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL{index:D2}", 12));
            }
            else if (compressed)
            {
                lastSelectedSubFrame = -1; // reset last selected sub frame index
                DetectFrames.ListFrames(fileDirectory, comboBox1, comboBox2);
            }
            Invalidate();
            RenderImage();
        }
        private void RenderImage()
        {
            var section = currentSections[comboBox1.SelectedIndex];
            if (!compressed)
            {
                (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, palette!, w, h);
            }
            else
            {
                DetectFrames.RenderSubFrame(fileDirectory, comboBox1, comboBox2, pictureBox1, palette);
            }
        }
        // export palette file button click
        private void button4_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Select output folder to save the .PAL file.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                byte[] saving = palette;
                string path = "";
                if (!usePAL)
                {
                    if (!compressed) // embedded palettes [512]
                    {
                        path = Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(fileDirectory) + $"_CL{comboBox1.SelectedIndex:D2}.PAL");
                    }
                    else // compressed palettes [512]
                    {
                        path = Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(fileDirectory) + "_C000.PAL");
                    }
                }
                else // regular palettes [768]
                {
                    if (trim) // trimmed palettes [672]
                    {
                        saving = new byte[672]; // resize to 672 bytes
                        Array.Copy(palette, 96, saving, 0, 672);
                        MessageBox.Show("Note: First 32 unused colors were trimmed from this palette.");
                    }
                    path = Path.Combine(fbd.SelectedPath, selectedPalette + ".PAL");
                }
                File.WriteAllBytes(path, saving);
                MessageBox.Show($"Palette saved to : {path}");
            }
        }
        // import palette file button click
        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PAL Files (*.pal)|*.pal|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select a palette (.pal) file";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] loaded = File.ReadAllBytes(openFileDialog.FileName);
                    if (loaded.Length == 672) // BONESHIP, COLONY & PRISHOLD
                    {
                        palette = new byte[768];
                        trim = true; // set trimmed to true for these files
                        Array.Copy(loaded, 0, palette, 96, 672); // 96 padded bytes at the beginning for these palettes
                        MessageBox.Show("Note: First 32 unused colors were trimmed from this palette.");
                    }
                    else if (loaded.Length != 768)
                    {
                        MessageBox.Show("Palettes smaller than 768 or 672 bytes not supported.");
                        return;
                    }
                    else
                    {
                        palette = loaded;
                    }
                    button3.Enabled = true; // enable undo button
                    button1.Enabled = true; // enable save button
                    Invalidate();
                    RenderImage();
                }
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == lastSelectedSubFrame) { return; } // still happens twice on keyboard up / down
            lastSelectedSubFrame = comboBox2.SelectedIndex; // store last selected sub frame index
            DetectFrames.RenderSubFrame(fileDirectory, comboBox1, comboBox2, pictureBox1, palette);
            Invalidate();
        }
        // form closing event
        private void PaletteEditor_FormClosing(object sender, FormClosingEventArgs e) { UnsavedChanges(e, "exiting", button1); }
        // check for unsaved changes
        public void UnsavedChanges(FormClosingEventArgs e, string reason, Button button)
        { if (changesMade) { Utilities.UnsavedChanges(reason, () => button.PerformClick(), e); } }
    }
}
