using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart;
using Vison_Inspect_System._2_ComPart.Normal_Form;

namespace Vison_Inspect_System._2_ComPart
{
    public partial class ParameterForm : Form
    {
        public ParameterForm(string title,object ParaSetting,string SaveAddress)
        {
            InitializeComponent();
            this.Text = title;
            propertyGrid1.SelectedObject = ParaSetting;
            propertyGrid1.Tag = SaveAddress;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path.GetDirectoryName( propertyGrid1.Tag.ToString())))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(propertyGrid1.Tag.ToString()));
            }
            if (!File.Exists(propertyGrid1.Tag.ToString()))
            {
                FileStream fs = File.Create(propertyGrid1.Tag.ToString());
                fs.Close();
            }
            STE.Common.XML.XMLHelper.Serializer(propertyGrid1.Tag.ToString(), propertyGrid1.SelectedObject, propertyGrid1.SelectedObject.GetType());
            MessageBox.Show("参数保存成功");
            GloabalTool.EquipmentConfigRefresh();
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:对系统参数进行保存");
            this.Close();
        }
        private void btnCancell_Click(object sender, EventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:关闭系统参数设置界面");
            this.Close();
        }

        private void ParameterForm_Load(object sender, EventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:打开系统参数设置界面");
        }
    }
}
