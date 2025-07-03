﻿using System;
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

namespace WheelRecognitionSystem.Views.Dialogs
{
    /// <summary>
    /// WheelTypeSettingDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WheelTypeSettingDialog : UserControl
    {
        public WheelTypeSettingDialog()
        {
            InitializeComponent();
            Loaded += WheelTypeSettingDialog_Loaded;
        }

        private void WheelTypeSettingDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbw.Focus();
            this.tbw.SelectAll();
            cmb.ItemsSource =  new List<string>() { "成品", "半成品" };
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
