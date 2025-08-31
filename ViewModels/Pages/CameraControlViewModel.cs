using HalconDotNet;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using static WheelRecognitionSystem.Public.SystemDatas;
using WheelRecognitionSystem.Models;
using SqlSugar;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Media3D;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models.IModels;
using System.Windows.Threading;
using MvCamCtrl.NET;
using Prism.Services.Dialogs;
using WheelRecognitionSystem.Public;
using System.Windows;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class CameraControlViewModel : BindableBase
    {
        #region 属性
        private bool _cameraStatus;
        /// <summary>
        /// 相机连接状态
        /// </summary>
        public bool CameraStatus
        {
            get { return _cameraStatus; }
            set { SetProperty(ref _cameraStatus, value); }
        }

        private bool _isIndeterminate;
        //显示流动状态
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set { SetProperty(ref _isIndeterminate, value); }
        }

        //public bool IsIndeterminate => !CameraStatus;

        private string _displayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }

        private HObject _currentImage;
        /// <summary>
        /// 当前图像
        /// </summary>
        public HObject CurrentImage
        {
            get { return _currentImage; }
            set
            {
                // 释放旧对象
                SafeDisposeHObject(ref _currentImage);
                SetProperty<HObject>(ref _currentImage, value);
            }
        }
        private HObject _wheelContour;
        /// <summary>
        ///轮毂轮廓
        /// </summary>
        public HObject WheelContour
        {
            get { return _wheelContour; }
            set
            {
                // 释放旧对象
                SafeDisposeHObject(ref _wheelContour);
                SetProperty<HObject>(ref _wheelContour, value);
            }
        }
        private HObject _templateContour;
        /// <summary>
        /// 模板轮廓
        /// </summary>
        public HObject TemplateContour
        {
            get { return _templateContour; }
            set
            {
                // 释放旧对象
                SafeDisposeHObject(ref _templateContour);
                SetProperty<HObject>(ref _templateContour, value);
            }
        }
        private string _fullGray;
        /// <summary>
        /// 图像平均灰度值
        /// </summary>
        public string FullGray
        {
            get { return _fullGray; }
            set { SetProperty(ref _fullGray, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 开启
        /// </summary>
        public ICommand SwitchOnCommand { get; }
        /// <summary>
        /// 关闭
        /// </summary>
        public ICommand SwitchOffCommand { get; }
        //拍照
        public ICommand BtnTakePhotoCommand { get; }
        //连接
        public ICommand BtnLinkCommand { get; }
        public ICommand BtnSaveCommand { get; }

        #endregion

        /// <summary>
        /// 相机
        /// </summary>
        public IMyCamera myCamera;

        /// <summary>
        /// 数据源
        /// </summary>
        private Sys_bd_camerainformation camerainformation;

        /// <summary>
        /// 相机连接状态定时扫描
        /// </summary>
        public DispatcherTimer LinkTimer;

        /// <summary>
        /// 用于实时取图
        /// </summary>
        public DispatcherTimer RealTimer;


        /// <summary>
        /// 弹窗服务
        /// </summary>
        readonly IDialogService _dialogService;



        public CameraControlViewModel(IDialogService dialogService)
        {
            SwitchOnCommand = new DelegateCommand<object>(SwitchOn);
            SwitchOffCommand = new DelegateCommand<object>(SwitchOff);
            BtnTakePhotoCommand = new DelegateCommand<object>(BtnTakePhoto);
            BtnLinkCommand = new DelegateCommand<object>(BtnLink);
            BtnSaveCommand = new DelegateCommand<object>(BtnSave);

            _dialogService = dialogService;

            LinkTimer = new DispatcherTimer();
            LinkTimer.Tick += new EventHandler(LinkTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            LinkTimer.Interval = new TimeSpan(0, 0, 0, 30, 0);//设置时间间隔为30秒

            RealTimer = new DispatcherTimer();
            RealTimer.Tick += new EventHandler(RealTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            RealTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);//设置时间间隔为0.1秒

        }



        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(Sys_bd_camerainformation _camerainformation)
        {
            camerainformation = _camerainformation.SafeClone();
            DisplayName = camerainformation.Name;

            myCamera = new MyCameraMV();

            CameraStatus = CameraConnect();
            IsIndeterminate = !CameraStatus;
            CameraExposureTime();
            LinkTimer.Start();//启动定时器



        }
        /// <summary>
        /// 相机循环扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkTimer_Tick(object sender, EventArgs e)
        {
            CameraStatus = CameraConnect();
            IsIndeterminate = !CameraStatus;
        }

        /// <summary>
        /// 检查是否初始化
        /// </summary>
        /// <returns></returns>
        public bool CheckInitialize()
        {
            if (myCamera != null && camerainformation != null)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 连接相机
        /// </summary>
        private bool CameraConnect()
        {
            if (CheckInitialize())
            {
                return myCamera.Connect(camerainformation.LinkID);
            }
            else
                return false;
        }
        /// <summary>
        /// 设置相机曝光时间
        /// </summary>
        private void CameraExposureTime()
        {
            if (CheckInitialize())
            {
                myCamera.SetExposureTime(camerainformation.Exposure);
            }
        }

        #region 功能
        /// <summary>
        /// 单帧拍照
        /// </summary>
        /// <param name="obj"></param>
        private void BtnTakePhoto(object obj)
        {
            if (!CheckInitialize())
                return;
            HObject image = null;
            try
            {
                image = GetImage();
                if (image != null)
                {
                    AutoRecognitionResultDisplayModel resultDisplayModel = new AutoRecognitionResultDisplayModel()
                    {
                        CurrentImage = image.Clone()
                    };
                    ResultDisplay(resultDisplayModel);

                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                SafeDisposeHObject(ref image);
            }

        }


        public HObject GetImage()
        {
            if (myCamera != null)
            {
                return myCamera.Grabimage();
            }
            else
                return null;
        }
        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="obj"></param>
        private void BtnLink(object obj)
        {
            if (camerainformation == null)
                return;
            //打开相机编辑窗口
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("Name", camerainformation.Name);
            dialogParameters.Add("LinkID", camerainformation.LinkID);
            dialogParameters.Add("Exposure", camerainformation.Exposure);

            string newLinkID = string.Empty;
            int newExposure = 0;
            bool IsRefreshCam = false;
            bool IsRefreshExp = false;

            _dialogService.ShowDialog("CameraSet", dialogParameters, new Action<IDialogResult>((IDialogResult callback) =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    //回调中对应传参方法的接口 Parameters 
                    if (callback.Parameters.ContainsKey("LinkID"))
                    {
                        //获取接口中的参数
                        newLinkID = callback.Parameters.GetValue<string>("LinkID");
                    }
                    if (callback.Parameters.ContainsKey("Exposure"))
                    {
                        newExposure = callback.Parameters.GetValue<int>("Exposure");
                    }
                }
            }));

            //如果修改了参数值
            if (!string.IsNullOrEmpty(newLinkID) && newLinkID != camerainformation.LinkID)
            {
                camerainformation.LinkID = newLinkID;
                IsRefreshCam = true;
            }
            if (newExposure != 0 && newExposure != camerainformation.Exposure)
            {
                camerainformation.Exposure = newExposure;
                IsRefreshExp = true;
            }
            if (IsRefreshCam || IsRefreshExp)
            {
                //1.通知父级模型保存数据
                EventMessage.MessageHelper.GetEvent<CameraParameterChangedEvent>().Publish(camerainformation.SafeClone());
            }
            //2.修改相机参数
            if (IsRefreshCam)
            {
                myCamera.Disconnect(); //断开原来的连接
                myCamera.Connect(newLinkID); //连接
                if (IsRefreshExp)
                {
                    //修改曝光时间
                    myCamera.SetExposureTime(newExposure);
                }
            }
            //只修改了曝光时间
            if (IsRefreshExp)
            {
                myCamera.SetExposureTime(newExposure);
            }

        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="obj"></param>
        private void BtnSave(object obj)
        {
            if (!CheckInitialize())
                return;
            try
            {
                string savePath = GetImageSavePath(SaveWay.Hand, HandImagesPath);
                SaveImageDatasAsync(CurrentImage, savePath);
                Application.Current.Dispatcher.Invoke(() =>
                       EventMessage.MessageDisplay($"图片保存成功：{savePath}", true, false));
            }
            catch (Exception ex)
            {
                // 异常处理（可根据需要记录日志）
                Application.Current.Dispatcher.Invoke(() =>
                   EventMessage.MessageDisplay($"保存失败: {ex.Message}", true, false));
            }


        }

        /// <summary>
        /// 关闭实时
        /// </summary>
        /// <param name="obj"></param>
        private void SwitchOff(object obj)
        {
            if (!CheckInitialize())
                return;
            RealTimer.Stop();//启动定时器
            Console.WriteLine($"关闭实时");
        }

        /// <summary>
        /// 开启实时
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SwitchOn(object obj)
        {
            if (!CheckInitialize())
                return;

            RealTimer.Start();//启动定时器
        }

        private void RealTimer_Tick(object sender, EventArgs e)
        {
            BtnTakePhoto(null);
        }

        #endregion

        public void ResultDisplay(AutoRecognitionResultDisplayModel model)
        {
            if (model == null)
                return;
            string value = "均:" + model.FullFigureGary.ToString("0.0");
            SafeDisposeHObject(ref _currentImage);
            SafeDisposeHObject(ref _wheelContour);
            SafeDisposeHObject(ref _templateContour);

            CurrentImage = CloneImageSafely(model.CurrentImage);
            //WheelContour = CloneImageSafely(model.WheelContour);
            TemplateContour = CloneImageSafely(model.TemplateContour);
            //轮毂资源可以随时关闭，原图可能需要手动保存
            SafeDisposeHObject(ref _wheelContour);
            SafeDisposeHObject(ref _templateContour);

            FullGray = value;
            model.Dispose(); //销毁图像
        }

    }

}
