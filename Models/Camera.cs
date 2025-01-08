using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.Models
{
    public class Camera
    {
        public HTuple acqHandle;
        public bool IsConnected { get; set; } = false;

        public CameraInformation info;
        /// <summary>
        /// 用于实时采集
        /// </summary>
        public DispatcherTimer _dispatcherTimer;

        public Camera()
        {
        }
        /// <summary>
        /// 连接相机
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            IsConnected = false;
            acqHandle?.Dispose();
            if (info != null && !IsConnected)
            {
                try
                {
                    acqHandle = CameraHelper.ConnectCamera(info.LinkID);
                    IsConnected = acqHandle == null ? false : true;
                }
                catch (Exception ex)
                {

                }
            }
            return IsConnected;
        }
        /// <summary>
        /// 断开相机
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected) //存在连接状态先断开
            {
                try
                {
                    IsConnected = false;
                    CameraHelper.DisconnectCamera(acqHandle);
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureTime()
        {
            if (info != null && IsConnected)
            {
                try
                {
                    CameraHelper.SetExposureTime(acqHandle, info.Exposure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
