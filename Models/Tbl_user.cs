using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 用户登录表
    /// </summary>
    public class Tbl_user
    {
        [SqlSugar.SugarColumn(IsNullable = false)]
        public long Id { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [SugarColumn(IsNullable = false,Length =30)]
        public string User_account { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(IsNullable =true,Length =10)]
        public string User_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(IsNullable = true,Length =50)]
        public string Password { get; set; }

        /// <summary>
        /// 盐加密
        /// </summary>
        [SugarColumn(IsNullable =true,Length =20)]
        public string Salt { get; set; }

        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        [SugarColumn(IsNullable =true, ColumnDataType = "char(1)")]
        public string Status { get; set; }

        /// <summary>
        /// 删除标志（0代表存在 2代表删除）
        /// </summary>
        [SugarColumn(IsNullable =true , ColumnDataType ="char(1)")]
        public string Del_flag { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [SugarColumn(IsNullable =true, ColumnDataType = "datetime")]
        public DateTime Login_date { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(IsNullable =true , Length =64)]
        public string Create_by { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "datetime")]
        public DateTime Create_time { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 64)]
        public string Update_by { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "datetime")]
        public string Update_time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable =true, Length = 500)]
        public string Remark { get; set; }


    }
}
