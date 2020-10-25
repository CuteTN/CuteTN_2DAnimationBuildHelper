using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
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

        public string RawFileName()
        {
            return Path.GetFileName(Utils.DecodePathToRaw(EncodedFilePath));
        }

        #region preview
        /// <summary>
        /// <para>Total file to import in this single import tag including the file in parameter and recursive ones.</para>
        /// <para>However this number can also be 0 if the file in the parameter is already added previously</para> 
        /// </summary>
        public int CountTotalFilesToImport = 0;
        public List<Import> RecurImports = null;

        private static int imageWidth = 400;
        private static Point rootOffset = new Point(10, 0);
        private static Point firstChildOffset = new Point(30, 30);
        private static Point rootIndentPathOffset = new Point(5, 15);
        private static Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);
        private static Brush brush = new SolidBrush(Color.Black);
        public Bitmap previewTreeBitmap = null;
        #endregion

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

            StringId = $"{EncodedFilePath}";
            return "";
        }

        /// <summary>
        /// Import: add every data in imported code to library
        /// </summary>
        public override void AddToLib()
        {
            // don't re-import a file, otherwise will cause infinite recursion!
            if(AnimatingObjectsLib.Instance.Get(StringId) != null)
                return;

            base.AddToLib();

            CountTotalFilesToImport = 1;
            RecurImports = new List<Import>();

            var scopes = ImportedCode.Split(Utils.EndScopeChar);

            foreach (var scope in scopes)
            {
                string error;
                var tempObj = ObjectAnimations.InterpretScope(scope, out error);    
                
                if(error == "" || error == null)
                { 
                    // if this object has not been added before
                    if(tempObj!=null)
                    { 
                        bool notAddedBefore = AnimatingObjectsLib.Instance.Get(tempObj.StringId) == null;
                        tempObj.AddToLib();
                        
                        // this is save for generating preview
                        if(tempObj is Import && notAddedBefore)
                        { 
                            RecurImports.Add(tempObj as Import);
                            CountTotalFilesToImport += (tempObj as Import).CountTotalFilesToImport; 
                        }
                    }
                }
            }
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            if(previewTreeBitmap != null)
                return previewTreeBitmap;

            if(CountTotalFilesToImport == 0)
                return base.GetPreviewBitmap(time);

            Point offset = firstChildOffset;
            Point indentPathOffset = new Point(rootIndentPathOffset.X + firstChildOffset.X, rootIndentPathOffset.Y + firstChildOffset.Y);

            Pen linePen = new Pen(brush, 4);
            linePen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;

            Bitmap result = new Bitmap(imageWidth, offset.Y * CountTotalFilesToImport);
            Graphics resG = Graphics.FromImage(result);

            resG.DrawString(RawFileName(), font, brush, rootOffset);
            
            foreach(var imp in RecurImports)
            {
                resG.DrawImage(imp.GetPreviewBitmap(), offset);

                // Drawing indent path, for fun :^) I'm bored
                resG.DrawLine(linePen, rootIndentPathOffset.X, rootIndentPathOffset.Y, rootIndentPathOffset.X, indentPathOffset.Y);
                resG.DrawLine(linePen, indentPathOffset.X, indentPathOffset.Y, rootIndentPathOffset.X, indentPathOffset.Y);

                offset = new Point(offset.X, offset.Y + firstChildOffset.Y * imp.CountTotalFilesToImport);
                indentPathOffset = new Point(indentPathOffset.X, indentPathOffset.Y + firstChildOffset.Y * imp.CountTotalFilesToImport);
            }

            previewTreeBitmap = result;
            return result;
        }
    }
}
