using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    public class CollisionBox : AnimatingObject, IAdditionalProperty
    {
        /*
            GetContext
            GetHint
            GetSnippet
            ParseData
            AddToLib
            -GetPreviewBitmap
        */
        public string SpriteId = "";
        public Rectangle Box = new Rectangle();

        static private Brush brush = new SolidBrush( Color.FromArgb(200, Color.Green) );
        private Bitmap bitmap = null;

        public CollisionBox(string id, Sprite sprite, Rectangle box)
        {
            StringId = id;
            SpriteId = sprite.StringId;
            Box = box;
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            if (bitmap == null)
            {
                bitmap = (AnimatingObjectsLib.Instance.Get(SpriteId) as Sprite).GetPreviewBitmap(time);

                using(Graphics gfx = Graphics.FromImage(bitmap))
                {  
                    gfx.FillRectangle(brush, Box);
                }
            }

            return bitmap;
        }

    }
}
