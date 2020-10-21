using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace GameAnimationBuilder
{
    public class Sprite : AnimatingObject
    {
        /*
            GetContext
            GetSnippet
            ParseData
            AddToLib
            -GetPreviewBitmap
        */
        public string TextureId = "";
        public Rectangle Rectangle = new Rectangle();
        private Bitmap bitmap = null;

        public Sprite(Texture texture, int rowId, int colId)
        {
            TextureId = texture.StringId;
            StringId = TextureId + $"_R{rowId}C{colId}";

            Rectangle.Width  = texture.Bitmap.Width  / texture.ColCount;
            Rectangle.Height = texture.Bitmap.Height / texture.RowCount;
            Rectangle.X = colId * Rectangle.Width;
            Rectangle.Y = rowId * Rectangle.Height;
            
            DirectBitmap origin = new DirectBitmap(texture.Bitmap);
            DirectBitmap cropped = origin.Crop(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            bitmap = cropped.Bitmap;
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            return new Bitmap(bitmap);
        }
    }
}
