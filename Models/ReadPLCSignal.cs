﻿using Prism.Mvvm;
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

        //public event Action ArrivalSignalTriggered; // 定义触发事件

        public event EventHandler<EventArgs> ArrivalSignalTriggered;

        private int _index;
        /// <summary>
        /// 名称
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

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
        public bool ArrivalSignal
        {
            get { return _arrivalSignal; }
            set {
                bool oldValue = _arrivalSignal;
                SetProperty(ref _arrivalSignal, value);
                // 仅在值从 false 变为 true 时触发
                if (oldValue != value && value)
                {
                    // 触发时传递参数
                    ArrivalSignalTriggered?.Invoke(this, EventArgs.Empty);
                }
            }

        }



        public int newArrival = 0;


        private int _wheelDefect;
        /// <summary>
        /// 缺陷编码
        /// </summary>
        public int WheelDefect
        {
            get { return _wheelDefect; }
            set { SetProperty(ref _wheelDefect, value); }
        }

        private float _wheelTemperature;
        /// <summary>
        /// 轮毂温度
        /// </summary>
        public float WheelTemperature
        {
            get { return _wheelTemperature; }
            set { SetProperty(ref _wheelTemperature, value); }
        }


        private float _wheelHeight;
        /// <summary>
        /// 轮毂高度
        /// </summary>
        public float WheelHeight
        {
            get { return _wheelHeight; }
            set { SetProperty(ref _wheelHeight, value); }
        }

    }
}
