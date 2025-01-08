using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Public
{
    public class DataValidationBase : IDataErrorInfo
    {
        public string Error { get; set; }
        public string this[string columnName]
        {       
            get
            {

                return "";
            }
        }

    }
}
