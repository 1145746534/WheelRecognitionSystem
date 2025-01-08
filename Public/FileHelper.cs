using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Public
{
    public class FileHelper
    {
        /// <summary>
        /// 从文件的最后读取指定的行数的内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="lineCount">读取的行数</param>
        /// <returns></returns>
        public static List<string> ReadLastLines(string filePath, int lineCount)
        {
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (lines.Count >= lineCount)
                        {
                            lines.RemoveAt(0);
                        }
                        lines.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
            return lines;
        }
    }
}
