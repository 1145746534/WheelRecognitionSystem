using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 自动模式识别结果显示模型
    /// </summary>
    public class AutoRecognitionResultDisplayModel: IDisposable
    {
        /// <summary>
        /// 1开始
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 轮型
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 当前图像
        /// </summary>
        public HObject CurrentImage { get; set; }
        /// <summary>
        /// 轮毂轮廓
        /// </summary>
        public HObject WheelContour { get; set; }
        /// <summary>
        /// 模板轮廓
        /// </summary>
        public HObject TemplateContour { get; set; }

        public void Dispose()
        {
            CurrentImage?.Dispose();
            WheelContour?.Dispose();
            TemplateContour?.Dispose();
        }
        ///// <summary>
        /////  浇口轮廓
        ///// </summary>
        //public HObject GateContour { get; set; }
        ///// <summary>
        ///// 是否存在浇口轮廓
        ///// </summary>
        //public bool IsGate { get; set; }


    }
}
