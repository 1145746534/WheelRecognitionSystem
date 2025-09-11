using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 信号交互类-PLC发送的信号
    /// </summary>
    public class InteractS7PLCModel : IDisposable
    {
        /// <summary>
        /// 轮毂到位延时
        /// </summary>
        public int ArrivalDelay;

        public HObject Image;

        /// <summary>
        /// 手动读取图片地址
        /// </summary>
        public string ManualReadImagePath;

        /// <summary>
        /// 是否二次拍照
        /// </summary>
        public bool IsSecondPhoto = false;

        /// <summary>
        /// 二次曝光值
        /// </summary>
        public int SecondPhotoExposure = 0;

        /// <summary>
        /// 二次拍照图
        /// </summary>
        public HObject SecondImage;

        /// <summary>
        /// 二次拍照图保存位置
        /// </summary>
        public string SecondImagePath;



        /// <summary>
        /// 保存或者移动图片 
        /// </summary>
        public bool IsSaveOrMoveImage = true;

        /// <summary>
        /// 是否显示图片到界面
        /// </summary>
        public bool IsDisplay = true;

        /// <summary>
        /// 是否发送PLC信号
        /// </summary>
        public bool IsSendPLCInfo = true;

        /// <summary>
        /// 是否通知界面
        /// </summary>
        public bool IsInteraction = true;

        /// <summary>
        /// 数据处理方式
        /// </summary>
        public InfoHandle InfoHanleWay = InfoHandle.Save;


        /// <summary>
        /// 读取到的PLC信息
        /// </summary>
        public ReadPLCSignal readPLCSignal;



        /// <summary>
        /// 识别结果
        /// </summary>
        public RecognitionResultModel resultModel;

        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime starTime;
        /// <summary>
        /// 结束处理时间
        /// </summary>
        public DateTime endTime;

        public string imagePath;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public TimeSpan Interval
        {
            get { return endTime.Subtract(starTime); }
        }

        /// <summary>
        /// 释放图片
        /// </summary>
        public void Dispose()
        {
            if (Image != null && Image.IsInitialized())
            {
                Image.Dispose();
                Image = null;
            }
            if(SecondImage != null && SecondImage.IsInitialized())
            {
                SecondImage.Dispose();
                SecondImage = null;
            }
        }
    }

    public enum InfoHandle
    {
        /// <summary>
        /// 数据保存
        /// </summary>
        Save = 0,
        /// <summary>
        /// 数据修改
        /// </summary>
        Update = 1,
    }
}
