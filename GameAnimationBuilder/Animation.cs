using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    /*
        -GetContext
        -GetHint
        -GetSnippet
        -ParseData
        AddToLib
        -GetPreviewBitmap
    */
    public class Animation : AnimatingObject, IScriptable
    {
        public List<string> SpriteIds;
        public List<int> Durations;
        public int TotalDuration;

        public  ContextType GetContext(int order)
        {
            if (order == 0)
                return ContextType.Tag;

            if (order == 1)
                return ContextType.Id;

            if (order % 2 == 0)
                return ContextType.Sprite;
            else
                return ContextType.Int;
        }

        public string GetHint(int order)
        {
            if (order == 0)
                return $"ANIMATION: Include frames to animate";

            if (order == 1)
                return $"AnimationID: Should be ANIMATION_<OBJECT_KIND>_<ACTION>";

            int spriteIndex = order / 2;

            if (order % 2 == 0)
                return $"SpriteId{spriteIndex}: The sprite of {spriteIndex}-th frame";
            else
                return $"Duration{spriteIndex}: The duration of {spriteIndex}-th frame (in milisecond)";
        }

        public  string GetSnippet()
        {
            string result = "";

            result += "ANIMATION\r\n"
                    + "\tAnimationId\r\n"
                    + "\tSprite1\tDuration1\r\n"
                    + "\tSprite2\tDuration2\r\n"
                    + "\t...";

            return result;
        }


        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count < 2 || codeWords.Count % 2 == 1)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];

            TotalDuration = 0;
            SpriteIds = new List<string>();
            Durations = new List<int>();

            for(int i=2; i<codeWords.Count; i+=2)
            {
                string spriteId = codeWords[i];
                if(AnimatingObjectsLib.Instance.Get(spriteId) == null)
                    return $"Sprite Id {spriteId} is not found";

                int duration = 0;

                try
                {
                    duration = int.Parse(codeWords[i+1]);

                    if(duration <= 0)
                        throw new OverflowException();
                }
                catch(Exception e)
                {
                    if(e is FormatException || e is OverflowException)
                        return $"Frame duration must be a positive integer and not exceed 2^32";

                    return $"{Utils.UnknownErrorMsg}: {e.Message}";
                }

                TotalDuration += duration;
                SpriteIds.Add(spriteId);
                Durations.Add(duration);
            }

            return "";
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            if(TotalDuration == 0)
                return base.GetPreviewBitmap(time);

            int modTime = time % TotalDuration;

            int sum = 0;
            for(int i=0; i<SpriteIds.Count; i++)
            {
                sum += Durations[i];

                if(sum >= modTime)
                {
                    Sprite sprite = AnimatingObjectsLib.Instance.Get(SpriteIds[i]) as Sprite;
                    return sprite.GetPreviewBitmap();
                }
            }

            // It can be proven the code can not reach here
            return null;
        }
    }
}
