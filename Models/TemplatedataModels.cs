using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using HalconDotNet;
using Prism.Mvvm;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NPOI.OpenXmlFormats.Vml;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 模板数据 - 新构造
    /// </summary>
    public class TemplatedataModels : BindableBase
    {
        // 新增字段
        private string _templateFilePath; // 硬盘存储路径
        private bool _isLoaded = false;   // 内存加载状态

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


        private string _style;
        /// <summary>
        /// 成品或者半成品
        /// </summary>
        public string Style
        {
            get { return _style; }
            set { SetProperty(ref _style, value); }
        }

        // 初始化方法（必须调用！）
        public void Initialize(string storageDirectory)
        {
            _templateFilePath = Path.Combine(storageDirectory, $"{TemplateName}.bin");
            // 首次创建时直接保存到硬盘
            if (_template != null && !File.Exists(_templateFilePath))
            {
                SaveTemplateAsync();
                _template.Dispose();
                _template = null;
                _isLoaded = false;
            }
        }

        // 保存模板到文件
        public async void SaveTemplateAsync()
        {
            if (_template != null)
            {

                await Task.Run(() => HOperatorSet.WriteNccModel(_template, _templateFilePath));

            }
        }

        // 从文件加载模板
        private async void LoadTemplate()
        {
            if (!File.Exists(_templateFilePath))
                return;
            await Task.Run(() =>
            {
                HOperatorSet.ReadNccModel(_templateFilePath, out HTuple modelID); //读NCC模板
                Template = modelID;
            });        
            _isLoaded = true;
            _lastUsedTime = DateTime.Now;
        }

        // 手动卸载模板（释放内存）
        public void UnloadTemplate()
        {
            if (_isLoaded && _template != null)
            {
                _template.Dispose();
                _template = null;
                _isLoaded = false;
            }
        }
    }
}
