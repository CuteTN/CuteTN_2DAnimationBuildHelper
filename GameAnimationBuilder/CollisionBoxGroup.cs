using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class CollisionBoxGroup : AnimatingObject, IScriptable
    {
        /*
            -GetContext
            -GetHint
            -GetSnippet
            -ParseData
            -AddToLib
            GetPreviewBitmap
        */
        static private readonly int parametersOnLine = 6;
        public List<CollisionBox> CollisionBoxes = new List<CollisionBox>();

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            switch(order % parametersOnLine)
            {
                case 1:
                    return ContextType.NewCollisionBoxId;
                case 2:
                    return ContextType.Sprite;
                case 3: case 4: case 5: case 0:
                    return ContextType.Int;
                default:
                    throw new Exception("Get Context for CollisionBoxGroup failed.");
            }
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"COLLISION_BOX_GROUP: define Collision boxes for a set of sprites";

            int colBoxIndex =  (order + parametersOnLine - 1) / parametersOnLine;

            switch(order % parametersOnLine)
            {
                case 1:
                    return $"NewCollisionBoxId{colBoxIndex}: Should be COLBOX_<SPRITE_ID>";
                case 2:
                    return $"SpriteId{colBoxIndex}: The {colBoxIndex}-th sprite to define collision box";
                case 3:
                    return $"Left{colBoxIndex}: X-coordinate of the left edge of {colBoxIndex}-th collision box";
                case 4:
                    return $"Top{colBoxIndex}: Y-coordinate of the top edge of {colBoxIndex}-th collision box";
                case 5:
                    return $"Right{colBoxIndex}: X-coordinate of the right edge of {colBoxIndex}-th collision box";
                case 0:
                    return $"Bottom{colBoxIndex}: Y-coordinate of the bottom edge of {colBoxIndex}-th collision box";
                default:
                    throw new Exception("Get Hint for CollisionBoxGroup failed.");
            }
        }

        public string GetSnippet()
        {
            string result = "";

            result += "COLLISION_BOX_GROUP\r\n"
                    + "\tNewCollisionBoxId1\tSpriteId1\tLeft1\tTop1\tRight1\tBottom1\r\n"
                    + "\tNewCollisionBoxId2\tSpriteId2\tLeft2\tTop2\tRight2\tBottom2\r\n"
                    + "\t...";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if (codeWords.Count % parametersOnLine != 1)
                return $"Please follow this snippet: \n{GetSnippet()}";

            for(int i=1; i<codeWords.Count; i+=parametersOnLine)
            {
                string colBoxId = codeWords[i];

                string spriteId = codeWords[i+1];
                Sprite sprite = AnimatingObjectsLib.Instance.Get(spriteId) as Sprite;

                if(sprite == null)
                {
                    return $"Sprite Id {spriteId} is not found";
                }

                Rectangle box = new Rectangle();
                int left, top, right, bottom;

                try
                {
                    left    = int.Parse(codeWords[i+2]);
                    top     = int.Parse(codeWords[i+3]);
                    right   = int.Parse(codeWords[i+4]);
                    bottom  = int.Parse(codeWords[i+5]);

                    if(left < 0 || top < 0 || right < 0 || bottom < 0)
                        throw new OverflowException();

                    if(left > right)
                        return "Left must be less than Right";

                    if(top > bottom)
                        return "Top must be less than Bottom";
                }
                catch(Exception e)
                {
                    if (e is FormatException || e is OverflowException)
                        return $"Box edges must be a positive integer and not exceed 2^32";

                    return $"{Utils.UnknownErrorMsg}: {e.Message}";
                }

                try
                { 
                    box.X = left;
                    box.Y = top;
                    box.Width = right - left + 1;
                    box.Height = bottom - top + 1;
                }
                catch(Exception e)
                {
                    return $"{Utils.UnknownErrorMsg}: {e.Message}";
                }

                CollisionBox colBox = new CollisionBox(colBoxId, sprite, box);
                CollisionBoxes.Add(colBox);
            }

            return "";
        }

        public override void AddToLib()
        {
            // I'm sorry Liskov...
            foreach(var box in CollisionBoxes)
                box.AddToLib();
        }
    }
}
