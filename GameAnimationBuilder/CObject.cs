using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class CObject : AnimatingObject, IDynamicContext
    {
        public string ClassId;

        public List<string> UndefinedPropsVal;

        /// <summary>
        /// Shallow copied from class
        /// </summary>
        public List<PropertyInfo> Properties;

        private void GenerateCompleteProps()
        {
            var cClass = AnimatingObjectsLib.Instance.Get(ClassId) as CClass;
            if(cClass == null)
                return;

            int cnt = 0;
            Properties = new List<PropertyInfo>();

            foreach(var prop in cClass.Properties)
            {
                var temp = prop.Clone();

                if(temp.EncodedValue == Utils.UndefinedValue)
                { 
                    temp.EncodedValue = UndefinedPropsVal[cnt];
                    cnt++;
                }

                Properties.Add(temp);    
            }
        }

        public PropertyInfo GetProperty(string propName)
        {
            var cClass = AnimatingObjectsLib.Instance.Get(ClassId) as CClass;
            if(cClass == null)
                return null;

            int index = cClass.GetPropertyIndex(propName);
            return GetProperty(index);
        }

        public PropertyInfo GetProperty(int index)
        {
            return Properties[index];
        }

        public void SetProperty(string propName, string newValue)
        {
            var prop = GetProperty(propName);
            prop.EncodedValue = newValue;
        }

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.Id;

            if(order == 2)
                return ContextType.Class;

            return ContextType.Unknown;
        }

        public ContextType GetDynamicContext(int order, List<string> code)
        {
            if(order <= 2)
                return GetContext(order);

            CClass tempBClass = AnimatingObjectsLib.Instance.Get(code[2]) as CClass;

            if(tempBClass == null)
                return ContextType.Unknown;

            PropertyInfo prop = null;
            try { prop = tempBClass.GetUndefinedProperty(order - 3); }
            catch { };

            if(prop == null)
                return ContextType.Unknown;

            return prop.Type;
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"OBJECT: Include concrete object values of a class";

            if(order == 1)
                return $"ObjectId: Should be OBJECT_CLASSID_No";

            if(order == 2)
                return $"ClassId: A class from which this object inherits";

            int undefPropIndex = order - 2; 
            return $"PropertyValue{undefPropIndex}: Set concrete value of {undefPropIndex}-th undefined property for this object";
        }

        public string GetDynamicHint(int order, List<string> code)
        {
            if (order <= 2)
                return GetHint(order);

            CClass tempBClass = AnimatingObjectsLib.Instance.Get(code[2]) as CClass;

            if (tempBClass == null)
                return GetHint(order);

            PropertyInfo prop = null;
            try { prop = tempBClass.GetUndefinedProperty(order - 3); }
            catch { };

            if (prop == null)
                return GetHint(order);

            string hint = prop.Name + ": " + Utils.DecodeStringToRaw(prop.Hint);
            return hint;
        }

        public string GetSnippet()
        {
            string result = "";
            result += "OBJECT\r\n"
                    + "\tObjectId\r\n"
                    + "\tClassId\r\n"
                    + "\tProperty1\r\n"
                    + "\tProperty2\r\n"
                    + "\t...";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count < 3)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];

            ClassId = codeWords[2];
            var cClass = AnimatingObjectsLib.Instance.Get(ClassId) as CClass;

            if(cClass == null)
                return $"Class Id {ClassId} is not found";

            if(codeWords.Count - 3 != cClass.GetUndefinedPropertyCount())
                return $"Number of undefined property does not match";

            // no checking properties value yet...
            UndefinedPropsVal = new List<string>();
            for(int i=0; i<cClass.GetUndefinedPropertyCount(); i++)
                UndefinedPropsVal.Add(codeWords[i+3]);

            GenerateCompleteProps();

            return "";
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            PropertyInfo prop = null;
            try
            {
                prop = GetProperty(Utils.SpecialProp_Preview);
                if(prop == null)
                    throw new Exception();
            }
            catch
            {
                return base.GetPreviewBitmap(time);
            }

            var objId = prop.EncodedValue;

            var obj = AnimatingObjectsLib.Instance.Get(objId);

            if (obj == null)
                return base.GetPreviewBitmap(time);

            return obj.GetPreviewBitmap();
        }
    }
}
