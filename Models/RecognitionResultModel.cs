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
        /// 识别结果轮毂
        /// </summary>
        public HObject RecognitionContour { get; set; }

        /// <summary>
        /// 中心行坐标
        /// </summary>
        public double CenterRow { get; set; }

        /// <summary>
        /// 中心列坐标
        /// </summary>
        public double CenterColumn { get; set; }

        public double Radius { get; set; }

        /// <summary>
        /// 仿射变化矩阵
        /// </summary>
        public HTuple HomMat2D { get; set; }


        public void Dispose()
        {
            try
            {
                if (RecognitionContour != null && RecognitionContour.IsInitialized())
                {
                    RecognitionContour?.Dispose();
                }
                if (HomMat2D != null)
                {
                    HomMat2D?.Dispose();
                }
            }
            catch(Exception ex) {
                Console.WriteLine(  ex.ToString());
            }
           
        }

    }
}
