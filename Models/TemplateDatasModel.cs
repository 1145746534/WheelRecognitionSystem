using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 匹配用模板数据模型
    /// </summary>
    public class TemplateDatasModel
    {
        /// <summary>
        /// 活跃模板
        /// </summary>
        public List<HTuple> ActiveTemplates { get; set; } = new List<HTuple>();
        /// <summary>
        ///不活跃模板
        /// </summary>
        public List<HTuple> NotActiveTemplates { get; set; } = new List<HTuple>();
        /// <summary>
        /// 活跃模板名
        /// </summary>
        public List<string> ActiveTemplateNames { get; set; } = new List<string>();
        /// <summary>
        /// 不活跃模板名
        /// </summary>
        public List<string> NotActiveTemplateNames { get; set; } = new List<string>();
    }
}
