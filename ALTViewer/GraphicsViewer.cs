using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ALTViewer
{
    public partial class GraphicsViewer : Form
    {
        public GraphicsViewer()
        {
            InitializeComponent();
            //TestRender();
            GraphicsViewer_Load(this, EventArgs.Empty); // Load the image on form load
        }
        public void TestRender()
        {
            var palette = LoadPalette("TRILOGY\\CD\\PALS\\NEWFONT.PAL");
            var bmp = RenderIndexedImage("TRILOGY\\CD\\GFX\\FONT1GFX.BND", palette); // Adjust width/height if needed
            pictureBox1.Image = bmp;
        }
        private void GraphicsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                string imagePath = "TRILOGY\\CD\\GFX\\FONT1GFX.BND"; // your test file
                string palettePath = "TRILOGY\\CD\\PALS\\NEWFONT.PAL";
                Color[] palette = LoadPalette(palettePath);
                Bitmap bmp = RenderIndexedImage(imagePath, palette);

                if (bmp != null)
                {
                    pictureBox1.Image = bmp;
                }
                else
                {
                    MessageBox.Show("Bitmap render failed.");
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("fatal_error_log.txt", ex.ToString());
                MessageBox.Show("Exception thrown: " + ex.Message);
            }
        }
        private Color[] LoadPalette(string path)
        {
            byte[] paletteBytes = File.ReadAllBytes(path);

            int colorCount = paletteBytes.Length / 3;
            if (colorCount == 0 || paletteBytes.Length % 3 != 0)
            {
                throw new Exception("Invalid palette file length.");
            }

            Color[] palette = new Color[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                int r = paletteBytes[i * 3];
                int g = paletteBytes[i * 3 + 1];
                int b = paletteBytes[i * 3 + 2];
                palette[i] = Color.FromArgb(r << 2, g << 2, b << 2); // Often 6-bit VGA (multiply by 4)
            }

            return palette;
        }
        public static Bitmap RenderIndexedImage(string imagePath, Color[] palette)
        {
            byte[] fileBytes = File.ReadAllBytes(imagePath);

            if (fileBytes.Length < 32)
            {
                MessageBox.Show("File too small to contain valid image data.");
                return null;
            }

            // Print header for debugging
            string header = BitConverter.ToString(fileBytes.Take(32).ToArray());
            File.WriteAllText("header_debug.txt", header);

            // Attempt to read header dimensions
            ushort width = BitConverter.ToUInt16(fileBytes, 0);   // e.g., 0x00 0x01 = 256 (LE)
            ushort height = BitConverter.ToUInt16(fileBytes, 2);  // e.g., 0x06 0x14 = 5126 (LE), possibly wrong

            if (width > 1024 || height > 1024)
            {
                // Possibly incorrect — try alternative bytes
                width = BitConverter.ToUInt16(fileBytes, 4);   // Try offset 4
                height = BitConverter.ToUInt16(fileBytes, 6);  // Try offset 6
            }

            // Print decoded dimensions
            File.AppendAllText("header_debug.txt", $"\nWidth: {width}, Height: {height}, FileLength: {fileBytes.Length}");

            int pixelDataStart = 8;  // Guess
            int pixelCount = width * height;

            if (pixelDataStart + pixelCount > fileBytes.Length)
            {
                MessageBox.Show("Image size exceeds file length.");
                return null;
            }

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette bmpPalette = bmp.Palette;
            for (int i = 0; i < 256; i++)
                bmpPalette.Entries[i] = palette[i];
            bmp.Palette = bmpPalette;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            IntPtr ptr = data.Scan0;

            // Copy pixel data
            Marshal.Copy(fileBytes, pixelDataStart, ptr, pixelCount);
            bmp.UnlockBits(data);

            return bmp;
        }

    }
}
