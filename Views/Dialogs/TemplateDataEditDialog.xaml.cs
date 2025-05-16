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
using System.Windows.Shapes;
using Prism.Services.Dialogs;
using Prism.Events;
using Prism.Mvvm;


namespace WheelRecognitionSystem.Views.Dialogs
{
    /// <summary>
    /// TemplateDataEditDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateDataEditDialog : UserControl
    {
        public event Action<IDialogResult> RequestClose;

        public string Message { get;set; }
        
        public TemplateDataEditDialog()
        {
            InitializeComponent();
        }

        
      

        private void SpokeQuantity_tbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void DarkMaxThreshold_tbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void LightMinThreshold_tbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
