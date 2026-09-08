
namespace Vison_Inspect_System._2_ComPart.Normal_Form
{
    partial class DataSelectForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labResultDisplay = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grbSelectMode = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rdbLimit1000 = new System.Windows.Forms.RadioButton();
            this.btnDataSelect = new System.Windows.Forms.Button();
            this.BtnExportCsv = new System.Windows.Forms.Button();
            this.grbResultSelect = new System.Windows.Forms.GroupBox();
            this.Rdb_NG = new System.Windows.Forms.RadioButton();
            this.Rdb_OK = new System.Windows.Forms.RadioButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.grbSelectMode.SuspendLayout();
            this.grbResultSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1169, 732);
            this.dataGridView1.TabIndex = 0;
            // 
            // labResultDisplay
            // 
            this.labResultDisplay.AutoSize = true;
            this.labResultDisplay.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labResultDisplay.Location = new System.Drawing.Point(-1, 736);
            this.labResultDisplay.Name = "labResultDisplay";
            this.labResultDisplay.Size = new System.Drawing.Size(229, 19);
            this.labResultDisplay.TabIndex = 1;
            this.labResultDisplay.Text = "共查询到200条的数据记录";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Location = new System.Drawing.Point(1194, 346);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(175, 21);
            this.dateTimePickerStart.TabIndex = 3;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Location = new System.Drawing.Point(1194, 424);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(175, 21);
            this.dateTimePickerEnd.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1251, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "起始时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1251, 399);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "结束时间";
            // 
            // grbSelectMode
            // 
            this.grbSelectMode.Controls.Add(this.radioButton3);
            this.grbSelectMode.Controls.Add(this.radioButton2);
            this.grbSelectMode.Controls.Add(this.rdbLimit1000);
            this.grbSelectMode.Location = new System.Drawing.Point(1194, 12);
            this.grbSelectMode.Name = "grbSelectMode";
            this.grbSelectMode.Size = new System.Drawing.Size(175, 182);
            this.grbSelectMode.TabIndex = 6;
            this.grbSelectMode.TabStop = false;
            this.grbSelectMode.Text = "查询方式";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(8, 132);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(149, 16);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Text = "按总结果+插入时间查询";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(8, 86);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(107, 16);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Text = "按插入时间查询";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // rdbLimit1000
            // 
            this.rdbLimit1000.AutoSize = true;
            this.rdbLimit1000.Checked = true;
            this.rdbLimit1000.Location = new System.Drawing.Point(8, 38);
            this.rdbLimit1000.Name = "rdbLimit1000";
            this.rdbLimit1000.Size = new System.Drawing.Size(131, 16);
            this.rdbLimit1000.TabIndex = 0;
            this.rdbLimit1000.TabStop = true;
            this.rdbLimit1000.Text = "查询最新1000条数据";
            this.rdbLimit1000.UseVisualStyleBackColor = true;
            // 
            // btnDataSelect
            // 
            this.btnDataSelect.Location = new System.Drawing.Point(1205, 554);
            this.btnDataSelect.Name = "btnDataSelect";
            this.btnDataSelect.Size = new System.Drawing.Size(142, 69);
            this.btnDataSelect.TabIndex = 7;
            this.btnDataSelect.Text = "开始查询";
            this.btnDataSelect.UseVisualStyleBackColor = true;
            this.btnDataSelect.Click += new System.EventHandler(this.btnDataSelect_Click);
            // 
            // BtnExportCsv
            // 
            this.BtnExportCsv.Location = new System.Drawing.Point(1205, 663);
            this.BtnExportCsv.Name = "BtnExportCsv";
            this.BtnExportCsv.Size = new System.Drawing.Size(142, 69);
            this.BtnExportCsv.TabIndex = 7;
            this.BtnExportCsv.Text = "导出CSV文件";
            this.BtnExportCsv.UseVisualStyleBackColor = true;
            this.BtnExportCsv.Click += new System.EventHandler(this.BtnExportCsv_Click);
            // 
            // grbResultSelect
            // 
            this.grbResultSelect.Controls.Add(this.Rdb_NG);
            this.grbResultSelect.Controls.Add(this.Rdb_OK);
            this.grbResultSelect.Location = new System.Drawing.Point(1194, 484);
            this.grbResultSelect.Name = "grbResultSelect";
            this.grbResultSelect.Size = new System.Drawing.Size(175, 48);
            this.grbResultSelect.TabIndex = 8;
            this.grbResultSelect.TabStop = false;
            this.grbResultSelect.Text = "结果选择";
            // 
            // Rdb_NG
            // 
            this.Rdb_NG.AutoSize = true;
            this.Rdb_NG.Checked = true;
            this.Rdb_NG.Location = new System.Drawing.Point(125, 20);
            this.Rdb_NG.Name = "Rdb_NG";
            this.Rdb_NG.Size = new System.Drawing.Size(35, 16);
            this.Rdb_NG.TabIndex = 0;
            this.Rdb_NG.TabStop = true;
            this.Rdb_NG.Text = "NG";
            this.Rdb_NG.UseVisualStyleBackColor = true;
            // 
            // Rdb_OK
            // 
            this.Rdb_OK.AutoSize = true;
            this.Rdb_OK.Location = new System.Drawing.Point(36, 20);
            this.Rdb_OK.Name = "Rdb_OK";
            this.Rdb_OK.Size = new System.Drawing.Size(35, 16);
            this.Rdb_OK.TabIndex = 0;
            this.Rdb_OK.Text = "OK";
            this.Rdb_OK.UseVisualStyleBackColor = true;
            // 
            // DataSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 760);
            this.Controls.Add(this.grbResultSelect);
            this.Controls.Add(this.BtnExportCsv);
            this.Controls.Add(this.btnDataSelect);
            this.Controls.Add(this.grbSelectMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.labResultDisplay);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.Name = "DataSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataSelectForm";
            this.Load += new System.EventHandler(this.DataSelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.grbSelectMode.ResumeLayout(false);
            this.grbSelectMode.PerformLayout();
            this.grbResultSelect.ResumeLayout(false);
            this.grbResultSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labResultDisplay;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grbSelectMode;
        private System.Windows.Forms.Button btnDataSelect;
        private System.Windows.Forms.Button BtnExportCsv;
        private System.Windows.Forms.GroupBox grbResultSelect;
        private System.Windows.Forms.RadioButton Rdb_NG;
        private System.Windows.Forms.RadioButton Rdb_OK;
        private System.Windows.Forms.RadioButton rdbLimit1000;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}