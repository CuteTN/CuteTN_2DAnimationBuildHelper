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
        // primitive
        Unknown = 0,
        Tag = 1,
        Id = 2,
        Int = 3,
        Bool = 4,
        String = 5,
        ImageFilePath = 6,
        TextFilePath = 7,
        NewStateId = 8,
        NewCollisionBoxId = 9,
        Type = 10,
        NewClassProp = 11,

        // Data
        Data = 100, // Everything in lib 
        Texture = 101,
        Sprite = 102,
        Animation = 103,
        ObjectAnimatons = 104,
        Class = 105,
        Object = 106,
        Section = 107,
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
        static public AnimatingObject InterpretScope(List<string> codeWords, out string errorReport)
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

        static public AnimatingObject InterpretScope(string scope, out string errorReport)
        {
            var words = new List<string>(scope.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries));
            var tempObj = AnimatingObject.InterpretScope(words, out errorReport);

            return tempObj;
        }

        /// <summary>
        /// Warning, this can only add independent data objects because it doesn't add object to lib automatically!
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorReport"></param>
        /// <returns></returns>
        static public List<AnimatingObject> Interpret(string code, out string errorReport)
        {
            var scopes = code.Split(Utils.EndScopeChar);
            errorReport = "";

            List<AnimatingObject> result = new List<AnimatingObject>();

            foreach (var sc in scopes)
            {
                string newError;
                var tempObj = InterpretScope(sc, out newError);

                errorReport += newError + "\n";
                if(tempObj != null)
                    result.Add(tempObj);
            }

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
                case "IMPORT":
                    return new Import();
                case "CLASS":
                    return new CClass();
                case "OBJECT":
                    return new CObject();
                case "SECTION":
                    return new Section();

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
