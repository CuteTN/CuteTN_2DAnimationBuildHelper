using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    static class Utils
    {
        static public string MainTitle = "Animation Build Helper";

        static private string[] _wordSeperators = null;
        static public string[] WordSeperators
        {
            get
            {
                if(_wordSeperators == null)
                {
                    List<string> result = new List<string>();
                    result.Add(" ");
                    result.Add("\n");
                    result.Add("\r");
                    result.Add("\t");
                    result.Add(Environment.NewLine);

                    _wordSeperators = result.ToArray();
                }
                return _wordSeperators;
            }
        }

        public static readonly char EndScopeChar = ';';
        public static readonly string AlternativeSpaceInPath = "<>";
        public static readonly string UndefinedValue = "-";

        #region Special properties
        public static readonly string SpecialProp_Preview = "Preview";
        public static readonly string SpecialProp_Section = "Section";
        public static readonly string SpecialProp_X = "X";
        public static readonly string SpecialProp_Y = "Y";
        #endregion

        public static readonly string UnknownErrorMsg = "Unknown Error!";
        public static readonly string BackUpFileName = "ANIMATION_BACKUPFILE.txt";
        public static readonly string GeneratedInputFileName = "InputData.txt";
        public static readonly string GeneratedCppConstsFileName = "Consts.cpp";

        static private List<string> _tags = null;
        static public List<string> Tags
        {
            get
            {
                if(_tags == null)
                {
                    _tags = new List<string>();
                    _tags.Add("TEXTURE");
                    _tags.Add("ANIMATION");
                    _tags.Add("OBJECT_ANIMATIONS");
                    _tags.Add("COLLISION_BOX_GROUP");
                    _tags.Add("IMPORT");
                    _tags.Add("CLASS");
                    _tags.Add("OBJECT");
                    _tags.Add("SECTION");
                }

                return _tags;
            }
        }

        static private List<string> _boolValues;
        static public List<string> BoolValues
        {
            get
            {
                if(_boolValues == null)
                {
                    _boolValues = new List<string>();
                    _boolValues.Add(Boolean.TrueString);
                    _boolValues.Add(Boolean.FalseString);
                }
                return _boolValues;
            }
        }

        static public bool IsValidEncodedString(string str)
        {
            if(str.Length < 2)
                return false;

            if(str[0] != '\"')
                return false;

            if(str[str.Length - 1] != '\"')
                return false;

            if(str.Contains(" "))
                return false;

            // don't accept a stand-alone double prime
            string temp = str.Substring(1, str.Length - 2);
            while(temp.Contains("\"\""))
                temp = temp.Replace("\"\"", "");
            if(temp.Contains("\""))
                return false;

            return true;
        }

        static public string EncodeString(string rawStr, bool addQuote = true)
        {
            if(rawStr == "" || rawStr == null)
                rawStr = Application.StartupPath;

            // Replace white space with encode string
            string result = rawStr.Replace(" ", Utils.AlternativeSpaceInPath);

            // replace " by ""
            result.Replace("\"", "\"\"");

            // Add quotes
            if(addQuote)
                result = $"\"{result}\"";

            return result;
        }

        static public string DecodeStringToRaw(string encodedPath)
        {
            if(!IsValidEncodedString(encodedPath))
                throw new Exception("Invalid encoded file path!");

            string result = encodedPath.Replace(Utils.AlternativeSpaceInPath, " ");
            return result.Substring(1, result.Length - 2);
        }

        static public string DecodePathToWork(string encodedPath)
        {
            string result = DecodeStringToRaw(encodedPath);
            result = Path.GetFullPath(result);

            // well, let's hope Directory class does the job well...
            // -> Yes it does :)

            //if(result[0] == '.')
            //    result = WorkingDir + result.Substring(1, result.Length - 1);

            return result;
        }

        public static bool isSeperator(string s)
        {
            foreach(var e in WordSeperators)
            {
                if(e == s)
                    return true;
            }

            return false;
        }

        public static bool isEndScope(string s)
        {
            return s == EndScopeChar.ToString();
        }


        /// <summary>
        /// if this function is just to hard to understand, I suggest reading GetWordAt and GetScopeAt instead!
        /// </summary>
        /// <param name="para"></param>
        /// <param name="index"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="fenceFunction"></param>
        /// <returns></returns>
        public static string GetAtAfterSplit(string para, int index, out int startPos, out int endPos, Func<string,bool> fenceFunction)
        {
            string result = "";
            startPos = index + 1;
            endPos = index;

            if(index < 0 || index >= para.Length)
            {
                return "";
            }

            if (fenceFunction(para[index].ToString()))
            {
                return "";
            }

            startPos = endPos = index;

            while (startPos != 0 && !fenceFunction(para[startPos - 1].ToString()))
                startPos--;

            while (endPos != para.Length - 1 && !fenceFunction(para[endPos + 1].ToString()))
                endPos++;

            result = para.Substring(startPos, endPos - startPos + 1);
            return result;
        }

        public static string GetWordAt(string para, int index, out int startPos, out int endPos)
        {
            return GetAtAfterSplit(para, index, out startPos, out endPos, isSeperator);
        }

        public static string GetScopeAt(string para, int index, out int startPos, out int endPos)
        {
            return GetAtAfterSplit(para, index, out startPos, out endPos, isEndScope);
        }

        public static string GetStringAt(string para, int index, out int startPos, out int endPos)
        {
            return GetAtAfterSplit(para, index, out startPos, out endPos, (string str) => { return str == "\""; } );
        }

        public static string ReplaceRange(string para, int startPos, int endPos, string value)
        {
            string result = para;
            if (result == "")
            {
                return value;
            }

            if (startPos <= endPos)
            { 
                result = result.Remove(startPos, endPos - startPos + 1);
            }
            result = result.Insert(startPos, value);

            return result;
        }

        public static void ReplaceCurrentTextBoxWord(TextBox textBox, string value)
        {
            int index = textBox.SelectionStart - 1;
            if(index == -1)
                index++;

            int startPos, endPos;
            var currentWord = Utils.GetWordAt(textBox.Text, index, out startPos, out endPos);

            textBox.Text = ReplaceRange(textBox.Text, startPos, endPos, value);

            textBox.SelectionStart = startPos + value.Length;
            textBox.SelectionLength = 0;
        }

        static public bool IsInString(string str, int index)
        {
            // count double primes in prefix
            int countDP = Regex.Matches(str, "\"").Count;

            if (countDP % 2 == 1)
                return true;

            return false;
        }

        static public ContextType ParseContextType(string str)
        {
            ContextType result = ContextType.Unknown;
            try
            {
                result = (ContextType)Enum.Parse(typeof(ContextType), str);
            }
            catch { }

            return result;
        }
    }
}
