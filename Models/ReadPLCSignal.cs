using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 读取PLC信号
    /// </summary>
    public class ReadPLCSignal : BindableBase
    {

        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _wheelCoding;
        /// <summary>
        /// 轮型编码
        /// </summary>
        public string WheelCoding
        {
            get { return _wheelCoding; }
            set { SetProperty(ref _wheelCoding, value); }
        }

        public bool _arrivalSignal;
        /// <summary>
        /// 轮毂到达信号-允许拍照
        /// </summary>
        public bool ArrivalSignal { 
            get { return _arrivalSignal; }
            set { SetProperty(ref _arrivalSignal, value); }
        }




    }
}
