using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class StateAnimation : AnimatingObject
    {
        /*
            GetContext
            GetSnippet
            ParseData
            AddToLib
            -GetPreviewBitmap
        */
        public string AnimationId;
        public bool FlipX;
        public bool FlipY;
        public int TimesRotate90;

        public int TotalDuration
        {
            get;
            private set;
        }


        /// <summary>
        /// Caution: the image would flip before it rotates
        /// </summary>
        /// <param name="id"></param>
        /// <param name="animation"></param>
        /// <param name="flipX"></param>
        /// <param name="flipY"></param>
        /// <param name="timesRotate90"></param>
        public StateAnimation(string id, Animation animation, bool flipX, bool flipY, int timesRotate90)
        {
            StringId = id;
            AnimationId = animation.StringId;
            FlipX = flipX;
            FlipY = flipY;
            TimesRotate90 = (timesRotate90 % 4 + 4) % 4;
            TotalDuration = animation.TotalDuration;
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            Bitmap result = new Bitmap(AnimatingObjectsLib.Instance.Get(AnimationId).GetPreviewBitmap(time));
            
            if(FlipX)
                result.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if(FlipY)
                result.RotateFlip(RotateFlipType.RotateNoneFlipY);
            
            for(int i=1; i<=TimesRotate90; i++)
                result.RotateFlip(RotateFlipType.Rotate90FlipNone);

            return result;
        }
    }
}
