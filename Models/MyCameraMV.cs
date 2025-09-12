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
    public class MyCameraMV : IMyCamera, IDisposable
    {
        /// <summary>
        /// 相机
        /// </summary>
        private HTuple acqHandle;


        private bool IsConnected { get; set; }

        /// <summary>
        /// 用于实时采集
        /// </summary>
        public DispatcherTimer _dispatcherTimer;

        public MyCameraMV()
        {
            IsConnected = false;
        }

        public bool Connect(string camerID)
        { 
            if (string.IsNullOrEmpty(camerID))
            {
                return false;
            }
            if (IsConnected)
            {
                return true;
            }

            acqHandle?.Dispose();
            acqHandle = null;

            try
            {
                HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive",
                  8, "default", -1, "false", "auto", camerID,
                  0, -1, out acqHandle);
                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "Off");
                //开始采集图像
                HOperatorSet.GrabImageStart(acqHandle, -1);
                //Console.WriteLine($"ConnectConnect {camerID} ");
                IsConnected = acqHandle == null ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connect: {ex.Message}");
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
                {
                    HOperatorSet.CloseFramegrabber(acqHandle);
                    acqHandle.Dispose();
                    IsConnected = false;

                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
            }
            finally
            {
                acqHandle = null;
            }

        }

        /// <summary>
        /// 采集图像
        /// </summary>
        /// <returns></returns>
        public HObject Grabimage()
        {
            if (!IsConnected)
            {
                return null;
            }

            try
            {
                // 同步采集一帧（丢弃），以便让新曝光设置生效
                HObject discardImage;
                HOperatorSet.GrabImage(out discardImage, acqHandle);
                discardImage.Dispose();

                HObject ho_Image;//定义图片变量
                HOperatorSet.GenEmptyObj(out ho_Image);
                // 再同步采集一帧，这一帧应该是新曝光下的图像
                HOperatorSet.GrabImage(out ho_Image, acqHandle);


             
               // 初始化本地图像空间的变量
                                                       //采集图像
                //HOperatorSet.GetFramegrabberParam(acqHandle, "ExposureTime", out HTuple exposureValue);
                //Console.WriteLine( $"采集图像曝光值：{exposureValue}");
                //exposureValue.Dispose();
                //HOperatorSet.GrabImageAsync(out ho_Image, acqHandle, -1);
                return ho_Image;
            }
            catch (Exception ex)
            {
                Disconnect(); //采集错误断开连接
            }
            return null;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureTime(float value)
        {
            if (!IsConnected) //相机未连接
            {
                return;
            }
            try
            {
                HOperatorSet.SetFramegrabberParam(acqHandle, "ExposureTime", value);
                Console.WriteLine($"设置曝光时间:{acqHandle}.{value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置曝光时间识别:{ex.ToString()}");
                
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        ~MyCameraMV()
        {
            Disconnect();
        }

    }
}
