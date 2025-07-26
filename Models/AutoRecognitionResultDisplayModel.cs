using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 结果显示模型
    /// </summary>
    public class AutoRecognitionResultDisplayModel : IDisposable
    {
        /// <summary>
        /// 标签 / DisplayRegion+ID
        /// </summary>
        public string Tag { get; set; }

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

        
        /// <summary>
        /// 全图灰度
        /// </summary>
        public float FullFigureGary { get; set; }


        public void Dispose()
        {
            CurrentImage?.Dispose();
            WheelContour?.Dispose();
            TemplateContour?.Dispose();
            CurrentImage= null;
            WheelContour = null;
            TemplateContour = null;
        }
    }
}
