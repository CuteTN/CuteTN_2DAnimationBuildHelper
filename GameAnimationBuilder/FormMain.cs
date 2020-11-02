using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    public partial class FormMain : Form
    {
        #region Init
        public int TimeElapsed = 0;

        private string _lastOpenedFilePath = null;
        public string LastOpenedFilePath
        {
            get => _lastOpenedFilePath;
            set
            {
                _lastOpenedFilePath = value;
                this.Text = $"{Utils.MainTitle} - {Path.GetFileName(_lastOpenedFilePath)}";
            }

        }


        private AnimatingObject _selectedObj;
        public AnimatingObject SelectedObj
        {
            get => _selectedObj;
            set { _selectedObj = value; }
        }

        private Timer timer;
        public FormMain()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            textBox_Code.AcceptsTab = true;

            // working directory
            textBox_WorkingDir.Text = Application.StartupPath;

            // timer
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Timer_Tick;
            timer.Start();

            // preview pricture box
            pictureBox_Preview.SizeMode = PictureBoxSizeMode.Zoom;

            // form close
            FormClosing += FormMain_FormClosing;
        }
        #endregion

        #region timer update
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeElapsed += timer.Interval;
            if(TimeElapsed > 2e9)
                TimeElapsed = 0;

            MyUpdate();
        }

        private void MyUpdate()
        {
            if (TimeElapsed % 100 == 0)
            {
                SaveBackUp();
                RefreshSuggestionsList();
            }

            if(AutoView && !IsSuggestionsVisible)
                UpdateCurrentSelectionPreview();

            UpdateCurrentHint();
            Invalidate();
        }
        #endregion

        #region Code Editor
        public string Code
        {
            get => textBox_Code.Text;
            set{ textBox_Code.Text = value; }
        }

        public void UpperCaseCurrentWord()
        {
            int index = textBox_Code.SelectionStart - 1;
            int startPos, endPos;
            string word = Utils.GetWordAt(Code, index, out startPos, out endPos).ToUpper();
            Utils.ReplaceCurrentTextBoxWord(textBox_Code, word);
        }

        private bool _codeModified;
        private void textBox_Code_TextChanged(object sender, EventArgs e)
        {
            // cannot put BackUp here since it would fck up IO, dedlock or sth idk
            _codeModified = true;
        }
        #endregion

        #region buttons
        private DialogResult RemindSaving()
        {
            if(!_codeModified || Code == "")
                return DialogResult.No;

            var dlgRes = MessageBox.Show(null, "Do you want to save your current code?", "Save change?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if(dlgRes == DialogResult.Yes)
            {
                button_Save_Click(null, null);
            }

            return dlgRes;
        }

        private void InterpretScope(string scope, bool showError = true)
        {
            var words = new List<string>(scope.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries));
            string error = "";
            var tempObj = AnimatingObject.InterpretScope(words, out error);

            if (error == "" || error == null)
            {
                SelectedObj = tempObj;
                SelectedObj?.AddToLib();
            }
            else
                if(showError)
                    MessageBox.Show(null, error, $"Cannot create {words[0]}: {words[1]}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trimAfterSelection">When true, the scope would be count from the begining of the word to the current SelectionStart</param>
        /// <param name="showError">Message box to show the error</param>
        private void InterpretCurrentScope(bool trimAfterSelection = false, bool showError = true)
        {
            int index = textBox_Code.SelectionStart;
            if(index > 0)
                index--;
            int startPos, endPos;

            var scope = Utils.GetScopeAt(Code, index, out startPos, out endPos);
            if(trimAfterSelection)
                scope = scope.Substring(0, index - startPos + 1);

            InterpretScope(scope, showError);
        }

        private void UpdateCurrentSelectionPreview()
        {
            string selectedCode = textBox_Code.SelectedText;
            
            // if there are more than 1 word in selected code
            if (selectedCode.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries).Length > 1)
            {
                return;
            }

            int startPos, endPos;
            selectedCode = Utils.GetWordAt(Code, textBox_Code.SelectionStart - 1, out startPos, out endPos);

            var words = new List<string>(selectedCode.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries));

            if (words.Count == 1)
            {
                AnimatingObject obj = AnimatingObjectsLib.Instance.Get(words[0]);
                SelectedObj = obj;
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            var dlgRes = RemindSaving();
            if(dlgRes == DialogResult.Cancel)
                return;

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlgRes = dlg.ShowDialog();

            if(dlgRes == DialogResult.OK)
            {
                button_Clear_Click(null, null);

                StreamReader sw = new StreamReader(dlg.FileName);
                Code = sw.ReadToEnd();
                sw.Close();
                sw.Dispose();

                LastOpenedFilePath = dlg.FileName;
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            
            if(LastOpenedFilePath != null)
            { 
                dlg.InitialDirectory = Path.GetDirectoryName(LastOpenedFilePath);
                dlg.FileName = Path.GetFileName(LastOpenedFilePath); 
            }

            var dlgRes = dlg.ShowDialog();

            if (dlgRes == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dlg.FileName);
                sw.Write(Code);
                sw.Close();
                sw.Dispose();

                LastOpenedFilePath = dlg.FileName;
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            string selectedCode = textBox_Code.SelectedText;
            if(selectedCode == "")
            {
                AnimatingObjectsLib.Instance.Clear();
                SelectedObj = null;
                selectedCode = Code;
            }

            var scopes = selectedCode.Split(Utils.EndScopeChar);
            
            foreach(var sc in scopes)
            { 
                InterpretScope(sc);
            }
        }

        private void button_View_Click(object sender, EventArgs e)
        {
            UpdateCurrentSelectionPreview();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            AnimatingObjectsLib.Instance.Clear();
            SelectedObj = null;
        }

        private void button_Export_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var dlgRes = dlg.ShowDialog();

            if(dlgRes == DialogResult.OK)
            {
                CppCodeGenerator codeGenerator = new CppCodeGenerator(AnimatingObjectsLib.Instance.GetAllItems());

                string inputData = codeGenerator.GenerateInput();
                string cppConsts = codeGenerator.GenerateCppConsts();

                StreamWriter sw;
                sw = new StreamWriter( $"{dlg.SelectedPath}\\{Utils.GeneratedInputFileName}" );
                sw.WriteLine(inputData);
                sw.Close();

                sw = new StreamWriter( $"{dlg.SelectedPath}\\{Utils.GeneratedCppConstsFileName}" );
                sw.WriteLine(cppConsts);
                sw.Close();

                sw.Dispose();
            }
        }

        private void button_ChangeWorkingDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var dlgRes = dlg.ShowDialog();

            if (dlgRes == DialogResult.OK)
            {
                textBox_WorkingDir.Text = dlg.SelectedPath;
            }
        }

        private void button_BackUp_Click(object sender, EventArgs e)
        {
            LoadBackUp();
        }

        private void button_Tricks_Click(object sender, EventArgs e)
        {
            string tricks = "";

            tricks += $"💡 In case a string has whitespace in it, replace it with {Utils.AlternativeSpaceInPath}\n";
            tricks += $"💡 If a whitespace in a string is detected, it will ask you to automatically encode it after closing quote\n";
            tricks += $"💡 Press ? to trigger auto complete!!!\n";
            tricks += $"💡 You can press [F_Number key on buttons] instead of clicking on them\n";
            tricks += $"💡 Press ^ to uppercase the current word\n";
            tricks += $"💡 Press Ctrl+F5 to interpret just the current scope/command\n";
            tricks += $"💡 Press Ctrl+Enter at suggested tag to apply the snippet\n";
            tricks += $"💡 Auto Add: when you press {Utils.EndScopeChar}, data will be added automatically\n";
            tricks += $"💡 If the program stops accidentally, you can still get your old code in the file {Utils.BackUpFileName}\n";
            tricks += $"💡 You can change the working directory to the game resource folder for more convenience\n";
            tricks += $"💡 Let the value of a property in a class be {Utils.UndefinedValue} to mark it as undefined\n";

            MessageBox.Show(tricks, "Cool tips and tricks");
        }
        #endregion

        #region rendering
        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            if (SelectedObj != null)
            {
                pictureBox_Preview.Image = SelectedObj.GetPreviewBitmap(TimeElapsed);
            }
            else
                pictureBox_Preview.Image = null;
        }
        #endregion

        #region Handle full control of keyboard :^)
        /// <summary>
        /// Override ProcessCmdKey to have full control of autocompletelistbox
        /// And catch keys down to control Undo Action
        /// </summary>
        /// <param name="m"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if(AutoAdd)
                if(keyData == Keys.OemSemicolon)
                {
                    InterpretCurrentScope(true, true);
                }

            // press ? - trigger auto complete
            if(keyData == (Keys.OemQuestion | Keys.Shift))
            {
                RefreshSuggestionsList();
                if(listBox_Suggestions.Items.Count != 0)
                { 
                    listBox_Suggestions.Visible = true;
                    listBox_Suggestions.SelectedIndex = 0;
                }

                // only works when current context is file path
                PeekImagePath();
                PeekTextPath();

                // only works when current context is id
                SetIdPrefix(); 

                return true;
            }

            // press " - check auto encode string
            if(keyData == (Keys.Oem7 | Keys.Shift))
            {
                if(textBox_Code.Focused)
                { 
                    HandleAutoEncodeString();

                    int tempSStart = textBox_Code.SelectionStart;
                    Code = Code.Insert(tempSStart, "\"");
                    textBox_Code.SelectionStart = tempSStart + 1;
                    return true;
                }
            }

            #region buttons shortcut
            if (keyData == (Keys.F5 | Keys.Control))
            {
                InterpretCurrentScope();
                return true;
            } 

            if (keyData == Keys.F5)
            {
                button_Add_Click(null, null);
                return true;
            }

            if (keyData == Keys.F6)
            {
                button_View_Click(null, null);
                return true;
            }

            if (keyData == Keys.F4)
            {
                button_Clear_Click(null, null);
                return true;
            }

            if (keyData == Keys.F2)
            {
                button_Save_Click(null, null);
                return true;
            }

            if (keyData == Keys.F3)
            {
                button_Load_Click(null, null);
                return true;
            }

            if (keyData == Keys.F1)
            {
                button_Tricks_Click(null, null);
                return true;
            }

            if (keyData == Keys.F7)
            {
                button_Export_Click(null, null);
                return true;
            }

            if (keyData == Keys.F8)
            {
                button_BackUp_Click(null, null);
                return true;
            }
            #endregion

            if(IsSuggestionsVisible)
            {
                bool temp = ProcessCmdKey_SuggestionsMode(ref m, keyData);
                if(temp)
                    return true;
            }
 
            if(keyData == (Keys.D6 | Keys.Shift))
            {
                UpperCaseCurrentWord();
                return true;
            }

            bool baseRes = base.ProcessCmdKey(ref m, keyData);
            return baseRes;
        }

        #endregion

        #region Auto completion :^)
        private ContextType GetCurrentContext(out string currentTag, out int orderInScope, out List<string> words)
        {
            orderInScope = 0;
            currentTag = "";
            words = new List<string>();

            // if the text is currently selected, skip
            if(textBox_Code.SelectedText != "")
                return ContextType.Unknown;

            int index = textBox_Code.SelectionStart - 1;
            int startScope, endScope;
            string currentScope = Utils.GetScopeAt(Code, index, out startScope, out endScope);

            // if the current scope is empty, returns TAG
            if(currentScope == "")
                return ContextType.Tag;

            // cut everything after index off, we don't care about them!
            currentScope = currentScope.Substring(0, index - startScope + 1);

            words = currentScope.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries).ToList();

            // tag simply is the first word in the scope
            currentTag = words.Count>0? words[0]:"";

            orderInScope = words.Count;

            // if the selection is in the middle of a word
            if(!Utils.isSeperator(currentScope.Last().ToString()))
                orderInScope--;

            if(orderInScope == 0)
                return ContextType.Tag;

            var obj = AnimatingObject.CreateTypeFromTag(currentTag);

            if(obj == null)
                return ContextType.Unknown;
            else
            {
                if(obj is IDynamicContext)
                    return (obj as IDynamicContext).GetDynamicContext(orderInScope, words.ToList());

                return (obj as IScriptable).GetContext(orderInScope);
            }
        }

        private ContextType GetCurrentContext(out string currentTag, out int orderInScope)
        {
            List<string> words;
            return GetCurrentContext(out currentTag, out orderInScope, out words);
        }

        private ContextType GetCurrentContext()
        {
            string tag; int order; List<string> words;
            return GetCurrentContext(out tag, out order, out words);
        }

        private List<string> GetSuggestionOnContext(ContextType context)
        {
            switch (context)
            {
                case ContextType.Tag:
                    return new List<string>(Utils.Tags);
                case ContextType.Bool:
                    return new List<string>(Utils.BoolValues);
                case ContextType.Texture:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<Texture>();
                case ContextType.Sprite:
                case ContextType.NewCollisionBoxId:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<Sprite>();
                case ContextType.Animation:
                case ContextType.NewStateId:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<Animation>();
                case ContextType.Class:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<CClass>();
                case ContextType.Object:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<CObject>();
                case ContextType.Section:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<Section>();
                case ContextType.Data:
                    return AnimatingObjectsLib.Instance.GetAllIdOfType<Object>();
                case ContextType.Type:
                    return Enum.GetNames(typeof(ContextType)).ToList();
                default:
                    // return empty list: Unknown, Int, FilePath, Id
                    return new List<string>();
            }
        }

        private void RefreshSuggestionsList()
        {
            var context = GetCurrentContext();

            var suggestions = GetSuggestionOnContext(context);
            SortSuggestions(ref suggestions);

            int temp = listBox_Suggestions.SelectedIndex;

            listBox_Suggestions.Items.Clear();
            listBox_Suggestions.Items.AddRange(suggestions.ToArray());

            if(listBox_Suggestions.Items.Count > temp)
                listBox_Suggestions.SelectedIndex = temp;

            if(listBox_Suggestions.Items.Count == 0)
                IsSuggestionsVisible = false;
        }

        private void SortSuggestions(ref List<string> suggestions)
        {
            int index = textBox_Code.SelectionStart - 1;
            int startPos, endPos;
            var currentWord = Utils.GetWordAt(Code, index, out startPos, out endPos);

            SmartAutoCompletionSorter.Sort(ref suggestions, currentWord);
        }

        private void ApplySuggestionSpecial()
        {
            var context = GetCurrentContext();

            string suggestion = listBox_Suggestions.SelectedItem as string;

            if(context == ContextType.NewStateId)
            {
                // suggestion would be an animation
                suggestion = $"<OBJECT_KIND>_STATE_<ACTION> {suggestion} False False 0\n";
                Utils.ReplaceCurrentTextBoxWord(textBox_Code, suggestion);
            }

            if(context == ContextType.NewCollisionBoxId)
            {
                // suggestion would be a sprite
                Sprite sprite = AnimatingObjectsLib.Instance.Get(suggestion) as Sprite;
                if(sprite != null)
                { 
                    string autoGeneratedId = $"COLBOX_{suggestion}";
                    string boxString = $"0 0 {sprite.Rectangle.Width-1} {sprite.Rectangle.Height-1}";

                    CollisionBox tempColBox = AnimatingObjectsLib.Instance.Get(autoGeneratedId) as CollisionBox;
                    if(tempColBox != null)
                    {
                        var tempBox = tempColBox.Box;
                        boxString = $"{tempBox.Left} {tempBox.Top} {tempBox.Right-1} {tempBox.Bottom-1}";
                    }

                    string toApply = $"{autoGeneratedId} {suggestion} {boxString}\n";
                    Utils.ReplaceCurrentTextBoxWord(textBox_Code, toApply);
                }
            }
        }

        private void ApplySuggestion()
        {
            string suggestion = listBox_Suggestions.SelectedItem as string;
            Utils.ReplaceCurrentTextBoxWord(textBox_Code, suggestion);

            // NewCollisionBoxId and NewStateId have special kind of suggestion
            // and some other...
            ApplySuggestionSpecial();
        }

        /// <summary>
        /// Only work for Context type = tag
        /// </summary>
        private void ApplySnippet()
        {
            var context = GetCurrentContext();
            if (context != ContextType.Tag)
                return;

            // suggestion must be a tag now
            string suggestion = listBox_Suggestions.SelectedItem as string;

            var obj = AnimatingObject.CreateTypeFromTag(suggestion);
            string snippet = (obj as IScriptable)?.GetSnippet();

            Utils.ReplaceCurrentTextBoxWord(textBox_Code, snippet);
        }

        public bool IsSuggestionsVisible
        {
            get => listBox_Suggestions.Visible;
            set { listBox_Suggestions.Visible = value; }
        }

        private bool ProcessCmdKey_SuggestionsMode(ref Message m, Keys keyData)
        {
            if(!IsSuggestionsVisible)
                return false;

            if(keyData == Keys.Down)
            { 
                if(listBox_Suggestions.SelectedIndex < listBox_Suggestions.Items.Count - 1)
                    listBox_Suggestions.SelectedIndex++;
                return true;
            }

            if(keyData == Keys.Up)
            {
                if(listBox_Suggestions.SelectedIndex > 0)
                    listBox_Suggestions.SelectedIndex--;
                return true;
            }

            if(keyData == Keys.Escape || keyData == Keys.Left || keyData == Keys.Right)
            {
                IsSuggestionsVisible = false;
                return true;
            }

            if(keyData == (Keys.Enter | Keys.Control))
            {
                var context = GetCurrentContext();

                if (context == ContextType.Tag)
                    ApplySnippet();
                else
                    ApplySuggestion();

                IsSuggestionsVisible = false;
            }

            if(keyData == Keys.Enter || keyData == Keys.Tab)
            {
                ApplySuggestion();
                IsSuggestionsVisible = false;
                return true;
            }

            // steal the control of Shift and Ctrl Keys
            if(keyData == (Keys.Shift | Keys.ShiftKey) || keyData == (Keys.Control | Keys.ControlKey))
                return true;

            return false;
        }

        /// <summary>
        /// Preview the selected item in the suggestions list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_Suggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = AnimatingObjectsLib.Instance.Get(listBox_Suggestions.SelectedItem as string);

            if(obj != null)
                SelectedObj = obj;
        }
        #endregion

        #region Special AutoFill
        /// <summary>
        /// Auto complete with context = FileName
        /// </summary>
        private void PeekPath(string filter)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter;
            var res = dlg.ShowDialog();

            if(res == DialogResult.OK)
            {
                string encodedPath = Utils.EncodeString(dlg.FileName);
                Utils.ReplaceCurrentTextBoxWord(textBox_Code, encodedPath);
            }
        }

        private void PeekImagePath()
        {
            var context = GetCurrentContext();
            if (context != ContextType.ImageFilePath)
                return;

            PeekPath("Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
        }

        private void PeekTextPath()
        {
            var context = GetCurrentContext();
            if (context != ContextType.TextFilePath)
                return;

            PeekPath("Text files(*.txt)|*.txt|All files (*.*)|*.*");
        }

        /// <summary>
        /// Auto complete with context = Id
        /// </summary>
        private void SetIdPrefix()
        {
            string tag; int order;
            var context = GetCurrentContext(out tag, out order);
            if (context != ContextType.Id)
                return;

            if (tag == "OBJECT_ANIMATIONS")
            { 
                int index = textBox_Code.SelectionStart;
                if(index > 0)
                    index--;

                Utils.ReplaceCurrentTextBoxWord(textBox_Code, "_ANIMATIONS");

                // move selection position to the beginning of the word
                int startPos, endPos;
                Utils.GetWordAt(Code, index, out startPos, out endPos);
                
                textBox_Code.SelectionStart = startPos;
            }
            else
                Utils.ReplaceCurrentTextBoxWord(textBox_Code, tag + "_");
        }

        #endregion

        #region hint
        private void UpdateCurrentHint()
        {
            if(Code == "")
            {
                textBox_Hint.Text = "To get started, click on \"Tricks\" button!";
                return;
            }    

            string tag; int order; List<string> words;
            var context = GetCurrentContext(out tag, out order, out words);

            var obj = AnimatingObject.CreateTypeFromTag(tag);

            // invalid tag
            if(obj == null)
            {
                // incomplete tag
                if (order == 0)
                {
                    textBox_Hint.Text = "Create a tag to add new data (press ? for the list of available tags)";
                    return;
                }

                textBox_Hint.Text = "Invalid tags would be ignored, you can use them as comments";
                return;
            }

            var hint = (obj as IScriptable)?.GetHint(order);

            if(obj is IDynamicContext)
                hint = (obj as IDynamicContext).GetDynamicHint(order, words);

            textBox_Hint.Text = hint;
        }

        #endregion

        #region auto
        public bool AutoAdd
        {
            get => checkBox_AutoAdd.Checked;
            set { checkBox_AutoAdd.Checked = value; }
        }
        // Auto add logic is in the function ProcessCmdKey

        public bool AutoView
        {
            get => checkBox_AutoView.Checked;
            set { checkBox_AutoView.Checked = value; }
        }
        // Auto view logic is in the function Timer_Tick

        #endregion

        #region Backup Code
        private void SaveBackUp()
        {
            // nothing to backup. Also, this prevents overwriting backup data since the previous run 
            if(Code == "" || Code == null)
                return;

            StreamWriter sw = new StreamWriter($"{Application.StartupPath}\\{Utils.BackUpFileName}");
            sw.WriteLine(Code);
            sw.Close();
            sw.Dispose();
        }

        private void LoadBackUp()
        {
            var dlgRes = RemindSaving();
            if(dlgRes == DialogResult.Cancel)
                return;

            StreamReader sr = new StreamReader($"{Application.StartupPath}\\{Utils.BackUpFileName}");
            Code = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
        }
        #endregion

        #region Working Dir
        private void textBox_WorkingDir_TextChanged(object sender, EventArgs e)
        {
            // Utils.WorkingDir = textBox_WorkingDir.Text;
            Directory.SetCurrentDirectory(textBox_WorkingDir.Text);
        }
        #endregion

        #region Handle Form close
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dlgRes = RemindSaving();

            // if user clicked on cancel on Reminding, don't close the window
            e.Cancel = dlgRes == DialogResult.Cancel;
        }
        #endregion

        #region Auto encode string

        /// <summary>
        /// index the index of a character that maybe in a string
        /// </summary>
        /// <param name="index"></param>
        private bool IsInInvalidString(string str, int index)
        {
            // index not in string
            if(!Utils.IsInString(str, index))
                return false; 

            int startPos, endPos;
            string temp = $"\"{Utils.GetStringAt(str, index, out startPos, out endPos)}\"";
            return !Utils.IsValidEncodedString(temp);
        }

        /// <summary>
        /// specifies what to do when user closed a string
        /// </summary>
        /// <param name="index"></param>
        private bool HandleAutoEncodeString()
        {
            int index = textBox_Code.SelectionStart - 1;
            if(index < 0)
                index++;

            if(!IsInInvalidString(Code, index))
                return false;

            var dlgRes = MessageBox.Show(null, "You've input a string with invalid format\nWould you like to fix it automatically", "Invalid string format", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if(dlgRes == DialogResult.No)
                return false;

            string temp1 = Code.Substring(0, index+1);
            int startPos, endPos;
            string temp = Utils.GetStringAt(temp1, index, out startPos, out endPos);
            
            temp = Utils.EncodeString(temp);
            temp = temp.Substring(1, temp.Length - 2);
            
            Code = Code.Remove(startPos, endPos - startPos + 1);
            Code = Code.Insert(startPos, temp);
            textBox_Code.SelectionStart = startPos + temp.Length;

            return true;
        }
        #endregion
    }
}
