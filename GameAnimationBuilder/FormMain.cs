using System;
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

            // timer
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Timer_Tick;
            timer.Start();

            // preview pricture box
            pictureBox_Preview.SizeMode = PictureBoxSizeMode.CenterImage;
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
            if (TimeElapsed % 10 == 0)
            {
                BackUp();
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
        #endregion

        #region buttons
        private void InterpretScope(string scope, bool showError = true)
        {
            var words = new List<string>(scope.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries));
            string error = "";
            var tempObj = AnimatingObject.Interpret(words, out error);

            if (error == "" || error == null)
            {
                SelectedObj = tempObj;
                SelectedObj?.AddToLib();
            }
            else
                if(showError)
                    MessageBox.Show(error, $"Cannot create {words[0]}: {words[1]}");
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
            if (selectedCode == "")
            {
                int startPos, endPos;
                selectedCode = Utils.GetWordAt(Code, textBox_Code.SelectionStart - 1, out startPos, out endPos);
            }
            var words = new List<string>(selectedCode.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries));

            if (words.Count == 1)
            {
                AnimatingObject obj = AnimatingObjectsLib.Instance.Get(words[0]);
                SelectedObj = obj;
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            var dlgRes = dlg.ShowDialog();

            if(dlgRes == DialogResult.OK)
            {
                StreamReader sw = new StreamReader(dlg.FileName);
                Code = sw.ReadToEnd();
                sw.Close();
                sw.Dispose();
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            var dlgRes = dlg.ShowDialog();

            if (dlgRes == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dlg.FileName);
                sw.Write(Code);
                sw.Close();
                sw.Dispose();
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

        private void button_Tricks_Click(object sender, EventArgs e)
        {
            string tricks = "";

            tricks += $"💡 In case your file path has whitespace in it, replace it with {Utils.AlternativeSpaceInPath}\n";
            tricks += $"💡 Press ? to trigger auto complete!!!\n";
            tricks += $"💡 You can press [F_Number key on buttons] instead of clicking on them\n";
            tricks += $"💡 Press ^ to uppercase the current word\n";
            tricks += $"💡 Press Ctrl+F5 to interpret just the current scope/command\n";
            tricks += $"💡 Press Ctrl+Enter at suggested tag to apply the snippet\n";
            tricks += $"💡 Auto Add: when you press {Utils.EndScopeChar}, data will be added automatically\n";
            tricks += $"💡 If the program stops accidentally, you can still get your old code in the file {Utils.BackUpFileName}";

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

                // only works when current context is id
                SetIdPrefix(); 

                return true;
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
        private ContextType GetCurrentContext(out string currentTag, out int orderInScope)
        {
            orderInScope = 0;
            currentTag = "";

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

            var words = currentScope.Split(Utils.WordSeperators, StringSplitOptions.RemoveEmptyEntries);

            // tag simply is the first word in the scope
            currentTag = words.Length>0? words[0]:"";

            orderInScope = words.Length;

            // if the selection is in the middle of a word
            if(!Utils.isSeperator(currentScope.Last().ToString()))
                orderInScope--;

            if(orderInScope == 0)
                return ContextType.Tag;

            var obj = AnimatingObject.CreateTypeFromTag(currentTag);

            if(obj == null)
                return ContextType.Unknown;
            else
                return (obj as IScriptable).GetContext(orderInScope);
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
                default:
                    // return empty list: Unknown, Int, FilePath, Id
                    return new List<string>();
            }
        }

        private void RefreshSuggestionsList()
        {
            string tag; int order;
            var context = GetCurrentContext(out tag, out order);

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
            string tag; int order;
            var context = GetCurrentContext(out tag, out order);

            string suggestion = listBox_Suggestions.SelectedItem as string;

            if(context == ContextType.NewStateId)
            {
                // suggestion would be an animation
                suggestion = $"<OBJECT_KIND>_STATE_<ACTION> {suggestion} False False 0";
                Utils.ReplaceCurrentTextBoxWord(textBox_Code, suggestion);
            }

            if(context == ContextType.NewCollisionBoxId)
            {
                // suggestion would be a sprite
                Sprite sprite = AnimatingObjectsLib.Instance.Get(suggestion) as Sprite;
                if(sprite != null)
                { 
                    suggestion = $"COLBOX_{suggestion} {suggestion} 0 0 {sprite.Rectangle.Width-1} {sprite.Rectangle.Height-1}";
                    Utils.ReplaceCurrentTextBoxWord(textBox_Code, suggestion);
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
            string tag; int order;
            var context = GetCurrentContext(out tag, out order);
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
                string tag; int order;
                var context = GetCurrentContext(out tag, out order);

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
        private void PeekImagePath()
        {
            string tag; int order;
            var context = GetCurrentContext(out tag, out order);
            if (context != ContextType.FileName)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            var res = dlg.ShowDialog();

            if(res == DialogResult.OK)
            {
                string encodedFilePath = "\"" + dlg.FileName.Replace(" ", Utils.AlternativeSpaceInPath) + "\"";
                Utils.ReplaceCurrentTextBoxWord(textBox_Code, encodedFilePath);
            }
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

            string tag; int order;
            var context = GetCurrentContext(out tag, out order);

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

        private void checkBox_AutoView_CheckedChanged(object sender, EventArgs e)
        {
            if(AutoView)
                MessageBox.Show(null, "Really buggy, I suggest turning Auto View off if not necessary :)", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Backup Code
        private void BackUp()
        {
            // nothing to backup. Also, this prevents overwriting backup data since the previous run 
            if(Code == "" || Code == null)
                return;

            StreamWriter sw = new StreamWriter($"{Application.StartupPath}\\{Utils.BackUpFileName}");
            sw.WriteLine(Code);
            sw.Close();
            sw.Dispose();
        }
        #endregion

 
    }
}
