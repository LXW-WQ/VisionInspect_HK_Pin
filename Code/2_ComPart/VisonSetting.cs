using System;
using System.IO;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart.Normal_Form;

namespace Vison_Inspect_System._2_ComPart
{
    public partial class VisonSetting : Form
    {
        public RecipeConfig _currentRecipe = null;
        private string _productFullName = "";

        public VisonSetting(string _ProductFullName = "")
        {
            InitializeComponent();
            _productFullName = _ProductFullName;
        }

        private void VisonSetting_Load(object sender, EventArgs e)
        {

            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:打开配方参数设置界面");
            LabFileName.Text = _productFullName;
            if (LabFileName.Text == "")
            {
                //MessageBox.Show("空配方无法加载、请新建或打开现有配方");
                return;
            }
            if (GloabalTool.LoadRecipe(_productFullName).Item1 == false)
            {
                MessageBox.Show("配方加载失败、请新建或打开正确配方");
                return;
            }
            _currentRecipe = GloabalTool.LoadRecipe(_productFullName).Item2;
            if (UpdateConfigToUI() == false)
            {
                MessageBox.Show("配方加载失败");
                return;
            }

        }

        private void btnCreateRecipe_Click(object sender, EventArgs e)
        {
            this.LabFileName.Text = "";
            _currentRecipe = new RecipeConfig();
            if (UpdateConfigToUI() == true)
            {
                using (SaveFileDialog saveFile = new SaveFileDialog())
                {
                    saveFile.InitialDirectory = GloabalTool.ProductionCofigDir.ToString();//默认配方路径
                    saveFile.Title = "保存配方对话框";
                    saveFile.Filter = "Json文件|*.json|所有文件|*.*";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        _productFullName = saveFile.FileName;
                        LabFileName.Text = _productFullName;
                        MessageBox.Show("新建配方成功");
                        Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:创建配方【{_productFullName}】");
                    }
                }
            }
            else
            {
                MessageBox.Show("新建配方失败");
            }
        }
        private void btnOpenRecipe_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.InitialDirectory = GloabalTool.ProductionCofigDir.ToString();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                LabFileName.Text = OpenFile.FileName;
                if (GloabalTool.LoadRecipe(OpenFile.FileName).Item1 == false)
                {
                    MessageBox.Show("配方加载失败");
                    return;
                }
                _currentRecipe = GloabalTool.LoadRecipe(OpenFile.FileName).Item2;
                if (UpdateConfigToUI() == true)
                {
                    MessageBox.Show("配方加载成功");
                    Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:加载配方【{LabFileName.Text}】成功");
                }
                else
                {
                    MessageBox.Show("配方加载失败");
                }
            }
        }

        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            if (UpdateUIToConfig() == false)
            {
                MessageBox.Show("配方参数保存失败");
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:配方参数保存【{LabFileName.Text}】失败");
                return;
            }

            if (LabFileName.Text == "")
            {
                MessageBox.Show("空配方无法保存、请新建配方或加载配方");
                return;
            }

            if (System.IO.File.Exists(LabFileName.Text))
                System.IO.File.Delete(LabFileName.Text);
            string fileName = Path.GetFileName(LabFileName.Text);
            if (GloabalTool.SaveRecipe(LabFileName.Text, _currentRecipe) == false)
            {
                MessageBox.Show("配方保存失败");
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:配方参数保存【{LabFileName.Text}】失败");
                return;
            }
            else
            {
                MessageBox.Show("配方保存成功");
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:配方参数保存【{LabFileName.Text}】成功");
            }
        }



        private void VisonSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:关闭配方参数设置界面");
            this.Close();
        }



        private bool UpdateConfigToUI()
        {
            try
            {
                labVisonProjectPath.Text = _currentRecipe.VisionProjectPath;
                nudExposure.Value = (decimal)_currentRecipe.Exposure;
                nudGain.Value = (decimal)_currentRecipe.GainValue;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool UpdateUIToConfig()
        {
            try
            {
                _currentRecipe.VisionProjectPath = labVisonProjectPath.Text;
                _currentRecipe.Exposure = (double)nudExposure.Value;
                _currentRecipe.GainValue = (double)nudGain.Value;
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        private void btnGetCamExposure_Click(object sender, EventArgs e)
        {
            if (GloabalTool.CamDic.ContainsKey(CameraType.Camera1))
            {
                GloabalTool.CamDic[CameraType.Camera1].CamDev.GetExposureTime(out double exposureTime);
                nudExposure.Value = (decimal)exposureTime;
            }

        }

        private void btnSetCamExposure_Click(object sender, EventArgs e)
        {
            if (GloabalTool.CamDic.ContainsKey(CameraType.Camera1))
            {
                var flag = GloabalTool.CamDic[CameraType.Camera1].CamDev.SetExposureTime((double)nudExposure.Value);
                if (flag)
                {
                    MessageBox.Show($"设置相机【{CameraType.Camera1.ToString()}】曝光参数值【{nudGain.Value}】成功");
                }
            }
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            if (GloabalTool.CamDic.ContainsKey(CameraType.Camera1))
            {
                GloabalTool.CamDic[CameraType.Camera1].CamDev.GetGain(out double gain);
                nudGain.Value = (decimal)gain;
            }
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            if (GloabalTool.CamDic.ContainsKey(CameraType.Camera1))
            {
                var flag = GloabalTool.CamDic[CameraType.Camera1].CamDev.SetGain((double)nudGain.Value);
                if (flag)
                {
                    MessageBox.Show($"设置相机【{CameraType.Camera1.ToString()}】增益参数值【{nudGain.Value}】成功");
                }
            }
        }

        private void btnSelectVisonProject_Click(object sender, EventArgs e)
        {
            if (_currentRecipe == null)
            {
                MessageBox.Show("配方为空、请先选择配方或新建配方！！");
                return;
            }
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                labVisonProjectPath.Text = op.FileName;
                _currentRecipe.VisionProjectPath = labVisonProjectPath.Text;
            }
        }
    }
}
