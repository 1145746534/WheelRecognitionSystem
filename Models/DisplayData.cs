using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 显示数据
    /// </summary>
    public class DisplayData : BindableBase
    {
        private string _station;
        /// <summary>
        /// 工站
        /// </summary>
        public string Station
        {
            get { return _station; }
            set { SetProperty(ref _station, value); }
        }

        private string _status;
        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _wheelType;
        /// <summary>
        /// 轮型
        /// </summary>
        public string WheelType
        {
            get { return _wheelType; }
            set { SetProperty(ref _wheelType, value); }
        }


        private decimal _similarity;
        /// <summary>
        /// 相似度
        /// </summary>
        public decimal Similarity
        {
            get { return _similarity; }
            set { SetProperty(ref _similarity, value); }
        }  
        
        private float? _timeConsumed;
        /// <summary>
        /// 用时
        /// </summary>
        public float? TimeConsumed
        {
            get { return _timeConsumed; }
            set { SetProperty(ref _timeConsumed, value); }
        }   
        
        private string _remark;
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value); }
        }


    }
}
