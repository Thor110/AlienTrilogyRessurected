using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace ALTViewer
{
    // Represents a section in a BND file
    public class BndSection
    {
        public string Name { get; set; } = "";
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
    public static class TileRenderer
    {
        // Parse BND file sections from a byte array
        public static List<BndSection> ParseBndFormSections(byte[] bnd)
        {
            var sections = new List<BndSection>();
            using var br = new BinaryReader(new MemoryStream(bnd));
            string formTag = Encoding.ASCII.GetString(br.ReadBytes(4)); // Read FORM header
            if (formTag != "FORM") { throw new Exception("Invalid BND file: missing FORM header."); }
            int formSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            string platform = Encoding.ASCII.GetString(br.ReadBytes(4)); // e.g., "PSXT"
            while (br.BaseStream.Position + 8 <= br.BaseStream.Length) // Parse chunks
            {
                string chunkName = Encoding.ASCII.GetString(br.ReadBytes(4));
                int chunkSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
                if (br.BaseStream.Position + chunkSize > br.BaseStream.Length) { break; }
                byte[] chunkData = br.ReadBytes(chunkSize);
                if (chunkName.StartsWith("TP") || chunkName.StartsWith("F0")) { sections.Add(new BndSection { Name = chunkName, Data = chunkData }); }
                if ((chunkSize % 2) != 0) { br.BaseStream.Seek(1, SeekOrigin.Current); } // IFF padding to 2-byte alignment
            }
            return sections;
        }
        // Auto-detect dimensions based on the total pixel count in the image data
        public static (int Width, int Height) AutoDetectDimensions(byte[] imageData)
        {
            int totalPixels = imageData.Length;
            var knownResolutions = new List<(int Width, int Height)> { (256, 256), (256, 128) };
            foreach (var res in knownResolutions) { if (res.Width * res.Height == totalPixels) { return res; } }
            // Try square fallback // probably not necessary now or when the enemies and guns are worked out. // TODO : remove this
            int dim = (int)Math.Sqrt(totalPixels);
            if (dim * dim == totalPixels) { return (dim, dim); }
            throw new Exception($"Unable to auto-detect dimensions for {totalPixels} bytes.");
        }
        // Render a raw 8bpp image with a palette
        public static Bitmap RenderRaw8bppImage(byte[] pixelData, byte[] palette, int width, int height)
        {
            int colors = palette.Length / 3;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    if (idx >= pixelData.Length) { continue; }
                    byte colorIndex = pixelData[idx];
                    if (colorIndex < colors)
                    {
                        int r = palette[colorIndex * 3] * 4;
                        int g = palette[colorIndex * 3 + 1] * 4;
                        int b = palette[colorIndex * 3 + 2] * 4;
                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b)); // Clamp values to 255 just in case
                    }
                    else // Fallback color for invalid palette index
                    {
                        MessageBox.Show("FALLBACK COLOUR ERROR");
                        //if (!transparency) { bmp.SetPixel(x, y, Color.Black); }
                        //else { bmp.SetPixel(x, y, Color.Transparent); }
                    }
                }
            }
            return bmp;
        }
        // used for embedded palettes 6-bit rgb 0-63
        public static byte[] Convert16BitPaletteToRGB(byte[] rawPalette)
        {
            if (rawPalette == null || rawPalette.Length < 2)
                throw new ArgumentException("Palette data is missing or too short.");

            int colorCount = rawPalette.Length / 2;
            byte[] rgbPalette = new byte[256 * 3];

            for (int i = 0; i < colorCount && i < 256; i++)
            {
                ushort color = (ushort)((rawPalette[i * 2 + 1] << 8) | rawPalette[i * 2]);
                int r = (color & 0x1F) * 63 / 31;
                int g = ((color >> 5) & 0x1F) * 63 / 31;
                int b = ((color >> 10) & 0x1F) * 63 / 31;

                rgbPalette[i * 3 + 0] = (byte)r;
                rgbPalette[i * 3 + 1] = (byte)g;
                rgbPalette[i * 3 + 2] = (byte)b;
            }

            return rgbPalette;
        }
        // Extract 8bpp indexed pixel data from a Bitmap
        public static byte[] Extract8bppData(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int stride = data.Stride;
            int height = data.Height;
            byte[] buffer = new byte[stride * height];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            bmp.UnlockBits(data);
            if (stride != bmp.Width) // Remove padding if necessary
            {
                byte[] cropped = new byte[bmp.Width * height];
                for (int y = 0; y < height; y++) { Array.Copy(buffer, y * stride, cropped, y * bmp.Width, bmp.Width); }
                return cropped;
            }
            return buffer;
        }
        // Rebuild a BND file from sections and INFO data
        public static byte[] RebuildBndForm(List<BndSection> sections, byte[] infoData)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write(Encoding.ASCII.GetBytes("FORM")); // Write FORM header
            bw.Write(new byte[4]); // Placeholder for form size (will update later)
            bw.Write(Encoding.ASCII.GetBytes("PSXT")); // Write platform signature, assumed fixed
            bw.Write(Encoding.ASCII.GetBytes("INFO"));
            bw.Write(infoData); // 16 bytes INFO section
            foreach (var section in sections)
            {
                bw.Write(Encoding.ASCII.GetBytes(section.Name));
                int chunkSize = section.Data.Length;
                byte[] sizeBytes = BitConverter.GetBytes(chunkSize).Reverse().ToArray(); // big endian
                bw.Write(sizeBytes);
                bw.Write(section.Data);
                if (chunkSize % 2 != 0) { bw.Write((byte)0); } // Align to even byte count
            }
            long fileSize = ms.Length; // Go back and write correct FORM size
            int formSize = (int)(fileSize - 8); // FORM size excludes first 8 bytes
            ms.Position = 4;
            byte[] formSizeBytes = BitConverter.GetBytes(formSize).Reverse().ToArray(); // big endian
            bw.Write(formSizeBytes);
            return ms.ToArray();
        }
        // Build an indexed bitmap from pixel data and a palette
        public static Bitmap BuildIndexedBitmap(byte[] pixelData, byte[] palette, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = bmp.Palette; // Set the palette
            for (int i = 0; i < palette.Length / 3 && i < 256; i++) { pal.Entries[i] = Color.FromArgb(255, palette[i * 3], palette[i * 3 + 1], palette[i * 3 + 2]); }
            bmp.Palette = pal;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed); // Lock and copy the pixel data
            int stride = data.Stride;
            for (int y = 0; y < height; y++) { Marshal.Copy(pixelData, y * width, data.Scan0 + y * stride, width); }
            bmp.UnlockBits(data);
            return bmp;
        }
        // Decompress a sprite section using a custom compression algorithm
        public static byte[] DecompressSpriteSection(byte[] input)
        {
            List<byte> output = new List<byte>();
            int i = 0;
            int ptr = 0; // input index
            while (true)
            {
                while (true) // Refill control bits if needed
                {
                    i >>= 1;
                    if ((i & 0xFF00) == 0)
                    {
                        if (ptr >= input.Length) { return output.ToArray(); }
                        i = 0xFF00 | input[ptr++];
                    }
                    if ((i & 1) == 1) { break; }
                    if (ptr >= input.Length) { return output.ToArray(); } // Copy literal byte
                    output.Add(input[ptr++]);
                }
                if (ptr >= input.Length) { return output.ToArray(); }
                int offs, size;
                if (input[ptr] >= 96)
                {
                    offs = input[ptr++] - 256;
                    size = 3;
                }
                else
                {
                    size = (input[ptr] & 0xF0) >> 4;
                    offs = (input[ptr] & 0x0F) << 8;
                    if (++ptr >= input.Length) { return output.ToArray(); }
                    offs |= input[ptr++];
                    if (offs == 0) { break; } // End of stream
                    offs = -offs;
                    if (size == 5)
                    {
                        if (ptr >= input.Length) { return output.ToArray(); }
                        size = input[ptr++] + 9;
                    }
                    else { size += 4; }
                }
                for (int j = 0; j < size - 1; j++) // Copy previous bytes from output
                {
                    int srcIndex = output.Count + offs;
                    if (srcIndex < 0 || srcIndex >= output.Count) { break; } // safety check
                    output.Add(output[srcIndex]);
                }
            }
            return output.ToArray();
        }
        // Extract F0## sections from a byte array, optionally decompressing them
        public static List<byte[]> ExtractF0Sections(byte[] data, bool decompress)
        {
            if (decompress) { data = DecompressSpriteSection(data); }
            List<byte[]> f0Sections = new();
            int offset = 0;
            while (offset < data.Length - 8)
            {
                // Check for F0## marker (e.g. F000, F001...)
                if (data[offset] == 'F' && (data[offset + 1] & 0xF0) == 0x30)
                {
                    int sectionLength = BitConverter.ToInt32(data, offset + 4);
                    if (sectionLength <= 0 || offset + 8 + sectionLength > data.Length) { break; } // malformed or truncated
                    byte[] section = new byte[4 + 4 + sectionLength]; // header + length + body
                    Array.Copy(data, offset, section, 0, section.Length);
                    f0Sections.Add(section);
                    offset += section.Length; // move to next section
                }
                else { offset++; }
            }
            return f0Sections;
        }
        // extract level palette from a level file C0## sections
        public static byte[] ExtractEmbeddedPalette(string filePath, string clSectionName, int skipHeader)
        {
            // TODO : this runs twice along with FindSectionDataOffset
            byte[] fileBytes = File.ReadAllBytes(filePath);
            long startOffset = FindSectionDataOffset(filePath, clSectionName, skipHeader);

            if (startOffset < 0 || startOffset >= fileBytes.Length) { throw new Exception("CL section not found or out of bounds."); }

            // Search for the next section header
            int maxSearch = fileBytes.Length;
            int sectionLength = maxSearch;

            for (int i = 0; i < maxSearch - 1; i++)
            {
                if ((startOffset + i + 1) >= fileBytes.Length) break; // Avoid OOB

                byte a = fileBytes[startOffset + i];
                byte b = fileBytes[startOffset + i + 1];

                if ((a == 0x43 && b == 0x30) || // C0 section ( compressed )
                    (a == 0x43 && b == 0x4C) || // CL
                    (a == 0x54 && b == 0x50) || // TP
                    (a == 0x42 && b == 0x58))   // BX
                {
                    sectionLength = i;
                    break;
                }
            }
            if (startOffset + sectionLength > fileBytes.Length)
            {
                sectionLength = fileBytes.Length - (int)startOffset;
            }
            byte[] palette = new byte[sectionLength];

            Array.Copy(fileBytes, startOffset, palette, 0, sectionLength);
            return palette;
        }
        // find the offset of the section data in the file
        public static long FindSectionDataOffset(string filePath, string sectionName, int skipHeader)
        {
            // TODO : stop this triggering twice
            byte[] label = Encoding.ASCII.GetBytes(sectionName);
            byte[] fileBytes = File.ReadAllBytes(filePath);
            for (int i = 0; i < fileBytes.Length - label.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < label.Length; j++)
                {
                    if (fileBytes[i + j] != label[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) { return i + skipHeader; } // label (4) + size (4) = data starts here ( 8 compressed / 12 uncompressed )
            }
            throw new Exception("Section not found in file.");
        }
        // Overwrite an embedded palette in a file
        public static void OverwriteEmbeddedPalette(string filePath, string sectionName, byte[] newPalette, int skipHeader)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            long offset = FindSectionDataOffset(filePath, sectionName, skipHeader);
            var replacements = new List<Tuple<long, byte[]>>();
            replacements.Add(new Tuple<long, byte[]>(offset, ConvertRGBTripletsToEmbeddedPalette(newPalette)));
            BinaryUtility.ReplaceBytes(replacements, filePath);
        }
        // Convert an RGB triplet palette (768 bytes) to an embedded 16-bit palette (512 bytes)
        public static byte[] ConvertRGBTripletsToEmbeddedPalette(byte[] rgbPalette768)
        {
            if (rgbPalette768.Length != 768)
                throw new ArgumentException("Expected a 768-byte palette (256 RGB triplets).");

            byte[] embeddedPalette = new byte[512]; // 256 entries × 2 bytes each

            for (int i = 0; i < 256; i++)
            {
                int r = rgbPalette768[i * 3 + 0] >> 3; // 5 bits
                int g = rgbPalette768[i * 3 + 1] >> 2; // 6 bits
                int b = rgbPalette768[i * 3 + 2] >> 3; // 5 bits

                // Pack into 16 bits (RGB565): RRRRRGGGGGGBBBBB
                ushort packed = (ushort)((r << 11) | (g << 5) | b);

                // Write as little-endian
                embeddedPalette[i * 2] = (byte)(packed & 0xFF);
                embeddedPalette[i * 2 + 1] = (byte)((packed >> 8) & 0xFF);
            }

            return embeddedPalette;
        }
        // Save an 8bpp indexed image as a PNG file with a specified palette
        public static void Save8bppPng(string path, byte[] indexedData, Color[] palette, int width, int height)
        {
            using Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // Set the palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length && i < 256; i++)
                pal.Entries[i] = palette[i];
            bmp.Palette = pal;

            // Lock the bitmap data
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            // Copy indexed data into the bitmap
            for (int y = 0; y < height; y++)
            {
                IntPtr dest = data.Scan0 + y * data.Stride;
                Marshal.Copy(indexedData, y * width, dest, width);
            }

            bmp.UnlockBits(data);
            bmp.Save(path, ImageFormat.Png);
        }
        // Convert a 768-byte RGB triplet palette to a Color array
        public static Color[] ConvertPalette(byte[] rgbTriplets)
        {
            var colors = new Color[256];
            for (int i = 0; i < 256; i++)
                colors[i] = Color.FromArgb(
                    rgbTriplets[i * 3] * 4,
                    rgbTriplets[i * 3 + 1] * 4,
                    rgbTriplets[i * 3 + 2] * 4
                );
            return colors;
        }
    }
}