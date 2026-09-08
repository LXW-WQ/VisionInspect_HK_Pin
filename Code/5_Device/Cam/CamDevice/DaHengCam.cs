
using DataCollection_System._5_Device.Cam;
using GxIAPINET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Vison_Inspect_System._5_Device.Cam.CamDevice
{
    /// <summary>
    /// 大恒相机类
    /// </summary>
    public class DaHengCam : ICam
    {
        /// <summary>
        /// <掉线回调句柄
        /// </summary>
        GX_DEVICE_OFFLINE_CALLBACK_HANDLE m_hCB = null;

        /// <summary>
        /// 发送开采命令标识
        /// </summary>
        bool m_bIsSnap = false;

        /// <summary>
        /// Factory对像
        /// </summary>
        IGXFactory m_objIGXFactory = null;

        /// <summary>
        /// 设备对像
        /// </summary>
        IGXDevice m_objIGXDevice = null;

        /// <summary>
        /// 流对像
        /// </summary>
        IGXStream m_objIGXStream = null;

        /// <summary>
        /// 远端设备属性控制器对像
        /// </summary>
        IGXFeatureControl m_objIGXFeatureControl = null;

        /// <summary>
        /// 流层属性控制器对象
        /// </summary>
        IGXFeatureControl m_objIGXStreamFeatureControl = null;

        /// <summary>
        /// 图像流
        /// </summary>
        ImageStream _imageInstance = null;

        /// <summary>
        /// 是否是彩色相机
        /// </summary>
        private bool _isColorCam = false;

        private ManualResetEvent _AcqImageResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// 图像回调事件
        /// </summary>
        public event Action<ImageStream> NotifiImageEvent;

        /// <summary>
        /// 相机掉线回调事件
        /// </summary>
        public event Action<bool> NotifiCamOfflineEvent;

        private string _camName;
        /// <summary>
        /// 相机名称
        /// </summary>
        public string CamName
        {
            get { return _camName; }
            set { _camName = value; }
        }

        private bool _isSavePicture;
        /// <summary>
        /// 是否保存图片
        /// </summary>
        public bool IsSavePitcure
        {
            get { return _isSavePicture; }
            set { _isSavePicture = value; }
        }


        private string _savePicturePath;
        /// <summary>
        /// 保存图片路径
        /// </summary>
        public string SavePicturePath
        {
            get { return _savePicturePath; }
            set { _savePicturePath = value; }
        }

        private ImageFormat _savePictureFormat = ImageFormat.JPG;
        /// <summary>
        /// 保存图片的格式
        /// </summary>
        public ImageFormat SavePictureFormat
        {
            get { return _savePictureFormat; }
            set { _savePictureFormat = value; }
        }

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <returns></returns>
        public string ConnectCamera()
        {
            var err = OpenDevice(_camName);
            if (err != "")
            {
                return err;
            }
            err = StartDevice();
            SetTriggerSource();//设置触发源为软触发
            return err;
        }

        /// <summary>
        /// 断开相机
        /// </summary>
        /// <returns></returns>
        public string DisconnectCamera()
        {
            var err = CloseDeviceAndStream();
            return err;
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="camName"></param>
        /// <returns></returns>
        private string OpenDevice(string camName)
        {
            try
            {
                List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();

                if (m_objIGXFactory == null)
                {
                    m_objIGXFactory = IGXFactory.GetInstance();
                    m_objIGXFactory.Init();
                }
                //关闭流
                CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                CloseDevice();

                m_objIGXFactory.UpdateAllDeviceList(200, listGXDeviceInfo);

                if (listGXDeviceInfo.Count == 0)
                {
                    return "未发现设备";
                }

                //打开相机设备
                m_objIGXDevice = m_objIGXFactory.OpenDeviceByUserID(camName, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                IsSupportColor(ref _isColorCam, m_objIGXDevice);
                m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();
                //设置心跳超时时间 5 分钟
                m_objIGXFeatureControl.GetIntFeature("GevHeartbeatTimeout").SetValue(300000);
                //打开流
                if (null != m_objIGXDevice)
                {
                    m_objIGXStream = m_objIGXDevice.OpenStream(0);
                    m_objIGXStreamFeatureControl = m_objIGXStream.GetFeatureControl();
                }
                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能,设置方法参考以下代码。
                GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
                if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                {
                    // 判断设备是否支持流通道数据包功能
                    if (true == m_objIGXFeatureControl.IsImplemented("GevSCPSPacketSize"))
                    {
                        // 获取当前网络环境的最优包长值
                        uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                        // 将最优包长值设置为当前设备的流通道包长值
                        m_objIGXFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                    }
                }
                InitDevice();
                if (null != m_objIGXDevice)
                {
                    //RegisterDeviceOfflineCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    m_hCB = m_objIGXDevice.RegisterDeviceOfflineCallback(null, OnDeviceOfflineCallbackFun);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        ///   /// <returns></returns>
        private string StartDevice()
        {
            try
            {
                if (null != m_objIGXStreamFeatureControl)
                {
                    try
                    {
                        //设置流层Buffer处理模式为OldestFirst
                        m_objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                    }
                    catch (Exception)
                    {
                    }
                }

                //开启采集流通道
                if (null != m_objIGXStream)
                {
                    //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    m_objIGXStream.RegisterCaptureCallback(this, CaptureCallbackPro);
                    m_objIGXStream.StartGrab();
                }

                //发送开采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }
                m_bIsSnap = true;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 停止采集 
        /// </summary>
        /// <returns></returns>
        private string StopDevice()
        {
            try
            {

                //发送停采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                }

                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();

                }

                m_bIsSnap = false;

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 关闭流并关闭设备
        /// </summary>
        /// <returns></returns>
        private string CloseDeviceAndStream()
        {
            try
            {
                try
                {
                    //如果未停采则先停止采集
                    if (m_bIsSnap)
                    {
                        if (null != m_objIGXFeatureControl)
                        {
                            m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                            m_objIGXFeatureControl = null;
                        }
                    }
                }
                catch (Exception)
                {

                }
                m_bIsSnap = false;
                try
                {
                    //停止流通道、注销采集回调和关闭流
                    if (null != m_objIGXStream)
                    {
                        m_objIGXStream.StopGrab();
                        //注销采集回调函数
                        m_objIGXStream.UnregisterCaptureCallback();
                        m_objIGXStream.Close();
                        m_objIGXStream = null;
                        m_objIGXStreamFeatureControl = null;
                    }
                }
                catch (Exception)
                {

                }
                CloseDevice();
                // 注销设备掉线事件回调
                Console.WriteLine("<Unregister device Offline callback>");
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.UnregisterDeviceOfflineCallback(m_hCB);
                }
                try
                {
                    //反初始化
                    if (null != m_objIGXFactory)
                    {
                        m_objIGXFactory.Uninit();
                        m_objIGXFactory = null;
                    }
                }
                catch (Exception)
                {

                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// 回调函数,用于获取图像信息和显示图像
        /// </summary>
        /// <param name="obj">用户自定义传入参数</param>
        /// <param name="objIFrameData">图像信息对象</param>
        private void CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                DateTime StartTime = DateTime.Now;
                if (_imageInstance == null)
                {
                    _imageInstance = new ImageStream();
                }
                _imageInstance.width = (int)objIFrameData.GetWidth();
                _imageInstance.height = (int)objIFrameData.GetHeight();
                if (_isColorCam)
                {
                    IntPtr intColor = objIFrameData.ConvertToRGB24(GX_VALID_BIT_LIST.GX_BIT_0_7, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                    _imageInstance.Intp = intColor;
                    _imageInstance.IsColor = true;
                }
                else
                {
                    IntPtr intMone = objIFrameData.GetBuffer();
                    _imageInstance.Intp = intMone;
                    _imageInstance.IsColor = false;
                }
                _AcqImageResetEvent.Set();
                _imageInstance.CaptureTime = DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString();
                NotifiImageEvent(_imageInstance);

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        private void CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void CloseDevice()
        {
            try
            {
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }

            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 相机初始化
        /// </summary>
        private void InitDevice()
        {
            if (null != m_objIGXFeatureControl)
            {
                //设置采集模式连续采集
                m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
            }
        }

        /// <summary>
        /// 是否支持彩色
        /// </summary>
        /// <param name="bIsColorFilter">是否支持彩色</param>
        private void IsSupportColor(ref bool bIsColorFilter, IGXDevice device)
        {
            bool bIsImplemented = false;
            bool bIsMono = false;
            string strPixelFormat = "";

            strPixelFormat = device.GetRemoteFeatureControl().GetEnumFeature("PixelFormat").GetValue();
            if (0 == string.Compare(strPixelFormat, 0, "Mono", 0, 4))
            {
                bIsMono = true;
            }
            else
            {
                bIsMono = false;
            }
            bIsImplemented = device.GetRemoteFeatureControl().IsImplemented("PixelFormatFilter");
            if ((!bIsMono) && (!bIsImplemented))
            {
                bIsColorFilter = true;
            }
            else
            {
                bIsColorFilter = false;
            }
        }

        /// <summary>
        /// 掉线回调函数
        /// </summary>
        /// <param name="pUserParam">用户私有参数</param>
        private void OnDeviceOfflineCallbackFun(object pUserParam)
        {
            try
            {
                NotifiCamOfflineEvent(true);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 切换触发模式
        /// </summary>
        /// <returns></returns>
        public bool SwitchTrigMode()
        {
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    //获取触发模式
                    IEnumFeature pEnumFeature = m_objIGXFeatureControl.GetEnumFeature("TriggerMode");
                    if (null != pEnumFeature)
                    {
                        //获取当前的触发模式
                        string strTriggerMode = pEnumFeature.GetValue();
                        if ("On" == strTriggerMode)
                        {
                            //设置触发模式为Off
                            pEnumFeature.SetValue("Off");
                        }
                        else
                        {
                            //设置触发模式为On
                            pEnumFeature.SetValue("On");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 切换触发模式
        /// </summary>
        /// <param name="IsOn">true：置ON；false：置off</param>
        /// <returns></returns>
        public bool SwitchTrigMode(bool IsOn)
        {
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    //获取触发模式
                    IEnumFeature pEnumFeature = m_objIGXFeatureControl.GetEnumFeature("TriggerMode");
                    if (null != pEnumFeature)
                    {
                        //获取当前的触发模式
                        string strTriggerMode = pEnumFeature.GetValue();
                        if (IsOn)
                        {
                            //设置触发模式为On
                            pEnumFeature.SetValue("On");
                        }
                        else
                        {
                            //设置触发模式为Off
                            pEnumFeature.SetValue("Off");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="exposureTimeValue">曝光时间</param>
        /// <returns></returns>
        public bool SetExposureTime(double exposureTimeValue)
        {
            double dMin = 0.0;                       //最小值
            double dMax = 0.0;                       //最大值
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    IFloatFeature pFloatFeature = m_objIGXFeatureControl.GetFloatFeature("ExposureTime");
                    if (null != pFloatFeature)
                    {
                        dMin = pFloatFeature.GetMin();
                        dMax = pFloatFeature.GetMax();
                        //判断输入值是否在曝光时间的范围内
                        //若大于最大值则将曝光值设为最大值
                        if (exposureTimeValue > dMax)
                        {
                            exposureTimeValue = dMax;
                        }
                        //若小于最小值将曝光值设为最小值
                        if (exposureTimeValue < dMin)
                        {
                            exposureTimeValue = dMin;
                        }
                        m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(exposureTimeValue);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取相机曝光时间
        /// </summary>
        /// <param name="exposureTimeValue">曝光时间</param>
        /// <returns></returns>
        public bool GetExposureTime(out double exposureTimeValue)
        {
            exposureTimeValue = 0;
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    exposureTimeValue = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetValue();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gainValue">增益大小</param>
        /// <returns></returns>
        public bool SetGain(double gainValue)
        {
            double dMin = 0.0;                       //最小值
            double dMax = 0.0;                       //最大值
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    IFloatFeature pFloatFeature = m_objIGXFeatureControl.GetFloatFeature("Gain");
                    if (null != pFloatFeature)
                    {
                        dMin = pFloatFeature.GetMin();
                        dMax = pFloatFeature.GetMax();
                        //判断输入值是否在增益的范围内
                        //若大于最大值则将增益值设为最大值
                        if (gainValue > dMax)
                        {
                            gainValue = dMax;
                        }
                        //若小于最小值将增益值设为最小值
                        if (gainValue < dMin)
                        {
                            gainValue = dMin;
                        }
                        m_objIGXFeatureControl.GetFloatFeature("Gain").SetValue(gainValue);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取相机增益值
        /// </summary>
        /// <param name="gainValue">增益大小</param>
        /// <returns></returns>
        public bool GetGain(out double gainValue)
        {
            gainValue = 0;
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    gainValue = m_objIGXFeatureControl.GetFloatFeature("Gain").GetValue();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置触发源为软触发
        /// </summary>
        private bool SetTriggerSource()
        {
            try
            {
                if (null != m_objIGXFeatureControl)
                {
                    //获取触发源
                    IEnumFeature pEnumFeature = m_objIGXFeatureControl.GetEnumFeature("TriggerSource");
                    if (null != pEnumFeature)
                    {
                        //设置触发源为软触发
                        pEnumFeature.SetValue("Software");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送软触发命令
        /// </summary>
        /// <returns></returns>
        public ImageStream SofwareTrigger()
        {
            try
            {
                _AcqImageResetEvent.Reset();
                if (null != m_objIGXFeatureControl)
                {
                    //发送软触发命令
                    m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }
                bool isTimeOut = _AcqImageResetEvent.WaitOne(1000);
                int indx = 0;
                if (!isTimeOut)
                {
                    while (true)
                    {
                        SwitchTrigMode(true);
                        Thread.Sleep(500);
                        if (null != m_objIGXFeatureControl)
                        {
                            //发送软触发命令
                            m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                        }
                        indx++;
                        var istimeout2 = _AcqImageResetEvent.WaitOne(1000);
                        if (istimeout2)
                        {
                            break;
                        }
                        if (_imageInstance.width > 0 || indx >= 3)
                        {
                            break;
                        }
                    }
                }
                //if (_isSavePicture && _imageInstance!=null)
                //{
                //    SaveImage(_savePicturePath);
                //}
                return _imageInstance;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool SaveImage(string path)
        {
            Bitmap bitmap = null;
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (_imageInstance == null)
                {
                    return false;
                }
                if (_imageInstance.IsColor)
                {
                    bitmap = new Bitmap(_imageInstance.width, _imageInstance.height, _imageInstance.width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, _imageInstance.Intp);
                }
                else
                {
                    bitmap = new Bitmap(_imageInstance.width, _imageInstance.height, _imageInstance.width, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, _imageInstance.Intp);

                    System.Drawing.Imaging.ColorPalette palette = bitmap.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        palette.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
                    }
                    bitmap.Palette = palette;
                }
                string strDateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                string fullName = System.IO.Path.Combine(path, strDateTime);

                Bitmap img = new Bitmap(bitmap);
                Graphics draw = Graphics.FromImage(img);
                draw.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                bitmap.Dispose();
                switch (_savePictureFormat)
                {
                    case ImageFormat.BMP:
                        {
                            fullName += ".bmp";
                            img.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        }
                    case ImageFormat.JPG:
                        {
                            fullName += ".jpg";

                            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        }
                    case ImageFormat.PNG:
                        {
                            fullName += ".png";
                            img.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        }
                }
                img.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // 释放 Bitmap 资源
                bitmap?.Dispose();
            }
        }
    }
}
