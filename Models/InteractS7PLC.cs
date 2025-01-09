using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 信号交互类
    /// </summary>
    public class InteractS7PLC
    {
        /// <summary>
        /// 到位轮毂位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 轮毂到位延时
        /// </summary>
        public int ArrivalDelay;
        /// <summary>
        /// 轮毂到位信号
        /// </summary>
        public bool ArrivalSignal;

        /// <summary>
        /// 轮毂到位高度
        /// </summary>
        public float ArrivalHeight;

    }
}
