using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 浇口检测结果模型
    /// </summary>
    public class GateDetectionResultModel
    {
        /// <summary>
        /// 浇口轮廓
        /// </summary>
        public HObject GateContour { get; set; }
        /// <summary>
        /// 检测结果
        /// </summary>
        public bool DetectionResult { get; set; } = true;
        /// <summary>
        /// 浇口面积
        /// </summary>
        public double GateArea { get; set; }
        /// <summary>
        /// 浇口半径
        /// </summary>
        public double GateRadiu { get; set; }
    }
}
