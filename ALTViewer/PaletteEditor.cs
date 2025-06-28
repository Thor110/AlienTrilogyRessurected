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
        private List<BndSection> currentSections = new();
        public PaletteEditor(string selected, bool palfile, List<BndSection> loadedSections)
        {
            InitializeComponent();
            usePAL = palfile; // store boolean for latre use
            if (!palfile)
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
                palette = File.ReadAllBytes(fileDirectory); // store the selected palette
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
                int x = (i % 16) * 16;
                int y = (i / 16) * 16;
                x += 32;
                y += 32;
                Color color = Color.FromArgb(palette[i * 3], palette[i * 3 + 1], palette[i * 3 + 2]);
                using Brush b = new SolidBrush(color);
                e.Graphics.FillRectangle(b, x, y, 16, 16);
                e.Graphics.DrawRectangle(Pens.Black, x, y, 16, 16);
            }
        }
        // palette section mouse click event
        private void PaletteEditorForm_MouseClick(object sender, MouseEventArgs e)
        {
            int index = ((e.Y - 32) / 16) * 16 + ((e.X - 32) / 16);
            if (index < palette.Length / 3)
            {
                using ColorDialog dlg = new();
                dlg.Color = Color.FromArgb(palette[index * 3], palette[index * 3 + 1], palette[index * 3 + 2]);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    palette[index * 3 + 0] = dlg.Color.R;
                    palette[index * 3 + 1] = dlg.Color.G;
                    palette[index * 3 + 2] = dlg.Color.B;
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
            if (!usePAL)
            {
                MessageBox.Show("SAVE NOT IMPLEMENTED FOR EMBEDDED PALETTES YET!");
                //get file
                //seek to palette
                //check length
                //write palette
            }
            else
            {
                if (!File.Exists(backupDirectory)) // make backup of the original file
                {
                    File.Copy(fileDirectory, backupDirectory);
                    button2.Enabled = true; // enable restore backup button
                }
                File.WriteAllBytes(fileDirectory, palette);
                button1.Enabled = false; // disable save button
                MessageBox.Show("Palette saved successfully.");
            }
        }
        // restore backup button clicked
        private void button2_Click(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                MessageBox.Show("RESTORE BACKUP NOT IMPLEMENTED FOR EMBEDDED PALETTES YET!");
                //seek to palette
                //check length
                //write palette
                //delete backup
            }
            else
            {
                File.Move(backupDirectory, fileDirectory, true);
                File.Delete(backupDirectory);
                button2.Enabled = false; // restore backup button
                button1.Enabled = false; // disable save button
                palette = File.ReadAllBytes(fileDirectory);
                Invalidate();
                RenderImage();
                MessageBox.Show("Palette restored from backup.");
            }
        }
        // undo changes button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            if (!usePAL)
            {
                palette = TileRenderer.Convert16BitPaletteToRGB(
                    TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL0{comboBox1.SelectedIndex.ToString()}", 12));
            }
            else { palette = File.ReadAllBytes(fileDirectory); }
            Invalidate();
            RenderImage();
            button3.Enabled = false; // disable undo button
            button1.Enabled = false; // disable save button
        }
        // frame selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            backupDirectory = selectedPalette + $"_CL0{comboBox1.SelectedIndex}.BAK";
            RenderImage();
        }
        private void RenderImage()
        {
            var section = currentSections[comboBox1.SelectedIndex];
            if (compressed)
            {
                //var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                //pictureBox1.Image = TileRenderer.BuildIndexedBitmap(section.Data, currentPalette, width, height);
            }
            else
            {
                if (!usePAL) // update embedded palette to match selected frame
                {
                    palette = TileRenderer.Convert16BitPaletteToRGB(
                    TileRenderer.ExtractEmbeddedPalette(fileDirectory, $"CL0{comboBox1.SelectedIndex.ToString()}", 12));
                }
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
                string path = "";
                if (usePAL) // if using a .PAL file
                {
                    path = Path.Combine(fbd.SelectedPath, selectedPalette + ".PAL");
                }
                else // embedded palette
                {
                    path = Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(fileDirectory) + $"_CL0{comboBox1.SelectedIndex}.PAL");
                }
                File.WriteAllBytes(path, palette);
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
