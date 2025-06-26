using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace ALTViewer
{
    public class BndSection
    {
        public string Name { get; set; } = "";
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
    public static class TileRenderer
    {
        public static List<BndSection> ParseBndFormSections(byte[] bnd)
        {
            var sections = new List<BndSection>();
            using var br = new BinaryReader(new MemoryStream(bnd));
            string formTag = Encoding.ASCII.GetString(br.ReadBytes(4)); // Read FORM header
            if (formTag != "FORM") { throw new Exception("Invalid BND file: missing FORM header."); }
            int formSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            string platform = Encoding.ASCII.GetString(br.ReadBytes(4)); // e.g., "PSXT"
            // Parse chunks
            while (br.BaseStream.Position + 8 <= br.BaseStream.Length)
            {
                string chunkName = Encoding.ASCII.GetString(br.ReadBytes(4));
                int chunkSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
                if (br.BaseStream.Position + chunkSize > br.BaseStream.Length) { break; }
                byte[] chunkData = br.ReadBytes(chunkSize);
                if (chunkName.StartsWith("TP") || chunkName.StartsWith("F0"))
                {
                    sections.Add(new BndSection { Name = chunkName, Data = chunkData });
                }
                // IFF padding to 2-byte alignment
                if ((chunkSize % 2) != 0) { br.BaseStream.Seek(1, SeekOrigin.Current); }
            }
            return sections;
        }
        public static (int Width, int Height) AutoDetectDimensions(byte[] imageData)
        {
            int totalPixels = imageData.Length;
            var knownResolutions = new List<(int Width, int Height)> { (256, 256), (256, 128) };
            foreach (var res in knownResolutions) { if (res.Width * res.Height == totalPixels) { return res; } }
            // Try square fallback // probably not necessary now or when the enemies and guns are worked out.
            int dim = (int)Math.Sqrt(totalPixels);
            if (dim * dim == totalPixels) { return (dim, dim); }
            throw new Exception($"Unable to auto-detect dimensions for {totalPixels} bytes.");
        }
        public static Bitmap RenderRaw8bppImage(byte[] pixelData, byte[] palette, int width, int height, bool transparency)
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

                        // Clamp values to 255 just in case
                        bmp.SetPixel(x, y, Color.FromArgb(255, Math.Min(r, 255), Math.Min(g, 255), Math.Min(b, 255)));
                    }
                    else
                    {
                        // Fallback color for invalid palette index
                        if (!transparency) { bmp.SetPixel(x, y, Color.Magenta); }
                        else { bmp.SetPixel(x, y, Color.Transparent); }
                    }
                }
            }

            return bmp;
        }
        public static byte[] Extract8bppData(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int stride = data.Stride;
            int height = data.Height;
            byte[] buffer = new byte[stride * height];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            bmp.UnlockBits(data);
            // Remove padding if necessary
            if (stride != bmp.Width)
            {
                byte[] cropped = new byte[bmp.Width * height];
                for (int y = 0; y < height; y++)
                {
                    Array.Copy(buffer, y * stride, cropped, y * bmp.Width, bmp.Width);
                }
                return cropped;
            }
            return buffer;
        }
        public static byte[] RebuildBndForm(List<BndSection> sections, byte[] infoData)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            // Write FORM header
            bw.Write(Encoding.ASCII.GetBytes("FORM"));
            // Placeholder for form size (will update later)
            bw.Write(new byte[4]);
            // Write platform signature, assumed fixed
            bw.Write(Encoding.ASCII.GetBytes("PSXT"));
            bw.Write(Encoding.ASCII.GetBytes("INFO"));
            bw.Write(infoData); // 16 bytes INFO section
            foreach (var section in sections)
            {
                bw.Write(Encoding.ASCII.GetBytes(section.Name));
                int chunkSize = section.Data.Length;
                byte[] sizeBytes = BitConverter.GetBytes(chunkSize).Reverse().ToArray(); // big endian
                bw.Write(sizeBytes);
                bw.Write(section.Data);
                // Align to even byte count
                if (chunkSize % 2 != 0)
                {
                    bw.Write((byte)0);
                }
            }
            // Go back and write correct FORM size
            long fileSize = ms.Length;
            int formSize = (int)(fileSize - 8); // FORM size excludes first 8 bytes
            ms.Position = 4;
            byte[] formSizeBytes = BitConverter.GetBytes(formSize).Reverse().ToArray(); // big endian
            bw.Write(formSizeBytes);
            return ms.ToArray();
        }
        public static Bitmap BuildIndexedBitmap(byte[] pixelData, int width, int height, byte[] palette)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            // Set the palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length / 3 && i < 256; i++)
            {
                pal.Entries[i] = Color.FromArgb(255, palette[i * 3], palette[i * 3 + 1], palette[i * 3 + 2]);
            }
            bmp.Palette = pal;
            // Lock and copy the pixel data
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int stride = data.Stride;
            for (int y = 0; y < height; y++)
            {
                Marshal.Copy(pixelData, y * width, data.Scan0 + y * stride, width);
            }
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}