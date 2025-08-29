using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 识别结果模型
    /// </summary>
    public class RecognitionResultModel
    {

        /// <summary>
        /// 识别状态
        /// </summary>
        public string status = "识别失败";

        /// <summary>
        /// 识别结果bool值
        /// </summary>
        public bool ResultBol
        {
            get
            {
                if (status.Equals("识别成功"))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 标签颜色
        /// </summary>
        public string Colour { get; set; }

        /// <summary>
        /// 识别方式
        /// </summary>
        public string Way { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string WheelStyle { get; set; }
        /// <summary>
        /// 识别的轮型
        /// </summary>
        public string RecognitionWheelType { get; set; }
        /// <summary>
        /// 识别的相似度
        /// </summary>
        public double Similarity { get; set; }

        /// <summary>
        /// 全图灰度
        /// </summary>
        public float FullFigureGray { get; set; }
        
        /// <summary>
        /// 与模板全局灰度的绝对差值
        /// </summary>
        public float AbsDifferenceGray { get; set; }

        /// <summary>
        /// 识别的中心行坐标
        /// </summary>
        public double CenterRow { get; set; }

        /// <summary>
        /// 识别的中心列坐标
        /// </summary>
        public double CenterColumn { get; set; }

        /// <summary>
        ///识别的弧度
        /// </summary>
        public double Radian { get; set; }

        /// <summary>
        /// 识别结果轮毂
        /// </summary>
        public HObject RecognitionContour { get; set; }


        public void Dispose()
        {
            if (RecognitionContour != null)
            {
                RecognitionContour.Dispose();
            }
        }

    }
}
