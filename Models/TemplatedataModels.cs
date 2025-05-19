using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using HalconDotNet;
using Prism.Mvvm;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 模板数据 - 新构造
    /// </summary>
    public class TemplatedataModels : BindableBase
    {
        public HTuple _template;
        /// <summary>
        /// 模板
        /// </summary>
        public HTuple Template
        {
            get { return _template; }
            set { SetProperty(ref _template, value); }
        }


        public string _templateName;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            get { return _templateName; }
            set { SetProperty(ref _templateName, value); }
        }

        public bool _use;
        /// <summary>
        /// 使用状态 可用/不可用
        /// </summary>
        public bool Use
        {
            get { return _use; }
            set { SetProperty(ref _use, value); }
        }

        public DateTime _lastUsedTime;
        /// <summary>
        /// 最后使用时间
        /// </summary>
        public DateTime LastUsedTime
        {
            get { return _lastUsedTime; }
            set
            {
                SetProperty(ref _lastUsedTime, value);
            }
        }
    }
}
