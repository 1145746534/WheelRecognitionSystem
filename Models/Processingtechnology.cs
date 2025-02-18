using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 加工工艺类
    /// </summary>
    public class Processingtechnology
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int ID { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
    }
}
