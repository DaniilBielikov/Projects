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
    public partial class RemoteScreen : Form
    {
        public event EventHandler RemoteScreenFormClosed;

        public RemoteScreen()
        {
            InitializeComponent();
        }
        public Image SG_pictureBox_Screen
        {
            set
            {
                if (pictureBox_Screen.IsHandleCreated == true && this.IsHandleCreated == true)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            pictureBox_Screen.Image = value;
                        });
                    }
                    else
                    {
                        pictureBox_Screen.Image = value;
                    }
                }
            }
            get { return pictureBox_Screen.Image; }
        }
        public void SetLanguage(string _formName)
        {
            this.Text = _formName;
        }

        private void RemoteScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RemoteScreenFormClosed != null) RemoteScreenFormClosed(this, EventArgs.Empty);
        }
    }
}
