using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    // Custom class, let's not be confused with the "class" of C# itself :)

    /*
        -GetContext
        -GetHint
        -GetSnippet
        -ParseData
        AddToLib
        -GetPreviewBitmap
    */
    public class CClass : AnimatingObject, IDynamicContext
    {
        private static readonly int parametersOnLine = 4;

        /// map from property name into its position
        private Dictionary<string, int> nameDict = new Dictionary<string, int>();
        public List<PropertyInfo> Properties = new List<PropertyInfo>();
        public List<int> UndefinedPos = new List<int>();

        public PropertyInfo GetProperty(string propName)
        {
            return Properties[nameDict[propName]];
        }

        public PropertyInfo GetProperty(int index)
        {
            return Properties[index];
        }

        public int GetPropertyIndex(string propName)
        {
            return nameDict[propName];
        }

        public PropertyInfo GetUndefinedProperty(int index)
        {
            return GetProperty(UndefinedPos[index]);
        }

        public int GetUndefinedPropertyCount()
        {
            return UndefinedPos.Count;
        }

        public List<string> GetUndefinedPropertiesNames()
        {
            List<string> result = new List<string>();

            foreach(int i in UndefinedPos)
            {
                result.Add(Properties[i].Name);
            }

            return result;
        }

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.Id;

            switch(order % parametersOnLine)
            {
                case 2:
                    return ContextType.Type;
                case 3:
                    return ContextType.NewClassProp;
                case 0:
                    return ContextType.Data;
                case 1:
                    return ContextType.String;
                default:
                    throw new Exception("Get Context for Class failed.");
            }
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"CLASS: Define you own class of object";

            if(order == 1)
                return $"ClassId: Should be CLASS_<object kind>";

            int propIndex = (order + parametersOnLine - 2) / parametersOnLine;

            switch (order % parametersOnLine)
            {
                case 2:
                    return $"Type{propIndex}: type of {propIndex}-th property";
                case 3:
                    return $"PropName{propIndex}: name of {propIndex}-th property";
                case 0:
                    return $"PropValue{propIndex}: value of {propIndex}-th property. You can also type {Utils.UndefinedValue} to let every concrete instance of this class define it.";
                case 1:
                    return $"Hint{propIndex}: a short explanation for {propIndex}-th property";
                default:
                    throw new Exception("Get Hint for Class failed.");
            }
        }

        public string GetSnippet()
        {
            string result = "";
            result += "CLASS\r\n"
                    + "\tClassId\r\n"
                    + "\tType1\tName1\tValue1\tHint1\r\n"
                    + "\tType2\tName2\tValue2\tHint2\r\n"
                    + "\t...";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if (codeWords.Count % parametersOnLine != 2)
                return $"Please follow this snippet: \n{GetSnippet()}";

            Properties = new List<PropertyInfo>();
            StringId = codeWords[1];

            for(int i=2; i<codeWords.Count; i+=parametersOnLine)
            {
                string encodedPropType = codeWords[i];
                string propName = codeWords[i+1];
                string encodedPropVal = codeWords[i+2];
                string propHint = codeWords[i+3];

                var proptype = Utils.ParseContextType(encodedPropType);

                PropertyInfo prop = new PropertyInfo(proptype, propName, encodedPropVal, propHint);
                nameDict.Add(propName, Properties.Count);

                if(encodedPropVal == Utils.UndefinedValue)
                    UndefinedPos.Add(Properties.Count);

                Properties.Add(prop);
            }

            return "";
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            if(nameDict.ContainsKey(Utils.SpecialProp_Preview))
            {
                var prop = GetProperty(Utils.SpecialProp_Preview); 
                var objId = prop.EncodedValue;

                var obj = AnimatingObjectsLib.Instance.Get(objId);

                if(obj == null)
                    return base.GetPreviewBitmap(time);

                return obj.GetPreviewBitmap();
            }

            return base.GetPreviewBitmap(time);
        }

        public ContextType GetDynamicContext(int order, List<string> codeWords)
        {
            // if the current order context is a prop value
            if(order != 0 && order % 4 == 0)
            {
                int propTypeIndex = order - 2; // 1 to PropName, 1 to PropType

                var encodedPropType = codeWords[propTypeIndex];
                return Utils.ParseContextType(encodedPropType);
            }

            return GetContext(order);
        }

        public string GetDynamicHint(int order, List<string> codeWords)
        {
            return GetHint(order);
        }
    }
}
