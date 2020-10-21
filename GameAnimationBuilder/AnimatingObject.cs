using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public enum ContextType
    {
        Tag,
        Id,
        Int,
        Bool,
        FileName,
        Texture,
        Sprite,
        Animation,
        ObjectAnimatons,
        ObjectKind,
        NewStateId,
        NewCollisionBoxId,
        Unknown,
    }

    public abstract class AnimatingObject
    {
        /*
            GetContext
            GetSnippet
            ParseData
            AddToLib
            GetPreviewBitmap
        */

        /// <summary>
        /// [ Factory method ]
        /// </summary>
        /// <returns></returns>
        static public AnimatingObject Interpret(List<string> codeWords, out string errorReport)
        {
            AnimatingObject result = null;
            errorReport = "";

            // there should be at least a TAG and an ID
            if(codeWords.Count <= 1)
                return null;

            result = CreateTypeFromTag(codeWords[0]);
            if(result == null)
                return null;

            errorReport = (result as IScriptable)?.ParseData(codeWords);

            if(errorReport != "" && errorReport != null)
                return null;

            return result;
        }

        static public AnimatingObject CreateTypeFromTag(string tag)
        {
            switch (tag.ToUpper())
            {
                case "TEXTURE":
                    return new Texture();
                case "ANIMATION":
                    return new Animation();
                case "OBJECT_ANIMATIONS":
                    return new ObjectAnimations();
                case "COLLISION_BOX_GROUP":
                    return new CollisionBoxGroup();

                default:
                    // if the tag name is invalid, simply skip interpreting this
                    return null;
            }
        }

        public string StringId = "";

        public virtual void AddToLib()
        {
            AnimatingObjectsLib.Instance.Add(this);
        }

        public virtual Bitmap GetPreviewBitmap(int time = 0)
        {
            return new Bitmap(1, 1);
        }
    }
}
