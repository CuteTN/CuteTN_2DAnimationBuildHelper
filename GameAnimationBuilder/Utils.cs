using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    static class Utils
    {
        static public string WorkingDir = null;

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

        static public bool IsValidEncodedFilePath(string path)
        {
            if(path.Length < 2)
                return false;

            if(path[0] != '\"')
                return false;

            if(path[path.Length - 1] != '\"')
                return false;

            return true;
        }

        static public string EncodePath(string rawPath)
        {
            if(rawPath == "" || rawPath == null)
                rawPath = Application.StartupPath;

            // Replace white space with encode string
            string result = rawPath.Replace(" ", Utils.AlternativeSpaceInPath);

            // Add quotes
            result = $"\"{result}\"";

            return result;
        }

        static public string DecodePathToRaw(string encodedPath)
        {
            if(!IsValidEncodedFilePath(encodedPath))
                throw new Exception("Invalid encoded file path!");

            string result = encodedPath.Replace(Utils.AlternativeSpaceInPath, " ");
            return result.Substring(1, result.Length - 2);
        }

        static public string DecodePathToWork(string encodedPath)
        {
            string result = DecodePathToRaw(encodedPath);

            if(result[0] == '.')
                result = WorkingDir + result.Substring(1, result.Length - 1);

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
        #region Caret Position
        ///for System.Windows.Forms.TextBox
        public static System.Drawing.Point GetCaretPoint(System.Windows.Forms.TextBox textBox)
        {
            int start = textBox.SelectionStart;
            if (start == textBox.TextLength)
                start--;

            return textBox.GetPositionFromCharIndex(start);
        }

        ///for System.Windows.Controls.TextBox requires reference to the following dlls: PresentationFramework, PresentationCore & WindowBase.
        //So if not using those dlls you should comment the code below:

        /*public static System.Windows.Point GetCaretPoint(System.Windows.Controls.TextBox textBox)
        {
            return textBox.GetRectFromCharacterIndex(textBox.SelectionStart).Location;
        }*/

        #endregion
    }
}
