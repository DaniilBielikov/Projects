namespace Segmentation_and_other
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.butEnterSlovoIDZ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lvFileManager = new System.Windows.Forms.ListView();
            this.NameObj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TypeObj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txSlovo = new System.Windows.Forms.TextBox();
            this.butBackwardFileManager = new System.Windows.Forms.Button();
            this.butForwardFileManager = new System.Windows.Forms.Button();
            this.txTotalPath = new System.Windows.Forms.TextBox();
            this.butEnterPute = new System.Windows.Forms.Button();
            this.progressBarOperation = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lvLogicalDisks = new System.Windows.Forms.ListView();
            this.NameLD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // butEnterSlovoIDZ
            // 
            this.butEnterSlovoIDZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butEnterSlovoIDZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butEnterSlovoIDZ.Location = new System.Drawing.Point(628, 21);
            this.butEnterSlovoIDZ.Name = "butEnterSlovoIDZ";
            this.butEnterSlovoIDZ.Size = new System.Drawing.Size(100, 72);
            this.butEnterSlovoIDZ.TabIndex = 2;
            this.butEnterSlovoIDZ.Text = "Поиск";
            this.butEnterSlovoIDZ.UseVisualStyleBackColor = true;
            this.butEnterSlovoIDZ.Click += new System.EventHandler(this.butEnterSlovoIDZ_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Введите слово для поиска:";
            // 
            // lvFileManager
            // 
            this.lvFileManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileManager.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameObj,
            this.TypeObj,
            this.SizeFile});
            this.lvFileManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvFileManager.FullRowSelect = true;
            this.lvFileManager.Location = new System.Drawing.Point(178, 160);
            this.lvFileManager.MultiSelect = false;
            this.lvFileManager.Name = "lvFileManager";
            this.lvFileManager.Size = new System.Drawing.Size(550, 545);
            this.lvFileManager.TabIndex = 4;
            this.lvFileManager.UseCompatibleStateImageBehavior = false;
            this.lvFileManager.View = System.Windows.Forms.View.Details;
            // 
            // NameObj
            // 
            this.NameObj.Text = "Имя";
            this.NameObj.Width = 250;
            // 
            // TypeObj
            // 
            this.TypeObj.Text = "Тип";
            this.TypeObj.Width = 150;
            // 
            // SizeFile
            // 
            this.SizeFile.Text = "Размер";
            this.SizeFile.Width = 150;
            // 
            // txSlovo
            // 
            this.txSlovo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txSlovo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txSlovo.Location = new System.Drawing.Point(260, 21);
            this.txSlovo.Name = "txSlovo";
            this.txSlovo.Size = new System.Drawing.Size(356, 26);
            this.txSlovo.TabIndex = 5;
            // 
            // butBackwardFileManager
            // 
            this.butBackwardFileManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butBackwardFileManager.Image = ((System.Drawing.Image)(resources.GetObject("butBackwardFileManager.Image")));
            this.butBackwardFileManager.Location = new System.Drawing.Point(12, 114);
            this.butBackwardFileManager.Name = "butBackwardFileManager";
            this.butBackwardFileManager.Size = new System.Drawing.Size(60, 40);
            this.butBackwardFileManager.TabIndex = 6;
            this.butBackwardFileManager.UseVisualStyleBackColor = true;
            this.butBackwardFileManager.Click += new System.EventHandler(this.butBackwardFileManager_Click);
            // 
            // butForwardFileManager
            // 
            this.butForwardFileManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butForwardFileManager.Image = ((System.Drawing.Image)(resources.GetObject("butForwardFileManager.Image")));
            this.butForwardFileManager.Location = new System.Drawing.Point(78, 114);
            this.butForwardFileManager.Name = "butForwardFileManager";
            this.butForwardFileManager.Size = new System.Drawing.Size(60, 40);
            this.butForwardFileManager.TabIndex = 7;
            this.butForwardFileManager.UseVisualStyleBackColor = true;
            this.butForwardFileManager.Click += new System.EventHandler(this.butForwardFileManager_Click);
            // 
            // txTotalPath
            // 
            this.txTotalPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txTotalPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txTotalPath.Location = new System.Drawing.Point(144, 121);
            this.txTotalPath.Name = "txTotalPath";
            this.txTotalPath.Size = new System.Drawing.Size(472, 26);
            this.txTotalPath.TabIndex = 8;
            // 
            // butEnterPute
            // 
            this.butEnterPute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butEnterPute.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butEnterPute.Location = new System.Drawing.Point(628, 114);
            this.butEnterPute.Name = "butEnterPute";
            this.butEnterPute.Size = new System.Drawing.Size(100, 40);
            this.butEnterPute.TabIndex = 9;
            this.butEnterPute.Text = "Перейти";
            this.butEnterPute.UseVisualStyleBackColor = true;
            this.butEnterPute.Click += new System.EventHandler(this.butEnterPute_Click);
            // 
            // progressBarOperation
            // 
            this.progressBarOperation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarOperation.Location = new System.Drawing.Point(16, 711);
            this.progressBarOperation.Maximum = 1000000;
            this.progressBarOperation.Name = "progressBarOperation";
            this.progressBarOperation.Size = new System.Drawing.Size(716, 30);
            this.progressBarOperation.TabIndex = 10;
            // 
            // lvLogicalDisks
            // 
            this.lvLogicalDisks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvLogicalDisks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameLD});
            this.lvLogicalDisks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvLogicalDisks.FullRowSelect = true;
            this.lvLogicalDisks.Location = new System.Drawing.Point(12, 160);
            this.lvLogicalDisks.MultiSelect = false;
            this.lvLogicalDisks.Name = "lvLogicalDisks";
            this.lvLogicalDisks.Size = new System.Drawing.Size(160, 545);
            this.lvLogicalDisks.TabIndex = 11;
            this.lvLogicalDisks.UseCompatibleStateImageBehavior = false;
            this.lvLogicalDisks.View = System.Windows.Forms.View.Details;
            // 
            // NameLD
            // 
            this.NameLD.Text = "Локальный диск";
            this.NameLD.Width = 170;
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEncoding.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Location = new System.Drawing.Point(272, 65);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(344, 28);
            this.comboBoxEncoding.TabIndex = 12;
            this.comboBoxEncoding.SelectedIndexChanged += new System.EventHandler(this.comboBoxEncoding_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Выберите кодировку текста:";
            // 
            // toolTip1
            // 
            this.toolTip1.Tag = "";
           
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 753);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxEncoding);
            this.Controls.Add(this.lvLogicalDisks);
            this.Controls.Add(this.progressBarOperation);
            this.Controls.Add(this.butEnterPute);
            this.Controls.Add(this.txTotalPath);
            this.Controls.Add(this.butForwardFileManager);
            this.Controls.Add(this.butBackwardFileManager);
            this.Controls.Add(this.txSlovo);
            this.Controls.Add(this.lvFileManager);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butEnterSlovoIDZ);
            this.MinimumSize = new System.Drawing.Size(550, 500);
            this.Name = "Form1";
            this.Text = "File Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button butEnterSlovoIDZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvFileManager;
        private System.Windows.Forms.TextBox txSlovo;
        private System.Windows.Forms.Button butBackwardFileManager;
        private System.Windows.Forms.Button butForwardFileManager;
        private System.Windows.Forms.TextBox txTotalPath;
        private System.Windows.Forms.ColumnHeader NameObj;
        private System.Windows.Forms.ColumnHeader TypeObj;
        private System.Windows.Forms.ColumnHeader SizeFile;
        private System.Windows.Forms.Button butEnterPute;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView lvLogicalDisks;
        private System.Windows.Forms.ColumnHeader NameLD;
        private System.Windows.Forms.ProgressBar progressBarOperation;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

