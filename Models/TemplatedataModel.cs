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
    /// 模板数据
    /// </summary>
    public class TemplatedataModel : sys_bd_Templatedatamodel
    {

        public HTuple _template;
        /// <summary>
        /// 模板
        /// </summary>
        public HTuple Template
        {
            get
            {
                if (_template == null && File.Exists(TemplateUsePath))
                {
                    // 同步加载（）
                    Console.WriteLine( $"同步加载文件：{TemplateUsePath}");
                    string strPath = TemplateUsePath.Replace(@"\", "/");
                    //HOperatorSet.ReadNccModel(strPath, out HTuple modelID);
                    HOperatorSet.ReadShapeModel(strPath, out HTuple modelID);
                    _template = modelID;
                }
                return _template;
            }
            set
            {
                _template?.Dispose();
                _template = value;
            }
        }

        public string _templateUsePath;

        /// <summary>
        /// 模板使用路径 缓存路径
        /// </summary>
        public string TemplateUsePath
        {
            get { return _templateUsePath; }
            set { SetProperty(ref _templateUsePath, value); }
        }


        public TemplateStatus _status;

        /// <summary>
        /// 模板状态
        /// </summary>
        public TemplateStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

         

        /// <summary>
        /// 释放模板
        /// </summary>
        public void ReleaseTemplate()
        {
            if (_template != null)
            {
                Console.WriteLine($"释放模板:{WheelType} :{TemplateUsePath}");
                _template?.Dispose();
                _template = null;
            }
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            ReleaseTemplate();
        }

        /// <summary>
        /// 更新使用时间
        /// </summary>
        /// <param name="model"></param>
        public void UseTemplate()
        {
            LastUsedTime = DateTime.Now; // 更新使用时间
            UnusedDays = (DateTime.Now - LastUsedTime).Days;
        }
        /// <summary>
        /// 从另一个基类对象复制所有属性值（包含模板路径）
        /// </summary>
        public void CopyPropertiesFrom(sys_bd_Templatedatamodel source)
        {
            if (source == null) return;

            // 直接复制基类属性（非绑定方式避免通知触发）
            this.Index = source.Index;
            this.WheelType = source.WheelType;   //
            this.UnusedDays = source.UnusedDays; //
            this.WheelHeight = source.WheelHeight;
            this.WheelStyle = source.WheelStyle;
            this.FullGary = source.FullGary;
            this.CreationTime = source.CreationTime;
            this.UpdateTime = source.UpdateTime;
            this.LastUsedTime = source.LastUsedTime;  //
            this.TemplatePath = source.TemplatePath;
            this.TemplatePicturePath = source.TemplatePicturePath;        

        }
    }

    public enum TemplateStatus
    {
        /// <summary>
        /// 存在
        /// </summary>
        Exist,
        /// <summary>
        /// 需要更新
        /// </summary>
        Update,
        /// <summary>
        /// 需要删除
        /// </summary>
        Delete
    }
}
