using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace Utilities
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public DirectBitmap(Bitmap bitmap): this(bitmap.Width, bitmap.Height)
        {
            for (int i = 0; i < Width * Height; i++)
                Bits[i] = bitmap.GetPixel(i % Width, i / Width).ToArgb();
        }

        public DirectBitmap(string filename): this(new Bitmap(filename)) { }

        public void SetPixel(int x, int y, Color colour)
        {
            if (x < 0 || x >= Width)
                return;
            if (y < 0 || y >= Height)
                return;

            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= Width)
                return Color.Black;
            if (y < 0 || y >= Height)
                return Color.Black;

            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public DirectBitmap Crop(int x, int y, int width, int height)
        {
            var result = new DirectBitmap(width, height);

            for(int ix=0; ix<width; ix++)
                for(int iy=0; iy<height; iy++)
                    result.SetPixel(ix, iy, this.GetPixel(ix+x, iy+y));

            return result;
        }

        public void DrawOver(int x, int y, DirectBitmap bitmap)
        {
            for (int ix = x; ix < x + bitmap.Width; ix++)
                for (int iy = y; iy < y + this.Height; iy++)
                    this.SetPixel(ix, iy, bitmap.GetPixel(ix - x, iy - y));
        }

        public void DrawOver(int x, int y, Bitmap bitmap)
        {
            for (int ix = x; ix < x + bitmap.Width; ix++)
                for (int iy = y; iy < y + this.Height; iy++)
                    this.SetPixel(ix, iy, bitmap.GetPixel(ix - x, iy - y));
        }

        public void SetTransparent()
        {
            for (int i = 0; i < Width * Height; i++)
                Bits[i] = 0;
        }
        
        public void Save(string filename)
        {
            Bitmap.Save(filename);
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
