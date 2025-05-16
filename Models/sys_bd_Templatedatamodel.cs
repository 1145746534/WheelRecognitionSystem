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
        private int _index;
        private string _wheelType;
        private int _unusedDays;
        private float _wheelHeight;
        private float _innerCircleGary;
        private string _wheelStyle;
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        /// <summary>
        /// 轮毂型号
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 25)]
        public string WheelType
        {
            get { return _wheelType; }
            set { SetProperty(ref _wheelType, value); }
        }
        /// <summary>
        /// 未用天数
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int UnusedDays
        {
            get { return _unusedDays; }
            set { SetProperty(ref _unusedDays, value); }
        }
        /// <summary>
        /// 轮毂高度
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = false)]
        public float WheelHeight
        {
            get { return _wheelHeight; }
            set { SetProperty(ref _wheelHeight, value); }
        }
        /// <summary>
        /// 轮毂样式
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 25)]
        public string WheelStyle
        {
            get { return _wheelStyle; }
            set { SetProperty(ref _wheelStyle, value); }
        }

        /// <summary>
        /// 内圈灰度
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = false)]
        public float InnerCircleGary
        {
            get { return _innerCircleGary; }
            set { SetProperty(ref _innerCircleGary, value); }
        }

        private string _creationTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 25)]
        public string CreationTime
        {
            get { return _creationTime; }
            set { SetProperty(ref _creationTime, value); }
        }


        private string _lastUsedTime;
        /// <summary>
        /// 上次使用时间
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "datetime")]
        public string LastUsedTime
        {
            get { return _lastUsedTime; }
            set { SetProperty(ref _lastUsedTime, value); }
        }


        private string _templatePath;
        /// <summary>
        /// 模板保存路径
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 255)]
        public string TemplatePath
        {
            get { return _templatePath; }
            set { SetProperty(ref _templatePath, value); }
        }

        private string _templatePicturePath;
        /// <summary>
        /// 模板图片保存路径
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, Length = 255)]
        public string TemplatePicturePath
        {
            get { return _templatePicturePath; }
            set { SetProperty(ref _templatePicturePath, value); }
        }
    }
}
