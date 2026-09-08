namespace Vison_Inspect_System._5_Device.Cam
{
    partial class FrmAdjustExposure
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
            this.lTrackBar1 = new Vison_Inspect_System.LTrackBar();
            this.textBox_SetValue1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.labTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lTrackBar1
            // 
            this.lTrackBar1.L_BarColor = System.Drawing.Color.Black;
            this.lTrackBar1.L_BarSize = 10;
            this.lTrackBar1.L_IsRound = true;
            this.lTrackBar1.L_Maximum = 100000;
            this.lTrackBar1.L_Minimum = 0;
            this.lTrackBar1.L_Orientation = Vison_Inspect_System.Orientation.Horizontal_LR;
            this.lTrackBar1.L_SliderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lTrackBar1.L_Value = 19;
            this.lTrackBar1.Location = new System.Drawing.Point(28, 79);
            this.lTrackBar1.Name = "lTrackBar1";
            this.lTrackBar1.Size = new System.Drawing.Size(267, 10);
            this.lTrackBar1.TabIndex = 24;
            this.lTrackBar1.Text = "lTrackBar1";
            // 
            // textBox_SetValue1
            // 
            this.textBox_SetValue1.Location = new System.Drawing.Point(334, 74);
            this.textBox_SetValue1.Name = "textBox_SetValue1";
            this.textBox_SetValue1.Size = new System.Drawing.Size(50, 21);
            this.textBox_SetValue1.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(416, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "(us)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "曝光值调整：";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Location = new System.Drawing.Point(370, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 30);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labTitle
            // 
            this.labTitle.AutoSize = true;
            this.labTitle.BackColor = System.Drawing.Color.Aqua;
            this.labTitle.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labTitle.Location = new System.Drawing.Point(134, 12);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(125, 22);
            this.labTitle.TabIndex = 29;
            this.labTitle.Text = "曝光值调整";
            // 
            // FrmAdjustExposure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 115);
            this.Controls.Add(this.labTitle);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_SetValue1);
            this.Controls.Add(this.lTrackBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAdjustExposure";
            this.Text = "FrmAdjustExposure";
            this.Load += new System.EventHandler(this.FrmAdjustExposure_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Vison_Inspect_System.LTrackBar lTrackBar1;
        private System.Windows.Forms.TextBox textBox_SetValue1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label labTitle;
    }
}