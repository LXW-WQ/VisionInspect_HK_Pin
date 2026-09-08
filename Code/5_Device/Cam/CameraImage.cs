using DataCollection_System._5_Device.Cam;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionDesigner;
using Vison_Inspect_System._2_ComPart;
using Vison_Inspect_System._2_ComPart.Normal_Form;
using Vison_Inspect_System._5_Device.Cam;
using VM.PlatformSDKCS;
using static Vison_Inspect_System._6_Process.VisionAlgorithm;

namespace Vison_Inspect_System
{
    public delegate void OnInspectionHandler(object o1, object o2, object o3, object obj4, CMvdImage curImage);
    public partial class CameraImage : UserControl
    {
        public CameraImage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 相机设备接口
        /// </summary>
        public ICam CamDev { get; set; }
        /// <summary>
        /// 相机参数
        /// </summary>
        public CameraConfig CamConfig { get; set; }


        private bool _camConnectStatus;
        /// <summary>
        /// 相机连接状态
        /// </summary>
        public bool CamConnectStatus
        {
            get { return _camConnectStatus; }
        }



        /// <summary>
        /// 相机离线事件
        /// </summary>
        public event Action<bool> NotifiCamOfflineEvent;


        public CMvdImage SaveImage = new CMvdImage();

        /// <summary>
        /// 相机对象初始化
        /// </summary>
        /// <returns></returns>
        public string Init()
        {
            var err = string.Empty;
            if (CamDev == null)
            {
                return "相机对象为null、未进行声明实例化";
            }
            if (CamConfig == null)
            {
                return "相机参数为null、未进行声明实例化";
            }
            CamDev.NotifiImageEvent -= LoadImage;
            CamDev.NotifiImageEvent += LoadImage;
            CamDev.NotifiCamOfflineEvent -= CamOffline;
            CamDev.NotifiCamOfflineEvent += CamOffline;
            err = SetConnect();
            if (err == string.Empty)
            {
                _camConnectStatus = true;
            }
            else
            {
                _camConnectStatus = false;
            }
            return err;
        }

        /// <summary>
        /// 设置相机名称
        /// </summary>
        /// <param name="CameraName"></param>
        public void SetCameraName(string CameraName)
        {
            Cameralabel.Text = CameraName;
        }

        /// <summary>
        /// 获取图片流、与相机的图像回调事件绑定、获取图片流对象
        /// </summary>
        /// <param name="imageStream"></param>
        private void LoadImage(ImageStream imageStream)
        {
            try
            {
                CMvdImage Image = CCDToCMvdImage(imageStream);
                ImagemvdRenderActivex.ClearImages();
                ImagemvdRenderActivex.LoadImageFromObject(Image);
                ImagemvdRenderActivex.ClearShapes();
                SaveImage = Image.Clone();
                ImagemvdRenderActivex.Display();
                RefreshCaptureTime(imageStream.CaptureTime);
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }

        /// <summary>
        /// 与相机的掉线事件绑定
        /// </summary>
        /// <param name="isOffline"></param>
        private void CamOffline(bool isOffline)
        {
            _camConnectStatus = !isOffline;
            NotifiCamOfflineEvent.Invoke(isOffline);
        }

        /// <summary>
        /// 设置相机连接
        /// </summary>
        /// <param name="IsConnect"></param>
        public string SetConnect()
        {
            if (CamDev == null)
            {
                return "相机对象为null、未进行声明实例化";
            }
            if (CamConfig == null)
            {
                return "相机参数为null、未进行声明实例化";
            }
            CamDev.CamName = CamConfig.CamName;
            switch (CamConfig.CameraType)
            {
                case CameraType.Camera1:
                    SetCameraName("注塑检测相机");
                    break;
                default:
                    SetCameraName("相机1");
                    break;
            }

            var err = CamDev.ConnectCamera();
            if (err == "")
            {
                Cameralabel.BackColor = Color.LightGreen;
            }
            else
            {
                Cameralabel.BackColor = Color.Red;
            }
            return err;
        }
        private string SetContinue()
        {
            if (CamDev == null)
            {
                return "相机对象为null、未进行声明实例化";
            }
            var cnFlag = CamDev.SwitchTrigMode(false);
            if (!cnFlag)
            {
                return "相机开启实时失败";
            }
            return "";
        }

        private string SetStop()
        {
            if (CamDev == null)
            {
                return "相机对象为null、未进行声明实例化";
            }
            var cnFlag = CamDev.SwitchTrigMode(true);
            if (!cnFlag)
            {
                return "相机关闭实时失败";
            }
            return "";
        }


        private void CameraNameLabel_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshCaptureTime(string CaptureTime)
        {
            this.Invoke(new Action(() =>
            {
                label1.Text = "CaptureTime(ms):" + CaptureTime;
            }));
        }

        private void TriggerToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (CamDev == null)
            {
                MessageBox.Show("相机对象为null、未进行声明实例化");
                return;
            }
            ImageStream imageStream = CamDev.SofwareTrigger();
            if(imageStream==null)
            {
                MessageBox.Show("获取图片数据流对象为null,请检查相机是否正常");
                return;
            }

            TriggerOnceInspection(CamConfig, imageStream);//执行算法处理

        }

