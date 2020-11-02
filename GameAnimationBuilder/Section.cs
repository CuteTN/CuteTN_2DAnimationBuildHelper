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
        public string TextureId;

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.Id;

            if(order == 2)
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

            return "";
        }

        public string GetSnippet()
        {
            string result = "";
            result += "SECTION\r\n"
                    + "\tSectionId\r\n"
                    + "\tBackground\r\n";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count != 3)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];

            TextureId = codeWords[2];
            var texture = AnimatingObjectsLib.Instance.Get(TextureId) as Texture;
            if(texture == null)
                return $"Texture Id {TextureId} is not found";

            return "";
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            var texture = AnimatingObjectsLib.Instance.Get(TextureId) as Texture;
            return texture.GetPreviewBitmap(time);
        }
    }
}
