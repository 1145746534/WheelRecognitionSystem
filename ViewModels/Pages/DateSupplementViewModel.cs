﻿using NPOI.SS.Formula.Functions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<ProductionDataModel> _templateDatas;
        /// <summary>
        /// 未识别数据
        /// </summary>
        public ObservableCollection<ProductionDataModel> TemplateDatas
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

        private TemplateDataModel _temDataGridSelectedItem;
        /// <summary>
        /// 模板数据窗口选中的行
        /// </summary>
        public TemplateDataModel TemDataGridSelectedItem
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

        #endregion




        private IRegionManager _regionManager;

        public DateSupplementViewModel(IRegionManager regionManager)
        {
            UnrMouseLeftButtonDownCommand = new DelegateCommand<object>(UnrMouseLeftButtonDown);
            _regionManager = regionManager;
            UnrecognizedDatas = new ObservableCollection<ProductionDataModel>();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);//添加事件(到达时间间隔后会自动调用)
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5000);//设置时间间隔为5000ms
            _dispatcherTimer.Start();//启动定时器
            //DataInquire();

        }

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
                //var sDB = new SqlAccess().SystemDataAccess;
                //sDB.Updateable(DataGridSelectedItem).ExecuteCommand();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            IRegion region = _regionManager.Regions["ViewRegion"];

            if (region.ActiveViews.Count() > 0)
            {
                string activeViewName = region.ActiveViews.First().ToString();
                if (activeViewName.Contains("DateSupplementView"))
                {
                    DataInquireProduct();
                }

                // activeView 就是当前焦点页面（视图）
            }
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        private void DataInquireProduct()
        {
            SqlSugarClient pDB = new SqlAccess().ProductionDataAccess;
            var exp = Expressionable.Create<ProductionDataModel>()
                .And(it => it.Reserve1 == "").Or(it => it.Reserve1 == null).ToExpression();
            List<ProductionDataModel> productionList = pDB.Queryable<ProductionDataModel>().Where(exp).ToList();
            //List<ProductionDataModel> productionList = pDB.Queryable<ProductionDataModel>()
            //    .Where(it => SqlFunc.EqualsNull(it.Reserve1, "")).OrderBy((sc) => sc.Index).ToList();
            if (productionList.Count > UnrecognizedDatas.Count)
            {
                UnrecognizedDatas?.Clear();
                UnrecognizedDatas = new ObservableCollection<ProductionDataModel>(productionList);
            }
        }

        public void DataInquireTemplate()
        {
            List<TemplateDataModel> datas = new SqlAccess().SystemDataAccess.Queryable<TemplateDataModel>().ToList();

        }
    }
}
