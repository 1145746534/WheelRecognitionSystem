using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 识别数据模型
    /// </summary>
    public class IdentityDataModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int Index { get; set; }
        /// <summary>
        /// 轮型
        /// </summary>
        public string WheelType { get; set; }
        /// <summary>
        /// 识别耗时
        /// </summary>
        public string SpendTime { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        public string Similarity { get; set; }
        /// <summary>
        /// 识别时间
        /// </summary>
        public string IdentificationTime { get; set; }
    }
}
