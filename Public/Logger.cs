using System;
using System.IO;

namespace WheelRecognitionSystem.Public
{


    /// <summary>
    /// 日志工具类，提供按天记录日志的功能
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 写入一条日志信息
        /// </summary>
        /// <param name="message">要写入的信息</param>
        /// <param name="saveDirectory">日志保存目录（自动创建）</param>
        public static void WriteLog(string message, string saveDirectory)
        {
            try
            {
                // 确保目录存在
                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                // 生成当天的日志文件名，例如 2026-04-14.log
                string fileName = $"{DateTime.Now:yyyy-MM-dd}.log";
                string filePath = Path.Combine(saveDirectory, fileName);

                // 构造带时间戳的日志内容
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                // 追加写入文件，每条日志后自动换行
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // 简单处理：可改为记录到控制台或抛出，按需修改
                Console.WriteLine($"写入日志失败: {ex.Message}");
            }
        }
    }


}
