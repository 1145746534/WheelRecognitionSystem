using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;


namespace WheelRecognitionSystem.Models.IModels
{
    public interface IMyCamera
    {
        bool Connect(string camerID);

        void Disconnect();

        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="value"></param>
        void SetExposureTime( float value);

        HObject Grabimage();

    }
}
