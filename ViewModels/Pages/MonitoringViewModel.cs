using HalconDotNet;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using static WheelRecognitionSystem.Public.ExternalConnections;
using static WheelRecognitionSystem.Public.SystemDatas;


namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class MonitoringViewModel : BindableBase
    {
        public string Title { get; set; } = "识别监控";

        private HObject _currentImage;
        /// <summary>
        /// 当前图像
        /// </summary>
        public HObject CurrentImage
        {
            get { return _currentImage; }
            set { SetProperty<HObject>(ref _currentImage, value); }
        }

        private HObject _wheelContour;
        /// <summary>
        ///轮毂轮廓
        /// </summary>
        public HObject WheelContour
        {
            get { return _wheelContour; }
            set { SetProperty<HObject>(ref _wheelContour, value); }
        }

        private HObject _templateContour;
        /// <summary>
        /// 模板轮廓
        /// </summary>
        public HObject TemplateContour
        {
            get { return _templateContour; }
            set { SetProperty<HObject>(ref _templateContour, value); }
        }

        private HObject _gateContour;
        /// <summary>
        /// 浇口轮廓
        /// </summary>
        public HObject GateContour
        {
            get { return _gateContour; }
            set { SetProperty<HObject>(ref _gateContour, value); }
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

        private ObservableCollection<ScreenedDataModel> _screenedDatas;
        /// <summary>
        /// 分选数据
        /// </summary>
        public ObservableCollection<ScreenedDataModel> ScreenedDatas
        {
            get { return _screenedDatas; }
            set { SetProperty(ref _screenedDatas, value); }
        }

        private Visibility _screenedDataVisibility;
        /// <summary>
        /// 分选数据显示控制
        /// </summary>
        public Visibility ScreenedDataVisibility
        {
            get { return _screenedDataVisibility; }
            set {SetProperty(ref _screenedDataVisibility, value); }
        }

        private Visibility _wheelCountDataVisibility;
        /// <summary>
        /// 当天轮毂数据统计图表显示控制
        /// </summary>
        public Visibility WheelCountDataVisibility
        {
            get { return _wheelCountDataVisibility; }
            set { SetProperty(ref _wheelCountDataVisibility, value); }
        }

        private SeriesCollection _wheelCountDatas;
        /// <summary>
        /// 当天识别数据的轮毂统计数据
        /// </summary>
        public SeriesCollection WheelCountDatas
        {
            get { return _wheelCountDatas; }
            set {SetProperty(ref _wheelCountDatas, value); }
        }

        private List<string> _wheelTypes;
        /// <summary>
        /// 当天识别数据的轮型列表
        /// </summary>
        public List<string> WheelTypes
        {
            get { return _wheelTypes; }
            set {SetProperty(ref _wheelTypes, value); }
        }

        //private bool _screenedDataChecked;
        ///// <summary>
        ///// 选择分选数据
        ///// </summary>
        //public bool ScreenedDataChecked
        //{
        //    get { return _screenedDataChecked; }
        //    set 
        //    {
        //        if (value != _screenedDataChecked)
        //        {
        //            SetProperty(ref _screenedDataChecked, value);
        //            if (value)
        //            {
        //                ScreenedDataDisplayControl = true;
        //                WheelCountDataVisibility = Visibility.Collapsed;
        //                ScreenedDataVisibility = Visibility.Visible;
        //                SqlAccess.SystemDatasWrite("DataDisplayChoose", "分选数据");
        //            }
        //            else ScreenedDataDisplayControl = false;
        //        }
        //    }
        //}

        //private bool _currentDayDataChecked;
        ///// <summary>
        ///// 选择当日识别数据图表显示
        ///// </summary>
        //public bool CurrentDayDataChecked
        //{
        //    get { return _currentDayDataChecked; }
        //    set 
        //    {
        //        SetProperty(ref _currentDayDataChecked, value); 
        //        if(value)
        //        {
        //            GetCurrentDayDatas();
        //            ScreenedDataVisibility = Visibility.Collapsed;
        //            WheelCountDataVisibility = Visibility.Visible;
        //            SqlAccess.SystemDatasWrite("DataDisplayChoose", "当日数据");
        //        }
        //    }
        //}


        public MonitoringViewModel()
        {
            CurrentImage = new HObject();
            WheelContour = new HObject();
            TemplateContour = new HObject();
            GateContour = new HObject();
            ScreenedDatas = new ObservableCollection<ScreenedDataModel>();
            WheelCountDatas = new SeriesCollection();
            WheelTypes = new List<string>();
            EventMessage.MessageHelper.GetEvent<AutoRecognitionResultDisplayEvent>().Subscribe(RecognitionResultDisplay);
            EventMessage.MessageHelper.GetEvent<ScreenedDataDisplayEvent>().Subscribe(e =>
            {
                ScreenedDatas = new ObservableCollection<ScreenedDataModel>(e);
            });
            //EventMessage.MessageHelper.GetEvent<CurrentDayDataUpdataEvent>().Subscribe(e =>
            //{
            //    if (CurrentDayDataChecked) GetCurrentDayDatas();

            //});
            //var sDB = new SqlAccess().SystemDataAccess;
            //SystemSettingsDataModel data = sDB.Queryable<SystemSettingsDataModel>().First(x => x.Name == "DataDisplayChoose");
            //if(data != null && data.Value == "分选数据") ScreenedDataChecked = true;
            //else CurrentDayDataChecked = true;
        }

        /// <summary>
        /// 识别图像结果显示
        /// </summary>
        /// <param name="model"></param>
        private void RecognitionResultDisplay(AutoRecognitionResultDisplayModel model)
        {
            CurrentImage.Dispose();
            WheelContour.Dispose();
            TemplateContour.Dispose();
            GateContour.Dispose();
            if (model.CurrentImage != null) CurrentImage = model.CurrentImage;
            if (model.WheelContour != null) WheelContour = model.WheelContour;
            if (model.TemplateContour != null) TemplateContour = model.TemplateContour;
            //if (model.GateContour != null) GateContour = model.GateContour;
            //if (model.IsGate) GateContourColor = "green";
            else GateContourColor = "red";
            //图表数据更新
            //if(CurrentDayDataChecked)
            //{
            //    int index = WheelTypes.FindIndex(x => x == model.WheelType.Trim('_'));
            //    if (index >= 0)
            //    {
            //        WheelCountDatas[0].Values[index] = (int)WheelCountDatas[0].Values[index] + 1;
            //    }
            //    else GetCurrentDayDatas();
            //}
        }
        /// <summary>
        /// 获取当天识别数据
        /// </summary>
        private void GetCurrentDayDatas()
        {
            Task.Run(() =>
            {
                var pDB = new SqlAccess().SystemDataAccess;
                List<Tbl_productiondatamodel> productionList = new List<Tbl_productiondatamodel>();
                if (DateTime.Now.Hour < 8)
                {
                    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 8, 0, 0);
                    productionList = pDB.Queryable<Tbl_productiondatamodel>().Where(it => SqlFunc.Between(it.RecognitionTime, startTime, DateTime.Now)).ToList();
                }
                else
                {
                    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                    productionList = pDB.Queryable<Tbl_productiondatamodel>().Where(it => SqlFunc.Between(it.RecognitionTime, startTime, DateTime.Now.AddDays(1))).ToList();
                }
                if (productionList.Count > 0)
                {
                    List<StatisticsDataModel> statisticsDatas = new List<StatisticsDataModel>();
                    for (int i = 0; i < productionList.Count; i++)
                    {
                        int index = statisticsDatas.FindIndex(x => x.WheelType == productionList[i].WheelType.Trim('_'));
                        if (index < 0)
                        {
                            StatisticsDataModel statisticsDataModel = new StatisticsDataModel
                            {
                                Index = statisticsDatas.Count + 1,
                                WheelType = productionList[i].WheelType.Trim('_'),
                                WheelCount = 1
                            };
                            statisticsDatas.Add(statisticsDataModel);
                        }
                        else
                        {
                            statisticsDatas[index].WheelCount += 1;
                        }
                    }
                    //按轮型排序
                    statisticsDatas.Sort((p1, p2) =>
                    {
                        if (p1.WheelType != p2.WheelType)
                        {
                            return p1.WheelType.CompareTo(p2.WheelType);
                        }
                        else return 0;
                    });
                    WheelTypes.Clear();
                    WheelTypes = statisticsDatas.Select(x => x.WheelType).ToList();
                    List<int> counts = statisticsDatas.Select(x => x.WheelCount).ToList();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        WheelCountDatas.Clear();
                        WheelCountDatas.Add(new RowSeries
                        {
                            Values = new ChartValues<int>(counts),
                            Title = "数量",
                            Fill = Brushes.Aquamarine
                        });
                    }));
                }
                else
                {
                    WheelTypes.Clear();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        WheelCountDatas.Clear();
                        WheelCountDatas.Add(new RowSeries
                        {
                            Values = new ChartValues<int>(),
                            Title = "数量",
                            Fill = Brushes.Aquamarine
                        });
                    }));
                }
            });
        }
    }
}
