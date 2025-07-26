using HalconDotNet;
using MahApps.Metro.Controls;
using Prism.Events;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.ViewModels;
using WheelRecognitionSystem.Views.Dialogs;
using WheelRecognitionSystem.Views.Pages;
using static WheelRecognitionSystem.Public.ExternalConnections;

namespace WheelRecognitionSystem.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : MetroWindow
    {

        public MainView(RegionManager regionManager)
        {
            InitializeComponent();
            this.Loaded += MainView_Loaded;
            Closing += WindowClose;
            EventMessage.MessageHelper.GetEvent<SystemMessageEvent>().Subscribe(SystemMessageDisplay, ThreadOption.UIThread);

        }

        private void WindowClose(object sender, CancelEventArgs e)
        {
            bool result = WMessageBox.Show("退出系统", "确认退出系统？");
            if (result)
            {
                EventMessage.MessageDisplay("系统退出！", false, true);

                PlcDataInteractionControl = false;
                HeartbeatThreadControl = false;
                Thread.Sleep(1000);

                Application.Current.Shutdown();
            }
            else
                e.Cancel = true;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMessageDisplay();
        }

        /// <summary>
        /// 初始化信息显示
        /// </summary>
        private void InitializeMessageDisplay()
        {
            //读取Log文件
            string strPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yy-MM") + "_log.txt";
            var str = FileHelper.ReadLastLines(strPath, 20);
            if (str != null && str.Count > 0)
            {
                foreach (var data in str)
                {
                    SystemMessage.AppendText(data + "\r\n");
                }
                SystemMessage.ScrollToEnd();
            }
        }

        /// <summary>
        /// 系统信息显示
        /// </summary>
        /// <param name="message"></param>
        private void SystemMessageDisplay(string message)
        {
            string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + "\r\n";
            if (SystemMessage.LineCount > 200)
            {
                SystemMessage.Clear();
            }
            SystemMessage.AppendText(str);
            SystemMessage.ScrollToEnd();
        }


    }
}
