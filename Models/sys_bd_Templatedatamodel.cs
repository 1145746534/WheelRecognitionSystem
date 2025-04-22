using Prism.Mvvm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 模板数据模型
    /// </summary>
    public class sys_bd_Templatedatamodel : BindableBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int Index { get; set; }
        /// <summary>
        /// 轮毂型号
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 未用天数
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int UnusedDays { get; set; }
        /// <summary>
        /// 轮毂高度
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = false)]
        public float WheelHeight { get; set; }
        /// <summary>
        /// 轮毂样式
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 255)]
        public string WheelStyle { get; set; }

        

        private string _creationTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 255)]
        public string CreationTime
        {
            get { return _creationTime; }
            set { SetProperty(ref _creationTime, value); }
        }
    }
}
