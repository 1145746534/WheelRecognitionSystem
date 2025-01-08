using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelRecognitionSystem.Public
{
    public class ConfigEdit
    {
        /// <summary>
        /// 读取应用设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ReadAppSettings(string key, out string value)
        {
            //获取Configuration对象
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取Value
            value = config.AppSettings.Settings[key].Value;
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 写入应用设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 增加应用设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //增加
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 删除应用设置
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppSettings(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //删除
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
