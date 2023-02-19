using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class MainForm : Form
    {
        public event EventHandler MainFormClosed;
        public event EventHandler Start_Stop_Listen;
        public event EventHandler SendTextMessage;
        public event EventHandler SendRemoteScreenImage;

        int NumOfSelectLanguage;
        bool MakeCaptureScreen = false;

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainFormClosed != null) MainFormClosed(this, EventArgs.Empty);
        }
        private void comboBox_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumOfSelectLanguage = comboBox_Language.SelectedIndex;
        }
        public void SetComboBox_Language(List<string> language)
        {
            comboBox_Language.Items.AddRange(language.ToArray());
            comboBox_Language.SelectedIndex = 0;
        }
        public string GetPassword()
        {
            return textBox_Password.Text;
        }
        public void SetPassword(string _Password)
        {
            textBox_Password.Text = _Password;
        }
        public string GetPort()
        {
            return textBox_Port.Text;
        }
        public void SetPort(string _Port)
        {
            textBox_Port.Text = _Port;
        }
        private void button_Start_Stop_Listen_Click(object sender, EventArgs e)
        {
            if (Start_Stop_Listen != null) Start_Stop_Listen(this, EventArgs.Empty);
        }
        private void but_SendMessege_Click(object sender, EventArgs e)
        {
            if (SendTextMessage != null) SendTextMessage(this, EventArgs.Empty);
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
            if (IsThisPC == true)
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
        public void SetLanguage(string _formName, string _label_TextMesseger, string _label_Language, string _button_Start_Stop_Listen_Text,
            string _but_SendMessege_Text, string _label_Password, string _label_Port, string _label_Status)
        {
            this.Text = _formName;
            this.label_TextMesseger.Text = _label_TextMesseger;
            this.label_Language.Text = _label_Language;
            this.button_Start_Stop_Listen.Text = _button_Start_Stop_Listen_Text;
            this.but_SendMessege.Text = _but_SendMessege_Text;
            this.label_Password.Text = _label_Password;
            this.label_Port.Text = _label_Port;
            this.label_Status.Text = _label_Status;
        }
        public void Set_Start_Stop_Listen_ButtonText(string text)
        {
            this.button_Start_Stop_Listen.Text = text;
        }
        public string Get_Start_Stop_Listen_ButtonText()
        {
            return this.button_Start_Stop_Listen.Text;
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
        private void timer_RemoteScreen_Tick(object sender, EventArgs e)
        {
            MakeCaptureScreen = true;
        }
        public void Set_RemoteScreenTimer_interval(int _interval)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    timer_RemoteScreen.Interval = _interval;
                });
            }
            else
            {
                timer_RemoteScreen.Interval = _interval;
            }
        }
        public void Start_RemoteScreenTimer()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    timer_RemoteScreen.Start();
                });
            }
            else
            {
                timer_RemoteScreen.Start();
            }
        }
        public void Stop_RemoteScreenTimer()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    timer_RemoteScreen.Stop();
                });
            }
            else
            {
                timer_RemoteScreen.Stop();
            }
        }
        public void CaptureScreen()
        {
            while (true)
            {
                if (MakeCaptureScreen == true)
                {
                    Bitmap bmp;
                    Graphics gr;
                    bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    gr = Graphics.FromImage(bmp);
                    gr.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                    if (SendRemoteScreenImage != null) SendRemoteScreenImage(bmp, EventArgs.Empty);
                    MakeCaptureScreen = false;
                    //bmp.Dispose();
                    //gr.Dispose();
                }
            }
        }
    }
}
