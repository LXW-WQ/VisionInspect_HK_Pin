using System;

namespace DataCollection_System._5_Device.Cam
{
    public class ImageStream
    {
        /// <summary>
        /// 图像句柄
        /// </summary>
        public IntPtr Intp = IntPtr.Zero;

        /// <summary>
        /// 图像数据
        /// </summary>
        public byte[] buffBytes { get; set; }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 是否是彩色图像
        /// </summary>
        public bool IsColor { get; set; }

        /// <summary>
        /// 获取图片时间
        /// </summary>
        public string CaptureTime { get; set; }
    }
}
