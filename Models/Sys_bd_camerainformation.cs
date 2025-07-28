using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class Sys_bd_camerainformation : ICloneable
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

        /// <summary>
        /// 是否灰度图
        /// </summary>
        [SugarColumn(IsNullable = false )]
        public bool Grayscale { get; set; }

        /// <summary>
        /// 实现深拷贝克隆方法
        /// </summary>
        public object Clone()
        {
            return new Sys_bd_camerainformation
            {
                ID = this.ID,
                Name = this.Name,
                LinkID = this.LinkID,
                Exposure = this.Exposure,
                Grayscale = this.Grayscale
            };
        }
        /// <summary>
        /// 类型安全的克隆方法（推荐使用）
        /// </summary>
        public Sys_bd_camerainformation SafeClone()
        {
            return (Sys_bd_camerainformation)this.Clone();
        }
    }
}
