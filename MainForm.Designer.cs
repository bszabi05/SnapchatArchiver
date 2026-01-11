namespace SnapchatArchiver
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txbPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.lsbLog = new System.Windows.Forms.ListBox();
            this.btnEN = new System.Windows.Forms.Button();
            this.btnHU = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(0)))));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.ForeColor = System.Drawing.Color.Black;
            this.btnBrowse.Location = new System.Drawing.Point(736, 35);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(117, 32);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "TALLÓZÁS";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbPath
            // 
            this.txbPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.txbPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbPath.ForeColor = System.Drawing.Color.White;
            this.txbPath.Location = new System.Drawing.Point(45, 38);
            this.txbPath.Name = "txbPath";
            this.txbPath.ReadOnly = true;
            this.txbPath.Size = new System.Drawing.Size(661, 22);
            this.txbPath.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(0)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(45, 427);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(808, 41);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "ARCHIVÁLÁS INDÍTÁSA";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.button2_Click);
            // 
            // lsbLog
            // 
            this.lsbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lsbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.lsbLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.lsbLog.FormattingEnabled = true;
            this.lsbLog.ItemHeight = 18;
            this.lsbLog.Location = new System.Drawing.Point(45, 83);
            this.lsbLog.Name = "lsbLog";
            this.lsbLog.Size = new System.Drawing.Size(808, 324);
            this.lsbLog.TabIndex = 3;
            // 
            // btnEN
            // 
            this.btnEN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(0)))));
            this.btnEN.FlatAppearance.BorderSize = 0;
            this.btnEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEN.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEN.ForeColor = System.Drawing.Color.Black;
            this.btnEN.Location = new System.Drawing.Point(80, 4);
            this.btnEN.Name = "btnEN";
            this.btnEN.Size = new System.Drawing.Size(32, 22);
            this.btnEN.TabIndex = 5;
            this.btnEN.Text = "EN";
            this.btnEN.UseVisualStyleBackColor = false;
            this.btnEN.Click += new System.EventHandler(this.btnEN_Click);
            // 
            // btnHU
            // 
            this.btnHU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnHU.FlatAppearance.BorderSize = 0;
            this.btnHU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHU.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHU.ForeColor = System.Drawing.Color.White;
            this.btnHU.Location = new System.Drawing.Point(46, 4);
            this.btnHU.Name = "btnHU";
            this.btnHU.Size = new System.Drawing.Size(32, 22);
            this.btnHU.TabIndex = 4;
            this.btnHU.Text = "HU";
            this.btnHU.UseVisualStyleBackColor = false;
            this.btnHU.Click += new System.EventHandler(this.btnHU_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(896, 492);
            this.Controls.Add(this.btnHU);
            this.Controls.Add(this.btnEN);
            this.Controls.Add(this.lsbLog);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.btnBrowse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Snapchat Archiver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txbPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lsbLog;
        private System.Windows.Forms.Button btnEN;
        private System.Windows.Forms.Button btnHU;
    }
}

