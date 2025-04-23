using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 缺陷名称类
    /// </summary>
    public class Defect
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int ID { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
    }
}
