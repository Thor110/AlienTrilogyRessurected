using System.Windows.Forms;

namespace ALTViewer
{
    internal class DetectFrames
    {
        public static byte[] RenderSubFrame(string fileDirectory, ComboBox comboBox1, ComboBox comboBox2, PictureBox pictureBox1, byte[] palette)
        {
            int w = 0, h = 0;
            (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(fileDirectory), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            pictureBox1.Width = w;
            pictureBox1.Height = h;
            byte[] fullFile = File.ReadAllBytes(fileDirectory);
            List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
            var f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            var section = f0Sections[comboBox1.SelectedIndex];
            List<byte[]> frames = TileRenderer.DecompressAllFramesInSection(section.Data);
            byte[] frameData = frames[comboBox2.SelectedIndex];
            try
            {
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(frameData, palette, w, h);
            }
            catch (Exception ex) { MessageBox.Show("Render failed: " + ex.Message); }
            return frameData;
        }
        public static void ReplaceSubFrame(string fileDirectory, ComboBox comboBox1, ComboBox comboBox2, PictureBox pictureBox1, string newFrame)
        {
            int w = 0, h = 0;
            (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(fileDirectory), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            byte[] fullFile = File.ReadAllBytes(fileDirectory); // file to replace a frame in
            Bitmap frameImage; // file to import a frame from
            try { frameImage = new Bitmap(newFrame); } // safety first...
            catch (Exception ex) { MessageBox.Show("Failed to load image:\n" + ex.Message); return; }
            if (!GraphicsViewer.IsIndexed8bpp(frameImage.PixelFormat)) { MessageBox.Show("Image must be 8bpp indexed PNG."); return; }
            if (new Size(w, h) != frameImage.Size) // compare dimensions
            {
                MessageBox.Show($"Imported frame dimensions do not match expected dimensions of {w}x{h} pixels. Please check the image file.");
                return;
            }
            // get original frame section and data
            // find section based on comboBox1 selection
            List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
            var f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            // find offset of selected frame insead of decompressing
            int index = comboBox1.SelectedIndex;
            var section = f0Sections[index];
            List<byte[]> frames = TileRenderer.DecompressAllFramesInSection(section.Data);
            // get the original frame
            byte[] frameData = frames[comboBox2.SelectedIndex];
            // compress the new frame image to match the original frame data
            long offset = TileRenderer.FindBndFormSectionOffset(fullFile, index); // get the offset of the selected frame
            byte[] bytes = TileRenderer.CompressFrameToPicFormat(TileRenderer.Extract8bppData(frameImage)); // compress frame
            if (bytes.Length != frameData.Length) // compare new frame byte array length to the original
            {
                MessageBox.Show($"Compressed data length does not match the original compressed frame length.\n\nOriginal : {frameData.Length}\n\nNew : {bytes.Length}");
                return;
            }
            // write the new frame data to the section of the file
            var replacements = new List<Tuple<long, byte[]>>();
            replacements.Add(new Tuple<long, byte[]>(offset, bytes));
            BinaryUtility.ReplaceBytes(replacements, fileDirectory);
        }
        // list sub frames
        public static void ListFrames(string fileDirectory, ComboBox comboBox1, ComboBox comboBox2)
        {
            comboBox2.Items.Clear();
            // Get original B16 file
            byte[] fullFile = File.ReadAllBytes(fileDirectory);
            List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
            var f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            // Get section currently selected in comboBox1
            var selectedSectionName = comboBox1.SelectedItem!.ToString();
            var selectedOriginalSection = f0Sections.FirstOrDefault(s => s.Name == selectedSectionName);
            if (selectedOriginalSection == null) // this should never happen
            {
                MessageBox.Show("Selected section not found in original file.");
                return;
            }
            var frames = TileRenderer.DecompressAllFramesInSection(selectedOriginalSection.Data);
            for (int i = 0; i < frames.Count; i++) { comboBox2.Items.Add($"Frame {i}"); }
            if (comboBox2.Items.Count > 0) { comboBox2.SelectedIndex = 0; }
        }
        // test method to report frame counts
        /*public static void PrintExtraFrameCounts(string b16File)
        {
            byte[] fullFile = File.ReadAllBytes(b16File);
            List<BndSection> sections = TileRenderer.ParseBndFormSections(fullFile);
            var f0Sections = sections.Where(s => s.Name.StartsWith("F0")).ToList();

            foreach (var section in f0Sections)
            {
                var frames = TileRenderer.DecompressAllFramesInSection(section.Data);
                MessageBox.Show($"[{Path.GetFileName(b16File)}] Section {section.Name} has {frames.Count} frame(s)");
            }
        }*/
    }
}
