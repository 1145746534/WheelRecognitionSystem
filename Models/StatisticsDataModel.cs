using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class StatisticsDataModel
    {
        public int Index { get; set; }
        /// <summary>
        /// 轮型
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 轮毂数量
        /// </summary>
        public int WheelCount { get; set; }
    }
}
