namespace ALTViewer
{
    public partial class PaletteEditor : Form
    {
        public static string paletteDirectory = "HDD\\TRILOGY\\CD\\PALS\\";
        public string backupDirectory = "";
        public string fileDirectory = "";
        public string selectedPalette = "";
        private byte[] palette = null!;
        private bool compressed;
        private List<BndSection> currentSections = new();
        public PaletteEditor(string selected, bool embedded, List<BndSection> loadedSections)
        {
            InitializeComponent();
            if (embedded)
            {

            }
            else
            {
                fileDirectory = paletteDirectory + selected + ".PAL";
                palette = File.ReadAllBytes(fileDirectory); // store the selected palette
            }
            selectedPalette = selected;
            currentSections = loadedSections;
            foreach (var section in currentSections) { comboBox1.Items.Add(section.Name); }
            comboBox1.SelectedIndex = 0;
            RenderImage();
            compressed = embedded;
            Paint += PaletteEditorForm_Paint!;
            MouseClick += PaletteEditorForm_MouseClick!;
            backupDirectory = fileDirectory + ".BAK";
            if (File.Exists(backupDirectory)) { button2.Enabled = true; }
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
                    button3.Enabled = true;
                }
            }
        }
        // save palette button clicked
        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(backupDirectory)) // make backup of the original file
            {
                File.Copy(fileDirectory, backupDirectory);
                button2.Enabled = true; // enable restore backup button
            }
            File.WriteAllBytes(fileDirectory, palette);
            MessageBox.Show("Palette saved successfully.");
        }
        // restore backup button clicked
        private void button2_Click(object sender, EventArgs e)
        {
            File.Move(backupDirectory, fileDirectory, true);
            File.Delete(backupDirectory);
            button2.Enabled = false;
            palette = File.ReadAllBytes(fileDirectory);
            Invalidate();
            RenderImage();
            MessageBox.Show("Palette restored from backup.");
        }
        // undo changes button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            palette = File.ReadAllBytes(fileDirectory);
            Invalidate();
            button3.Enabled = false;
        }
        // frame selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { RenderImage(); }
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
                string path = Path.Combine(fbd.SelectedPath, selectedPalette + ".PAL");
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
                    button3.Enabled = true;
                    Invalidate();
                    RenderImage();
                }
            }
        }
    }
}
