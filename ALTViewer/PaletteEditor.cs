using System.Formats.Tar;
using static System.Collections.Specialized.BitVector32;

namespace ALTViewer
{
    public partial class PaletteEditor : Form
    {
        public static string paletteDirectory = "HDD\\TRILOGY\\CD\\PALS\\";
        public string backupDirectory = "";
        public string fileDirectory = "";
        private byte[] palette;
        private bool compressed;
        private List<BndSection> currentSections = new();
        public PaletteEditor(string selected, bool embedded, List<BndSection> loadedSections)
        {
            InitializeComponent();
            fileDirectory = paletteDirectory + selected + ".PAL";
            palette = File.ReadAllBytes(fileDirectory); // store the selected palette
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
            int index = (e.Y / 16) * 16 + (e.X / 16);
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
            File.Move(backupDirectory, fileDirectory);
            File.Delete(backupDirectory);
            button2.Enabled = false;
            ReDrawPalette();
            MessageBox.Show("Palette restored from backup.");
        }
        // undo changes button clicked
        private void button3_Click(object sender, EventArgs e) { ReDrawPalette(); }
        public void ReDrawPalette()
        {
            palette = File.ReadAllBytes(fileDirectory);
            Invalidate(); // Redraw with original colors
        }
        // frame selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderImage();
        }
        private void RenderImage()
        {
            var section = currentSections[comboBox1.SelectedIndex];
            if (compressed)
            {
                var selectedSection = currentSections[comboBox1.SelectedIndex];
                //pictureBox1.Image = TileRenderer.BuildIndexedBitmap(selectedSection.Data, currentPalette, width, height);
            }
            else
            {
                var (w, h) = TileRenderer.AutoDetectDimensions(section.Data);
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(section.Data, palette!, w, h, false);
            }
        }
    }
}
