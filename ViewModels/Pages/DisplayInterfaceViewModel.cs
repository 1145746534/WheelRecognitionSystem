using HalconDotNet;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.Views.Pages;
using static WheelRecognitionSystem.Public.SystemDatas;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using MvCameraControl;
using System.IO;
using System.ComponentModel;
using System.Windows;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using MvCamCtrl.NET;
using System.Collections;
using Prism.Events;
using Microsoft.Office.Interop.Excel;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class DisplayInterfaceViewModel : BindableBase
    {


        public string Title { get; set; } = "主控显示";


        // 
        public DelegateCommand LoadedCommand { get; private set; }

        /// <summary>
        /// 弹窗服务
        /// </summary>
        readonly IDialogService _dialogService;

        private readonly IContainerProvider _containerProvider;

        private IRegionManager _regionManager;

        public DisplayInterfaceViewModel(IRegionManager regionManager, IDialogService dialogService, IContainerProvider containerProvider)
        {
            _dialogService = dialogService;
            _containerProvider = containerProvider;
            _regionManager = regionManager;

            _regionManager.RegisterViewWithRegion("DisplayRegion1", typeof(CameraControl));
            _regionManager.RegisterViewWithRegion("DisplayRegion2", typeof(CameraControl));
            _regionManager.RegisterViewWithRegion("DisplayRegion3", typeof(CameraControl));
            _regionManager.RegisterViewWithRegion("DisplayRegion4", typeof(CameraControl));
            _regionManager.RegisterViewWithRegion("DisplayRegion5", typeof(CameraControl));
            LoadedCommand = new DelegateCommand(OnControlLoaded);



            //订阅子界面反馈
            EventMessage.MessageHelper.GetEvent<CameraParameterChangedEvent>().Subscribe(CameraParameterChanged);
            //订阅识别结果显示事件
            EventMessage.MessageHelper.GetEvent<RecognitionDisplayEvent>().Subscribe(RecognitionDisplay, ThreadOption.UIThread, // 确保在UI线程执行
                            keepSubscriberReferenceAlive: true);
            //订阅采集图像事件
            EventMessage.MessageHelper.GetEvent<GetGrabimageByViewEvent>().Subscribe(GrabimageByView, ThreadOption.UIThread, // 确保在UI线程执行
                            keepSubscriberReferenceAlive: true);
            //SDKSystem.Initialize();
        }



        // 方法，当Loaded事件触发时执行
        private async void OnControlLoaded()
        {
            Console.WriteLine($"加载显示界面");
            await Task.Delay(1000);
            //查询数据库中所有的相机信息
            SqlSugarClient sDB = new SqlAccess().SystemDataAccess;
            List<Sys_bd_camerainformation> DatasCamera = sDB.Queryable<Sys_bd_camerainformation>().OrderBy(o => o.ID).ToList();
            sDB.Close(); sDB.Dispose();

            for (int i = 0; i < 5; i++)
            {
                if (DatasCamera.Count > i)
                {
                    int ID = i + 1;
                    SetChildViewModelParameter($"DisplayRegion{ID}", DatasCamera[i]);
                }
            }
        }
        /// <summary>
        /// 初始化给个区域的参数
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="camera"></param>
        private void SetChildViewModelParameter(string regionName, Sys_bd_camerainformation camera)
        {
            // 获取 ViewModel
            CameraControlViewModel viewModel = GetViewModelByName(regionName); ;

            // 传递参数
            if (viewModel != null)
            {
                //调用初始化方法：
                viewModel.Initialize(camera);
            }

        }

        /// <summary>
        /// 图像结果下发显示
        /// </summary>
        /// <param name="obj"></param>
        private void RecognitionDisplay(AutoRecognitionResultDisplayModel obj)
        {
            if (obj.Tag == null)
            {
                obj.Dispose();
            }
            CameraControlViewModel viewModel = GetViewModelByName(obj.Tag);
            if (viewModel != null)
            {
                viewModel.ResultDisplay(obj);

            }
            //Console.WriteLine("步骤1");

        }


        /// <summary>
        /// 采集指定区域的图像
        /// </summary>
        public void GrabimageByView(InteractS7PLCModel interact)
        {
            int index = interact.readPLCSignal.Index + 1;
            string name = $"DisplayRegion{index}";
          
            CameraControlViewModel viewModel = GetViewModelByName(name);
            if (viewModel != null)
            {
                viewModel.SetExposure();
                HObject iamge = viewModel.GetImage();
                //string savePath = GetImageSavePath(SaveWay.Hand, HandImagesPath);
                //SaveImageDatasAsync(iamge, savePath);
                interact.Image = CloneImageSafely(iamge);
                if (interact.IsSecondPhoto)
                {
                    viewModel.SetExposure(interact.SecondPhotoExposure);
                    
                   
                    HObject SecondIamge = viewModel.GetImage();
                    //string savePath2 = GetImageSavePath(SaveWay.Hand, HandImagesPath);
                    //SaveImageDatasAsync(SecondIamge, savePath2);
                    //bool areEqual = iamge == SecondIamge; // 比较对象引用
                    interact.SecondImage = CloneImageSafely(SecondIamge);
                    //Console.WriteLine($"首次图 - {areEqual}：{iamge.GetHashCode()} 二次图：{SecondIamge.GetHashCode()}");
                    SafeDisposeHObject(ref SecondIamge);
                }
               
                SafeDisposeHObject(ref iamge);
              
               
            }
                    
            //推送回去处理
            EventMessage.MessageHelper.GetEvent<ImagePushHandleEvent>().Publish(interact);
        }


        /// <summary>
        /// 根据区域名称获取绑定的视图模型
        /// </summary>
        /// <param name="DisplayRegionName"></param>
        /// <returns></returns>
        public CameraControlViewModel GetViewModelByName(string DisplayRegionName)
        {
            try
            {
                // 1. 获取区域
                IRegion region = _regionManager.Regions[DisplayRegionName];
                // 2. 获取区域中的视图（假设只有一个视图）
                CameraControl view = region.Views.FirstOrDefault() as CameraControl;
                if (view != null)
                {
                    CameraControlViewModel viewModel = view.DataContext as CameraControlViewModel;
                    return viewModel;
                }
            }
            catch (Exception ex) { }
            return null;
        }


        /// <summary>
        /// 相机参数修改
        /// </summary>
        /// <param name="obj"></param>
        private void CameraParameterChanged(object obj)
        {
            Sys_bd_camerainformation _Camerainformation = obj as Sys_bd_camerainformation;
            if (_Camerainformation != null)
            {
                using (SqlSugarClient sDB = new SqlAccess().SystemDataAccess)
                {
                    // 1. 查询数据库中的原始记录
                    var original = sDB.Queryable<Sys_bd_camerainformation>()
                                      .Where(x => x.ID == _Camerainformation.ID)
                                      .First();

                    if (original == null) return; // 如果记录不存在则退出

                    // 2. 动态构建更新表达式
                    var update = sDB.Updateable<Sys_bd_camerainformation>();
                    bool isChanged = false;

                    // 检查LinkID是否变化
                    if (original.LinkID != _Camerainformation.LinkID)
                    {
                        update = update.SetColumns(x => x.LinkID == _Camerainformation.LinkID);
                        EventMessage.MessageDisplay($"相机连接ID:{original.LinkID} -> {_Camerainformation.LinkID}", true, true);
                        isChanged = true;
                    }

                    // 检查Exposure是否变化
                    if (original.Exposure != _Camerainformation.Exposure)
                    {
                        update = update.SetColumns(x => x.Exposure == _Camerainformation.Exposure);
                        EventMessage.MessageDisplay($"相机连接曝光:{original.Exposure} -> {_Camerainformation.Exposure}", true, true);
                        isChanged = true;
                    }

                    // 3. 如果有变化则执行更新
                    if (isChanged)
                    {
                        update.Where(x => x.ID == _Camerainformation.ID)
                              .ExecuteCommand();
                    }
                } // 使用using自动释放资源
            }
        }








    }
}
