using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class Sys_bd_camerainformation
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 相机名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 相机连接ID
        /// </summary>
        public string LinkID { get; set; }
        /// <summary>
        /// 曝光
        /// </summary>
        public int Exposure { get; set; }
    }
}
