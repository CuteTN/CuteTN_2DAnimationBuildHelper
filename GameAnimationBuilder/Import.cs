using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    class Import : AnimatingObject, IScriptable, IAdditionalProperty
    {
        /*
            -GetContext
            -GetHint
            -GetSnippet
            -ParseData
            -AddToLib
            -GetPreviewBitmap
        */
        public string EncodedFilePath;
        public string ImportedCode;

        public ContextType GetContext(int order)
        {
            if(order == 0)
                return ContextType.Tag;

            if(order == 1)
                return ContextType.TextFilePath;

            // the code shouldn't reach here :)
            return ContextType.Unknown;
        }

        public string GetHint(int order)
        {
            if(order == 0)
                return $"IMPORT: Import existing code";

            if(order == 1)
                return $"FilePath: Should be text file.";

            // the code shouldn't reach here :)
            return "";
        }

        public string GetSnippet()
        {
            string result = "";
            result += "IMPORT\r\n" 
                    + "\tFilePath\r\n";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count != 2)
                return $"Please follow this snippet: \n{GetSnippet()}";

            if(!Utils.IsValidEncodedFilePath(codeWords[1]))
                return $"Invalid filepath {codeWords[1]}";

            EncodedFilePath = codeWords[1];

            string FilePath = Utils.DecodePathToWork(EncodedFilePath);

            try
            {

                StreamReader sr = new StreamReader(FilePath);
                ImportedCode = sr.ReadToEnd(); 

                sr.Close();
                sr.Dispose();
            }
            catch
            {
                return $"Code import failed {FilePath}";
            }

            StringId = $"IMPORT_{FilePath}";
            return "";
        }

        /// <summary>
        /// Import: add every data in imported code to library
        /// </summary>
        public override void AddToLib()
        {
            // don't re-import a file, otherwise will cause recursion!
            if(AnimatingObjectsLib.Instance.Get(StringId) != null)
                return;

            base.AddToLib();

            var scopes = ImportedCode.Split(Utils.EndScopeChar);

            foreach (var scope in scopes)
            {
                string error;
                var tempObj = ObjectAnimations.InterpretScope(scope, out error);    
                
                if(error == "" || error == null)
                { 
                    tempObj?.AddToLib();
                }
            }
        }
    }
}
