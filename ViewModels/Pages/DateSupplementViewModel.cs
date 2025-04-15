using HalconDotNet;
using NPOI.SS.Formula.Functions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    /// <summary>
    /// 数据补充
    /// </summary>
    public class DateSupplementViewModel : BindableBase
    {
        public string Title { get; set; } = "数据补录";

        #region  未识别数据属性
        private ObservableCollection<ProductionDataModel> _unrecognizedDatas;
        /// <summary>
        /// 未识别数据
        /// </summary>
        public ObservableCollection<ProductionDataModel> UnrecognizedDatas
        {
            get { return _unrecognizedDatas; }
            set { SetProperty(ref _unrecognizedDatas, value); }
        }

        private int _unrDataGridSelectedIndex;
        /// <summary>
        /// 模板数据窗口选中的行索引
        /// </summary>
        public int UnrDataGridSelectedIndex
        {
            get { return _unrDataGridSelectedIndex; }
            set { SetProperty(ref _unrDataGridSelectedIndex, value); }
        }

        private ProductionDataModel _unrDataGridSelectedItem;
        /// <summary>
        /// 模板数据窗口选中的行
        /// </summary>
        public ProductionDataModel UnrDataGridSelectedItem
        {
            get { return _unrDataGridSelectedItem; }
            set
            {
                SetProperty(ref _unrDataGridSelectedItem, value);
            }
        }

        public ICommand UnrMouseLeftButtonDownCommand { get; set; }

        public DispatcherTimer _dispatcherTimer;

        #endregion

        #region  模板数据属性
        private ObservableCollection<sys_bd_Templatedatamodel> _templateDatas;
        /// <summary>
        /// 未识别数据
        /// </summary>
        public ObservableCollection<sys_bd_Templatedatamodel> TemplateDatas
        {
            get { return _templateDatas; }
            set { SetProperty(ref _templateDatas, value); }
        }

        private int _temDataGridSelectedIndex;
        /// <summary>
        /// 模板数据窗口选中的行索引
        /// </summary>
        public int TemDataGridSelectedIndex
        {
            get { return _temDataGridSelectedIndex; }
            set { SetProperty(ref _temDataGridSelectedIndex, value); }
        }

        private sys_bd_Templatedatamodel _temDataGridSelectedItem;
        /// <summary>
        /// 模板数据窗口选中的行
        /// </summary>
        public sys_bd_Templatedatamodel TemDataGridSelectedItem
        {
            get { return _temDataGridSelectedItem; }
            set
            {
                SetProperty(ref _temDataGridSelectedItem, value);
            }
        }

        public ICommand TemMouseLeftButtonDownCommand { get; set; }

        #endregion

        #region  待补录数据属性

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

        public string RecWheelType
        {
            get { return _recWheelType; }
            set { SetProperty(ref _recWheelType, value); }
        }
        private string _recWheelStyle;

        public string RecWheelStyle
        {
            get { return _recWheelStyle; }
            set { SetProperty(ref _recWheelStyle, value); }
        }

        public ICommand HubChangesCommand { get; set; }


        #endregion

        #region  图像显示

        private HObject _currentImage;
        /// <summary>
        /// 当前图像
        /// </summary>
        public HObject CurrentImage
        {
            get { return _currentImage; }
            set { SetProperty<HObject>(ref _currentImage, value); }
        }

        #endregion


        private IRegionManager _regionManager;

        public DateSupplementViewModel(IRegionManager regionManager)
        {
            UnrMouseLeftButtonDownCommand = new DelegateCommand<object>(UnrMouseLeftButtonDown);
            TemMouseLeftButtonDownCommand = new DelegateCommand<object>(TemMouseLeftButtonDown);
            HubChangesCommand = new DelegateCommand(HubChanges);
            _regionManager = regionManager;
            UnrecognizedDatas = new ObservableCollection<ProductionDataModel>();
            TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);//设置时间间隔为2000ms
            _dispatcherTimer.Start();//启动定时器
        }


        /// <summary>
        /// 单击未识别数据表格
        /// </summary>
        /// <param name="obj"></param>
        private void UnrMouseLeftButtonDown(object obj)
        {
            DataGrid dataGrid = (DataGrid)obj;
            if (dataGrid != null && dataGrid.Items.Count > 0 && dataGrid.CurrentItem != null)
            {
                //获取选中的行索引
                int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
                //获取选中的列索引
                int columnIndex = dataGrid.CurrentCell.Column.DisplayIndex;

                UnrDataGridSelectedItem = UnrecognizedDatas[UnrDataGridSelectedIndex];
                UnrIndex = UnrDataGridSelectedItem.Index.ToString();
                UnrWheelType = UnrDataGridSelectedItem.WheelType;
                string path = UnrDataGridSelectedItem.ImagePath;
                EventMessage.MessageHelper.GetEvent<ClearEvent>().Publish("");
                CurrentImage?.Dispose();
                if (File.Exists(path))
                {
                    HObject image = new HObject();
                    HOperatorSet.ReadImage(out image, path);

                    CurrentImage = image.Clone();
                    image.Dispose();
                }

                //var sDB = new SqlAccess().SystemDataAccess;
                //sDB.Updateable(DataGridSelectedItem).ExecuteCommand();
            }
        }

        private void TemMouseLeftButtonDown(object obj)
        {
            DataGrid dataGrid = (DataGrid)obj;
            if (dataGrid != null && dataGrid.Items.Count > 0 && dataGrid.CurrentItem != null)
            {
                //获取选中的行索引
                int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
                //获取选中的列索引
                int columnIndex = dataGrid.CurrentCell.Column.DisplayIndex;

                TemDataGridSelectedItem = TemplateDatas[TemDataGridSelectedIndex];
                RecWheelType = TemDataGridSelectedItem.WheelType;
                RecWheelStyle = TemDataGridSelectedItem.WheelStyle;
                //var sDB = new SqlAccess().SystemDataAccess;
                //sDB.Updateable(DataGridSelectedItem).ExecuteCommand();
            }
        }

        /// <summary>
        /// 确认修改
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void HubChanges()
        {
            SqlSugarClient pDB = new SqlAccess().ProductionDataAccess;
            var result = pDB.Updateable<ProductionDataModel>()
                .SetColumns(it => new ProductionDataModel()
                {
                    Model = RecWheelType,
                    WheelType = RecWheelType,
                    WheelStyle = RecWheelStyle
                }).Where(it => it.Index == Convert.ToInt32(UnrIndex)).ExecuteCommand();
            DataInquireProduct();
            RecWheelType = "";
            RecWheelStyle = "";
            UnrIndex = "";
            UnrWheelType = "";
            CurrentImage = null;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            IRegion region = _regionManager.Regions["ViewRegion"];

            if (region.ActiveViews.Count() > 0)
            {
                // activeView 就是当前焦点页面（视图）
                string activeViewName = region.ActiveViews.First().ToString();
                if (activeViewName.Contains("DateSupplementView"))
                {
                    DataInquireProduct();
                    DataInquireTemplate();
                }
            }
        }
        /// <summary>
        /// 数据查询未识别的产品
        /// </summary>
        private void DataInquireProduct()
        {
            SqlSugarClient pDB = new SqlAccess().ProductionDataAccess;
            var exp = Expressionable.Create<ProductionDataModel>()
                .And(it => it.Model == "").Or(it => it.Model == null).ToExpression();
            List<ProductionDataModel> productionList = pDB.Queryable<ProductionDataModel>().Where(exp).ToList();
            //List<ProductionDataModel> productionList = pDB.Queryable<ProductionDataModel>()
            //    .Where(it => SqlFunc.EqualsNull(it.Reserve1, "")).OrderBy((sc) => sc.Index).ToList();
            if (productionList.Count != UnrecognizedDatas.Count)
            {
                UnrecognizedDatas?.Clear();
                UnrecognizedDatas = new ObservableCollection<ProductionDataModel>(productionList);
            }
        }

        /// <summary>
        /// 数据查询模板数据
        /// </summary>
        public void DataInquireTemplate()
        {
            List<sys_bd_Templatedatamodel> datas = new SqlAccess().SystemDataAccess.Queryable<sys_bd_Templatedatamodel>().ToList();
            datas.ForEach(data => { data.WheelType = data.WheelType.Trim('_'); });
            var datasGrb = datas.GroupBy(g => new { g.WheelType, g.WheelStyle }).ToList();



            if (datasGrb.Count != TemplateDatas.Count)
            {
                List<sys_bd_Templatedatamodel> newDatas = new List<sys_bd_Templatedatamodel>();
                int i = 0;
                foreach (var item in datasGrb)
                {
                    var key = item.Key; // 获取分组键（匿名类型）
                    newDatas.Add(new sys_bd_Templatedatamodel()
                    {
                        Index = i++,
                        WheelType = key.WheelType,
                        WheelStyle = key.WheelStyle
                    });
                }
                TemplateDatas?.Clear();
                TemplateDatas = new ObservableCollection<sys_bd_Templatedatamodel>(newDatas);
            }


        }
    }
}
