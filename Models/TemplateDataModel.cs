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
    public class TemplateDataModel : BindableBase
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
        public int UnusedDays { get; set; }
        /// <summary>
        /// 轮毂高度
        /// </summary>
        public float WheelHeight { get; set; }
        /// <summary>
        /// 轮毂样式
        /// </summary>
        public string WheelStyle { get; set; }

        private bool _sortingEnable;
        /// <summary>
        /// 分选使能
        /// </summary>
        public bool SortingEnable
        {
            get { return _sortingEnable; }
            set { SetProperty(ref _sortingEnable, value); }
        }

        private string _creationTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime
        {
            get { return _creationTime; }
            set { SetProperty(ref _creationTime, value); }
        }
    }
}
