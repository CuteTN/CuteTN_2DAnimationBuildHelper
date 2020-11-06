using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace GameAnimationBuilder
{
    public partial class SectionDesigner : Form
    {
        // the goal is to make this form a separated module, so I'm not gonna use singleton here
        Dictionary<string, AnimatingObject> AnimatingObjects = null;
        Section Section;
        Bitmap PreviewBitmap;

        #region init
        /// <summary>
        /// return null if the library of animating objects is not there yet...
        /// </summary>
        public CClass SelectedClass
        {
            get 
            {
                if(AnimatingObjects == null || AnimatingObjects.Count == 0)
                {
                    return null;
                }

                CClass result = AnimatingObjects[comboBox_Class.Text] as CClass;
                return result;
            }
        }

        /// <summary>
        /// return null if there isn't exactly one selected object
        /// </summary>
        public CObject SelectedObject
        {
            get
            {
                if(dataGridView_Objects.SelectedRows.Count != 1)
                    return null;

                string id = dataGridView_Objects.SelectedCells[0].Value as string;
                if(id!=null && AnimatingObjects.ContainsKey(id))
                {
                    // CuteTN note: it must be a CObject or not defined id here
                    return AnimatingObjects[id] as CObject;
                }

                return null;
            }
        }

        private SectionDesigner()
        {
            InitializeComponent();
        }

        public SectionDesigner(Section section, Dictionary<string, AnimatingObject> animObjs) : this()
        {
            Section = section;
            AnimatingObjects = new Dictionary<string, AnimatingObject>();

            // warning: shallow copy
            foreach(var i in animObjs)
                AnimatingObjects.Add(i.Key, i.Value);

            LoadClassesToCombobox();
            UpdatePreviewPictureBox();
        }

        private void LoadClassesToCombobox()
        {
            foreach(var i in AnimatingObjects)
                if(i.Value is CClass)
                    comboBox_Class.Items.Add(i.Key);

            if(comboBox_Class.Items.Count == 0)
            {
                MessageBox.Show("You should have at least 1 class in your data to design a section!");
                Close();
                return;
            }

            comboBox_Class.SelectedIndex = 0;
        }
        #endregion

        #region LoadObjectsOfSelectedClass
        private void LoadObjectsOfSelectedClass()
        {
            dataGridView_Objects.Rows.Clear();

            foreach (var i in AnimatingObjects)
                if (i.Value is CObject)
                { 
                    var obj = i.Value as CObject;

                    if(obj.ClassId == SelectedClass.StringId)
                        dataGridView_Objects.Rows.Add(obj.StringId);
                }
        }
        #endregion

        #region update properties
        private string CommonPropertyValue(string propName)
        {
            if(dataGridView_Objects.SelectedRows.Count == 0)
                return null;

            string objId = dataGridView_Objects.SelectedRows[0].Cells[0].Value as string;
            CObject obj = AnimatingObjects[objId] as CObject;

            string result = obj.GetProperty(propName).EncodedValue;

            foreach(DataGridViewRow row in dataGridView_Objects.SelectedRows)
            {
                using(var cell = row.Cells[0])
                {
                    objId = cell.Value as string;
                    obj = AnimatingObjects[objId] as CObject;

                    string value = obj.GetProperty(propName).EncodedValue;
                    if(value != result)
                        return null;
                }
            }

            return result;
        }
        private void LoadPropertiesOfObject()
        {
            if(SelectedClass == null)
                return;

            var propNames = SelectedClass.GetUndefinedPropertiesNames();

            dataGridView_Properties.Rows.Clear();

            foreach(var propName in propNames)
            {
                try
                { 
                    if(dataGridView_Objects.SelectedRows.Count == 1)
                    { 
                        dataGridView_Properties.Rows.Add(propName, SelectedObject.GetProperty(propName).EncodedValue);
                    }
                    else
                    {
                        // CuteTN Note: buggy buddy
                        string commonVal = CommonPropertyValue(propName);
                        if(commonVal == null)
                            commonVal = Utils.UndefinedValue; 
                        dataGridView_Properties.Rows.Add(propName, commonVal);
                    }
                }
                catch
                {
                    MessageBox.Show("Something's gone wrong in LoadPropertiesOfObject!");
                }
            }
        }
        private void dataGridView_Objects_SelectionChanged(object sender, EventArgs e)
        {
            LoadPropertiesOfObject();
            UpdatePreviewPictureBox();
        }
        #endregion

        #region Updated selected class
        private void comboBox_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadObjectsOfSelectedClass();
            LoadPropertiesOfObject();
        }
        #endregion


        #region Update value to data
        private void EditChangedValue()
        {
            foreach(DataGridViewRow propRow in dataGridView_Properties.Rows)
            {
                string propName = propRow.Cells[0].Value as string;
                string propVal = propRow.Cells[1].Value as string;

                if(propVal == Utils.UndefinedValue)
                    continue;

                foreach(DataGridViewRow objRow in dataGridView_Objects.SelectedRows)
                {
                    string objId = objRow.Cells[0].Value as string;

                    var obj = AnimatingObjects[objId] as CObject;
                    obj.SetProperty(propName, propVal);
                }
            }
        }

        /// <summary>
        /// Evaluate mathematical expression
        /// </summary>
        /// <returns>true if the is at least one expression is evaluated</returns>
        private bool EvaluateIntPropertyCells(int rowIndex, int colIndex)
        {
            bool result = false;

            if(colIndex < 1)
                return result;

            // dont know why but this fixes a bug :)
            if(rowIndex < 0)
                return result;

            DataGridViewCell cellPropVal = dataGridView_Properties.Rows[rowIndex].Cells[colIndex]; 
            DataGridViewCell cellPropName = dataGridView_Properties.Rows[rowIndex].Cells[0]; 

            try
            { 
                string propName = cellPropName.Value as string;
                string propOldVal = cellPropVal.Value as string;

                var prop = SelectedClass.GetProperty(propName);
                if(prop.Type != ContextType.Int)
                    return false;

                DataTable dt = new DataTable();
                var propNewVal = ((int)dt.Compute(propOldVal, "")).ToString();

                if(propOldVal != propNewVal)
                    result = true;
                else
                    return false;

                dataGridView_Properties.Rows[rowIndex].Cells[colIndex].Value = propNewVal;
            }
            catch { }

            return result;
        }

        private void dataGridView_Properties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool sthEvaluated = EvaluateIntPropertyCells(e.RowIndex, e.ColumnIndex);
            if(sthEvaluated)
                return;

            EditChangedValue();
            UpdatePreviewPictureBox();
        }
        #endregion

        #region Add new Game object

        /// <summary>
        /// CuteTN note: VERY DIRTY CODE HERE, couple with the creation of CObject
        /// Cant fix because the initializing data was hardcoded in the ParseData function :)
        /// </summary>
        private List<string> GenerateCodeAddNewObject(string id)
        {
            var result = new List<string>();

            result.Add("OBJECT");
            result.Add(id);
            result.Add(SelectedClass.StringId);

            int unknownValsCount = SelectedClass.GetUndefinedPropertyCount();
            for(int i=0; i<unknownValsCount; i++)
                result.Add(Utils.UndefinedValue);

            return result;
        }

        private string CombinePrefixNumber(string prefix, int number)
        {
            return $"{prefix}_{number}";
        }

        private int AutoGenerateIdNumber(string prefix, Func<string, int, string> toStringFunc)
        {
            for(int i=0; ; i++)
            {
                var temp = toStringFunc(prefix, i);
                if(!AnimatingObjects.ContainsKey(temp))
                    return i;
            }
        }

        private void AddNewObject(int? idNumber = null)
        {
            int idNum = 0; 
            string classId = SelectedClass.StringId;

            if (idNumber == null)
                idNum = AutoGenerateIdNumber(classId, CombinePrefixNumber);
            else
                idNum = idNumber.Value;

            string id = CombinePrefixNumber(classId, idNum);

            ///// Add to data
            var codewords = GenerateCodeAddNewObject(id);
            var obj = new CObject();
            obj.ParseData(codewords);
            
            try { obj.SetProperty(Utils.SpecialProp_Section, Section.StringId); }
            catch { }

            try { obj.SetProperty(Utils.SpecialProp_X, "0"); }
            catch { }

            try { obj.SetProperty(Utils.SpecialProp_Y, "0"); }
            catch { }

            try { obj.SetProperty(Utils.SpecialProp_Width, "32"); }
            catch { }

            try { obj.SetProperty(Utils.SpecialProp_Height, "32"); }
            catch { }

            AnimatingObjects.Add(id, obj);

            LoadObjectsOfSelectedClass();
            LoadPropertiesOfObject();
            UpdatePreviewPictureBox();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            AddNewObject();
        }
        #endregion

        #region Delete game object
        private void DeleteSelectedObjects()
        {
            List<string> toDelId = new List<string>();
            List<int> toDelIndex = new List<int>();

            foreach(DataGridViewRow rowObj in dataGridView_Objects.SelectedRows)
            {
                toDelId.Add(rowObj.Cells[0].Value as string);
                toDelIndex.Add(rowObj.Index);
            }

            // remove from datagridview
            // remember to delete them backward...
            toDelIndex.Sort((a, b) => b.CompareTo(a));
            foreach (int index in toDelIndex)
            {
                if(index >= 0)
                {
                    MessageBox.Show(index.ToString());
                    dataGridView_Objects.Rows.RemoveAt(index);
                }
            }

            // remove from data
            foreach (string id in toDelId)
            {
                AnimatingObjects.Remove(id);
            }
        }
        
        private void button_Delete_Click(object sender, EventArgs e)
        {
            var dlgRes = MessageBox.Show(null, "Are you sure? (you can't undo this command)", "Delete selected objects", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(dlgRes == DialogResult.No)
                return;

            DeleteSelectedObjects();
            
            LoadPropertiesOfObject();
            UpdatePreviewPictureBox();
        }
        #endregion

        #region update rendering
        /// <summary>
        /// return true if draw successfully
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool DrawVisibleObject(CObject obj, Graphics g)
        {
            int x, y;
            Bitmap preview;

            // don't draw object of another section
            try
            { 
                if(obj.GetProperty(Utils.SpecialProp_Section).EncodedValue != Section.StringId)
                    return false;
            }
            catch
            {
                return false;
            }

            try { x = int.Parse(obj.GetProperty(Utils.SpecialProp_X).EncodedValue); }
            catch { return false;}

            try { y = int.Parse(obj.GetProperty(Utils.SpecialProp_Y).EncodedValue); }
            catch { return false;}

            try { 
                var prop = obj.GetProperty(Utils.SpecialProp_Preview);
                preview = AnimatingObjects[prop.EncodedValue].GetPreviewBitmap();
            }
            catch { return false;}

            g.DrawImage(preview, x, y);

            return true;
        }

        /// <summary>
        /// Invisible objects only have x, y, width, heightr simply add a rectangle
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool DrawInvisibleObject(CObject obj, Graphics g)
        {
            int x, y, width, height;

            // don't draw object of another section
            try
            {
                if (obj.GetProperty(Utils.SpecialProp_Section).EncodedValue != Section.StringId)
                    return false;
            }
            catch
            {
                return false;
            }

            try { x = int.Parse(obj.GetProperty(Utils.SpecialProp_X).EncodedValue); }
            catch { return false; }

            try { y = int.Parse(obj.GetProperty(Utils.SpecialProp_Y).EncodedValue); }
            catch { return false; }

            try { width = int.Parse(obj.GetProperty(Utils.SpecialProp_Width).EncodedValue); }
            catch { return false; }

            try { height = int.Parse(obj.GetProperty(Utils.SpecialProp_Height).EncodedValue); }
            catch { return false; }

            Pen pen = new Pen(Color.Beige, 1);
            Brush brush = new SolidBrush(Color.FromArgb(50, Color.Beige));

            g.FillRectangle(brush, x, y, width, height);
            g.DrawRectangle(pen, x, y, width, height);

            return true;
        }

        private bool HighlightVisibleObject(CObject obj, Graphics g)
        {
            int x,y;
            Bitmap preview;

            try { x = int.Parse(obj.GetProperty(Utils.SpecialProp_X).EncodedValue); }
            catch { return false;}

            try { y = int.Parse(obj.GetProperty(Utils.SpecialProp_Y).EncodedValue); }
            catch { return false;}

            try { 
                var prop = obj.GetProperty(Utils.SpecialProp_Preview);
                preview = AnimatingObjects[prop.EncodedValue].GetPreviewBitmap();
            }
            catch { return false;}

            Pen highlightPen = new Pen(Color.HotPink, 3);
            g.DrawRectangle(highlightPen, x, y, preview.Width, preview.Height);

            return true;
        }

        private bool HighlightInvisibleObject(CObject obj, Graphics g)
        {
            int x, y, width, height;

            // don't draw object of another section
            try
            {
                if (obj.GetProperty(Utils.SpecialProp_Section).EncodedValue != Section.StringId)
                    return false;
            }
            catch
            {
                return false;
            }

            try { x = int.Parse(obj.GetProperty(Utils.SpecialProp_X).EncodedValue); }
            catch { return false; }

            try { y = int.Parse(obj.GetProperty(Utils.SpecialProp_Y).EncodedValue); }
            catch { return false; }

            try { width = int.Parse(obj.GetProperty(Utils.SpecialProp_Width).EncodedValue); }
            catch { return false; }

            try { height = int.Parse(obj.GetProperty(Utils.SpecialProp_Height).EncodedValue); }
            catch { return false; }

            Pen highlightPen = new Pen(Color.HotPink, 3);
            g.DrawRectangle(highlightPen, x, y, width, height);

            return true;
        }

        private bool HighlightSelectedObjects(Graphics g)
        {
            foreach(DataGridViewRow rowObj in dataGridView_Objects.SelectedRows)
            {
                var objId = rowObj.Cells[0].Value as string;
                var obj = AnimatingObjects[objId] as CObject;

                HighlightVisibleObject(obj, g);
                HighlightInvisibleObject(obj, g);
            }

            return true;
        }

        private void UpdatePreviewPictureBox()
        {
            if(AnimatingObjects == null)
                return;

            Texture background = AnimatingObjects[Section.TextureId] as Texture;
            PreviewBitmap = new Bitmap(background.Bitmap);

            Graphics g = Graphics.FromImage(PreviewBitmap);

            // draw every game object
            foreach(DataGridViewRow objRow in dataGridView_Objects.Rows)
            {
                string objId = objRow.Cells[0].Value as string;
                CObject obj;
                try
                {
                    obj = AnimatingObjects[objId] as CObject;
                }
                catch
                {
                    continue;
                }

                DrawVisibleObject(obj, g);
                DrawInvisibleObject(obj, g);
            }

            // highlight selected ones
            HighlightSelectedObjects(g);

            pictureBox_SectionPreview.Image = PreviewBitmap;
        }

        private void SectionDesigner_Paint(object sender, PaintEventArgs e)
        {
        }
        #endregion

        #region Generate code
        private string GenerateCode()
        {
            string result = "";
            result += "// This code was auto generated by Game Build Helper tool.\r\n"
                    + "// Please import this into your data file with IMPORT tag.\r\n"
                    + ";\r\n\r\n";
                   

            foreach(var pair in AnimatingObjects)
            {
                if(pair.Value is CObject)
                {
                    string scope = $"OBJECT\r\n";

                    CObject obj = pair.Value as CObject;
                    CClass cclass = AnimatingObjects[obj.ClassId] as CClass;

                    scope += $"\t{obj.StringId}\r\n";
                    scope += $"\t{obj.ClassId}\r\n";
                    
                    List<string> propNames = cclass.GetUndefinedPropertiesNames();
                    foreach(var propName in propNames)
                    {
                        scope += $"\t{obj.GetProperty(propName).EncodedValue}\r\n";
                    }

                    scope += ";\r\n";

                    result += scope + "\r\n";
                }
            }

            return result;
        }

        #endregion

        #region Save code
        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            var dlgRes = dlg.ShowDialog();

            if(dlgRes == DialogResult.Cancel)
                return;

            StreamWriter sr = new StreamWriter(dlg.FileName);
            sr.Write(GenerateCode());
            sr.Close();
            sr.Dispose();
        }
        #endregion
    }
}
