using HalconDotNet;
using MySqlX.XDevAPI.Common;
using NPOI.OpenXmlFormats.Vml;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WheelRecognitionSystem.Public.SystemDatas;
using WheelRecognitionSystem.Models;
using System.Runtime.Remoting.Channels;
using MvCameraControl;
using System.Threading;
using System.Windows;

namespace WheelRecognitionSystem.Public
{
    public class CameraHelper : IDisposable
    {
        readonly static DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
         | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;
        static List<IDeviceInfo> deviceInfoList = new List<IDeviceInfo>();
        static IDevice device = null;




        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="cameraIdentifier">相机标识符</param>
        /// <returns>相机句柄</returns>
        //public static HTuple ConnectCamera(string cameraIdentifier)
        //{
        //    //HOperatorSet.CloseAllFramegrabbers(); //释放相机句柄  
        //    try
        //    {

        //        //         open_framegrabber ('MVision', 1, 1, 0, 0, 0, 0, 'progressive',
        //        //         8, 'default', -1, 'false', 'auto', 'GEV:DA0241653 MV-CS050-10GC',
        //        //         0, -1, AcqHandle)
        //        HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive",
        //                8, "default", -1, "false", "auto", cameraIdentifier,
        //                0, -1, out HTuple acqHandle);
        //        HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "Off");
        //        //开始采集图像
        //        HOperatorSet.GrabImageStart(acqHandle, -1);
        //        return acqHandle;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        return null;
        //    }

        //}

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="cameraIdentifier">相机标识符</param>
        /// <returns>相机句柄</returns>
        //public static bool ConnectCamera1()
        //{
        //    //HOperatorSet.CloseAllFramegrabbers(); //释放相机句柄  
        //    try
        //    {
                
        //        IDeviceInfo deviceInfo = null;
        //        SDKSystem.Initialize();
        //        int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
        //        if (nRet == MvError.MV_OK)
        //        {
        //            deviceInfo = deviceInfoList.Find(x => x.SerialNumber == "00DA0879936");

        //        }



        //        // ch:打开设备 | en:Open device
        //        device = DeviceFactory.CreateDevice(deviceInfo);
        //        int result = device.Open();
        //        if (result == MvError.MV_OK)
        //        {
        //            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
        //            if (device is IGigEDevice)
        //            {
        //                //ch: 转换为gigE设备 | en: Convert to Gige device
        //                IGigEDevice gigEDevice = device as IGigEDevice;

        //                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
        //                int optionPacketSize;
        //                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
        //                if (result != MvError.MV_OK)
        //                {
        //                }
        //                else
        //                {
        //                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
        //                    if (result != MvError.MV_OK)
        //                    {
        //                    }
        //                }
        //            }

        //            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
        //            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
        //            //device.Parameters.SetEnumValueByString("TriggerMode", "Off");
        //            device.Parameters.SetEnumValueByString("TriggerMode", "On");
        //            //device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
        //            device.Parameters.SetEnumValueByString("TriggerSource", "Software");


        //        }





        //        //毛刺机相机参数
        //        //open_framegrabber('GigEVision2', 0, 0, 0, 0, 0, 0, 'progressive', -1, 'default', -1, 'false', 'default', '34bd2028ebb4_Hikrobot_MVCH25090GM', 0, -1, AcqHandle)


        //        //open_framegrabber ('GigEVision2', 0, 0, 0, 0, 0, 0, 'progressive', -1, 'default', -1, 'false', 'default', '34bd2022b18b_Hikrobot_MVCS05010GC', 0, -1, AcqHandle)

        //        //HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive",
        //        //        -1, "default", -1, "false", "default", cameraIdentifier,
        //        //        0, -1, out HTuple acqHandle);
        //        //HOperatorSet.SetFramegrabberParam(acqHandle, "continuous_grabbing", "true");
        //        //HOperatorSet.SetFramegrabberParam(acqHandle, "realtime", "true");
        //        //HOperatorSet.SetFramegrabberParam(acqHandle, "buffer_num", 2);
        //        //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", cameraIdentifier, 0, -1, out HTuple acqHandle);

        //        //SetExposureTime(acqHandle, 11000.0);
        //        //HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "Off");
        //        ////开始采集图像
        //        //HOperatorSet.GrabImageStart(acqHandle, -1);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        return false;
        //    }

        //}

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="acqHandle"></param>
        /// <param name="value"></param>
        //public static void SetExposureTime(HTuple acqHandle, HTuple value)
        //{
        //    try
        //    {
        //        HOperatorSet.SetFramegrabberParam(acqHandle, "ExposureTime", value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="acqHandle"></param>
        //public static void DisconnectCamera(HTuple acqHandle)
        //{
        //    try
        //    {
               
        //        if (acqHandle != null)
        //            HOperatorSet.CloseFramegrabber(acqHandle);
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //    }
        //}

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="acqHandle"></param>
        //public static void DisconnectCamera1()
        //{
        //    try
        //    {
        //        // ch:关闭设备 | en:Close Device
        //        if (device != null)
        //        {
        //            device.Close();
        //            device.Dispose();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //    }
        //}

