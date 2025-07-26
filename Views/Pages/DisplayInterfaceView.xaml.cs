using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WheelRecognitionSystem.ViewModels.Pages;
using HalconDotNet;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;
using Prism.Regions;

namespace WheelRecognitionSystem.Views.Pages
{
    // 声明委托
    public delegate void MyEventHandler(object sender, MyEventArgs e);
    /// <summary>
    /// DisplayInterfaceView.xaml 的交互逻辑
    /// </summary>
    public partial class DisplayInterfaceView : UserControl
    {
        // 声明实时事件



        public DisplayInterfaceView()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }



    // 自定义事件参数类，继承自 EventArgs
    public class MyEventArgs : EventArgs
    {
        public string Message { get; }

        public MyEventArgs(string message)
        {
            Message = message;
        }
    }
}
