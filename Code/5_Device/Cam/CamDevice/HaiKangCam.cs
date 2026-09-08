using DataCollection_System._5_Device.Cam;
using MvCamCtrl.NET;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using static MvCamCtrl.NET.MyCamera;

namespace Vison_Inspect_System._5_Device.Cam.CamDevice
{
    /// <summary>
    /// 海康相机类实现
    /// </summary>
    public class HaiKangCam : ICam
    {
        private string _camName;
        /// <summary>
        /// 相机名称
        /// </summary>
        /// 
        public string CamName
        {
            get { return _camName; }
            set { _camName = value; }
        }


        /// <summary>
        /// 图像回调事件
        /// </summary>
        public event Action<ImageStream> NotifiImageEvent;
        /// <summary>
        /// 相机掉线回调事件
        /// </summary>
        public event Action<bool> NotifiCamOfflineEvent;


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


        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        MyCamera.cbOutputExdelegate cbImage;
        private MyCamera m_pMyCamera;
        int m_nDevNum;        // ch:在线设备数量 | en:Online Device Number
        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();
        private static Object BufForDriverLock = new Object();
        IntPtr m_BufForDriver = IntPtr.Zero;
        // ch:用于从驱动获取图像的缓存 | en:Buffer for getting image from driver
        UInt32 m_nBufSizeForDriver = 0;

        AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        public ImageStream currentImage;

