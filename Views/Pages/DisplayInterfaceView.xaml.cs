using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WheelRecognitionSystem.ViewModels.Pages;

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
        public event MyEventHandler RealTimeStartEvent;
        public event MyEventHandler RealTimeStopEvent;

        public DisplayInterfaceViewModel ViewModel;


        public DisplayInterfaceView()
        {
            InitializeComponent();

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)            
                ViewModel = this.DataContext as DisplayInterfaceViewModel;           
            if (RealTimeStartEvent == null)
                RealTimeStartEvent += ViewModel.RealTime;
            if (RealTimeStopEvent == null)
                RealTimeStopEvent += ViewModel.StopReal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)VisualTreeHelper.GetParent(button);
            TextBlock block1 = (TextBlock)stackPanel.Children[1];
            string name = block1.Text; //显示窗口名称
            string btuTip = button.ToolTip.ToString();
            if (btuTip.Equals("实时取像"))
            {
                button.ToolTip = "关闭实时";
                Grid grid = (Grid)button.Content;
                TextBlock block2 = grid.Children[0] as TextBlock;
                block2.Text = "\xe6b4";
                RealTimeStartEvent?.Invoke(name, new MyEventArgs("实时取像"));
            }
            else if (btuTip.Equals("关闭实时"))
            {
                button.ToolTip = "实时取像";
                Grid grid = (Grid)button.Content;
                TextBlock block2 = grid.Children[0] as TextBlock;
                block2.Text = "\xe6bb";
                RealTimeStopEvent?.Invoke(name, new MyEventArgs("关闭实时"));

            }



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
