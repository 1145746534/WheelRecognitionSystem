using HalconDotNet;
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
using System.Windows.Controls;

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
            set
            {
                // 释放旧对象
                SafeHalconDispose(_currentImage1);
                SetProperty<HObject>(ref _currentImage1, value);
            }
        }

        private HObject _wheelContour1;
        /// <summary>
        ///轮毂轮廓1
        /// </summary>
        public HObject WheelContour1
        {
            get { return _wheelContour1; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_wheelContour1);
                SetProperty<HObject>(ref _wheelContour1, value);
            }
        }

        private HObject _templateContour1;
        /// <summary>
        /// 模板轮廓1
        /// </summary>
        public HObject TemplateContour1
        {
            get { return _templateContour1; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_templateContour1);
                SetProperty<HObject>(ref _templateContour1, value);
            }
        }

        private HObject _currentImage2;
        /// <summary>
        /// 当前图像2
        /// </summary>
        public HObject CurrentImage2
        {
            get { return _currentImage2; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_currentImage2);
                SetProperty<HObject>(ref _currentImage2, value);
            }
        }
        private HObject _wheelContour2;
        /// <summary>
        ///轮毂轮廓2
        /// </summary>
        public HObject WheelContour2
        {
            get { return _wheelContour2; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_wheelContour2);
                SetProperty<HObject>(ref _wheelContour2, value);
            }
        }
        private HObject _templateContour2;
        /// <summary>
        /// 模板轮廓2
        /// </summary>
        public HObject TemplateContour2
        {
            get { return _templateContour2; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_templateContour2);
                SetProperty<HObject>(ref _templateContour2, value);
            }
        }

        private HObject _currentImage3;
        /// <summary>
        /// 当前图像3
        /// </summary>
        public HObject CurrentImage3
        {
            get { return _currentImage3; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_currentImage3);
                SetProperty<HObject>(ref _currentImage3, value);
            }
        }
        private HObject _wheelContour3;
        /// <summary>
        ///轮毂轮廓3
        /// </summary>
        public HObject WheelContour3
        {
            get { return _wheelContour3; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_wheelContour3);
                SetProperty<HObject>(ref _wheelContour3, value);
            }
        }
        private HObject _templateContour3;
        /// <summary>
        /// 模板轮廓3
        /// </summary>
        public HObject TemplateContour3
        {
            get { return _templateContour3; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_templateContour3);
                SetProperty<HObject>(ref _templateContour3, value);
            }
        }

        private HObject _currentImage4;
        /// <summary>
        /// 当前图像4
        /// </summary>
        public HObject CurrentImage4
        {
            get { return _currentImage4; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_currentImage4);
                SetProperty<HObject>(ref _currentImage4, value);
            }
        }
        private HObject _wheelContour4;
        /// <summary>
        ///轮毂轮廓4
        /// </summary>
        public HObject WheelContour4
        {
            get { return _wheelContour4; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_wheelContour4);
                SetProperty<HObject>(ref _wheelContour4, value);
            }
        }
        private HObject _templateContour4;
        /// <summary>
        /// 模板轮廓4
        /// </summary>
        public HObject TemplateContour4
        {
            get { return _templateContour4; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_templateContour4);
                SetProperty<HObject>(ref _templateContour4, value);
            }
        }

        private HObject _currentImage5;
        /// <summary>
        /// 当前图像5
        /// </summary>
        public HObject CurrentImage5
        {
            get { return _currentImage5; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_currentImage5);
                SetProperty<HObject>(ref _currentImage5, value);
            }
        }
        private HObject _wheelContour5;
        /// <summary>
        ///轮毂轮廓5
        /// </summary>
        public HObject WheelContour5
        {
            get { return _wheelContour5; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_wheelContour5);
                SetProperty<HObject>(ref _wheelContour5, value);
            }
        }
        private HObject _templateContour5;
        /// <summary>
        /// 模板轮廓5
        /// </summary>
        public HObject TemplateContour5
        {
            get { return _templateContour5; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_templateContour5);
                SetProperty<HObject>(ref _templateContour5, value);
            }
        }
        #endregion

        #region  按钮命令

        /// <summary>
        /// 拍照按钮命令
        /// </summary>
        public DelegateCommand<string> BtnTakePhotoCommand { get; set; }
        public DelegateCommand<string> TestCommand { get; set; }
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
        /// <summary>
        /// 大模型hdict文件路径
        /// </summary>
        private HTuple hv_PreprocessParamFileName = new HTuple();
        /// <summary>
        /// 文件内存
        /// </summary>
        private HTuple hv_DLPreprocessParam = new HTuple();
        /// <summary>
        /// 大模型文件路径
        /// </summary>
        private HTuple hv_RetrainedModelFileName = new HTuple();
        /// <summary>
        /// 模型内存
        /// </summary>
        private HTuple hv_DLModelHandle = new HTuple();


        public DisplayInterfaceViewModel(IDialogService dialogService, IContainerProvider containerProvider)
        {
            //loadAIProcessParam(); //加载AI参数
            _dialogService = dialogService;
            _containerProvider = containerProvider;
            BtnSettingCommand = new DelegateCommand<string>(BtnSetting);
            BtnTakePhotoCommand = new DelegateCommand<string>(BtnTakePhoto);
            TestCommand = new DelegateCommand<string>(Test);
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
        /// 加载模型参数
        /// </summary>
        private void loadAIProcessParam()
        {

            try
            {
                //Halcon路径
                hv_PreprocessParamFileName = "D:/ZS/终检/DLT/model_preprocess_params.hdict";
                HOperatorSet.ReadDict(hv_PreprocessParamFileName, new HTuple(), new HTuple(),
                    out hv_DLPreprocessParam);
                hv_RetrainedModelFileName = "D:/ZS/终检/DLT/model_opt.hdl";
                HOperatorSet.ReadDlModel(hv_RetrainedModelFileName, out hv_DLModelHandle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载AI参数异常：{ex.ToString()}");
            }

        }



        /// <summary>
        /// 轮毂到位显示
        /// </summary>
        /// <param name="obj"></param>
        private void Inplace(KeyValuePair<bool, int> obj)
        {
            string value = obj.Key ? "1" : "0";
            //显示
            switch (obj.Value)
            {
                case 1:
                    Inplace1 = value;
                    break;
                case 2:
                    Inplace2 = value;
                    break;
                case 3:
                    Inplace3 = value;
                    break;
                case 4:
                    Inplace4 = value;
                    break;
                case 5:
                    Inplace5 = value;
                    break;
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
            if (cameras[index] != null && cameras[index].IsConnected)
            {
                AutoRecognitionResultDisplayModel resultDisplayModel = null;
                HObject _image = null;
                HObject gray = null;
                try
                {

                    MyCameraMV camera = cameras[index];
                    interact.IsGrayscale = camera.info.Grayscale;
                    interact.starTime = DateTime.Now;
                    _image = camera.Grabimage();
                    //传统识别用灰度图  大模型识别轮毂用灰度图  显示用原图 大模型识别标签用原图 保存图片用原图
                    gray = RGBTransGray(_image);
                    resultDisplayModel = Tackle(interact, gray);
                    resultDisplayModel.CurrentImage = CloneImageSafely(_image); //显示
                    interact.endTime = DateTime.Now;
                    ResultDisplay(resultDisplayModel);
                    SaveWay way = interact.resultModel.ResultBol ? SaveWay.AutoOK : SaveWay.AutoNG;
                    //保存图片
                    string value = interact.resultModel.WheelStyle == "成品" ? "成" : "半";
                    string _prefixName = $"{interact.resultModel.RecognitionWheelType}_{value}";
                    interact.imagePath = await SaveImageDatasAsync(_image, way, _prefixName);
                }
                catch (Exception ex)
                {
                    //cameras[index].IsConnected = false;
                    interact.resultModel.status = "识别错误" + ex.Message;
                }
                finally
                {
                    SafeHalconDispose(resultDisplayModel);

                    SafeHalconDispose(_image);
                    SafeHalconDispose(gray);

                }
            }
            else
            {
                interact.resultModel.status = "相机未连接";
            }
            //清除到位显示
            Inplace(new KeyValuePair<bool, int>(false, interact.Index));
            //回复消息
            EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Publish(interact);
        }

        /// <summary>
        /// 处理识别图像
        /// </summary>
        /// </summary>
        /// <param name="interact"></param>
        /// <param name="grayImage"></param>
        public AutoRecognitionResultDisplayModel Tackle(InteractS7PLCModel interact, HObject gray)
        {
            HObject grayImage = CloneImageSafely(gray);
            HObject imageRecogn = new HObject();
            RecognitionResultModel recognitionResult = new RecognitionResultModel();
            List<RecognitionResultModel> list = new List<RecognitionResultModel>();
            TemplateManagementViewModel someService = _containerProvider.Resolve<TemplateManagementViewModel>();
            HObject templateContour = new HObject();

            //定位轮毂
            PositioningWheelResultModel pResult = PositioningWheel(grayImage, WheelMinThreshold, 255, WheelMinRadius);

            //存储识别结果                    
            if (pResult.WheelImage != null && pResult.WheelImage.IsInitialized())
                imageRecogn = CloneImageSafely(pResult.WheelImage);
            else
                imageRecogn = CloneImageSafely(grayImage);

            List<TemplatedataModels> models = someService.GetCanUseTemplates();
            recognitionResult = WheelRecognitionAlgorithm(imageRecogn, models, AngleStart, AngleExtent, MinSimilarity, list);
            recognitionResult.FullFigureGary = pResult.FullFigureGary;
            //如果定位到轮毂             
            recognitionResult.InnerCircleGary = pResult.InnerCircleMean;
            if (recognitionResult.RecognitionWheelType != "NG") //
            {

                templateContour = GetAffineTemplateContour(someService.GetHTupleByName(recognitionResult.RecognitionWheelType),
                    recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
                //根据高度确定为哪个轮型

            }
            if (recognitionResult.RecognitionWheelType == "NG" && pResult.WheelImage == null) //识别NG & 没定位到轮毂
            {
                //大模型推算
                HTuple hv_DLResult = WheelDeepLearning(grayImage);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
                if (names.Length > 0 && confidences[0].D > 0.85)
                {
                    string[] name = names[0].S.Split('_'); //00619C70_半
                    string value = name[1] == "半" ? "半成品" : "成品";
                    recognitionResult.RecognitionWheelType = name[0]; //识别结果
                    recognitionResult.WheelStyle = value; //识别样式
                    recognitionResult.Similarity = double.Parse(confidences[0].D.ToString("0.0000"));
                    recognitionResult.status = "识别成功";

                }
                //for (int i = 0; i < names.Length; i++)
                //{
                //    double similar = double.Parse(confidences[i].D.ToString("0.0000"));
                //    Console.WriteLine($"数据：{names[i].S} 结果：{similar}");
                //}
                SafeHalconDispose(hv_DLResult);
                SafeHalconDispose(names);
                SafeHalconDispose(confidences);

            }
            interact.resultModel = recognitionResult;

            //显示需要的参数
            AutoRecognitionResultDisplayModel autoRecognitionResult = new AutoRecognitionResultDisplayModel();
            autoRecognitionResult = new AutoRecognitionResultDisplayModel
            {
                FullFigureGary = recognitionResult.FullFigureGary,
                WheelType = recognitionResult.RecognitionWheelType,

                WheelContour = CloneImageSafely(pResult.WheelContour),
                TemplateContour = CloneImageSafely(templateContour),
                index = interact.Index

            };

            SafeHalconDispose(pResult);
            SafeHalconDispose(imageRecogn);
            SafeHalconDispose(grayImage);
            SafeHalconDispose(templateContour);

            return autoRecognitionResult;
        }

        private async void Test(string obj)
        {
            string[] files = Directory.GetFiles("D:\\ZS\\终检\\训练图\\11");
            //5.处理每个文件
            foreach (string filePath in files)
            {
                try
                {
                    await Task.Delay(1000);

                    HOperatorSet.ReadImage(out HObject image, filePath);
                    HObject gray = RGBTransGray(image);
                    AutoRecognitionResultDisplayModel displayModel = new AutoRecognitionResultDisplayModel();
                    displayModel = Tackle(new InteractS7PLCModel(), gray);

                    displayModel.index = 1;
                    displayModel.CurrentImage = CloneImageSafely(image);

                    ResultDisplay(displayModel);
                    SaveWay way = SaveWay.Hand;
                    //保存图片

                    await SaveImageDatasAsync(image, way, displayModel.WheelType);

                    SafeHalconDispose(displayModel);
                    SafeHalconDispose(gray);
                    SafeHalconDispose(image);






                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  !! 处理文件 {filePath} 时出错: {ex.Message}");
                }

            }
            // 仅用于诊断，不要在生产环境中使用
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
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
                    sDB.Close();
                    sDB.Dispose();
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
                    //ResultDisplay(new AutoRecognitionResultDisplayModel()
                    //{
                    //    CurrentImage = null,
                    //    WheelContour = null,
                    //    TemplateContour = null,
                    //    index = _index + 1
                    //});
                    image = camera.Grabimage();
                    AutoRecognitionResultDisplayModel resultDisplayModel = new AutoRecognitionResultDisplayModel() { CurrentImage = image, index = _index + 1 };
                    ResultDisplay(resultDisplayModel);

                }
                catch (Exception ex)
                {
                    //camera.IsConnected = false;
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
            sDB.Close();
            sDB.Dispose();
            LoadCameraConnStatus();
        }
        /// <summary>
        /// 图像显示
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ResultDisplay(AutoRecognitionResultDisplayModel model)
        {
            if (model == null)
                return;
            string value = "均:" + model.FullFigureGary;

            if (model.index == 1)
            {
                // 使用SafeDispose封装资源释放
                SafeHalconDispose(CurrentImage1);
                SafeHalconDispose(WheelContour1);
                SafeHalconDispose(TemplateContour1);

                CurrentImage1 = CloneImageSafely(model.CurrentImage);
                WheelContour1 = CloneImageSafely(model.WheelContour);
                TemplateContour1 = CloneImageSafely(model.TemplateContour);


            }
            else if (model.index == 2)
            {
                // 使用SafeDispose封装资源释放
                SafeHalconDispose(CurrentImage2);
                SafeHalconDispose(WheelContour2);
                SafeHalconDispose(TemplateContour2);

                CurrentImage2 = CloneImageSafely(model.CurrentImage);
                WheelContour2 = CloneImageSafely(model.WheelContour);
                TemplateContour2 = CloneImageSafely(model.TemplateContour);

                FullGray2 = value;

            }
            else if (model.index == 3)
            {
                // 使用SafeDispose封装资源释放
                SafeHalconDispose(CurrentImage3);
                SafeHalconDispose(WheelContour3);
                SafeHalconDispose(TemplateContour3);

                CurrentImage3 = CloneImageSafely(model.CurrentImage);
                WheelContour3 = CloneImageSafely(model.WheelContour);
                TemplateContour3 = CloneImageSafely(model.TemplateContour);

                FullGray3 = value;

            }
            else if (model.index == 4)
            {
                // 使用SafeDispose封装资源释放
                SafeHalconDispose(CurrentImage4);
                SafeHalconDispose(WheelContour4);
                SafeHalconDispose(TemplateContour4);

                CurrentImage4 = CloneImageSafely(model.CurrentImage);
                WheelContour4 = CloneImageSafely(model.WheelContour);
                TemplateContour4 = CloneImageSafely(model.TemplateContour);

                FullGray4 = value;

            }
            else if (model.index == 5)
            {
                // 使用SafeDispose封装资源释放
                SafeHalconDispose(CurrentImage5);
                SafeHalconDispose(WheelContour5);
                SafeHalconDispose(TemplateContour5);

                CurrentImage5 = CloneImageSafely(model.CurrentImage);
                WheelContour5 = CloneImageSafely(model.WheelContour);
                TemplateContour5 = CloneImageSafely(model.TemplateContour);

                FullGray5 = value;

            }

        }

        // 安全释放方法
        private void SafeHalconDispose<T>(T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null; // 关键：解除引用使GC可回收
            }
        }

        // 安全克隆方法
        private HObject CloneImageSafely(HObject source)
        {
            return (source != null && source.IsInitialized()) ? source.Clone() : null;
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

        /// <summary>
        /// 存储图片
        /// </summary>
        /// <param name="_image"></param>
        /// <param name="way"></param>
        /// <param name="prefixName"></param>
        /// <returns></returns>
        private async Task<string> SaveImageDatasAsync(HObject _image, SaveWay way, string prefixName = null)
        {
            HObject saveImage = CloneImageSafely(_image);

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
                    // 查找下划线的位置
                    int index = prefixName.IndexOf("_", StringComparison.Ordinal);

                    // 如果找到双下划线，返回前面的部分
                    if (index >= 0)
                    {
                        string value = prefixName.Substring(0, index);
                        string finallyName = prefixName.Contains("半") ? "半": "成";
                        value = $"{value}_{finallyName}";
                        saveWheelTypePath = Path.Combine(dayPath, value);

                    }
                    else
                        saveWheelTypePath = Path.Combine(dayPath, prefixName);

                    await Task.Run(() => Directory.CreateDirectory(saveWheelTypePath));
                    savePath = Path.Combine(saveWheelTypePath, $"{prefixName}&{dateTime:yyMMddHHmmss}.tif");
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
                    SafeHalconDispose(saveImage);
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
