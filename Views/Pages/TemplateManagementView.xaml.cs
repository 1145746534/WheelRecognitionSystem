using HalconDotNet;
using Prism.Events;
using Prism.Services.Dialogs;
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
using Prism.Ioc;
using Org.BouncyCastle.Asn1.Ocsp;
using WheelRecognitionSystem.ViewModels.Pages;

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


        

        private void TemplateDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = this.DataContext as TemplateManagementViewModel;
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.Items.Count > 0 && dataGrid.CurrentItem != null)
            {
                viewModel.DataGridSelectedItem = dataGrid.CurrentItem as sys_bd_Templatedatamodel;
                viewModel.DataGridSelectedIndex = viewModel.DataGridSelectedItem.Index - 1;

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem();
                menuItem.Header = "修改";
                menuItem.FontSize = 14;
                menuItem.Click += viewModel.MenuItem_Click;
                contextMenu.Items.Add(menuItem);
                contextMenu.IsOpen = true;
            }
        }
    }
}
