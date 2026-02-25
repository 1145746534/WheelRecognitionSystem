using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Models
{
    /// <summary>
    /// 修改数据
    /// </summary>
    public class ModfiModel
    {
        /// <summary>
        /// 轮毂编码
        /// </summary>
        public string WheelCoding { get; set; }
        /// <summary>
        /// 回流或下转
        /// </summary>
        public string FlowOrDown { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string Station { get; set; }

    }
}
