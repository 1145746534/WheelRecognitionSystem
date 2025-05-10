using HalconDotNet;
using Microsoft.Win32;
using NPOI.OpenXmlFormats.Vml;
using NPOI.SS.Formula.Functions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.Views.Dialogs;
using static WheelRecognitionSystem.Public.SystemDatas;
using static WheelRecognitionSystem.Public.ImageProcessingHelper;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Media3D;
using System.Drawing;
using MvCameraControl;


namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class TemplateManagementViewModel : BindableBase
    {
        #region======属性字段定义======
        public string Title { get; set; } = "模板管理";

        private ObservableCollection<sys_bd_Templatedatamodel> _templateDatas;
        /// <summary>
        /// 模板数据
        /// </summary>
        public ObservableCollection<sys_bd_Templatedatamodel> TemplateDatas
        {
            get { return _templateDatas; }
            set { SetProperty(ref _templateDatas, value); }
        }

        private HObject _displayTemplateImage;
        /// <summary>
        /// 用于显示的模板原始图像
        /// </summary>
        public HObject DisplayTemplateImage
        {
            get { return _displayTemplateImage; }
            set { SetProperty<HObject>(ref _displayTemplateImage, value); }
        }

        private HObject _displayTemplate;
        /// <summary>
        /// 用于显示的制作模板的图像
        /// </summary>
        public HObject DisplayTemplate
        {
            get { return _displayTemplate; }
            set { SetProperty<HObject>(ref _displayTemplate, value); }
        }

        private HObject _displayTemplateContour;
        /// <summary>
        /// 用于显示的模板轮廓
        /// </summary>
        public HObject DisplayTemplateContour
        {
            get { return _displayTemplateContour; }
            set { SetProperty<HObject>(ref _displayTemplateContour, value); }
        }

        private HObject _displayWheelContour;
        /// <summary>
        /// 用于显示轮毂的轮廓
        /// </summary>
        public HObject DisplayWheelContour
        {
            get { return _displayWheelContour; }
            set { SetProperty(ref _displayWheelContour, value); }
        }

        private HObject _displayInGateContour;
        /// <summary>
        /// 用于显示的浇口轮廓
        /// </summary>
        public HObject DisplayInGateContour
        {
            get { return _displayInGateContour; }
            set { SetProperty<HObject>(ref _displayInGateContour, value); }
        }

        private double _lineWidth = 1.0;
        /// <summary>
        /// 模板制作窗口轮廓显示线宽
        /// </summary>
        public double LineWidth
        {
            get { return _lineWidth; }
            set { SetProperty(ref _lineWidth, value); }
        }


        private sys_bd_Templatedatamodel _dataGridSelectedItem;
        /// <summary>
        /// 模板数据窗口选中的行
        /// </summary>
        public sys_bd_Templatedatamodel DataGridSelectedItem
        {
            get { return _dataGridSelectedItem; }
            set
            {
                SetProperty(ref _dataGridSelectedItem, value);
            }
        }

        private int _dataGridSelectedIndex;
        /// <summary>
        /// 模板数据窗口选中的行索引
        /// </summary>
        public int DataGridSelectedIndex
        {
            get { return _dataGridSelectedIndex; }
            set { SetProperty(ref _dataGridSelectedIndex, value); }
        }

        private string _recognitionWheelType;
        /// <summary>
        /// 识别的轮型
        /// </summary>
        public string RecognitionWheelType
        {
            get { return _recognitionWheelType; }
            set { SetProperty(ref _recognitionWheelType, value); }
        }

        private string _recognitionSimilarity;
        /// <summary>
        /// 识别的相似度
        /// </summary>
        public string RecognitionSimilarity
        {
            get { return _recognitionSimilarity; }
            set { SetProperty(ref _recognitionSimilarity, value); }
        }

        private string _recognitionConsumptionTime;
        /// <summary>
        /// 识别消耗的时间
        /// </summary>
        public string RecognitionConsumptionTime
        {
            get { return _recognitionConsumptionTime; }
            set { SetProperty(ref _recognitionConsumptionTime, value); }
        }

        private Visibility _recognitionResultDisplay;
        /// <summary>
        /// 识别结果显示
        /// </summary>
        public Visibility RecognitionResultDisplay
        {
            get { return _recognitionResultDisplay; }
            set { SetProperty(ref _recognitionResultDisplay, value); }
        }

        private Visibility _imageDisVisibility;
        /// <summary>
        /// 图片的来源显示
        /// </summary>
        public Visibility ImageDisVisibility
        {
            get { return _imageDisVisibility; }
            set { SetProperty(ref _imageDisVisibility, value); }
        }

        private string _gateDetectionResult;
        /// <summary>
        /// 浇口检测结果
        /// </summary>
        public string GateDetectionResult
        {
            get { return _gateDetectionResult; }
            set { SetProperty(ref _gateDetectionResult, value); }
        }

        private string _gateArea;
        /// <summary>
        /// 浇口面积
        /// </summary>
        public string GateArea
        {
            get { return _gateArea; }
            set { SetProperty(ref _gateArea, value); }
        }

        private string _gateRadiu;
        /// <summary>
        /// 浇口半径
        /// </summary>
        public string GateRadiu
        {
            get { return _gateRadiu; }
            set { SetProperty(ref _gateRadiu, value); }
        }

        private Visibility _gateDetectionVisibility;
        /// <summary>
        /// 浇口检测结果显示
        /// </summary>
        public Visibility GateDetectionVisibility
        {
            get { return _gateDetectionVisibility; }
            set { SetProperty(ref _gateDetectionVisibility, value); }
        }

        private string _imageGrayval;
        /// <summary>
        /// 图像灰度
        /// </summary>
        public string ImageGrayval
        {
            get { return _imageGrayval; }
            set { SetProperty(ref _imageGrayval, value); }
        }

        private string _imageDisName;
        /// <summary>
        /// 图像来源窗口名称
        /// </summary>
        public string ImageDisName
        {
            get { return _imageDisName; }
            set { SetProperty(ref _imageDisName, value); }
        }

        private string _gateContourColor;
        /// <summary>
        /// 浇口轮廓颜色
        /// </summary>
        public string GateContourColor
        {
            get { return _gateContourColor; }
            set { SetProperty(ref _gateContourColor, value); }
        }

        /// <summary>
        /// 弹窗服务
        /// </summary>
        readonly IDialogService _dialogService;

        /// <summary>
        /// 读取或采集的原始模板图像
        /// </summary>
        private HObject SourceTemplateImage = new HObject();
        /// <summary>
        /// 定位轮毂获取到的轮毂图像
        /// </summary>
        private HObject InPoseWheelImage = new HObject();
        /// <summary>
        ///用于制作和保存的模板图像
        /// </summary>
        private HObject TemplateImage = new HObject();
        /// <summary>
        /// 匹配结果弹窗是否打开
        /// </summary>
        private bool IsMatchResultDialog = false;
        #endregion

        #region======命令定义======
        /// <summary>
        /// 模板制作按钮命令
        /// </summary>
        public DelegateCommand<string> TemplateBtnCommand { get; set; }

        /// <summary>
        /// 模板数据窗口鼠标左键按下命令
        /// </summary>
        public ICommand MouseLeftButtonDownCommand { get; set; }
        /// <summary>
        /// 模板窗口鼠标移动命令
        /// </summary>
        public ICommand HWindowMouseMoveCommand { get; set; }
        #endregion

        public TemplateManagementViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            TemplateBtnCommand = new DelegateCommand<string>(TemplateButtonCommand);
            MouseLeftButtonDownCommand = new DelegateCommand<object>(MouseLeftButtonDown);
            HWindowMouseMoveCommand = new DelegateCommand<object>(HWindowMouseMove);
            DisplayTemplateImage = new HObject();
            DisplayTemplate = new HObject();
            DisplayWheelContour = new HObject();
            DisplayTemplateContour = new HObject();
            DisplayInGateContour = new HObject();
            TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>();
            LoadedTemplateDatas();
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            ImageDisVisibility = Visibility.Collapsed;
            //订阅事件
            EventMessage.MessageHelper.GetEvent<TemplateDataUpdataEvent>().Subscribe(TemplateDataUpdata);
            EventMessage.MessageHelper.GetEvent<TemplatePicUpdateEvent>().Subscribe(PicUpdate);

        }



        private void TemplateDataUpdata(string obj)
        {
            LoadedTemplateDatas();
        }
        /// <summary>
        /// 设置转过来图片更新
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PicUpdate(ServletInfoModel model)
        {

            if (model.image != null && model.camera != null)
            {
                SourceTemplateImage.Dispose();

                HOperatorSet.CountChannels(model.image, out HTuple Channels);
                if (Channels?.I == 3)
                    HOperatorSet.Decompose3(model.image, out SourceTemplateImage, out HObject image2, out HObject image3);
                else
                    SourceTemplateImage = model.image;


                TemplateWindowDisplay(SourceTemplateImage, null, null, null, null);
                ImageDisVisibility = Visibility.Visible;
                ImageDisName = model.DisplayName;
            }
        }

        /// <summary>
        /// 模板窗口鼠标移动
        /// </summary>
        /// <param name="obj"></param>
        private void HWindowMouseMove(object obj)
        {
            var data = obj as HSmartWindowControlWPF.HMouseEventArgsWPF;
            if (SourceTemplateImage.IsInitialized())
            {
                try
                {
                    int row = (int)data.Row;
                    int column = (int)data.Column;
                    HOperatorSet.GetGrayval(SourceTemplateImage, row, column, out HTuple grayval);
                    ImageGrayval = "行: " + row.ToString() + " 列: " + column.ToString() + " 灰度值: " + grayval;
                }
                catch { }
            }
        }
        /// <summary>
        /// 模板数据窗口鼠标左键按下
        /// </summary>
        /// <param name="obj"></param>
        private void MouseLeftButtonDown(object obj)
        {
            DataGrid dataGrid = (DataGrid)obj;
            if (dataGrid != null && dataGrid.Items.Count > 0 && dataGrid.CurrentItem != null)
            {
                //获取选中的行索引
                int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
                //获取选中的列索引
                int columnIndex = dataGrid.CurrentCell.Column.DisplayIndex;
                //强制分选设置
                if (columnIndex == 4)
                {
                    DataGridSelectedItem = TemplateDatas[DataGridSelectedIndex];
                    var sDB = new SqlAccess().SystemDataAccess;
                    sDB.Updateable(DataGridSelectedItem).ExecuteCommand();
                }
            }
        }
        /// <summary>
        /// 加载模板数据
        /// </summary>
        private void LoadedTemplateDatas()
        {
            List<sys_bd_Templatedatamodel> datas = new SqlAccess().SystemDataAccess.Queryable<sys_bd_Templatedatamodel>().ToList();
            //此处必须使用Invoke
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                TemplateDatas.Clear();
                TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>(datas);
            }));
        }
        /// <summary>
        /// 模板管理按钮命令响应
        /// </summary>
        /// <param name="obj"></param>
        private void TemplateButtonCommand(string obj)
        {
            if (obj == "添加模板") AddTemplate();
            else if (obj == "读取图片") ReadImage();
            else if (obj == "显示模板") DisplayTemplates();
            else if (obj == "采集图像") AcquireImage();
            else if (obj == "定位轮毂") PositionHub();
            else if (obj == "删除模板") DelTemplate();
            else if (obj == "参数设置") ParameterSetting();
            else if (obj == "预览模板") PreviewTemplate();
            else if (obj == "匹配结果") MatchResult();
            else if (obj == "识别测试") RecognitionTest();
            else if (obj == "保存模板") SaveTemplate();
            else if (obj == "模板检查") TemplateExamine();
            else return;
        }

        /// <summary>
        /// 添加轮型
        /// </summary>
        private void AddTemplate()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            //弹出窗口带返回结果写法
            //_dialogService.ShowDialog("WheelTypeSetting", new Action<IDialogResult>(AddWheelResult));

            _dialogService.ShowDialog("WheelTypeSetting", new Action<IDialogResult>((IDialogResult result) =>
            {
                if (result.Parameters.Count != 0)
                {
                    LoadedTemplateDatas();
                    DataGridSelectedItem = result.Parameters.GetValue<sys_bd_Templatedatamodel>("set");
                    DataGridSelectedIndex = DataGridSelectedItem.Index - 1;
                    //发布修改项用于模板数据显示控件滚动到修改项
                    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
            }));
            //弹出窗口不带返回结果写法
            //_dialogService.ShowDialog("WheelTypeSetting");
        }
        /// <summary>
        /// 读取图像
        /// </summary>
        private void ReadImage()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            var path = HistoricalImagesPath + @"\" + DateTime.Now.Month.ToString() + @"月\" + DateTime.Now.Day.ToString() + @"日";
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "请选择要制作模板的图片，当日图像存储路径为：" + path,
                Filter = "TIF文件|*.tif|JPEG文件|*.jpg|BMP文件|*.bmp|PNG文件|*.png|所有文件(*.*)|*.*",//文件筛选器设定
                FilterIndex = 1,
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var File_Name = openFileDialog.FileName;
                    SourceTemplateImage.Dispose();
                    HOperatorSet.ReadImage(out HObject image, File_Name);
                    //彩色图需转成灰度图
                    HOperatorSet.CountChannels(image, out HTuple Channels);
                    if (Channels.I == 3)
                    {
                        HOperatorSet.Decompose3(image, out HObject image1, out HObject image2, out HObject image3);
                        SourceTemplateImage = image1;
                    }
                    else
                        SourceTemplateImage = image;

                    TemplateWindowDisplay(SourceTemplateImage, null, null, null, null);
                }
                catch (Exception ex)
                {
                    EventMessage.SystemMessageDisplay(ex.Message, MessageType.Error);
                }
            }
        }
        /// <summary>
        /// 采集图像
        /// </summary>
        private void AcquireImage()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            if (SystemModel)
            {
                EventMessage.SystemMessageDisplay("请在手动模式下采集图像!", MessageType.Default);
                return;
            }
            try
            {
                HOperatorSet.GrabImageAsync(out HObject image, ExternalConnections.CameraHandle, -1);
                HOperatorSet.Rgb1ToGray(image, out HObject grayImage);
                SourceTemplateImage.Dispose();
                if (CroppingOrNot)
                {
                    Cropping(grayImage, out HObject SourceImage);
                    HOperatorSet.ZoomImageFactor(SourceImage, out SourceTemplateImage, ScalingCoefficient, ScalingCoefficient, "constant");
                }
                else
                {
                    HOperatorSet.ZoomImageFactor(grayImage, out SourceTemplateImage, ScalingCoefficient, ScalingCoefficient, "constant");
                }
                TemplateWindowDisplay(SourceTemplateImage, null, null, null, null);
            }
            catch
            {
                EventMessage.SystemMessageDisplay("采集异常，请检查相机连接!", MessageType.Error);
            }
        }
        /// <summary>
        /// 定位轮毂
        /// </summary>
        private void PositionHub()
        {
            if (!SourceTemplateImage.IsInitialized())
            {
                EventMessage.SystemMessageDisplay("请先执行读取图片或采集图像!", MessageType.Warning);
                return;
            }
            try
            {
                PositioningWheelResultModel pResult = PositioningWheel(SourceTemplateImage, WheelMinThreshold, 255, WheelMinRadius, false);
                if (pResult.WheelImage == null)
                {
                    EventMessage.SystemMessageDisplay("定位轮毂失败，请调整轮毂阈值参数后再试!", MessageType.Warning);
                    return;
                }
                else
                {
                    EventMessage.SystemMessageDisplay($"轮毂半径：{pResult.Radius}。", MessageType.Default);
                    InPoseWheelImage.Dispose();
                    InPoseWheelImage = pResult.WheelImage;
                    TemplateWindowDisplay(SourceTemplateImage, null, pResult.WheelContour, null, null);
                }
            }
            catch (Exception ex)
            {
                EventMessage.SystemMessageDisplay(ex.Message, MessageType.Error);
                return;
            }
        }
        /// <summary>
        /// 预览模板
        /// </summary>
        private void PreviewTemplate()
        {
            if (!InPoseWheelImage.IsInitialized())
            {
                EventMessage.SystemMessageDisplay("请先执行定位轮毂!", MessageType.Warning);
                return;
            }
            HOperatorSet.InnerCircle(InPoseWheelImage, out HTuple row, out HTuple column, out HTuple radius);
            //根据轮毂生成一个90度扇形区域
            HOperatorSet.GenCircleSector(out HObject circleSector, row, column, radius, TemplateStartAngle, TemplateEndAngle);
            //剪切制作模板的区域
            HOperatorSet.ReduceDomain(InPoseWheelImage, circleSector, out HObject reduced);
            HOperatorSet.Threshold(reduced, out HObject region, 0, WindowMaxThreshold);
            HOperatorSet.Connection(region, out HObject connectedRegions);
            HOperatorSet.ClosingCircle(connectedRegions, out HObject regionClosing, 3.5);
            HOperatorSet.OpeningCircle(regionClosing, out HObject RegionOpening, 1.5);
            //HOperatorSet.FillUp(regionClosing, out HObject regionFillUp);
            HOperatorSet.SelectShape(RegionOpening, out HObject selectedRegions, "area", "and", RemoveMixArea, 999999);
            HOperatorSet.Union1(selectedRegions, out HObject regionUnion);
            HOperatorSet.Difference(reduced, regionUnion, out HObject regionDifference);
            TemplateImage.Dispose();
            HOperatorSet.ReduceDomain(reduced, regionDifference, out TemplateImage);
            TemplateWindowDisplay(null, TemplateImage, null, null, null);
        }
        /// <summary>
        /// 保存模板
        /// </summary>
        private void SaveTemplate()
        {
            if (DataGridSelectedItem == null)
            {
                EventMessage.SystemMessageDisplay("请先选择轮型!", MessageType.Warning);
                return;
            }
            if (!TemplateImage.IsInitialized())
            {
                EventMessage.SystemMessageDisplay("请先执行预览模板!", MessageType.Warning);
                return;
            }
            bool result = WMessageBox.Show("保存确认", "您选择的轮型是：《" + DataGridSelectedItem.WheelType + "》，请确定轮型选择正确后，再点击确认！");
            if (result)
            {
                //模板图像保存路径
                string tPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
                //活跃模板保存路径
                string aPath = ActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                //不活跃模板保存路径
                string nPath = NotActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                //生成NCC模板
                HOperatorSet.CreateNccModel(TemplateImage, "auto", AngleStart, AngleExtent, 0.05, "use_polarity", out HTuple nccTemplate);
                //查找当前轮型名的索引
                int activeIndex = TemplateDataCollection.ActiveTemplateNames.FindIndex(x => x == DataGridSelectedItem.WheelType);
                int notActiveIndex = TemplateDataCollection.NotActiveTemplateNames.FindIndex(x => x == DataGridSelectedItem.WheelType);
                //如果在活跃轮型列表中找到 或 在活跃轮型列表和不活跃轮型列表中都没找到
                if (activeIndex >= 0 || (activeIndex < 0 && notActiveIndex < 0))
                {
                    HOperatorSet.WriteNccModel(nccTemplate, aPath);
                    //在AddOrReviseTemplateDatas中查找当前轮型
                    int index = AddOrReviseTemplateDatas.ActiveTemplateNames.FindIndex(x => x == DataGridSelectedItem.WheelType);
                    //如果没找到则增加
                    if (index < 0)
                    {
                        AddOrReviseTemplateDatas.ActiveTemplateNames.Add(DataGridSelectedItem.WheelType);
                        AddOrReviseTemplateDatas.ActiveTemplates.Add(nccTemplate);
                    }
                    //找到则修改
                    else
                    {
                        AddOrReviseTemplateDatas.ActiveTemplates[index].Dispose();
                        AddOrReviseTemplateDatas.ActiveTemplates[index] = nccTemplate;
                    }
                }
                //如果在不活跃轮型列表中找到 并且 在活跃轮型列表中没找到 则修改
                else if (notActiveIndex >= 0 && activeIndex < 0)
                {
                    HOperatorSet.WriteNccModel(nccTemplate, nPath);
                    var n = AddOrReviseTemplateDatas.NotActiveTemplateNames;
                    int index = AddOrReviseTemplateDatas.NotActiveTemplateNames.FindIndex(x => x == DataGridSelectedItem.WheelType);
                    if (index < 0)
                    {
                        AddOrReviseTemplateDatas.NotActiveTemplateNames.Add(DataGridSelectedItem.WheelType);
                        AddOrReviseTemplateDatas.NotActiveTemplates.Add(nccTemplate);
                    }
                    else
                    {
                        AddOrReviseTemplateDatas.NotActiveTemplates[index].Dispose();
                        AddOrReviseTemplateDatas.NotActiveTemplates[index] = nccTemplate;
                    }
                }
                HOperatorSet.WriteImage(TemplateImage, "tiff", 0, tPath);//保存模板图像
                DataGridSelectedItem.CreationTime = DateTime.Now.ToString("yy-MM-dd HH:mm");
                var sDB = new SqlAccess().SystemDataAccess;
                sDB.Updateable(DataGridSelectedItem).ExecuteCommand();
                TemplateDatas[DataGridSelectedIndex].CreationTime = DataGridSelectedItem.CreationTime;

                //修改自动模式匹配用模板数据
                if (!TemplateDataUpdataControl)
                    TemplateDataUpdataControl = true;
                if (!AutoTemplateDataLoadControl)
                    AutoTemplateDataLoadControl = true;

                SourceTemplateImage.Dispose();
                InPoseWheelImage.Dispose();
                TemplateImage.Dispose();
                EventMessage.SystemMessageDisplay("模板保存成功，型号是：" + DataGridSelectedItem.WheelType, MessageType.Success);
                EventMessage.MessageDisplay("模板保存成功，型号是：" + DataGridSelectedItem.WheelType, true, true);
            }
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        private void DelTemplate()
        {
            if (TemplateDatas.Count == 0)
            {
                EventMessage.SystemMessageDisplay("无模板数据，请先录入模板!", MessageType.Warning);
                return;
            }
            if (DataGridSelectedItem == null)
            {
                EventMessage.SystemMessageDisplay("请先选择轮型!", MessageType.Warning);
                return;
            }
            bool result = WMessageBox.Show("删除确认", "确定删除模板：" + DataGridSelectedItem.WheelType + " 吗？");
            if (result)
            {
                //模板图像保存路径
                string tPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
                //活跃模板保存路径
                string aPath = ActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                //不活跃模板保存路径
                string nPath = NotActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                //删除轮型模板图片
                if (File.Exists(tPath)) File.Delete(tPath);
                //删除活跃轮型模板
                if (File.Exists(aPath)) File.Delete(aPath);
                //删除不活跃轮型模板
                if (File.Exists(nPath)) File.Delete(nPath);

                //查找当前删除轮型的索引
                int dIndex = DelTemplateNames.FindIndex(x => x == DataGridSelectedItem.WheelType);
                if (dIndex < 1) DelTemplateNames.Add(DataGridSelectedItem.WheelType);

                string wheelType = DataGridSelectedItem.WheelType;
                int index = -1;
                //移除模板数据中的选中项
                for (int i = 0; i < TemplateDatas.Count; i++)
                {
                    if (TemplateDatas[i].WheelType.Equals(DataGridSelectedItem.WheelType))
                    {
                        TemplateDatas.RemoveAt(i);
                        index = i;
                        break;
                    }
                }
                //整理Index
                var datas = TemplateDatas.ToList();
                for (int i = 0; i < datas.Count; i++)
                {
                    datas[i].Index = i + 1;
                }
                //更新窗口显示
                TemplateDatas.Clear();
                TemplateDatas.AddRange(datas);
                if (TemplateDatas.Count - 1 >= index)
                {
                    DataGridSelectedItem = TemplateDatas[index];
                    DataGridSelectedIndex = index;
                    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
                else if (TemplateDatas.Count - 1 < index && index != 0)
                {
                    DataGridSelectedItem = TemplateDatas[index - 1];
                    DataGridSelectedIndex = index - 1;
                    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
                else
                {
                    DataGridSelectedItem = null;
                    DataGridSelectedIndex = -1;
                }
                //修改数据库
                var sDB = new SqlAccess().SystemDataAccess;
                sDB.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
                sDB.Insertable(datas).ExecuteCommand();
                //修改自动模式匹配用模板数据
                if (!TemplateDataUpdataControl) TemplateDataUpdataControl = true;
                if (!AutoTemplateDataLoadControl) AutoTemplateDataLoadControl = true;
                EventMessage.SystemMessageDisplay("模板删除成功，轮型是：" + wheelType, MessageType.Success);
                EventMessage.MessageDisplay("模板删除成功，轮型是：" + wheelType, true, true);
            }
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        private void ParameterSetting()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            _dialogService.ShowDialog("ParameterSetting");
        }
        /// <summary>
        /// 显示模板
        /// </summary>
        private void DisplayTemplates()
        {
            if (DataGridSelectedItem == null)
            {
                EventMessage.SystemMessageDisplay("请先选择轮型!", MessageType.Warning);
                return;
            }
            string strPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
            if (File.Exists(strPath))
            {
                HOperatorSet.ReadImage(out HObject Image, strPath);
                TemplateWindowDisplay(Image, null, null, null, null);
            }
            else
            {
                EventMessage.SystemMessageDisplay("轮型" + DataGridSelectedItem.WheelType + "无模板图像，请先录入模板!", MessageType.Warning);
            }
        }
        /// <summary>
        /// 匹配结果
        /// </summary>
        private void MatchResult()
        {
            if (!IsMatchResultDialog)
            {
                IsMatchResultDialog = true;
                _dialogService.Show("MatchResult", dialogResult =>
                {
                    IsMatchResultDialog = false;
                });
            }
        }
        /// <summary>
        /// 识别测试
        /// </summary>
        private void RecognitionTest()
        {
            if (TemplateDataCollection.ActiveTemplateNames.Count == 0 && TemplateDataCollection.NotActiveTemplateNames.Count == 0)
            {
                EventMessage.SystemMessageDisplay("无模板数据，请先录入模板!", MessageType.Warning);
                return;
            }
            if (!SourceTemplateImage.IsInitialized())
            {
                EventMessage.SystemMessageDisplay("无图像数据，请先执行采集图像或读取图片!", MessageType.Warning);
                return;
            }
            DateTime startTime = DateTime.Now;
            RecognitionResultModel results;
            var positioningResult = PositioningWheel(SourceTemplateImage, WheelMinThreshold, 255, WheelMinRadius);
            if (positioningResult.WheelImage != null)
            {
                results = WheelRecognitionAlgorithm(positioningResult.WheelImage, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
            }
            else
                results = WheelRecognitionAlgorithm(SourceTemplateImage, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
            DateTime endTime = DateTime.Now;
            //浇口检测
            //GateDetectionResultModel gateResult = new GateDetectionResultModel();
            //if (positioningResult.WheelImage != null)
            //{
            //    gateResult = GateDetection(positioningResult.WheelImage, positioningResult.CenterRow, positioningResult.CenterColumn, PositioningGateRadius, GateOutMinThreshold, GateMinArea, GateMinRadius);
            //    if (gateResult.DetectionResult)
            //    {
            //        GateContourColor = "green";
            //        GateDetectionResult = "OK";
            //    }
            //    else
            //    {
            //        GateContourColor = "red";
            //        GateDetectionResult = "NG";
            //    }
            //    GateArea = gateResult.GateArea.ToString();
            //    GateRadiu = gateResult.GateRadiu.ToString();
            //    GateDetectionVisibility = Visibility.Visible;
            //}

            HObject templateContour = null;
            if (results.RecognitionWheelType != "NG")
            {
                templateContour = GetAffineTemplateContour(results.TemplateID, results.CenterRow, results.CenterColumn, results.Radian);
                RecognitionWheelType = results.RecognitionWheelType;
                RecognitionSimilarity = results.Similarity.ToString();
            }
            else
            {
                RecognitionWheelType = "NG";
                RecognitionSimilarity = "0";
                //大模型推算
                HTuple hv_DLResult = WheelDeepLearning(SourceTemplateImage);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
                HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
                for (int i = 0; i < names.Length; i++)
                {
                    Console.WriteLine($"数据：{names[i].S} 结果：{confidences[i].D.ToString("0.0000")}");
                }
                if(names.Length > 0 )
                {
                    results.RecognitionWheelType = names[0].S;
                    RecognitionWheelType = names[0].S;
                    RecognitionSimilarity = confidences[0].D.ToString("0.0000");
                }
                hv_DLResult.Dispose();
            }
            TemplateWindowDisplay(SourceTemplateImage, null, positioningResult.WheelContour, templateContour, null);

            //匹配相似度结果显示
            List<MatchResultModel> matchResultModels = new List<MatchResultModel>();
            for (int i = 0; i < results.WheelTypes.Count; i++)
            {
                MatchResultModel data = new MatchResultModel
                {
                    Index = i + 1,
                    WheelType = results.WheelTypes[i],
                    Similarity = results.Similaritys[i].ToString()
                };
                matchResultModels.Add(data);
            }
            EventMessage.MessageHelper.GetEvent<MatchResultDatasDisplayEvent>().Publish(matchResultModels);


            TimeSpan consumeTime = endTime.Subtract(startTime);
            RecognitionConsumptionTime = Convert.ToString(Convert.ToInt32(consumeTime.TotalMilliseconds)) + " ms"; ;
            RecognitionResultDisplay = Visibility.Visible;
        }
        /// <summary>
        /// 模板检查
        /// </summary>
        private void TemplateExamine()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            if (SystemModel)
            {
                EventMessage.SystemMessageDisplay("请在手动模式下执行模板检查!", MessageType.Default);
                return;
            }
            //合并活跃模板名列表与不活跃模板名列表
            List<string> templateNameList = TemplateDataCollection.ActiveTemplateNames.Concat(TemplateDataCollection.NotActiveTemplateNames).ToList();
            //获取所有模板图像路径
            string[] imageFiles = Directory.GetFiles(TemplateImagesPath);
            //判断合并后的模板名列表中每一个轮型是否都包含在模板数据表格中
            bool result = false;
            for (int i = 0; i < TemplateDatas.Count; i++)
            {
                result = templateNameList.Contains(TemplateDatas[i].WheelType);
                if (!result) break;
            }
            //如果都包含，且模板表格轮毂数量与模板图像数量相等，则认为模板正常
            if (result && TemplateDatas.Count == imageFiles.Length)
            {
                EventMessage.SystemMessageDisplay("模板数据正常", MessageType.Success);
                return;
            }
            try
            {
                //1.检查模板表格数据中是否包含合并后的模板名列表中的每一个轮型
                List<string> wheels = TemplateDatas.Select(x => x.WheelType).ToList();
                List<sys_bd_Templatedatamodel> templates = new List<sys_bd_Templatedatamodel>();
                for (int i = 0; i < templateNameList.Count; i++)
                {
                    bool r = wheels.Contains(templateNameList[i]);
                    //如果包含，则将当前轮型添加到新的模板数据列表
                    if (r)
                    {
                        int index = wheels.FindIndex(x => x == templateNameList[i]);
                        templates.Add(TemplateDatas[index]);
                    }
                    //如果不包含，则新建当前模板数据再添加到新的模板数据列表
                    else
                    {
                        sys_bd_Templatedatamodel data = new sys_bd_Templatedatamodel
                        {
                            Index = 999,
                            WheelType = templateNameList[i],
                            UnusedDays = 0,
                            CreationTime = DateTime.Now.ToString("yy-MM-dd HH:mm")
                        };
                        EventMessage.MessageDisplay($"增加模板数据，轮型是：{templateNameList[i]}", true, true);
                        templates.Add(data);
                    }
                }
                //2.根据新列表的数据删除模板图像文件夹中多余的图像
                for (int i = 0; i < imageFiles.Length; i++)
                {
                    int lastIndex = imageFiles[i].LastIndexOf(@"\") + 1;
                    string wheelType = imageFiles[i].Substring(lastIndex, imageFiles[i].Length - lastIndex).Trim('.', 't', 'i', 'f');
                    bool isContain = false;
                    for (int j = 0; j < templates.Count; j++)
                    {
                        if (wheelType == templates[j].WheelType) isContain = true;
                    }
                    if (!isContain)
                    {
                        File.Delete(imageFiles[i]);
                        EventMessage.MessageDisplay($"删除多余模板图像，轮型是：{wheelType}", true, true);
                    }
                }
                //3.更新数据库模板数据，更新显示模板数据
                //根据轮型排序
                var newTemplateDatas = templates.OrderBy(x => x.WheelType).ToList();
                //整理Index
                for (int i = 0; i < newTemplateDatas.Count; i++) newTemplateDatas[i].Index = i + 1;
                //修改显示数据与匹配数据
                AutoTemplateDataLoadControl = true;
                TemplateDatas.Clear();
                TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>(newTemplateDatas);
                //修改数据库
                var sDB = new SqlAccess().SystemDataAccess;
                sDB.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
                sDB.Insertable(newTemplateDatas).ExecuteCommand();
                EventMessage.SystemMessageDisplay("模板检查执行成功！", MessageType.Success);
                EventMessage.MessageDisplay("模板检查执行成功！", true, true);
            }
            catch (Exception ex)
            {
                EventMessage.SystemMessageDisplay($"模板检查执行失败：{ex.Message}", MessageType.Error);
            }
        }
        /// <summary>
        /// 模板窗口显示
        /// </summary>
        /// <param name="templateImage">模板图像</param>
        /// <param name="templateContour">模板轮廓</param>
        /// <param name="inGateContour">浇口轮廓</param>
        private void TemplateWindowDisplay(HObject sourceImage, HObject templateImage, HObject wheelContour, HObject templateContour, HObject gateContour)
        {
            DisplayTemplateImage.Dispose();
            DisplayTemplate.Dispose();
            DisplayWheelContour.Dispose();
            DisplayTemplateContour.Dispose();
            DisplayInGateContour.Dispose();
            if (sourceImage != null)
                DisplayTemplateImage = sourceImage.Clone();
            if (templateImage != null)
                DisplayTemplate = templateImage.Clone();
            if (wheelContour != null)
            {
                LineWidth = 4.0;
                DisplayWheelContour = wheelContour.Clone();
            }
            if (templateContour != null)
            {
                LineWidth = 3.0;
                DisplayTemplateContour = templateContour.Clone();
            }
            if (gateContour != null)
            {
                LineWidth = 3.0;
                DisplayInGateContour = gateContour.Clone();
            }
        }
    }
}

