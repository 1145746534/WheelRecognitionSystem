using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using WheelRecognitionSystem.Models;
using HalconDotNet;

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
        public static void MessageDisplay(string message, bool isDisplay, bool isToLog)
        {
            if (isDisplay)
            {
                MessageHelper.GetEvent<SystemMessageEvent>().Publish(message);
            }
            if (isToLog)
            {
                try
                {
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} {message}");
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

            //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} {message}");
            MessageModel model = new MessageModel
            {
                Message = message,
                Type = Type
            };
            MessageHelper.GetEvent<SystemMessageDisplayEvent>().Publish(model);

        }
    }



    public class TemplateClearEvent : PubSubEvent<string> { }
    /// <summary>
    /// 模板表格数据编辑事件，用于编辑数据时将表格显示滚动到编辑的项
    /// </summary>
    public class TemplateDataEditEvent : PubSubEvent<sys_bd_Templatedatamodel> { }
    /// <summary>
    /// 系统消息框信息事件的发布和订阅
    /// </summary>
    public class SystemMessageEvent : PubSubEvent<string> { }

    /// <summary>
    /// 系统信息显示事件
    /// </summary>
    public class SystemMessageDisplayEvent : PubSubEvent<MessageModel> { }

    /// <summary>
    /// 信号交互-回复事件
    /// </summary>
    public class InteractCallEvent : PubSubEvent<InteractS7PLCModel> { }


    /// <summary>
    /// 相机参数修改反馈事件
    /// </summary>
    public class CameraParameterChangedEvent : PubSubEvent<object> { }

    /// <summary>
    /// 根据视图名称采集对应相机图像
    /// </summary>
    public class GetGrabimageByViewEvent : PubSubEvent<InteractS7PLCModel> { }

    /// <summary>
    /// 图片推送处理事件
    /// </summary>
    public class ImagePushHandleEvent : PubSubEvent<InteractS7PLCModel> { }
    /// <summary>
    /// 识别结果推送显示事件
    /// </summary>
    public class RecognitionDisplayEvent : PubSubEvent<AutoRecognitionResultDisplayModel> { }

    /// <summary>
    /// 模板参数刷新事件
    /// </summary>
    public class RefreshNCCParaEvent : PubSubEvent { }

}
