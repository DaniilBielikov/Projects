namespace Client
{
    partial class FileManager
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
            this.components = new System.ComponentModel.Container();
            this.listView_ThisPC = new System.Windows.Forms.ListView();
            this.columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DateOfChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBar_SendingProgress = new System.Windows.Forms.ProgressBar();
            this.label_SendingProgress = new System.Windows.Forms.Label();
            this.textBox_Path_ThisPC = new System.Windows.Forms.TextBox();
            this.label_ThisPC = new System.Windows.Forms.Label();
            this.label_RemotePC = new System.Windows.Forms.Label();
            this.textBox_Path_RemotePC = new System.Windows.Forms.TextBox();
            this.comboBox_LD_ThisPC = new System.Windows.Forms.ComboBox();
            this.comboBox_LD_RemotePC = new System.Windows.Forms.ComboBox();
            this.label_LD_ThisPC = new System.Windows.Forms.Label();
            this.label_LD_RemotePC = new System.Windows.Forms.Label();
            this.button_Back_ThisPC = new System.Windows.Forms.Button();
            this.button_SendFile_toRemotePC = new System.Windows.Forms.Button();
            this.button_Back_RemotePC = new System.Windows.Forms.Button();
            this.listView_RemotePC = new System.Windows.Forms.ListView();
            this.columnHeader__Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader__DateOfChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader__Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader__Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_SendFile_toThisPC = new System.Windows.Forms.Button();
            this.contextMenuStrip_LV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_Speed = new System.Windows.Forms.Label();
            this.label_Received = new System.Windows.Forms.Label();
            this.label_NameThisPC = new System.Windows.Forms.Label();
            this.label_NameRemotePC = new System.Windows.Forms.Label();
            this.label_ReceivedCount = new System.Windows.Forms.Label();
            this.label_SpeedCount = new System.Windows.Forms.Label();
            this.label_MustReceivedCount = new System.Windows.Forms.Label();
            this.label_OR = new System.Windows.Forms.Label();
            this.label_Status = new System.Windows.Forms.Label();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_LV.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_ThisPC
            // 
            this.listView_ThisPC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Name,
            this.columnHeader_DateOfChange,
            this.columnHeader_Type,
            this.columnHeader_Size});
            this.listView_ThisPC.FullRowSelect = true;
            this.listView_ThisPC.GridLines = true;
            this.listView_ThisPC.HideSelection = false;
            this.listView_ThisPC.Location = new System.Drawing.Point(12, 101);
            this.listView_ThisPC.MultiSelect = false;
            this.listView_ThisPC.Name = "listView_ThisPC";
            this.listView_ThisPC.Size = new System.Drawing.Size(540, 660);
            this.listView_ThisPC.TabIndex = 0;
            this.listView_ThisPC.UseCompatibleStateImageBehavior = false;
            this.listView_ThisPC.View = System.Windows.Forms.View.Details;
            this.listView_ThisPC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_ThisPC_MouseClick);
            this.listView_ThisPC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_ThisPC_MouseDoubleClick);
            // 
            // columnHeader_Name
            // 
            this.columnHeader_Name.Text = "Name";
            this.columnHeader_Name.Width = 190;
            // 
            // columnHeader_DateOfChange
            // 
            this.columnHeader_DateOfChange.Text = "Date of change";
            this.columnHeader_DateOfChange.Width = 150;
            // 
            // columnHeader_Type
            // 
            this.columnHeader_Type.Text = "Type";
            this.columnHeader_Type.Width = 100;
            // 
            // columnHeader_Size
            // 
            this.columnHeader_Size.Text = "Size";
            this.columnHeader_Size.Width = 100;
            // 
            // progressBar_SendingProgress
            // 
            this.progressBar_SendingProgress.Location = new System.Drawing.Point(490, 774);
            this.progressBar_SendingProgress.Name = "progressBar_SendingProgress";
            this.progressBar_SendingProgress.Size = new System.Drawing.Size(245, 25);
            this.progressBar_SendingProgress.TabIndex = 1;
            // 
            // label_SendingProgress
            // 
            this.label_SendingProgress.AutoSize = true;
            this.label_SendingProgress.Location = new System.Drawing.Point(411, 778);
            this.label_SendingProgress.Name = "label_SendingProgress";
            this.label_SendingProgress.Size = new System.Drawing.Size(73, 18);
            this.label_SendingProgress.TabIndex = 2;
            this.label_SendingProgress.Text = "Progress:";
            // 
            // textBox_Path_ThisPC
            // 
            this.textBox_Path_ThisPC.Location = new System.Drawing.Point(58, 68);
            this.textBox_Path_ThisPC.Name = "textBox_Path_ThisPC";
            this.textBox_Path_ThisPC.Size = new System.Drawing.Size(494, 24);
            this.textBox_Path_ThisPC.TabIndex = 3;
            // 
            // label_ThisPC
            // 
            this.label_ThisPC.AutoSize = true;
            this.label_ThisPC.Location = new System.Drawing.Point(9, 15);
            this.label_ThisPC.Name = "label_ThisPC";
            this.label_ThisPC.Size = new System.Drawing.Size(65, 18);
            this.label_ThisPC.TabIndex = 5;
            this.label_ThisPC.Text = "This PC:";
            // 
            // label_RemotePC
            // 
            this.label_RemotePC.AutoSize = true;
            this.label_RemotePC.Location = new System.Drawing.Point(672, 15);
            this.label_RemotePC.Name = "label_RemotePC";
            this.label_RemotePC.Size = new System.Drawing.Size(90, 18);
            this.label_RemotePC.TabIndex = 6;
            this.label_RemotePC.Text = "Remote PC:";
            // 
            // textBox_Path_RemotePC
            // 
            this.textBox_Path_RemotePC.Location = new System.Drawing.Point(718, 68);
            this.textBox_Path_RemotePC.Name = "textBox_Path_RemotePC";
            this.textBox_Path_RemotePC.Size = new System.Drawing.Size(497, 24);
            this.textBox_Path_RemotePC.TabIndex = 7;
            // 
            // comboBox_LD_ThisPC
            // 
            this.comboBox_LD_ThisPC.FormattingEnabled = true;
            this.comboBox_LD_ThisPC.Location = new System.Drawing.Point(105, 36);
            this.comboBox_LD_ThisPC.Name = "comboBox_LD_ThisPC";
            this.comboBox_LD_ThisPC.Size = new System.Drawing.Size(100, 26);
            this.comboBox_LD_ThisPC.TabIndex = 8;
            this.comboBox_LD_ThisPC.SelectedIndexChanged += new System.EventHandler(this.comboBox_LD_ThisPC_SelectedIndexChanged);
            this.comboBox_LD_ThisPC.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboBox_LD_ThisPC_MouseDown);
            // 
            // comboBox_LD_RemotePC
            // 
            this.comboBox_LD_RemotePC.FormattingEnabled = true;
            this.comboBox_LD_RemotePC.Location = new System.Drawing.Point(768, 36);
            this.comboBox_LD_RemotePC.Name = "comboBox_LD_RemotePC";
            this.comboBox_LD_RemotePC.Size = new System.Drawing.Size(100, 26);
            this.comboBox_LD_RemotePC.TabIndex = 9;
            this.comboBox_LD_RemotePC.SelectedIndexChanged += new System.EventHandler(this.comboBox_LD_RemotePC_SelectedIndexChanged);
            this.comboBox_LD_RemotePC.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboBox_LD_RemotePC_MouseDown);
            // 
            // label_LD_ThisPC
            // 
            this.label_LD_ThisPC.AutoSize = true;
            this.label_LD_ThisPC.Location = new System.Drawing.Point(9, 39);
            this.label_LD_ThisPC.Name = "label_LD_ThisPC";
            this.label_LD_ThisPC.Size = new System.Drawing.Size(90, 18);
            this.label_LD_ThisPC.TabIndex = 10;
            this.label_LD_ThisPC.Text = "Logical disk:";
            // 
            // label_LD_RemotePC
            // 
            this.label_LD_RemotePC.AutoSize = true;
            this.label_LD_RemotePC.Location = new System.Drawing.Point(672, 39);
            this.label_LD_RemotePC.Name = "label_LD_RemotePC";
            this.label_LD_RemotePC.Size = new System.Drawing.Size(90, 18);
            this.label_LD_RemotePC.TabIndex = 11;
            this.label_LD_RemotePC.Text = "Logical disk:";
            // 
            // button_Back_ThisPC
            // 
            this.button_Back_ThisPC.Location = new System.Drawing.Point(12, 65);
            this.button_Back_ThisPC.Name = "button_Back_ThisPC";
            this.button_Back_ThisPC.Size = new System.Drawing.Size(40, 30);
            this.button_Back_ThisPC.TabIndex = 12;
            this.button_Back_ThisPC.Text = "<-";
            this.button_Back_ThisPC.UseVisualStyleBackColor = true;
            this.button_Back_ThisPC.Click += new System.EventHandler(this.button_Back_ThisPC_Click);
            // 
            // button_SendFile_toRemotePC
            // 
            this.button_SendFile_toRemotePC.Location = new System.Drawing.Point(567, 291);
            this.button_SendFile_toRemotePC.Name = "button_SendFile_toRemotePC";
            this.button_SendFile_toRemotePC.Size = new System.Drawing.Size(90, 30);
            this.button_SendFile_toRemotePC.TabIndex = 13;
            this.button_SendFile_toRemotePC.Text = "Send >>";
            this.button_SendFile_toRemotePC.UseVisualStyleBackColor = true;
            this.button_SendFile_toRemotePC.Click += new System.EventHandler(this.button_SendFile_toRemotePC_Click);
            // 
            // button_Back_RemotePC
            // 
            this.button_Back_RemotePC.Location = new System.Drawing.Point(672, 65);
            this.button_Back_RemotePC.Name = "button_Back_RemotePC";
            this.button_Back_RemotePC.Size = new System.Drawing.Size(40, 30);
            this.button_Back_RemotePC.TabIndex = 15;
            this.button_Back_RemotePC.Text = "<-";
            this.button_Back_RemotePC.UseVisualStyleBackColor = true;
            this.button_Back_RemotePC.Click += new System.EventHandler(this.button_Back_RemotePC_Click);
            // 
            // listView_RemotePC
            // 
            this.listView_RemotePC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader__Name,
            this.columnHeader__DateOfChange,
            this.columnHeader__Type,
            this.columnHeader__Size});
            this.listView_RemotePC.FullRowSelect = true;
            this.listView_RemotePC.GridLines = true;
            this.listView_RemotePC.HideSelection = false;
            this.listView_RemotePC.Location = new System.Drawing.Point(675, 101);
            this.listView_RemotePC.MultiSelect = false;
            this.listView_RemotePC.Name = "listView_RemotePC";
            this.listView_RemotePC.Size = new System.Drawing.Size(540, 660);
            this.listView_RemotePC.TabIndex = 17;
            this.listView_RemotePC.UseCompatibleStateImageBehavior = false;
            this.listView_RemotePC.View = System.Windows.Forms.View.Details;
            this.listView_RemotePC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_RemotePC_MouseClick);
            this.listView_RemotePC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_RemotePC_MouseDoubleClick);
            // 
            // columnHeader__Name
            // 
            this.columnHeader__Name.Text = "Name";
            this.columnHeader__Name.Width = 190;
            // 
            // columnHeader__DateOfChange
            // 
            this.columnHeader__DateOfChange.Text = "Date of change";
            this.columnHeader__DateOfChange.Width = 150;
            // 
            // columnHeader__Type
            // 
            this.columnHeader__Type.Text = "Type";
            this.columnHeader__Type.Width = 100;
            // 
            // columnHeader__Size
            // 
            this.columnHeader__Size.Text = "Size";
            this.columnHeader__Size.Width = 100;
            // 
            // button_SendFile_toThisPC
            // 
            this.button_SendFile_toThisPC.Location = new System.Drawing.Point(567, 421);
            this.button_SendFile_toThisPC.Name = "button_SendFile_toThisPC";
            this.button_SendFile_toThisPC.Size = new System.Drawing.Size(90, 30);
            this.button_SendFile_toThisPC.TabIndex = 18;
            this.button_SendFile_toThisPC.Text = "<< Send";
            this.button_SendFile_toThisPC.UseVisualStyleBackColor = true;
            this.button_SendFile_toThisPC.Click += new System.EventHandler(this.button_SendFile_toThisPC_Click);
            // 
            // contextMenuStrip_LV
            // 
            this.contextMenuStrip_LV.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_LV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip_LV.Name = "contextMenuStrip_LV";
            this.contextMenuStrip_LV.Size = new System.Drawing.Size(211, 80);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // label_Speed
            // 
            this.label_Speed.AutoSize = true;
            this.label_Speed.Location = new System.Drawing.Point(743, 778);
            this.label_Speed.Name = "label_Speed";
            this.label_Speed.Size = new System.Drawing.Size(54, 18);
            this.label_Speed.TabIndex = 20;
            this.label_Speed.Text = "Speed:";
            // 
            // label_Received
            // 
            this.label_Received.AutoSize = true;
            this.label_Received.Location = new System.Drawing.Point(930, 778);
            this.label_Received.Name = "label_Received";
            this.label_Received.Size = new System.Drawing.Size(73, 18);
            this.label_Received.TabIndex = 21;
            this.label_Received.Text = "Received:";
            // 
            // label_NameThisPC
            // 
            this.label_NameThisPC.AutoSize = true;
            this.label_NameThisPC.Location = new System.Drawing.Point(105, 13);
            this.label_NameThisPC.Name = "label_NameThisPC";
            this.label_NameThisPC.Size = new System.Drawing.Size(13, 18);
            this.label_NameThisPC.TabIndex = 22;
            this.label_NameThisPC.Text = "-";
            // 
            // label_NameRemotePC
            // 
            this.label_NameRemotePC.AutoSize = true;
            this.label_NameRemotePC.Location = new System.Drawing.Point(768, 13);
            this.label_NameRemotePC.Name = "label_NameRemotePC";
            this.label_NameRemotePC.Size = new System.Drawing.Size(13, 18);
            this.label_NameRemotePC.TabIndex = 23;
            this.label_NameRemotePC.Text = "-";
            // 
            // label_ReceivedCount
            // 
            this.label_ReceivedCount.AutoSize = true;
            this.label_ReceivedCount.Location = new System.Drawing.Point(1019, 778);
            this.label_ReceivedCount.Name = "label_ReceivedCount";
            this.label_ReceivedCount.Size = new System.Drawing.Size(49, 18);
            this.label_ReceivedCount.TabIndex = 26;
            this.label_ReceivedCount.Text = "0 Byte";
            // 
            // label_SpeedCount
            // 
            this.label_SpeedCount.AutoSize = true;
            this.label_SpeedCount.Location = new System.Drawing.Point(826, 778);
            this.label_SpeedCount.Name = "label_SpeedCount";
            this.label_SpeedCount.Size = new System.Drawing.Size(61, 18);
            this.label_SpeedCount.TabIndex = 27;
            this.label_SpeedCount.Text = "0 Byte/s";
            // 
            // label_MustReceivedCount
            // 
            this.label_MustReceivedCount.AutoSize = true;
            this.label_MustReceivedCount.Location = new System.Drawing.Point(1129, 778);
            this.label_MustReceivedCount.Name = "label_MustReceivedCount";
            this.label_MustReceivedCount.Size = new System.Drawing.Size(49, 18);
            this.label_MustReceivedCount.TabIndex = 28;
            this.label_MustReceivedCount.Text = "0 Byte";
            // 
            // label_OR
            // 
            this.label_OR.AutoSize = true;
            this.label_OR.Location = new System.Drawing.Point(1111, 778);
            this.label_OR.Name = "label_OR";
            this.label_OR.Size = new System.Drawing.Size(12, 18);
            this.label_OR.TabIndex = 29;
            this.label_OR.Text = "/";
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(9, 778);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(54, 18);
            this.label_Status.TabIndex = 30;
            this.label_Status.Text = "Status:";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox_Status.Location = new System.Drawing.Point(69, 775);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_Status.Size = new System.Drawing.Size(333, 25);
            this.richTextBox_Status.TabIndex = 31;
            this.richTextBox_Status.Text = "";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // FileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 811);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.label_OR);
            this.Controls.Add(this.label_MustReceivedCount);
            this.Controls.Add(this.label_SpeedCount);
            this.Controls.Add(this.label_ReceivedCount);
            this.Controls.Add(this.label_NameRemotePC);
            this.Controls.Add(this.label_NameThisPC);
            this.Controls.Add(this.label_Received);
            this.Controls.Add(this.label_Speed);
            this.Controls.Add(this.button_SendFile_toThisPC);
            this.Controls.Add(this.listView_RemotePC);
            this.Controls.Add(this.button_Back_RemotePC);
            this.Controls.Add(this.button_SendFile_toRemotePC);
            this.Controls.Add(this.button_Back_ThisPC);
            this.Controls.Add(this.label_LD_RemotePC);
            this.Controls.Add(this.label_LD_ThisPC);
            this.Controls.Add(this.comboBox_LD_RemotePC);
            this.Controls.Add(this.comboBox_LD_ThisPC);
            this.Controls.Add(this.textBox_Path_RemotePC);
            this.Controls.Add(this.label_RemotePC);
            this.Controls.Add(this.label_ThisPC);
            this.Controls.Add(this.textBox_Path_ThisPC);
            this.Controls.Add(this.label_SendingProgress);
            this.Controls.Add(this.progressBar_SendingProgress);
            this.Controls.Add(this.listView_ThisPC);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "FileManager";
            this.Text = "File Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FileManager_FormClosed);
            this.Load += new System.EventHandler(this.FileManager_Load);
            this.contextMenuStrip_LV.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_ThisPC;
        private System.Windows.Forms.ProgressBar progressBar_SendingProgress;
        private System.Windows.Forms.Label label_SendingProgress;
        private System.Windows.Forms.TextBox textBox_Path_ThisPC;
        private System.Windows.Forms.Label label_ThisPC;
        private System.Windows.Forms.Label label_RemotePC;
        private System.Windows.Forms.TextBox textBox_Path_RemotePC;
        private System.Windows.Forms.ComboBox comboBox_LD_ThisPC;
        private System.Windows.Forms.ComboBox comboBox_LD_RemotePC;
        private System.Windows.Forms.Label label_LD_ThisPC;
        private System.Windows.Forms.Label label_LD_RemotePC;
        private System.Windows.Forms.Button button_Back_ThisPC;
        private System.Windows.Forms.Button button_SendFile_toRemotePC;
        private System.Windows.Forms.Button button_Back_RemotePC;
        private System.Windows.Forms.ColumnHeader columnHeader_Name;
        private System.Windows.Forms.ColumnHeader columnHeader_DateOfChange;
        private System.Windows.Forms.ColumnHeader columnHeader_Type;
        private System.Windows.Forms.ColumnHeader columnHeader_Size;
        private System.Windows.Forms.ListView listView_RemotePC;
        private System.Windows.Forms.ColumnHeader columnHeader__Name;
        private System.Windows.Forms.ColumnHeader columnHeader__DateOfChange;
        private System.Windows.Forms.ColumnHeader columnHeader__Type;
        private System.Windows.Forms.ColumnHeader columnHeader__Size;
        private System.Windows.Forms.Button button_SendFile_toThisPC;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_LV;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.Label label_Speed;
        private System.Windows.Forms.Label label_Received;
        private System.Windows.Forms.Label label_NameThisPC;
        private System.Windows.Forms.Label label_NameRemotePC;
        private System.Windows.Forms.Label label_ReceivedCount;
        private System.Windows.Forms.Label label_SpeedCount;
        private System.Windows.Forms.Label label_MustReceivedCount;
        private System.Windows.Forms.Label label_OR;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}