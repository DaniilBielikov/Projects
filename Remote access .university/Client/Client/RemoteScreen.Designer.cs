namespace Client
{
    partial class RemoteScreen
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
            this.pictureBox_Screen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Screen)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Screen
            // 
            this.pictureBox_Screen.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_Screen.Location = new System.Drawing.Point(1, 1);
            this.pictureBox_Screen.Name = "pictureBox_Screen";
            this.pictureBox_Screen.Size = new System.Drawing.Size(1920, 1080);
            this.pictureBox_Screen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Screen.TabIndex = 0;
            this.pictureBox_Screen.TabStop = false;
            // 
            // RemoteScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(921, 527);
            this.Controls.Add(this.pictureBox_Screen);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "RemoteScreen";
            this.Text = "Remote Screen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteScreen_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Screen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Screen;
    }
}