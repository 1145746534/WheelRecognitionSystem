using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.Models;

namespace WheelRecognitionSystem.Public
{
    public static class TemplateManager
    {
        private static readonly System.Timers.Timer _cleanupTimer;
        private static string _storageDirectory;

        static TemplateManager()
        {
            _cleanupTimer = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            _cleanupTimer.Elapsed += CleanupHandler;
            _cleanupTimer.Start();
        }

        // 初始化存储路径
        public static void Initialize(string storageDir)
        {
            _storageDirectory = storageDir;
            Directory.CreateDirectory(storageDir); // 确保目录存在
        }

        // 定时清理内存
        private static void CleanupHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            var threshold = DateTime.Now.AddDays(-1);

            //foreach (var model in Models.Where(m =>
            //    m.IsLoaded &&
            //    m.LastUsedTime < threshold))
            //{
            //    model.UnloadTemplate();
            //}
        }

        // 显式加载指定模板（可选）
        public static void EnsureLoaded(TemplatedataModels model)
        {
            // 触发get访问器
            var temp = model.Template;
        }
    }
}
