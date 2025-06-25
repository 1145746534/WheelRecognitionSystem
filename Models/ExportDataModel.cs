using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class ExportDataModel
    {
        /// <summary>
        /// 匹配行
        /// </summary>
        public int MatchRow { get; set; }
       
        /// <summary>
        /// 匹配参数
        /// </summary>
        public string MatchName{ get; set; }

        /// <summary>
        /// 设置行
        /// </summary>
        public int SettingRow { get; set; }
 

        /// <summary>
        /// 设置值
        /// </summary>
        public object SettingValue { get; set; }
    }
}
