
namespace Vison_Inspect_System._2_ComPart.Normal_Form
{
    partial class LoadingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labCompanyInfo = new System.Windows.Forms.Label();
            this.labLoadingInfo = new System.Windows.Forms.Label();
            this.progressBarLoadProgress = new System.Windows.Forms.ProgressBar();
            this.pictureBoxLog = new System.Windows.Forms.PictureBox();
            this.pictureBoxCmp = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCmp)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.16667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.83333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 269F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxLog, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxCmp, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labCompanyInfo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labLoadingInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBarLoadProgress, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.21622F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.78378F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(507, 300);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labCompanyInfo
            // 
            this.labCompanyInfo.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labCompanyInfo, 2);
            this.labCompanyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labCompanyInfo.Location = new System.Drawing.Point(3, 42);
            this.labCompanyInfo.Name = "labCompanyInfo";
            this.labCompanyInfo.Size = new System.Drawing.Size(231, 221);
            this.labCompanyInfo.TabIndex = 2;
            this.labCompanyInfo.Text = "label1";
            // 
            // labLoadingInfo
            // 
            this.labLoadingInfo.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labLoadingInfo, 3);
            this.labLoadingInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labLoadingInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labLoadingInfo.Location = new System.Drawing.Point(3, 263);
            this.labLoadingInfo.Name = "labLoadingInfo";
            this.labLoadingInfo.Size = new System.Drawing.Size(501, 16);
            this.labLoadingInfo.TabIndex = 3;
            this.labLoadingInfo.Text = "Loading";
            this.labLoadingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarLoadProgress
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.progressBarLoadProgress, 3);
            this.progressBarLoadProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarLoadProgress.Location = new System.Drawing.Point(3, 282);
            this.progressBarLoadProgress.Name = "progressBarLoadProgress";
            this.progressBarLoadProgress.Size = new System.Drawing.Size(501, 15);
            this.progressBarLoadProgress.TabIndex = 4;
            // 
            // pictureBoxLog
            // 
            this.pictureBoxLog.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxLog.Name = "pictureBoxLog";
            this.pictureBoxLog.Size = new System.Drawing.Size(122, 36);
            this.pictureBoxLog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLog.TabIndex = 0;
            this.pictureBoxLog.TabStop = false;
            // 
            // pictureBoxCmp
            // 
            this.pictureBoxCmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCmp.Image = global::Vison_Inspect_System.Properties.Resources.R_C;
            this.pictureBoxCmp.Location = new System.Drawing.Point(240, 3);
            this.pictureBoxCmp.Name = "pictureBoxCmp";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBoxCmp, 2);
            this.pictureBoxCmp.Size = new System.Drawing.Size(264, 257);
            this.pictureBoxCmp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCmp.TabIndex = 1;
            this.pictureBoxCmp.TabStop = false;
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 300);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLoading";
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCmp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxLog;
        private System.Windows.Forms.PictureBox pictureBoxCmp;
        private System.Windows.Forms.Label labCompanyInfo;
        private System.Windows.Forms.Label labLoadingInfo;
        private System.Windows.Forms.ProgressBar progressBarLoadProgress;
    }
}