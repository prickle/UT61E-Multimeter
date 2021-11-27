namespace UT61E_Multimeter
{
    partial class UT61E
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UT61E));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.cbxPorts = new System.Windows.Forms.ToolStripComboBox();
            this.statusLbl = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.display1 = new UT61E_Multimeter.display();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.cbxPorts,
            this.statusLbl,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 183);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(425, 30);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConnect
            // 
            this.btnConnect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnConnect.Checked = true;
            this.btnConnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnConnect.Image = global::UT61E_Multimeter.Properties.Resources.connect;
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(87, 28);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cbxPorts
            // 
            this.cbxPorts.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cbxPorts.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cbxPorts.Margin = new System.Windows.Forms.Padding(1);
            this.cbxPorts.Name = "cbxPorts";
            this.cbxPorts.Size = new System.Drawing.Size(100, 28);
            this.cbxPorts.DropDown += new System.EventHandler(this.cbxPorts_DropDown);
            // 
            // statusLbl
            // 
            this.statusLbl.AutoToolTip = true;
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(109, 27);
            this.statusLbl.Text = "Not Connected";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // display1
            // 
            this.display1.Ac = false;
            this.display1.Auto = true;
            this.display1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(124)))), ((int)(((byte)(0)))));
            this.display1.Dc = false;
            this.display1.Delta = false;
            this.display1.Digits = "-----";
            this.display1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display1.Freq = false;
            this.display1.Hold = false;
            this.display1.Location = new System.Drawing.Point(0, 0);
            this.display1.Lowbatt = false;
            this.display1.Max = false;
            this.display1.Min = false;
            this.display1.Minus = false;
            this.display1.Mode = 7;
            this.display1.Name = "display1";
            this.display1.Ol = false;
            this.display1.Pct = false;
            this.display1.Pmax = false;
            this.display1.Pmin = false;
            this.display1.Range = 0;
            this.display1.Size = new System.Drawing.Size(425, 183);
            this.display1.TabIndex = 4;
            this.display1.Ul = false;
            // 
            // UT61E
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(425, 213);
            this.Controls.Add(this.display1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UT61E";
            this.Text = "UNI-T UT61E - TinkerTech Cafe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbxPorts;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripLabel statusLbl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private display display1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

