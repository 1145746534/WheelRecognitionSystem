using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    public class MatchResultModel
    {
        public int Index { get; set; }
        /// <summary>
        /// 轮毂型号
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        public string Similarity { get; set; }
    }
}
