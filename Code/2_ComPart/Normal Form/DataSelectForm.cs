using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Vison_Inspect_System._2_ComPart.Normal_Form
{
    public partial class DataSelectForm : Form
    {
        //MYSQL Mysql = new MYSQL("localhost", "root", "lxw201314wq", "datacollect_db", 3306);
        DataTable dt = new DataTable();
        public DataSelectForm()
        {
            InitializeComponent();

        }
        private void DataSelectForm_Load(object sender, EventArgs e)
        {
            this.dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerStart.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            //this.dateTimePickerStart.ShowUpDown = true;
            this.dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            //this.dateTimePickerEnd.ShowUpDown = true;
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:进入数据查询界面");
        }
        /// <summary>
        /// 开始查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataSelect_Click(object sender, EventArgs e)
        {
            string SelectSql = "";
            switch (GetModel(grbSelectMode))
            {
                case "查询最新1000条数据":
                    SelectSql = "SELECT * FROM ProductInspectTable2 ORDER BY 日期 DESC LIMIT 1000";
                    break;
                case "按插入时间查询":
                    SelectSql = $"SELECT * FROM ProductInspectTable2 WHERE 日期 BETWEEN '{this.dateTimePickerStart.Text.Trim()}' AND '{this.dateTimePickerEnd.Text.Trim()}'";
                    break;
                case "按总结果+插入时间查询":
                    string StrResult1 = Rdb_OK.Checked ? Rdb_OK.Text.Trim() : Rdb_NG.Text.Trim();
                    SelectSql = $"SELECT * FROM ProductInspectTable2 WHERE 成品总结果='{StrResult1}' AND  日期 BETWEEN '{this.dateTimePickerStart.Text.Trim()}' AND '{this.dateTimePickerEnd.Text.Trim()}'";
                    break;
            }
            dt = GloabalTool.mysql_Select.ExecSQLQuery(SelectSql, null);
            bindingSource1.DataSource = dt;
            dataGridView1.DataSource = bindingSource1;
            labResultDisplay.Text = "本次共查询到" + dt.Rows.Count + "条数据记录";
            if (dt.Rows.Count > 0)
            {
                labResultDisplay.BackColor = Color.LimeGreen;
            }
            else
            {
                labResultDisplay.BackColor = Color.Yellow;
            }
        }
        /// <summary>
        /// 导出Csv按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//默认桌面路径
                saveFile.Title = "保存数据对话框";
                saveFile.Filter = "CSV文件|*.csv|所有文件|*.*";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string filepath = saveFile.FileName;
                    if (dt.Rows.Count == 0 || dt == null)
                    {
                        MessageBox.Show("查询的数据记录为空或0条，是无法保存CSV文件的！！！", "无法保存CSV文件", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (DataTableToCsv(dt, filepath))
                        {
                            MessageBox.Show("导出CSV文件成功");
                            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:导出SCV文件【{filepath}】成功");
                        }
                        else
                        {
                            MessageBox.Show("导出CSV文件失败");
                        }

                    }
                }
            }
        }
        /// <summary>
        /// 获取查询方式
        /// </summary>
        /// <param name="grp"></param>
        /// <returns></returns>
        private string GetModel(GroupBox grp)
        {
            List<RadioButton> Rb = new List<RadioButton>();
            Rb.Clear();
            for (int i = 0; i < grp.Controls.Count; i++)
            {
                if (grp.Controls[i] is RadioButton)
                {
                    Rb.Add((RadioButton)grp.Controls[i]);
                }
            }
            RadioButton tmp = Rb.Find(e => e.Checked);
            return tmp.Text.Trim();
        }
        /// <summary>
        /// DataTable导出CSV文件
        /// </summary>
        /// <param name="vContent">DataTable</param>
        /// <param name="vOutputFilePath">存储路径</param>
        /// <returns></returns>
        public bool DataTableToCsv(System.Data.DataTable vContent, string vOutputFilePath)
        {
            System.Text.StringBuilder sCsvContent;
            try
            {
                sCsvContent = new System.Text.StringBuilder();
                //栏位
                for (int i = 0; i < vContent.Columns.Count; i++)
                {
                    sCsvContent.Append(vContent.Columns[i].ColumnName);
                    sCsvContent.Append(i == vContent.Columns.Count - 1 ? "\r\n" : ",");
                }
                //数据
                foreach (System.Data.DataRow row in vContent.Rows)
                {
                    for (int i = 0; i < vContent.Columns.Count; i++)
                    {
                        sCsvContent.Append(row[i].ToString().Trim());
                        sCsvContent.Append(i == vContent.Columns.Count - 1 ? "\r\n" : ",");
                    }
                }
                File.WriteAllText(vOutputFilePath, sCsvContent.ToString(), Encoding.UTF8);
                return true;
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// DataTable转TXT文件
        /// </summary>
        /// <param name="vContent">DataTable</param>
        /// <param name="vOutputFilePath">存储路径</param>
        /// <returns></returns>
        public object DataTableToTXT(DataTable vContent, string vOutputFilePath)
        {
            object resObj;
            StringBuilder sTxtContent;

            try
            {
                if (File.Exists(vOutputFilePath))
                    File.Delete(vOutputFilePath);

                sTxtContent = new StringBuilder();

                //数据
                foreach (DataRow row in vContent.Rows)
                {
                    for (int i = 0; i < vContent.Columns.Count; i++)
                    {
                        sTxtContent.Append(row[i].ToString().Trim());
                        sTxtContent.Append(i == vContent.Columns.Count - 1 ? "\r\n" : "\t");
                    }
                }
                File.WriteAllText(vOutputFilePath, sTxtContent.ToString(), Encoding.Unicode);
                resObj = new object[] { 0, "OK" };
            }
            catch
            {
                resObj = new object[] { 0, "OK" };
            }
            return resObj;
        }

    }
}
