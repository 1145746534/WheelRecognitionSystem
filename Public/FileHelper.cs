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
        /// 提取文件所在目录并重命名目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newDirectoryName"></param>
        public static string RenameDirectory(string filePath)
        {
            string newDirectory = null;
            try
            {
                FileInfo file = new FileInfo(filePath);
                string parentDir = file.Directory?.FullName;
                if (parentDir != null)
                {
                    string newName = "~" + file.Directory.Name;
                    string sourceDirectory = Path.GetDirectoryName(filePath);
                    string parentDirectory = Directory.GetParent(sourceDirectory)?.FullName;


                    string targetDirectory = Path.Combine(parentDirectory, newName);


                    Directory.CreateDirectory(targetDirectory);

                    Console.WriteLine($"目录已成功重命名: \n" +
                                      $"原目录: {sourceDirectory}\n" +
                                      $"新目录: {targetDirectory}");
                    newDirectory = targetDirectory;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"操作失败: {ex.Message}");
            }
            return newDirectory;
        }


        /// <summary>
        /// 拷贝文件到另外一个目录
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationDirectory"></param>
        public static string CopyFile(string sourcePath, string destinationDirectory)
        {
            string tagerPath = null;
            try
            {
                // 验证源文件是否存在
                if (!File.Exists(sourcePath))
                {
                    throw new FileNotFoundException("源文件不存在", sourcePath);
                }

                // 确保目标目录存在
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // 获取源文件名
                string fileName = Path.GetFileName(sourcePath);

                // 构建目标路径
                string destPath = Path.Combine(destinationDirectory, fileName);
                tagerPath = destPath;
                // 复制文件（覆盖已存在的文件）
                File.Copy(sourcePath, destPath, true);
                tagerPath = destPath;
                Console.WriteLine($"文件已成功复制到: {destPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"复制文件时出错: {ex.Message}");
                // 实际应用中可能需要重新抛出异常或记录日志

            }
            return tagerPath;
        }




        /// <summary>
        /// 复制文件到指定目录并重命名
        /// </summary>
        /// <param name="sourceFilePath">源文件完整路径</param>
        /// <param name="destinationDirectory">目标目录</param>
        /// <param name="newFileName">新文件名（包含扩展名）</param>
        /// <param name="overwrite">是否覆盖已存在的文件</param>
        /// <returns>新文件的完整路径</returns>
        public static string CopyAndRenameFile(
            string sourceFilePath,
            string destinationDirectory,
            string newFileName,
            bool overwrite = true)
        {
            try
            {
                // 验证源文件是否存在
                if (!File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException($"源文件不存在: {sourceFilePath}");
                }

                // 确保目标目录存在
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Console.WriteLine($"已创建目录: {destinationDirectory}");
                }

                // 构造目标文件完整路径
                string destinationFilePath = Path.Combine(destinationDirectory, newFileName);

                // 检查是否覆盖
                if (File.Exists(destinationFilePath) && !overwrite)
                {
                    throw new IOException($"目标文件已存在且不允许覆盖: {destinationFilePath}");
                }

                // 执行文件复制
                File.Copy(sourceFilePath, destinationFilePath, overwrite);

                Console.WriteLine($"文件复制并重命名成功:\n" +
                                  $"源文件: {sourceFilePath}\n" +
                                  $"新文件: {destinationFilePath}");

                return destinationFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"操作失败: {ex.Message}");
                throw; // 根据需求决定是否重新抛出异常
            }
        }




        /// <summary>
        /// 移动文件并覆盖目标文件
        /// </summary>
        public static bool MoveFile(string sourceFilePath, string targetFilePath, bool overwrite = false)
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

        /// <summary>
        /// 从路径中提取目标目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ExtractTargetDirectory(string path)
        {
            // 将路径分割成部分
            string[] parts = path.Split(Path.DirectorySeparatorChar);

            // 找到"HistoricalImages"的索引
            int historicalImagesIndex = -1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals("HistoricalImages", StringComparison.OrdinalIgnoreCase))
                {
                    historicalImagesIndex = i;
                    break;
                }
            }

            // 如果找到"HistoricalImages"，则返回它后面的目录
            if (historicalImagesIndex != -1 && historicalImagesIndex + 1 < parts.Length)
            {
                return parts[historicalImagesIndex + 1];
            }

            // 如果没有找到，返回空字符串或抛出异常
            return string.Empty;
        }

        /// <summary>
        /// 替换路径中的目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newDirectory"></param>
        /// <returns></returns>
       public static string ReplaceDirectoryInPath(string path, string newDirectory)
        {
            // 将路径分割成部分
            string[] parts = path.Split(Path.DirectorySeparatorChar);

            // 找到"HistoricalImages"的索引
            int historicalImagesIndex = -1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals("HistoricalImages", StringComparison.OrdinalIgnoreCase))
                {
                    historicalImagesIndex = i;
                    break;
                }
            }

            // 如果找到"HistoricalImages"，则替换它后面的目录
            if (historicalImagesIndex != -1 && historicalImagesIndex + 1 < parts.Length)
            {
                parts[historicalImagesIndex + 1] = newDirectory;

                // 重新组合路径
                return string.Join(Path.DirectorySeparatorChar.ToString(), parts);
            }

            // 如果没有找到，返回原始路径
            return path;
        }
    }
}
