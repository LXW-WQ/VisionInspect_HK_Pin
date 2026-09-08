using System;
using Vison_Inspect_System;

namespace DataCollection_System._5_Device.Cam
{
    /// <summary>
    /// 相机类接口
    /// </summary>
    public interface ICam
    {
        /// <summary>
        /// 图像回调事件
        /// </summary>
        event Action<ImageStream> NotifiImageEvent;

        /// <summary>
        /// 相机离线事件
        /// </summary>
        event Action<bool> NotifiCamOfflineEvent;

        /// <summary>
        /// 相机名称
        /// </summary>
        string CamName { get; set; }

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="camName">相机名称</param>
        /// <returns></returns>
        string ConnectCamera();

        /// <summary>
        /// 断开相机
        /// </summary>
        /// <returns></returns>
        string DisconnectCamera();

        /// <summary>
        /// 切换相机模式
        /// </summary>
        /// <returns></returns>
        bool SwitchTrigMode(bool IsOn);

        /// <summary>
        ///设置曝光时间
        /// </summary>
        /// <param name="exposureTime">曝光时间</param>
        /// <returns></returns>
        bool SetExposureTime(double exposureTime);

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain">增益数值</param>
        /// <returns></returns>
        bool SetGain(double gain);

        /// <summary>
        ///设置曝光时间
        /// </summary>
        /// <param name="exposureTime">曝光时间</param>
        /// <returns></returns>
        bool GetExposureTime(out double exposureTime);

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain">增益数值</param>
        /// <returns></returns>
        bool GetGain(out double gain);

        /// <summary>
        /// 发送软触发
        /// </summary>
        /// <returns></returns>
        ImageStream SofwareTrigger();

        /// <summary>
        /// 是否保存图片
        /// </summary>
        bool IsSavePitcure { get; set; }

        /// <summary>
        /// 保存图片路径
        /// </summary>
        string SavePicturePath { get; set; }

        /// <summary>
        /// 保存图片的格式
        /// </summary>
        ImageFormat SavePictureFormat { get; set; }

    }
}
