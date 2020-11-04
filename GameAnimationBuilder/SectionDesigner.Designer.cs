namespace GameAnimationBuilder
{
    partial class SectionDesigner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_Class = new System.Windows.Forms.ComboBox();
            this.pictureBox_SectionPreview = new System.Windows.Forms.PictureBox();
            this.dataGridView_Objects = new System.Windows.Forms.DataGridView();
            this.Objects = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_Properties = new System.Windows.Forms.DataGridView();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SectionPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Objects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_Class
            // 
            this.comboBox_Class.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Class.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Class.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Class.FormattingEnabled = true;
            this.comboBox_Class.Location = new System.Drawing.Point(552, 12);
            this.comboBox_Class.Name = "comboBox_Class";
            this.comboBox_Class.Size = new System.Drawing.Size(390, 36);
            this.comboBox_Class.TabIndex = 0;
            this.comboBox_Class.SelectedIndexChanged += new System.EventHandler(this.comboBox_Class_SelectedIndexChanged);
            // 
            // pictureBox_SectionPreview
            // 
            this.pictureBox_SectionPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_SectionPreview.Location = new System.Drawing.Point(13, 12);
            this.pictureBox_SectionPreview.Name = "pictureBox_SectionPreview";
            this.pictureBox_SectionPreview.Size = new System.Drawing.Size(533, 578);
            this.pictureBox_SectionPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_SectionPreview.TabIndex = 1;
            this.pictureBox_SectionPreview.TabStop = false;
            // 
            // dataGridView_Objects
            // 
            this.dataGridView_Objects.AllowUserToAddRows = false;
            this.dataGridView_Objects.AllowUserToDeleteRows = false;
            this.dataGridView_Objects.AllowUserToResizeRows = false;
            this.dataGridView_Objects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Objects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Objects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Objects});
            this.dataGridView_Objects.Location = new System.Drawing.Point(552, 54);
            this.dataGridView_Objects.Name = "dataGridView_Objects";
            this.dataGridView_Objects.ReadOnly = true;
            this.dataGridView_Objects.RowHeadersVisible = false;
            this.dataGridView_Objects.RowHeadersWidth = 51;
            this.dataGridView_Objects.RowTemplate.Height = 24;
            this.dataGridView_Objects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Objects.Size = new System.Drawing.Size(390, 171);
            this.dataGridView_Objects.TabIndex = 3;
            this.dataGridView_Objects.SelectionChanged += new System.EventHandler(this.dataGridView_Objects_SelectionChanged);
            // 
            // Objects
            // 
            this.Objects.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Objects.HeaderText = "Objects";
            this.Objects.MinimumWidth = 6;
            this.Objects.Name = "Objects";
            this.Objects.ReadOnly = true;
            this.Objects.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridView_Properties
            // 
            this.dataGridView_Properties.AllowUserToAddRows = false;
            this.dataGridView_Properties.AllowUserToDeleteRows = false;
            this.dataGridView_Properties.AllowUserToResizeRows = false;
            this.dataGridView_Properties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Properties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Property,
            this.Value});
            this.dataGridView_Properties.Location = new System.Drawing.Point(552, 270);
            this.dataGridView_Properties.Name = "dataGridView_Properties";
            this.dataGridView_Properties.RowHeadersVisible = false;
            this.dataGridView_Properties.RowHeadersWidth = 51;
            this.dataGridView_Properties.RowTemplate.Height = 24;
            this.dataGridView_Properties.Size = new System.Drawing.Size(390, 320);
            this.dataGridView_Properties.TabIndex = 4;
            this.dataGridView_Properties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Properties_CellValueChanged);
            // 
            // Property
            // 
            this.Property.HeaderText = "Property";
            this.Property.MinimumWidth = 6;
            this.Property.Name = "Property";
            this.Property.ReadOnly = true;
            this.Property.Width = 125;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 6;
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Add.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button_Add.Location = new System.Drawing.Point(865, 231);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(77, 33);
            this.button_Add.TabIndex = 5;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Delete.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button_Delete.Location = new System.Drawing.Point(782, 231);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(77, 33);
            this.button_Delete.TabIndex = 6;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // SectionDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(954, 602);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.dataGridView_Properties);
            this.Controls.Add(this.dataGridView_Objects);
            this.Controls.Add(this.pictureBox_SectionPreview);
            this.Controls.Add(this.comboBox_Class);
            this.DoubleBuffered = true;
            this.Name = "SectionDesigner";
            this.Text = "Section Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SectionDesigner_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SectionPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Objects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Class;
        private System.Windows.Forms.PictureBox pictureBox_SectionPreview;
        private System.Windows.Forms.DataGridView dataGridView_Objects;
        private System.Windows.Forms.DataGridView dataGridView_Properties;
        private System.Windows.Forms.DataGridViewTextBoxColumn Objects;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Delete;
    }
}