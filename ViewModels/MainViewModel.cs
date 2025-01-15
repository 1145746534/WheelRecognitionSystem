using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Public;
using HalconDotNet;
using System.IO;
using Sharp7;
using WheelRecognitionSystem.Models;
using static WheelRecognitionSystem.Public.SystemDatas;
using static WheelRecognitionSystem.Public.ConfigEdit;
using static WheelRecognitionSystem.Public.ExternalConnections;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using WheelRecognitionSystem.Views.Dialogs;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using SqlSugar;
using Prism.Ioc;
using WheelRecognitionSystem.Views.Pages;
using System.Windows.Shapes;
using NPOI.SS.Formula.Functions;
using System.Threading;
using System.Reflection;

namespace WheelRecognitionSystem.ViewModels
{
    public class MainViewModel : BindableBase
    {

        #region 识别相关属性定义
        private string _recognitionStatus1;
        /// <summary>
        /// 自动模式下识别状态1
        /// </summary>
        public string RecognitionStatus1
        {
            get { return _recognitionStatus1; }
            set { SetProperty(ref _recognitionStatus1, value); }
        }
        private string _recognitionWheelType1 = "";
        /// <summary>
        /// 识别轮型1
        /// </summary>
        public string RecognitionWheelType1
        {
            get { return _recognitionWheelType1; }
            set { SetProperty(ref _recognitionWheelType1, value); }
        }
        private string _similarity1 = "";
        /// <summary>
        /// 相似度1
        /// </summary>
        public string Similarity1
        {
            get { return _similarity1; }
            set { SetProperty(ref _similarity1, value); }
        }
        private string _timeConsumed1 = "";
        /// <summary>
        /// 用时1
        /// </summary>
        public string TimeConsumed1
        {
            get { return _timeConsumed1; }
            set { SetProperty(ref _timeConsumed1, value); }
        }
        private string _colour1 = "";
        /// <summary>
        /// 颜色1
        /// </summary>
        public string Colour1
        {
            get { return _colour1; }
            set { SetProperty(ref _colour1, value); }
        }

        private string _recognitionStatus2;
        /// <summary>
        /// 自动模式下识别状态2
        /// </summary>
        public string RecognitionStatus2
        {
            get { return _recognitionStatus2; }
            set { SetProperty(ref _recognitionStatus2, value); }
        }
        private string _recognitionWheelType2 = "";
        /// <summary>
        /// 识别轮型2
        /// </summary>
        public string RecognitionWheelType2
        {
            get { return _recognitionWheelType2; }
            set { SetProperty(ref _recognitionWheelType2, value); }
        }
        private string _similarity2 = "";
        /// <summary>
        /// 相似度2
        /// </summary>
        public string Similarity2
        {
            get { return _similarity2; }
            set { SetProperty(ref _similarity2, value); }
        }
        private string _timeConsumed2 = "";
        /// <summary>
        /// 用时2
        /// </summary>
        public string TimeConsumed2
        {
            get { return _timeConsumed2; }
            set { SetProperty(ref _timeConsumed2, value); }
        }
        private string _colour2 = "";
        /// <summary>
        /// 颜色2
        /// </summary>
        public string Colour2
        {
            get { return _colour2; }
            set { SetProperty(ref _colour2, value); }
        }

        private string _recognitionStatus3;
        /// <summary>
        /// 自动模式下识别状态3
        /// </summary>
        public string RecognitionStatus3
        {
            get { return _recognitionStatus3; }
            set { SetProperty(ref _recognitionStatus3, value); }
        }
        private string _recognitionWheelType3 = "";
        /// <summary>
        /// 识别轮型3
        /// </summary>
        public string RecognitionWheelType3
        {
            get { return _recognitionWheelType3; }
            set { SetProperty(ref _recognitionWheelType3, value); }
        }
        private string _similarity3 = "";
        /// <summary>
        /// 相似度3
        /// </summary>
        public string Similarity3
        {
            get { return _similarity3; }
            set { SetProperty(ref _similarity3, value); }
        }
        private string _timeConsumed3 = "";
        /// <summary>
        /// 用时3
        /// </summary>
        public string TimeConsumed3
        {
            get { return _timeConsumed3; }
            set { SetProperty(ref _timeConsumed3, value); }
        }
        private string _colour3 = "";
        /// <summary>
        /// 颜色3
        /// </summary>
        public string Colour3
        {
            get { return _colour3; }
            set { SetProperty(ref _colour3, value); }
        }

        private string _recognitionStatus4;
        /// <summary>
        /// 自动模式下识别状态4
        /// </summary>
        public string RecognitionStatus4
        {
            get { return _recognitionStatus4; }
            set { SetProperty(ref _recognitionStatus4, value); }
        }
        private string _recognitionWheelType4 = "";
        /// <summary>
        /// 识别轮型4
        /// </summary>
        public string RecognitionWheelType4
        {
            get { return _recognitionWheelType4; }
            set { SetProperty(ref _recognitionWheelType4, value); }
        }
        private string _similarity4 = "";
        /// <summary>
        /// 相似度4
        /// </summary>
        public string Similarity4
        {
            get { return _similarity4; }
            set { SetProperty(ref _similarity4, value); }
        }
        private string _timeConsumed4 = "";
        /// <summary>
        /// 用时4
        /// </summary>
        public string TimeConsumed4
        {
            get { return _timeConsumed4; }
            set { SetProperty(ref _timeConsumed4, value); }
        }
        private string _colour4 = "";
        /// <summary>
        /// 颜色4
        /// </summary>
        public string Colour4
        {
            get { return _colour4; }
            set { SetProperty(ref _colour4, value); }
        }

        private string _recognitionStatus5;
        /// <summary>
        /// 自动模式下识别状态5
        /// </summary>
        public string RecognitionStatus5
        {
            get { return _recognitionStatus5; }
            set { SetProperty(ref _recognitionStatus5, value); }
        }
        private string _recognitionWheelType5 = "";
        /// <summary>
        /// 识别轮型5
        /// </summary>
        public string RecognitionWheelType5
        {
            get { return _recognitionWheelType5; }
            set { SetProperty(ref _recognitionWheelType5, value); }
        }
        private string _similarity5 = "";
        /// <summary>
        /// 相似度5
        /// </summary>
        public string Similarity5
        {
            get { return _similarity5; }
            set { SetProperty(ref _similarity5, value); }
        }
        private string _timeConsumed5 = "";
        /// <summary>
        /// 用时5
        /// </summary>
        public string TimeConsumed5
        {
            get { return _timeConsumed5; }
            set { SetProperty(ref _timeConsumed5, value); }
        }
        private string _colour5 = "";
        /// <summary>
        /// 颜色5
        /// </summary>
        public string Colour5
        {
            get { return _colour5; }
            set { SetProperty(ref _colour5, value); }
        }

        #endregion

        #region======通知属性定义======
        private string _plcStatus = "2";
        /// <summary>
        /// PLC连接状态 0:失败 1：成功 2：未知
        /// </summary>
        public string PlcStatus
        {
            get { return _plcStatus; }
            set
            {
                if (value != PlcStatus)
                {
                    SetProperty(ref _plcStatus, value);
                    if (value == "1")
                    {
                        EventMessage.MessageDisplay("PLC连接成功！", true, false);
                    }
                    else if (value == "0")
                    {
                        EventMessage.MessageDisplay("PLC连接失败！", true, false);
                    }
                }
            }
        }

        private string _cameraStatus = "2";
        /// <summary>
        /// 相机连接状态 0:失败 1：成功 2：未知
        /// </summary>
        public string CameraStatus
        {
            get { return _cameraStatus; }
            set
            {
                SetProperty(ref _cameraStatus, value);
                if (value == "1")
                {
                    EventMessage.MessageDisplay("相机连接成功！", true, false);
                    S7.SetBitAt(ref WriteBuffer, 12, 3, false);
                }
                else if (value == "0")
                {
                    EventMessage.MessageDisplay("相机连接失败！", true, false);
                    S7.SetBitAt(ref WriteBuffer, 12, 3, true);
                }
            }
        }

        private string _signalInPlace = "2";
        /// <summary>
        /// 轮毂到位信号  0:位空 1：到位 2：未知
        /// </summary>
        public string SignalInPlace
        {
            get { return _signalInPlace; }
            set { SetProperty(ref _signalInPlace, value); }
        }



        private string _screenedResultDisplay;
        /// <summary>
        /// 分选结果显示
        /// </summary>
        public string ScreenedResultDisplay
        {
            get { return _screenedResultDisplay; }
            set { SetProperty(ref _screenedResultDisplay, value); }
        }

        private string _systemModelContent = "手动模式";
        /// <summary>
        /// 系统模式切换按钮显示文本
        /// </summary>
        public string SystemModelContent
        {
            get { return _systemModelContent; }
            set
            {
                SetProperty(ref _systemModelContent, value);
                if (value == "手动模式") SystemModeButtonForeground = "#FF363644";
                else SystemModeButtonForeground = "#FF5DDC4C";
            }
        }

