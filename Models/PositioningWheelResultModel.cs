using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class PositioningWheelResultModel
    {
        /// <summary>
        /// 定位到的轮毂图像
        /// </summary>
        public HObject WheelImage { get; set; }
        /// <summary>
        /// 轮毂轮廓
        /// </summary>
        public HObject WheelContour { get; set; }
        /// <summary>
        /// 轮毂中心行坐标
        /// </summary>
        public HTuple CenterRow { get; set; }
        /// <summary>
        /// 轮毂中心列坐标
        /// </summary>
        public HTuple CenterColumn { get; set; }
    }
}
