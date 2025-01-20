﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 生产数据模型
    /// </summary>
    public class ProductionDataModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)] ////IsIdentity为True时Id为自增型
        public int Index { get; set; }
        /// <summary>
        /// 轮型
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 识别耗时
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string TimeConsumed { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Similarity { get; set; }

        /// <summary>
        /// 轮毂高度
        /// </summary>
        public  float WheelHeight { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string WheelStyle { get; set; }

        /// <summary>
        /// 识别时间
        /// </summary>
        public DateTime RecognitionTime { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 预留1
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Reserve1 { get; set; }
        /// <summary>
        /// 预留2
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Reserve2 { get; set; }
        
    }
}
