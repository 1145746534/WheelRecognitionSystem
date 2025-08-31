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
        /// 是否保存图片
        /// </summary>
        public bool IsSaveImage = true;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay = true;

        /// <summary>
        /// 是否发送PLC信号
        /// </summary>
        public bool IsSendPLCInfo = true;

        /// <summary>
        /// 数据处理方式
        /// </summary>
        public InfoHandle InfoHanleWay = InfoHandle.Save;


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

    public enum InfoHandle
    {
        /// <summary>
        /// 数据保存
        /// </summary>
        Save = 0,
        /// <summary>
        /// 数据修改
        /// </summary>
        Update = 1,
    }
}
