﻿using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.ViewModels.Dialogs;
using WheelRecognitionSystem.ViewModels.Pages;
using WheelRecognitionSystem.Views;
using WheelRecognitionSystem.Views.Dialogs;
using WheelRecognitionSystem.Views.Pages;
using static WheelRecognitionSystem.Public.SystemDatas;

namespace WheelRecognitionSystem
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;

            ThemeManager.Current.SyncTheme();


        }
        private static Mutex mutex;
        protected override Window CreateShell()
        {
            //判断当前程序是否运行
            string processName = Process.GetCurrentProcess().ProcessName;
            mutex = new Mutex(true, processName, out bool createdNew);
            if(!createdNew)
            {
                WMessageBox.Show("当前程序已运行！", MessageType.Default);
                return null;
            }
            //数据存储根目录
            ConfigEdit.ReadAppSettings("RootDirectory", out string rootDirectory);
            RootDirectory = rootDirectory;
            //模板图片保存路径
            ConfigEdit.ReadAppSettings("TemplateImageFolder", out string templateImageFolder);
            TemplateImagesPath = RootDirectory + @"\" + templateImageFolder;
            //活跃模板保存路径
            ConfigEdit.ReadAppSettings("ActiveTemplateFolder", out string activeTemplateFolder);
            ActiveTemplatesPath = RootDirectory + @"\" + activeTemplateFolder;
            //不活跃模板保存路径
            // ConfigEdit.ReadAppSettings("NotActiveTemplateFolder", out string notActiveTemplateFolder);
            //NotActiveTemplatesPath = RootDirectory + @"\" + notActiveTemplateFolder;
            //历史图片保存路径
            ConfigEdit.ReadAppSettings("HistoricalImageFolder", out string historicalImageFolder);
            HistoricalImagesPath = RootDirectory + @"\" + historicalImageFolder;
            //手动图片保存路径
            ConfigEdit.ReadAppSettings("HandImageFolder", out string handImageFolder);
            HandImagesPath = RootDirectory + @"\" + handImageFolder; 
            
            //存储Log文件路径
            string logDirectory = AppDomain.CurrentDomain.BaseDirectory + "Logs";
            //创建系统所需文件夹
            if (!Directory.Exists(RootDirectory)) Directory.CreateDirectory(RootDirectory);
            if (!Directory.Exists(TemplateImagesPath)) Directory.CreateDirectory(TemplateImagesPath);
            if (!Directory.Exists(ActiveTemplatesPath)) Directory.CreateDirectory(ActiveTemplatesPath);
            //if (!Directory.Exists(NotActiveTemplatesPath)) 
            //    Directory.CreateDirectory(NotActiveTemplatesPath);
            //if (!Directory.Exists(DeepParaPath)) 
            //    Directory.CreateDirectory(DeepParaPath);
            if (!Directory.Exists(HistoricalImagesPath)) Directory.CreateDirectory(HistoricalImagesPath);
            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            //创建当月Log文件
            string currentLogPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yy-MM") + "_log.txt";
            if (!File.Exists(currentLogPath)) File.Create(currentLogPath);
            //初始化全局静态消息助手
            EventMessage.MessageHelper = Container.Resolve<IEventAggregator>();
            //打开主窗口
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册功能页面
            //containerRegistry.RegisterForNavigation<MonitoringView>();
            //containerRegistry.RegisterForNavigation<DateSupplementView>();

            containerRegistry.RegisterForNavigation<DisplayInterfaceView>();          
            containerRegistry.RegisterForNavigation<ReportManagementView>();
            containerRegistry.RegisterForNavigation<SystemSettingsView>();
            //containerRegistry.RegisterForNavigation<TemplateManagementView>();
            containerRegistry.RegisterForNavigation<CameraControl>();


            //注册轮型设置弹窗内容
            //containerRegistry.RegisterDialog<DateSupplementView>("DateSupplement");
            //containerRegistry.RegisterDialogWindow<DialogParent>();

            containerRegistry.RegisterDialog<TemplateManagementView>("TemplateManagement");
            
            containerRegistry.RegisterDialog<CameraSetDialog>("CameraSet");
            containerRegistry.RegisterDialog<FileManageDialog>("FileManage");
            containerRegistry.RegisterDialog<UpdateAiFileDialog>("UpdateAiFile");
            //注册弹窗窗口，这句代码会将框架内的默认弹窗窗口替换掉
            containerRegistry.RegisterDialogWindow<MetroDialog>();

            containerRegistry.RegisterInstance<IDialogCoordinator>(DialogCoordinator.Instance);

            //注册viewModel 跨视图模型使用
            //containerRegistry.RegisterSingleton<TemplateManagementViewModel>();

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
