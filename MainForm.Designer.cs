namespace SyncDataTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listBox_Product_Ids = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Websites = new System.Windows.Forms.ComboBox();
            this.textBox_Id = new System.Windows.Forms.TextBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.checkedListBox_Websites = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listView_Results = new System.Windows.Forms.ListView();
            this.columnHeader_Product_Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox_Delete = new System.Windows.Forms.PictureBox();
            this.pictureBox_Clear = new System.Windows.Forms.PictureBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_gif = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clear)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox_Product_Ids
            // 
            this.listBox_Product_Ids.ForeColor = System.Drawing.Color.Blue;
            this.listBox_Product_Ids.FormattingEnabled = true;
            this.listBox_Product_Ids.HorizontalScrollbar = true;
            this.listBox_Product_Ids.ItemHeight = 12;
            this.listBox_Product_Ids.Location = new System.Drawing.Point(14, 45);
            this.listBox_Product_Ids.Name = "listBox_Product_Ids";
            this.listBox_Product_Ids.Size = new System.Drawing.Size(235, 148);
            this.listBox_Product_Ids.TabIndex = 3;
            this.listBox_Product_Ids.SelectedIndexChanged += new System.EventHandler(this.listBox_Product_Ids_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Product ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "From:";
            // 
            // comboBox_Websites
            // 
            this.comboBox_Websites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Websites.ForeColor = System.Drawing.Color.Blue;
            this.comboBox_Websites.FormattingEnabled = true;
            this.comboBox_Websites.Location = new System.Drawing.Point(399, 11);
            this.comboBox_Websites.Name = "comboBox_Websites";
            this.comboBox_Websites.Size = new System.Drawing.Size(183, 20);
            this.comboBox_Websites.TabIndex = 0;
            this.toolTip1.SetToolTip(this.comboBox_Websites, "Select website");
            this.comboBox_Websites.SelectedIndexChanged += new System.EventHandler(this.comboBox_Websites_SelectedIndexChanged);
            // 
            // textBox_Id
            // 
            this.textBox_Id.ForeColor = System.Drawing.Color.Blue;
            this.textBox_Id.Location = new System.Drawing.Point(89, 11);
            this.textBox_Id.Name = "textBox_Id";
            this.textBox_Id.Size = new System.Drawing.Size(116, 21);
            this.textBox_Id.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBox_Id, "Input Product ID, Split Char: \',\'");
            this.textBox_Id.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_Id_KeyUp);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(211, 10);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(41, 23);
            this.btn_Add.TabIndex = 2;
            this.btn_Add.Text = "Add";
            this.toolTip1.SetToolTip(this.btn_Add, "Add to list");
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // checkedListBox_Websites
            // 
            this.checkedListBox_Websites.CheckOnClick = true;
            this.checkedListBox_Websites.ForeColor = System.Drawing.Color.Blue;
            this.checkedListBox_Websites.FormattingEnabled = true;
            this.checkedListBox_Websites.HorizontalScrollbar = true;
            this.checkedListBox_Websites.Location = new System.Drawing.Point(399, 44);
            this.checkedListBox_Websites.Name = "checkedListBox_Websites";
            this.checkedListBox_Websites.Size = new System.Drawing.Size(183, 148);
            this.checkedListBox_Websites.TabIndex = 5;
            this.checkedListBox_Websites.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_Websites_ItemCheck);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "To:";
            // 
            // listView_Results
            // 
            this.listView_Results.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Product_Id});
            this.listView_Results.Font = new System.Drawing.Font("宋体", 9F);
            this.listView_Results.FullRowSelect = true;
            this.listView_Results.GridLines = true;
            this.listView_Results.Location = new System.Drawing.Point(13, 225);
            this.listView_Results.Name = "listView_Results";
            this.listView_Results.Size = new System.Drawing.Size(569, 133);
            this.listView_Results.TabIndex = 6;
            this.listView_Results.UseCompatibleStateImageBehavior = false;
            this.listView_Results.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_Product_Id
            // 
            this.columnHeader_Product_Id.Text = "Product ID";
            this.columnHeader_Product_Id.Width = 110;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "Results:";
            // 
            // pictureBox_Delete
            // 
            this.pictureBox_Delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Delete.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Delete.Image")));
            this.pictureBox_Delete.Location = new System.Drawing.Point(255, 45);
            this.pictureBox_Delete.Name = "pictureBox_Delete";
            this.pictureBox_Delete.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_Delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_Delete.TabIndex = 17;
            this.pictureBox_Delete.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Delete, "Delete");
            this.pictureBox_Delete.Visible = false;
            this.pictureBox_Delete.Click += new System.EventHandler(this.pictureBox_Delete_Click);
            // 
            // pictureBox_Clear
            // 
            this.pictureBox_Clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Clear.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Clear.Image")));
            this.pictureBox_Clear.Location = new System.Drawing.Point(255, 74);
            this.pictureBox_Clear.Name = "pictureBox_Clear";
            this.pictureBox_Clear.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_Clear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_Clear.TabIndex = 18;
            this.pictureBox_Clear.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Clear, "Clear");
            this.pictureBox_Clear.Visible = false;
            this.pictureBox_Clear.Click += new System.EventHandler(this.pictureBox_Clear_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.Enabled = false;
            this.btn_Start.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Start.Image = ((System.Drawing.Image)(resources.GetObject("btn_Start.Image")));
            this.btn_Start.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Start.Location = new System.Drawing.Point(271, 104);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(104, 43);
            this.btn_Start.TabIndex = 4;
            this.btn_Start.Text = "Start";
            this.btn_Start.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btn_Start, "Start Sync Product");
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // textBox_Password
            // 
            this.textBox_Password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Password.Location = new System.Drawing.Point(399, 198);
            this.textBox_Password.MaxLength = 32;
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '*';
            this.textBox_Password.Size = new System.Drawing.Size(183, 21);
            this.textBox_Password.TabIndex = 20;
            this.toolTip1.SetToolTip(this.textBox_Password, "Please input password.");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_gif,
            this.toolStripStatusLabel_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 370);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(594, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_gif
            // 
            this.toolStripStatusLabel_gif.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel_gif.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel_gif.Image")));
            this.toolStripStatusLabel_gif.Name = "toolStripStatusLabel_gif";
            this.toolStripStatusLabel_gif.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel_gif.Visible = false;
            // 
            // toolStripStatusLabel_Status
            // 
            this.toolStripStatusLabel_Status.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel_Status.Name = "toolStripStatusLabel_Status";
            this.toolStripStatusLabel_Status.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel_Status.Text = "Ready";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(377, 200);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 392);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.pictureBox_Clear);
            this.Controls.Add(this.pictureBox_Delete);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listView_Results);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkedListBox_Websites);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.textBox_Id);
            this.Controls.Add(this.comboBox_Websites);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Product_Ids);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sync Data Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clear)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Product_Ids;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Websites;
        private System.Windows.Forms.TextBox textBox_Id;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.CheckedListBox checkedListBox_Websites;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView_Results;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox_Delete;
        private System.Windows.Forms.PictureBox pictureBox_Clear;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColumnHeader columnHeader_Product_Id;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Status;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_gif;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