        private void ExposureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CamDev == null)
            {
                MessageBox.Show("相机对象为null、未进行声明实例化");
                return;
            }
            FrmAdjustExposure FrmAdjust = new FrmAdjustExposure();
            FrmAdjust.CamDev = CamDev;
            FrmAdjust.isSetGain = false;
            FrmAdjust.Show();
        }

        private void SaveImageToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveImageDialog = new SaveFileDialog();
                saveImageDialog.Filter = "Image Files (*.BMP)|*BMP";
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveImage.SaveImage(saveImageDialog.FileName + ".bmp", MVD_FILE_FORMAT.MVD_FILE_BMP);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveImg(CameraConfig cameraConfig)
        {
            new Task(() => {
                string timeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                switch (cameraConfig.SavePictureFormat)
                {
                
                    case ImageFormat.BMP:
                        SaveImage.SaveImage(cameraConfig.SavePicturePath + timeStr+ ".bmp", MVD_FILE_FORMAT.MVD_FILE_BMP);
                        break;
                    case ImageFormat.JPG:
                        SaveImage.SaveImage(cameraConfig.SavePicturePath + timeStr + ".jpg", MVD_FILE_FORMAT.MVD_FILE_JPEG);
                        break;
                    case ImageFormat.PNG:
                        SaveImage.SaveImage(cameraConfig.SavePicturePath + timeStr + ".png", MVD_FILE_FORMAT.MVD_FILE_PNG);
                        break;
                    default:
                        SaveImage.SaveImage(cameraConfig.SavePicturePath + timeStr + ".png", MVD_FILE_FORMAT.MVD_FILE_PNG);
                        break;
                }
            }).Start();
            
        }
        private void SetGainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CamDev == null)
            {
                MessageBox.Show("相机对象为null、未进行声明实例化");
                return;
            }
            FrmAdjustExposure FrmAdjust = new FrmAdjustExposure();
            FrmAdjust.CamDev = CamDev;
            FrmAdjust.isSetGain = true;
            FrmAdjust.Show();
        }
        private void btnOpenLive_Click(object sender, EventArgs e)
        {
            var err = SetContinue();
            if (err != "")
            {
                MessageBox.Show(err);
                return;
            }
        }

        private void btnCloseLive_Click(object sender, EventArgs e)
        {
            var err = SetStop();
            if (err != "")
            {
                MessageBox.Show(err);
                return;
            }
        }

        /// <summary>
        /// 将图片数据流转换成控件显示的格式
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private CMvdImage CCDToCMvdImage(ImageStream image)
        {
            VisionDesigner.CMvdImage cMvdImage = new VisionDesigner.CMvdImage();
            VisionDesigner.MVD_IMAGE_DATA_INFO stImageData = new VisionDesigner.MVD_IMAGE_DATA_INFO();
            stImageData.stDataChannel[0].nLen = (uint)(image.buffBytes.Length);
            stImageData.stDataChannel[0].nSize = (uint)(image.buffBytes.Length);
            byte[] m_BufForDriver1 = new byte[image.buffBytes.Length];
            //数据Copy
            Marshal.Copy(image.Intp, m_BufForDriver1, 0, (int)(image.buffBytes.Length));
            stImageData.stDataChannel[0].arrDataBytes = m_BufForDriver1;
            if (image.IsColor)
            {
                stImageData.stDataChannel[0].nRowStep = (uint)image.width * 3;
                //初始化CMvdImage
                cMvdImage.InitImage((uint)image.width, (uint)image.height, MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3, stImageData);
            }
            else
            {
                stImageData.stDataChannel[0].nRowStep = (uint)image.width;
                //初始化CMvdImage
                cMvdImage.InitImage((uint)image.width, (uint)image.height, MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08, stImageData);
            }
            return cMvdImage;
        }

        /// <summary>
        /// 转换成VM所用的基础格式
        /// </summary>
        /// <param name="frameOut"></param>
        /// <returns></returns>
        private ImageBaseData CCDToImageBaseData(ImageStream image)
        {
            ImageBaseData imageBaseData = new ImageBaseData();
            imageBaseData.Width = image.width;
            imageBaseData.Height = image.height;
            imageBaseData.DataLen = 3 * ((uint)image.buffBytes.Length) + 2048;// 3 * stFrameInfo.nDataLen + 2048
            if (image.IsColor)
            {
                imageBaseData.Pixelformat = (int)VMPixelFormat.VM_PIXEL_RGB24_C3;
            }
            else
            {
                imageBaseData.Pixelformat = (int)VMPixelFormat.VM_PIXEL_MONO_08;
            }
            imageBaseData.ImageData = new byte[image.buffBytes.Length];
            Marshal.Copy(image.Intp, imageBaseData.ImageData, 0, (int)image.buffBytes.Length);
            return imageBaseData;
        }

        /// <summary>
        /// 触发一次拍照并进行视觉检测
        /// </summary>
        /// <param name="cameraConfig"></param>
        /// <returns></returns>
        public (string, InspectionResult) TriggerOnceInspection(CameraConfig cameraConfig, ImageStream image)
        {
            var err = string.Empty;
            if (cameraConfig == null)
            {
                err = "相机参数对象为空失败报警";
                Log.SaveLog(LogType.comm, err);
                return (err, null);
            }
            if (image == null)
            {
                err = "图片对象为空、未成功获取图片对象";
                Log.SaveLog(LogType.comm, err);
                return (err, null);
            }
            Log.SaveLog(LogType.Operate, "开始执行算法处理");

            ImageBaseData imgBaseData = CCDToImageBaseData(image);
            InspectionResult result = GloabalTool.VisionAlgorithm.RunInspection(imgBaseData, cameraConfig);//执行算法处理
            CameraType camtype = cameraConfig.CameraType;
            switch (camtype)
            {
                case CameraType.Camera1:

                    break;
                default:
                    break;
            }
            Log.SaveLog(LogType.Operate, "执行算法处理结束");
            return (err, result);
        }


    }
}
