using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vison_Inspect_System._2_ComPart.Normal_Form
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }
        public LoadingForm(byte maxValue) : this()
        {
            progressBarLoadProgress.Maximum = maxValue;
            progressBarLoadProgress.Step = 1;
            labLoadingInfo.Text = "加载中";
        }
        private void LoadingForm_Load(object sender, EventArgs e)
        {
          labCompanyInfo.Text = "\r\n\r\n公司名称：深圳宁达自动化设备有限公司\r\n\r\n\r\n苏州分公司\r\n\r\n\r\n手机号：18851567102\r\n\r\n\r\n  ";
        }
        public void ShowMessage(string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), str);
            }
            else
            {
                labLoadingInfo.Text = str;
                progressBarLoadProgress.PerformStep();
                if (progressBarLoadProgress.Value >= progressBarLoadProgress.Maximum)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
