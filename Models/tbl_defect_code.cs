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
    public class tbl_defect_code
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int ID { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string name { get; set; }

        /// <summary>
        /// 是否启用（0正常 1停用）
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "char(1)")]
        public string status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 255)]
        public string remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string create_by { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "datetime")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string update_by { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "datetime")]
        public DateTime update_time { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(IsNullable = true,  Length =1)]
        public string del_flag { get; set; }


    }
}
