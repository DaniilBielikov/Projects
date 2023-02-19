namespace Client
{
    partial class MainForm
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
            this.but_SendMessege = new System.Windows.Forms.Button();
            this.but_FileManager = new System.Windows.Forms.Button();
            this.button_RemoteScreen = new System.Windows.Forms.Button();
            this.label_IP = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.button_Connect = new System.Windows.Forms.Button();
            this.label_TextMesseger = new System.Windows.Forms.Label();
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.label_Language = new System.Windows.Forms.Label();
            this.richTextBox_Messeger = new System.Windows.Forms.RichTextBox();
            this.textBox_Messege = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.button_RestartServerPC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // but_SendMessege
            // 
            this.but_SendMessege.Location = new System.Drawing.Point(695, 536);
            this.but_SendMessege.Name = "but_SendMessege";
            this.but_SendMessege.Size = new System.Drawing.Size(193, 30);
            this.but_SendMessege.TabIndex = 8;
            this.but_SendMessege.Text = "Send Messenge";
            this.but_SendMessege.UseVisualStyleBackColor = true;
            this.but_SendMessege.Click += new System.EventHandler(this.but_SendMessege_Click);
            // 
            // but_FileManager
            // 
            this.but_FileManager.Location = new System.Drawing.Point(15, 356);
            this.but_FileManager.Name = "but_FileManager";
            this.but_FileManager.Size = new System.Drawing.Size(209, 30);
            this.but_FileManager.TabIndex = 9;
            this.but_FileManager.Text = "File Manager";
            this.but_FileManager.UseVisualStyleBackColor = true;
            this.but_FileManager.Click += new System.EventHandler(this.but_FileManager_Click);
            // 
            // button_RemoteScreen
            // 
            this.button_RemoteScreen.Location = new System.Drawing.Point(15, 306);
            this.button_RemoteScreen.Name = "button_RemoteScreen";
            this.button_RemoteScreen.Size = new System.Drawing.Size(209, 30);
            this.button_RemoteScreen.TabIndex = 10;
            this.button_RemoteScreen.Text = "Remote screen";
            this.button_RemoteScreen.UseVisualStyleBackColor = true;
            this.button_RemoteScreen.Click += new System.EventHandler(this.button_RemoteScreen_Click);
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(12, 65);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(25, 18);
            this.label_IP.TabIndex = 14;
            this.label_IP.Text = "IP:";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(15, 86);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(209, 24);
            this.textBox_IP.TabIndex = 19;
            this.textBox_IP.Text = "127.0.0.1";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(15, 134);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(209, 24);
            this.textBox_Port.TabIndex = 21;
            this.textBox_Port.Text = "49152";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(12, 113);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(40, 18);
            this.label_Port.TabIndex = 20;
            this.label_Port.Text = "Port:";
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(15, 226);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(209, 30);
            this.button_Connect.TabIndex = 22;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // label_TextMesseger
            // 
            this.label_TextMesseger.AutoSize = true;
            this.label_TextMesseger.Location = new System.Drawing.Point(261, 9);
            this.label_TextMesseger.Name = "label_TextMesseger";
            this.label_TextMesseger.Size = new System.Drawing.Size(110, 18);
            this.label_TextMesseger.TabIndex = 23;
            this.label_TextMesseger.Text = "Text messeger:";
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Location = new System.Drawing.Point(15, 36);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(209, 26);
            this.comboBox_Language.TabIndex = 24;
            this.comboBox_Language.SelectedIndexChanged += new System.EventHandler(this.comboBox_Language_SelectedIndexChanged);
            // 
            // label_Language
            // 
            this.label_Language.AutoSize = true;
            this.label_Language.Location = new System.Drawing.Point(12, 15);
            this.label_Language.Name = "label_Language";
            this.label_Language.Size = new System.Drawing.Size(76, 18);
            this.label_Language.TabIndex = 25;
            this.label_Language.Text = "Language:";
            // 
            // richTextBox_Messeger
            // 
            this.richTextBox_Messeger.Location = new System.Drawing.Point(264, 30);
            this.richTextBox_Messeger.Name = "richTextBox_Messeger";
            this.richTextBox_Messeger.ReadOnly = true;
            this.richTextBox_Messeger.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_Messeger.Size = new System.Drawing.Size(624, 428);
            this.richTextBox_Messeger.TabIndex = 26;
            this.richTextBox_Messeger.Text = "";
            // 
            // textBox_Messege
            // 
            this.textBox_Messege.Location = new System.Drawing.Point(264, 464);
            this.textBox_Messege.Multiline = true;
            this.textBox_Messege.Name = "textBox_Messege";
            this.textBox_Messege.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Messege.Size = new System.Drawing.Size(624, 66);
            this.textBox_Messege.TabIndex = 27;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(15, 182);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(209, 24);
            this.textBox_Password.TabIndex = 28;
            this.textBox_Password.Text = "1234567890";
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(12, 161);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(79, 18);
            this.label_Password.TabIndex = 29;
            this.label_Password.Text = "Password:";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Location = new System.Drawing.Point(15, 576);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ReadOnly = true;
            this.richTextBox_Status.Size = new System.Drawing.Size(873, 50);
            this.richTextBox_Status.TabIndex = 30;
            this.richTextBox_Status.Text = "";
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 555);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(54, 18);
            this.label_Status.TabIndex = 31;
            this.label_Status.Text = "Status:";
            // 
            // button_RestartServerPC
            // 
            this.button_RestartServerPC.Location = new System.Drawing.Point(15, 464);
            this.button_RestartServerPC.Name = "button_RestartServerPC";
            this.button_RestartServerPC.Size = new System.Drawing.Size(209, 30);
            this.button_RestartServerPC.TabIndex = 32;
            this.button_RestartServerPC.Text = "Restart Server PC";
            this.button_RestartServerPC.UseVisualStyleBackColor = true;
            this.button_RestartServerPC.Click += new System.EventHandler(this.button_RestartServerPC_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 638);
            this.Controls.Add(this.button_RestartServerPC);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.label_Password);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.textBox_Messege);
            this.Controls.Add(this.richTextBox_Messeger);
            this.Controls.Add(this.label_Language);
            this.Controls.Add(this.comboBox_Language);
            this.Controls.Add(this.label_TextMesseger);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.button_RemoteScreen);
            this.Controls.Add(this.but_FileManager);
            this.Controls.Add(this.but_SendMessege);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button but_SendMessege;
        private System.Windows.Forms.Button but_FileManager;
        private System.Windows.Forms.Button button_RemoteScreen;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Label label_TextMesseger;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.Label label_Language;
        private System.Windows.Forms.RichTextBox richTextBox_Messeger;
        private System.Windows.Forms.TextBox textBox_Messege;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button_RestartServerPC;
    }
}

