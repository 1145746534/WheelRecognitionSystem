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
using System.Windows.Threading;
using Prism.Regions;
using System.Timers;


namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class TemplateManagementViewModel : BindableBase, IDialogAware, IDisposable
    {
        private bool _disposed = false;

        #region======属性字段定义======
        public string Title { get; set; } = "模板管理";

        private readonly object _locker = new object();


        private const int KEEP_ALIVE_COUNT = 3;
        private readonly object _lock = new object();


        public event Action<IDialogResult> RequestClose;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }


        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnConfirm()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("saveImageDays", SaveImageDays);
            parameters.Add("maintainQuantity", 8);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void OnCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Unsubscribe from events and release managed resources
                }

                // Free unmanaged resources (if any) and set large fields to null
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 弹窗是否可以关闭
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }
        /// <summary>
        /// 弹窗关闭时
        /// </summary>
        public void OnDialogClosed()
        {
            Dispose();
        }


        private ObservableCollection<sys_bd_Templatedatamodel> _templateDatas;
        /// <summary>
        /// 模板数据
        /// </summary>
        public ObservableCollection<sys_bd_Templatedatamodel> TemplateDatas
        {
            get { return _templateDatas; }
            set
            {
                SetProperty(ref _templateDatas, value);
            }
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

        private string _recognitionWay;
        /// <summary>
        /// 识别方式
        /// </summary>
        public string RecognitionWay
        {
            get { return _recognitionWay; }
            set { SetProperty(ref _recognitionWay, value); }
        }

        private string _innerCircleGary;
        /// <summary>
        /// 内圈灰度
        /// </summary>
        public string InnerCircleGary
        {
            get { return _innerCircleGary; }
            set { SetProperty(ref _innerCircleGary, value); }
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
        private Visibility _grayDisplay;
        /// <summary>
        /// 内圈灰度显示
        /// </summary>
        public Visibility GrayDisplay
        {
            get { return _grayDisplay; }
            set { SetProperty(ref _grayDisplay, value); }
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
        private string _maskContent;
        /// <summary>
        /// 掩膜显示内容
        /// </summary>
        public string MASKContent
        {
            get { return _maskContent; }
            set { SetProperty(ref _maskContent, value); }
        }
        /// <summary>
        /// 是否掩膜中
        /// </summary>
        public bool isMask = false;
        /// <summary>
        /// 是否正在擦除
        /// </summary>
        private bool _isErasing = false;
        /// <summary>
        /// 橡皮擦大小
        /// </summary>
        private double _eraserSize;
        /// <summary>
        /// 掩膜大小
        /// </summary>
        public double EraserSize
        {
            get { return _eraserSize; }
            set { SetProperty(ref _eraserSize, value); }
        }

        /// <summary>
        /// 弹窗服务
        /// </summary>
        readonly IDialogService _dialogService;


        /// <summary>
        /// 匹配结果弹窗是否打开
        /// </summary>
        private bool IsMatchResultDialog = false;

        private static System.Timers.Timer _cleanupTimer;

        #endregion

        #region 非托管内存
        // ----- 用于显示

        private HObject _displayTemplateImage;
        /// <summary>
        /// 用于显示的模板原始图像
        /// </summary>
        public HObject DisplayTemplateImage
        {
            get { return _displayTemplateImage; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_displayTemplateImage);
                SetProperty<HObject>(ref _displayTemplateImage, value);
            }
        }

        private HObject _displayTemplate;
        /// <summary>
        /// 用于显示的制作模板的图像
        /// </summary>
        public HObject DisplayTemplate
        {
            get { return _displayTemplate; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_displayTemplate);
                SetProperty<HObject>(ref _displayTemplate, value);
            }
        }

        private HObject _displayTemplateContour;
        /// <summary>
        /// 用于显示的模板轮廓
        /// </summary>
        public HObject DisplayTemplateContour
        {
            get { return _displayTemplateContour; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_displayTemplateContour);
                SetProperty<HObject>(ref _displayTemplateContour, value);
            }
        }

        private HObject _displayWheelContour;
        /// <summary>
        /// 用于显示轮毂的轮廓
        /// </summary>
        public HObject DisplayWheelContour
        {
            get { return _displayWheelContour; }
            set
            {
                // 释放旧对象
                SafeHalconDispose(_displayWheelContour);
                SetProperty(ref _displayWheelContour, value);
            }
        }

        //private HObject _displayInGateContour;
        ///// <summary>
        ///// 用于显示的浇口轮廓
        ///// </summary>
        //public HObject DisplayInGateContour
        //{
        //    get { return _displayInGateContour; }
        //    set { SetProperty<HObject>(ref _displayInGateContour, value); }
        //}

        // ----- 用于操作
        private HObject _sourceTemplateImage;

        /// <summary>
        /// 读取或采集的原始模板图像
        /// </summary>
        private HObject SourceTemplateImage
        {
            get { return _sourceTemplateImage; }
            set
            {
                SafeHalconDispose(_sourceTemplateImage);
                SetProperty(ref _sourceTemplateImage, value);
            }
        }

        private HObject _ho_Region_Removeds;
        /// <summary>
        /// 掩膜区域
        /// </summary>
        private HObject ho_Region_Removeds
        {
            get { return _ho_Region_Removeds; }
            set
            {
                SafeHalconDispose(_ho_Region_Removeds);
                SetProperty(ref _ho_Region_Removeds, value);
            }
        }

        private HObject _InPoseWheelImage;
        /// <summary>
        /// 定位轮毂获取到的轮毂图像
        /// </summary>
        private HObject InPoseWheelImage
        {
            get { return _InPoseWheelImage; }
            set
            {
                SafeHalconDispose(_InPoseWheelImage);
                SetProperty(ref _InPoseWheelImage, value);
            }
        }

        private HObject _templateImage = new HObject();
        /// <summary>
        ///用于制作和保存的模板图像
        /// </summary>
        private HObject TemplateImage
        {
            get { return _templateImage; }
            set
            {
                SafeHalconDispose(_templateImage);
                SetProperty(ref _templateImage, value);
            }
        }
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
        /// <summary>
        /// 模板窗口鼠标按下命令
        /// </summary>
        public ICommand OnHalconMouseDownCommand { get; set; }
        /// <summary>
        /// 模板窗口鼠标抬起命令
        /// </summary>
        public ICommand OnHalconMouseUpCommand { get; set; }

        #endregion

        #region  待补录数据属性

        private ObservableCollection<Tbl_productiondatamodel> _unrecognizedDatas;
        /// <summary>
        /// 未识别数据
        /// </summary>
        public ObservableCollection<Tbl_productiondatamodel> UnrecognizedDatas
        {
            get { return _unrecognizedDatas; }
            set { SetProperty(ref _unrecognizedDatas, value); }
        }
        private int _unrDataGridSelectedIndex;
        /// <summary>
        /// 待补录数据窗口选中的行索引
        /// </summary>
        public int UnrDataGridSelectedIndex
        {
            get { return _unrDataGridSelectedIndex; }
            set { SetProperty(ref _unrDataGridSelectedIndex, value); }
        }

        private Tbl_productiondatamodel _unrDataGridSelectedItem;
        /// <summary>
        /// 待补录数据窗口选中的行
        /// </summary>
        public Tbl_productiondatamodel UnrDataGridSelectedItem
        {
            get { return _unrDataGridSelectedItem; }
            set
            {
                SetProperty(ref _unrDataGridSelectedItem, value);
            }
        }
        public ICommand UnrMouseLeftButtonDownCommand { get; set; }

        public DispatcherTimer _dispatcherTimer;


        private string _unrIndex;

        public string UnrIndex
        {
            get { return _unrIndex; }
            set { SetProperty(ref _unrIndex, value); }
        }

        private string _unrWheelType;

        public string UnrWheelType
        {
            get { return _unrWheelType; }
            set { SetProperty(ref _unrWheelType, value); }
        }

        private string _recWheelType;

        /// <summary>
        /// 补录模板名称
        /// </summary>
        public string RecWheelType
        {
            get { return _recWheelType; }
            set
            {

                // 过滤非法字符并转换为大写
                var filteredValue = new string(value?
                    .Where(c => char.IsUpper(c) || char.IsLower(c) || char.IsDigit(c))
                    .ToArray());
                filteredValue = filteredValue.ToUpper();
                if (SetProperty(ref _recWheelType, filteredValue))
                {
                    // 属性更改逻辑
                }

            }
        }

        private string _recWheelStyle;

        public string RecWheelStyle
        {
            get { return _recWheelStyle; }
            set { SetProperty(ref _recWheelStyle, value); }
        }

        private bool _handInput;
        /// <summary>
        /// 手动输入轮毂型号
        /// </summary>
        public bool HandInput
        {
            get { return _handInput; }
            set { SetProperty(ref _handInput, value); }
        }

        public ICommand HubChangesCommand { get; set; }

        public ICommand OneClickAddCommand { get; set; }

        private IRegionManager _regionManager;

        #endregion

        public TemplateManagementViewModel(IDialogService dialogService, IRegionManager regionManager)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;
            TemplateBtnCommand = new DelegateCommand<string>(TemplateButtonCommand);
            MouseLeftButtonDownCommand = new DelegateCommand<object>(MouseLeftButtonDown);
            HWindowMouseMoveCommand = new DelegateCommand<object>(HWindowMouseMove);
            OnHalconMouseDownCommand = new DelegateCommand<object>(OnHalconMouseDown);
            OnHalconMouseUpCommand = new DelegateCommand<object>(OnHalconMouseUp);

            // 初始化命令（带 Halcon 事件参数）

            UnrecognizedDatas = new ObservableCollection<Tbl_productiondatamodel>();
            DisplayTemplateImage = new HObject();
            DisplayTemplate = new HObject();
            DisplayWheelContour = new HObject();
            DisplayTemplateContour = new HObject();
            //DisplayInGateContour = new HObject();
            TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>();
            LoadedTemplateDatas();
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            GrayDisplay = Visibility.Collapsed;
            ImageDisVisibility = Visibility.Collapsed;
            MASKContent = "图像掩膜";
           

        }



        #region  模板窗口显示

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
                    sDB.Close(); sDB.Dispose();
                }
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
            EventMessage.MessageHelper.GetEvent<TemplateClearEvent>().Publish(string.Empty);
            if (sourceImage != null)
                DisplayTemplateImage = CloneImageSafely(sourceImage);
            if (templateImage != null)
                DisplayTemplate = CloneImageSafely(templateImage);
            if (wheelContour != null)
            {
                LineWidth = 4.0;
                DisplayWheelContour = CloneImageSafely(wheelContour);
            }
            if (templateContour != null)
            {
                LineWidth = 3.0;
                DisplayTemplateContour = CloneImageSafely(templateContour);
            }
            if (gateContour != null)
            {
                LineWidth = 3.0;
                //DisplayInGateContour = gateContour;
            }
            //显示完后可以立即清除内存
            SafeHalconDispose(DisplayTemplateImage);
            SafeHalconDispose(DisplayTemplate);
            SafeHalconDispose(DisplayWheelContour);
            SafeHalconDispose(DisplayTemplateContour);
        }

        #endregion

        #region 加载 增加 删除 插入 修改 查找 模板参数 显示 整理数据




        /// <summary>
        /// 软件启动时加载模板数据到使用区
        /// </summary>
        private void LoadedTemplateDatas()
        {
            var db = new SqlAccess().SystemDataAccess;
            List<sys_bd_Templatedatamodel> Datas = db.Queryable<sys_bd_Templatedatamodel>().ToList();
            DisplayTemplateDatas(Datas);
            db.Close(); db.Dispose();           
        }

        /// <summary>
        /// 删除使用区模型跟本地模型
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        private void DeleteModel(string path, string name)
        {
            if (File.Exists(path)) //文件存在
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 将使用区的数据反向刷新到数据表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        //private void CleanupHandler(object sender, ElapsedEventArgs e)
        //{
        //    for (int i = 0; i < TemplateModels.Count; i++)
        //    {
        //        string _type = TemplateModels[i].WheelType;
        //        sys_bd_Templatedatamodel result = TemplateDatas.FirstOrDefault(item => string.Equals(item.WheelType, _type, StringComparison.OrdinalIgnoreCase));
        //        if (result != null)
        //        {
        //            result.LastUsedTime = TemplateModels[i].LastUsedTime;
        //        }
        //    }
        //    InsertTemplateDatas(TemplateDatas.ToList());
        //}



        /// <summary>
        /// 插入整个数据
        /// </summary>
        /// <param name="newDatas"></param>
        private void InsertTemplateDatas(List<sys_bd_Templatedatamodel> newDatas)
        {
            var db = new SqlAccess().SystemDataAccess;
            db.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
            db.Insertable(newDatas).ExecuteCommand();
            db.Close(); db.Dispose();
        }

        /// <summary>
        /// 修改单个模板参数
        /// </summary>
        /// <param name="newModel"></param>
        private void UpdataTemplateData(sys_bd_Templatedatamodel newModel)
        {
            var db = new SqlAccess().SystemDataAccess;
            db.Updateable(newModel).ExecuteCommand();
            db.Close(); db.Dispose();
        }

        /// <summary>
        /// 查找指定 WheelType 的第一条记录
        /// </summary>
        /// <param name="targetType">要查找的 WheelType</param>
        /// <returns>找到的 Templatedata，未找到时返回null</returns>
        public sys_bd_Templatedatamodel GetTemplatedataForType(string targetType)
        {
            var db = new SqlAccess().SystemDataAccess;
            sys_bd_Templatedatamodel foundItem = db.Queryable<sys_bd_Templatedatamodel>()
                                            .Where(w => w.WheelType == targetType).First();
            db.Close(); db.Dispose();
            // 检查是否找到
            if (foundItem != null)
            {
                return foundItem;
            }

            return null;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="Datas"></param>
        private void DisplayTemplateDatas(List<sys_bd_Templatedatamodel> Datas)
        {
            //此处必须使用Invoke
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                TemplateDatas.Clear();
                TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>(Datas);
            }));
        }

        /// <summary>
        /// 整理TemplateDatas数据
        /// </summary>
        private void OrganizeTemplateDatas()
        {
            //内部数据重新整理

            //数据根据轮型还有轮毂样式排序
            var datas = TemplateDatas.OrderBy(x => x.WheelType).ThenBy(x => x.WheelStyle).ToList();
            for (int i = 0; i < datas.Count; i++)
            {
                datas[i].Index = i + 1;
            }
            //更新窗口显示
            TemplateDatas.Clear();
            TemplateDatas.AddRange(datas);
            //修改数据库
            InsertTemplateDatas(datas);
        }



        #endregion

        #region   操作模板
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
            else if (obj == "图像掩膜") MASKClick(obj);
            else if (obj == "关闭掩膜") MASKClick(obj);
            else if (obj == "匹配结果") MatchResult();
            else if (obj == "识别测试") RecognitionTest();
            else if (obj == "保存模板") SaveTemplate();
            //else if (obj == "模板检查") TemplateExamine();
            else return;
        }


        /// <summary>
        /// 添加轮型 *
        /// </summary>
        private void AddTemplate()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            //GateDetectionVisibility = Visibility.Collapsed;
            GrayDisplay = Visibility.Collapsed;
            //弹出窗口带返回结果写法
            //_dialogService.ShowDialog("WheelTypeSetting", new Action<IDialogResult>(AddWheelResult));
            int ID = TemplateDatas.Count;
            var parameters = new DialogParameters
            {
                { "templateDatas", TemplateDatas }
            };
            _dialogService.ShowDialog("WheelTypeSetting", parameters,
                new Action<IDialogResult>((IDialogResult result) =>
            {
                if (result.Parameters.Count != 0)
                {
                    if (result.Parameters.GetValue<string>("set") == "add_OK")
                    {
                        //插入数据库
                        InsertTemplateDatas(TemplateDatas.ToList());
                        int index = Convert.ToInt32(result.Parameters.GetValue<string>("Index"));
                        DataGridSelectedItem = TemplateDatas[index];
                        DataGridSelectedIndex = index;
                    }

                    //LoadedTemplateDatas();
                    //DataGridSelectedItem = result.Parameters.GetValue<sys_bd_Templatedatamodel>("set");
                    //DataGridSelectedIndex = DataGridSelectedItem.Index - 1;
                    //发布修改项用于模板数据显示控件滚动到修改项
                    //EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
            }));
            //弹出窗口不带返回结果写法
            //_dialogService.ShowDialog("WheelTypeSetting");
            //定位到添加的位置

        }
        /// <summary>
        /// 读取图像
        /// </summary>
        private void ReadImage()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            GrayDisplay = Visibility.Collapsed;
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
                    SafeHalconDispose(SourceTemplateImage);
                    HOperatorSet.ReadImage(out HObject image, File_Name);
                    SourceTemplateImage = image;

                    ImageDisVisibility = Visibility.Visible;
                    ImageDisName = "手动读取图片";
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
            //RecognitionResultDisplay = Visibility.Collapsed;
            //GateDetectionVisibility = Visibility.Collapsed;
            //GrayDisplay = Visibility.Collapsed;
            //if (SystemModel)
            //{
            //    EventMessage.SystemMessageDisplay("请在手动模式下采集图像!", MessageType.Default);
            //    return;
            //}
            //try
            //{
            //    HOperatorSet.GrabImageAsync(out HObject image, ExternalConnections.CameraHandle, -1);
            //    HOperatorSet.Rgb1ToGray(image, out HObject grayImage);
            //    SourceTemplateImage.Dispose();
            //    if (CroppingOrNot)
            //    {
            //        Cropping(grayImage, out HObject SourceImage);
            //        HOperatorSet.ZoomImageFactor(SourceImage, out SourceTemplateImage, ScalingCoefficient, ScalingCoefficient, "constant");
            //    }
            //    else
            //    {
            //        HOperatorSet.ZoomImageFactor(grayImage, out SourceTemplateImage, ScalingCoefficient, ScalingCoefficient, "constant");
            //    }
            //    TemplateWindowDisplay(SourceTemplateImage, null, null, null, null);
            //}
            //catch
            //{
            //    EventMessage.SystemMessageDisplay("采集异常，请检查相机连接!", MessageType.Error);
            //}
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
                    EventMessage.SystemMessageDisplay($"轮毂半径：{pResult.Radius.D}。", MessageType.Default);
                    pResult.CenterColumn?.Dispose();
                    pResult.CenterRow?.Dispose();
                    pResult.Radius?.Dispose();

                    InPoseWheelImage = pResult.WheelImage;
                    //EventMessage.MessageHelper.GetEvent<TemplateClearEvent>().Publish(SourceTemplateImage);

                    TemplateWindowDisplay(SourceTemplateImage, null, pResult.WheelContour, null, null);
                    pResult.WheelContour.Dispose();
                    InnerCircleGary = pResult.InnerCircleMean.ToString();
                    pResult = null;
                    GrayDisplay = Visibility.Visible;
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
            HOperatorSet.ReduceDomain(reduced, regionDifference, out HObject _tImage);
            TemplateImage = _tImage.Clone();
            SafeHalconDispose(_tImage);
            TemplateWindowDisplay(null, TemplateImage, null, null, null);
            row.Dispose(); column.Dispose(); radius.Dispose(); circleSector.Dispose();
            reduced.Dispose(); region.Dispose(); connectedRegions.Dispose(); regionClosing.Dispose();
            RegionOpening.Dispose(); selectedRegions.Dispose(); regionUnion.Dispose();
            regionDifference.Dispose();
        }
        /// <summary>
        /// 掩膜
        /// </summary>
        /// <param name="obj"></param>
        private void MASKClick(string obj)
        {
            if ("图像掩膜" == obj)
            {
                if (TemplateImage != null && TemplateImage.IsInitialized())
                {
                    HObject temp = null; // 创建一个临时变量
                    HOperatorSet.GenEmptyObj(out temp);
                    ho_Region_Removeds = temp; // 然后将临时变量赋值给属性
                    isMask = true;
                    MASKContent = "关闭掩膜";
                }
                else
                    EventMessage.SystemMessageDisplay("无模板图像", MessageType.Error);


            }
            else
            {
                isMask = false;
                MASKContent = "图像掩膜";
                if (TemplateImage != null && ho_Region_Removeds != null
                    && TemplateImage.IsInitialized() && ho_Region_Removeds.IsInitialized())
                {
                    //HOperatorSet.Difference(TemplateImage, ho_Region_Removeds, out HObject regionDifference); //差集操作（获取未被选中的区域）
                    //HOperatorSet.ReduceDomain(TemplateImage, regionDifference, out HObject TemplateImage1);
                    //SafeHalconDispose(TemplateImage);
                    //SafeHalconDispose(regionDifference);
                    //SafeHalconDispose(ho_Region_Removeds);

                    //TemplateImage = TemplateImage1;
                }

            }
        }

        private void OnHalconMouseDown(object obj)
        {
            var data = obj as HSmartWindowControlWPF.HMouseEventArgsWPF;
            if (data != null && data.Button.ToString() == "Right")
            {
                Console.WriteLine($"按钮：{data.Button}");
                if (isMask) //掩膜模式
                {
                    _isErasing = true;
                }
            }
        }

        private void OnHalconMouseUp(object obj)
        {
            var data = obj as HSmartWindowControlWPF.HMouseEventArgsWPF;
            if (data != null && data.Button.ToString() == "Right")
                _isErasing = false;
        }

        /// <summary>
        /// 模板窗口鼠标移动
        /// </summary>
        /// <param name="obj"></param>
        private void HWindowMouseMove(object obj)
        {
            var data = obj as HSmartWindowControlWPF.HMouseEventArgsWPF;
            if (data.Button.ToString() == "Right" && _isErasing)
            {
                if (TemplateImage != null && TemplateImage.IsInitialized())
                {

                    CoverUp1(TemplateImage, data.Row, data.Column);
                    TemplateWindowDisplay(null, TemplateImage, null, null, null);


                }
            }
            else
            {

                if (SourceTemplateImage != null && SourceTemplateImage.IsInitialized())
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
        }

        public void CoverUp1(HObject image, double row, double column)
        {
            HObject ho_EraserRegion = null;
            HObject regionDifference = null;
            HObject TemplateImage1 = null;
            HTuple hRow = null;
            HTuple hColumn = null;
            HTuple hEraserSize = null;

            try
            {
                // 创建临时对象
                hRow = new HTuple(row);
                hColumn = new HTuple(column);
                hEraserSize = new HTuple(EraserSize);

                // 生成橡皮擦区域
                HOperatorSet.GenCircle(out ho_EraserRegion, hRow, hColumn, hEraserSize);

                // 执行图像处理
                HOperatorSet.Difference(image, ho_EraserRegion, out regionDifference);
                HOperatorSet.ReduceDomain(image, regionDifference, out TemplateImage1);

                // 先保存旧图像引用
                HObject oldImage = TemplateImage;

                // 更新模板图像
                TemplateImage = TemplateImage1.Clone();

                // 安全释放旧图像
                SafeDisposeHObject(ref oldImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CoverUp:{ex}");
            }
            finally
            {
                // 确保所有临时对象都被释放
                SafeDisposeHObject(ref ho_EraserRegion);
                SafeDisposeHObject(ref regionDifference);
                SafeDisposeHObject(ref TemplateImage1);
                SafeDisposeHTuple(ref hRow);
                SafeDisposeHTuple(ref hColumn);
                SafeDisposeHTuple(ref hEraserSize);
            }
        }
        public void CoverUp(HObject image, double row, double column)
        {
            try
            {
                //生成橡皮擦擦过的区域 根据鼠标位置和橡皮擦尺寸生成圆形区域
                HOperatorSet.GenCircle(out HObject ho_EraserRegion, row, column, EraserSize);
                // 将新擦过的区域添加到已移除区域集合中
                HObject ExpTmpOutVar_0;
                HOperatorSet.Union2(ho_Region_Removeds, ho_EraserRegion, out ExpTmpOutVar_0
                    );

                SafeHalconDispose(ho_EraserRegion);
                SafeHalconDispose(ho_Region_Removeds);
                ho_Region_Removeds = ExpTmpOutVar_0;
                //*获取绘制了感兴趣区域、屏蔽区域的图像（透明色）

                HTuple hv_color = new HTuple();
                hv_color[0] = 0;
                hv_color[1] = 0;
                hv_color[2] = 0;
                //HOperatorSet.SetColor(hWindowControlWPF.HalconWindow, "red"); // 更改颜色为红色。
                //HOperatorSet.OverpaintRegion(ImageRGB4, ho_Region_Removeds, hv_color, "fill"); // 填充移除区域。
                //HOperatorSet.AddImage(srcImageRGB2, ImageRGB4, out HObject ImageResult3, 0.5, 0); // 更新显示效果。
                HOperatorSet.OverpaintRegion(image, ho_Region_Removeds, hv_color, "fill"); // 填充移除区域。
                hv_color.Dispose();                                                                 //hv_color.Dispose();
                                                                                                    //HOperatorSet.AddImage(TemplateImageRGB1, TemplateImageRGB2, out HObject ImageResult3, 0.5, 0); // 更新显示效果。                    
                                                                                                    //HOperatorSet.DispObj(ImageResult3, hWindowControlWPF.HalconWindow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CoverUp:{ex.ToString()}");
            }
        }

        // 专门处理 HObject 的释放
        public static void SafeDisposeHObject(ref HObject obj)
        {
            if (obj != null && obj.IsInitialized())
            {
                obj.Dispose();
                obj = null;
            }
        }

        // 专门处理 HTuple 的释放
        public static void SafeDisposeHTuple(ref HTuple tuple)
        {
            if (tuple != null)
            {
                tuple.Dispose();
                tuple = null;
            }
        }
        /// <summary>
        /// 保存模板
        /// </summary>
        private async void SaveTemplate()
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
                if (MASKContent == "关闭掩膜")
                {
                    MASKClick("关闭掩膜");
                }

                ////生成NCC模板
                //HOperatorSet.CreateNccModel(TemplateImage, "auto", AngleStart, AngleExtent, 0.05, "use_polarity", out HTuple nccTemplate);
                ////模板图像保存路径
                //string tPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
                ////活跃模板保存路径
                //string aPath = ActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";

                //HOperatorSet.WriteNccModel(nccTemplate, aPath);
                //HOperatorSet.WriteImage(TemplateImage, "tiff", 0, tPath);//保存模板图像
                //DataGridSelectedItem.CreationTime = DateTime.Now.ToString("yy-MM-dd HH:mm");
                //DataGridSelectedItem.InnerCircleGary = float.Parse(InnerCircleGary);
                //DataGridSelectedItem.TemplatePath = aPath;
                //DataGridSelectedItem.TemplatePicturePath = tPath;
                //UpdataTemplateData(DataGridSelectedItem);
                //addModel(nccTemplate, DataGridSelectedItem.WheelType, DateTime.Now);

                //SourceTemplateImage.Dispose();
                //InPoseWheelImage.Dispose();
                //TemplateImage.Dispose();

                try
                {
                    string tPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
                    string aPath = ActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                    // NCC模板生成及保存
                    HTuple nccTemplate = new HTuple();

                    // 在后台线程执行耗时操作
                    await Task.Run(async () =>
                    {

                        HOperatorSet.CreateNccModel(TemplateImage, "auto", AngleStart, AngleExtent, 0.05, "use_polarity", out nccTemplate);
                        // 异步保存模板文件
                        await Task.WhenAll(
                            Task.Run(() => HOperatorSet.WriteNccModel(nccTemplate, aPath)),
                            Task.Run(() => HOperatorSet.WriteImage(TemplateImage, "tiff", 0, tPath))
                        );
                    });
                    SafeHalconDispose(nccTemplate);
                    // 在UI线程更新数据 (确保线程安全)
                    DataGridSelectedItem.CreationTime = DateTime.Now.ToString("yy-MM-dd HH:mm");
                    DataGridSelectedItem.InnerCircleGary = float.Parse(InnerCircleGary);
                    DataGridSelectedItem.TemplatePath = aPath;
                    DataGridSelectedItem.TemplatePicturePath = tPath;
                    DataGridSelectedItem.LastUsedTime = DateTime.Now;
                    UpdataTemplateData(DataGridSelectedItem); // 假设这是数据层更新              
                }
                finally
                {
                    // 确保资源释放
                    SafeHalconDispose(SourceTemplateImage);
                    SafeHalconDispose(InPoseWheelImage);
                    SafeHalconDispose(TemplateImage);
                    SafeHalconDispose(ho_Region_Removeds);

                }

                EventMessage.MessageHelper.GetEvent<TemplateClearEvent>().Publish(string.Empty);
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
                ////模板图像保存路径
                //string tPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
                ////活跃模板保存路径
                //string aPath = ActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                ////不活跃模板保存路径
                //string nPath = NotActiveTemplatesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".ncm";
                ////删除轮型模板图片
                //if (File.Exists(tPath)) 
                //    File.Delete(tPath);
                ////删除活跃轮型模板
                //if (File.Exists(aPath)) 
                //    File.Delete(aPath);
                ////删除不活跃轮型模板
                //if (File.Exists(nPath)) 
                //    File.Delete(nPath);


                if (File.Exists(DataGridSelectedItem.TemplatePicturePath))
                    File.Delete(DataGridSelectedItem.TemplatePicturePath);
                //删除轮型模板
                if (File.Exists(DataGridSelectedItem.TemplatePath))
                    File.Delete(DataGridSelectedItem.TemplatePath);

                int deleteIndex = DataGridSelectedItem.Index - 1; //被删除项
                string wheelType = DataGridSelectedItem.WheelType;

                DeleteModel(DataGridSelectedItem.TemplatePath, wheelType);
                TemplateDatas.RemoveAt(deleteIndex);

                OrganizeTemplateDatas();


                //光标显示
                if (TemplateDatas.Count - 1 >= deleteIndex)
                {
                    DataGridSelectedItem = TemplateDatas[deleteIndex];
                    DataGridSelectedIndex = deleteIndex;
                    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
                else if (TemplateDatas.Count - 1 < deleteIndex && deleteIndex != 0)
                {
                    DataGridSelectedItem = TemplateDatas[deleteIndex - 1];
                    DataGridSelectedIndex = deleteIndex - 1;
                    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                }
                else
                {
                    DataGridSelectedItem = null;
                    DataGridSelectedIndex = -1;
                }


                //var sDB = new SqlAccess().SystemDataAccess;
                //sDB.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
                //sDB.Insertable(datas).ExecuteCommand();


                //string wheelType = DataGridSelectedItem.WheelType;
                //int index = -1;
                ////移除模板数据中的选中项
                //for (int i = 0; i < TemplateDatas.Count; i++)
                //{
                //    if (TemplateDatas[i].WheelType.Equals(DataGridSelectedItem.WheelType))
                //    {
                //        TemplateDatas.RemoveAt(i);
                //        index = i;
                //        break;
                //    }
                //}
                //整理Index
                //var datas = TemplateDatas.ToList();
                //for (int i = 0; i < datas.Count; i++)
                //{
                //    datas[i].Index = i + 1;
                //}
                ////更新窗口显示
                //TemplateDatas.Clear();
                //TemplateDatas.AddRange(datas);
                //if (TemplateDatas.Count - 1 >= index)
                //{
                //    DataGridSelectedItem = TemplateDatas[index];
                //    DataGridSelectedIndex = index;
                //    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                //}
                //else if (TemplateDatas.Count - 1 < index && index != 0)
                //{
                //    DataGridSelectedItem = TemplateDatas[index - 1];
                //    DataGridSelectedIndex = index - 1;
                //    EventMessage.MessageHelper.GetEvent<TemplateDataEditEvent>().Publish(DataGridSelectedItem);
                //}
                //else
                //{
                //    DataGridSelectedItem = null;
                //    DataGridSelectedIndex = -1;
                //}
                ////修改数据库
                //var sDB = new SqlAccess().SystemDataAccess;
                //sDB.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
                //sDB.Insertable(datas).ExecuteCommand();
                //修改自动模式匹配用模板数据
               
               
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
            GrayDisplay = Visibility.Collapsed;
            _dialogService.ShowDialog("ParameterSetting");
        }
        /// <summary>
        /// 显示模板 *
        /// </summary>
        private void DisplayTemplates()
        {
            if (DataGridSelectedItem == null)
            {
                EventMessage.SystemMessageDisplay("请先选择轮型!", MessageType.Warning);
                return;
            }
            ImageDisVisibility = Visibility.Hidden;
            //ImageDisName = "模板";
            string strPath = TemplateImagesPath.Replace(@"\", "/") + @"/" + DataGridSelectedItem.WheelType + ".tif";
            strPath = DataGridSelectedItem.TemplatePicturePath;
            if (File.Exists(strPath))
            {
                HOperatorSet.ReadImage(out HObject Image, strPath);
                TemplateImage = Image.Clone();
                InnerCircleGary = DataGridSelectedItem.InnerCircleGary.ToString();
                TemplateWindowDisplay(Image, null, null, null, null);
                SafeHalconDispose(Image);
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
            //if (TemplateModels.Count == 0)
            //{
            //    EventMessage.SystemMessageDisplay("无模板数据，请先录入模板!", MessageType.Warning);
            //    return;
            //}
            ////if (TemplateDataCollection.ActiveTemplateNames.Count == 0 && TemplateDataCollection.NotActiveTemplateNames.Count == 0)
            ////{
            ////    EventMessage.SystemMessageDisplay("无模板数据，请先录入模板!", MessageType.Warning);
            ////    return;
            ////}
            //if (!SourceTemplateImage.IsInitialized())
            //{
            //    EventMessage.SystemMessageDisplay("无图像数据，请先执行采集图像或读取图片!", MessageType.Warning);
            //    return;
            //}

            //RecognitionWay = " 传统视觉";

            //DateTime startTime = DateTime.Now;
            ////定位轮毂
            //PositioningWheelResultModel pResult = PositioningWheel(SourceTemplateImage, WheelMinThreshold, 255, WheelMinRadius);
            ////存储识别结果
            //RecognitionResultModel recognitionResult = new RecognitionResultModel();
            //HObject imageRecogn = new HObject();
            ////如果定位到轮毂
            //if (pResult.WheelImage != null)
            //    imageRecogn = pResult.WheelImage;
            //else
            //    imageRecogn = SourceTemplateImage;

            ////没有定位到轮毂            
            ////recognitionResult = WheelRecognitionAlgorithm(image, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);

            ////轮毂识别 传统视觉
            ////recognitionResult = WheelRecognitionAlgorithm(imageRecogn, TemplateDataCollection, AngleStart, AngleExtent, MinSimilarity);
            //List<RecognitionResultModel> list = new List<RecognitionResultModel>();
            //recognitionResult = WheelRecognitionAlgorithm(imageRecogn, TemplateModels, AngleStart, AngleExtent, MinSimilarity,out list);
            //InnerCircleGary = pResult.InnerCircleMean.ToString();
            //recognitionResult.FullFigureGary = pResult.FullFigureGary;
            //recognitionResult.InnerCircleGary = pResult.InnerCircleMean;
            //DateTime endTime = DateTime.Now;
            //HObject templateContour = null;
            //if (recognitionResult.RecognitionWheelType != "NG")
            //{
            //    templateContour = GetAffineTemplateContour(GetHTupleByName(recognitionResult.RecognitionWheelType), recognitionResult.CenterRow, recognitionResult.CenterColumn, recognitionResult.Radian);
            //    RecognitionWheelType = recognitionResult.RecognitionWheelType;
            //    RecognitionSimilarity = recognitionResult.Similarity.ToString();
            //    ReleaseUnusedTemplates();

            //}
            //else
            //{
            //    //RecognitionWheelType = "NG";
            //    //RecognitionSimilarity = "0";
            //    //RecognitionWay = " 大模型";
            //    ////大模型推算
            //    //HTuple hv_DLResult = WheelDeepLearning(SourceTemplateImage, hv_DLModelHandle, DisplayInterfaceViewModel.hv_DLPreprocessParam);
            //    //HOperatorSet.GetDictTuple(hv_DLResult, "classification_class_names", out HTuple names);
            //    //HOperatorSet.GetDictTuple(hv_DLResult, "classification_confidences", out HTuple confidences);
            //    //if (names.Length > 0 && confidences[0].D > 0.85) //识别结果
            //    //{
            //    //    recognitionResult.RecognitionWheelType = names[0].S;
            //    //    recognitionResult.Similarity = double.Parse(confidences[0].D.ToString("0.0000"));
            //    //    RecognitionWheelType = names[0].S;
            //    //    RecognitionSimilarity = confidences[0].D.ToString("0.0000");
            //    //}
            //    //list = new List<RecognitionResultModel>();
            //    //for (int i = 0; i < names.Length; i++)
            //    //{
            //    //    double similar = double.Parse(confidences[i].D.ToString("0.0000"));
            //    //    string name = names[i].S;
            //    //    list.Add(new RecognitionResultModel() { Similarity = similar, RecognitionWheelType = name });
            //    //    Console.WriteLine($"数据：{name} 结果：{similar}");
            //    //}
            //    //hv_DLResult.Dispose();
            //}


            //TemplateWindowDisplay(SourceTemplateImage, null, pResult.WheelContour, templateContour, null);
            //SafeHalconDispose(templateContour);
            ////匹配相似度结果显示
            //List<MatchResultModel> matchResultModels = new List<MatchResultModel>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    //最后识别时间更新
            //    MatchResultModel data = new MatchResultModel
            //    {
            //        Index = i + 1,
            //        WheelType = list[i].RecognitionWheelType,
            //        Similarity = list[i].Similarity.ToString(),
            //        FullFigureGary = list[i].FullFigureGary.ToString(),
            //        InnerCircleGary = list[i].InnerCircleGary.ToString()
            //    };
            //    matchResultModels.Add(data);
            //}
            //EventMessage.MessageHelper.GetEvent<MatchResultDatasDisplayEvent>().Publish(matchResultModels);

            //if (recognitionResult.RecognitionWheelType != "NG" && ImageDisName == "待补录轮毂"
            //    && RecognitionWay != " 大模型")
            //{
            //    string type = recognitionResult.RecognitionWheelType;

            //    sys_bd_Templatedatamodel data = GetTemplatedataForType(type);
            //    RecWheelType = data.WheelType.Replace("_", "");
            //    RecWheelStyle = data.WheelStyle;
            //    HandInput = true;
            //}

            //TimeSpan consumeTime = endTime.Subtract(startTime);
            //RecognitionConsumptionTime = Convert.ToString(Convert.ToInt32(consumeTime.TotalMilliseconds)) + " ms"; ;
            //RecognitionResultDisplay = Visibility.Visible;
            //GrayDisplay = Visibility.Visible;
        }
        /// <summary>
        /// 模板检查
        /// </summary>
        private void TemplateExamine()
        {
            RecognitionResultDisplay = Visibility.Collapsed;
            GateDetectionVisibility = Visibility.Collapsed;
            GrayDisplay = Visibility.Collapsed;
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
                
                TemplateDatas.Clear();
                TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>(newTemplateDatas);
                //修改数据库
                var sDB = new SqlAccess().SystemDataAccess;
                sDB.DbMaintenance.TruncateTable<sys_bd_Templatedatamodel>();
                sDB.Insertable(newTemplateDatas).ExecuteCommand();
                sDB.Close(); sDB.Dispose();
                EventMessage.SystemMessageDisplay("模板检查执行成功！", MessageType.Success);
                EventMessage.MessageDisplay("模板检查执行成功！", true, true);
            }
            catch (Exception ex)
            {
                EventMessage.SystemMessageDisplay($"模板检查执行失败：{ex.Message}", MessageType.Error);
            }
        }

        /// <summary>
        /// 打开修改模板参数界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new DialogParameters
            {
                { "para", DataGridSelectedItem }
            };
            _dialogService.ShowDialog("TemplateDataEdit", parameters, result =>
            {
                // 处理结果
                if (result.Parameters.Count != 0 && result.Parameters.ContainsKey("Result"))
                {

                    DataGridSelectedItem = result.Parameters.GetValue<sys_bd_Templatedatamodel>("Result");
                    UpdataTemplateData(DataGridSelectedItem);
                }
            });
        }


        #endregion


    }
}

