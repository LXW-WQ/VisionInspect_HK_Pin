namespace Vison_Inspect_System._2_ComPart
{
    partial class VisonSetting
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
            this.LabFileName = new System.Windows.Forms.Label();
            this.btnOpenRecipe = new System.Windows.Forms.Button();
            this.btnSaveRecipe = new System.Windows.Forms.Button();
            this.btnCreateRecipe = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.grbRecipe = new System.Windows.Forms.GroupBox();
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudGain = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetGain = new System.Windows.Forms.Button();
            this.btnSetCamExposure = new System.Windows.Forms.Button();
            this.btnGetGain = new System.Windows.Forms.Button();
            this.btnGetCamExposure = new System.Windows.Forms.Button();
            this.btnSelectVisonProject = new System.Windows.Forms.Button();
            this.labVisonProjectPath = new System.Windows.Forms.Label();
            this.grbRecipe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabFileName
            // 
            this.LabFileName.AutoSize = true;
            this.LabFileName.Location = new System.Drawing.Point(64, 20);
            this.LabFileName.Name = "LabFileName";
            this.LabFileName.Size = new System.Drawing.Size(227, 12);
            this.LabFileName.TabIndex = 3;
            this.LabFileName.Text = "C:\\Users\\qian.wang\\Desktop\\RecipeFile";
            // 
            // btnOpenRecipe
            // 
            this.btnOpenRecipe.Location = new System.Drawing.Point(165, 20);
            this.btnOpenRecipe.Name = "btnOpenRecipe";
            this.btnOpenRecipe.Size = new System.Drawing.Size(96, 40);
            this.btnOpenRecipe.TabIndex = 2;
            this.btnOpenRecipe.Text = "打开配方";
            this.btnOpenRecipe.UseVisualStyleBackColor = true;
            this.btnOpenRecipe.Click += new System.EventHandler(this.btnOpenRecipe_Click);
            // 
            // btnSaveRecipe
            // 
            this.btnSaveRecipe.Location = new System.Drawing.Point(328, 20);
            this.btnSaveRecipe.Name = "btnSaveRecipe";
            this.btnSaveRecipe.Size = new System.Drawing.Size(96, 40);
            this.btnSaveRecipe.TabIndex = 2;
            this.btnSaveRecipe.Text = "保存配方";
            this.btnSaveRecipe.UseVisualStyleBackColor = true;
            this.btnSaveRecipe.Click += new System.EventHandler(this.btnSaveRecipe_Click);
            // 
            // btnCreateRecipe
            // 
            this.btnCreateRecipe.Location = new System.Drawing.Point(9, 20);
            this.btnCreateRecipe.Name = "btnCreateRecipe";
            this.btnCreateRecipe.Size = new System.Drawing.Size(96, 40);
            this.btnCreateRecipe.TabIndex = 2;
            this.btnCreateRecipe.Text = "新建配方";
            this.btnCreateRecipe.UseVisualStyleBackColor = true;
            this.btnCreateRecipe.Click += new System.EventHandler(this.btnCreateRecipe_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "配方：";
            // 
            // grbRecipe
            // 
            this.grbRecipe.Controls.Add(this.btnCreateRecipe);
            this.grbRecipe.Controls.Add(this.btnOpenRecipe);
            this.grbRecipe.Controls.Add(this.btnSaveRecipe);
            this.grbRecipe.Location = new System.Drawing.Point(11, 90);
            this.grbRecipe.Name = "grbRecipe";
            this.grbRecipe.Size = new System.Drawing.Size(450, 73);
            this.grbRecipe.TabIndex = 7;
            this.grbRecipe.TabStop = false;
            this.grbRecipe.Text = "配方操作";
            // 
            // nudExposure
            // 
            this.nudExposure.Location = new System.Drawing.Point(125, 23);
            this.nudExposure.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudExposure.Name = "nudExposure";
            this.nudExposure.Size = new System.Drawing.Size(115, 21);
            this.nudExposure.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "相机曝光值设定：";
            // 
            // nudGain
            // 
            this.nudGain.Location = new System.Drawing.Point(125, 64);
            this.nudGain.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudGain.Name = "nudGain";
            this.nudGain.Size = new System.Drawing.Size(115, 21);
            this.nudGain.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "相机增益值设定：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetGain);
            this.groupBox1.Controls.Add(this.btnSetCamExposure);
            this.groupBox1.Controls.Add(this.btnGetGain);
            this.groupBox1.Controls.Add(this.btnGetCamExposure);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudExposure);
            this.groupBox1.Controls.Add(this.nudGain);
            this.groupBox1.Location = new System.Drawing.Point(11, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 116);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机参数设定";
            // 
            // btnSetGain
            // 
            this.btnSetGain.Location = new System.Drawing.Point(352, 64);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Size = new System.Drawing.Size(75, 23);
            this.btnSetGain.TabIndex = 10;
            this.btnSetGain.Tag = "";
            this.btnSetGain.Text = "Set";
            this.btnSetGain.UseVisualStyleBackColor = true;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // btnSetCamExposure
            // 
            this.btnSetCamExposure.Location = new System.Drawing.Point(352, 20);
            this.btnSetCamExposure.Name = "btnSetCamExposure";
            this.btnSetCamExposure.Size = new System.Drawing.Size(75, 23);
            this.btnSetCamExposure.TabIndex = 10;
            this.btnSetCamExposure.Tag = "";
            this.btnSetCamExposure.Text = "Set";
            this.btnSetCamExposure.UseVisualStyleBackColor = true;
            this.btnSetCamExposure.Click += new System.EventHandler(this.btnSetCamExposure_Click);
            // 
            // btnGetGain
            // 
            this.btnGetGain.Location = new System.Drawing.Point(257, 64);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Size = new System.Drawing.Size(75, 23);
            this.btnGetGain.TabIndex = 10;
            this.btnGetGain.Tag = "";
            this.btnGetGain.Text = "Get";
            this.btnGetGain.UseVisualStyleBackColor = true;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // btnGetCamExposure
            // 
            this.btnGetCamExposure.Location = new System.Drawing.Point(257, 20);
            this.btnGetCamExposure.Name = "btnGetCamExposure";
            this.btnGetCamExposure.Size = new System.Drawing.Size(75, 23);
            this.btnGetCamExposure.TabIndex = 10;
            this.btnGetCamExposure.Tag = "";
            this.btnGetCamExposure.Text = "Get";
            this.btnGetCamExposure.UseVisualStyleBackColor = true;
            this.btnGetCamExposure.Click += new System.EventHandler(this.btnGetCamExposure_Click);
            // 
            // btnSelectVisonProject
            // 
            this.btnSelectVisonProject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectVisonProject.Location = new System.Drawing.Point(19, 49);
            this.btnSelectVisonProject.Name = "btnSelectVisonProject";
            this.btnSelectVisonProject.Size = new System.Drawing.Size(109, 23);
            this.btnSelectVisonProject.TabIndex = 35;
            this.btnSelectVisonProject.Text = "选择视觉方案";
            this.btnSelectVisonProject.UseVisualStyleBackColor = true;
            this.btnSelectVisonProject.Click += new System.EventHandler(this.btnSelectVisonProject_Click);
            // 
            // labVisonProjectPath
            // 
            this.labVisonProjectPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labVisonProjectPath.Location = new System.Drawing.Point(134, 49);
            this.labVisonProjectPath.Name = "labVisonProjectPath";
            this.labVisonProjectPath.Size = new System.Drawing.Size(327, 23);
            this.labVisonProjectPath.TabIndex = 36;
            this.labVisonProjectPath.Text = "*********";
            this.labVisonProjectPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VisonSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 324);
            this.Controls.Add(this.labVisonProjectPath);
            this.Controls.Add(this.btnSelectVisonProject);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbRecipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LabFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VisonSetting";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VisonSetting_FormClosed);
            this.Load += new System.EventHandler(this.VisonSetting_Load);
            this.grbRecipe.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LabFileName;
        private System.Windows.Forms.Button btnOpenRecipe;
        private System.Windows.Forms.Button btnSaveRecipe;
        private System.Windows.Forms.Button btnCreateRecipe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grbRecipe;
        private System.Windows.Forms.NumericUpDown nudExposure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetGain;
        private System.Windows.Forms.Button btnSetCamExposure;
        private System.Windows.Forms.Button btnGetGain;
        private System.Windows.Forms.Button btnGetCamExposure;
        private System.Windows.Forms.Button btnSelectVisonProject;
        private System.Windows.Forms.Label labVisonProjectPath;
    }
}