namespace UT61E_Multimeter
{
    partial class display
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picDelta = new System.Windows.Forms.PictureBox();
            this.picBeep = new System.Windows.Forms.PictureBox();
            this.picDiode = new System.Windows.Forms.PictureBox();
            this.picEF = new System.Windows.Forms.PictureBox();
            this.picHold = new System.Windows.Forms.PictureBox();
            this.picBatt = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBatt)).BeginInit();
            this.SuspendLayout();
            // 
            // picDelta
            // 
            this.picDelta.Image = global::UT61E_Multimeter.Properties.Resources.ldelta;
            this.picDelta.Location = new System.Drawing.Point(300, 6);
            this.picDelta.Name = "picDelta";
            this.picDelta.Size = new System.Drawing.Size(30, 24);
            this.picDelta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picDelta.TabIndex = 5;
            this.picDelta.TabStop = false;
            // 
            // picBeep
            // 
            this.picBeep.Image = global::UT61E_Multimeter.Properties.Resources.lbeeper;
            this.picBeep.Location = new System.Drawing.Point(271, 6);
            this.picBeep.Name = "picBeep";
            this.picBeep.Size = new System.Drawing.Size(29, 24);
            this.picBeep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBeep.TabIndex = 4;
            this.picBeep.TabStop = false;
            // 
            // picDiode
            // 
            this.picDiode.Image = global::UT61E_Multimeter.Properties.Resources.ldiode;
            this.picDiode.Location = new System.Drawing.Point(237, 8);
            this.picDiode.Name = "picDiode";
            this.picDiode.Size = new System.Drawing.Size(31, 22);
            this.picDiode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picDiode.TabIndex = 3;
            this.picDiode.TabStop = false;
            // 
            // picEF
            // 
            this.picEF.Image = global::UT61E_Multimeter.Properties.Resources.lef;
            this.picEF.Location = new System.Drawing.Point(194, 5);
            this.picEF.Name = "picEF";
            this.picEF.Size = new System.Drawing.Size(44, 25);
            this.picEF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picEF.TabIndex = 2;
            this.picEF.TabStop = false;
            // 
            // picHold
            // 
            this.picHold.Image = global::UT61E_Multimeter.Properties.Resources.lhold;
            this.picHold.Location = new System.Drawing.Point(153, 7);
            this.picHold.Name = "picHold";
            this.picHold.Size = new System.Drawing.Size(35, 23);
            this.picHold.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picHold.TabIndex = 1;
            this.picHold.TabStop = false;
            // 
            // picBatt
            // 
            this.picBatt.Image = global::UT61E_Multimeter.Properties.Resources.lbatt;
            this.picBatt.Location = new System.Drawing.Point(14, 41);
            this.picBatt.Name = "picBatt";
            this.picBatt.Size = new System.Drawing.Size(26, 17);
            this.picBatt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBatt.TabIndex = 0;
            this.picBatt.TabStop = false;
            // 
            // display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picDelta);
            this.Controls.Add(this.picBeep);
            this.Controls.Add(this.picDiode);
            this.Controls.Add(this.picEF);
            this.Controls.Add(this.picHold);
            this.Controls.Add(this.picBatt);
            this.DoubleBuffered = true;
            this.Name = "display";
            this.Size = new System.Drawing.Size(422, 200);
            ((System.ComponentModel.ISupportInitialize)(this.picDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBeep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBatt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBatt;
        private System.Windows.Forms.PictureBox picHold;
        private System.Windows.Forms.PictureBox picEF;
        private System.Windows.Forms.PictureBox picDiode;
        private System.Windows.Forms.PictureBox picBeep;
        private System.Windows.Forms.PictureBox picDelta;
    }
}
