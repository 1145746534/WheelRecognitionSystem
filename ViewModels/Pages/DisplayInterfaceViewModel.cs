﻿using HalconDotNet;
using MySqlX.XDevAPI.Common;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Sharp7;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.Views.Dialogs;
using WheelRecognitionSystem.Views.Pages;
using static WheelRecognitionSystem.Public.SystemDatas;
using static WheelRecognitionSystem.Public.ConfigEdit;
using static WheelRecognitionSystem.Public.ExternalConnections;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using Prism.Regions;
using System.Windows.Media.Media3D;
using System.Runtime.InteropServices;
using MvCameraControl;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Prism.Ioc;
using WheelRecognitionSystem.ViewModels.Dialogs;
using static NPOI.HSSF.Util.HSSFColor;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class DisplayInterfaceViewModel : BindableBase, IDisposable
    {

        #region 窗口显示
        public string Title { get; set; } = "主控显示";

        /// <summary>
        ///  显示名称1
        /// </summary>
        public string DisplayName1
        {
            get
            {
                return cameras[0].info.Name;
            }
        }

        /// <summary>
        ///  显示名称2
        /// </summary>
        public string DisplayName2
        {
            get { return cameras[1].info.Name; }
        }
        /// <summary>
        ///  显示名称3
        /// </summary>
        public string DisplayName3
        {
            get { return cameras[2].info.Name; }

        }
        /// <summary>
        ///  显示名称4
        /// </summary>
        public string DisplayName4
        {
            get { return cameras[3].info.Name; }
        }
        /// <summary>
        ///  显示名称5
        /// </summary>
        public string DisplayName5
        {
            get { return cameras[4].info.Name; }
        }
        private string _cameraStatus1;
        /// <summary>
        /// 相机1连接状态
        /// </summary>
        public string CameraStatus1
        {
            get { return _cameraStatus1; }
            set { SetProperty(ref _cameraStatus1, value); }
        }
        private string _cameraStatus2;
        public string CameraStatus2
        {
            get { return _cameraStatus2; }
            set { SetProperty(ref _cameraStatus2, value); }
        }
        private string _cameraStatus3;
        public string CameraStatus3
        {
            get { return _cameraStatus3; }
            set { SetProperty(ref _cameraStatus3, value); }
        }
        private string _cameraStatus4;
        public string CameraStatus4
        {
            get { return _cameraStatus4; }
            set { SetProperty(ref _cameraStatus4, value); }
        }
        private string _cameraStatus5;
        public string CameraStatus5
        {
            get { return _cameraStatus5; }
            set { SetProperty(ref _cameraStatus5, value); }
        }

        private string _inplace1;
        /// <summary>
        /// 轮毂到位信号1
        /// </summary>
        public string Inplace1
        {
            get { return _inplace1; }
            set { SetProperty(ref _inplace1, value); }
        }

        private string _inplace2;
        /// <summary>
        /// 轮毂到位信号2
        /// </summary>
        public string Inplace2
        {
            get { return _inplace2; }
            set { SetProperty(ref _inplace2, value); }
        }

        private string _inplace3;
        /// <summary>
        /// 轮毂到位信号3
        /// </summary>
        public string Inplace3
        {
            get { return _inplace3; }
            set { SetProperty(ref _inplace3, value); }
        }

        private string _inplace4;
        /// <summary>
        /// 轮毂到位信号4
        /// </summary>
        public string Inplace4
        {
            get { return _inplace4; }
            set { SetProperty(ref _inplace4, value); }
        }

        private string _inplace5;
        /// <summary>
        /// 轮毂到位信号5
        /// </summary>
        public string Inplace5
        {
            get { return _inplace5; }
            set { SetProperty(ref _inplace5, value); }
        }

        private string _fullGray1;
        /// <summary>
        /// 图像1平均灰度值
        /// </summary>
        public string FullGray1
        {
            get { return _fullGray1; }
            set { SetProperty(ref _fullGray1, value); }
        }

        private string _fullGray2;
        /// <summary>
        /// 图像2平均灰度值
        /// </summary>
        public string FullGray2
        {
            get { return _fullGray2; }
            set { SetProperty(ref _fullGray2, value); }
        }

        private string _fullGray3;
        /// <summary>
        /// 图像3平均灰度值
        /// </summary>
        public string FullGray3
        {
            get { return _fullGray3; }
            set { SetProperty(ref _fullGray3, value); }
        }

        private string _fullGray4;
        /// <summary>
        /// 图像4平均灰度值
        /// </summary>
        public string FullGray4
        {
            get { return _fullGray4; }
            set { SetProperty(ref _fullGray4, value); }
        }

        private string _fullGray5;
        /// <summary>
        /// 图像5平均灰度值
        /// </summary>
        public string FullGray5
        {
            get { return _fullGray5; }
            set { SetProperty(ref _fullGray5, value); }
        }

        private HObject _currentImage1;
        /// <summary>
        /// 当前图像1
        /// </summary>
        public HObject CurrentImage1
        {
            get { return _currentImage1; }
            set { SetProperty<HObject>(ref _currentImage1, value); }
        }

        private HObject _wheelContour1;
        /// <summary>
        ///轮毂轮廓1
        /// </summary>
        public HObject WheelContour1
        {
            get { return _wheelContour1; }
            set { SetProperty<HObject>(ref _wheelContour1, value); }
        }

        private HObject _templateContour1;
        /// <summary>
        /// 模板轮廓1
        /// </summary>
        public HObject TemplateContour1
        {
            get { return _templateContour1; }
            set { SetProperty<HObject>(ref _templateContour1, value); }
        }

        private HObject _currentImage2;
        /// <summary>
        /// 当前图像2
        /// </summary>
        public HObject CurrentImage2
        {
            get { return _currentImage2; }
            set { SetProperty<HObject>(ref _currentImage2, value); }
        }
        private HObject _wheelContour2;
        /// <summary>
        ///轮毂轮廓2
        /// </summary>
        public HObject WheelContour2
        {
            get { return _wheelContour2; }
            set { SetProperty<HObject>(ref _wheelContour2, value); }
        }
        private HObject _templateContour2;
        /// <summary>
        /// 模板轮廓2
        /// </summary>
        public HObject TemplateContour2
        {
            get { return _templateContour2; }
            set { SetProperty<HObject>(ref _templateContour2, value); }
        }

        private HObject _currentImage3;
        /// <summary>
        /// 当前图像3
        /// </summary>
        public HObject CurrentImage3
        {
            get { return _currentImage3; }
            set { SetProperty<HObject>(ref _currentImage3, value); }
        }
        private HObject _wheelContour3;
        /// <summary>
        ///轮毂轮廓3
        /// </summary>
        public HObject WheelContour3
        {
            get { return _wheelContour3; }
            set { SetProperty<HObject>(ref _wheelContour3, value); }
        }
        private HObject _templateContour3;
        /// <summary>
        /// 模板轮廓3
        /// </summary>
        public HObject TemplateContour3
        {
            get { return _templateContour3; }
            set { SetProperty<HObject>(ref _templateContour3, value); }
        }

        private HObject _currentImage4;
        /// <summary>
        /// 当前图像4
        /// </summary>
        public HObject CurrentImage4
        {
            get { return _currentImage4; }
            set { SetProperty<HObject>(ref _currentImage4, value); }
        }
        private HObject _wheelContour4;
        /// <summary>
        ///轮毂轮廓4
        /// </summary>
        public HObject WheelContour4
        {
            get { return _wheelContour4; }
            set { SetProperty<HObject>(ref _wheelContour4, value); }
        }
        private HObject _templateContour4;
        /// <summary>
        /// 模板轮廓4
        /// </summary>
        public HObject TemplateContour4
        {
            get { return _templateContour4; }
            set { SetProperty<HObject>(ref _templateContour4, value); }
        }

        private HObject _currentImage5;
        /// <summary>
        /// 当前图像5
        /// </summary>
        public HObject CurrentImage5
        {
            get { return _currentImage5; }
            set { SetProperty<HObject>(ref _currentImage5, value); }
        }
        private HObject _wheelContour5;
        /// <summary>
        ///轮毂轮廓5
        /// </summary>
        public HObject WheelContour5
        {
            get { return _wheelContour5; }
            set { SetProperty<HObject>(ref _wheelContour5, value); }
        }
        private HObject _templateContour5;
        /// <summary>
        /// 模板轮廓5
        /// </summary>
        public HObject TemplateContour5
        {
            get { return _templateContour5; }
            set { SetProperty<HObject>(ref _templateContour5, value); }
        }
        #endregion

        #region  按钮命令

        /// <summary>
        /// 拍照按钮命令
        /// </summary>
        public DelegateCommand<string> BtnTakePhotoCommand { get; set; }
        /// <summary>
        /// 设置按钮命令
        /// </summary>
        public DelegateCommand<string> BtnSettingCommand { get; set; }
        /// <summary>
        /// 保存图片按钮命令
        /// </summary>
        public DelegateCommand<string> BtnSaveCommand { get; set; }

        /// <summary>
        /// 模板管理按钮命令
        /// </summary>
        public DelegateCommand<string> BtnTemplateCommand { get; set; }

        #endregion

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private readonly Task _task;

        /// <summary>
        /// 相机列表
        /// </summary>
        public MyCameraMV[] cameras = new MyCameraMV[5];
        /// <summary>
        /// 弹窗服务
        /// </summary>
        readonly IDialogService _dialogService;
        private readonly IContainerProvider _containerProvider;


        public DisplayInterfaceViewModel(IDialogService dialogService, IContainerProvider containerProvider)
        {
            _dialogService = dialogService;
            _containerProvider = containerProvider;
            BtnSettingCommand = new DelegateCommand<string>(BtnSetting);
            BtnTakePhotoCommand = new DelegateCommand<string>(BtnTakePhoto);
            BtnSaveCommand = new DelegateCommand<string>(BtnSave);
            BtnTemplateCommand = new DelegateCommand<string>(BtnTemplate);

            LoadCameraInfo();
            _task = Task.Run(() => MyMethod(cts.Token), cts.Token);

            EventMessage.MessageHelper.GetEvent<AutoRecognitionResultDisplayEvent>().Subscribe(ResultDisplay);
            EventMessage.MessageHelper.GetEvent<InteractHandleEvent>().Subscribe(ReceiveS7PLC);
            EventMessage.MessageHelper.GetEvent<InplaceEvent>().Subscribe(Inplace); //轮毂到位信号显示

            SDKSystem.Initialize();
        }

        /// <summary>
        /// 轮毂到位显示
        /// </summary>
        /// <param name="obj"></param>
        private void Inplace(KeyValuePair<bool, int> obj)
        {
            if (obj.Key)
            {
                //显示
                switch (obj.Value)
                {
                    case 1:
                        Inplace1 = "1";
                        break;
                    case 2:
                        Inplace2 = "1";
                        break;
                    case 3:
                        Inplace3 = "1";
                        break;
                    case 4:
                        Inplace4 = "1";
                        break;
                    case 5:
                        Inplace5 = "1";
                        break;
                }
            }
            else
            {
                switch (obj.Value)
                {
                    case 1:
                        Inplace1 = "0";
                        break;
                    case 2:
                        Inplace2 = "0";
                        break;
                    case 3:
                        Inplace3 = "0";
                        break;
                    case 4:
                        Inplace4 = "0";
                        break;
                    case 5:
                        Inplace5 = "0";
                        break;
                }
            }
        }




        /// <summary>
        /// 接收主界面传过来的PLC数据
        /// </summary>
        /// <param name="interact"></param>
        private void ReceiveS7PLC(InteractS7PLCModel interact)
        {
            if (interact != null)
            {
                //显示
                Inplace(new KeyValuePair<bool, int>(true, interact.Index));
                Thread.Sleep(interact.ArrivalDelay);
                //处理
                PhotoAndTackle(interact);
            }
        }

        /// <summary>
        /// 相机拍照&处理
        /// </summary>
        /// <param name="index"></param>
        public async void PhotoAndTackle(InteractS7PLCModel interact)
        {
            interact.resultModel = new RecognitionResultModel();
            int index = interact.Index - 1;
            HObject image = null;
            if (cameras[index] != null && cameras[index].IsConnected)
            {
                try
                {
                    //清空显示                    
                    ResultDisplay(new AutoRecognitionResultDisplayModel()
                    {
                        CurrentImage = null,
                        WheelContour = null,
                        TemplateContour = null,
                        index = interact.Index
                    });

                    MyCameraMV camera = cameras[index];
                    interact.IsGrayscale = camera.info.Grayscale;
                    interact.starTime = DateTime.Now;
                    image = camera.Grabimage();
                    AutoRecognitionResultDisplayModel resultDisplayModel = Tackle(interact, image);
                    interact.endTime = DateTime.Now;
                    ResultDisplay(resultDisplayModel);
                    SaveWay way = interact.resultModel.ResultBol ? SaveWay.AutoOK : SaveWay.AutoNG;
                    interact.imagePath = await SaveImageDatasAsync(image, way, resultDisplayModel.WheelType);
                }
                catch (Exception ex)
                {
                    cameras[index].IsConnected = false;
                    interact.resultModel.status = ex.Message;
                }
            }
            else
            {
                interact.resultModel.status = "相机未连接";
            }
            //保存图片
            //CameraHelper.SavePic(image, interact);
            //清除到位显示
            Inplace(new KeyValuePair<bool, int>(false, interact.Index));
            //回复消息
            EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Publish(interact);
        }

        /// <summary>
        /// 处理识别-接收主控发送的消息
        /// </summary>
        /// <param name="interact"></param>
        /// <param name="CurrentImage"></param>
        public AutoRecognitionResultDisplayModel Tackle(InteractS7PLCModel interact, HObject CurrentImage)
        {
            HObject image = new HObject();
            //彩色图需转成灰度图
            HOperatorSet.CountChannels(CurrentImage, out HTuple Channels);
            if (Channels.I == 3)
            {
                HOperatorSet.Decompose3(CurrentImage, out HObject image1, out HObject image2, out HObject image3);
                image = image1;
            }
            else
                image = CurrentImage;


            //定位轮毂
            PositioningWheelResultModel pResult = PositioningWheel(image, WheelMinThreshold, 255, WheelMinRadius);
            //存储识别结果
            RecognitionResultModel recognitionResult = new RecognitionResultModel();
            HObject imageRecogn = new HObject();
            //如果定位到轮毂
            if (pResult.WheelImage != null)
            {
                recognitionResult.FullFigureGary = pResult.FullFigureGary;
                recognitionResult.InnerCircleGary = pResult.InnerCircleMean;
                imageRecogn = pResult.WheelImage;
            }
            else
            {
                imageRecogn = image;
            }
            //没有定位到轮毂            
            //recognitionResult = WheelRecognitionAlgorithm(image, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);

            //轮毂识别 传统视觉
            //recognitionResult = WheelRecognitionAlgorithm(imageRecogn, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
            List<RecognitionResultModel> list = new List<RecognitionResultModel>();
            TemplateManagementViewModel someService = _containerProvider.Resolve<TemplateManagementViewModel>();
            List<TemplatedataModels> models = someService.GetCanUseTemplates();
            recognitionResult = WheelRecognitionAlgorithm(imageRecogn, models, AngleStart, AngleExtent, MinSimilarity, list);
            HObject templateContour = new HObject();
            if (recognitionResult.RecognitionWheelType != "NG")
            {

                templateContour = GetAffineTemplateContour(someService.GetHTupleByName(recognitionResult.RecognitionWheelType),
                    recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
                //根据高度确定为哪个轮型

            }
            if (recognitionResult.RecognitionWheelType == "NG" && pResult.WheelImage == null)
            {
                //大模型推算
                //HTuple hv_DLResult = WheelDeepLearning(image);
                HTuple hv_DLResult = WheelDeepLearning(CurrentImage);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
                if (names.Length > 0)
                {
                    recognitionResult.RecognitionWheelType = names[0].S;
                    recognitionResult.Similarity = double.Parse(confidences[0].D.ToString("0.0000"));
                    recognitionResult.status = "识别成功";

                }
                //for (int i = 0; i < names.Length; i++)
                //{
                //    double similar = double.Parse(confidences[i].D.ToString("0.0000"));
                //    Console.WriteLine($"数据：{names[i].S} 结果：{similar}");
                //}
                hv_DLResult.Dispose();
            }
            interact.resultModel = recognitionResult;

            //显示需要的参数
            AutoRecognitionResultDisplayModel autoRecognitionResult = new AutoRecognitionResultDisplayModel();
            autoRecognitionResult = new AutoRecognitionResultDisplayModel
            {
                WheelType = recognitionResult.RecognitionWheelType,
                CurrentImage = CurrentImage,
                WheelContour = pResult.WheelContour,
                TemplateContour = templateContour,
                index = interact.Index

            };
            return autoRecognitionResult;
            //图像结果显示
            //EventMessage.MessageHelper.GetEvent<AutoRecognitionResultDisplayEvent>().Publish(autoRecognitionResult);


        }

        /// <summary>
        /// 相机连接 - 循环扫描
        /// </summary>
        /// <param name="token"></param>
        private void MyMethod(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {

                foreach (MyCameraMV camera in cameras)
                {
                    try
                    {
                        if (camera?.IsConnected == false)
                        {
                            //camera.Connect();
                            //LoadCameraConnStatus();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Thread.Sleep(3000);

            }
        }

        /// <summary>
        /// 实时取像
        /// </summary>
        /// <param name="obj"></param>
        public void RealTime(object obj, MyEventArgs eventArgs)
        {
            MyCameraMV camera = cameras.ToList().Find((x => x.info.Name == obj.ToString()));
            int _index = cameras.ToList().FindIndex((x => x.info.Name == obj.ToString()));
            //启动定时器
            camera._dispatcherTimer = new DispatcherTimer();
            camera._dispatcherTimer.Tag = obj;
            camera._dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            camera._dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);//设置时间间隔为1秒
            camera._dispatcherTimer.Start();//启动定时器

        }
        /// <summary>
        /// 停止实时取像
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventArgs"></param>
        public void StopReal(object obj, MyEventArgs eventArgs)
        {
            MyCameraMV camera = cameras.ToList().Find((x => x.info.Name == obj.ToString()));
            int _index = cameras.ToList().FindIndex((x => x.info.Name == obj.ToString()));
            if (camera._dispatcherTimer != null)
            {
                camera._dispatcherTimer.Stop();
            }

        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnSave(string obj)
        {
            MyCameraMV camera = cameras.ToList().Find((x => x.info.Name == obj));
            int index = cameras.ToList().FindIndex((x => x.info.Name == obj));
            switch (index)
            {
                case 0:
                    //SaveImageDatas(CurrentImage1, SaveWay.Hand);
                    await SaveImageDatasAsync(CurrentImage1, SaveWay.Hand);
                    break;
                case 1:
                    //SaveImageDatas(CurrentImage2, SaveWay.Hand);
                    await SaveImageDatasAsync(CurrentImage2, SaveWay.Hand);
                    break;
                case 2:
                    await SaveImageDatasAsync(CurrentImage3, SaveWay.Hand);
                    break;
                case 3:
                    await SaveImageDatasAsync(CurrentImage4, SaveWay.Hand);
                    break;
                case 4:
                    await SaveImageDatasAsync(CurrentImage5, SaveWay.Hand);
                    break;
            }

        }
        /// <summary>
        /// 模板设置
        /// </summary>
        /// <param name="obj"></param>
        private void BtnTemplate(string obj)
        {



            ServletInfoModel model = new ServletInfoModel();
            model.Path = "TemplateManagementView";
            int index = cameras.ToList().FindIndex((x => x.info.Name == obj));
            model.camera = cameras[index];
            switch (index)
            {
                case 0:
                    model.DisplayName = DisplayName1;
                    model.image = CurrentImage1;
                    break;
                case 1:
                    model.DisplayName = DisplayName2;
                    model.image = CurrentImage2;
                    break;
                case 2:
                    model.DisplayName = DisplayName3;
                    model.image = CurrentImage3;
                    break;
                case 3:
                    model.DisplayName = DisplayName4;
                    model.image = CurrentImage4;
                    break;
                case 4:
                    model.DisplayName = DisplayName5;
                    model.image = CurrentImage5;
                    break;
            }
            EventMessage.MessageHelper.GetEvent<ServletInfoEvent>().Publish(model);
        }
        /// <summary>
        /// 设置相机参数
        /// </summary>
        /// <param name="obj"></param>
        private void BtnSetting(string obj)
        {
            DialogParameters dialogParameters = new DialogParameters();
            string newLinkID = string.Empty;
            int newExposure = 0;

            MyCameraMV camera = cameras.ToList().Find((x => x.info.Name == obj));
            int index = cameras.ToList().FindIndex((x => x.info.Name == obj));
            if (camera != null)
            {
                dialogParameters.Add("Name", camera.info.Name);
                dialogParameters.Add("LinkID", camera.info.LinkID);
                dialogParameters.Add("Exposure", camera.info.Exposure);
            }

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

                    //发布修改项用于模板数据显示控件滚动到修改项
                    //EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
            }));

            if (camera != null)
            {
                try
                {
                    //连接相机
                    if (!string.IsNullOrEmpty(newLinkID) && camera.info.LinkID != newLinkID)
                    {
                        camera.info.LinkID = newLinkID;
                        camera.Disconnect();
                        bool isSuc = camera.Connect(newLinkID);
                        LoadCameraConnStatus();
                        if (isSuc)
                            EventMessage.MessageDisplay("相机连接成功！", true, false);
                    }

                    //设置曝光时间
                    if (camera.IsConnected && newExposure != 0 && camera.info.Exposure != newExposure)
                    {

                        camera.info.Exposure = newExposure;
                        camera.SetExposureTime((float)newExposure);
                        EventMessage.MessageDisplay("曝光设置成功！", true, false);

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接相机:" + ex.Message);
                }
                finally
                {
                    cameras[index] = camera;
                    //数据库更新
                    SqlSugarClient sDB = new SqlAccess().SystemDataAccess;
                    var result = sDB.Updateable<Sys_bd_camerainformation>()
                    .SetColumns(it => new Sys_bd_camerainformation()
                    {
                        Exposure = newExposure,
                        LinkID = newLinkID
                    }).Where(it => it.Name == camera.info.Name).ExecuteCommand();
                    EventMessage.MessageDisplay("参数保存成功！", true, false);
                }



            }
        }


        /// <summary>
        /// 单帧拍照
        /// </summary>
        /// <param name="obj">相机名</param>
        private void BtnTakePhoto(string obj)
        {
            MyCameraMV camera = cameras.ToList().Find((x => x.info.Name == obj));
            int _index = cameras.ToList().FindIndex((x => x.info.Name == obj));
            HObject image = null;
            if (camera != null && camera.IsConnected)
            {
                try
                {
                    //清空显示                    
                    ResultDisplay(new AutoRecognitionResultDisplayModel()
                    {
                        CurrentImage = null,
                        WheelContour = null,
                        TemplateContour = null,
                        index = _index + 1
                    });
                    image = camera.Grabimage();
                    ResultDisplay(new AutoRecognitionResultDisplayModel() { CurrentImage = image, index = _index + 1 });
                }
                catch (Exception ex)
                {
                    camera.IsConnected = false;
                }
            }
        }



        /// <summary>
        /// 加载相机信息
        /// </summary>
        public void LoadCameraInfo()
        {
            SqlSugarClient sDB = new SqlAccess().SystemDataAccess;
            List<Sys_bd_camerainformation> DatasCamera = sDB.Queryable<Sys_bd_camerainformation>().OrderBy(o => o.ID).ToList();
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i] = new MyCameraMV();
                cameras[i].info = new Sys_bd_camerainformation();
                if (DatasCamera.Count > i)
                {
                    cameras[i].info = DatasCamera[i];
                    Thread.Sleep(10);
                    bool isSuc = cameras[i].Connect(DatasCamera[i].LinkID);
                    Console.WriteLine($"LoadCameraInfo {i} {DatasCamera[i].LinkID} {isSuc}");

                }

            }
            LoadCameraConnStatus();
        }
        /// <summary>
        /// 图像显示
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ResultDisplay(AutoRecognitionResultDisplayModel model)
        {
            string value = string.Empty;
            //计算图像灰度值
            if (model.CurrentImage != null && model.CurrentImage.IsInitialized())
            {
                HObject imageM = new HObject();
                imageM = model.CurrentImage.Clone();
                HOperatorSet.CountChannels(imageM, out HTuple Channels);
                if (Channels.I == 3)
                {
                    HOperatorSet.Decompose3(imageM, out HObject red, out HObject green, out HObject blue);
                    imageM = red;
                }
                HOperatorSet.Intensity(imageM, imageM, out HTuple Mean1, out HTuple Deviation);
                value = "均:" + (int)Mean1.D;
                imageM.Dispose();
            }
            if (model.index == 1)
            {
                CurrentImage1?.Dispose();
                WheelContour1?.Dispose();
                TemplateContour1?.Dispose();
                if (model.CurrentImage != null)
                    CurrentImage1 = model.CurrentImage;

                if (model.WheelContour != null)
                    WheelContour1 = model.WheelContour;

                if (model.TemplateContour != null)
                    TemplateContour1 = model.TemplateContour;

                FullGray1 = value;
            }
            else if (model.index == 2)
            {
                CurrentImage2?.Dispose();
                WheelContour2?.Dispose();
                TemplateContour2?.Dispose();
                if (model.CurrentImage != null)
                    CurrentImage2 = model.CurrentImage;

                if (model.WheelContour != null)
                    WheelContour2 = model.WheelContour;

                if (model.TemplateContour != null)
                    TemplateContour2 = model.TemplateContour;

                FullGray2 = value;

            }
            else if (model.index == 3)
            {
                CurrentImage3?.Dispose();
                WheelContour3?.Dispose();
                TemplateContour3?.Dispose();
                if (model.CurrentImage != null)
                    CurrentImage3 = model.CurrentImage;

                if (model.WheelContour != null)
                    WheelContour3 = model.WheelContour;

                if (model.TemplateContour != null)
                    TemplateContour3 = model.TemplateContour;

                FullGray3 = value;

            }
            else if (model.index == 4)
            {
                CurrentImage4?.Dispose();
                WheelContour4?.Dispose();
                TemplateContour4?.Dispose();
                if (model.CurrentImage != null)
                    CurrentImage4 = model.CurrentImage;

                if (model.WheelContour != null)
                    WheelContour4 = model.WheelContour;

                if (model.TemplateContour != null)
                    TemplateContour4 = model.TemplateContour;

                FullGray4 = value;

            }
            else if (model.index == 5)
            {
                CurrentImage5?.Dispose();
                WheelContour5?.Dispose();
                TemplateContour5?.Dispose();
                if (model.CurrentImage != null)
                    CurrentImage5 = model.CurrentImage;

                if (model.WheelContour != null)
                    WheelContour5 = model.WheelContour;

                if (model.TemplateContour != null)
                    TemplateContour5 = model.TemplateContour;

                FullGray5 = value;

            }

        }

        //计时器时间(从相机中采图并显示)
        private void dispatcherTimer_Tick(object sender, EventArgs e)//计时执行的程序
        {
            //Console.WriteLine("----------");
            DispatcherTimer dispatcher = sender as DispatcherTimer;
            string name = dispatcher.Tag.ToString();
            BtnTakePhoto(name);
            //ho_Image.Dispose();
            //HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);//获取图片

            //HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);//获取图片的宽和高


            //if (HDevWindowStack.IsOpen())
            //{
            //    HOperatorSet.SetPart(HDevWindowStack.GetActive(), 0, 0, hv_Height, hv_Width);//设置显示的范围
            //    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());//显示图片
            //}

        }
        /// <summary>
        /// 加载相机连接状态
        /// </summary>
        public void LoadCameraConnStatus()
        {
            CameraStatus1 = cameras[0].IsConnected ? "1" : "0";
            CameraStatus2 = cameras[1].IsConnected ? "1" : "0";
            CameraStatus3 = cameras[2].IsConnected ? "1" : "0";
            CameraStatus4 = cameras[3].IsConnected ? "1" : "0";
            CameraStatus5 = cameras[4].IsConnected ? "1" : "0";
        }


        private string SaveImageDatas(HObject saveImage, SaveWay way, string wheelType = null)
        {
            string savePath = string.Empty;
            DateTime dateTime = DateTime.Now;
            //月文件夹路径
            string monthPath = HistoricalImagesPath + @"\" + dateTime.Month + "月";
            //日文件夹路径
            string dayPath = HistoricalImagesPath + @"\" + dateTime.Month + @"月\" + dateTime.Day + "日";
            //当日未识别文件夹路径
            string ngPath = HistoricalImagesPath + @"\" + dateTime.Month + @"月\" + dateTime.Day + @"日\NG";
            if (Directory.Exists(monthPath) == false)
                Directory.CreateDirectory(monthPath);
            if (Directory.Exists(dayPath) == false)
                Directory.CreateDirectory(dayPath);
            if (Directory.Exists(ngPath) == false)
                Directory.CreateDirectory(ngPath);
            if (Directory.Exists(HandImagesPath) == false)
                Directory.CreateDirectory(HandImagesPath);
            var diskFree = GetHardDiskFreeSpace("D");//获取D盘剩余空间
            if (diskFree > 200)
            {
                if (way == SaveWay.AutoOK)
                {
                    //保存轮型的目录
                    string saveWheelTypePath = dayPath + @"\" + wheelType;
                    if (Directory.Exists(saveWheelTypePath) == false)
                        Directory.CreateDirectory(saveWheelTypePath);
                    savePath = saveWheelTypePath + @"\" + wheelType + "&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    string saveImagePath = savePath.Replace(@"\", "/");
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
                }
                else if (way == SaveWay.AutoNG)
                {
                    savePath = ngPath + @"/NG" + "&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    string saveImagePath = savePath.Replace(@"\", "/");
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
                }
                else
                {
                    //保存到临时图片列表
                    savePath = HandImagesPath + @"/" + "Hand&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    string saveImagePath = savePath.Replace(@"\", "/");
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);

                }

            }
            else
                EventMessage.MessageDisplay("磁盘存储空间不足，请检查！", true, false);

            return savePath;
        }

        private async Task<string> SaveImageDatasAsync(HObject saveImage, SaveWay way, string wheelType = null)
        {
            string savePath = string.Empty;
            DateTime dateTime = DateTime.Now;

            // 路径定义
            string monthPath = Path.Combine(HistoricalImagesPath, $"{dateTime.Month}月");
            string dayPath = Path.Combine(monthPath, $"{dateTime.Day}日");
            string ngPath = Path.Combine(dayPath, "NG");
            string handImagesPath = HandImagesPath;

            try
            {
                // 异步创建目录（优化多次IO操作）
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(monthPath); // CreateDirectory 自动处理已存在的情况
                    Directory.CreateDirectory(dayPath);
                    Directory.CreateDirectory(ngPath);
                    Directory.CreateDirectory(handImagesPath);
                });

                // 异步获取磁盘空间
                double diskFree = await Task.Run(() => GetHardDiskFreeSpace("D"));

                if (diskFree <= 200)
                {
                    // UI 线程安全的消息显示
                    Application.Current.Dispatcher.Invoke(() =>
                        EventMessage.MessageDisplay("磁盘存储空间不足，请检查！", true, false));
                    return savePath;
                }

                // 根据保存方式构建路径
                string saveWheelTypePath = "";
                if (way == SaveWay.AutoOK)
                {
                    saveWheelTypePath = Path.Combine(dayPath, wheelType);
                    await Task.Run(() => Directory.CreateDirectory(saveWheelTypePath));
                    savePath = Path.Combine(saveWheelTypePath, $"{wheelType}&{dateTime:yyMMddHHmmss}.tif");
                }
                else if (way == SaveWay.AutoNG)
                {
                    savePath = Path.Combine(ngPath, $"NG&{dateTime:yyMMddHHmmss}.tif");
                }
                else
                {
                    savePath = Path.Combine(handImagesPath, $"Hand&{dateTime:yyMMddHHmmss}.tif");
                }
                string saveImagePath = string.Empty;
                // 异步保存图像
                await Task.Run(() =>
                {
                    saveImagePath = savePath.Replace(@"\", "/");
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
                });
                if (way == SaveWay.Hand)
                    Application.Current.Dispatcher.Invoke(() =>
                        EventMessage.MessageDisplay($"图片保存成功：{saveImagePath}", true, false));

                return savePath;
            }
            catch (Exception ex)
            {
                // 异常处理（可根据需要记录日志）
                Application.Current.Dispatcher.Invoke(() =>
                    EventMessage.MessageDisplay($"保存失败: {ex.Message}", true, false));
                return string.Empty;
            }
        }
        public enum SaveWay
        {
            [Description("自动OK图")]
            AutoOK,
            [Description("自动NG图")]
            AutoNG,
            [Description("手动")]
            Hand
        }

        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为MB) 
        ///  </summary> 
        ///  <param name="HardDiskName">代表驱动器的字母(必须大写字母) </param> 
        ///  <returns> </returns> 
        private long GetHardDiskFreeSpace(string HardDiskName)
        {
            long freeSpace = new long();
            HardDiskName = HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024);
                }
            }
            return freeSpace;
        }


        public void Dispose()
        {
            SDKSystem.Finalize();
        }
    }
}
