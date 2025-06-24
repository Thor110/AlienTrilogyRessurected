using System.Drawing.Imaging;

namespace ALTViewer
{
    public class TileRenderer
    {
        public static Bitmap RenderTiledImage(byte[]? tntData, byte[] bndData, byte[] paletteData, int tileSize = 16, int width = 320, int height = 240)
        {
            int tilesPerRow = width / tileSize;
            int tilesPerCol = height / tileSize;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);

            var palette = LoadPalette(paletteData);
            var tiles = tntData != null ? ExtractTiles(tntData, tileSize, palette) : new List<Bitmap>();

            for (int y = 0; y < tilesPerCol; y++)
            {
                for (int x = 0; x < tilesPerRow; x++)
                {
                    int index = y * tilesPerRow + x;
                    if (index < bndData.Length)
                    {
                        int tileIndex = bndData[index];

                        if (tiles.Count > 0 && tileIndex >= 0 && tileIndex < tiles.Count)
                        {
                            g.DrawImage(tiles[tileIndex], x * tileSize, y * tileSize);
                        }
                        else
                        {
                            // optional: draw fallback tile (transparent by default)
                        }
                    }
                }
            }

            return bmp;
        }

        private static List<Bitmap> ExtractTiles(byte[] tntData, int tileSize, Color[] palette)
        {
            int tileByteSize = tileSize * tileSize;
            int tileCount = tntData.Length / tileByteSize;
            var tiles = new List<Bitmap>();

            for (int i = 0; i < tileCount; i++)
            {
                var bmp = new Bitmap(tileSize, tileSize, PixelFormat.Format32bppArgb);
                for (int y = 0; y < tileSize; y++)
                {
                    for (int x = 0; x < tileSize; x++)
                    {
                        int idx = i * tileByteSize + y * tileSize + x;
                        byte colorIndex = tntData[idx];
                        bmp.SetPixel(x, y, palette[colorIndex]);
                    }
                }
                tiles.Add(bmp);
            }

            return tiles;
        }

        private static Color[] LoadPalette(byte[] paletteData)
        {
            var colors = new Color[256];
            for (int i = 0; i < Math.Min(paletteData.Length / 3, 256); i++)
            {
                int r = paletteData[i * 3] * 4;
                int g = paletteData[i * 3 + 1] * 4;
                int b = paletteData[i * 3 + 2] * 4;
                colors[i] = Color.FromArgb(r, g, b);
            }
            return colors;
        }
    }

}
