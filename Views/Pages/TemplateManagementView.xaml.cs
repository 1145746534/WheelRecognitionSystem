using HalconDotNet;
using Prism.Events;
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
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.Views.Pages
{
    /// <summary>
    /// TemplateManagementViewModel.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateManagementView : UserControl
    {
        public TemplateManagementView()
        {
            InitializeComponent();
            //订阅模板数据编辑的消息
            EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Subscribe(Edit);
        }

        //将模板数据窗口的显示滚动到model项
        private void Edit(sys_bd_Templatedatamodel model)
        {
            TemplateDataGrid.ScrollIntoView(model);
        }
    }
}
