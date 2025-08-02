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
using System.Threading;
using System.Net.Http;
using Prism.Services.Dialogs;
using Microsoft.Win32;
using System.Windows.Input;
using static NPOI.HSSF.Util.HSSFColor;
using System.Collections.ObjectModel;
using WheelRecognitionSystem.Views.Pages;
using Prism.Events;
using System.Diagnostics;
using NPOI.OpenXmlFormats.Vml;
using System.IO.Pipes;


namespace WheelRecognitionSystem.ViewModels
{
    public class MainViewModel : BindableBase
    {


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

        public ObservableCollection<DisplayData> _displayCollections;
        /// <summary>
        /// 显示相机处理结果数据集合
        /// </summary>
        public ObservableCollection<DisplayData> DisplayCollections
        {
            get { return _displayCollections; }
            set { SetProperty(ref _displayCollections, value); }
        }

        #endregion



        /// <summary>
        /// 顶部 系统设置命令
        /// </summary>
        public DelegateCommand<string> ClickCommand { get; set; }
        public DelegateCommand<string> TestCommand { get; set; }
        public DelegateCommand RefreshParaCommand { get; set; }
        public DelegateCommand RefreshNCCCommand { get; set; }


        private S7Client PlcCilent;
        /// <summary>
        /// 读缓冲区
        /// </summary>
        private byte[] _readBuffer = new byte[1400];
        /// <summary>
        /// 写入缓冲区
        /// </summary>
        private byte[] WriteBuffer = new byte[90];

        /// <summary>
        /// 读取PLC信号数据信息组
        /// </summary>
        private ReadPLCSignal[] readPLCSignals = new ReadPLCSignal[5];

        /// <summary>
        /// 信息显示定时器
        /// </summary>
        private DispatcherTimer MessageShowTimer;

        /// <summary>
        /// 图片删除定时器
        /// </summary>
        private DispatcherTimer pictrueDeleteTimer;

        /// <summary>
        /// 回流状态上次更新的二维码
        /// </summary>
        private string lastUpdateCodeBack;
        private IRegionManager _regionManager;
        readonly IDialogService _dialogService;
        private WorkingPicture workingPicture;
        private int triggerCount = 0;
        private int count = 0;



        public MainViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;
            LoadSystemDatas();

            ViewInitialization();
            DataInitialization();
            CommandBinding();
            EventSubscribe();

