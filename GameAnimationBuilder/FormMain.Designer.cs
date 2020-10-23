namespace GameAnimationBuilder
{
    partial class FormMain
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
            this.textBox_Code = new System.Windows.Forms.TextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.pictureBox_Preview = new System.Windows.Forms.PictureBox();
            this.button_View = new System.Windows.Forms.Button();
            this.listBox_Suggestions = new System.Windows.Forms.ListBox();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_Tricks = new System.Windows.Forms.Button();
            this.textBox_Hint = new System.Windows.Forms.TextBox();
            this.checkBox_AutoAdd = new System.Windows.Forms.CheckBox();
            this.button_Export = new System.Windows.Forms.Button();
            this.checkBox_AutoView = new System.Windows.Forms.CheckBox();
            this.textBox_WorkingDir = new System.Windows.Forms.TextBox();
            this.button_ChangeWorkingDir = new System.Windows.Forms.Button();
            this.label_WorkingDir = new System.Windows.Forms.Label();
            this.button_BackUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_Code
            // 
            this.textBox_Code.AllowDrop = true;
            this.textBox_Code.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Code.BackColor = System.Drawing.Color.Black;
            this.textBox_Code.Font = new System.Drawing.Font("Courier New", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBox_Code.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.textBox_Code.Location = new System.Drawing.Point(14, 14);
            this.textBox_Code.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_Code.Multiline = true;
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Code.Size = new System.Drawing.Size(862, 653);
            this.textBox_Code.TabIndex = 0;
            this.textBox_Code.WordWrap = false;
            this.textBox_Code.TextChanged += new System.EventHandler(this.textBox_Code_TextChanged);
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(1275, 673);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(104, 30);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "Save (F2)";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Load
            // 
            this.button_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Load.Location = new System.Drawing.Point(1165, 673);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(104, 30);
            this.button_Load.TabIndex = 2;
            this.button_Load.Text = "Load (F3)";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Add.Location = new System.Drawing.Point(1275, 637);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(104, 30);
            this.button_Add.TabIndex = 3;
            this.button_Add.Text = "Add (F5)";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // pictureBox_Preview
            // 
            this.pictureBox_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Preview.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pictureBox_Preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Preview.Location = new System.Drawing.Point(883, 14);
            this.pictureBox_Preview.Name = "pictureBox_Preview";
            this.pictureBox_Preview.Size = new System.Drawing.Size(490, 215);
            this.pictureBox_Preview.TabIndex = 4;
            this.pictureBox_Preview.TabStop = false;
            // 
            // button_View
            // 
            this.button_View.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_View.Location = new System.Drawing.Point(1165, 637);
            this.button_View.Name = "button_View";
            this.button_View.Size = new System.Drawing.Size(104, 30);
            this.button_View.TabIndex = 5;
            this.button_View.Text = "View (F6)";
            this.button_View.UseVisualStyleBackColor = true;
            this.button_View.Click += new System.EventHandler(this.button_View_Click);
            // 
            // listBox_Suggestions
            // 
            this.listBox_Suggestions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Suggestions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBox_Suggestions.Font = new System.Drawing.Font("Courier New", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_Suggestions.ForeColor = System.Drawing.Color.Thistle;
            this.listBox_Suggestions.FormattingEnabled = true;
            this.listBox_Suggestions.ItemHeight = 31;
            this.listBox_Suggestions.Location = new System.Drawing.Point(883, 335);
            this.listBox_Suggestions.Name = "listBox_Suggestions";
            this.listBox_Suggestions.Size = new System.Drawing.Size(490, 252);
            this.listBox_Suggestions.TabIndex = 6;
            this.listBox_Suggestions.Visible = false;
            this.listBox_Suggestions.SelectedIndexChanged += new System.EventHandler(this.listBox_Suggestions_SelectedIndexChanged);
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Clear.Location = new System.Drawing.Point(1055, 637);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(104, 30);
            this.button_Clear.TabIndex = 7;
            this.button_Clear.Text = "Clear (F4)";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_Tricks
            // 
            this.button_Tricks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Tricks.Location = new System.Drawing.Point(945, 673);
            this.button_Tricks.Name = "button_Tricks";
            this.button_Tricks.Size = new System.Drawing.Size(104, 30);
            this.button_Tricks.TabIndex = 8;
            this.button_Tricks.Text = "Tricks (F1)";
            this.button_Tricks.UseVisualStyleBackColor = true;
            this.button_Tricks.Click += new System.EventHandler(this.button_Tricks_Click);
            // 
            // textBox_Hint
            // 
            this.textBox_Hint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Hint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_Hint.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Hint.ForeColor = System.Drawing.Color.Thistle;
            this.textBox_Hint.Location = new System.Drawing.Point(883, 244);
            this.textBox_Hint.Multiline = true;
            this.textBox_Hint.Name = "textBox_Hint";
            this.textBox_Hint.ReadOnly = true;
            this.textBox_Hint.Size = new System.Drawing.Size(490, 85);
            this.textBox_Hint.TabIndex = 9;
            // 
            // checkBox_AutoAdd
            // 
            this.checkBox_AutoAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_AutoAdd.BackColor = System.Drawing.Color.Lavender;
            this.checkBox_AutoAdd.Checked = true;
            this.checkBox_AutoAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AutoAdd.Location = new System.Drawing.Point(1275, 601);
            this.checkBox_AutoAdd.Name = "checkBox_AutoAdd";
            this.checkBox_AutoAdd.Size = new System.Drawing.Size(104, 30);
            this.checkBox_AutoAdd.TabIndex = 10;
            this.checkBox_AutoAdd.Text = "Auto Add";
            this.checkBox_AutoAdd.UseVisualStyleBackColor = false;
            // 
            // button_Export
            // 
            this.button_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Export.Location = new System.Drawing.Point(1055, 673);
            this.button_Export.Name = "button_Export";
            this.button_Export.Size = new System.Drawing.Size(104, 30);
            this.button_Export.TabIndex = 11;
            this.button_Export.Text = "Export (F7)";
            this.button_Export.UseVisualStyleBackColor = true;
            this.button_Export.Click += new System.EventHandler(this.button_Export_Click);
            // 
            // checkBox_AutoView
            // 
            this.checkBox_AutoView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_AutoView.BackColor = System.Drawing.Color.Lavender;
            this.checkBox_AutoView.Checked = true;
            this.checkBox_AutoView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AutoView.Location = new System.Drawing.Point(1165, 601);
            this.checkBox_AutoView.Name = "checkBox_AutoView";
            this.checkBox_AutoView.Size = new System.Drawing.Size(104, 30);
            this.checkBox_AutoView.TabIndex = 12;
            this.checkBox_AutoView.Text = "Auto View";
            this.checkBox_AutoView.UseVisualStyleBackColor = false;
            // 
            // textBox_WorkingDir
            // 
            this.textBox_WorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WorkingDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_WorkingDir.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_WorkingDir.ForeColor = System.Drawing.Color.Thistle;
            this.textBox_WorkingDir.Location = new System.Drawing.Point(149, 673);
            this.textBox_WorkingDir.Name = "textBox_WorkingDir";
            this.textBox_WorkingDir.ReadOnly = true;
            this.textBox_WorkingDir.Size = new System.Drawing.Size(649, 30);
            this.textBox_WorkingDir.TabIndex = 13;
            this.textBox_WorkingDir.WordWrap = false;
            this.textBox_WorkingDir.TextChanged += new System.EventHandler(this.textBox_WorkingDir_TextChanged);
            // 
            // button_ChangeWorkingDir
            // 
            this.button_ChangeWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ChangeWorkingDir.Location = new System.Drawing.Point(804, 673);
            this.button_ChangeWorkingDir.Name = "button_ChangeWorkingDir";
            this.button_ChangeWorkingDir.Size = new System.Drawing.Size(72, 30);
            this.button_ChangeWorkingDir.TabIndex = 14;
            this.button_ChangeWorkingDir.Text = "Change";
            this.button_ChangeWorkingDir.UseVisualStyleBackColor = true;
            this.button_ChangeWorkingDir.Click += new System.EventHandler(this.button_ChangeWorkingDir_Click);
            // 
            // label_WorkingDir
            // 
            this.label_WorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_WorkingDir.AutoSize = true;
            this.label_WorkingDir.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_WorkingDir.Location = new System.Drawing.Point(14, 678);
            this.label_WorkingDir.Name = "label_WorkingDir";
            this.label_WorkingDir.Size = new System.Drawing.Size(129, 20);
            this.label_WorkingDir.TabIndex = 15;
            this.label_WorkingDir.Text = "Working Dir:";
            // 
            // button_BackUp
            // 
            this.button_BackUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_BackUp.Location = new System.Drawing.Point(945, 637);
            this.button_BackUp.Name = "button_BackUp";
            this.button_BackUp.Size = new System.Drawing.Size(104, 30);
            this.button_BackUp.TabIndex = 16;
            this.button_BackUp.Text = "BackUp (F8)";
            this.button_BackUp.UseVisualStyleBackColor = true;
            this.button_BackUp.Click += new System.EventHandler(this.button_BackUp_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(1385, 714);
            this.Controls.Add(this.button_BackUp);
            this.Controls.Add(this.label_WorkingDir);
            this.Controls.Add(this.button_ChangeWorkingDir);
            this.Controls.Add(this.textBox_WorkingDir);
            this.Controls.Add(this.checkBox_AutoView);
            this.Controls.Add(this.button_Export);
            this.Controls.Add(this.checkBox_AutoAdd);
            this.Controls.Add(this.textBox_Hint);
            this.Controls.Add(this.button_Tricks);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.listBox_Suggestions);
            this.Controls.Add(this.button_View);
            this.Controls.Add(this.pictureBox_Preview);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.button_Load);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.textBox_Code);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormMain";
            this.Text = "Game Anim Builder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Code;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.PictureBox pictureBox_Preview;
        private System.Windows.Forms.Button button_View;
        private System.Windows.Forms.ListBox listBox_Suggestions;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_Tricks;
        private System.Windows.Forms.TextBox textBox_Hint;
        private System.Windows.Forms.CheckBox checkBox_AutoAdd;
        private System.Windows.Forms.Button button_Export;
        private System.Windows.Forms.CheckBox checkBox_AutoView;
        private System.Windows.Forms.TextBox textBox_WorkingDir;
        private System.Windows.Forms.Button button_ChangeWorkingDir;
        private System.Windows.Forms.Label label_WorkingDir;
        private System.Windows.Forms.Button button_BackUp;
    }
}

