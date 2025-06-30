namespace ALTViewer
{
    public partial class PaletteEditor : Form
    {
        public static string paletteDirectory = "HDD\\TRILOGY\\CD\\PALS\\";
        public string backupDirectory = "";
        public string fileDirectory = "";
        public string selectedPalette = ""; // either the name of the .PAL file or the name and location of the source file for the embedded palette
        private byte[] palette = null!;
        private bool compressed = false;
        private bool usePAL = false;
        private bool trim;
        private List<BndSection> currentSections = new();
        public PaletteEditor(string selected, bool palfile, List<BndSection> loadedSections, bool compression, bool trimmed)
        {
            InitializeComponent();
            usePAL = palfile; // store boolean for latre use
            compressed = compression; // is the file compressed or not
            trim = trimmed; // is the file trimmed or not (e.g. PRISHOLD, COLONY, BONESHIP)
            if (compressed)
            {
                fileDirectory = selected; // set selected filepath instead of palette path
                selectedPalette = Path.GetDirectoryName(fileDirectory) + "\\" + Path.GetFileNameWithoutExtension(fileDirectory);
                backupDirectory = selectedPalette + $"_C000.BAK"; // check for backup
                palette = File.ReadAllBytes(fileDirectory);
                palette = TileRenderer.Convert16BitPaletteToRGB(palette.Skip(palette.Length - 512).Take(512).ToArray());
            }
            else if (!palfile)
            {
                fileDirectory = selected; // set selected filepath instead of palette path
                selectedPalette = Path.GetDirectoryName(fileDirectory) + "\\" + Path.GetFileNameWithoutExtension(fileDirectory);
                backupDirectory = selectedPalette + $"_CL00.BAK"; // check for backup
                palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(selected, "CL00", 12));
            }
            else
            {
                fileDirectory = paletteDirectory + selected + ".PAL";
                backupDirectory = fileDirectory + ".BAK";
                if (trim) // these also use embedded palettes
                {
                    byte[] loaded = File.ReadAllBytes(fileDirectory);
                    palette = new byte[768];
                    Array.Copy(loaded, 0, palette, 96, Math.Min(loaded.Length, 672)); // 96 padded bytes at the beginning for these palettes
                }
                else
                {
                    palette = File.ReadAllBytes(fileDirectory); // store the selected palette
                }
                selectedPalette = selected;
            }
            currentSections = loadedSections;
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            comboBox1.SelectedIndex = 0; // trigger rendering
            Paint += PaletteEditorForm_Paint!;
            MouseClick += PaletteEditorForm_MouseClick!;
            if (File.Exists(backupDirectory)) { button2.Enabled = true; } // backup exists
        }
        // draw palette
        private void PaletteEditorForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < palette.Length / 3; i++)
            {
                int x = (i % 16) * 16 + 32; // + 32 for the initial offset
                int y = (i / 16) * 16 + 32;

                int r = Math.Min(palette[i * 3] * 4, 255);
                int g = Math.Min(palette[i * 3 + 1] * 4, 255);
                int b = Math.Min(palette[i * 3 + 2] * 4, 255);
                Color color = Color.FromArgb(r, g, b);
                using Brush brush = new SolidBrush(color);
                e.Graphics.FillRectangle(brush, x, y, 16, 16);

                if (trim && i < 32)
                {
                    using Pen crossPen = new Pen(Color.Red, 2);
                    e.Graphics.DrawLine(crossPen, x, y, x + 16, y + 16);
                    e.Graphics.DrawLine(crossPen, x + 16, y, x, y + 16);
                }
            }
        }
        // palette section mouse click event
        private void PaletteEditorForm_MouseClick(object sender, MouseEventArgs e)
        {
            int index = ((e.Y - 32) / 16) * 16 + ((e.X - 32) / 16);
            if(trim && index <= 31) { return; } // ignore trimmed colours
            if (index < palette.Length / 3)
            {
                using ColorDialog dlg = new();
                // Show scaled color in the dialog
                int r = Math.Min(palette[index * 3 + 0] * 4, 255);
                int g = Math.Min(palette[index * 3 + 1] * 4, 255);
                int b = Math.Min(palette[index * 3 + 2] * 4, 255);
                dlg.Color = Color.FromArgb(r, g, b);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    palette[index * 3 + 0] = (byte)(dlg.Color.R / 4);
                    palette[index * 3 + 1] = (byte)(dlg.Color.G / 4);
                    palette[index * 3 + 2] = (byte)(dlg.Color.B / 4);
                    Invalidate();
                    RenderImage();
                    button3.Enabled = true; // enable undo button
                    button1.Enabled = true; // enable save button
                }
            }
        }
        // save palette button clicked
        private void button1_Click(object sender, EventArgs e)
        {
            // TODO : backup logic needs to be improved, currently it only works for embedded palettes and .PAL files ( I think )
            if (!File.Exists(backupDirectory) && checkBox1.Checked) // make a backup of the original file
            {
                File.Copy(fileDirectory, backupDirectory);
                button2.Enabled = true; // enable restore backup button
            }
            if (!usePAL) // backup embedded palettes
            {
                if (!compressed)
                {
                    TileRenderer.OverwriteEmbeddedPalette(fileDirectory, $"CL0{comboBox1.SelectedIndex.ToString()}", palette, 12);
                }
                else
                {
                    var replacements = new List<Tuple<long, byte[]>>();
                    replacements.Add(new Tuple<long, byte[]>(File.ReadAllBytes(fileDirectory).Length - 512, TileRenderer.ConvertRGBTripletsToEmbeddedPalette(palette)));
                    BinaryUtility.ReplaceBytes(replacements, fileDirectory);
                }
            }
            else // backup .PAL files
            {
                if(trim)
                {
                    byte[] saving = new byte[672]; // resize to 672 bytes
                    Array.Copy(palette, 96, saving, 0, 672);
                    File.WriteAllBytes(fileDirectory, saving);
                    MessageBox.Show("Note: First 32 unused colors were trimmed from this palette.");
                }
                else
                {
                    File.WriteAllBytes(fileDirectory, palette);
                }
            }
            button1.Enabled = false; // disable save button
            MessageBox.Show("Palette saved successfully.");
        }
        // restore backup button clicked
        private void button2_Click(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                if (!compressed)
                {
                    palette = File.ReadAllBytes(backupDirectory);
                    TileRenderer.OverwriteEmbeddedPalette(fileDirectory, $"CL0{comboBox1.SelectedIndex.ToString()}", palette, 12);
                }
                else
                {
                    var replacements = new List<Tuple<long, byte[]>>();
                    replacements.Add(new Tuple<long, byte[]>(File.ReadAllBytes(fileDirectory).Length - 512, File.ReadAllBytes(backupDirectory)));
                    BinaryUtility.ReplaceBytes(replacements, fileDirectory);
                }
            }
            else
            {
                File.Move(backupDirectory, fileDirectory, true);
                palette = File.ReadAllBytes(fileDirectory);
            }
            File.Delete(backupDirectory);
            button2.Enabled = false; // restore backup button
            button1.Enabled = false; // disable save button
            Invalidate();
            RenderImage();
            MessageBox.Show("Palette restored from backup.");
        }
        // undo changes button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL0{comboBox1.SelectedIndex.ToString()}", 12));
            }
            else
            {
                if (trim)
                {
                    byte[] loaded = File.ReadAllBytes(fileDirectory);
                    palette = new byte[768];
                    Array.Copy(loaded, 0, palette, 96, Math.Min(loaded.Length, 672)); // 96 padded bytes at the beginning for these palettes
                }
                else
                {
                    palette = File.ReadAllBytes(fileDirectory);
                }
            }
            Invalidate();
            RenderImage();
            button3.Enabled = false; // disable undo button
            button1.Enabled = false; // disable save button
        }
        // frame selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                if(!compressed)
                {
                    int index = comboBox1.SelectedIndex;
                    backupDirectory = selectedPalette + $"_CL0{index}.BAK";
                    palette = TileRenderer.Convert16BitPaletteToRGB(TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL0{index}", 12));
                    Invalidate();
                }
            }
            RenderImage();
        }
        private void RenderImage()
        {
            var section = currentSections[comboBox1.SelectedIndex];
            if (compressed)
            {
                //var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                int w = 32; // BAMBI
                int h = 77;
                //int w = 84; // SHOTGUN
                //int h = 77;
                pictureBox1.Image = TileRenderer.BuildIndexedBitmap(section.Data, palette!, w, h);
            }
            else
            {
                var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, palette!, w, h, false);
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
                if (usePAL) // if using a .PAL file
                {
                    if(trim)
                    {
                        saving = new byte[672]; // resize to 672 bytes
                        Array.Copy(palette, 96, saving, 0, 672);
                        MessageBox.Show("Note: First 32 unused colors were trimmed from this palette.");
                    }
                    path = Path.Combine(fbd.SelectedPath, selectedPalette + ".PAL");
                }
                else // embedded palette
                {
                    path = Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(fileDirectory) + $"_CL0{comboBox1.SelectedIndex}.PAL");
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
                    palette = File.ReadAllBytes(openFileDialog.FileName);
                    button3.Enabled = true; // enable undo button
                    button1.Enabled = true; // enable save button
                    Invalidate();
                    RenderImage();
                }
            }
        }
    }
}
