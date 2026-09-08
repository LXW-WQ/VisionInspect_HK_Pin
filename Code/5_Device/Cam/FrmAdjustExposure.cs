using DataCollection_System._5_Device.Cam;
using System;
using System.Windows.Forms;

namespace Vison_Inspect_System._5_Device.Cam
{
    public partial class FrmAdjustExposure : Form
    {
        public FrmAdjustExposure()
        {
            InitializeComponent();
        }

        public ICam CamDev { get; set; }

        public bool isSetGain { get; set; }


        private void FrmAdjustExposure_Load(object sender, EventArgs e)
        {
            labTitle.Text = isSetGain ? "增益值调整" : "曝光值调整";
            double CurrentValue = isSetGain ? GteCurrentGainValue() : GteCurrentExposureTimeValue();
            textBox_SetValue1.Text = CurrentValue.ToString();
            lTrackBar1.L_Value = (int)CurrentValue;
            textBox_SetValue1.KeyPress += TextBox_SetValue1_KeyPress;
            lTrackBar1.LValueChanged += LTrackBar1_LValueChanged;
        }

        private void LTrackBar1_LValueChanged(object sender, LEventArgs e)
        {
            textBox_SetValue1.Text = Convert.ToInt32(e.Value.ToString()).ToString();
            try
            {
                double value = Convert.ToDouble(e.Value);
                if (isSetGain)
                {
                    SetGainValue(value);
                }
                else
                {
                    SetExposureTimeValue(value);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 设置曝光时间值
        /// </summary>
        private void SetExposureTimeValue(double exposure)
        {
            if (CamDev != null)
            {
                CamDev.SetExposureTime(exposure);
            }
        }


        /// <summary>
        /// 设置相机增益值
        /// </summary>
        private void SetGainValue(double Gain)
        {
            if (CamDev != null)
            {
                CamDev.SetGain(Gain);
            }
        }

        /// <summary>
        /// 获取当前曝光时间值
        /// </summary>
        /// <returns></returns>
        private double GteCurrentExposureTimeValue()
        {
            double exporsure = 0;
            if (CamDev != null)
            {
                CamDev.GetExposureTime(out exporsure);
            }
            return exporsure;
        }

        /// <summary>
        /// 获取当前相机增益值
        /// </summary>
        /// <returns></returns>
        private double GteCurrentGainValue()
        {
            double gain = 0;
            if (CamDev != null)
            {
                CamDev.GetGain(out gain);
            }
            return gain;
        }

        private void TextBox_SetValue1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    lTrackBar1.L_Value = Convert.ToInt32(textBox_SetValue1.Text.Trim());
                }
                catch
                { }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
