using System.Drawing.Imaging;
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

            // Read FORM header
            string formTag = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (formTag != "FORM")
                throw new Exception("Invalid BND file: missing FORM header.");

            int formSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            string platform = Encoding.ASCII.GetString(br.ReadBytes(4)); // e.g., "PSXT"

            // Parse chunks
            while (br.BaseStream.Position + 8 <= br.BaseStream.Length)
            {
                string chunkName = Encoding.ASCII.GetString(br.ReadBytes(4));
                int chunkSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);

                if (br.BaseStream.Position + chunkSize > br.BaseStream.Length)
                    break;

                byte[] chunkData = br.ReadBytes(chunkSize);

                if (chunkName.StartsWith("TP"))
                {
                    sections.Add(new BndSection
                    {
                        Name = chunkName,
                        Data = chunkData
                    });
                }

                // IFF padding to 2-byte alignment
                if ((chunkSize % 2) != 0)
                    br.BaseStream.Seek(1, SeekOrigin.Current);
            }

            return sections;
        }

        public static (int Width, int Height) AutoDetectDimensions(byte[] imageData)
        {
            int totalPixels = imageData.Length;
            int dim = (int)Math.Sqrt(totalPixels);
            if (dim * dim == totalPixels)
                return (dim, dim); // square image

            throw new Exception($"Unable to auto-detect dimensions for {totalPixels} bytes (not a perfect square).");
        }
        public static Bitmap RenderRaw8bppImage(byte[] pixelData, byte[] palette, int width, int height)
        {
            int colors = palette.Length / 3;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    if (idx >= pixelData.Length) continue;

                    byte colorIndex = pixelData[idx];
                    Color color = (colorIndex < colors)
                        ? Color.FromArgb(255, palette[colorIndex * 3], palette[colorIndex * 3 + 1], palette[colorIndex * 3 + 2])
                        : Color.Transparent;

                    bmp.SetPixel(x, y, color);
                }
            }

            return bmp;
        }
    }
}