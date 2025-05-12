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
        /// 到位轮毂位置-下标从1开始
        /// </summary>
        public int Index;
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

       

        /***
        0<=h<20， 红色
        30<=h<45， 黄色
        45<=h<90， 绿色
        90<=h<125， 青色
        125<=h<150， 蓝色
        150<=h<175， 紫色
        175<=h<200， 粉红色
        200<=h<220， 砖红色
        220<=h<255， 品红色
         **/

        ///// <summary>
        ///// 识别颜色
        ///// </summary>
        //public string colour;

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
        /// 是否灰度图
        /// </summary>
        public bool IsGrayscale  { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public TimeSpan Interval
        {
            get { return endTime.Subtract(starTime); }
        }

    }
}
