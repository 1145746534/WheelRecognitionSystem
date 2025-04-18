using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class Sys_bd_activewheeltypedatamodel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)] ////IsIdentity为True时Id为自增型
        public int ID { get; set; }
        public string WheelType { get; set; }
    }
}
