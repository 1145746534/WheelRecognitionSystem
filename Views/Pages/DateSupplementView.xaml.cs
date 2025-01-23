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
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.Views.Pages
{
    /// <summary>
    /// DateSupplementView.xaml 的交互逻辑
    /// </summary>
    public partial class DateSupplementView : UserControl
    {
        public DateSupplementView()
        {
            InitializeComponent();
            EventMessage.MessageHelper.GetEvent<ClearEvent>().Subscribe(ClearHalconWindow);
        }
        public void ClearHalconWindow(string value)
        {
            HSmart.HalconWindow.ClearWindow();
        }
    }
}