        private string _systemModeButtonForeground = "#FF363644";
        /// <summary>
        /// 系统模式按钮字体颜色
        /// </summary>
        public string SystemModeButtonForeground
        {
            get { return _systemModeButtonForeground; }
            set { SetProperty(ref _systemModeButtonForeground, value); }
        }



        private string _gateResult;
        /// <summary>
        /// 浇口检测结果
        /// </summary>
        public string GateResult
        {
            get { return _gateResult; }
            set { SetProperty(ref _gateResult, value); }
        }



        private int _currentNgNumber;
        /// <summary>
        /// 当前NG次数
        /// </summary>
        public int CurrentNgNumber
        {
            get { return _currentNgNumber; }
            set { SetProperty(ref _currentNgNumber, value); }
        }

        private int _recognitionPauseSetting;
        /// <summary>
        /// 识别暂停设置的次数
        /// </summary>
        public int RecognitionPauseSetting
        {
            get { return _recognitionPauseSetting; }
            set { SetProperty(ref _recognitionPauseSetting, value); }
        }

        private string _systemMessages = "";
        /// <summary>
        /// 系统信息
        /// </summary>
        public string SystemMessages
        {
            get { return _systemMessages; }
            set { SetProperty(ref _systemMessages, value); }
        }

        private bool _messageBorderVisibility;
        /// <summary>
        /// 信息框可见控制
        /// </summary>
        public bool MessageBorderVisibility
        {
            get { return _messageBorderVisibility; }
            set { SetProperty(ref _messageBorderVisibility, value); }
        }

        private Brush _messageBackground;
        /// <summary>
        /// 信息框背景颜色
        /// </summary>
        public Brush MessageBackground
        {
            get { return _messageBackground; }
            set { SetProperty(ref _messageBackground, value); }
        }

        private string _recognitionSignBackground = "2";
        /// <summary>
        /// 识别标志背景颜色 0:识别中 1：复位识别中 2：未知
        /// </summary>
        public string RecognitionSignBackground
        {
            get { return _recognitionSignBackground; }
            set { SetProperty(ref _recognitionSignBackground, value); }
        }

        private string _againRecognitionBackground = "2";
        /// <summary>
        /// 再次识别背景颜色 0:NO 1：OK 2：未知
        /// </summary>
        public string AgainRecognitionBackground
        {
            get { return _againRecognitionBackground; }
            set { SetProperty(ref _againRecognitionBackground, value); }
        }

        private bool _manualRecognitionEnabled = true;
        /// <summary>
        /// 手动识别按钮使能
        /// </summary>
        public bool ManualRecognitionEnabled
        {
            get { return _manualRecognitionEnabled; }
            set { SetProperty(ref _manualRecognitionEnabled, value); }
        }
        #endregion
        #region======其他======
        /// <summary>
        /// 系统模式切换命令
        /// </summary>
        public DelegateCommand SystemModeSwitchCommand { get; set; }
        /// <summary>
        /// 手动识别命令
        /// </summary>
        public DelegateCommand ManualRecognitionCommand { get; set; }

        /// <summary>
        /// 当前采集的图像
        /// </summary>
        private HObject CurrentImage = new HObject();

        /// <summary>
        /// 当天识别的轮型列表
        /// </summary>
        private List<string> TodayWheels { get; set; } = new List<string>();

        private string Plc2ConnectStatus = "0";

        /// <summary>
        /// PLC连接
        /// </summary>
        private S7Client PlcCilent;

        /// <summary>
        /// PLC IP地址
        /// </summary>
        private string PlcIP;
        /// <summary>
        /// PLC IP2地址
        /// </summary>
        private string PlcIP2;
        /// <summary>
        /// 读取PLC数据的DB块
        /// </summary>
        private int ReadDB;
        /// <summary>
        /// 读取PLC数据的起始地址
        /// </summary>
        private int ReadStartAddress;
        /// <summary>
        /// 读取PLC数据的长度
        /// </summary>
        private int ReadLenght;
        /// <summary>
        /// 读缓冲区
        /// </summary>
        private byte[] ReadBuffer = new byte[1];
        /// <summary>
        /// 写入PLC数据的DB块
        /// </summary>
        private int WriteDB;
        /// <summary>
        /// 写入PLC2数据的DB块
        /// </summary>
        private int WriteDB2;
        /// <summary>
        /// 写入PLC数据的起始地址
        /// </summary>
        private int WriteStartAddress;
        /// <summary>
        /// 写入PLC数据的起始地址
        /// </summary>
        private int WriteStartAddress2;
        /// <summary>
        /// 写入PLC数据的长度
        /// </summary>
        private int WriteLenght;
        /// <summary>
        /// 写入PLC数据的长度
        /// </summary>
        private int WriteLenght2;
        /// <summary>
        /// 写入缓冲区
        /// </summary>
        private byte[] WriteBuffer = new byte[14];
        /// <summary>
        /// 写入缓冲区2
        /// </summary>
        private byte[] WriteBuffer2 = new byte[1];

        /// <summary>
        /// 大屏显示的分选数据
        /// </summary>
        private List<ScreenedDataModel> ScreenedDatas = new List<ScreenedDataModel>();
        /// <summary>
        /// 大屏数据缓冲区
        /// </summary>
        private byte[] BigScreenDataBuffer = new byte[300];
        /// <summary>
        /// 大屏数据DB
        /// </summary>
        private int BigScreenDataDB;
        /// <summary>
        /// 大屏数据起始位
        /// </summary>
        private int BigScreenDataStartByte;
        /// <summary>
        /// 大屏数据长度
        /// </summary>
        private int BigScreenDataLength;
        /// <summary>
        /// 轮毂到位延时
        /// </summary>
        private int ArrivalDelay;
        /// <summary>
        /// 轮毂到位信号
        /// </summary>
        private bool ArrivalSignal;

        /// <summary>
        /// 轮毂到位信号数组
        /// </summary>
        private bool[] ArrivalSignals = new bool[5];

        /// <summary>
        /// 信息显示定时器
        /// </summary>
        private DispatcherTimer MessageShowTimer;

        /// <summary>
        /// 数据更新的日期
        /// </summary>
        private int UpdatedDay { get; set; }
        /// <summary>
        /// 系统数据在每天的几点更新
        /// </summary>
        private int UpdateTime { get; set; }
        /// <summary>
        /// 每天数据更新中标志
        /// </summary>
        private bool DataBeingUpdated { get; set; }

        /// <summary>
        /// 手动识别标志
        /// </summary>
        private bool ManualIdentify { get; set; }

        /// <summary>
        /// 电机故障信号
        /// </summary>
        private bool MotorFailureSignal { get; set; } = false;

