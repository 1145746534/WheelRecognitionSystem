﻿using HalconDotNet;
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
        public List<string> WheelTypes { get; set; } = new List<string>();
        /// <summary>
        /// 相似度列表
        /// </summary>
        public List<double> Similaritys { get; set; } = new List<double>();
        /// <summary>
        /// 识别的轮型
        /// </summary>
        public string RecognitionWheelType { get; set; }
        /// <summary>
        /// 识别的相似度
        /// </summary>
        public double Similarity { get; set; }

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
        /// 识别模板
        /// </summary>
        public HTuple TemplateID { get; set; }

        /// <summary>
        /// 是否在不活跃模板中识别，用于实时移动模板
        /// </summary>
        public bool IsInNotTemplate { get; set; }
    }
}
