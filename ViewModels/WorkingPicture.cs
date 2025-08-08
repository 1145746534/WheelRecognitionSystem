using HalconDotNet;
using Microsoft.Office.Interop.Excel;
using NPOI.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using System.Windows.Shapes;
using System.Windows.Threading;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using static NPOI.HSSF.Util.HSSFColor;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using static WheelRecognitionSystem.Public.SystemDatas;

namespace WheelRecognitionSystem.ViewModels
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class WorkingPicture : IDisposable
    {
        //大模型

        // 大模型hdict文件路径
        HTuple hv_PreprocessParamFileName = new HTuple();

        HTuple hv_RetrainedModelFileName = new HTuple();


        /// <summary>
        /// 文件内存
        /// </summary>
        public static HTuple hv_DLPreprocessParam = new HTuple();
        /// <summary>
        /// 模型内存
        /// </summary>
        public static HTuple hv_DLModelHandle = new HTuple();

        /// <summary>
        /// 传统识别模板数据
        /// </summary>

        public List<TemplatedataModel> TemplateModels;
        //是否刷新了数据
        private bool _isRefreshStatus = false;
        //是否需要加载AI参数 启动软件需要 更新了底层文件需要
        public bool _isNeedLoadAI = true;



        private readonly Queue<Dictionary<InteractS7PLCModel, HObject>> _processingQueue = new Queue<Dictionary<InteractS7PLCModel, HObject>>();
        private readonly object _processingLock = new object();
        private readonly Queue<SaveImageRequest> _saveImageQueue = new Queue<SaveImageRequest>();
        private readonly object _saveLock = new object();
        private bool _isProcessing = false;
        private bool _isSaving = false;


        /// <summary>
        /// 定时刷新使用状态到数据库
        /// </summary>
        public DispatcherTimer UseStatusTimer;

        // 添加停止标志
        private CancellationTokenSource _cts = new CancellationTokenSource();

        // 图片保存请求结构

        private struct SaveImageRequest
        {
            public HObject Image;
            public string Path { get; set; }
        }

        // 静态变量保存类的唯一实例
        private static readonly WorkingPicture instance = new WorkingPicture();

        // 私有构造函数防止外部实例化
        private WorkingPicture()
        {

        }

        // 公共静态方法为外部提供获取唯一实例的方法
        public static WorkingPicture Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            loadAIProcessParam(); //加载AI参数
            LoadedTemplateDatas();

            //订阅图像处理事件
            EventMessage.MessageHelper.GetEvent<ImagePushHandleEvent>().Subscribe(ImageHandle);
            //订阅模板参数刷新事件
            EventMessage.MessageHelper.GetEvent<RefreshNCCParaEvent>().Subscribe(RefreshNCCPara);
            // 启动保存线程（如果未运行）
            // 启动处理线程（如果未运行）
            if (!_isProcessing)
            {
                Task.Run(() => ProcessQueue());
            }
            // 启动保存线程（如果未运行）
            if (!_isSaving)
            {
                Task.Run(() => SaveImageWorker());
            }

            UseStatusTimer = new DispatcherTimer();
            UseStatusTimer.Tick += new EventHandler(UseStatus_Tick);//添加事件(到达时间间隔后会自动调用)
            UseStatusTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);//设置时间间隔为4小时
            UseStatusTimer.Start();
        }

        /// <summary>
        /// 把使用状态刷新到数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseStatus_Tick(object sender, EventArgs e)
        {
            //TemplateModels.FindLast(d => d.Index == 3).LastUsedTime = DateTime.Now;
            using (var _db = new SqlAccess().SystemDataAccess)
            {
                // 开启Sql日志输出（调试用）
                //_db.Aop.OnLogExecuting = (sql, pars) =>
                //{
                //    Console.WriteLine("UseStatus_Tick:" + sql);
                //};

                // 1. 提取需要检查的WheelType集合
                List<string> wheelTypes = TemplateModels.Select(d => d.WheelType).Distinct().ToList();

                // 2. 从数据库查询现有记录
                Dictionary<string, DateTime> dbRecords = _db.Queryable<sys_bd_Templatedatamodel>()
                    .Where(d => wheelTypes.Contains(d.WheelType))
                    .Select(d => new
                    {
                        d.WheelType,
                        d.LastUsedTime
                    })
                    .ToList()
                    .ToDictionary(d => d.WheelType, d => d.LastUsedTime);

                // 3. 筛选需要更新的记录
                List<TemplatedataModel> updates = TemplateModels
                    .Where(d => dbRecords.ContainsKey(d.WheelType) &&         // 确保数据库中存在记录
                                d.LastUsedTime != dbRecords[d.WheelType] &&   // 检查时间变化
                                d.LastUsedTime != default(DateTime))          // 排除未设置的时间
                    .ToList();

                if (!updates.Any())
                    return;
                List<sys_bd_Templatedatamodel> baseList = updates.Cast<sys_bd_Templatedatamodel>().ToList();                // 4. 批量更新（使用WheelType作为条件）
                _db.Updateable(baseList)
                   .UpdateColumns(d => new
                   {
                       d.LastUsedTime,  // 只更新LastUsedTime字段
                       d.UnusedDays
                   })
                   .WhereColumns(d => d.WheelType) // 根据WheelType匹配记录
                   .ExecuteCommand();

            }
        }





        /// <summary>
        /// 加载AI模型参数
        /// </summary>
        private void loadAIProcessParam()
        {
            if (_isNeedLoadAI)
            {

                try
                {
                    hv_DLPreprocessParam.Dispose();
                    hv_DLModelHandle.Dispose();
                    hv_PreprocessParamFileName.Dispose();
                    hv_RetrainedModelFileName.Dispose();
                    //Halcon路径
                    hv_PreprocessParamFileName = "D:/VisualDatas/大模型文件/model_preprocess_params.hdict";
                    HOperatorSet.ReadDict(hv_PreprocessParamFileName, new HTuple(), new HTuple(),
                        out hv_DLPreprocessParam);


                    hv_RetrainedModelFileName = "D:/VisualDatas/大模型文件/model_opt.hdl";
                    HOperatorSet.ReadDlModel(hv_RetrainedModelFileName, out hv_DLModelHandle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载AI参数异常：{ex.ToString()}");
                }
                _isNeedLoadAI = false;
                EventMessage.MessageDisplay($"大模型刷新", true, true);

            }
        }

        /// <summary>
        /// 加载传统识别模板
        /// </summary>
        private void LoadedTemplateDatas()
        {
            var db = new SqlAccess().SystemDataAccess;
            List<sys_bd_Templatedatamodel> Datas = db.Queryable<sys_bd_Templatedatamodel>().ToList();
            db.Close(); db.Dispose();
            TemplateModels = new List<TemplatedataModel>();


            for (int i = 0; i < Datas.Count; i++)
            {
                TemplatedataModel templatedata = new TemplatedataModel();
                templatedata.CopyPropertiesFrom(Datas[i]);
                templatedata.Status = TemplateStatus.Exist;
                string newDir = FileHelper.RenameDirectory(templatedata.TemplatePath);
                string usePath = FileHelper.CopyFile(templatedata.TemplatePath, newDir);
                templatedata.TemplateUsePath = usePath;

                TemplateModels.Add(templatedata);

            }
            Console.WriteLine($"模板加载完成");

            Datas.Clear();
        }

        /// <summary>
        /// 刷新模板状态
        /// </summary>
        private void RefreshNCCPara()
        {
            // 使用using确保资源释放
            using (var db = new SqlAccess().SystemDataAccess)
            {
                // 获取所有数据库记录
                var dbTemplates = db.Queryable<sys_bd_Templatedatamodel>().ToList();
                // 创建快速查找字典
                Dictionary<string, sys_bd_Templatedatamodel> dbDict = dbTemplates.ToDictionary(x => x.WheelType);
                foreach (var item in TemplateModels)
                {
                    if (dbDict.TryGetValue(item.WheelType, out var template))
                    {
                        // 存在匹配项
                        
                        if (template.UpdateTime != item.UpdateTime)
                        {
                            item.Status = TemplateStatus.Update;
                            item.CopyPropertiesFrom(template);
                            _isRefreshStatus = true;
                        }
                        
                    }
                    else
                    {
                        // 数据库中已不存在
                        item.Status = TemplateStatus.Delete;
                        _isRefreshStatus = true;
                    }
                }

                // 处理新增项（数据库中有但本地没有的）
                foreach (var newTemplate in dbTemplates.Where(x => !TemplateModels.Any(local => local.WheelType == x.WheelType)))
                {
                    var newItem = new TemplatedataModel();
                    newItem.CopyPropertiesFrom(newTemplate);
                    newItem.Status = TemplateStatus.Exist; // 或其他适当状态
                    TemplateModels.Add(newItem);
                    _isRefreshStatus = true;
                }
            }

           
            //var db = new SqlAccess().SystemDataAccess;
            //List<sys_bd_Templatedatamodel> Datas = db.Queryable<sys_bd_Templatedatamodel>().ToList();
            //db.Close(); db.Dispose();

            //foreach (TemplatedataModel item in TemplateModels)
            //{
            //    //判断 本机校准数据库
            //    sys_bd_Templatedatamodel template = Datas.Find((sys_bd_Templatedatamodel x) => x.WheelType == item.WheelType);
            //    if (template != null)
            //    {

            //        if (template.UpdateTime != item.UpdateTime)
            //        {
            //            //库里面的更新时间有变化 - 意味着模板更新了
            //            item.Status = TemplateStatus.Update;
            //            //拷贝数据
            //            item.CopyPropertiesFrom(template);
            //            _isRefreshStatus = true;
            //        }
            //    }
            //    else
            //    {
            //        //在数据中未查询到数据 - 意味着未删除
            //        item.Status = TemplateStatus.Delete; //这里只做标记
            //        _isRefreshStatus = true;
            //    }

            //}
            EventMessage.MessageDisplay($"刷新传统模板状态", true, true);

        }

        /// <summary>
        /// 使用前同步状态 避免异步处理
        /// </summary>
        public void UpdateTemplates()
        {
            try
            { 
            if (!_isRefreshStatus)
            {
                return;
            }

            for (int i = TemplateModels.Count - 1; i >= 0; i--)
            {

                TemplatedataModel item = TemplateModels[i];
                if (item.Status == TemplateStatus.Exist)
                {

                }
                if (item.Status == TemplateStatus.Update)
                {
                    // 复制文件（覆盖已存在的文件）
                    File.Copy(item.TemplatePath, item.TemplateUsePath, true);
                    item.ReleaseTemplate(); //下一次加载新的文件
                    item.Status = TemplateStatus.Exist;
                }
                if (item.Status == TemplateStatus.Delete)
                {
                    item.ReleaseTemplate();
                    //删除缓存文件
                    if (File.Exists(item.TemplateUsePath))
                    {
                        File.Delete(item.TemplateUsePath);
                    }
                    TemplateModels.RemoveAt(i);
                }
            }
            _isRefreshStatus = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateTemplates:" + ex);
            }
        }

        /// <summary>
        /// 释放不活跃内存
        /// </summary>
        /// <param name="keepCount"></param>
        public void ReleaseUnusedTemplates(int keepCount = 3)
        {
            if (TemplateModels == null || TemplateModels.Count <= keepCount)
                return;
            // 原地排序：按照LastUsedTime降序（从最近到最久）
            TemplateModels.Sort((TemplatedataModel a, TemplatedataModel b) => b.LastUsedTime.CompareTo(a.LastUsedTime));
            // 释放从索引keepCount开始的所有模板
            //for (int i = keepCount; i < TemplateModels.Count; i++)
            //{
            //    TemplateModels[i].ReleaseTemplate();

            //}
        }

        /// <summary>
        /// 根据名称获取模板数据 供生成模板区域使用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HTuple GetHTupleByName(string name)
        {
            TemplatedataModel templatedata = TemplateModels.Find((TemplatedataModel x) => x.WheelType == name);
            return templatedata.Template;
        }



        /// <summary>
        /// 接收主界面传过来的PLC数据
        /// </summary>
        /// <param name="interact"></param>
        public async void ReceiveS7PLC(InteractS7PLCModel interact)
        {
            if (interact != null)
            {
                //休眠
                await Task.Delay(interact.ArrivalDelay);

                //获取图片
                EventMessage.MessageHelper.GetEvent<GetGrabimageByViewEvent>().Publish(interact);
            }
        }

        public void ImageHandle(Dictionary<InteractS7PLCModel, HObject> dic)
        {
            lock (_processingLock)
            {
                _processingQueue.Enqueue(dic);
            }

        }

        // 处理队列的后台线程
        private void ProcessQueue()
        {
            _isProcessing = true;
            while (!_cts.IsCancellationRequested)
            {
                Dictionary<InteractS7PLCModel, HObject> dic = null;
                lock (_processingLock)
                {
                    if (_processingQueue.Count > 0)
                    {
                        UpdateTemplates();
                        dic = _processingQueue.Dequeue();
                        foreach (KeyValuePair<InteractS7PLCModel, HObject> item in dic)
                        {
                            RecognitionResultModel recognitionResult = null;
                            InteractS7PLCModel interact = item.Key;
                            //需销毁
                            HObject image = item.Value;
                            List<RecognitionResultModel> list = null;
                            HObject templateContour = new HObject();
                            HObject wheelContour = new HObject();

                            try
                            {
                                interact.starTime = DateTime.Now;
                                if (image == null)
                                {
                                    recognitionResult = new RecognitionResultModel();
                                    recognitionResult.RecognitionWheelType = "NG";
                                    recognitionResult.status = "图像采集失败";

                                }
                                else
                                {

                                    //处理图像
                                    HObject grayImage = RGBTransGray(image);
                                    string recognitionWay = "传统";
                                    string score = "0";

                                    //获取整张图的灰度值            
                                    double fullFigureGary = GetIntensity(grayImage);
                                    Console.WriteLine($"灰度比较 ： {fullFigureGary} ? {MinFullFigureGary}");
                                    if (fullFigureGary > MinFullFigureGary)
                                    {
                                        //定位轮毂
                                        PositioningWheelResultModel pResult = PositioningWheel(grayImage, WheelMinThreshold, 255, WheelMinRadius);
                                        //存储识别结果                    
                                        if (pResult.WheelImage != null && pResult.WheelImage.IsInitialized())
                                        {
                                            HObject imageRecogn = CloneImageSafely(pResult.WheelImage);
                                            //如果没定位到轮毂 就不要用传统识别 - 直接用大模型
                                            recognitionResult = WheelRecognitionAlgorithm(imageRecogn, TemplateModels, AngleStart, AngleExtent, MinSimilarity, out list);
                                            recognitionResult.FullFigureGary = (float)fullFigureGary;
                                            recognitionResult.InnerCircleGary = pResult.InnerCircleMean;
                                            wheelContour = CloneImageSafely(pResult.WheelContour);
                                            if (recognitionResult.RecognitionWheelType != "NG") //
                                            {

                                                templateContour = GetAffineTemplateContour(GetHTupleByName(recognitionResult.RecognitionWheelType),
                                                    recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
                                                ReleaseUnusedTemplates();
                                                score = recognitionResult.Similarity.ToString("F3");
                                            }
                                            SafeDisposeHObject(ref imageRecogn);
                                        }
                                        pResult.Dispose();

                                    }

                                    //没定位到轮毂 或 没识别到轮型 启动大模型识别
                                    if (recognitionResult == null || recognitionResult?.RecognitionWheelType == "NG")
                                    {
                                        loadAIProcessParam();
                                        recognitionResult = new RecognitionResultModel() { RecognitionWheelType = "NG" };
                                        recognitionWay = "大模型";
                                        //大模型推算
                                        HTuple hv_DLResult = WheelDeepLearning(grayImage, hv_DLModelHandle, hv_DLPreprocessParam);
                                        HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
                                        HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
                                        if (names.Length > 0 && confidences[0].D > ConfidenceMatch)
                                        {

                                            string[] name = names[0].S.Split('_'); //00619C70_半
                                            string value = string.Empty;
                                            if (name.Length == 2)
                                                value = name[1] == "半" ? "半成品" : "成品";


                                            recognitionResult.RecognitionWheelType = name[0]; //识别结果
                                            recognitionResult.WheelStyle = value; //识别样式
                                            recognitionResult.Similarity = double.Parse(confidences[0].D.ToString("0.000"));
                                            recognitionResult.status = "识别成功";
                                            score = confidences[0].D.ToString("F3");

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
                                    recognitionResult.Way = recognitionWay;
                                    SaveWay way = recognitionResult.ResultBol ? SaveWay.AutoOK : SaveWay.AutoNG;

                                    //保存图片
                                    string style = recognitionResult.WheelStyle == "成品" ? "成" : "半";
                                    string _prefixName = $"{recognitionResult.RecognitionWheelType}_{style}+{recognitionWay}{score}";

                                    string savePath = GetImageSavePath(way, HistoricalImagesPath, _prefixName);
                                    interact.imagePath = savePath;



                                    var saveRequest = new SaveImageRequest
                                    {
                                        Image = grayImage.Clone(),
                                        Path = savePath,
                                    };

                                    lock (_saveLock)
                                    {
                                        _saveImageQueue.Enqueue(saveRequest);
                                    }



                                    //下发显示
                                    AutoRecognitionResultDisplayModel autoRecognitionResult = new AutoRecognitionResultDisplayModel();
                                    autoRecognitionResult.Tag = $"DisplayRegion{interact.readPLCSignal.Index + 1}";
                                    autoRecognitionResult.FullFigureGary = (float)fullFigureGary;
                                    autoRecognitionResult.CurrentImage = CloneImageSafely(grayImage);
                                    autoRecognitionResult.TemplateContour = CloneImageSafely(templateContour);
                                    autoRecognitionResult.WheelContour = CloneImageSafely(wheelContour);
                                    EventMessage.MessageHelper.GetEvent<RecognitionDisplayEvent>().Publish(autoRecognitionResult);
                                    //Console.WriteLine("步骤2");
                                    SafeDisposeHObject(ref grayImage);
                                }

                            }
                            catch (Exception ex)
                            {
                                recognitionResult = new RecognitionResultModel();
                                recognitionResult.RecognitionWheelType = "NG";
                                recognitionResult.status = "处理失败";
                            }
                            finally
                            {


                                list?.Clear();
                                SafeDisposeHObject(ref image);
                                SafeDisposeHObject(ref templateContour);
                                SafeDisposeHObject(ref wheelContour);
                                interact.endTime = DateTime.Now;
                                interact.resultModel = recognitionResult;
                                //回调处理完成
                                EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Publish(interact);
                            }
                        }
                        dic.Clear();

                    }
                }

                if (dic == null)
                {
                    Thread.Sleep(100); // 队列空时短暂休眠
                    continue;
                }

                //ProcessImage(item.Value.Key, item.Value.Value);
            }
        }

        // 图片保存工作线程
        private async void SaveImageWorker()
        {
            _isSaving = true;
            while (!_cts.IsCancellationRequested)
            {
                SaveImageRequest? request = null;
                lock (_saveLock)
                {
                    if (_saveImageQueue.Count > 0)
                    {
                        request = _saveImageQueue.Dequeue();
                    }
                }
                if (!request.HasValue)
                {
                    await Task.Delay(100); // 队列空时异步等待
                    continue;
                }
                var req = request.Value;
                try
                {
                    SaveImageDatasAsync(req.Image, req.Path);

                    // 可在此处记录保存结果
                }
                finally
                {
                    SafeDisposeHObject(ref req.Image);
                }
            }
        }

        ///// <summary>
        ///// 图像处理
        ///// </summary>
        ///// <param name="image"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public async void ImageHandle1(Dictionary<InteractS7PLCModel, HObject> dic)
        //{
        //    foreach (KeyValuePair<InteractS7PLCModel, HObject> item in dic)
        //    {
        //        RecognitionResultModel recognitionResult = null;
        //        List<RecognitionResultModel> list = null;
        //        InteractS7PLCModel interact = item.Key;
        //        HObject image = item.Value;
        //        HObject templateContour = new HObject();
        //        HObject wheelContour = new HObject();
        //        interact.starTime = DateTime.Now;
        //        try
        //        {
        //            if (image == null)
        //            {
        //                recognitionResult = new RecognitionResultModel();
        //                recognitionResult.RecognitionWheelType = "NG";
        //                recognitionResult.status = "图像采集失败";

        //            }
        //            else
        //            {
        //                //处理图像
        //                HObject grayImage = RGBTransGray(image);
        //                string recognitionWay = "传统";

        //                //获取整张图的灰度值            
        //                double fullFigureGary = GetIntensity(grayImage);
        //                Console.WriteLine($"灰度比较 ： {fullFigureGary} ? {MinFullFigureGary}");
        //                if (fullFigureGary > MinFullFigureGary)
        //                {
        //                    //定位轮毂
        //                    PositioningWheelResultModel pResult = PositioningWheel(grayImage, WheelMinThreshold, 255, WheelMinRadius);
        //                    //存储识别结果                    
        //                    if (pResult.WheelImage != null && pResult.WheelImage.IsInitialized())
        //                    {
        //                        HObject imageRecogn = CloneImageSafely(pResult.WheelImage);
        //                        //如果没定位到轮毂 就不要用传统识别 - 直接用大模型
        //                        recognitionResult = WheelRecognitionAlgorithm(imageRecogn, TemplateModels, AngleStart, AngleExtent, MinSimilarity, out list);
        //                        recognitionResult.FullFigureGary = (float)fullFigureGary;
        //                        recognitionResult.InnerCircleGary = pResult.InnerCircleMean;
        //                        wheelContour = CloneImageSafely(pResult.WheelContour);
        //                        if (recognitionResult.RecognitionWheelType != "NG") //
        //                        {

        //                            templateContour = GetAffineTemplateContour(GetHTupleByName(recognitionResult.RecognitionWheelType),
        //                                recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
        //                            ReleaseUnusedTemplates();
        //                        }
        //                        SafeDisposeHObject(ref imageRecogn);
        //                    }
        //                    pResult.Dispose();

        //                }

        //                //没定位到轮毂 或 没识别到轮型 启动大模型识别
        //                if (recognitionResult == null || recognitionResult?.RecognitionWheelType == "NG")
        //                {
        //                    recognitionResult = new RecognitionResultModel() { RecognitionWheelType = "NG" };
        //                    //大模型推算
        //                    HTuple hv_DLResult = WheelDeepLearning(grayImage, hv_DLModelHandle, hv_DLPreprocessParam);
        //                    HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
        //                    HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
        //                    if (names.Length > 0 && confidences[0].D > ConfidenceMatch)
        //                    {
        //                        recognitionWay = "大模型";
        //                        string[] name = names[0].S.Split('_'); //00619C70_半
        //                        string value = string.Empty;
        //                        if (name.Length == 2)
        //                            value = name[1] == "半" ? "半成品" : "成品";


        //                        recognitionResult.RecognitionWheelType = name[0]; //识别结果
        //                        recognitionResult.WheelStyle = value; //识别样式
        //                        recognitionResult.Similarity = double.Parse(confidences[0].D.ToString("0.000"));
        //                        recognitionResult.status = "识别成功";

        //                    }
        //                    //for (int i = 0; i < names.Length; i++)
        //                    //{
        //                    //    double similar = double.Parse(confidences[i].D.ToString("0.0000"));
        //                    //    Console.WriteLine($"数据：{names[i].S} 结果：{similar}");
        //                    //}
        //                    SafeHalconDispose(hv_DLResult);
        //                    SafeHalconDispose(names);
        //                    SafeHalconDispose(confidences);
        //                }
        //                recognitionResult.Way = recognitionWay;
        //                SaveWay way = recognitionResult.ResultBol ? SaveWay.AutoOK : SaveWay.AutoNG;

        //                //保存图片
        //                string style = recognitionResult.WheelStyle == "成品" ? "成" : "半";
        //                string _prefixName = $"{recognitionResult.RecognitionWheelType}_{style}+{recognitionWay}";
        //                interact.imagePath = await SaveImageDatasAsync(grayImage, way, HistoricalImagesPath, _prefixName);

        //                //下发显示
        //                AutoRecognitionResultDisplayModel autoRecognitionResult = new AutoRecognitionResultDisplayModel();
        //                autoRecognitionResult.Tag = $"DisplayRegion{interact.readPLCSignal.Index + 1}";
        //                autoRecognitionResult.FullFigureGary = (float)fullFigureGary;
        //                autoRecognitionResult.CurrentImage = CloneImageSafely(grayImage);
        //                autoRecognitionResult.TemplateContour = CloneImageSafely(templateContour);
        //                autoRecognitionResult.WheelContour = CloneImageSafely(wheelContour);
        //                EventMessage.MessageHelper.GetEvent<RecognitionDisplayEvent>().Publish(autoRecognitionResult);
        //                //Console.WriteLine("步骤2");
        //                SafeDisposeHObject(ref grayImage);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            recognitionResult = new RecognitionResultModel();
        //            recognitionResult.RecognitionWheelType = "NG";
        //            recognitionResult.status = "处理失败";
        //        }
        //        finally
        //        {

        //            interact.endTime = DateTime.Now;
        //            interact.resultModel = recognitionResult;
        //            list?.Clear();
        //            SafeDisposeHObject(ref image);
        //            SafeDisposeHObject(ref templateContour);
        //            SafeDisposeHObject(ref wheelContour);
        //            //回调处理完成
        //            EventMessage.MessageHelper.GetEvent<InteractCallEvent>().Publish(interact);
        //        }







        //    }
        //    dic.Clear();
        //}




        public void Dispose()
        {
            _cts.Cancel();

        }
    }



}