            HeartbeatThread();
            PlcDataInteractionThread();
            //PictrueDeleteTimer_Tick(null, null);
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInitialization()
        {
            _regionManager.RegisterViewWithRegion("ViewRegion", typeof(DisplayInterfaceView));
            //_regionManager.RegisterViewWithRegion("ViewRegion1", typeof(ReportManagementView));
            //_regionManager.RegisterViewWithRegion("ViewRegion2", typeof(ReportManagementView));
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        public void DataInitialization()
        {
            //PLC
            PlcCilent = new S7Client();
            //初始化数据库
            SqlAccess sqlAccess = new SqlAccess();
            sqlAccess.InitializeTable();
            workingPicture = WorkingPicture.Instance;
            workingPicture.Initialize();
            //实例化数据组
            for (int i = 0; i < readPLCSignals.Length; i++)
            {
                readPLCSignals[i] = new ReadPLCSignal();
                readPLCSignals[i].ArrivalSignalTriggered += OnArrivalSignalTriggered;
            }

            List<DisplayData> displayDatas = new List<DisplayData>();
            for (int i = 0; i < 5; i++)
            {
                DisplayData data = new DisplayData()
                {
                    Station = "1检1",
                    Status = "",
                    WheelType = "",
                    Similarity = 0M,
                    TimeConsumed = null,
                    Remark = ""
                };
                displayDatas.Add(data);
            }
            DisplayCollections = new ObservableCollection<DisplayData>(displayDatas);
            //启动定时器
            pictrueDeleteTimer = new DispatcherTimer();
            pictrueDeleteTimer.Tick += new EventHandler(PictrueDeleteTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            pictrueDeleteTimer.Interval = new TimeSpan(12, 0, 0);//设置时间间隔为1秒
            pictrueDeleteTimer.Start();//启动定时器

            // 管道名称（客户端和服务端必须一致）
            string pipeName = "MyPipe";

            // 在单独的线程中启动服务器，避免阻塞主线程
            Thread serverThread = new Thread(() => StartServer(pipeName));
            serverThread.IsBackground = true;
            serverThread.Start();

        }
        /// <summary>
        /// 事件绑定
        /// </summary>
        public void CommandBinding()
        {
            ClickCommand = new DelegateCommand<string>(ClickManage);
            TestCommand = new DelegateCommand<string>(Test);
            RefreshParaCommand = new DelegateCommand(InitialPara);
            RefreshNCCCommand = new DelegateCommand(RefreshNCC);
        }



        /// <summary>
        /// 测试使用
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void Test(string obj)
        {
            string[] files = Directory.GetFiles("D:\\ZS\\终检\\测试图");
            //5.处理每个文件
            foreach (string filePath in files)
            {
                try
                {
                    await Task.Delay(1000);

                    HOperatorSet.ReadImage(out HObject image, filePath);
                    HObject gray = RGBTransGray(image);

                    DisplayDataGrid(0, new DisplayData()); //清空显示
                    InteractS7PLCModel interact = new InteractS7PLCModel()
                    {
                        ArrivalDelay = ArrivalDelay,
                        readPLCSignal = new ReadPLCSignal() { Index = 0, Name = "1检1" },
                    };
                    Dictionary<InteractS7PLCModel, HObject> dic = new Dictionary<InteractS7PLCModel, HObject>();
                    dic.Add(interact, gray);
                    workingPicture.ImageHandle(dic);


                    //AutoRecognitionResultDisplayModel displayModel = new AutoRecognitionResultDisplayModel();


                    //displayModel.Tag = $"DisplayRegion1";
                    //displayModel.CurrentImage = CloneImageSafely(image);

                    //EventMessage.MessageHelper.GetEvent<RecognitionDisplayEvent>().Publish(displayModel);


                    //SafeHalconDispose(displayModel);
                    //SafeHalconDispose(gray);
                    SafeHalconDispose(image);






                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  !! 处理文件 {filePath} 时出错: {ex.Message}");
                }
            }


        }

        /// <summary>
        /// 事件订阅
        /// </summary>
        public void EventSubscribe()
        {
            //订阅消息
            EventMessage.MessageHelper.GetEvent<SystemMessageDisplayEvent>().Subscribe(SystemMessageDisplay);
            EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Subscribe(CallShow, ThreadOption.UIThread, // 确保在UI线程执行
                            keepSubscriberReferenceAlive: true);
        }



        /// <summary>
        /// 加载系统数据
        /// </summary>
        private void LoadSystemDatas()
        {
            try
            {
                var sDB = new SqlAccess().SystemDataAccess;
                List<sys_bd_systemsettingsdatamodel> systemDatas = sDB.Queryable<sys_bd_systemsettingsdatamodel>().ToList();
                sDB.Close(); sDB.Dispose();
                //PLC参数
                PlcIP = GetPara(systemDatas, "PlcIP", "192.168.0.188");
                ReadDB = int.Parse(GetPara(systemDatas, "ReadDB", "100"));
                ReadStartAddress = int.Parse(GetPara(systemDatas, "ReadStartAddress", "0"));
                ReadLenght = int.Parse(GetPara(systemDatas, "ReadLenght", "394"));
                WriteDB = int.Parse(GetPara(systemDatas, "WriteDB", "101"));
                WriteStartAddress = int.Parse(GetPara(systemDatas, "WriteStartAddress", "0"));
                WriteLenght = int.Parse(GetPara(systemDatas, "WriteLenght", "146"));

                //图像参数
                SaveImageDays = int.Parse(systemDatas.First(x => x.Name == "SaveImageDays").Value);
                TemplateSoftwarePath = GetPara(systemDatas, "TemplateSoftwarePath", "D:\\My\\Software");
                SQLManageSoftwarePath = GetPara(systemDatas, "SQLManageSoftwarePath", "D:\\My\\Software");
                //不需要修改
                TemplateImagesPath = GetPara(systemDatas, "TemplateImagesPath", "D:\\VisualDatas\\TemplateImages");
                ActiveTemplatesPath = GetPara(systemDatas, "ActiveTemplatesPath", "D:\\VisualDatas\\ActiveTemplate");
                HistoricalImagesPath = GetPara(systemDatas, "HistoricalImagesPath", "D:\\VisualDatas\\HistoricalImages");
                DeepParaPath = GetPara(systemDatas, "DeepParaPath", @"D:\VisualDatas\大模型文件");


                //上传参数
                UpMesUri = GetPara(systemDatas, "UpMesUri", "http://192.168.0.101/vboard/boardGrid");
                WriteBuffer = new byte[WriteLenght - WriteStartAddress];

                InitialPara();

                EventMessage.MessageDisplay("系统数据加载完成。", true, false);
            }
            catch (Exception ex)
            {
                EventMessage.MessageDisplay("系统数据加载错误：" + ex.Message, true, true);
            }
        }

        /// <summary>
        /// 初始化外部控制参数
        /// </summary>
        private void InitialPara()
        {
            var sDB = new SqlAccess().SystemDataAccess;
            List<sys_bd_systemsettingsdatamodel> systemDatas = sDB.Queryable<sys_bd_systemsettingsdatamodel>().ToList();
            sDB.Close(); sDB.Dispose();

            //延迟参数
            ArrivalDelay = int.Parse(GetPara(systemDatas, "ArrivalDelay", "10"));

            //定位
            WheelMinThreshold = int.Parse(GetPara(systemDatas, "WheelMinThreshold", "22"));
            WheelMinRadius = int.Parse(GetPara(systemDatas, "WheelMinRadius", "120"));
            //制作模板时的参数 
            WindowMaxThreshold = int.Parse(GetPara(systemDatas, "WindowMaxThreshold", "100"));
            RemoveMixArea = double.Parse(GetPara(systemDatas, "RemoveMixArea", "60"));
            TemplateStartAngle = double.Parse(GetPara(systemDatas, "TemplateStartAngle", "3.1415"));
            TemplateEndAngle = double.Parse(GetPara(systemDatas, "TemplateEndAngle", "4.7123"));
            //匹配时需要的参数
            MinFullFigureGary = double.Parse(GetPara(systemDatas, "MinFullFigureGary", "45"));
            ConfidenceMatch = double.Parse(GetPara(systemDatas, "ConfidenceMatch", "0.8"));
            MinSimilarity = double.Parse(systemDatas.First(x => x.Name == "MinSimilarity").Value);

            //匹配时的参数 一般不需要修改下列参数
            AngleStart = double.Parse(GetPara(systemDatas, "AngleStart", "-1.57"));
            AngleExtent = double.Parse(GetPara(systemDatas, "AngleExtent", "1.57"));


            EventMessage.MessageDisplay($"参数加载成功！", true, true);

        }


        void StartServer(string pipeName)
        {
            while (true)
            {
                try
                {
                    using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(
                        pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous))
                    {
                        // 等待客户端连接
                        Console.WriteLine("等待客户端连接...");
                        pipeServer.WaitForConnection();
                        Console.WriteLine("客户端已连接！");

                        // 使用 StreamReader 读取客户端发送的字符串
                        using (StreamReader reader = new StreamReader(pipeServer))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Console.WriteLine("接收到客户端消息: " + line);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("发生错误: " + ex.Message);
                }
            }
        }

        private void RefreshNCC()
        {
            //推送到工作类中刷新
            EventMessage.MessageHelper.GetEvent<RefreshNCCParaEvent>().Publish();
        }

        /// <summary>
        ///获取参数 - 没有此参数插入数据库默认值
        /// </summary>
        /// <param name="systemDatas">数据源</param>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private string GetPara(List<sys_bd_systemsettingsdatamodel> systemDatas, string name, string defaultValue)
        {
            try
            {
                string value = systemDatas.First(x => x.Name == name).Value;
                return value;
            }
            catch (Exception ex)
            {
                SqlAccess.SystemDatasInsertable(name, defaultValue);
                return defaultValue;
            }

        }

        /// <summary>
        /// 心跳线程
        /// </summary>
        private void HeartbeatThread()
        {
            HeartbeatThreadControl = true;
            bool heartBool = true;
            Task.Run(async () =>
            {
                while (HeartbeatThreadControl)
                {
                    if (PlcCilent != null && PlcCilent.Connected)
                    {
                        //写心跳数据
                        S7.SetBitAt(ref WriteBuffer, 0, 5, heartBool);
                        heartBool = !heartBool;
                    }

                    await Task.Delay(500);
                }
            });
        }



        /// <summary>
        /// PLC数据交互线程
        /// </summary>
        private void PlcDataInteractionThread()
        {

            Thread.Sleep(500);
            Console.WriteLine("进来次数");
            readPLCSignals[0].Name = "1检1";
            readPLCSignals[0].Index = 0;
            readPLCSignals[1].Name = "1检2A_B";
            readPLCSignals[1].Index = 1;
            readPLCSignals[2].Name = "1检3";
            readPLCSignals[2].Index = 2;
            readPLCSignals[3].Name = "1检3返修";
            readPLCSignals[3].Index = 3;
            readPLCSignals[4].Name = "2检1";
            readPLCSignals[4].Index = 4;
            _readBuffer = new byte[ReadLenght - ReadStartAddress + 1];
            float temperature;
            KeyValuePair<string, string> modifiValue = new KeyValuePair<string, string>();
            KeyValuePair<string, int> wheel;
            Task.Run(async () =>
            {
                while (PlcDataInteractionControl)
                {

                    if (PlcCilent != null && !PlcCilent.Connected) //未连接
                    {
                        //连接PLC
                        int result = PlcCilent.ConnectTo(PlcIP, 0, 0);
                        //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")}:连接PLC");
                        if (result == 0 && PlcCilent.Connected)
                        {
                            if (PlcStatus != "1")
                                PlcStatus = "1";

                        }
                        else
                        {
                            //Console.WriteLine($"连接失败：{PlcCilent.Connected}");
                            if (PlcStatus != "0")
                                PlcStatus = "0";
                        }
                        Thread.Sleep(2000);

                    }


                    //1.相机拍照信号 分为五个相机触发信号  1检1 1检2A和2B共用一个相机  1检3 一检3返修  2检1
                    //5个相机 5个触摸屏
                    Thread.Sleep(100);
                    if (PlcCilent != null && PlcCilent.Connected)
                    {
                        try
                        {
                            // 重用缓冲区而不是重新创建
                            Array.Clear(_readBuffer, 0, _readBuffer.Length);
                            //_readBuffer = new byte[ReadLenght - ReadStartAddress + 1];
                            if (_readBuffer == null)
                                Console.WriteLine("缓冲区未初始化");

                            //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 扫描周期");
                            int bytes_read = PlcCilent.DBRead(ReadDB, ReadStartAddress, ReadLenght, _readBuffer);
                            if (bytes_read != 0)
                            {
                                Console.WriteLine($"实际 {_readBuffer.Length} 字节");
                                //读取失败
                                Console.WriteLine($"错误码: {bytes_read}, 描述: {PlcCilent.ErrorText(bytes_read)}");

                            }
                            else
                            {

                                //轮毂温度 3条线
                                //Console.WriteLine( $"轮毂温度：{S7.GetRealAt(_readBuffer, 124)}");
                                readPLCSignals[0].WheelTemperature = S7.GetRealAt(_readBuffer, 124);
                                temperature = S7.GetRealAt(_readBuffer, 128);
                                readPLCSignals[1].WheelTemperature = temperature; //1检2A 1检2B
                                //readPLCSignals[2].WheelTemperature = temperature; 
                                readPLCSignals[2].WheelTemperature = S7.GetRealAt(_readBuffer, 132); //1检3
                                //计数
                                count = S7.GetIntAt(_readBuffer, 156);

                                for (int i = 0; i < 5; i++)
                                {

                                    //轮毂高度
                                    readPLCSignals[i].WheelHeight = S7.GetRealAt(_readBuffer, 136 + i * 4);

                                    //1.相机拍照部分                                 
                                    bool photo = S7.GetBitAt(_readBuffer, 108, i);
                                    readPLCSignals[i].ArrivalSignal = photo;
                                    if (photo)
                                    {
                                        EventMessage.MessageDisplay($"接收到拍照信号：{readPLCSignals[i].Name} （108.{i}.True）", true, true);
                                        //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 接收到拍照信号108.{i}：{photo}");
                                        S7.SetBitAt(ref WriteBuffer, 143, i, true); //回复读取拍照成功
                                        EventMessage.MessageDisplay($"回复读取拍照成功：{readPLCSignals[i].Name} （143.{i}.True）", true, true);
                                        ResetSignal(143, i, 600); // 拍照信号复位                                 
                                    }

                                    //轮形编码 - PLC传输过来的 mmss_轮形号 用于修改数据
                                    string back_WheelCoding = GetBytesToString(_readBuffer, 314 + i * 16);
                                    string NG_WheelCoding = GetBytesToString(_readBuffer, 2 + i * 16);
                                    Thread.Sleep(10);
                                    // 2. 回流状态处理
                                    int backBit = i + 1;
                                    bool back = S7.GetBitAt(_readBuffer, 192, backBit);
                                    bool FlowOrDown = S7.GetBitAt(_readBuffer, 192, 0); //1回流 0下转
                                    string showStatus = FlowOrDown ? "回流" : "下转";
                                    if (back)
                                    {
                                        EventMessage.MessageDisplay($"接收到回流信号：（92.{i + 1}） 轮形：{back_WheelCoding}-{showStatus}", true, true);
                                        Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 回流信号192.{i + 1}：{back}");
                                        modifiValue = new KeyValuePair<string, string>(back_WheelCoding, showStatus);
                                        int indexPos = 144;
                                        int indexBit = i + 5;
                                        if (i >= 8)
                                        {
                                            indexPos = indexPos + 1;
                                            indexBit = indexBit - 8;
                                        }

                                        S7.SetBitAt(ref WriteBuffer, indexPos, indexBit, true); //回复读取回流状态成功
                                        EventMessage.MessageDisplay($"回复读取回流成功：（{indexPos}.{indexBit}）", true, true);
                                        //PlcCilent.DBWrite(WriteDB, WriteStartAddress, WriteLenght, WriteBuffer);
                                        //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 回复读取回流状态成功：{indexPos}.{indexBit}");
                                        //0 1检1  1 
                                        await OnDataModifiNextStationAsync(modifiValue);
                                        ResetSignal(indexPos, indexBit, 600); // 回流状态复位                                 




                                    }

                                    Thread.Sleep(10);
                                    // 3. 产品NG处理
                                    int wheelDefect = BitConverter.ToInt16(new byte[] { _readBuffer[99 + i * 2], _readBuffer[98 + i * 2] }, 0);
                                    wheel = new KeyValuePair<string, int>(NG_WheelCoding, wheelDefect);
                                    //数据修正信号
                                    bool b = S7.GetBitAt(_readBuffer, 193, i);
                                    if (b)
                                    {
                                        EventMessage.MessageDisplay($"接收到修正信号：（193.{i}） 轮形：{NG_WheelCoding}-{wheelDefect}", true, true);

                                        S7.SetBitAt(ref WriteBuffer, 144, i, true); //回复读取修正信号成功
                                        //PlcCilent.DBWrite(WriteDB, WriteStartAddress, WriteLenght, WriteBuffer);
                                        OnDataModificationTriggered(wheel);
                                        EventMessage.MessageDisplay($"回复读取修正成功：（144.{i}）", true, true);
                                        ResetSignal(144, i, 600); // 修正信号复位                                 

                                        //new Thread((obj) =>
                                        //{
                                        //    int threadI = (int)obj;  // 将 object 类型转为 int
                                        //    Thread.Sleep(500);
                                        //    S7.SetBitAt(ref WriteBuffer, 144, threadI, false); //复位读取成功
                                        //    EventMessage.MessageDisplay($"复位{144}.{threadI}回复读取修正信号成功", true, true);
                                        //}).Start(i);
                                    }

                                    //读取登录信号
                                    S7.SetBitAt(ref WriteBuffer, 141, i, false); //复位信号
                                    S7.SetBitAt(ref WriteBuffer, 142, i, false); //复位信号
                                                                                 //Console.WriteLine($"完成信号{i}：{S7.GetBitAt(WriteBuffer, 141, i)}");
                                    bool loginTrigger = S7.GetBitAt(_readBuffer, 191, i);
                                    string name = GetBytesToString(_readBuffer, 194 + i * 12).Replace("\0", "");
                                    string password = GetBytesToString(_readBuffer, 254 + i * 12).Replace("\0", "");
                                    if (loginTrigger)
                                    {
                                        bool isLogin = LoginCheck(name, password);
                                        if (isLogin)
                                            S7.SetBitAt(ref WriteBuffer, 142, i, true);

                                        S7.SetBitAt(ref WriteBuffer, 141, i, true);
                                        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff")}-loginIndex:{i},ID:{name},pass:{password},Result: {isLogin}");
                                    }

                                    ////发送PLC的数据
                                    //string prefix = DateTime.Now.ToString("ddss");
                                    //string text = prefix + "-" + "08124C05__".Trim('_');
                                    //int maxLength = 16;          // PLC 中定义的最大长度
                                    //                             // 转换字符串为 PLC 格式字节数组
                                    //byte[] buffer = StringToS7Bytes(text, maxLength);
                                    //CopyBytes(buffer, WriteBuffer, 10 + (1 - 1) * 16);
                                    //写给PLC的视觉系统数据
                                    PlcCilent.DBWrite(WriteDB, WriteStartAddress, WriteLenght, WriteBuffer);
                                    Thread.Sleep(10);
                                    //PlcCilent.DBRead(WriteDB, WriteStartAddress, WriteLenght, WriteBuffer);
                                    //string value = GetBytesToString(WriteBuffer, 10, 14).Trim();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"PlcDataInteractionControl:{ex.ToString()}");
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"PLC是空的或者没链接");
                        //PlcCilent.Disconnect();
                        //if (!ExternalConnectionThreadControl)
                        //{
                        //    ExternalConnectionThreadControl = true;
                        //    ExternalConnectionThread();
                        //}
                    }

                    await Task.Delay(10);
                }
            });
        }


        private void ResetSignal(int bytePos, int bitPos, int delayMs)
        {

            new Thread(() =>
            {
                {
                    Thread.Sleep(600);
                    S7.SetBitAt(ref WriteBuffer, bytePos, bitPos, false);
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 复位{bytePos}.{bitPos}成功");
                }
            }).Start();
        }

        /// <summary>
        /// 轮毂到位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArrivalSignalTriggered(object sender, EventArgs e)
        {
            try
            {

                if (triggerCount > 10000)
                {
                    triggerCount = 0;
                }

                triggerCount++;

                if (triggerCount % 100 == 0)
                {
                    WriteLogToFile(triggerCount, count);
                }


                ReadPLCSignal plcSignal = sender as ReadPLCSignal;
                int n = plcSignal.Index;
                // 在后台线程中修改集合时：
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayDataGrid(n, new DisplayData() { Similarity = 0M }); //清空显示
                });

                workingPicture.ReceiveS7PLC(new InteractS7PLCModel()
                {
                    ArrivalDelay = ArrivalDelay,
                    readPLCSignal = readPLCSignals[plcSignal.Index]
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoJob:{ex.ToString()}");
            }
        }

        /// <summary>
        /// 结果处理&回复PLC
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CallShow(InteractS7PLCModel model)
        {
            int index = model.readPLCSignal.Index;
            S7.SetBitAt(ref WriteBuffer, 0, index, true); //拍照流程完成
            //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 拍照流程完成: 0.{model.Index - 1} true");
            new Thread((obj) =>
            {
                int threadI = (int)obj;  // 将 object 类型转为 int
                Thread.Sleep(500);
                S7.SetBitAt(ref WriteBuffer, 0, threadI, false); //复位读取成功
                //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 复位{0}.{threadI}拍照流程完成信号");
            }).Start(index);



            //发送PLC的数据
            string prefix = DateTime.Now.ToString("mmss");
            string text;
            // PLC 中定义的最大长度
            // 转换字符串为 PLC 格式字节数组
            string recognType = model.resultModel.RecognitionWheelType;
            string wheelType = string.Empty;
            if (!model.resultModel.ResultBol || recognType == null || recognType == "NG")
            {
                text = prefix + DateTime.Now.ToString("yyMMddHH");
                recognType = text;
                wheelType = "error";
            }
            else
            {
                wheelType = recognType.Trim('_');
                text = prefix + wheelType;
            }
            int maxLength = 16;
            byte[] buffer = StringToS7Bytes(text, maxLength);
            CopyBytes(buffer, WriteBuffer, 10 + index * 16);
            string similarity = model.resultModel.Similarity == 0 ? "" : model.resultModel.Similarity.ToString();
            string timeConsumed = model.Interval.TotalMilliseconds == 0 ? "" : model.Interval.TotalMilliseconds.ToString();
            //显示状态信息
            decimal.TryParse(similarity, out decimal decSimilarity);
            float.TryParse(timeConsumed, out float fTimeConsumed);
            DisplayData data = new DisplayData();
            data.Station = model.readPLCSignal.Name;
            data.Status = model.resultModel.status;
            data.WheelType = wheelType;
            data.Similarity = decSimilarity * 100;
            data.TimeConsumed = fTimeConsumed;
            data.Remark = $"{model.resultModel.Way}:{decSimilarity}";
            DisplayDataGrid(index, data);
            //EventMessage.MessageDisplay($"拍照流程完成:{model.Index}：下标：{model.Index - 1}", true, true);

            EventMessage.MessageDisplay($"{model.readPLCSignal.Name} - 型号:{wheelType} - {model.resultModel.status}", true, false);


            //插入数据库  -跟着相机工位走的数据
            SqlSugarClient pDB = new SqlAccess().SystemDataAccess;
            Tbl_productiondatamodel dataModel = new Tbl_productiondatamodel();
            dataModel.GUID = Guid.NewGuid().ToString("N");
            dataModel.WheelType = recognType;
            dataModel.TimeConsumed = model.Interval.TotalMilliseconds.ToString();
            dataModel.Similarity = model.resultModel.Similarity.ToString();
            dataModel.WheelHeight = model.readPLCSignal.WheelHeight;
            dataModel.WheelTemperature = model.readPLCSignal.WheelTemperature;
            dataModel.WheelStyle = model.resultModel.WheelStyle;
            dataModel.RecognitionTime = model.endTime;
            dataModel.TransmissionCoding = text;
            dataModel.Model = wheelType;
            dataModel.Station = model.readPLCSignal.Name;
            dataModel.ImagePath = model.imagePath;
            dataModel.ReportWay = "线上";
            dataModel.ResultBool = model.resultModel.ResultBol;
            dataModel.Remark = "-1";
            dataModel.RecognitionDay = model.endTime;
            pDB.Insertable(dataModel).ExecuteCommand();

            pDB.Close();
            pDB.Dispose();
            model = null;

        }


        /// <summary>
        /// 修改产品流向 下转/回流
        /// </summary>
        /// <param name="keyValue"></param>

        private async Task OnDataModifiNextStationAsync(KeyValuePair<string, string> keyValue)
        {
            SqlSugarClient db = new SqlAccess().SystemDataAccess;
            try
            {

                string prefix_WheelCoding = keyValue.Key;
                string station = keyValue.Value;
                char[] parts = prefix_WheelCoding.ToCharArray();
                if (prefix_WheelCoding == lastUpdateCodeBack)
                {
                    EventMessage.MessageDisplay($"两次状态码一致：{lastUpdateCodeBack}", true, true);
                    return;
                }
                if (parts.Count() != 12)
                {
                    throw new Exception($"NextStation-Prefix_WheelCoding数据长度错误：{parts.Count()}");
                }
                lastUpdateCodeBack = prefix_WheelCoding;



                // 步骤1：查询符合条件的最新一条记录
                Tbl_productiondatamodel latestRecord = db.Queryable<Tbl_productiondatamodel>()
                    .Where(x => x.TransmissionCoding == prefix_WheelCoding)
                    .OrderByDescending(x => x.ID)
                    .First();

                if (latestRecord != null)
                {
                    //上传mes
                    await SendMes(UpMesUri, "1检1", latestRecord.GUID);
                    //await SendMes(UpMesUri, "1检1", "97624f8807e14e89b95b653e03b2c9e9");
                    // 步骤2：更新 Result 和 Code
                    var rowsAffected = db.Updateable<Tbl_productiondatamodel>()
                        .SetColumns(it => new Tbl_productiondatamodel()
                        {
                            NextStation = station
                        }).Where(it => it.ID == latestRecord.ID)
                        .ExecuteCommand();
                    Console.WriteLine($"NextStation-成功更新了{rowsAffected}条记录 - {latestRecord.GUID}");
                }
                else
                {
                    Console.WriteLine($"NextStation-未找到匹配的记录:{prefix_WheelCoding}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoJob:{ex.ToString()}");
            }
            finally
            {
                db?.Close();
                db?.Dispose();
            }
        }

        public async Task SendMes(string _uri, string _sation, string _guid)
        {

            // 使用传统using块管理HttpClient
            using (var httpClient = new HttpClient())
            {
                var apiClient = new ApiClient(httpClient);

                try
                {
                    var response = await apiClient.PostJsonAsync<LoginRequest, ApiResponse>(
                       _uri,
                        new LoginRequest { stationNo = _sation, guid = _guid }
                    );

                    Console.WriteLine($"SendMes成功: {response?.msg}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SendMes失败: {ex.Message}");
                }
            }

        }



        /// <summary>
        /// 数据修改 - 产品NG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataModificationTriggered(KeyValuePair<string, int> keyValue)
        {
            SqlSugarClient db = new SqlAccess().SystemDataAccess;
            try
            {

                //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")} 数据修正信号：{plcSignal.Index}");
                string prefix_WheelCoding = keyValue.Key;
                int wheelDefect = keyValue.Value;
                char[] parts = prefix_WheelCoding.ToCharArray();
                if (parts.Count() != 12)
                {
                    throw new Exception($"Prefix_WheelCoding数据长度{prefix_WheelCoding}错误：{parts.Count()}");
                }

                //string oldWheelType = new string(parts, 4, 8);

                // 步骤1：查询符合条件的最新一条记录
                Tbl_productiondatamodel latestRecord = db.Queryable<Tbl_productiondatamodel>()
                    .Where(x => x.TransmissionCoding == prefix_WheelCoding)
                    .OrderByDescending(x => x.ID)
                    .First();

                if (latestRecord != null)
                {
                    // 步骤2：更新 Result 和 Code
                    var rowsAffected = db.Updateable<Tbl_productiondatamodel>()
                        .SetColumns(it => new Tbl_productiondatamodel()
                        {
                            ResultBool = false,
                            Remark = wheelDefect.ToString(),
                            NextStation = "不合格"
                        })
                        .Where(it => it.ID == latestRecord.ID)
                        .ExecuteCommand();

                    Console.WriteLine($"{DateTime.Now} {prefix_WheelCoding}更新了{rowsAffected}条记录{wheelDefect}");
                }
                else
                {
                    Console.WriteLine($"未找到匹配的记录:{prefix_WheelCoding}");
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoJob:{ex.ToString()}");
            }
            finally
            {
                db?.Close();
                db?.Dispose();
            }
        }

        /// <summary>
        /// 登录校验
        /// </summary>
        /// <returns></returns>
        private bool LoginCheck(string name, string pass)
        {
            var sDB = new SqlAccess().SystemDataAccess;

            try
            {
                List<Tbl_user> listUsers = sDB.Queryable<Tbl_user>().Where(x => x.User_name == name
                                                && x.Password == pass
                                                && x.Del_flag == "0"
                                                && x.Status == "0").ToList();
                if (listUsers.Count > 0)
                    return true;

            }
            catch (Exception e)
            {
                Console.WriteLine($"LoginCheck : {e.Message}");
            }
            finally
            {
                sDB?.Close();
                sDB?.Dispose();
            }
            return false;
        }



        #region 日志栏显示

        /// <summary>
        /// 系统信息显示
        /// </summary>
        /// <param name="model"></param>
        private void SystemMessageDisplay(MessageModel model)
        {
            Application.Current.Dispatcher.Invoke(() =>
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
            });
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

        #endregion


        #region PLC字节数据处理

        /// <summary>
        /// 字节数组转字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private string GetBytesToString(byte[] bytes, int startIndex)
        {
            byte maxLength = bytes[startIndex]; //最大长度
            byte currentLength = bytes[startIndex + 1]; // 实际字符数
            string result = System.Text.Encoding.UTF8.GetString(bytes.Skip(startIndex + 2).Take(currentLength).ToArray());
            return result;
        }

        /// <summary>
        /// 字符串转PLC格式字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] StringToS7Bytes(string str, int maxLength)
        {
            // 验证长度
            if (str.Length > maxLength - 2)
                throw new ArgumentException($"字符串长度超过最大限制 {maxLength}");

            byte[] buffer = new byte[maxLength]; // 总长度 = 最大长度 + 2
            buffer[0] = (byte)maxLength;   // 最大长度
            buffer[1] = (byte)str.Length;  // 当前长度

            // 填充字符串内容（ASCII编码）
            Encoding.ASCII.GetBytes(str, 0, str.Length, buffer, 2);
            return buffer;
        }


        /// <summary>
        /// 将源字节数组复制到目标字节数组的指定位置
        /// </summary>
        /// <param name="sourceArray">源字节数组</param>
        /// <param name="targetArray">目标字节数组</param>
        /// <param name="startIndex">目标数组的起始位置</param>
        /// <returns>实际复制的字节数</returns>
        public static int CopyBytes(byte[] sourceArray, byte[] targetArray, int startIndex)
        {
            // 参数校验
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (targetArray == null)
                throw new ArgumentNullException(nameof(targetArray));
            if (startIndex < 0 || startIndex >= targetArray.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            // 计算可复制的字节数
            int bytesAvailable = targetArray.Length - startIndex;
            int bytesToCopy = Math.Min(sourceArray.Length, bytesAvailable);

            // 执行复制
            if (bytesToCopy > 0)
            {
                Array.Copy(
                    sourceArray,      // 源数组
                    0,                // 源起始位置
                    targetArray,      // 目标数组
                    startIndex,       // 目标起始位置
                    bytesToCopy       // 复制的字节数
                );
            }

            return bytesToCopy;
        }

        #endregion

        /// <summary>
        /// 定时删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictrueDeleteTimer_Tick(object sender, EventArgs e)
        {
            // 计算过期日期（保留当天）
            DateTime expirationDate = DateTime.Now.AddDays(-SaveImageDays).Date;

            // 检查基础目录是否存在
            if (!Directory.Exists(HistoricalImagesPath))
            {            
                return;
            }
            try
            {
                // 获取所有子目录
                string[] subDirectories = Directory.GetDirectories(HistoricalImagesPath);
                foreach (string dirPath in subDirectories)
                {
                    try
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

                        // 检查目录创建时间是否早于阈值日期
                        if (dirInfo.CreationTime < expirationDate)
                        {
                            // 递归删除目录及其所有内容
                            Directory.Delete(dirPath, recursive: true);
                            Console.WriteLine($"已删除目录: {dirPath} (创建时间: {dirInfo.CreationTime})");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"处理目录 '{dirPath}' 时出错: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"扫描目录 '{HistoricalImagesPath}' 时出错: {ex.Message}");
            }

        } 
        private void PictrueDeleteTimer_Tick1(object sender, EventArgs e)
        {
            // 计算过期日期（保留当天）
            DateTime expirationDate = DateTime.Now.AddDays(-SaveImageDays).Date;

            // 删除过期的天文件夹
            for (int i = 0; i <= SaveImageDays + 30; i++) // 覆盖所有可能过期的日期
            {
                DateTime currentDate = expirationDate.AddDays(-i);
                string monthDir = currentDate.ToString("M月");
                string dayDir = currentDate.ToString("d日");

                string fullPath = Path.Combine(HistoricalImagesPath, monthDir, dayDir);
                Console.WriteLine($"目标路径：{fullPath}");
                if (Directory.Exists(fullPath))
                {
                    try
                    {
                        Directory.Delete(fullPath, true);
                        EventMessage.MessageDisplay($"已删除 {currentDate:M月d日} 的图片文件", true, true);
                    }
                    catch (Exception ex)
                    {
                        // 添加错误处理
                        EventMessage.MessageDisplay($"删除失败: {ex.Message}", true, false);
                    }
                }
            }

            // 删除空月文件夹（更安全的逻辑）
            foreach (var monthPath in Directory.GetDirectories(HistoricalImagesPath))
            {
                // 检查月份文件夹是否为空
                if (!Directory.EnumerateFileSystemEntries(monthPath).Any())
                {
                    try
                    {
                        Directory.Delete(monthPath);
                        string monthName = Path.GetFileName(monthPath);
                        EventMessage.MessageDisplay($"已删除空文件夹: {monthName}", true, true);
                    }
                    catch { /* 错误处理 */ }
                }
            }
      
        }


        #region 显示

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        private void DisplayDataGrid(int index, DisplayData data)
        {
            if (index < DisplayCollections.Count)
            {
                DisplayCollections[index] = data;

            }
        }
        #region 系统设置 - 文件管理-模板管理
        /// <summary>
        /// 设置
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ClickManage(string obj)
        {
            if (obj == "文件管理") FileDialogOpen();
            if (obj == "大模型更新") UpdateAi();
            if (obj == "图片查看") ProcessStart(HistoricalImagesPath);
            if (obj == "模板制作") ProcessStart(TemplateSoftwarePath);
            if (obj == "报表管理") ProcessStart(SQLManageSoftwarePath);

        }

        private void ProcessStart(string _path)
        {
            string path = _path;
            Task.Run(() =>
            {

                if (Directory.Exists(path) || File.Exists(path))
                {
                    Process.Start("explorer.exe", path);
                }
            });
        }

        /// <summary>
        /// 大模型更新
        /// </summary>
        private void UpdateAi()
        {
            var parameters = new DialogParameters
            {
            };
            _dialogService.ShowDialog("UpdateAiFile", parameters,
                new Action<IDialogResult>((IDialogResult result) =>
                {

                    if (result.Parameters.Count != 0)
                    {
                        IDialogParameters dialog = result.Parameters;
                        if (dialog.ContainsKey("FilePath_Hdict") && dialog.ContainsKey("FilePath_Hdl"))
                        {
                            string filePath_Hdict = dialog.GetValue<string>("FilePath_Hdict");
                            string filePath_Hdl = dialog.GetValue<string>("FilePath_Hdl");

                            // 新文件名
                            string newHdlFileName = "model_opt.hdl";
                            string newHdictFileName = "model_preprocess_params.hdict";

                            try
                            {
                                // 执行复制并重命名
                                FileHelper.CopyAndRenameFile(filePath_Hdl, DeepParaPath, newHdlFileName, true);
                                FileHelper.CopyAndRenameFile(filePath_Hdict, DeepParaPath, newHdictFileName, true);
                                workingPicture._isNeedLoadAI = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"操作失败: {ex.Message}");
                            }

                        }


                    }
                }));
        }


        /// <summary>
        /// 打开文件管理器
        /// </summary>
        private void FileDialogOpen()
        {
            var parameters = new DialogParameters
            {
                { "saveImageDays", SystemDatas.SaveImageDays },
                { "maintainQuantity", SystemDatas.MaintainQuantity },
                { "TemplateSoftwarePath", SystemDatas.TemplateSoftwarePath },
                { "SQLManageSoftwarePath", SystemDatas.SQLManageSoftwarePath }
            };
            _dialogService.ShowDialog("FileManage", parameters,
                new Action<IDialogResult>((IDialogResult result) =>
                {
                    if (result.Parameters.Count != 0)
                    {
                        IDialogParameters dialog = result.Parameters;

                        string days = dialog.GetValue<string>("saveImageDays");
                        SystemDatas.SaveImageDays = Convert.ToInt32(days);

                        string quantity = dialog.GetValue<string>("maintainQuantity");
                        SystemDatas.MaintainQuantity = Convert.ToInt32(quantity);

                        TemplateSoftwarePath = dialog.GetValue<string>("OpenTemplateFilePath");
                        SQLManageSoftwarePath = dialog.GetValue<string>("SQLManageSoftwarePath");

                        SqlAccess.SystemDatasUpdateable("SaveImageDays", days);
                        SqlAccess.SystemDatasUpdateable("MaintainQuantity", quantity);
                        SqlAccess.SystemDatasUpdateable("TemplateSoftwarePath", TemplateSoftwarePath);
                        SqlAccess.SystemDatasUpdateable("SQLManageSoftwarePath", SQLManageSoftwarePath);
                    }
                }));

        }


        #endregion

        private readonly object _lock = new object(); // 线程同步锁

        private void WriteLogToFile(int count1, int count2)
        {
            lock (_lock)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string line = $"{timestamp} - Computer:{count1} PLC: {count2}{Environment.NewLine}";

                try
                {
                    File.AppendAllText(@"E:\临时\计数.txt", line);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing log: {ex.Message}");
                }
            }
        }

        #endregion


    }
}
