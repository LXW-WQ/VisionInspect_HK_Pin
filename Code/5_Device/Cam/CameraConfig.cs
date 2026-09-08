using System.ComponentModel;

namespace Vison_Inspect_System._5_Device.Cam
{
    /// <summary>
    /// 相机参数配置类
    /// </summary>
    public class CameraConfig
    {
        private CameraType _cameraType = CameraType.Camera1;
        [Category("01.相机参数配置")]
        [DisplayName("01.相机类型")]
        public CameraType CameraType
        {
            get { return _cameraType; }
            set { _cameraType = value; }
        }

        private string _CamName = "cam1";
        [Category("01.相机参数配置")]
        [DisplayName("02.相机名称")]
        public string CamName
        {
            get { return _CamName; }
            set { _CamName = value; }
        }

        private bool _IsSaveImage = false;
        [Category("01.相机参数配置")]
        [DisplayName("03.是否保存图片")]
        public bool IsSaveImage
        {
            get { return _IsSaveImage; }
            set { _IsSaveImage = value; }
        }

        private string _SavePicturePath = "D:/SaveImage/";
        [Category("01.相机参数配置")]
        [DisplayName("04.保存图片的路径")]
        public string SavePicturePath
        {
            get { return _SavePicturePath; }
            set { _SavePicturePath = value; }
        }

        private ImageFormat _savePictureFormat = ImageFormat.JPG;
        [Category("01.相机参数配置")]
        [DisplayName("05.保存图片的格式")]
        public ImageFormat SavePictureFormat
        {
            get { return _savePictureFormat; }
            set { _savePictureFormat = value; }
        }

        private bool _isShowCrossLine;
        [Category("02.相机显示设置")]
        [DisplayName("01.是否显示十字线交叉")]
        public bool IsShowCrossLine
        {
            get { return _isShowCrossLine; }
            set { _isShowCrossLine = value; }
        }


    }
}
