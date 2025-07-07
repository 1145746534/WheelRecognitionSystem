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
using WheelRecognitionSystem.Public;
using System.Windows.Shapes;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 模板数据 - 新构造
    /// </summary>
    public class TemplatedataModels : sys_bd_Templatedatamodel
    {
        
        private bool _isLoaded = false;   // 内存加载状态

        public HTuple _template;
        /// <summary>
        /// 模板
        /// </summary>
        public HTuple Template
        {
            get {
                // 按需加载：访问时若未加载则从硬盘加载
                if (!_isLoaded)
                {
                    LoadTemplate();
                }
                return _template;
            }
            set {
                // 释放旧对象但不保存到文件
                _template?.Dispose();
                _template = value;
                _isLoaded = true;               
            }
        }


        public bool _use;
        /// <summary>
        /// 使用状态 可用/不可用
        /// </summary>
        public bool Use
        {
            get {
                //当前使用时间减去上次使用时间
                TimeSpan timeSpan = DateTime.Now - LastUsedTime;
                UnusedDays = timeSpan.Days;
                if (UnusedDays < SystemDatas.TemplateAdjustDays) //属于使用范围               
                    _use = true;
                else
                {
                    _use = false;
                    if (UnusedDays > 3) //3天未使用
                    {
                        UnloadTemplate();
                    }
                }

                return _use; 
            }
        }



        // 保存模板到文件
        public async void SaveTemplateAsync()
        {
            if (_template != null)
            {

                await Task.Run(() => HOperatorSet.WriteNccModel(_template, TemplatePath));

            }
        }

        // 从文件加载模板
        private async void LoadTemplate()
        {
            if (!File.Exists(TemplatePath))
                return;
            await Task.Run(() =>
            {
                string strPath = TemplatePath.Replace(@"\", "/");//字符串替换
                HOperatorSet.ReadNccModel(strPath, out HTuple modelID); //读NCC模板
                Template = modelID;
            });        
            _isLoaded = true;
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

        /// <summary>
        /// 从另一个基类对象复制所有属性值（包含模板路径）
        /// </summary>
        public void CopyPropertiesFrom(sys_bd_Templatedatamodel source)
        {
            if (source == null) return;

            // 直接复制基类属性（非绑定方式避免通知触发）
            this.Index = source.Index;
            this.WheelType = source.WheelType;
            this.UnusedDays = source.UnusedDays;
            this.WheelHeight = source.WheelHeight;
            this.WheelStyle = source.WheelStyle;
            this.InnerCircleGary = source.InnerCircleGary;
            this.CreationTime = source.CreationTime;
            this.LastUsedTime = source.LastUsedTime;
            this.TemplatePath = source.TemplatePath;
            this.TemplatePicturePath = source.TemplatePicturePath;

            // 特殊处理：从模板路径加载模板（如果存在）
            if (!string.IsNullOrEmpty(TemplatePath)
                && File.Exists(TemplatePath))
            {
                LoadTemplate();
            }
            
        }
    }
}
