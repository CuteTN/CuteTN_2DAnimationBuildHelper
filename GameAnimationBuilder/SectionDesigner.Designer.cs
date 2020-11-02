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
            this.pictureBox_ScenePreview = new System.Windows.Forms.PictureBox();
            this.dataGridView_Objects = new System.Windows.Forms.DataGridView();
            this.dataGridView_Properties = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ScenePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Objects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_Class
            // 
            this.comboBox_Class.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Class.FormattingEnabled = true;
            this.comboBox_Class.Location = new System.Drawing.Point(525, 12);
            this.comboBox_Class.Name = "comboBox_Class";
            this.comboBox_Class.Size = new System.Drawing.Size(263, 24);
            this.comboBox_Class.TabIndex = 0;
            // 
            // pictureBox_ScenePreview
            // 
            this.pictureBox_ScenePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_ScenePreview.Location = new System.Drawing.Point(13, 13);
            this.pictureBox_ScenePreview.Name = "pictureBox_ScenePreview";
            this.pictureBox_ScenePreview.Size = new System.Drawing.Size(506, 425);
            this.pictureBox_ScenePreview.TabIndex = 1;
            this.pictureBox_ScenePreview.TabStop = false;
            // 
            // dataGridView_Objects
            // 
            this.dataGridView_Objects.AllowUserToAddRows = false;
            this.dataGridView_Objects.AllowUserToDeleteRows = false;
            this.dataGridView_Objects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Objects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Objects.Location = new System.Drawing.Point(525, 42);
            this.dataGridView_Objects.Name = "dataGridView_Objects";
            this.dataGridView_Objects.RowHeadersWidth = 51;
            this.dataGridView_Objects.RowTemplate.Height = 24;
            this.dataGridView_Objects.Size = new System.Drawing.Size(263, 222);
            this.dataGridView_Objects.TabIndex = 3;
            // 
            // dataGridView_Properties
            // 
            this.dataGridView_Properties.AllowUserToAddRows = false;
            this.dataGridView_Properties.AllowUserToDeleteRows = false;
            this.dataGridView_Properties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Properties.Location = new System.Drawing.Point(525, 270);
            this.dataGridView_Properties.Name = "dataGridView_Properties";
            this.dataGridView_Properties.RowHeadersWidth = 51;
            this.dataGridView_Properties.RowTemplate.Height = 24;
            this.dataGridView_Properties.Size = new System.Drawing.Size(263, 168);
            this.dataGridView_Properties.TabIndex = 4;
            // 
            // SectionDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView_Properties);
            this.Controls.Add(this.dataGridView_Objects);
            this.Controls.Add(this.pictureBox_ScenePreview);
            this.Controls.Add(this.comboBox_Class);
            this.DoubleBuffered = true;
            this.Name = "SectionDesigner";
            this.Text = "Section Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ScenePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Objects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Class;
        private System.Windows.Forms.PictureBox pictureBox_ScenePreview;
        private System.Windows.Forms.DataGridView dataGridView_Objects;
        private System.Windows.Forms.DataGridView dataGridView_Properties;
    }
}