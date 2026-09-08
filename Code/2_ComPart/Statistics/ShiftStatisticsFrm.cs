using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart.Normal_Form;

namespace Vison_Inspect_System._2_ComPart.Statistics
{
    public partial class ShiftStatisticsFrm : Form
    {
        public ShiftStatisticsFrm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        DataTable dt = new DataTable();
        private void ShiftStatisticsFrm_Load(object sender, EventArgs e)
        {
            GetSqlData();
        }
        //从数据库获取各个班次的数据
        private void GetSqlData()
        {
            string selectSql = $"SELECT * FROM statisticstable2";
            dt = GloabalTool.mysql_Insert.ExecSQLQuery(selectSql, null);
            bindingSource1.DataSource = dt;
            dgStatistics.DataSource = bindingSource1;
        }

        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            GetSqlData();
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
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


    }
}
