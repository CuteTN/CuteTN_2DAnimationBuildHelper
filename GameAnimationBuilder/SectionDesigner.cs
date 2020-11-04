using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAnimationBuilder
{
    public partial class SectionDesigner : Form
    {
        // the goal is to make this form a separated module, so I'm not gonna use singleton here
        Dictionary<string, AnimatingObject> AnimatingObjects;
        Section Section;

        Bitmap PreviewBitmap;

        public CClass SelectedClass
        {
            get => AnimatingObjects[comboBox_Class.Text] as CClass;
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
                else
                {
                    // CuteTN Note: be careful...
                    return new CObject();
                }
            }
        }

        #region init
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
            var propNames = SelectedClass.GetUndefinedPropertiesNames();

            dataGridView_Properties.Rows.Clear();

            if(dataGridView_Objects.SelectedRows.Count == 1)
            {
                dataGridView_Properties.Rows.Add("Id", SelectedObject.StringId);
                dataGridView_Properties.Rows[0].Cells[1].ReadOnly = false;
            }
            else
            {
                dataGridView_Properties.Rows.Add("Id", Utils.UndefinedValue);
                dataGridView_Properties.Rows[0].Cells[1].ReadOnly = true;
            }

            foreach(var propName in propNames)
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
        }
        private void dataGridView_Objects_SelectionChanged(object sender, EventArgs e)
        {
            LoadPropertiesOfObject(); 
        }
        #endregion

        #region Updated selected class
        private void comboBox_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadObjectsOfSelectedClass();
            LoadPropertiesOfObject();
        }
        #endregion

        #region update rendering
        private void UpdatePreviewPictureBox()
        {
            Texture background = AnimatingObjects[Section.TextureId] as Texture;
            pictureBox_SectionPreview.Image = background.Bitmap; 
        }

        private void SectionDesigner_Paint(object sender, PaintEventArgs e)
        {
            UpdatePreviewPictureBox();
        }

        #endregion

        #region Update value to data
        private void UpdateValueNotId()
        {
            foreach(DataGridViewRow propRow in dataGridView_Properties.Rows)
            {
                string propName = propRow.Cells[0].Value as string;
                string propVal = propRow.Cells[1].Value as string;

                if(propName == "Id")
                    continue;

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

        private void dataGridView_Properties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateValueNotId();
        }
        #endregion

    }
}
