using HalconDotNet;
using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.Models;

namespace WheelRecognitionSystem.Public
{
    /// <summary>
    /// 外部连接
    /// </summary>
    public class ExternalConnections
    {
        /// <summary>
        /// 主线程控制
        /// </summary>
        public static bool MainThreadControl { get; set; } = false;

        /// <summary>
        /// 外部连接线程控制
        /// </summary>
        public static bool ExternalConnectionThreadControl { get; set; } = false;

        /// <summary>
        /// PLC数据交互线程控制
        /// </summary>
        public static bool PlcDataInteractionControl { get; set; } = false;

        /// <summary>
        /// 心跳线程控制
        /// </summary>
        public static bool HeartbeatThreadControl { get; set; } = false;

        /// <summary>
        /// 相机句柄
        /// </summary>
        public static HTuple CameraHandle;
        /// <summary>
        /// 相机标识符
        /// </summary>
        public static string CameraIdentifier = null;
        /// <summary>
        /// 多相机列表
        /// </summary>
        public static List<CameraInformation> DatasCamera { get; set; }
    }
}
