using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using HalconDotNet;
using MvCameraControl;
using WheelRecognitionSystem.Models.IModels;

namespace WheelRecognitionSystem.Models
{
    internal class MyCameraHK_SDK : IMyCamera, IDisposable
    {
        public bool IsConnected
        {
            get;
            set;
        }

        /// <summary>
        /// 用于实时采集
        /// </summary>
        public DispatcherTimer _dispatcherTimer;

        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
        | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;
        List<IDeviceInfo> deviceInfoList = new List<IDeviceInfo>();
        IDevice device = null;

        public MyCameraHK_SDK()
        {
            RefreshDeviceList();
        }


        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="camerID"></param>
        /// <returns></returns>
        public bool Connect(string camerID)
        {

            if (deviceInfoList.Count == 0)
                return false;
            
            if (string.IsNullOrEmpty(camerID))
                return false;

            IDeviceInfo deviceInfo = null;
            deviceInfo = deviceInfoList.Find(x => x.SerialNumber == camerID);
            if (deviceInfo == null)
            {
                return false;
            }
            // ch:打开设备 | en:Open device
            device = DeviceFactory.CreateDevice(deviceInfo);
            int result = device.Open();
            if (result == MvError.MV_OK)
            {
                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = device as IGigEDevice;

                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    int optionPacketSize;
                    result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                    if (result == MvError.MV_OK)
                    {
                        result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);

                    }

                }

                // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
                //device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                device.Parameters.SetEnumValueByString("TriggerMode", "On");
                //device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
                device.Parameters.SetEnumValueByString("TriggerSource", "Software");

                IsConnected = true;
            }
            return IsConnected;
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void Disconnect()
        {
            if (device != null)
            {
                device.Close();
                device.Dispose();
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        ~MyCameraHK_SDK()
        {
            Disconnect();
        }

        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureTime(float value)
        {
            device.Parameters.SetEnumValue("ExposureAuto", 0);
            int result = device.Parameters.SetFloatValue("ExposureTime", value);
            if (result != MvError.MV_OK)
            {
                //ShowErrorMsg("Set Exposure Time Fail!", result);
            }
        }

        /// <summary>
        /// 单帧取图 | 同步
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HObject Grabimage()
        {
            HObject ho_Image;//定义图片变量
            HOperatorSet.GenEmptyObj(out ho_Image);// 初始化本地图像空间的变量
            int result1 = device.StreamGrabber.StartGrabbing();

            IFrameOut frameOut;
            int nRet;
            int result = device.Parameters.SetCommandValue("TriggerSoftware");
            if (result == MvError.MV_OK)
            {
                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == nRet)
                {

                    // 假设 frameOut.Image 是包含图像数据的对象
                    uint width = frameOut.Image.Width;      // 图像宽度
                    uint height = frameOut.Image.Height;     // 图像高度
                    IntPtr pData = frameOut.Image.PixelDataPtr;    // 图像数据指针
                    MvGvspPixelType pixelFormat = frameOut.Image.PixelType; // 像素格式（如 "Mono8", "BayerRG8", "RGB24" 等）

                    if (pixelFormat == MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        HOperatorSet.GenImage1(out ho_Image, "byte", width, height, pData);
                    }
                    if (pixelFormat == MvGvspPixelType.PixelType_Gvsp_BayerRG8)
                    {
                        HObject hoBayer;
                        HOperatorSet.GenImage1(out hoBayer, "byte", width, height, pData);
                        HOperatorSet.CfaToRgb(hoBayer, out ho_Image, "bayer_rg", "bilinear");
                        hoBayer.Dispose();
                    }


                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
            }
            int result2 = device.StreamGrabber.StopGrabbing();

            //采集图像
            return ho_Image;
        }

        private void RefreshDeviceList()
        {
            // ch:创建设备列表 | en:Create Device List           
            int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
            if (nRet != MvError.MV_OK)
            {
                throw new Exception("RefreshDeviceList - MvError:" + nRet.ToString());
            }
        }

       
    }
}
