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
    public partial class FileManager : Form
    {
        public event EventHandler FileManagerFormClosed;
        public event EventHandler FileManagerFormLoad;

        public event EventHandler SelectedLD_ThisPC;
        public event EventHandler Back_ThisPC;
        public event EventHandler SendFile_toThisPC;
        public event EventHandler MouseDoubleClick_ThisPC;
        public event EventHandler PropertiesFmObjClick_ThisPC;
        public event EventHandler FmLd_RightClick_ThisPC;
        public event EventHandler CancelSendingToThisPC;
        public event EventHandler FmDelete_RightClick_ThisPC;

        public event EventHandler SelectedLD_RemotePC;
        public event EventHandler Back_RemotePC;
        public event EventHandler SendFile_toRemotePC;
        public event EventHandler MouseDoubleClick_RemotePC;
        public event EventHandler PropertiesFmObjClick_RemotePC;
        public event EventHandler FmLd_RightClick_RemotePC;
        public event EventHandler CancelSendingToRemotePC;
        public event EventHandler FmDelete_RightClick_RemotePC;

        bool IsThisPC_RightClick;
        bool IsClickSendingToThisPC;
        bool IsFirstSetComboBoxThisPC = true;
        bool IsFirstSetComboBoxRemotePC = true;
        bool IsFirstSelectComboBoxThisPC = true;
        bool IsFirstSelectComboBoxRemotePC = true;
        int ComboBoxThisPC_Index = 0;
        int ComboBoxRemotePC_Index = 0;


        public FileManager()
        {
            InitializeComponent();
        }
        private void FileManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FileManagerFormClosed != null) FileManagerFormClosed(this, EventArgs.Empty);
        }
        private void FileManager_Load(object sender, EventArgs e)
        {
            if (FileManagerFormLoad != null) FileManagerFormLoad(this, EventArgs.Empty);
        }
        public void Set_progressBar_SendingProgress(int minVal, int maxVal, int step)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar_SendingProgress.Minimum = minVal;
                    progressBar_SendingProgress.Maximum = maxVal;
                    progressBar_SendingProgress.Step = step;
                });
            }
            else
            {
                progressBar_SendingProgress.Minimum = minVal;
                progressBar_SendingProgress.Maximum = maxVal;
                progressBar_SendingProgress.Step = step;
            } 
        }
        public void Change_progressBar_SendingProgress()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if ((progressBar_SendingProgress.Value + progressBar_SendingProgress.Step) <= progressBar_SendingProgress.Maximum)
                    {
                        progressBar_SendingProgress.Value = progressBar_SendingProgress.Value + progressBar_SendingProgress.Step;
                    }
                });
            }
            else
            {
                if ((progressBar_SendingProgress.Value + progressBar_SendingProgress.Step) <= progressBar_SendingProgress.Maximum)
                {
                    progressBar_SendingProgress.Value = progressBar_SendingProgress.Value + progressBar_SendingProgress.Step;
                }
            }
        }
        public void Reset_progressBar_SendingProgress()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar_SendingProgress.Value = 0;
                });
            }
            else
            {
                progressBar_SendingProgress.Value = 0;
            }
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsThisPC_RightClick == true)
            {
                if (PropertiesFmObjClick_ThisPC != null) PropertiesFmObjClick_ThisPC(listView_ThisPC.SelectedItems[0].Index, EventArgs.Empty);
            }
            else
            {
                if (PropertiesFmObjClick_RemotePC != null) PropertiesFmObjClick_RemotePC(listView_RemotePC.SelectedItems[0].Index, EventArgs.Empty);
            }
        }
        public void SetLanguage(string _formName, string _label_SendingProgress, string _label_ThisPC, string _label_LD, 
            string _label_RemotePC, string _button_SendFile, string _listView_NameObj, string _listView_DateOfChangeObj,
            string _listView_TypeObj, string _listView_SizeObj, string _label_Received, string _label_Speed, string _button_CancelSending,
            string _label_Status)
        {
            this.Text = _formName;
            this.label_SendingProgress.Text = _label_SendingProgress;
            this.label_ThisPC.Text = _label_ThisPC;
            this.label_LD_ThisPC.Text = _label_LD;
            this.label_LD_RemotePC.Text = _label_LD;
            this.label_RemotePC.Text = _label_RemotePC;
            this.button_SendFile_toRemotePC.Text = _button_SendFile;
            this.button_SendFile_toThisPC.Text = _button_SendFile;
            this.listView_RemotePC.Columns[0].Text = _listView_NameObj;
            this.listView_RemotePC.Columns[1].Text = _listView_DateOfChangeObj;
            this.listView_RemotePC.Columns[2].Text = _listView_TypeObj;
            this.listView_RemotePC.Columns[3].Text = _listView_SizeObj;
            this.listView_ThisPC.Columns[0].Text = _listView_NameObj;
            this.listView_ThisPC.Columns[1].Text = _listView_DateOfChangeObj;
            this.listView_ThisPC.Columns[2].Text = _listView_TypeObj;
            this.listView_ThisPC.Columns[3].Text = _listView_SizeObj;
            this.label_Received.Text = _label_Received;
            this.label_Speed.Text = _label_Speed;
            //this.button_CancelSending.Text = _button_CancelSending;
            this.label_Status.Text = _label_Status;
        }
        public void Set_HowMuchIsReceived(string _Received)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_ReceivedCount.Text = _Received;
                });
            }
            else
            {
                label_ReceivedCount.Text = _Received;
            }
        }
        public void Set_HowMuchMustReceived(string _Received)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_MustReceivedCount.Text = _Received;
                });
            }
            else
            {
                label_MustReceivedCount.Text = _Received;
            }
        }
        public void Set_ReceivingSpeed(string _Speed)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_SpeedCount.Text = _Speed;
                });
            }
            else
            {
                label_SpeedCount.Text = _Speed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Текст сообщения.</param>
        /// <param name="ColorNum">Цвет текста. 0 - черный, 1 - зеленый, 2 - красный, 3 - желтый</param>
        public void Messege_Status(string text, int ColorNum)
        {
            Color color = Color.Black;
            if (ColorNum == 0)
            {
                color = Color.Black;
            }
            if (ColorNum == 1)
            {
                color = Color.Green;
            }
            if(ColorNum == 2)
            {
                color = Color.Red;
            }
            if (ColorNum == 3)
            {
                color = Color.Yellow;
            }
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    richTextBox_Status.Clear();
                    richTextBox_Status.SelectionColor = color;
                    richTextBox_Status.AppendText(text);
                });
            }
            else
            {
                richTextBox_Status.Clear();
                richTextBox_Status.SelectionColor = color;
                richTextBox_Status.AppendText(text);
            }
        }

        //ДОПИСАТЬ отображение инфы о логич дисках и объектах ФС через messege box

        #region ThisPC
        public void SetComboBox_LD_ThisPC(List<string> LogicalDisks)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (IsFirstSetComboBoxThisPC == true)
                    {
                        comboBox_LD_ThisPC.Items.AddRange(LogicalDisks.ToArray());
                        comboBox_LD_ThisPC.SelectedIndex = 0;
                        IsFirstSetComboBoxThisPC = false;
                    }
                    else
                    {
                        IsFirstSelectComboBoxThisPC = true;
                        ComboBoxThisPC_Index = comboBox_LD_ThisPC.SelectedIndex;
                        comboBox_LD_ThisPC.Items.Clear();
                        comboBox_LD_ThisPC.Items.AddRange(LogicalDisks.ToArray());
                        comboBox_LD_ThisPC.SelectedIndex = ComboBoxThisPC_Index;
                    }
                });
            }
            else
            {
                if (IsFirstSetComboBoxThisPC == true)
                {
                    comboBox_LD_ThisPC.Items.AddRange(LogicalDisks.ToArray());
                    comboBox_LD_ThisPC.SelectedIndex = 0;
                    IsFirstSetComboBoxThisPC = false;
                }
                else
                {
                    IsFirstSelectComboBoxThisPC = true;
                    ComboBoxThisPC_Index = comboBox_LD_ThisPC.SelectedIndex;
                    comboBox_LD_ThisPC.Items.Clear();
                    comboBox_LD_ThisPC.Items.AddRange(LogicalDisks.ToArray());
                    comboBox_LD_ThisPC.SelectedIndex = ComboBoxThisPC_Index;
                }
            }
        }
        private void comboBox_LD_ThisPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsFirstSelectComboBoxThisPC == true)
            {
                IsFirstSelectComboBoxThisPC = false;
            }
            else
            {
                if (SelectedLD_ThisPC != null) SelectedLD_ThisPC(comboBox_LD_ThisPC.SelectedIndex, EventArgs.Empty);
            }
        }
        public void Set_textBox_Path_ThisPC(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    textBox_Path_ThisPC.Text = path;
                });
            }
            else
            {
                textBox_Path_ThisPC.Text = path;
            }
        }
        private void button_Back_ThisPC_Click(object sender, EventArgs e)
        {
            if (Back_ThisPC != null) Back_ThisPC(this, EventArgs.Empty);
        }
        public void Set_listView_ThisPC(List<string> Names, List<string> DateOfChange, List<string> Type, List<string> Size)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (Names.Count() == DateOfChange.Count() && Names.Count() == Type.Count() && Names.Count() == Size.Count())
                    {
                        listView_ThisPC.Items.Clear();
                        ListViewItem listViewItem_ThisPC;
                        for (int i = 0; i < Names.Count(); i++)
                        {
                            listViewItem_ThisPC = new ListViewItem();
                            listViewItem_ThisPC.Text = Names[i];
                            listViewItem_ThisPC.SubItems.Add(DateOfChange[i]);
                            listViewItem_ThisPC.SubItems.Add(Type[i]);
                            listViewItem_ThisPC.SubItems.Add(Size[i]);
                            listView_ThisPC.Items.Add(listViewItem_ThisPC);
                        }
                    }
                });
            }
            else
            {
                if (Names.Count() == DateOfChange.Count() && Names.Count() == Type.Count() && Names.Count() == Size.Count())
                {
                    listView_ThisPC.Items.Clear();
                    ListViewItem listViewItem_ThisPC;
                    for (int i = 0; i < Names.Count(); i++)
                    {
                        listViewItem_ThisPC = new ListViewItem();
                        listViewItem_ThisPC.Text = Names[i];
                        listViewItem_ThisPC.SubItems.Add(DateOfChange[i]);
                        listViewItem_ThisPC.SubItems.Add(Type[i]);
                        listViewItem_ThisPC.SubItems.Add(Size[i]);
                        listView_ThisPC.Items.Add(listViewItem_ThisPC);
                    }
                }
            }  
        }
        private void button_SendFile_toThisPC_Click(object sender, EventArgs e)
        {
            if (SendFile_toThisPC != null) SendFile_toThisPC(listView_RemotePC.SelectedItems[0].Index, EventArgs.Empty);
            IsClickSendingToThisPC = true;
        }//индекс объекта из удаленного пк для передачи этому
        private void listView_ThisPC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseDoubleClick_ThisPC != null) MouseDoubleClick_ThisPC(listView_ThisPC.SelectedItems[0].Index, EventArgs.Empty);
        }
        private void listView_ThisPC_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView_ThisPC.FocusedItem.Bounds.Contains(e.Location))
                {
                    IsThisPC_RightClick = true;
                    contextMenuStrip_LV.Show(Cursor.Position);
                }
            }
        }//ПКМ Obj
        public void Set_ThisPC_Name(string _name)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_NameThisPC.Text = _name;
                });
            }
            else
            {
                label_NameThisPC.Text = _name;
            }
        }
        #endregion

        #region RemotePC
        public void SetComboBox_LD_RemotePC(List<string> LogicalDisks)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (IsFirstSetComboBoxRemotePC == true)
                    {
                        comboBox_LD_RemotePC.Items.AddRange(LogicalDisks.ToArray());
                        comboBox_LD_RemotePC.SelectedIndex = 0;
                        IsFirstSetComboBoxRemotePC = false;
                    }
                    else
                    {
                        comboBox_LD_RemotePC.Items.Clear();
                        comboBox_LD_RemotePC.Items.AddRange(LogicalDisks.ToArray());
                    }
                });
            }
            else
            {
                if (IsFirstSetComboBoxRemotePC == true)
                {
                    comboBox_LD_RemotePC.Items.AddRange(LogicalDisks.ToArray());
                    comboBox_LD_RemotePC.SelectedIndex = 0;
                    IsFirstSetComboBoxRemotePC = false;
                }
                else
                {
                    comboBox_LD_RemotePC.Items.Clear();
                    comboBox_LD_RemotePC.Items.AddRange(LogicalDisks.ToArray());
                }
            }
        }
        private void comboBox_LD_RemotePC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsFirstSelectComboBoxRemotePC == true)
            {
                IsFirstSelectComboBoxRemotePC = false;
            }
            else
            {
                if (SelectedLD_RemotePC != null) SelectedLD_RemotePC(comboBox_LD_RemotePC.SelectedIndex, EventArgs.Empty);
            }
        }
        public void Set_textBox_Path_RemotePC(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    textBox_Path_RemotePC.Text = path;
                });
            }
            else
            {
                textBox_Path_RemotePC.Text = path;
            }
        }
        private void button_Back_RemotePC_Click(object sender, EventArgs e)
        {
            if (Back_RemotePC != null) Back_RemotePC(this, EventArgs.Empty);
        }
        public void Set_listView_RemotePC(List<string> Names, List<string> DateOfChange, List<string> Type, List<string> Size)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (Names.Count() == DateOfChange.Count() && Names.Count() == Type.Count() && Names.Count() == Size.Count())
                    {

                        listView_RemotePC.Items.Clear();
                        ListViewItem listViewItem_ThisPC;
                        for (int i = 0; i < Names.Count(); i++)
                        {
                            listViewItem_ThisPC = new ListViewItem();
                            listViewItem_ThisPC.Text = Names[i];
                            listViewItem_ThisPC.SubItems.Add(DateOfChange[i]);
                            listViewItem_ThisPC.SubItems.Add(Type[i]);
                            listViewItem_ThisPC.SubItems.Add(Size[i]);
                            listView_RemotePC.Items.Add(listViewItem_ThisPC);
                        }
                    }
                });
            }
            else
            {
                if (Names.Count() == DateOfChange.Count() && Names.Count() == Type.Count() && Names.Count() == Size.Count())
                {

                    listView_RemotePC.Items.Clear();
                    ListViewItem listViewItem_ThisPC;
                    for (int i = 0; i < Names.Count(); i++)
                    {
                        listViewItem_ThisPC = new ListViewItem();
                        listViewItem_ThisPC.Text = Names[i];
                        listViewItem_ThisPC.SubItems.Add(DateOfChange[i]);
                        listViewItem_ThisPC.SubItems.Add(Type[i]);
                        listViewItem_ThisPC.SubItems.Add(Size[i]);
                        listView_RemotePC.Items.Add(listViewItem_ThisPC);
                    }
                }
            }
        }
        private void button_SendFile_toRemotePC_Click(object sender, EventArgs e)
        {
            if (SendFile_toRemotePC != null) SendFile_toRemotePC(listView_ThisPC.SelectedItems[0].Index, EventArgs.Empty);
            IsClickSendingToThisPC = false;
        }//индекс объекта из этого пк для передачи удаленному
        private void listView_RemotePC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseDoubleClick_RemotePC != null) MouseDoubleClick_RemotePC(listView_RemotePC.SelectedItems[0].Index, EventArgs.Empty);
        }
        private void listView_RemotePC_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView_RemotePC.FocusedItem.Bounds.Contains(e.Location))
                {
                    IsThisPC_RightClick = false;
                    contextMenuStrip_LV.Show(Cursor.Position);
                }
            }
        }//ПКМ Obj
        public void Set_RemotePC_Name(string _name)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_NameRemotePC.Text = _name;
                });
            }
            else
            {
                label_NameRemotePC.Text = _name;
            }
        }
        #endregion

        private void button_CancelSending_Click(object sender, EventArgs e)
        {
            if(IsClickSendingToThisPC == true)
            {
                if (CancelSendingToThisPC != null) CancelSendingToThisPC(this, EventArgs.Empty);
            }
            else
            {
                if (CancelSendingToRemotePC != null) CancelSendingToRemotePC(this, EventArgs.Empty);
            }
        }

        public void ShowInfo(string info, string caption)
        {
            MessageBox.Show(info, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBox_LD_ThisPC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (FmLd_RightClick_ThisPC != null) FmLd_RightClick_ThisPC(comboBox_LD_ThisPC.SelectedText, EventArgs.Empty);
            }
        }
        private void comboBox_LD_RemotePC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (FmLd_RightClick_RemotePC != null) FmLd_RightClick_RemotePC(comboBox_LD_RemotePC.SelectedText, EventArgs.Empty);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsThisPC_RightClick == true)
            {
                if (FmDelete_RightClick_ThisPC != null) FmDelete_RightClick_ThisPC(listView_ThisPC.SelectedItems[0].Index, EventArgs.Empty);
            }
            else
            {
                if (FmDelete_RightClick_RemotePC != null) FmDelete_RightClick_RemotePC(listView_RemotePC.SelectedItems[0].Index, EventArgs.Empty);
            }
        }
    }
}
