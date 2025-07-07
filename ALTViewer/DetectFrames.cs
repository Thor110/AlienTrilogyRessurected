﻿using System.Security.Policy;

namespace ALTViewer
{
    internal class DetectFrames
    {
        public static byte[] RenderSubFrame(string fileDirectory, ComboBox comboBox1, ComboBox comboBox2, PictureBox pictureBox1, byte[] palette, int transparent, bool multiple, bool none, int[] values = null!)
        {
            int w = 0, h = 0;
            (w, h) = DetectDimensions.AutoDetectDimensions(Path.GetFileNameWithoutExtension(fileDirectory), comboBox1.SelectedIndex, comboBox2.SelectedIndex);
            pictureBox1.Width = w;
            pictureBox1.Height = h;
            byte[] fullFile = File.ReadAllBytes(fileDirectory);
            List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
            List<BndSection> f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            BndSection section = f0Sections[comboBox1.SelectedIndex];
            List<byte[]> frames = TileRenderer.DecompressAllFramesInSection(section.Data);
            byte[] frameData = frames[comboBox2.SelectedIndex];
            try { pictureBox1.Image = TileRenderer.RenderRaw8bppImage(frameData, palette, w, h, transparent, multiple, none, values); }
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
            List<BndSection> f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            // find offset of selected frame insead of decompressing
            int index = comboBox1.SelectedIndex;
            BndSection section = f0Sections[index];
            // compress the new frame image to match the original frame data
            long offset = TileRenderer.FindBndFormSectionOffset(fullFile, index); // get the offset of the selected frame
            // extract the compressed frames from the section data to compare against the newly compressed frame
            List<(byte[] frame, int length)> compressedFrames = TileRenderer.ExtractCompressedFrames(section.Data);
            byte[] oldCompressed = compressedFrames[comboBox2.SelectedIndex].frame;
            int relativeOffset = 0;
            // calculate the offset of the selected frame in the section data
            // detect padding inbetween each frame and adjust offset accordingly
            for (int i = 0; i < comboBox2.SelectedIndex; i++)
            {
                int length = compressedFrames[i].length;
                int additional = (8 - (length % 8)) % 8;
                relativeOffset += additional + length;
            }
            // add 8 for current section F0## header
            offset += relativeOffset + 8;
            // compress the new frame image to match the original frame data
            byte[] bytes = TileRenderer.CompressFrameToPicFormat(TileRenderer.Extract8bppData(frameImage)); // TODO : get compression working
            if (bytes.Length != oldCompressed.Length) // compare new frame byte array length to the original
            {
                MessageBox.Show($"Compressed data length does not match the original compressed frame length.\n\nOriginal : {oldCompressed.Length}\n\nNew : {bytes.Length}");
                return;
            }
            return; // return here while testing
            // write the new frame data to the section of the file
            BinaryUtility.ReplaceBndFrameWith8ByteAlignment(fileDirectory, offset, oldCompressed.Length, bytes); // replace the frame data in the file
            MessageBox.Show("Animation frame replaced successfully.");
        }
        // list sub frames
        public static void ListSubFrames(string fileDirectory, ComboBox comboBox1, ComboBox comboBox2)
        {
            comboBox2.Items.Clear();
            // Get original B16 file
            byte[] fullFile = File.ReadAllBytes(fileDirectory);
            List<BndSection> allSections = TileRenderer.ParseBndFormSections(fullFile);
            List<BndSection> f0Sections = allSections.Where(s => s.Name.StartsWith("F0")).ToList();
            // Get section currently selected in comboBox1
            string selectedSectionName = comboBox1.SelectedItem!.ToString()!;
            var selectedOriginalSection = f0Sections.FirstOrDefault(s => s.Name == selectedSectionName);
            if (selectedOriginalSection == null) // this should never happen
            {
                MessageBox.Show("Selected section not found in original file.");
                return;
            }
            List<byte[]> frames = TileRenderer.DecompressAllFramesInSection(selectedOriginalSection.Data);
            for (int i = 0; i < frames.Count; i++) { comboBox2.Items.Add($"Frame {i}"); }
            if (comboBox2.Items.Count > 0) { comboBox2.SelectedIndex = 0; }
        }
    }
}