using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Segmentation_and_other
{
    public interface IForm1
    {
        event EventHandler EnterSlovoIDZClick;
        event EventHandler EnterPuteClick;
        event EventHandler BackwardFileManagerClick;
        event EventHandler ForwardFileManagerClick;
        string SG_txSlovo { set; get; }
        string SG_txTotalPath { set; get; }
        int SG_progressBarOperationValue { set; get; }

        void LoadDataToListViewLogicalDisks(List<string> ListLD, int NumSelectedLD);
        event EventHandler LogicalDisksMouseDoubleClick;
        string SG_NameSelectedLD { set; get; }
        int SG_NumSelectedLD { set; get; }

        void LoadDataToListViewFileManager(List<string> NameObj, List<int> TypeObj, List<Int64> SizeObj, List<int> SelectedObj);
        event EventHandler FileManagerMouseDoubleClick;
        string SG_NameSelectedObj { set; get; }
        int SG_NumSelectedObj { set; get; }

        void ComboBoxEncodingAddListItems(List<string> ItemsList);
        event EventHandler comboBoxEncodingSelectedItem;
        string SG_ComboBoxEncodingSelectedItem { set; get; }

        event EventHandler LogicalDisksMouseRightClick;
        int SG_NumRightClickLD { set; get; }
    }
    public partial class Form1 : Form, IForm1
    {
        public string NameSelectedLD;
        public int NumSelectedLD;

        public string NameSelectedObj;
        public int NumSelectedObj;

        public string ComboBoxEncodingSelectedItem;

        public int NumRightClickLD;

        public Form1()
        {
            InitializeComponent();

            lvFileManager.MouseDoubleClick += new MouseEventHandler(lvFileManager_MouseDoubleClick);
            lvLogicalDisks.MouseDoubleClick += new MouseEventHandler(lvLogicalDisks_MouseDoubleClick);

            butEnterSlovoIDZ.Click -= butEnterSlovoIDZ_Click; //отписаться от события. связи сохраняються даже при создании новых. Событие вполняется 2 раза, хотя нажатие на кнопку еденичное.
            butEnterSlovoIDZ.Click += butEnterSlovoIDZ_Click;

            butEnterPute.Click -= butEnterPute_Click;
            butEnterPute.Click += butEnterPute_Click;

            butBackwardFileManager.Click -= butBackwardFileManager_Click;
            butBackwardFileManager.Click += butBackwardFileManager_Click;

            butForwardFileManager.Click -= butForwardFileManager_Click;
            butForwardFileManager.Click += butForwardFileManager_Click;

            comboBoxEncoding.SelectedIndexChanged -= comboBoxEncoding_SelectedIndexChanged;
            comboBoxEncoding.SelectedIndexChanged += comboBoxEncoding_SelectedIndexChanged;

            lvLogicalDisks.MouseClick -= lvLogicalDisks_MouseClick;
            lvLogicalDisks.MouseClick += lvLogicalDisks_MouseClick;
        }

        public event EventHandler LogicalDisksMouseRightClick;
        public event EventHandler EnterSlovoIDZClick;
        public event EventHandler EnterPuteClick;
        public event EventHandler BackwardFileManagerClick;
        public event EventHandler ForwardFileManagerClick;
        public event EventHandler LogicalDisksMouseDoubleClick;
        public event EventHandler FileManagerMouseDoubleClick;
        public event EventHandler comboBoxEncodingSelectedItem;

        public int SG_NumRightClickLD
        {
            set { NumRightClickLD = value; }
            get { return NumRightClickLD; }
        }
        public string SG_NameSelectedObj
        {
            set { NameSelectedObj = value; }
            get { return NameSelectedObj; }
        }
        public int SG_NumSelectedObj
        {
            set { NumSelectedObj = value; }
            get { return NumSelectedObj; }
        }
        public string SG_NameSelectedLD
        {
            set { NameSelectedLD = value; }
            get { return NameSelectedLD; }
        }
        public int SG_NumSelectedLD
        {
            set { NumSelectedLD = value; }
            get { return NumSelectedLD; }
        }
        public string SG_txSlovo
        {
            set { txSlovo.Text = value; }
            get { return txSlovo.Text; }
        }
        public int SG_progressBarOperationValue
        {
            set { progressBarOperation.Value = value; }
            get { return progressBarOperation.Value; }
        }
        public string SG_txTotalPath
        {
            set { txTotalPath.Text = value; }
            get { return txTotalPath.Text; }
        }
        public string SG_ComboBoxEncodingSelectedItem
        {
            set { ComboBoxEncodingSelectedItem = value; }
            get { return ComboBoxEncodingSelectedItem; }
        }

        public void LoadDataToListViewLogicalDisks (List<string> ListLD, int NumSelectedLD)
        {
            lvLogicalDisks.Items.Clear();
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);
            imageList.Images.Add(new Bitmap("Images/disk.png"));

            int cou = 0;
            lvLogicalDisks.SmallImageList = imageList;

            for (int i = 0; i < ListLD.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem(new string[] { "" + ListLD[i] });
                listViewItem.ImageIndex = 0;

                if (NumSelectedLD == i)
                {
                    listViewItem.BackColor = Color.FromName("LightGreen"); //выделение строки цветом
                }
                
                listViewItem.ToolTipText = "LD"+ cou;
                lvLogicalDisks.Items.Add(listViewItem);
               // toolTip1.SetToolTip(lvLogicalDisks, "LD"+ cou);
                cou++;
            }
        }

        public void LoadDataToListViewFileManager(List<string> NameObj, List<int> TypeObj, List<Int64> SizeObj, List<int> SelectedObj)
        {
            lvFileManager.Items.Clear();
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);
            imageList.Images.Add(new Bitmap("Images/dir.png"));
            imageList.Images.Add(new Bitmap("Images/file.png"));

            lvFileManager.SmallImageList = imageList;

            string Type="";
            string Size = "";
            int NumStartFiles = 0;

            for (int i = 0; i < NameObj.Count; i++)
            {
                if(TypeObj[i] == 0)
                {
                    Type = "Папка с файлами";
                    Size = "";
                }
                else
                {
                    Type = "Файл";
                    Size = Convert.ToString(SizeObj[i])+" байт";
                }

                ListViewItem listViewItem = new ListViewItem(new string[] { "" + NameObj[i], Type, Size });

                if(TypeObj[i] == 0)
                {
                    listViewItem.ImageIndex = 0;
                    
                        NumStartFiles++;
                  
                }
                else
                {
                    listViewItem.ImageIndex = 1;

                    if (SelectedObj != null)
                    {
                        if (SelectedObj[i - NumStartFiles] == 1)
                        {
                            listViewItem.BackColor = Color.FromName("SlateBlue"); //выделение строки цветом
                        }
                    }
                }

                lvFileManager.Items.Add(listViewItem);
            }
        }

        private void lvFileManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = lvFileManager.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                //MessageBox.Show("The selected Item Name is: " + text +"   "+ intselectedindex);
                NameSelectedObj = item.Text;
                NumSelectedObj = item.Index;
                if (FileManagerMouseDoubleClick != null) FileManagerMouseDoubleClick(this, EventArgs.Empty);
            }

        }

        private void lvLogicalDisks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = lvLogicalDisks.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                //MessageBox.Show("The selected Item Name is: " + text +"   "+ intselectedindex);
                NameSelectedLD = item.Text;
                NumSelectedLD = item.Index;
                if (LogicalDisksMouseDoubleClick != null) LogicalDisksMouseDoubleClick(this, EventArgs.Empty);
            }

        }

        private void butEnterSlovoIDZ_Click(object sender, EventArgs e)
        {
            if (EnterSlovoIDZClick != null) EnterSlovoIDZClick(this, EventArgs.Empty);
        }

        private void butEnterPute_Click(object sender, EventArgs e)
        {
            if (EnterPuteClick != null) EnterPuteClick(this, EventArgs.Empty);
        }

        private void butBackwardFileManager_Click(object sender, EventArgs e)
        {
            if (BackwardFileManagerClick != null) BackwardFileManagerClick(this, EventArgs.Empty);
        }

        private void butForwardFileManager_Click(object sender, EventArgs e)
        {
            if (ForwardFileManagerClick != null) ForwardFileManagerClick(this, EventArgs.Empty);
        }

        public void ComboBoxEncodingAddListItems(List<string> ItemsList)
        {
            for (int i = 0; i < ItemsList.Count; i++)
            {
                comboBoxEncoding.Items.Add(ItemsList[i]);
                if(ItemsList[i]== "1200   -   utf-16")
                { comboBoxEncoding.SelectedIndex = i; }
            }
        }

        private void comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEncodingSelectedItem = comboBoxEncoding.SelectedItem.ToString();
            if (comboBoxEncodingSelectedItem != null) comboBoxEncodingSelectedItem(this, EventArgs.Empty);
        }

        private void lvLogicalDisks_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvLogicalDisks.FocusedItem.Bounds.Contains(e.Location))
                {
                    ListViewItem lv = (ListViewItem)lvLogicalDisks.GetItemAt(e.X, e.Y);
                    if (lv == null)
                        return;
                    NumRightClickLD=lv.Index;
                    if (LogicalDisksMouseRightClick != null) LogicalDisksMouseRightClick(this, EventArgs.Empty);
                }
            }
        }//ПКМ

    }
}
