namespace Server
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
            this.components = new System.ComponentModel.Container();
            this.label_Password = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.richTextBox_Messeger = new System.Windows.Forms.RichTextBox();
            this.but_SendMessege = new System.Windows.Forms.Button();
            this.button_Start_Stop_Listen = new System.Windows.Forms.Button();
            this.label_Port = new System.Windows.Forms.Label();
            this.label_Language = new System.Windows.Forms.Label();
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label_TextMesseger = new System.Windows.Forms.Label();
            this.textBox_Messege = new System.Windows.Forms.TextBox();
            this.timer_RemoteScreen = new System.Windows.Forms.Timer(this.components);
            this.label_Status = new System.Windows.Forms.Label();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(12, 113);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(79, 18);
            this.label_Password.TabIndex = 0;
            this.label_Password.Text = "Password:";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(15, 134);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(209, 24);
            this.textBox_Password.TabIndex = 1;
            this.textBox_Password.Text = "1234567890";
            // 
            // richTextBox_Messeger
            // 
            this.richTextBox_Messeger.Location = new System.Drawing.Point(264, 30);
            this.richTextBox_Messeger.Name = "richTextBox_Messeger";
            this.richTextBox_Messeger.ReadOnly = true;
            this.richTextBox_Messeger.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_Messeger.Size = new System.Drawing.Size(624, 428);
            this.richTextBox_Messeger.TabIndex = 2;
            this.richTextBox_Messeger.Text = "";
            // 
            // but_SendMessege
            // 
            this.but_SendMessege.Location = new System.Drawing.Point(695, 536);
            this.but_SendMessege.Name = "but_SendMessege";
            this.but_SendMessege.Size = new System.Drawing.Size(193, 30);
            this.but_SendMessege.TabIndex = 3;
            this.but_SendMessege.Text = "Send Messege";
            this.but_SendMessege.UseVisualStyleBackColor = true;
            this.but_SendMessege.Click += new System.EventHandler(this.but_SendMessege_Click);
            // 
            // button_Start_Stop_Listen
            // 
            this.button_Start_Stop_Listen.Location = new System.Drawing.Point(15, 175);
            this.button_Start_Stop_Listen.Name = "button_Start_Stop_Listen";
            this.button_Start_Stop_Listen.Size = new System.Drawing.Size(209, 30);
            this.button_Start_Stop_Listen.TabIndex = 4;
            this.button_Start_Stop_Listen.Text = "Stop";
            this.button_Start_Stop_Listen.UseVisualStyleBackColor = true;
            this.button_Start_Stop_Listen.Click += new System.EventHandler(this.button_Start_Stop_Listen_Click);
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(12, 65);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(40, 18);
            this.label_Port.TabIndex = 5;
            this.label_Port.Text = "Port:";
            // 
            // label_Language
            // 
            this.label_Language.AutoSize = true;
            this.label_Language.Location = new System.Drawing.Point(12, 15);
            this.label_Language.Name = "label_Language";
            this.label_Language.Size = new System.Drawing.Size(76, 18);
            this.label_Language.TabIndex = 6;
            this.label_Language.Text = "Language:";
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Location = new System.Drawing.Point(15, 36);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(209, 26);
            this.comboBox_Language.TabIndex = 7;
            this.comboBox_Language.SelectedIndexChanged += new System.EventHandler(this.comboBox_Language_SelectedIndexChanged);
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(15, 86);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(209, 24);
            this.textBox_Port.TabIndex = 8;
            this.textBox_Port.Text = "49152";
            // 
            // label_TextMesseger
            // 
            this.label_TextMesseger.AutoSize = true;
            this.label_TextMesseger.Location = new System.Drawing.Point(261, 9);
            this.label_TextMesseger.Name = "label_TextMesseger";
            this.label_TextMesseger.Size = new System.Drawing.Size(110, 18);
            this.label_TextMesseger.TabIndex = 9;
            this.label_TextMesseger.Text = "Text messeger:";
            // 
            // textBox_Messege
            // 
            this.textBox_Messege.Location = new System.Drawing.Point(264, 464);
            this.textBox_Messege.Multiline = true;
            this.textBox_Messege.Name = "textBox_Messege";
            this.textBox_Messege.Size = new System.Drawing.Size(624, 66);
            this.textBox_Messege.TabIndex = 10;
            // 
            // timer_RemoteScreen
            // 
            this.timer_RemoteScreen.Interval = 17;
            this.timer_RemoteScreen.Tick += new System.EventHandler(this.timer_RemoteScreen_Tick);
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 555);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(54, 18);
            this.label_Status.TabIndex = 11;
            this.label_Status.Text = "Status:";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Location = new System.Drawing.Point(12, 576);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ReadOnly = true;
            this.richTextBox_Status.Size = new System.Drawing.Size(876, 50);
            this.richTextBox_Status.TabIndex = 12;
            this.richTextBox_Status.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 638);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox_Messege);
            this.Controls.Add(this.label_TextMesseger);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.comboBox_Language);
            this.Controls.Add(this.label_Language);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.button_Start_Stop_Listen);
            this.Controls.Add(this.but_SendMessege);
            this.Controls.Add(this.richTextBox_Messeger);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.label_Password);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.RichTextBox richTextBox_Messeger;
        private System.Windows.Forms.Button but_SendMessege;
        private System.Windows.Forms.Button button_Start_Stop_Listen;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.Label label_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label_TextMesseger;
        private System.Windows.Forms.TextBox textBox_Messege;
        private System.Windows.Forms.Timer timer_RemoteScreen;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
    }
}

