using System;

namespace Vison_Inspect_System
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public class LEventArgs : EventArgs
    {
        /// <summary>
        /// 事件数据
        /// </summary>
        /// <param name="value">值</param>
        public LEventArgs(object value)
        {
            Value = value;
        }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
