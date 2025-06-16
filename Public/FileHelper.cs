using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.Models;

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

        /// <summary>
        /// 移动文件并覆盖目标文件
        /// </summary>
        public static bool  MoveFile(string sourceFilePath, string targetFilePath, bool overwrite = false)
        {
            try
            {
                // 1. 校验源文件是否存在
                if (!File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException("源文件不存在", sourceFilePath);
                }

                // 2. 确保目标目录存在
                string targetDir = Path.GetDirectoryName(targetFilePath);
                Directory.CreateDirectory(targetDir);

                // 3. 如果目标文件存在且允许覆盖，先删除目标文件
                if (overwrite && File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                // 4. 移动文件（自动删除源文件）
                File.Move(sourceFilePath, targetFilePath);

                return true;
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException ||
                ex is PathTooLongException ||
                ex is DirectoryNotFoundException
            )
            {
                Console.WriteLine($"文件移动失败: {ex.Message}");
                
                return false;
            }
        }
   
        
    }
}
