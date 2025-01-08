using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 分选数据模型
    /// </summary>
    public class ScreenedDataModel
    {
        /// <summary>
        /// 单元号
        /// </summary>
        public string Unit {  get; set; }
        /// <summary>
        /// 单元状态
        /// </summary>
        public bool State {  get; set; }
        /// <summary>
        /// 轮毂型号
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 在线数量
        /// </summary>
        public int OnlineQuantity { get; set; }
        /// <summary>
        /// 目标数量
        /// </summary>
        public int TargetQuantity { get; set; }
    }
}
