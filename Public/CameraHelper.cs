using HalconDotNet;
using NPOI.OpenXmlFormats.Vml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public static void SavePic(HObject image)
        {
            //以当前日期保存图像到D盘下
            HOperatorSet.WriteImage(image, "png", 0, "D:\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }
    }
}
