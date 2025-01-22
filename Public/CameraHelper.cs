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

namespace WheelRecognitionSystem.Public
{
    public class CameraHelper
    {

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="cameraIdentifier">相机标识符</param>
        /// <returns>相机句柄</returns>
        public static HTuple ConnectCamera(string cameraIdentifier)
        {
            //HOperatorSet.CloseAllFramegrabbers(); //释放相机句柄  
            try
            {
                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive",
                        -1, "default", -1, "false", "default", cameraIdentifier,
                        0, -1, out HTuple acqHandle);
                //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", cameraIdentifier, 0, -1, out HTuple acqHandle);

                //SetExposureTime(acqHandle, 11000.0);
                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "Off");
                //开始采集图像
                HOperatorSet.GrabImageStart(acqHandle, -1);
                return acqHandle;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }

        }
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="acqHandle"></param>
        /// <param name="value"></param>
        public static void SetExposureTime(HTuple acqHandle, HTuple value)
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
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="acqHandle"></param>
        public static void DisconnectCamera(HTuple acqHandle)
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
        /// <summary>
        /// 单次采集图像方法
        /// </summary>
        /// <param name="acqHandle"></param>
        /// <returns></returns>
        public static HObject Grabimage(HTuple acqHandle)
        {
            if (acqHandle == null)
            {
                return null;
            }
            HObject ho_Image;//定义图片变量
            HOperatorSet.GenEmptyObj(out ho_Image);// 初始化本地图像空间的变量
            //采集图像
            HOperatorSet.GrabImageAsync(out ho_Image, acqHandle, -1);
            //图片自适应窗口
            //HOperatorSet.GetImageSize(ho_Image, out imageWidth, out imageHeight);
            //HOperatorSet.SetPart(HW.HalconWindow, 0, 0, imageHeight - 1, imageWidth - 1);
            //显示图像
            //HOperatorSet.DispObj(ho_Image, HW.HalconWindow);

            //以当前日期保存图像到D盘下
            //HOperatorSet.WriteImage(ho_Image, "png", 0, "D:\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            return ho_Image;
        }

        public static void SavePic(HObject saveImage, InteractS7PLCModel data)
        {
            if (saveImage == null)
            {
                return;
            }
            //月文件夹路径
            string monthPath = HistoricalImagesPath + @"\" + data.endTime.Month + "月";
            //日文件夹路径
            string dayPath = HistoricalImagesPath + @"\" + data.endTime.Month + @"月\" + data.endTime.Day + "日";
            //当日未识别文件夹路径
            string ngPath = HistoricalImagesPath + @"\" + data.endTime.Month + @"月\" + data.endTime.Day + @"日\NG";
            if (Directory.Exists(monthPath) == false) 
                Directory.CreateDirectory(monthPath);
            if (Directory.Exists(dayPath) == false)
                Directory.CreateDirectory(dayPath);
            if (Directory.Exists(ngPath) == false)
                Directory.CreateDirectory(ngPath);
            var diskFree = GetHardDiskFreeSpace("D");//获取D盘剩余空间
            if (diskFree > 200)
            {
                if (data.status == "识别成功")
                {
                    //保存轮型的目录
                    string saveWheelTypePath = dayPath + @"\" + data.wheelType.Trim('_');
                    if (Directory.Exists(saveWheelTypePath) == false) Directory.CreateDirectory(saveWheelTypePath);
                    string saveImagePath = saveWheelTypePath.Replace(@"\", "/") + "/" + data.wheelType + "&" + data.endTime.ToString("yyMMddHHmmss") + ".BMP";
                    HOperatorSet.WriteImage(saveImage, "BMP", 0, saveImagePath);
                    data.imagePath = saveImagePath;
                }
                else
                {
                    string saveImagePath = ngPath.Replace(@"\", "/") + "/NG" + "&" + data.endTime.ToString("yyMMddHHmmss") + ".BMP";
                    HOperatorSet.WriteImage(saveImage, "BMP", 0, saveImagePath);
                    data.imagePath = saveImagePath;
                }
                //if (gateResult == "NG")
                //{
                //    string saveGateNGImagePath = gateNGPath.Replace(@"\", "/") + "/GateNG&" + data.WheelType + "&" + data.RecognitionTime.ToString("yyMMddHHmmss") + ".tif";
                //    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveGateNGImagePath);
                //}
            }
            else
                EventMessage.MessageDisplay("磁盘存储空间不足，请检查！", true, false);
            //以当前日期保存图像到D盘下
            //HOperatorSet.WriteImage(image, "png", 0, "D:\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }

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
        private void SaveImageDatas(HObject saveImage, ProductionDataModel data, DateTime dateTime, string gateResult)
        {
            
        }

    }
}