        private void DeviceListAcq()
        {
            System.GC.Collect();
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                return;
            }
            m_nDevNum = (int)m_pDeviceList.nDeviceNum;
        }


        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        private string OpenDevice(string camName)
        {
            // ch:判断输入格式是否正确 | en:Determine whether the input format is correct
            cbImage = new MyCamera.cbOutputExdelegate(ImageCallBack);
            DeviceListAcq();
            int nRet = -1;
            MyCamera.MV_CC_DEVICE_INFO device = new MyCamera.MV_CC_DEVICE_INFO();
            bool IsHeatTime = false;
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {
                        if (gigeInfo.chUserDefinedName == camName)
                        {
                            IsHeatTime = true;
                            break;
                        }
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        if (usbInfo.chUserDefinedName == camName)
                        {
                            break;
                        }
                    }
                }
            }
            if (m_pMyCamera == null)
            {
                m_pMyCamera = new MyCamera();
            }
            //ch:打开设备 | en:Open Device
            nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);

            if (MyCamera.MV_OK != nRet)
            {
                return $"m_pMyCamera.MV_CC_CreateDevice_NET error:[{nRet}]";
            }

            nRet = m_pMyCamera.MV_CC_OpenDevice_NET();
            if (MyCamera.MV_OK != nRet)
            {
                m_pMyCamera.MV_CC_DestroyDevice_NET();
                return $"m_pMyCamera.MV_CC_OpenDevice_NET error:[{nRet}]";
            }
            // ch:探测网络最佳包大小(只对GigE相机有效)
            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                int nPacketSize = m_pMyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = m_pMyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MyCamera.MV_OK)
                    {
                        Console.WriteLine("Warning: Set Packet Size failed {0:x8}", nRet);
                    }
                }
                else
                {
                    Console.WriteLine("Warning: Get Packet Size failed {0:x8}", nPacketSize);
                }
            }
            if (IsHeatTime)
            {
                m_pMyCamera.MV_CC_SetIntValue_NET("GevHeartbeatTimeout", (uint)(20000));
            }
            //设置图像格式
            var ret = m_pMyCamera.MV_CC_SetEnumValue_NET("PixelFormat", (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed);
            if (ret != MyCamera.MV_OK)
            {
                return $"m_pMyCamera.MV_CC_SetEnumValue_NET(PixelFormat) error:[{nRet}]";
            }

            //设置相机触发模式
            ret = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            if (ret != MyCamera.MV_OK)
            {
                return $"m_pMyCamera.MV_CC_SetEnumValue_NET(TriggerMode) error:[{nRet}]";
            }

            ret = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            if (ret != MyCamera.MV_OK)
            {
                return $"m_pMyCamera.MV_CC_SetEnumValue_NET(TriggerSource) error:[{nRet}]";
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            ret = m_pMyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);

            if (MyCamera.MV_OK != nRet)
            {
                return $"m_pMyCamera.MV_CC_SetEnumValue_NET(AcquisitionMode) error:[{nRet}]";
            }
            //开始采集
            var err = StartGrab();
            return err;
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        ///   /// <returns></returns>
        private string StartGrab()
        {
            //m_stFrameInfo.nFrameLen = 0;//取流之前先清除帧长度
            //m_stFrameInfo.enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
            int ret = m_pMyCamera.MV_CC_RegisterImageCallBackEx_NET(cbImage, IntPtr.Zero);
            if (MyCamera.MV_OK != ret)
            {
                return $"MV_CC_RegisterImageCallBackEx_NET  error:{ret}";
            }
            int nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                return $"MV_CC_StartGrabbing_NET error:{nRet}";
            }
            return "";
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        private string CloseDevice()
        {
            try
            {
                if (m_pMyCamera == null)
                {
                    return "m_pMyCamera is null";
                }
                //停止采集
                var err = StopGrab();
                int nRet;
                nRet = m_pMyCamera.MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return $"err:MV_CC_CloseDevice_NET:{nRet}";
                }

                nRet = m_pMyCamera.MV_CC_DestroyDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return $"err:MV_CC_DestroyDevice_NET:{nRet}";
                }
                return err;
            }
            catch (Exception ex)
            {
                return $"err:{ex}";
            }

        }


        /// <summary>
        /// 停止采集
        /// </summary>
        ///   /// <returns></returns>
        private string StopGrab()
        {

            int nRet = m_pMyCamera.MV_CC_StopGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                return $"error:{nRet}";
            }
            return "";



        }

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <returns></returns>
        public string ConnectCamera()
        {
            var err = OpenDevice(_camName);
            return err;
        }

        /// <summary>
        /// 断开相机
        /// </summary>
        /// <returns></returns>
        public string DisconnectCamera()
        {
            var err = CloseDevice();
            return err;
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
                int nRet;
                if (IsOn)
                {
                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                }
                else
                {
                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                }
                if (MyCamera.MV_OK != nRet)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="exposureTimeValue">曝光时间</param>
        /// <returns></returns>
        public bool SetExposureTime(double exposureTimeValue)
        {
            double dMin = 12;                       //最小值
            double dMax = 9999999;                  //最大值
            try
            {
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
                m_pMyCamera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
                var nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTime", (float)exposureTimeValue);
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <param name="exposureTimeValue">曝光时间</param>
        /// <returns></returns>
        public bool GetExposureTime(out double exposureTimeValue)
        {
            exposureTimeValue = 0;
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                var nRet = m_pMyCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
                exposureTimeValue = stParam.fCurValue;
                return true;
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
            double dMin = 0;                       //最小值
            double dMax = 13.9651;                  //最大值
            try
            {
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
                m_pMyCamera.MV_CC_SetEnumValue_NET("GainAuto", 0);
                var nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("Gain", (float)gainValue);
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取相机增益参数
        /// </summary>
        /// <param name="gainValue">增益大小</param>
        /// <returns></returns>
        public bool GetGain(out double gainValue)
        {
            gainValue = 0;
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                var nRet = m_pMyCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
                gainValue = stParam.fCurValue;
                return true;

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
                AutoResetEvent.Reset();
                if (SwitchTrigMode(true))
                {
                    var nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                    if (nRet != MyCamera.MV_OK)
                    {
                        return null;
                    }

                    var timeoutflag = AutoResetEvent.WaitOne(10000);
                    if (timeoutflag)
                    {
                        return currentImage.buffBytes.Length>0?currentImage:null;
                    }
                }
                return null;

            }
            catch
            {
                return null;
            }
        }


        private void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            DateTime StartTime = DateTime.Now;
            int nIndex = (int)pUser;
            lock (BufForDriverLock)
            {
                if (m_BufForDriver == IntPtr.Zero || pFrameInfo.nFrameLen > m_nBufSizeForDriver)
                {
                    if (m_BufForDriver != IntPtr.Zero)
                    {
                        Marshal.Release(m_BufForDriver);
                        m_BufForDriver = IntPtr.Zero;
                    }

                    m_BufForDriver = Marshal.AllocHGlobal((Int32)pFrameInfo.nFrameLen);
                    if (m_BufForDriver == IntPtr.Zero)
                    {
                        return;
                    }
                    m_nBufSizeForDriver = pFrameInfo.nFrameLen;

                }
                CopyMemory(m_BufForDriver, pData, pFrameInfo.nFrameLen);
            }

            //用于显示图片
            MyCamera.MV_DISPLAY_FRAME_INFO stDisplayInfo = new MyCamera.MV_DISPLAY_FRAME_INFO();
            //#region 是否旋转图片
            stDisplayInfo.pData = pData;
            stDisplayInfo.nDataLen = pFrameInfo.nFrameLen;
            stDisplayInfo.nWidth = (ushort)pFrameInfo.nWidth;
            stDisplayInfo.nHeight = (ushort)pFrameInfo.nHeight;
            stDisplayInfo.enPixelType = pFrameInfo.enPixelType;

            //#endregion
            try
            {
                MV_FRAME_OUT frameOut = new MV_FRAME_OUT();
                frameOut.stFrameInfo = pFrameInfo;
                frameOut.pBufAddr = pData;
                ImageStream image = new ImageStream();
                image.Intp = frameOut.pBufAddr;
                image.width = (ushort)frameOut.stFrameInfo.nWidth;
                image.height = (ushort)frameOut.stFrameInfo.nHeight;
                image.buffBytes = new byte[pFrameInfo.nFrameLen];
                Marshal.Copy(frameOut.pBufAddr, image.buffBytes, 0, (int)frameOut.stFrameInfo.nFrameLen);
                image.IsColor = IsColorData(frameOut.stFrameInfo.enPixelType);
                image.CaptureTime = DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString();
                currentImage = image;
                AutoResetEvent.Set();
                NotifiImageEvent(currentImage);
             
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 判断是否黑白相机
        /// </summary>
        /// <param name="enGvspPixelType"></param>
        /// <returns></returns>
        /// 
        private Boolean IsMonoData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;

                default:
                    return false;
            }
        }

        /************************************************************************
        *  @fn     IsColorData()
        *  @brief  判断是否是彩色数据
        *  @param  enGvspPixelType         [IN]           像素格式
        *  @return 成功，返回true；错误，返回false 
        ************************************************************************/
        private Boolean IsColorData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return true;

                default:
                    return false;
            }
        }

    }
}