        /// <summary>
        /// 单次采集图像方法
        /// </summary>
        /// <param name="acqHandle"></param>
        /// <returns></returns>
        //public static HObject Grabimage(HTuple acqHandle)
        //{
        //    if (acqHandle == null)
        //    {
        //        return null;
        //    }
        //    HObject ho_Image;//定义图片变量
        //    HOperatorSet.GenEmptyObj(out ho_Image);// 初始化本地图像空间的变量
        //    //采集图像
        //    HOperatorSet.GrabImageAsync(out ho_Image, acqHandle, -1);
        //    return ho_Image;
        //}

        //public static HObject Grabimage1()
        //{
        //    HObject ho_Image;//定义图片变量
        //    HOperatorSet.GenEmptyObj(out ho_Image);// 初始化本地图像空间的变量
        //    int result1 = device.StreamGrabber.StartGrabbing();

        //    IFrameOut frameOut;
        //    int nRet;
        //    int result = device.Parameters.SetCommandValue("TriggerSoftware");
        //    if (result == MvError.MV_OK)
        //    {
        //        nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
        //        if (MvError.MV_OK == nRet)
        //        {

        //            // 假设 frameOut.Image 是包含图像数据的对象
        //            uint width = frameOut.Image.Width;      // 图像宽度
        //            uint height = frameOut.Image.Height;     // 图像高度
        //            IntPtr pData = frameOut.Image.PixelDataPtr;    // 图像数据指针
        //            MvGvspPixelType pixelFormat = frameOut.Image.PixelType; // 像素格式（如 "Mono8", "BayerRG8", "RGB24" 等）

        //            if (pixelFormat == MvGvspPixelType.PixelType_Gvsp_Mono8)
        //            {
        //                HOperatorSet.GenImage1(out ho_Image, "byte", width, height, pData);
        //            }


        //            device.StreamGrabber.FreeImageBuffer(frameOut);
        //        }
        //    }
        //    int result2 = device.StreamGrabber.StopGrabbing();

        //    //采集图像
        //    return ho_Image;
        //}

        //public static void SavePic(HObject saveImage, InteractS7PLCModel data)
        //{
        //    if (saveImage == null)
        //    {
        //        return;
        //    }
        //    if (data.resultModel.WheelType == null)
        //    {
        //        return;
        //    }
        //    //月文件夹路径
        //    string monthPath = HistoricalImagesPath + @"\" + data.endTime.Month + "月";
        //    //日文件夹路径
        //    string dayPath = HistoricalImagesPath + @"\" + data.endTime.Month + @"月\" + data.endTime.Day + "日";
        //    //当日未识别文件夹路径
        //    string ngPath = HistoricalImagesPath + @"\" + data.endTime.Month + @"月\" + data.endTime.Day + @"日\NG";
        //    if (Directory.Exists(monthPath) == false)
        //        Directory.CreateDirectory(monthPath);
        //    if (Directory.Exists(dayPath) == false)
        //        Directory.CreateDirectory(dayPath);
        //    if (Directory.Exists(ngPath) == false)
        //        Directory.CreateDirectory(ngPath);
        //    var diskFree = GetHardDiskFreeSpace("D");//获取D盘剩余空间
        //    if (diskFree > 200)
        //    {
        //        if (data.resultModel.ResultBol)
        //        {
        //            //保存轮型的目录
        //            string saveWheelTypePath = dayPath + @"\" + data.resultModel.WheelType.Trim('_');
        //            if (Directory.Exists(saveWheelTypePath) == false) Directory.CreateDirectory(saveWheelTypePath);
        //            string saveImagePath = saveWheelTypePath.Replace(@"\", "/") + "/" + data.resultModel.WheelType + "&" + data.endTime.ToString("yyMMddHHmmss") + ".tiff";
        //            HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
        //            data.imagePath = saveImagePath;
        //        }
        //        else
        //        {
        //            string saveImagePath = ngPath.Replace(@"\", "/") + "/NG" + "&" + data.endTime.ToString("yyMMddHHmmss") + ".tiff";
        //            HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
        //            data.imagePath = saveImagePath;
        //        }
        //        //if (gateResult == "NG")
        //        //{
        //        //    string saveGateNGImagePath = gateNGPath.Replace(@"\", "/") + "/GateNG&" + data.WheelType + "&" + data.RecognitionTime.ToString("yyMMddHHmmss") + ".tif";
        //        //    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveGateNGImagePath);
        //        //}
        //    }
        //    else
        //        EventMessage.MessageDisplay("磁盘存储空间不足，请检查！", true, false);
        //    //以当前日期保存图像到D盘下
        //    //HOperatorSet.WriteImage(image, "png", 0, "D:\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        //}

        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为MB) 
        ///  </summary> 
        ///  <param name="HardDiskName">代表驱动器的字母(必须大写字母) </param> 
        ///  <returns> </returns> 
        private static long GetHardDiskFreeSpace(string HardDiskName)
        {
            long freeSpace = new long();
            HardDiskName = HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024);
                }
            }
            return freeSpace;
        }

        /// <summary>
        /// 保存图像数据
        /// </summary>
        /// <param name="saveImage">需要保存的图像</param>
        /// <param name="data">识别数据</param>
        /// <param name="dateTime">识别的时间</param>
        /// <param name="gateResult">浇口检测结果</param>
        private void SaveImageDatas(HObject saveImage, Tbl_productiondatamodel data, DateTime dateTime, string gateResult)
        {

        }

        public void Dispose()
        {
            
        }


    }
}
