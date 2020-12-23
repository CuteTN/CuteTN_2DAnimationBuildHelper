using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class Section : AnimatingObject, IScriptable
    {
        public string BackgroundId, ForegroundId;

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.Id;

            if(order == 2)
                return ContextType.Texture;

            if(order == 3)
                return ContextType.Texture;

            return ContextType.Unknown;
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"SECTION: Contain a group of game objects";

            if(order == 1)
                return $"SectionId: Should be SECTION_ID";

            if(order == 2)
                return $"Background: The background image for the section, this also gives the section the rendering size";

            if(order == 2)
                return $"Foreground: The foreground image for the section";

            return "";
        }

        public string GetSnippet()
        {
            string result = "";
            result += "SECTION\r\n"
                    + "\tSectionId\r\n"
                    + "\tBackground\r\n"
                    + "\tForeground\r\n";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count != 4)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];

            BackgroundId = codeWords[2];
            var texture = AnimatingObjectsLib.Instance.Get(BackgroundId) as Texture;
            if(texture == null)
                return $"Texture Id {BackgroundId} is not found";

            ForegroundId = codeWords[3];
            texture = AnimatingObjectsLib.Instance.Get(ForegroundId) as Texture;
            if (texture == null)
                return $"Texture Id {ForegroundId} is not found";

            return "";
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            var texture = AnimatingObjectsLib.Instance.Get(BackgroundId) as Texture;
            return texture.GetPreviewBitmap(time);
        }
    }
}
