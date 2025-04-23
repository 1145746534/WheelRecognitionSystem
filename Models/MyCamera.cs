using HalconDotNet;
using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WheelRecognitionSystem.Models.IModels;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.Models
{
    public class MyCamera : IMyCamera, IDisposable
    {
        /// <summary>
        /// 相机
        /// </summary>
        public HTuple acqHandle;
        public bool IsConnected { get; set; } = false;

        public Sys_bd_camerainformation info;
        /// <summary>
        /// 用于实时采集
        /// </summary>
        public DispatcherTimer _dispatcherTimer;

        public MyCamera()
        {
        }

        public bool Connect( string camerID  )
        {
            IsConnected = false;
            acqHandle?.Dispose();
            if (info != null && !IsConnected)
            {
                try
                {
                    HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive",
                      8, "default", -1, "false", "auto", camerID,
                      0, -1, out acqHandle);
                    HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "Off");
                    //开始采集图像
                    HOperatorSet.GrabImageStart(acqHandle, -1);
                    Console.WriteLine( $"ConnectConnect {camerID} ");
                    IsConnected = acqHandle == null ? false : true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine( $"Connect: {ex.Message}" );
                }
            }
            return IsConnected;
        }

        /// <summary>
        /// 断开相机
        /// </summary>
        public void Disconnect()
        {
            try
            {

                if (acqHandle != null)
                    HOperatorSet.CloseFramegrabber(acqHandle);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public HObject Grabimage()
        {
            if (acqHandle == null)
            {
                return null;
            }
            HObject ho_Image;//定义图片变量
            HOperatorSet.GenEmptyObj(out ho_Image);// 初始化本地图像空间的变量
            //采集图像
            HOperatorSet.GrabImageAsync(out ho_Image, acqHandle, -1);
            return ho_Image;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureTime(float value)
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(acqHandle, "ExposureTime", value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        ~MyCamera()
        {
            Disconnect();
        }

    }
}
