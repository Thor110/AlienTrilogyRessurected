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
            int frameIndex = comboBox2.SelectedIndex;
            byte[] frameData = frames[frameIndex];
            try
            {
                pictureBox1.Image = TileRenderer.RenderRaw8bppImage(frameData, palette, w, h);
            }
            catch (Exception ex) { MessageBox.Show("Render failed: " + ex.Message); }
            return frameData;
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
