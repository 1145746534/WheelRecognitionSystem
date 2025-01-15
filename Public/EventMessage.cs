using HalconDotNet;
using Prism.Events;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.Models;

namespace WheelRecognitionSystem.Public
{
    /// <summary>
    /// 事件消息
    /// </summary>
    public class EventMessage
    {
        /// <summary>
        /// 全局静态消息助手
        /// </summary>
        public static IEventAggregator MessageHelper { get; set; }
        /// <summary>
        /// 信息框信息显示，带Log写入
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="isDisplay">是否在信息框显示信息</param>
        /// <param name="isLog">是否将信息存储到Log</param>
        public static void MessageDisplay(string message,bool isDisplay, bool isToLog)
        {
            if(isDisplay)
            {
                MessageHelper.GetEvent<SystemMessageEvent>().Publish(message);
            }
            if(isToLog)
            {
                try
                {
                    string currentLogPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yy-MM") + "_log.txt";
                    FileStream fs;
                    StreamWriter sw;
                    if (File.Exists(currentLogPath))
                    //验证文件是否存在，有则追加，无则创建
                    {
                        fs = new FileStream(currentLogPath, FileMode.Append, FileAccess.Write);
                    }
                    else
                    {
                        fs = new FileStream(currentLogPath, FileMode.Create, FileAccess.Write);
                    }
                    sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message);
                    sw.Close();
                    fs.Close();
                }
                catch { }
            }
        }
        /// <summary>
        /// 全局系统信息显示（标题栏位置）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Type"></param>
        public static void SystemMessageDisplay(string message, MessageType Type)
        {
            MessageModel model = new MessageModel
            {
                Message = message,
                Type = Type
            };
            MessageHelper.GetEvent<SystemMessageDisplayEvent>().Publish(model);
        }
    }

    /// <summary>
    /// 模板表格数据编辑事件，用于编辑数据时将表格显示滚动到编辑的项
    /// </summary>
    public class TemplateDataEditEvent : PubSubEvent<TemplateDataModel> { }
    /// <summary>
    /// 系统消息框信息事件的发布和订阅
    /// </summary>
    public class SystemMessageEvent : PubSubEvent<string> { }
    /// <summary>
    /// 匹配结果数据显示事件
    /// </summary>
    public class MatchResultDatasDisplayEvent : PubSubEvent<List<MatchResultModel>> { }
    /// <summary>
    /// 自动识别结果显示事件
    /// </summary>
    public class AutoRecognitionResultDisplayEvent : PubSubEvent<AutoRecognitionResultDisplayModel> { }
    /// <summary>
    /// 信号交互-处理事件
    /// </summary>
    public class InteractHandleEvent : PubSubEvent<InteractS7PLCModel> { }

    /// <summary>
    /// 信号交互-回复事件
    /// </summary>
    public class InteractCallEvent : PubSubEvent<InteractS7PLCModel> { }

    /// <summary>
    /// 分选数据显示事件
    /// </summary>
    public class ScreenedDataDisplayEvent : PubSubEvent<List<ScreenedDataModel>> { }
    /// <summary>
    /// 系统信息显示事件
    /// </summary>
    public class SystemMessageDisplayEvent : PubSubEvent<MessageModel> { }
    /// <summary>
    /// 模板参数设置数据改变事件
    /// </summary>
    public class ParameterSettingChangedEvent : PubSubEvent<string> { }
    /// <summary>
    /// 设置跳转页面事件
    /// </summary>
    public class ServletInfoEvent : PubSubEvent<ServletInfoModel> { }

    /// <summary>
    /// 模板图片更新事件
    /// </summary>
    public class TemplatePicUpdateEvent: PubSubEvent<ServletInfoModel> { }

    /// <summary>
    /// 模板表格数据更新事件，用于执行模板动态调整时的数据更新
    /// </summary>
    public class TemplateDataUpdataEvent : PubSubEvent<string> { }
    /// <summary>
    /// 识别暂停设置事件
    /// </summary>
    public class RecognitionPauseSettingEvent : PubSubEvent<string> { }
    /// <summary>
    /// 当天识别数据8点更新事件
    /// </summary>
    public class CurrentDayDataUpdataEvent : PubSubEvent<string> { }
}