        private bool _isIdentifying;
        /// <summary>
        /// 自动模式识别中标志
        /// </summary>
        public bool IsIdentifying
        {
            get { return _isIdentifying; }
            set
            {
                _isIdentifying = value;
                if (_isIdentifying)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RecognitionSignBackground = "0";
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RecognitionSignBackground = "1";
                    }));
                }
            }
        }


        /// <summary>
        /// 模板加载中
        /// </summary>
        private bool TemplatesLoading { get; set; } = true;

        private IRegionManager _regionManager;

        #endregion

        private CancellationTokenSource cts = new CancellationTokenSource();

        public MainViewModel(IRegionManager regionManager)
        {
            Task.Run(() =>
            {
                LoadTemplates();
                TemplatesLoading = false;
            });
            LoadSystemDatas();
            ExternalConnectionThread();
            MainThread();
            MainThread5();

            //初始化数据库
            SqlAccess sqlAccess = new SqlAccess();
            sqlAccess.InitializeTable();
            //PLC连接
            PlcCilent = new S7Client();

            //加载功能页面
            regionManager.RegisterViewWithRegion("ViewRegion", "DisplayInterfaceView");
            regionManager.RegisterViewWithRegion("ViewRegion", "MonitoringView");
            regionManager.RegisterViewWithRegion("ViewRegion", "TemplateManagementView");
            regionManager.RegisterViewWithRegion("ViewRegion", "ReportManagementView");
            regionManager.RegisterViewWithRegion("ViewRegion", "SystemSettingsView");
            _regionManager = regionManager;

            SystemModeSwitchCommand = new DelegateCommand(SystemModeSwitch);
            ManualRecognitionCommand = new DelegateCommand(ManualRecognition);
            //订阅消息
            EventMessage.MessageHelper.GetEvent<SystemMessageDisplayEvent>().Subscribe(SystemMessageDisplay);
            EventMessage.MessageHelper.GetEvent<RecognitionPauseSettingEvent>().Subscribe(RecognitionPauseSet);
            EventMessage.MessageHelper.GetEvent<ServletInfoEvent>().Subscribe(ServletDisplay);
            EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Subscribe(CallShow);

            RecognitionStatus1 = "";
            IsIdentifying = false;
        }

        /// <summary>
        /// 主线程
        /// </summary>
        private void MainThread()
        {
            Task.Run(async () =>
            {
                MainThreadControl = true;
                int grabAbnormal = 0;
                while (MainThreadControl)
                {
                    // 轮毂到位          自动模式      没有在识别中     没有在模板数据更新中            PLC已连接              相机已连接              再次识别使能信号已复位                   
                    if (ArrivalSignal && SystemModel && !IsIdentifying && !TemplateDataUpdataControl && PlcCilent.Connected && CameraStatus == "1" && !S7.GetBitAt(ReadBuffer, 0, 3)
                    && !S7.GetBitAt(WriteBuffer, 12, 4) && !DataBeingUpdated && !MotorFailureSignal && !AutoTemplateDataLoadControl || ManualIdentify)
                    //没有在识别暂停中                没有在每天数据更新中    识别工位电机正常        没有在加模板表格数据
                    {
                        //再次识别使能复位信号复位
                        S7.SetBitAt(ref WriteBuffer, 12, 5, false);
                        IsIdentifying = true;
                        RecognitionStatus1 = "识别中...";
                        #region======清除显示======
                        RecognitionWheelType1 = "";
                        ScreenedResultDisplay = "";
                        Similarity1 = "";
                        GateResult = "";
                        TimeConsumed1 = "";
                        #endregion
                        //轮毂到位延时
                        await Task.Delay(ArrivalDelay);
                        #region======采集图像======
                        try
                        {
                            HOperatorSet.GrabImage(out HObject image, CameraHandle);
                            HOperatorSet.Rgb1ToGray(image, out HObject grayImage);
                            CurrentImage.Dispose();
                            if (CroppingOrNot)
                            {
                                Cropping(grayImage, out HObject SourceImage);
                                HOperatorSet.ZoomImageFactor(SourceImage, out CurrentImage, ScalingCoefficient, ScalingCoefficient, "constant");
                            }
                            else
                            {
                                HOperatorSet.ZoomImageFactor(grayImage, out CurrentImage, ScalingCoefficient, ScalingCoefficient, "constant");
                            }
                            grabAbnormal = 0;
                        }
                        catch (Exception ex)
                        {
                            grabAbnormal++;
                            if (grabAbnormal == 3)
                            {
                                EventMessage.MessageDisplay($"图像采集异常,相机重新连接!", true, true);
                                CameraStatus = "0";
                                CameraHandle = null;
                                if (!ExternalConnectionThreadControl) ExternalConnectionThreadControl = true;
                                ExternalConnectionThread();
                                CurrentImage.Dispose();
                                grabAbnormal = 0;
                                IsIdentifying = false;
                                ManualIdentify = false;
                            }
                            else
                            {
                                await Task.Delay(1000);
                                CurrentImage.Dispose();
                                RecognitionStatus1 = "采集异常";
                                EventMessage.MessageDisplay($"第{grabAbnormal}次图像采集异常:" + ex.Message, false, true);
                                IsIdentifying = false;
                            }
                        }
                        #endregion
                        if (CurrentImage.IsInitialized())
                        {
                            #region======浇口检测与轮毂识别======
                            DateTime startTime = DateTime.Now;
                            //定位轮毂
                            PositioningWheelResultModel pResult = PositioningWheel(CurrentImage, WheelMinThreshold, 255, WheelMinRadius);
                            //存储浇口检测结果
                            GateDetectionResultModel gateResult = new GateDetectionResultModel();
                            //存储识别结果
                            RecognitionResultModel recognitionResult = new RecognitionResultModel();
                            //如果定位到轮毂
                            if (pResult.WheelImage != null)
                            {
                                if (GateDetectionSwitch)
                                {
                                    //浇口检测
                                    gateResult = GateDetection(pResult.WheelImage, pResult.CenterRow, pResult.CenterColumn,
                                        PositioningGateRadius, GateOutMinThreshold, GateMinArea, GateMinRadius);
                                    if (gateResult.DetectionResult) GateResult = "OK";
                                    else GateResult = "NG";
                                }
                                //轮毂识别
                                recognitionResult = WheelRecognitionAlgorithm(pResult.WheelImage, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
                            }
                            else//没有定位到轮毂
                            {
                                GateResult = "_";
                                recognitionResult = WheelRecognitionAlgorithm(CurrentImage, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
                            }
                            #endregion
                            #region======结果显示与识别暂停判断======
                            HObject templateContour = new HObject();
                            if (recognitionResult.RecognitionWheelType != "NG")
                            {
                                templateContour = GetAffineTemplateContour(recognitionResult.TemplateID, recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
                            }
                            AutoRecognitionResultDisplayModel autoRecognitionResult = new AutoRecognitionResultDisplayModel();
                            autoRecognitionResult = new AutoRecognitionResultDisplayModel
                            {
                                WheelType = recognitionResult.RecognitionWheelType,
                                CurrentImage = CurrentImage,
                                WheelContour = pResult.WheelContour,
                                TemplateContour = templateContour,
                                GateContour = gateResult.GateContour,
                                IsGate = gateResult.DetectionResult
                            };
                            //图像结果显示
                            EventMessage.MessageHelper.GetEvent<AutoRecognitionResultDisplayEvent>().Publish(autoRecognitionResult);
                            RecognitionWheelType1 = recognitionResult.RecognitionWheelType;
                            Similarity1 = recognitionResult.Similarity.ToString();

                            if (recognitionResult.RecognitionWheelType == "NG" && RecognitionPauseSetting != 0)
                            {
                                //在此处判断识别是否暂停
                                CurrentNgNumber++;
                                SqlAccess.SystemDatasWrite("CurrentNgNumber", CurrentNgNumber.ToString());
                                //如果当前NG次数大于等于设置的识别暂停次数
                                if (CurrentNgNumber >= RecognitionPauseSetting)
                                {
                                    //将识别暂停状态发送给PLC
                                    S7.SetBitAt(ref WriteBuffer, 12, 4, true);
                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        WMessageBox.Show("视觉识别NG次数已达到设定次数，视觉识别暂停，请检查是否需要录入新模板！", MessageType.Warning);
                                    }));
                                }
                            }
                            DateTime endTime = DateTime.Now;
                            TimeSpan consumeTime = endTime.Subtract(startTime);
                            TimeConsumed1 = Convert.ToString(Convert.ToInt32(consumeTime.TotalMilliseconds)) + " ms";
                            #endregion
                            #region======发送轮型给PLC======
                            byte[] wheelTypeBuffer = new byte[12];
                            string wheelType;
                            //如果浇口检测开关打开，并且浇口检测结果为False，给PLC发送轮型为NG
                            if (GateDetectionSwitch && !gateResult.DetectionResult)
                            {
                                wheelType = "NG";
                                S7.SetBitAt(ref WriteBuffer2, 0, 0, true); //浇口NG
                            }
                            else
                            {
                                wheelType = RecognitionWheelType1.Trim('_');
                                S7.SetBitAt(ref WriteBuffer2, 0, 0, false); //浇口OK
                            }


                            //将轮型字符串转换成字节数组
                            var wheelTypeBytes = Encoding.Default.GetBytes(wheelType);
                            //将轮型字符串的长度转换为字节数组byte[]
                            byte[] wheelTypeLength = BitConverter.GetBytes(wheelType.Length);
                            WriteBuffer[0] = 10;
                            WriteBuffer[1] = wheelTypeLength[0];
                            for (int i = 2; i < wheelTypeBytes.Length + 2; i++)
                            {
                                WriteBuffer[i] = wheelTypeBytes[i - 2];
                            }
                            #endregion
                            #region======轮毂分选判断======
                            if (IsScreenedResult)
                            {
                                TemplateDataModel templateDate = null;
                                bool screenedResult = false;
                                if (recognitionResult.RecognitionWheelType != "NG")
                                {
                                    //从在线数据中判断是否分选
                                    screenedResult = ScreenedResult(recognitionResult.RecognitionWheelType.Trim('_'), ScreenedDatas);
                                    //根据识别轮型获取模板表格数据
                                    templateDate = TemplateDataList.First(x => x.WheelType == recognitionResult.RecognitionWheelType);
                                }
                                //如果没有检测到浇口 或  当前轮型强制分选打开 或 从在线数据中判断需要分选 或 识别为NG
                                if (!gateResult.DetectionResult || templateDate != null && templateDate.SortingEnable || screenedResult || RecognitionWheelType1 == "NG")
                                {
                                    WriteBuffer[13] = 1;
                                    ScreenedResultDisplay = "分选";
                                }
                                else
                                {
                                    WriteBuffer[13] = 0;
                                    ScreenedResultDisplay = "下转";
                                }
                            }
                            #endregion
                            #region======如果在不活跃模板中匹配成功,实时调整模板======
                            if (recognitionResult.RecognitionWheelType != null && recognitionResult.IsInNotTemplate)
                            {
                                string activePath = ActiveTemplatesPath + @"\" + recognitionResult.RecognitionWheelType + ".ncm";
                                string notActivePath = NotActiveTemplatesPath + @"\" + recognitionResult.RecognitionWheelType + ".ncm";
                                if (File.Exists(notActivePath))
                                {
                                    try
                                    {
                                        File.Move(notActivePath, activePath);
                                        int index = TemplateDataCollection.NotActiveTemplateNames.FindIndex(x => x == recognitionResult.RecognitionWheelType);
                                        if (index >= 0)
                                        {
                                            TemplateDataCollection.ActiveTemplateNames.Add(TemplateDataCollection.NotActiveTemplateNames[index]);
                                            TemplateDataCollection.ActiveTemplates.Add(TemplateDataCollection.NotActiveTemplates[index]);
                                            TemplateDataCollection.NotActiveTemplateNames.RemoveAt(index);
                                            TemplateDataCollection.NotActiveTemplates.RemoveAt(index);
                                        }
                                        EventMessage.MessageDisplay("实时调整模板，型号是：" + recognitionResult.RecognitionWheelType, true, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        EventMessage.MessageDisplay("实时调整模板失败：" + ex.Message, false, true);
                                    }
                                }
                            }
                            #endregion
                            #region======保存当天识别轮型======
                            //如果当天活跃轮型列表中没有当前识别的轮型
                            if (recognitionResult.RecognitionWheelType != "NG" && !TodayWheels.Contains(recognitionResult.RecognitionWheelType))
                            {
                                //将当前轮型添加到列表中
                                TodayWheels.Add(recognitionResult.RecognitionWheelType);
                                //添加到数据库，防止停电或系统崩溃
                                var sDB = new SqlAccess().SystemDataAccess;
                                var d = sDB.Queryable<ActiveWheelTypeDataModel>().First(x => x.WheelType == recognitionResult.RecognitionWheelType);
                                if (d == null)
                                {
                                    var id = sDB.Queryable<ActiveWheelTypeDataModel>().Max(x => x.ID);
                                    ActiveWheelTypeDataModel activeWheelTypeDataModel = new ActiveWheelTypeDataModel()
                                    {
                                        ID = id + 1,
                                        WheelType = recognitionResult.RecognitionWheelType,
                                    };
                                    sDB.Insertable(activeWheelTypeDataModel).ExecuteCommand();
                                }
                            }
                            #endregion
                            #region======构建数据并保存======
                            try
                            {
                                var pDB = new SqlAccess().ProductionDataAccess;
                                int maxIndex = pDB.Queryable<ProductionDataModel>().Max(i => i.Index);
                                ProductionDataModel productionDataModel = new ProductionDataModel
                                {
                                    Index = maxIndex + 1,
                                    WheelType = RecognitionWheelType1,
                                    TimeConsumed = TimeConsumed1,
                                    Similarity = Similarity1,
                                    RecognitionTime = DateTime.Parse(endTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                };
                                pDB.Insertable(productionDataModel).ExecuteCommand();
                                if (SaveImageDays != 0) SaveImageDatas(CurrentImage, productionDataModel, endTime, GateResult);
                            }
                            catch (Exception ex)
                            {
                                EventMessage.MessageDisplay(ex.Message, false, true);
                            }
                            #endregion
                            //发送识别完成信号
                            S7.SetBitAt(ref WriteBuffer, 12, 2, true);
                            RecognitionStatus1 = "识别完成";
                        }
                    }
                    //手动识别复位
                    if (ManualIdentify && IsIdentifying)
                    {
                        ManualIdentify = false;
                    }
                    //更新匹配用模板数据
                    if (TemplateDataUpdataControl && !IsIdentifying && !DataBeingUpdated)
                    {
                        try
                        {
                            UpdateTemplateDatas();
                        }
                        catch (Exception ex)
                        {
                            EventMessage.MessageDisplay(ex.Message, false, true);
                        }
                        finally
                        {
                            TemplateDataUpdataControl = false;
                        }
                    }
                    if (AutoTemplateDataLoadControl && !IsIdentifying)
                    {
                        try
                        {
                            List<TemplateDataModel> datas = new SqlAccess().SystemDataAccess.Queryable<TemplateDataModel>().ToList();
                            TemplateDataList.Clear();
                            TemplateDataList = datas;
                        }
                        catch (Exception ex)
                        {
                            EventMessage.MessageDisplay(ex.Message, false, true);
                        }
                        finally
                        {
                            AutoTemplateDataLoadControl = false;
                        }
                    }
                    //定时执行数据更新
                    if (DateTime.Now.Day != UpdatedDay && DateTime.Now.Hour >= UpdateTime && !IsIdentifying && !DataBeingUpdated && !TemplatesLoading)
                    {
                        DataBeingUpdated = true;
                        PerformDataUpdates();
                    }
                    await Task.Delay(20);
                }
            });
        }
        private void MainThread5()
        {
           
            for (int i = 0; i < ArrivalSignals.Count(); i++)
            {
                Thread.Sleep(100);
                int localI = i; // 创建一个局部变量来保存当前的i值
                Task.Run(async () => await DoJob(cts.Token, localI), cts.Token);
            }

           
        }

        private async Task DoJob(CancellationToken token, int index)
        {
            while(!token.IsCancellationRequested)
            {
                // 模拟一些工作
                await Task.Delay(1000);
                Console.WriteLine($"任务 {index} 完成");
                //  轮毂到位                 自动模式        PLC已连接   
                if (ArrivalSignals[index] && SystemModel && PlcCilent.Connected)
                {

                }
            }
            
        }

        /// <summary>
        /// 页面跳转显示
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ServletDisplay(ServletInfoModel model)
        {
            string path = model.Path;
            _regionManager.RequestNavigate("ViewRegion", "TemplateManagementView");
            EventMessage.MessageHelper.GetEvent<TemplatePicUpdateEvent>().Publish(model);
        }
        /// <summary>
        /// 处理回复显示
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CallShow(InteractS7PLCModel model)
        {
            switch (model.Index)
            {
                case 1:
                    RecognitionWheelType1 = model.wheelType;
                    Similarity1 = model.similarity.ToString();
                    TimeConsumed1 = model.Interval.TotalMilliseconds.ToString();
                    Colour1 = model.colour;
                    break;
                case 2:
                    RecognitionWheelType2 = model.wheelType;
                    Similarity2 = model.similarity.ToString();
                    TimeConsumed2 = model.Interval.TotalMilliseconds.ToString();
                    Colour2 = model.colour;
                    break;
                case 3:
                    RecognitionWheelType3 = model.wheelType;
                    Similarity3 = model.similarity.ToString();
                    TimeConsumed3 = model.Interval.TotalMilliseconds.ToString();
                    Colour3 = model.colour;
                    break;
                case 4:
                    RecognitionWheelType4 = model.wheelType;
                    Similarity4 = model.similarity.ToString();
                    TimeConsumed4 = model.Interval.TotalMilliseconds.ToString();
                    Colour4 = model.colour;
                    break;
                case 5:
                    RecognitionWheelType5 = model.wheelType;
                    Similarity5 = model.similarity.ToString();
                    TimeConsumed5 = model.Interval.TotalMilliseconds.ToString();
                    Colour5 = model.colour;
                    break;
            }
        }

        private void RecognitionPauseSet(string obj)
        {
            RecognitionPauseSetting = SystemDatas.RecognitionPauseSetting;
            if (obj == "0")
            {
                CurrentNgNumber = 0;
                SqlAccess.SystemDatasWrite("CurrentNgNumber", CurrentNgNumber.ToString());
            }
        }



        /// <summary>
        /// 系统信息显示
        /// </summary>
        /// <param name="model"></param>
        private void SystemMessageDisplay(MessageModel model)
        {
            SystemMessages = model.Message;
            if (model.Type == MessageType.Default)
            {
                MessageBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE3FB"));
            }
            else if (model.Type == MessageType.Warning)
            {
                MessageBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3F5C8"));
            }
            else if (model.Type == MessageType.Success)
            {
                MessageBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAADEB7"));
            }
            else if (model.Type == MessageType.Error)
            {
                MessageBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF78883"));
            }
            MessageBorderVisibility = true;
            if (MessageShowTimer != null && MessageShowTimer.IsEnabled)
                MessageShowTimer.Stop();
            MessageShowTimer = new DispatcherTimer();
            MessageShowTimer.Interval = new TimeSpan(0, 0, 3);
            MessageShowTimer.Tick += MessageShowTimer_Tick;
            MessageShowTimer.Start();
        }
        /// <summary>
        /// 信息显示定时器响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageShowTimer_Tick(object sender, EventArgs e)
        {
            MessageBorderVisibility = false;
            MessageShowTimer.Stop();
        }

        /// <summary>
        /// 解析分选数据
        /// </summary>
        /// <param name="data"></param>
        private List<ScreenedDataModel> ParseScreenedDatas(byte[] data)
        {
            List<ScreenedDataModel> bigScreenDatas = new List<ScreenedDataModel>();
            List<string> unitName = new List<string> { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "B8", "B7", "B6", "B5", "B4", "B3", "B2", "B1", "A9", "B9" };
            List<bool> unitState = new List<bool> { S7.GetBitAt(data, 0, 0), S7.GetBitAt(data, 0, 1), S7.GetBitAt(data, 0, 2), S7.GetBitAt(data, 0, 3), S7.GetBitAt(data, 0, 4), S7.GetBitAt(data, 0, 5), S7.GetBitAt(data, 0, 6), S7.GetBitAt(data, 0, 7),
            S7.GetBitAt(data, 1, 0), S7.GetBitAt(data, 1, 1), S7.GetBitAt(data, 1, 2), S7.GetBitAt(data, 1, 3), S7.GetBitAt(data, 1, 4), S7.GetBitAt(data, 1, 5), S7.GetBitAt(data, 1, 6), S7.GetBitAt(data, 1, 7),
            S7.GetBitAt(data, 2, 0),S7.GetBitAt(data, 2, 1)};
            for (int i = 0; i < 18; i++)
            {
                ScreenedDataModel unitData = new ScreenedDataModel()
                {
                    Unit = unitName[i],
                    State = unitState[i],
                    WheelType = Encoding.Default.GetString(data.Skip(6 + i * 12).Take(8).ToArray()),
                    OnlineQuantity = BitConverter.ToInt16(new byte[] { data[220 + i * 2 + 1], data[220 + i * 2] }, 0),
                    TargetQuantity = BitConverter.ToInt16(new byte[] { data[256 + i * 2 + 1], data[256 + i * 2] }, 0)
                };
                bigScreenDatas.Add(unitData);
            }
            return bigScreenDatas;
        }

        /// <summary>
        /// 加载系统数据
        /// </summary>
        private void LoadSystemDatas()
        {
            try
            {
                ReadAppSettings("UpdateTime", out string updateTime);
                UpdateTime = int.Parse(updateTime);
                #region======图像处理数据======
                ReadAppSettings("CameraIdentifier", out string cameraIdentifier);
                CameraIdentifier = cameraIdentifier;
                ReadAppSettings("TemplateStartAngle", out string templateStartAngle);
                TemplateStartAngle = double.Parse(templateStartAngle);
                ReadAppSettings("TemplateEndAngle", out string templateEndAngle);
                TemplateEndAngle = double.Parse(templateEndAngle);
                ReadAppSettings("AngleStart", out string angleStart);
                AngleStart = double.Parse(angleStart);
                ReadAppSettings("AngleExtent", out string angleExtent);
                AngleExtent = double.Parse(angleExtent);
                ReadAppSettings("ScalingCoefficient", out string scalingCoefficient);
                ScalingCoefficient = double.Parse(scalingCoefficient);
                #endregion
                #region======外部连接数据======
                ReadAppSettings("PlcIP", out string plcIP);
                PlcIP = plcIP;
                ReadAppSettings("ReadDB", out string readDB);
                ReadDB = int.Parse(readDB);
                ReadAppSettings("ReadStartAddress", out string readStartAddress);
                ReadStartAddress = int.Parse(readStartAddress);
                ReadAppSettings("ReadLenght", out string readLenght);
                ReadLenght = int.Parse(readLenght);
                ReadAppSettings("WriteDB", out string writeDB);
                WriteDB = int.Parse(writeDB);
                ReadAppSettings("WriteStartAddress", out string writeStartAddress);
                WriteStartAddress = int.Parse(writeStartAddress);
                ReadAppSettings("WriteLenght", out string writeLenght);
                WriteLenght = int.Parse(writeLenght);
                ReadAppSettings("BigScreenDataDB", out string bigScreenDataDB);
                BigScreenDataDB = int.Parse(bigScreenDataDB);
                ReadAppSettings("BigScreenDataStartByte", out string bigScreenDataStartByte);
                BigScreenDataStartByte = int.Parse(bigScreenDataStartByte);
                ReadAppSettings("BigScreenDataLength", out string bigScreenDataLength);
                BigScreenDataLength = int.Parse(bigScreenDataLength);
                ReadAppSettings("ArrivalDelay", out string arrivalDelay);
                ArrivalDelay = int.Parse(arrivalDelay);

                ReadAppSettings("IsScreenedResult", out string isScreenedResult);
                bool r = bool.TryParse(isScreenedResult, out bool result);
                if (r) IsScreenedResult = result;
                else IsScreenedResult = false;
                AutoTemplateDataLoadControl = true;
                ReadAppSettings("CroppingOrNot", out string isCroppingOrNot);
                bool r1 = bool.TryParse(isCroppingOrNot, out bool result1);
                if (r1) CroppingOrNot = result1;
                else CroppingOrNot = false;
                //---PLC2 用来发浇口NG
                ReadAppSettings("PlcIP2", out string plcIP2);
                PlcIP2 = plcIP2;
                ReadAppSettings("WriteDB2", out string writeDB2);
                WriteDB2 = int.Parse(writeDB2);
                ReadAppSettings("WriteStartAddress2", out string writeStartAddress2);
                WriteStartAddress2 = int.Parse(writeStartAddress2);
                ReadAppSettings("WriteLenght2", out string writeLenght2);
                WriteLenght2 = int.Parse(writeLenght2);
                #endregion
                #region======系统其他数据======
                var sDB = new SqlAccess().SystemDataAccess;
                var systemDatas = sDB.Queryable<SystemSettingsDataModel>().ToList();
                WheelMinThreshold = int.Parse(systemDatas.First(x => x.Name == "WheelMinThreshold").Value);
                WindowMaxThreshold = int.Parse(systemDatas.First(x => x.Name == "WindowMaxThreshold").Value);
                RemoveMixArea = double.Parse(systemDatas.First(x => x.Name == "RemoveMixArea").Value);
                MinSimilarity = double.Parse(systemDatas.First(x => x.Name == "MinSimilarity").Value);
                PositioningGateRadius = double.Parse(systemDatas.First(x => x.Name == "PositioningGateRadius").Value);
                GateOutMinThreshold = int.Parse(systemDatas.First(x => x.Name == "GateOutMinThreshold").Value);
                GateMinArea = int.Parse(systemDatas.First(x => x.Name == "GateMinArea").Value);
                GateMinRadius = double.Parse(systemDatas.First(x => x.Name == "GateMinRadius").Value);
                GateDetectionSwitch = bool.Parse(systemDatas.First(x => x.Name == "GateDetectionSwitch").Value);
                SaveImageDays = int.Parse(systemDatas.First(x => x.Name == "SaveImageDays").Value);
                SaveDataMonths = int.Parse(systemDatas.First(x => x.Name == "SaveDataMonths").Value);
                RecognitionPauseSetting = int.Parse(systemDatas.First(x => x.Name == "RecognitionPauseSetting").Value);
                CurrentNgNumber = int.Parse(systemDatas.First(x => x.Name == "CurrentNgNumber").Value);
                TemplateAdjustDays = int.Parse(systemDatas.First(x => x.Name == "TemplateAdjustDays").Value);
                UpdatedDay = int.Parse(systemDatas.First(x => x.Name == "UpdatedDay").Value);
                WheelMinRadius = int.Parse(systemDatas.First(x => x.Name == "WheelMinRadius").Value);
                #endregion
                //加载活跃轮型数据
                TodayWheels.Clear();
                var datas = sDB.Queryable<ActiveWheelTypeDataModel>().ToList();
                for (int i = 0; i < datas.Count; i++) TodayWheels.Add(datas[i].WheelType);
                EventMessage.MessageDisplay("系统数据加载完成。", true, false);
            }
            catch (Exception ex)
            {
                EventMessage.MessageDisplay("系统数据加载错误：" + ex.Message, true, true);
            }
        }
        /// <summary>
        /// 加载模板
        /// </summary>
        private void LoadTemplates()
        {
            //清空模板
            for (int i = 0; i < TemplateDataCollection.ActiveTemplates.Count; i++)
            {
                HOperatorSet.ClearNccModel(TemplateDataCollection.ActiveTemplates[i]);
            }
            for (int i = 0; i < TemplateDataCollection.NotActiveTemplates.Count; i++)
            {
                HOperatorSet.ClearNccModel(TemplateDataCollection.NotActiveTemplates[i]);
            }
            TemplateDataCollection.ActiveTemplates.Clear();
            TemplateDataCollection.NotActiveTemplates.Clear();
            TemplateDataCollection.ActiveTemplateNames.Clear();
            TemplateDataCollection.NotActiveTemplateNames.Clear();

            //获取指定路径下所有文件
            string[] activeFiles = Directory.GetFiles(ActiveTemplatesPath);
            string[] notActiveFiles = Directory.GetFiles(NotActiveTemplatesPath);
            try
            {
                if (activeFiles.Length > 0)
                {
                    for (int a = 0; a < activeFiles.Length; a++)
                    {
                        int index = activeFiles[a].LastIndexOf(@"\") + 1;
                        string str = activeFiles[a].Substring(index, activeFiles[a].Length - index);//截取路径后的字符串

                        string wheelType = str.Trim('.', 'n', 'c', 'm');//修剪掉其中的.ncm
                        TemplateDataCollection.ActiveTemplateNames.Add(wheelType);

                        string strPath = activeFiles[a].Replace(@"\", "/");//字符串替换
                        HOperatorSet.ReadNccModel(strPath, out HTuple modelID);//读NCC模板
                        TemplateDataCollection.ActiveTemplates.Add(modelID);
                    }
                }
                if (notActiveFiles.Length > 0)
                {
                    for (int a = 0; a < notActiveFiles.Length; a++)
                    {
                        int index = notActiveFiles[a].LastIndexOf(@"\") + 1;
                        string str = notActiveFiles[a].Substring(index, notActiveFiles[a].Length - index);
                        string wheelType = str.Trim('.', 'n', 'c', 'm');
                        TemplateDataCollection.NotActiveTemplateNames.Add(wheelType);

                        string strPath = notActiveFiles[a].Replace(@"\", "/");
                        HOperatorSet.ReadNccModel(strPath, out HTuple modelID);
                        TemplateDataCollection.NotActiveTemplates.Add(modelID);
                    }
                }
                EventMessage.MessageDisplay($"模板加载完成，活跃模板{TemplateDataCollection.ActiveTemplates.Count}个，不活跃模板{TemplateDataCollection.NotActiveTemplates.Count}个。", true, false);
            }
            catch (Exception ex)
            {
                EventMessage.MessageDisplay("加载模板错误：" + ex.Message, true, true);
            }
        }
        /// <summary>
        /// 更新匹配用模板数据
        /// </summary>
        private void UpdateTemplateDatas()
        {
            if (DelTemplateNames.Count > 0)
            {
                //处理删除的模板
                for (int i = 0; i < DelTemplateNames.Count; i++)
                {
                    int activeIndex = TemplateDataCollection.ActiveTemplateNames.FindIndex(x => x == DelTemplateNames[i]);
                    int notActiveIndex = TemplateDataCollection.NotActiveTemplateNames.FindIndex(x => x == DelTemplateNames[i]);
                    if (activeIndex >= 0)
                    {
                        TemplateDataCollection.ActiveTemplateNames.RemoveAt(activeIndex);
                        TemplateDataCollection.ActiveTemplates[activeIndex].Dispose();
                        TemplateDataCollection.ActiveTemplates.RemoveAt(activeIndex);
                    }
                    if (notActiveIndex >= 0)
                    {
                        TemplateDataCollection.NotActiveTemplateNames.RemoveAt(notActiveIndex);
                        TemplateDataCollection.NotActiveTemplates[notActiveIndex].Dispose();
                        TemplateDataCollection.NotActiveTemplates.RemoveAt(notActiveIndex);
                    }
                }
                //清空删除数据
                DelTemplateNames.Clear();
            }
            if (AddOrReviseTemplateDatas.ActiveTemplateNames.Count > 0)
            {
                //处理增加或修改的活跃模板
                for (int i = 0; i < AddOrReviseTemplateDatas.ActiveTemplateNames.Count; i++)
                {
                    int activeIndex = TemplateDataCollection.ActiveTemplateNames.FindIndex(x => x == AddOrReviseTemplateDatas.ActiveTemplateNames[i]);
                    if (activeIndex < 0)
                    {
                        TemplateDataCollection.ActiveTemplateNames.Add(AddOrReviseTemplateDatas.ActiveTemplateNames[i]);
                        TemplateDataCollection.ActiveTemplates.Add(AddOrReviseTemplateDatas.ActiveTemplates[i]);
                    }
                    else
                    {
                        TemplateDataCollection.ActiveTemplates[activeIndex].Dispose();
                        TemplateDataCollection.ActiveTemplates[activeIndex] = AddOrReviseTemplateDatas.ActiveTemplates[i];
                    }
                }
                //清空增加或修改的活跃模板数据
                for (int i = 0; i < AddOrReviseTemplateDatas.ActiveTemplates.Count; i++)
                {
                    AddOrReviseTemplateDatas.ActiveTemplates.RemoveAt(i);
                }
                AddOrReviseTemplateDatas.ActiveTemplates.Clear();
                AddOrReviseTemplateDatas.ActiveTemplateNames.Clear();
            }
            if (AddOrReviseTemplateDatas.NotActiveTemplateNames.Count > 0)
            {
                //处理增加或修改的不活跃模板
                for (int i = 0; i < AddOrReviseTemplateDatas.NotActiveTemplateNames.Count; i++)
                {
                    int notActiveIndex = TemplateDataCollection.NotActiveTemplateNames.FindIndex(x => x == AddOrReviseTemplateDatas.NotActiveTemplateNames[i]);
                    if (notActiveIndex < 0)
                    {
                        TemplateDataCollection.NotActiveTemplateNames.Add(AddOrReviseTemplateDatas.NotActiveTemplateNames[i]);
                        TemplateDataCollection.NotActiveTemplates.Add(AddOrReviseTemplateDatas.NotActiveTemplates[i]);
                    }
                    else
                    {
                        TemplateDataCollection.NotActiveTemplates[notActiveIndex].Dispose();
                        TemplateDataCollection.NotActiveTemplates[notActiveIndex] = AddOrReviseTemplateDatas.NotActiveTemplates[i];
                    }
                }
                //清空增加或修改的不活跃模板数据
                for (int i = 0; i < AddOrReviseTemplateDatas.NotActiveTemplateNames.Count; i++)
                {
                    AddOrReviseTemplateDatas.NotActiveTemplates.RemoveAt(i);
                }
                AddOrReviseTemplateDatas.NotActiveTemplates.Clear();
                AddOrReviseTemplateDatas.NotActiveTemplateNames.Clear();
            }
        }

        /// <summary>
        /// 系统运行模式切换
        /// </summary>
        private void SystemModeSwitch()
        {
            if (SystemModelContent == "手动模式")
            {
                bool result = WMessageBox.Show("系统运行模式切换", "是否切换到自动模式？");
                if (result)
                {
                    if (!PlcCilent.Connected || CameraStatus != "1")
                    {
                        EventMessage.SystemMessageDisplay("切换失败，请检查PLC和相机的连接状态！", MessageType.Error);
                        return;
                    }
                    Task.Run(async () =>
                    {
                        //切换到自动模式时如果再次识别使能信号为True, 则复位再次识别使能信号
                        if (S7.GetBitAt(ReadBuffer, 0, 3))
                        {
                            S7.SetBitAt(ref WriteBuffer, 12, 5, false);
                            await Task.Delay(1000);
                            S7.SetBitAt(ref WriteBuffer, 12, 5, true);
                        }
                    });
                    //切换到自动模式时如果识别完成信号为Treu, 则复位识别完成信号
                    if (S7.GetBitAt(WriteBuffer, 12, 2))
                        S7.SetBitAt(ref WriteBuffer, 12, 2, false);
                    //切换到自动模式时如果识别中标志为True, 则复位识别中标志
                    if (IsIdentifying) 
                        IsIdentifying = false;
                    SystemModelContent = "自动模式";
                    EventMessage.MessageDisplay("系统切换到自动模式！", true, true);
                    ManualRecognitionEnabled = false;
                    SystemModel = true;
                }
            }
            else
            {
                bool result = WMessageBox.Show("系统运行模式切换", "是否切换到手动模式？");
                if (result)
                {
                    SystemModel = false;
                    SystemModelContent = "手动模式";
                    RecognitionStatus1 = "";

                    EventMessage.MessageDisplay("系统切换到手动模式！", true, true);
                    ManualRecognitionEnabled = true;

                    //如果当前NG次数大于等于设置的识别暂停次数
                    if (CurrentNgNumber >= RecognitionPauseSetting)
                    {
                        CurrentNgNumber = 0;
                        SqlAccess.SystemDatasWrite("CurrentNgNumber", CurrentNgNumber.ToString());
                        //复位给PLC的识别暂停状态
                        S7.SetBitAt(ref WriteBuffer, 12, 4, false);
                    }
                }
            }
            S7.SetBitAt(ref WriteBuffer, 12, 0, SystemModel);
        }

        /// <summary>
        /// 手动识别
        /// </summary>
        private void ManualRecognition()
        {
            if (ManualIdentify)
                return;
            if (!PlcCilent.Connected || CameraStatus != "1")
            {
                EventMessage.SystemMessageDisplay("操作失败，请确认PLC、相机连接状态！", MessageType.Warning);
                return;
            }
            if (TemplateDataUpdataControl || DataBeingUpdated || AutoTemplateDataLoadControl)
            {
                EventMessage.SystemMessageDisplay("数据更新中，请稍后再试！", MessageType.Warning);
                return;
            }
            ManualIdentify = true;
        }

        /// <summary>
        /// 心跳线程
        /// </summary>
        private void HeartbeatThread()
        {
            HeartbeatThreadControl = true;
            Task.Run(async () =>
            {
                while (HeartbeatThreadControl)
                {
                    if (PlcCilent.Connected)
                    {
                        //写心跳数据
                        S7.SetBitAt(ref WriteBuffer, 12, 1, true);
                        if (IsScreenedResult)
                        {
                            //大屏分选数据处理
                            ScreenedDatas = ParseScreenedDatas(BigScreenDataBuffer);
                            List<ScreenedDataModel> screenedDatas = new List<ScreenedDataModel>();
                            if (ScreenedDatas.Count >= 18)
                            {
                                //A1 - A8
                                for (int i = 0; i < 8; i++)
                                {
                                    ScreenedDataModel dataA1A8 = new ScreenedDataModel
                                    {
                                        Unit = ScreenedDatas[i].Unit,
                                        State = ScreenedDatas[i].State,
                                        WheelType = ScreenedDatas[i].WheelType,
                                        OnlineQuantity = ScreenedDatas[i].OnlineQuantity,
                                        TargetQuantity = ScreenedDatas[i].TargetQuantity
                                    };
                                    screenedDatas.Add(dataA1A8);
                                }
                                //A9
                                ScreenedDataModel dataA9 = new ScreenedDataModel
                                {
                                    Unit = ScreenedDatas[16].Unit,
                                    State = ScreenedDatas[16].State,
                                    WheelType = ScreenedDatas[16].WheelType,
                                    OnlineQuantity = ScreenedDatas[16].OnlineQuantity,
                                    TargetQuantity = ScreenedDatas[16].TargetQuantity
                                };
                                screenedDatas.Add(dataA9);

                                //B1-B8
                                for (int i = 15; i >= 8; i--)
                                {
                                    ScreenedDataModel dataB1B8 = new ScreenedDataModel
                                    {
                                        Unit = ScreenedDatas[i].Unit,
                                        State = ScreenedDatas[i].State,
                                        WheelType = ScreenedDatas[i].WheelType,
                                        OnlineQuantity = ScreenedDatas[i].OnlineQuantity,
                                        TargetQuantity = ScreenedDatas[i].TargetQuantity
                                    };
                                    screenedDatas.Add(dataB1B8);
                                }
                                //B9
                                ScreenedDataModel dataB9 = new ScreenedDataModel
                                {
                                    Unit = ScreenedDatas[17].Unit,
                                    State = ScreenedDatas[17].State,
                                    WheelType = ScreenedDatas[17].WheelType,
                                    OnlineQuantity = ScreenedDatas[17].OnlineQuantity,
                                    TargetQuantity = ScreenedDatas[17].TargetQuantity
                                };
                                screenedDatas.Add(dataB9);
                            }
                            //大屏分选数据显示
                            EventMessage.MessageHelper.GetEvent<ScreenedDataDisplayEvent>().Publish(screenedDatas);
                        }
                    }
                    else
                    {
                        List<ScreenedDataModel> screenedDatas = new List<ScreenedDataModel>();
                        EventMessage.MessageHelper.GetEvent<ScreenedDataDisplayEvent>().Publish(screenedDatas);
                    }
                    await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// 外部连接线程
        /// </summary>
        private void ExternalConnectionThread()
        {
            ExternalConnectionThreadControl = true;
            Task.Run(() =>
            {
                while (ExternalConnectionThreadControl)
                {
                    if (PlcCilent != null && !PlcCilent.Connected)
                    {
                        //连接PLC
                        int result = PlcCilent.ConnectTo(PlcIP, 0, 0);
                        if (result == 0 && PlcCilent.Connected)
                        {
                            if (PlcStatus != "1") PlcStatus = "1";
                            //复位再次识别使能信号
                            S7.SetBitAt(ref WriteBuffer, 12, 5, true);
                            HeartbeatThread();
                            PlcDataInteractionThread();
                        }
                        else
                        {
                            if (PlcDataInteractionControl)
                                PlcDataInteractionControl = false;
                            if (HeartbeatThreadControl)
                                HeartbeatThreadControl = false;
                            PlcCilent.Disconnect();
                            if (PlcStatus != "0") PlcStatus = "0";
                        }
                    }


                    //多相机连接
                    if (ExternalConnections.DatasCamera != null && ExternalConnections.DatasCamera.Count > 0)
                    {

                    }

                    if (CameraHandle == null || CameraHandle.Length == 0)
                    {
                        try
                        {
                            //连接相机
                            HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", CameraIdentifier, 0, -1, out CameraHandle);
                            HOperatorSet.GrabImageStart(CameraHandle, -1);
                            HOperatorSet.SetFramegrabberParam(CameraHandle, "TriggerMode", "Off");
                            if (CameraStatus != "1") CameraStatus = "1";
                        }
                        catch
                        {
                            if (CameraStatus != "0")
                            {
                                CameraStatus = "0";
                            }
                        }
                    }

                    if (PlcStatus == "1" && CameraStatus == "1") ExternalConnectionThreadControl = false;
                }
            });
        }

        /// <summary>
        /// PLC数据交互线程
        /// </summary>
        private void PlcDataInteractionThread()
        {
            PlcDataInteractionControl = true;
            Task.Run(async () =>
            {
                while (PlcDataInteractionControl)
                {
                    if (PlcCilent != null && PlcCilent.Connected)
                    {
                        //读PLC给视觉的数据
                        PlcCilent.DBRead(ReadDB, ReadStartAddress, ReadLenght, ReadBuffer);
                        //获取PLC轮毂到位信号
                        ArrivalSignal = S7.GetBitAt(ReadBuffer, 0, 0);
                        if (ArrivalSignal)
                        {
                            if (SignalInPlace != "1")
                                SignalInPlace = "1";
                        }
                        else
                        {
                            if (SignalInPlace != "2")
                                SignalInPlace = "2";
                        }
                        //      再次识别使能信号,               在识别中 
                        if (S7.GetBitAt(ReadBuffer, 0, 3) && IsIdentifying)
                        {
                            //复位识别中信号
                            IsIdentifying = false;
                            RecognitionStatus1 = "等待识别";
                            //复位再次识别使能信号
                            S7.SetBitAt(ref WriteBuffer, 12, 5, true);
                        }
                        //再次识别使能信号状态显示
                        if (S7.GetBitAt(ReadBuffer, 0, 3))
                        {
                            if (AgainRecognitionBackground != "0")
                            {
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    AgainRecognitionBackground = "0";
                                }));
                            }
                        }
                        else
                        {
                            var sss = AgainRecognitionBackground;
                            if (AgainRecognitionBackground != "1")
                            {
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    AgainRecognitionBackground = "1";
                                }));
                            }
                        }

                        //如果视觉拍照工位电机故障
                        if (!S7.GetBitAt(ReadBuffer, 0, 1) && !MotorFailureSignal)
                        {
                            MotorFailureSignal = true;
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                WMessageBox.Show("视觉拍照工位电机故障，请检查！", MessageType.Error);
                            }));
                        }
                        else
                        {
                            if (S7.GetBitAt(ReadBuffer, 0, 1)) MotorFailureSignal = false;
                        }
                        //   视觉策略完成信号                      识别完成
                        if (S7.GetBitAt(ReadBuffer, 0, 2) && S7.GetBitAt(WriteBuffer, 12, 2))
                        {
                            //复位识别完成信号
                            S7.SetBitAt(ref WriteBuffer, 12, 2, false);
                            //清空轮型区域
                            Array.Clear(WriteBuffer, 0, 10);
                        }
                        //写给PLC的视觉系统数据
                        PlcCilent.DBWrite(WriteDB, WriteStartAddress, WriteLenght, WriteBuffer);

                        if (IsScreenedResult)
                        {
                            //读PLC的大屏分选数据
                            PlcCilent.DBRead(BigScreenDataDB, BigScreenDataStartByte, BigScreenDataLength, BigScreenDataBuffer);
                        }

                    }
                    else
                    {
                        PlcCilent.Disconnect();
                        if (!ExternalConnectionThreadControl)
                        {
                            ExternalConnectionThreadControl = true;
                            ExternalConnectionThread();
                        }
                    }


                    await Task.Delay(10);
                }
            });
        }

        

        /// <summary>
        /// 执行数据更新
        /// </summary>
        private void PerformDataUpdates()
        {
            Task.Run(async () =>
            {
                UpdatedDay = DateTime.Now.Day;
                SqlAccess.SystemDatasWrite("UpdatedDay", UpdatedDay.ToString());
                try
                {
                    #region======模板动态调整======
                    var sDB = new SqlAccess().SystemDataAccess;
                    //第1步：读取活跃轮型数据库中的所有轮型，并清空活跃轮型数据库
                    List<string> activeWheels = sDB.Queryable<ActiveWheelTypeDataModel>().Select(x => x.WheelType).ToList();
                    sDB.DbMaintenance.TruncateTable<ActiveWheelTypeDataModel>();
                    TodayWheels.Clear();
                    List<TemplateDataModel> datas = sDB.Queryable<TemplateDataModel>().ToList();
                    if (datas.Count > 0)
                    {
                        //第2步：获取模板数据库中所有数据，并将所有轮型的未用天数+1
                        foreach (var data in datas)
                        {
                            data.UnusedDays += 1;
                        }
                        //第3步：将模板数据中的轮型与活跃数据中的轮型比较，相等则将对应不活跃天数清零
                        foreach (var wheelType in activeWheels)
                        {
                            int index = datas.FindIndex(x => x.WheelType == wheelType);
                            if (index != -1) datas[index].UnusedDays = 0;
                        }
                        //第4步：根据设定的不活跃天数调整模板
                        foreach (var data in datas)
                        {
                            if (data.UnusedDays > TemplateAdjustDays)
                            {
                                string activePath = ActiveTemplatesPath + @"\" + data.WheelType + ".ncm";//活跃模板路径
                                string notActivePath = NotActiveTemplatesPath + @"\" + data.WheelType + ".ncm";//不活跃模板路径
                                if (File.Exists(activePath))
                                {
                                    File.Move(activePath, notActivePath);
                                    int index = TemplateDataCollection.ActiveTemplateNames.FindIndex(x => x == data.WheelType);
                                    if (index != -1)
                                    {
                                        TemplateDataCollection.NotActiveTemplateNames.Add(TemplateDataCollection.ActiveTemplateNames[index]);
                                        TemplateDataCollection.NotActiveTemplates.Add(TemplateDataCollection.ActiveTemplates[index]);
                                        TemplateDataCollection.ActiveTemplateNames.RemoveAt(index);
                                        TemplateDataCollection.ActiveTemplates.RemoveAt(index);
                                    }
                                    EventMessage.MessageDisplay("模板动态调整，移动的模板是：" + data.WheelType, true, true);
                                }
                            }
                        }
                        //第5步：更新模板数据库，并重新读取所有模板到内存中
                        sDB.DbMaintenance.TruncateTable<TemplateDataModel>();
                        sDB.Insertable(datas).ExecuteCommand();
                        AutoTemplateDataLoadControl = true;
                        EventMessage.MessageHelper.GetEvent<TemplateDataUpdataEvent>().Publish("");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    EventMessage.MessageDisplay("模板动态调整异常：" + ex.Message, true, true);
                }
                try
                {
                    #region======删除图像数据======
                    //获取删除开始的时间
                    DateTime startDateTime = DateTime.Now.AddDays(-SaveImageDays);
                    //从开始时间往前删30天
                    for (int i = 1; i <= 30; i++)
                    {
                        DateTime currentTime = startDateTime.AddDays(-i);
                        string path = HistoricalImagesPath + @"\" + currentTime.Month + "月" + @"\" + currentTime.Day + "日";
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                            EventMessage.MessageDisplay("已自动删除" + currentTime.Month + "月" + currentTime.Day + "日的图片文件！", true, true);
                        }
                    }
                    //删除当月以前的月文件夹
                    if (DateTime.Now.Day >= SaveImageDays)
                    {
                        //从开始时间往前删12个月
                        for (int i = 1; i < 12; i++)
                        {
                            DateTime currentMonth = DateTime.Now.AddMonths(-i);
                            string path = HistoricalImagesPath + @"\" + currentMonth.Month + "月";
                            if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                                await Task.Delay(2000);
                                EventMessage.MessageDisplay("已自动删除" + currentMonth.Month + "月" + "的图片文件夹！", true, true);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    EventMessage.MessageDisplay("自动删除图像异常：" + ex.Message, false, true);
                }
                try
                {
                    #region======删除识别数据======
                    DateTime delTime = DateTime.Now.AddDays(-SaveDataMonths * 30);
                    var pDB = new SqlAccess().ProductionDataAccess;
                    int minIndex = pDB.Queryable<ProductionDataModel>().Min(x => x.Index);
                    var startData = pDB.Queryable<ProductionDataModel>().First(x => x.Index == minIndex);
                    //如果设定的删除时间大于表内最早保存数据的时间，说明有数据达到删除条件，执行删除
                    if (startData != null && startData.RecognitionTime < delTime)
                    {
                        var ds = pDB.Queryable<ProductionDataModel>().Where(x => SqlFunc.Between(x.RecognitionTime, startData.RecognitionTime, delTime)).ToList();
                        foreach (var d in ds)
                        {
                            pDB.Deleteable<ProductionDataModel>().Where(x => x.Index == d.Index).ExecuteCommand();
                        }
                        EventMessage.MessageDisplay($"已自动删除{SaveDataMonths}个月之前的识别数据", true, true);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    EventMessage.MessageDisplay("自动删除识别数据异常：" + ex.Message, false, true);
                }
                DataBeingUpdated = false;
            });
        }

        /// <summary>
        /// 分选结果,false不需分选，true需要分选
        /// </summary>
        /// <param name="wheel">轮型</param>
        /// <param name="datas">分选数据</param>
        /// <returns>false不需分选，true需要分选</returns>
        private bool ScreenedResult(string wheel, List<ScreenedDataModel> datas)
        {
            var result = datas.FindAll(x => x.WheelType == wheel);
            if (result != null && result.Count > 0)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < result.Count; i++)
                {
                    //如果单元在线数量 大于等于 单元目标数量，则分选 并且 如果单元状态为false（停机或检修），则分选
                    if (result[i].OnlineQuantity >= result[i].TargetQuantity || !result[i].State) list.Add(1);
                    else list.Add(0);
                }
                var index = list.FindIndex(t => t == 0);
                if (index >= 0) return false;
                else return true;
            }
            else return true;
        }

        /// <summary>
        /// 保存图像数据
        /// </summary>
        /// <param name="saveImage">需要保存的图像</param>
        /// <param name="data">识别数据</param>
        /// <param name="dateTime">识别的时间</param>
        /// <param name="gateResult">浇口检测结果</param>
        private void SaveImageDatas(HObject saveImage, ProductionDataModel data, DateTime dateTime, string gateResult)
        {
            //月文件夹路径
            string monthPath = HistoricalImagesPath + @"\" + dateTime.Month + "月";
            //日文件夹路径
            string dayPath = HistoricalImagesPath + @"\" + dateTime.Month + @"月\" + dateTime.Day + "日";
            //当日未识别文件夹路径
            string ngPath = HistoricalImagesPath + @"\" + dateTime.Month + @"月\" + dateTime.Day + @"日\NG";
            //当日浇口检测失败文件夹路径
            string gateNGPath = HistoricalImagesPath + @"\" + dateTime.Month + @"月\" + dateTime.Day + @"日\GateNG";
            if (Directory.Exists(monthPath) == false) Directory.CreateDirectory(monthPath);
            if (Directory.Exists(dayPath) == false) Directory.CreateDirectory(dayPath);
            if (Directory.Exists(ngPath) == false) Directory.CreateDirectory(ngPath);
            if (Directory.Exists(gateNGPath) == false) Directory.CreateDirectory(gateNGPath);
            var diskFree = GetHardDiskFreeSpace("D");//获取D盘剩余空间
            if (diskFree > 200)
            {
                if (data.WheelType != "NG")
                {
                    //保存轮型的目录
                    string saveWheelTypePath = dayPath + @"\" + data.WheelType.Trim('_');
                    if (Directory.Exists(saveWheelTypePath) == false) Directory.CreateDirectory(saveWheelTypePath);
                    string saveImagePath = saveWheelTypePath.Replace(@"\", "/") + "/" + data.WheelType + "&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
                }
                else
                {
                    string saveImagePath = ngPath.Replace(@"\", "/") + "/NG" + "&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveImagePath);
                }
                if (gateResult == "NG")
                {
                    string saveGateNGImagePath = gateNGPath.Replace(@"\", "/") + "/GateNG&" + data.WheelType + "&" + dateTime.ToString("yyMMddHHmmss") + ".tif";
                    HOperatorSet.WriteImage(saveImage, "tiff", 0, saveGateNGImagePath);
                }
            }
            else EventMessage.MessageDisplay("磁盘存储空间不足，请检查！", true, false);
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
    }
}
