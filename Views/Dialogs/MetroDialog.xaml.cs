﻿using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace WheelRecognitionSystem.Views.Dialogs
{
    /// <summary>
    /// MetroDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MetroDialog : MetroWindow, IDialogWindow
    {
        public MetroDialog()
        {
            InitializeComponent();
        }
        public IDialogResult Result { get; set; }

    }
}
