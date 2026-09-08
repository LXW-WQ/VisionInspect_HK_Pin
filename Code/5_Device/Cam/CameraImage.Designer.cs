namespace Vison_Inspect_System
{
    partial class CameraImage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraImage));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.Cameralabel = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnOpenLive = new System.Windows.Forms.Button();
            this.btnCloseLive = new System.Windows.Forms.Button();
            this.ImagemvdRenderActivex = new VisionDesigner.MVDRenderActivex();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TriggerToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ExposureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveImageToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.SetGainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.ImagemvdRenderActivex);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            // 
            // splitContainer4
            // 
            resources.ApplyResources(this.splitContainer4, "splitContainer4");
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            resources.ApplyResources(this.splitContainer4.Panel1, "splitContainer4.Panel1");
            this.splitContainer4.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer4.Panel2
            // 
            resources.ApplyResources(this.splitContainer4.Panel2, "splitContainer4.Panel2");
            this.splitContainer4.Panel2.Controls.Add(this.Cameralabel);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.LightGreen;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Name = "label1";
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CameraNameLabel_MouseClick);
            // 
            // Cameralabel
            // 
            resources.ApplyResources(this.Cameralabel, "Cameralabel");
            this.Cameralabel.BackColor = System.Drawing.Color.LightGreen;
            this.Cameralabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Cameralabel.Name = "Cameralabel";
            this.Cameralabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CameraNameLabel_MouseClick);
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            resources.ApplyResources(this.splitContainer3.Panel1, "splitContainer3.Panel1");
            this.splitContainer3.Panel1.Controls.Add(this.btnOpenLive);
            // 
            // splitContainer3.Panel2
            // 
            resources.ApplyResources(this.splitContainer3.Panel2, "splitContainer3.Panel2");
            this.splitContainer3.Panel2.Controls.Add(this.btnCloseLive);
            // 
            // btnOpenLive
            // 
            resources.ApplyResources(this.btnOpenLive, "btnOpenLive");
            this.btnOpenLive.Name = "btnOpenLive";
            this.btnOpenLive.UseVisualStyleBackColor = true;
            this.btnOpenLive.Click += new System.EventHandler(this.btnOpenLive_Click);
            // 
            // btnCloseLive
            // 
            resources.ApplyResources(this.btnCloseLive, "btnCloseLive");
            this.btnCloseLive.Name = "btnCloseLive";
            this.btnCloseLive.UseVisualStyleBackColor = true;
            this.btnCloseLive.Click += new System.EventHandler(this.btnCloseLive_Click);
            // 
            // ImagemvdRenderActivex
            // 
            resources.ApplyResources(this.ImagemvdRenderActivex, "ImagemvdRenderActivex");
            this.ImagemvdRenderActivex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ImagemvdRenderActivex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagemvdRenderActivex.EnableRetainShape = false;
            this.ImagemvdRenderActivex.InteractType = VisionDesigner.MVDRenderInteractType.StandardAndCustom;
            this.ImagemvdRenderActivex.InterpolationMode = VisionDesigner.MVD_INTERPOLATION_MODE.MvdNearestNeighbor;
            this.ImagemvdRenderActivex.MenuLanguage = VisionDesigner.MVDRenderMenuLangType.Default;
            this.ImagemvdRenderActivex.Name = "ImagemvdRenderActivex";
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TriggerToolStripMenuItem3,
            this.ExposureToolStripMenuItem,
            this.SaveImageToolStripMenuItem4,
            this.SetGainToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // TriggerToolStripMenuItem3
            // 
            resources.ApplyResources(this.TriggerToolStripMenuItem3, "TriggerToolStripMenuItem3");
            this.TriggerToolStripMenuItem3.Image = global::Vison_Inspect_System.Properties.Resources.触发;
            this.TriggerToolStripMenuItem3.Name = "TriggerToolStripMenuItem3";
            this.TriggerToolStripMenuItem3.Click += new System.EventHandler(this.TriggerToolStripMenuItem3_Click);
            // 
            // ExposureToolStripMenuItem
            // 
            resources.ApplyResources(this.ExposureToolStripMenuItem, "ExposureToolStripMenuItem");
            this.ExposureToolStripMenuItem.Image = global::Vison_Inspect_System.Properties.Resources.曝光;
            this.ExposureToolStripMenuItem.Name = "ExposureToolStripMenuItem";
            this.ExposureToolStripMenuItem.Click += new System.EventHandler(this.ExposureToolStripMenuItem_Click);
            // 
            // SaveImageToolStripMenuItem4
            // 
            resources.ApplyResources(this.SaveImageToolStripMenuItem4, "SaveImageToolStripMenuItem4");
            this.SaveImageToolStripMenuItem4.Image = global::Vison_Inspect_System.Properties.Resources.保存图片;
            this.SaveImageToolStripMenuItem4.Name = "SaveImageToolStripMenuItem4";
            this.SaveImageToolStripMenuItem4.Click += new System.EventHandler(this.SaveImageToolStripMenuItem4_Click);
            // 
            // SetGainToolStripMenuItem
            // 
            resources.ApplyResources(this.SetGainToolStripMenuItem, "SetGainToolStripMenuItem");
            this.SetGainToolStripMenuItem.Image = global::Vison_Inspect_System.Properties.Resources.增益;
            this.SetGainToolStripMenuItem.Name = "SetGainToolStripMenuItem";
            this.SetGainToolStripMenuItem.Click += new System.EventHandler(this.SetGainToolStripMenuItem_Click);
            // 
            // CameraImage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "CameraImage";
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label Cameralabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TriggerToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem SaveImageToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem ExposureToolStripMenuItem;
        public VisionDesigner.MVDRenderActivex ImagemvdRenderActivex;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnCloseLive;
        private System.Windows.Forms.SplitContainer splitContainer4;
        public System.Windows.Forms.Button btnOpenLive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem SetGainToolStripMenuItem;
    }
}
