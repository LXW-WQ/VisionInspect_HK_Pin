namespace Vison_Inspect_System
{
    public class RecipeConfig
    {
        private string _visionProjectPath;

        /// <summary>
        /// 视觉方案的保存路径
        /// </summary>
        public string VisionProjectPath
        {
            get { return _visionProjectPath; }
            set { _visionProjectPath = value; }
        }

        private double _exposure;
        /// <summary>
        /// 曝光时间
        /// </summary>
        public double Exposure
        {
            get { return _exposure; }
            set { _exposure = value; }
        }

        private double _gainValue;
        /// <summary>
        /// 增益时间
        /// </summary>
        public double GainValue
        {
            get { return _gainValue; }
            set { _gainValue = value; }
        }

    }

}
