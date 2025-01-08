using HalconDotNet;
using Prism.Events;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.ViewModels;
using WheelRecognitionSystem.Views.Dialogs;
using static WheelRecognitionSystem.Public.ExternalConnections;

namespace WheelRecognitionSystem.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public string ButtonIconCode
        {
            get { return (string)GetValue(ButtonIconCodeProperty); }
            set { SetValue(ButtonIconCodeProperty, value); }
        }
        public static readonly DependencyProperty ButtonIconCodeProperty =
            DependencyProperty.Register("ButtonIconCode", typeof(string), typeof(MainView), new PropertyMetadata(default(string)));

        public Thickness WindowThickness
        {
            get { return (Thickness)GetValue(WindowThicknessProperty); }
            set { SetValue(WindowThicknessProperty, value); }
        }
        public static readonly DependencyProperty WindowThicknessProperty =
            DependencyProperty.Register("WindowThickness", typeof(Thickness), typeof(MainView), new PropertyMetadata(new Thickness(0)));

        public MainView()
        {
            InitializeComponent();
            ButtonIconCode = "\ue692";
            this.Loaded += MainView_Loaded;
            EventMessage.MessageHelper.GetEvent<SystemMessageEvent>().Subscribe(SystemMessageDisplay, ThreadOption.UIThread);
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
            if(str != null && str.Count >0)
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
            SystemMessage.AppendText(str);
            SystemMessage.ScrollToEnd();
        }

        private void WindowMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void WindowMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                ButtonIconCode = "\ue692";
                WindowThickness = new Thickness(0);
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                ButtonIconCode = "\ue65d";
                WindowThickness = new Thickness(7);
            }
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            bool result = WMessageBox.Show("退出系统", "确认退出系统？");
            if (result)
            {
                EventMessage.MessageDisplay("系统退出！", false, true);
                CameraHandle = null;
                MainThreadControl = false;
                ExternalConnectionThreadControl = false;
                PlcDataInteractionControl = false;
                HeartbeatThreadControl = false;
                Thread.Sleep(1000);
                HOperatorSet.CloseFramegrabber(CameraHandle);
                Application.Current.Shutdown();
            }
        }
    }
}
