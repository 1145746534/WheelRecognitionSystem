using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 信号交互类-PLC发送的信号
    /// </summary>
    public class InteractS7PLCModel
    {
        /// <summary>
        /// 轮毂到位延时
        /// </summary>
        public int ArrivalDelay;

        /// <summary>
        /// 读取到的PLC信息
        /// </summary>
        public ReadPLCSignal readPLCSignal;



        /// <summary>
        /// 识别结果
        /// </summary>
        public RecognitionResultModel resultModel;

        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime starTime;
        /// <summary>
        /// 结束处理时间
        /// </summary>
        public DateTime endTime;

        public string imagePath;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public TimeSpan Interval
        {
            get { return endTime.Subtract(starTime); }
        }

    }
}
