﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class SystemSettingsDataModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Index { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }
}
