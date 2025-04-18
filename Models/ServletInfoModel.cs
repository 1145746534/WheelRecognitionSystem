using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 页面跳转信息
    /// </summary>
    public class ServletInfoModel
    {
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 窗口图片
        /// </summary>
        public HObject image { get; set; }

        /// <summary>
        /// 窗口相机
        /// </summary>
        public Camera camera { get; set; }
    }
}
