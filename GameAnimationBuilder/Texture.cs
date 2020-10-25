using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{

    public class Texture : AnimatingObject, IScriptable
    {
        /*
            -GetContext
            -GetHint
            -GetSnippet
            -ParseData
            -AddToLib
            -GetPreviewBitmap
        */

        public string EncodedFilePath = "";
        public Bitmap Bitmap;
        public int RowCount, ColCount;

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.Id;

            if (order == 2)
                return ContextType.ImageFilePath;

            if ((order == 3) || (order == 4))
                return ContextType.Int;

            return ContextType.Unknown;
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"TEXTURE: Import spritesheet image.";

            if(order == 1)
                return $"TextureID: Should be TEXTURE_<OBJECT_KIND>";

            if(order == 2)
                return $"FilePath: Must be an image file";

            if(order == 3)
                return $"RowCount: Number of sprites in 1 sheet column";

            if(order == 4)
                return $"ColumnCount: Number of sprites in 1 sheet row";

            return "";
        }

        public string GetSnippet()
        {
            string result = "";

            result += "TEXTURE\r\n"
                    + "\tTextureId\r\n"
                    + "\t\"Filepath\"\r\n"
                    + "\tRowCount\tColumnCount\r\n";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count != 5)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];
            string filePath = "";
            
            try
            { 
                if(!Utils.IsValidEncodedString(codeWords[2]))
                    return $"Invalid filepath {codeWords[2]}";

                EncodedFilePath = codeWords[2];
                
                filePath = Utils.DecodePathToWork(EncodedFilePath);
                Bitmap = new Bitmap(filePath);
            }
            catch(Exception e)
            {
                return $"File path {filePath} is not found or not an image file.\n"
                    +  $"Hint: in case your file path has whitespace in it, replace it with {Utils.AlternativeSpaceInPath}\n";

                // return $"{Utils.UnknownErrorMsg}: {e.Message}";
            }

            try
            { 
                RowCount = int.Parse(codeWords[3]);
                ColCount = int.Parse(codeWords[4]);

                if(RowCount <= 0 || ColCount <= 0)
                    throw new FormatException();
            }
            catch(Exception e)
            {
                if(e is FormatException || e is OverflowException)
                    return $"Number of rows and columns must be a positive integer and not exceed 2^32";

                return $"{Utils.UnknownErrorMsg}: {e.Message}";
            }

            return "";
        }

        public override void AddToLib()
        {
            base.AddToLib();

            for(int x=0; x<ColCount; x++)
                for(int y=0; y<RowCount; y++)
                {
                    Sprite sprite = new Sprite(this, y, x);
                    sprite.AddToLib();
                }
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            return new Bitmap(Bitmap);
        }
    }
}
