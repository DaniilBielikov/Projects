using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        public event EventHandler Connect;
        public event EventHandler SendTextMessage;
        public event EventHandler OpenFileManeger;
        public event EventHandler OpenRemoteScreen;
        public event EventHandler MainFormClosed;
        public event EventHandler RestartRemotePC;

        public int NumOfSelectLanguage;

        public MainForm()
        {
            InitializeComponent();
        }      
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainFormClosed != null) MainFormClosed(this, EventArgs.Empty);
        }
        private void but_SendMessege_Click(object sender, EventArgs e)
        {
            if (SendTextMessage != null) SendTextMessage(this, EventArgs.Empty);
        }
        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (Connect != null) Connect(this, EventArgs.Empty);
        }
        private void button_RemoteScreen_Click(object sender, EventArgs e)
        {
            if (OpenRemoteScreen != null) OpenRemoteScreen(this, EventArgs.Empty);
        }
        private void but_FileManager_Click(object sender, EventArgs e)
        {
            if (OpenFileManeger != null) OpenFileManeger(this, EventArgs.Empty);
        }
        public string GetIp()
        {
            return textBox_IP.Text;
        }
        public void SetIp(string _Ip)
        {
            textBox_IP.Text = _Ip;
        }
        public string GetPort()
        {
            return textBox_Port.Text;
        }
        public void SetPort(string _Port)
        {
            textBox_Port.Text = _Port;
        }
        public string GetPassword()
        {
            return textBox_Password.Text;
        }
        public void SetPassword(string _Password)
        {
            textBox_Password.Text = _Password;
        }
        public string Get_TextMessenge()
        {
            string messenge = textBox_Messege.Text;
            textBox_Messege.Clear();
            return messenge;
        }
        public void TextMessenger_AllMessenges(string timeOrName, string messege, bool IsThisPC)
        {
            Color color;
            if(IsThisPC == true)
            {
                color = Color.RoyalBlue;
            }
            else
            {
                color = Color.DarkViolet;
            }

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() =>
                {
                    richTextBox_Messeger.SelectionColor = color;
                    richTextBox_Messeger.AppendText(timeOrName);
                    richTextBox_Messeger.SelectionColor = Color.Black;
                    richTextBox_Messeger.AppendText(messege);
                    richTextBox_Messeger.AppendText(System.Environment.NewLine);
                }));
            }
            else
            {
                richTextBox_Messeger.SelectionColor = color;
                richTextBox_Messeger.AppendText(timeOrName);
                richTextBox_Messeger.SelectionColor = Color.Black;
                richTextBox_Messeger.AppendText(messege);
                richTextBox_Messeger.AppendText(System.Environment.NewLine);
            }
        }
        public void SetComboBox_Language(List<string> language)
        {
            comboBox_Language.Items.AddRange(language.ToArray());
            comboBox_Language.SelectedIndex = 0;
        }
        public void SetLanguage(string _formName, string _label_TextMesseger, string _label_Language, string _button_Connect_Text,
            string _button_RemoteScreen_Text, string _but_FileManager_Text, string _but_SendMessege_Text, string _label_Password,
            string _label_Port, string _label_Status)
        {
            this.Text = _formName;
            this.label_TextMesseger.Text = _label_TextMesseger;
            this.label_Language.Text = _label_Language;
            this.button_Connect.Text = _button_Connect_Text;
            this.button_RemoteScreen.Text = _button_RemoteScreen_Text;
            this.but_FileManager.Text = _but_FileManager_Text;
            this.but_SendMessege.Text = _but_SendMessege_Text;
            this.label_Password.Text = _label_Password;
            this.label_Port.Text = _label_Port;
            this.label_Status.Text = _label_Status;
        }
        public void SetConnectButtonText(string text)
        {
            this.button_Connect.Text = text;
        }
        public string GetConnectButtonText()
        {
            return this.button_Connect.Text;
        }
        private void comboBox_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumOfSelectLanguage = comboBox_Language.SelectedIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Текст сообщения.</param>
        /// <param name="ColorNum">Цвет текста. 0 - черный, 1 - зеленый, 2 - красный, 3 - RoyalBlue</param>
        public void Messege_Status(string text, int ColorNum)
        {
            string Time = DateTime.Now.ToLongTimeString();
            Color color = Color.Black;
            if (ColorNum == 0)
            {
                color = Color.Black;
            }
            if (ColorNum == 1)
            {
                color = Color.Green;
            }
            if (ColorNum == 2)
            {
                color = Color.Red;
            }
            if (ColorNum == 3)
            {
                color = Color.RoyalBlue;
            }
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    //richTextBox_Status.Clear();
                    richTextBox_Status.SelectionColor = Color.Black;
                    richTextBox_Status.AppendText(Time + " :: ");
                    richTextBox_Status.SelectionColor = color;
                    richTextBox_Status.AppendText(text);
                    richTextBox_Status.AppendText(System.Environment.NewLine);
                });
            }
            else
            {
                //richTextBox_Status.Clear();
                richTextBox_Status.SelectionColor = Color.Black;
                richTextBox_Status.AppendText(Time + " :: ");
                richTextBox_Status.SelectionColor = color;
                richTextBox_Status.AppendText(text);
                richTextBox_Status.AppendText(System.Environment.NewLine);
            }
        }

        private void button_RestartServerPC_Click(object sender, EventArgs e)
        {
            if (RestartRemotePC != null) RestartRemotePC(this, EventArgs.Empty);
        }
    }
}
