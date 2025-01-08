﻿using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class ReportManagementViewModel : BindableBase
    {
        public string Title { get; set; } = "报表管理";

        #region==============日期时间相关属性================
        private DateTime _startDate;
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate;
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private string _startHour;
        /// <summary>
        /// 起始小时
        /// </summary>
        public string StartHour
        {
            get { return _startHour; }
            set { SetProperty(ref _startHour, value); }
        }

        private string _startMinute;
        /// <summary>
        /// 起始分钟
        /// </summary>
        public string StartMinute
        {
            get { return _startMinute; }
            set { SetProperty(ref _startMinute, value); }
        }

        private string _endHour;
        /// <summary>
        /// 结束小时
        /// </summary>
        public string EndHour
        {
            get { return _endHour; }
            set { SetProperty(ref _endHour, value); }
        }

        private string _endMinute;
        /// <summary>
        /// 结束分钟
        /// </summary>
        public string EndMinute
        {
            get { return _endMinute; }
            set { SetProperty(ref _endMinute, value); }
        }
        #endregion
        #region==============按钮命令相关属性================
        /// <summary>
        /// 数据刷新命令
        /// </summary>
        public DelegateCommand DataRefreshCommand { get; set; }
        /// <summary>
        /// 数据查询命令
        /// </summary>
        public DelegateCommand DataInquireCommand { get; set; }
        /// <summary>
        /// 数据统计命令
        /// </summary>
        public DelegateCommand DataStatisticsCommand { get; set; }
        /// <summary>
        /// 数据导出命令
        /// </summary>
        public DelegateCommand DataExportCommand { get; set; }
        #endregion

        private ObservableCollection<ProductionDataModel> _identificationDatas;
        /// <summary>
        /// 识别数据
        /// </summary>
        public ObservableCollection<ProductionDataModel> IdentificationDatas
        {
            get { return _identificationDatas; }
            set {SetProperty(ref _identificationDatas, value); }
        }

        private ObservableCollection<StatisticsDataModel> _statisticsDatas;
        /// <summary>
        /// 统计数据
        /// </summary>
        public ObservableCollection<StatisticsDataModel> StatisticsDatas
        {
            get { return _statisticsDatas; }
            set { SetProperty(ref _statisticsDatas, value); }
        }

        private Visibility _identificationDataVisibility;
        /// <summary>
        /// 识别数据表格显示控制
        /// </summary>
        public Visibility IdentificationDataVisibility
        {
            get { return _identificationDataVisibility; }
            set {SetProperty(ref _identificationDataVisibility, value); }
        }

        private Visibility _statisticsDataVisibility;
        /// <summary>
        /// 统计数据表格显示控制
        /// </summary>
        public Visibility StatisticsDataVisibility
        {
            get { return _statisticsDataVisibility; }
            set { SetProperty(ref _statisticsDataVisibility, value); }
        }


        public ReportManagementViewModel()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            StartHour = DateTime.Now.Hour.ToString();
            StartMinute = "00";
            EndHour = DateTime.Now.Hour.ToString();
            EndMinute = "00";

            DataRefreshCommand = new DelegateCommand(DataRefresh);
            DataInquireCommand = new DelegateCommand(DataInquire);
            DataStatisticsCommand = new DelegateCommand(DataStatistics);
            DataExportCommand = new DelegateCommand(DataExport);

            IdentificationDatas = new ObservableCollection<ProductionDataModel>();
            StatisticsDatas = new ObservableCollection<StatisticsDataModel>();
        }
        private void DataRefresh()
        {
            var pDB = new SqlAccess().ProductionDataAccess;
            int maxId = pDB.Queryable<ProductionDataModel>().Max(x => x.Index);
            StatisticsDataVisibility = Visibility.Collapsed;
            List<ProductionDataModel> productionList = new List<ProductionDataModel>();
            if (maxId > 100)
            {
                //查询两个序号区间的数据
                productionList = pDB.Queryable<ProductionDataModel>().Where(it => SqlFunc.Between(it.Index, maxId - 100, maxId)).OrderBy((sc) => sc.Index, OrderByType.Desc).ToList();
            }
            else
            {
                productionList = pDB.Queryable<ProductionDataModel>().Where(it => SqlFunc.Between(it.Index, 0, maxId)).OrderBy((sc) => sc.Index, OrderByType.Desc).ToList();
            }
            IdentificationDatas?.Clear();
            IdentificationDatas = new ObservableCollection<ProductionDataModel>(productionList);
            IdentificationDataVisibility = Visibility.Visible;
        }
        private void DataInquire()
        {
            bool r = JudgmentInputsDateTime(StartDate, StartHour, StartMinute, EndDate, EndHour, EndMinute, out GetDateTimeModel result);
            if (r)
            {
                StatisticsDataVisibility = Visibility.Collapsed;
                var pDB = new SqlAccess().ProductionDataAccess;
                var productionList = pDB.Queryable<ProductionDataModel>().
                    Where(it => SqlFunc.Between(it.RecognitionTime, result.StartDateTime, result.EndDateTime)).
                    OrderBy((sc) => sc.Index, OrderByType.Desc).ToList();
                IdentificationDatas?.Clear();
                IdentificationDatas = new ObservableCollection<ProductionDataModel>(productionList);
                IdentificationDataVisibility = Visibility.Visible;
            }
            else EventMessage.SystemMessageDisplay(result.Result, MessageType.Warning);
        }
        private void DataStatistics()
        {
            bool r = JudgmentInputsDateTime(StartDate, StartHour, StartMinute, EndDate, EndHour, EndMinute, out GetDateTimeModel result);
            if (r)
            {
                IdentificationDataVisibility = Visibility.Hidden;
                var pDB = new SqlAccess().ProductionDataAccess;
                var productionList = pDB.Queryable<ProductionDataModel>().Where(it => SqlFunc.Between(it.RecognitionTime, result.StartDateTime, result.EndDateTime)).ToList();
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
                statisticsDatas.Sort((p1, p2) =>//排序
                {
                    if (p1.WheelType != p2.WheelType)
                    {
                        return p1.WheelType.CompareTo(p2.WheelType);
                    }
                    else return 0;
                });
                //调整序号
                for (int i = 0; i < statisticsDatas.Count; i++)//整理序号
                {
                    statisticsDatas[i].Index = i + 1;
                }
                //计算总数
                StatisticsDataModel statisticsData = new StatisticsDataModel();
                statisticsData.Index = statisticsDatas.Count + 1;
                statisticsData.WheelType = "总计";
                for (int i = 0; i < statisticsDatas.Count; i++)
                {
                    statisticsData.WheelCount = statisticsData.WheelCount + statisticsDatas[i].WheelCount;
                }
                statisticsDatas.Add(statisticsData);
                IdentificationDatas?.Clear();
                StatisticsDatas?.Clear();
                StatisticsDatas = new ObservableCollection<StatisticsDataModel>(statisticsDatas);
                StatisticsDataVisibility = Visibility.Visible;
            }
            else EventMessage.SystemMessageDisplay(result.Result, MessageType.Warning);
        }
        private void DataExport()
        {
            if (IdentificationDatas.Count == 0 && StatisticsDatas.Count == 0)
            {
                EventMessage.SystemMessageDisplay("无导出的数据，请检查！", MessageType.Warning);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "请选择要导出的位置",
                Filter = "Excel文件(*.xls,*.xlsx)|*.xls;*.xlsx"
            };
            if (saveFileDialog.ShowDialog() != true) return;
            if (saveFileDialog.FileName != "")
            {
                var FileSavePath = saveFileDialog.FileName.ToString();
                DataTable datas = new DataTable();
                if (IdentificationDatas.Count > 0) datas = ExcelDataAccess.ListToDataTable(IdentificationDatas);
                else if (StatisticsDatas.Count > 0) datas = ExcelDataAccess.ListToDataTable(StatisticsDatas);
                var result = ExcelDataAccess.DataTableToExcel(datas, FileSavePath, out string exportResult);
                if (result) EventMessage.SystemMessageDisplay(exportResult, MessageType.Success);
                else EventMessage.SystemMessageDisplay(exportResult, MessageType.Error);
            }
        }

        /// <summary>
        /// 检查输入的日期时间
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="startHour">起始小时</param>
        /// <param name="startMinute">起始分钟</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="endHour">结束小时</param>
        /// <param name="endMinute">结束分钟</param>
        /// <param name="result">检查结果</param>
        /// <returns>True为无错误</returns>
        private bool JudgmentInputsDateTime(DateTime startDate, string startHour, string startMinute, DateTime endDate, string endHour, string endMinute, out GetDateTimeModel result)
        {
            GetDateTimeModel dateTime = new GetDateTimeModel();
            if (!int.TryParse(startHour, out int sh) || sh < 0 || sh > 24)
            {
                dateTime.Result = "起始小时输入错误，请检查！";
                result = dateTime;
                return false;
            }
            if (!int.TryParse(startMinute, out int sm) || sm < 0 || sm > 59)
            {
                dateTime.Result = "起始分钟输入错误，请检查！";
                result = dateTime;
                return false;
            }
            if (!int.TryParse(endHour, out int eh) || eh < 0 || eh > 24)
            {
                dateTime.Result = "结束小时输入错误，请检查！";
                result = dateTime;
                return false;
            }
            if (!int.TryParse(endMinute, out int em) || em < 0 || em > 59)
            {
                dateTime.Result = "结束分钟输入错误，请检查！";
                result = dateTime;
                return false;
            }
            GenDateTime(startDate, startHour, startMinute, out DateTime startDateTime);
            GenDateTime(endDate, endHour, endMinute, out DateTime endDateTime);
            if (startDateTime >= endDateTime)
            {
                dateTime.Result = "起始日期大于或等于结束日期，请检查！";
                result = dateTime;
                return false;
            }
            dateTime.StartDateTime = startDateTime;
            dateTime.EndDateTime = endDateTime;
            dateTime.Result = "OK";
            result = dateTime;
            return true;
        }

        /// <summary>
        /// 生成日期时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="dateTime">生成的日期时间</param>
        /// <returns>True为生成成功</returns>
        private bool GenDateTime(DateTime date, string hour, string minute, out DateTime dateTime)
        {
            string strDate = date.ToString().Replace("0:00:00", "");
            string strDateTime = strDate + hour + ":" + minute + ":00";
            return DateTime.TryParse(strDateTime, out dateTime);
        }
    }
}
