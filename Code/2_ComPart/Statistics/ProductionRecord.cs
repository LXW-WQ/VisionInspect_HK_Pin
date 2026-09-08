namespace Vison_Inspect_System._2_ComPart.Statistics
{
    /// <summary>
    /// 生产数据记录类
    /// </summary>
    public class ProductionRecord
    {
        public int SN { get; set; }
        public string Timestamp { get; set; }
        public int TotalCount { get; set; }
        public int TotalNGCount { get; set; }
        public int TotalOKCount { get; set; }
        public int HeightNGCount { get; set; }
        public int HeightOKCount { get; set; }
        public int PlateNGCount { get; set; }
        public int PlateOKCount { get; set; }
        public int ExcessExpoxyVisionNGCount { get; set; }
        public int ExcessExpoxyVisionOKCount { get; set; }
        public int PressureVisionNGCount { get; set; }
        public int PressureVisionOKCount { get; set; }

    }
}
