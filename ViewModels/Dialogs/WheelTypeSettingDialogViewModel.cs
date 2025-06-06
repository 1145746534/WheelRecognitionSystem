﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;
using WheelRecognitionSystem.Views.Dialogs;

namespace WheelRecognitionSystem.ViewModels.Dialogs
{
    public class WheelTypeSettingDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "轮型设置";

        /// <summary>
        /// 关闭弹窗
        /// </summary> 
        public event Action<IDialogResult> RequestClose;



        private ObservableCollection<sys_bd_Templatedatamodel> _templateDatas;

        public ObservableCollection<sys_bd_Templatedatamodel> TemplateDatas
        {
            get { return _templateDatas; }
            set { SetProperty(ref _templateDatas, value); }
        }

        private string _id;
        /// <summary>
        /// 轮型ID
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _wheelType;
        /// <summary>
        /// 轮毂型号
        /// </summary>
        public string WheelType
        {
            get { return _wheelType; }
            set
            {
                // 过滤非法字符并转换为大写
                var filteredValue = new string(value?
                    .Where(c => char.IsUpper(c) || char.IsLower(c) || char.IsDigit(c) || c == '_')
                    .ToArray());
                filteredValue = filteredValue.ToUpper();
                if (SetProperty(ref _wheelType, filteredValue))
                {
                    // 属性更改逻辑
                }
            }
        }

        private string _wheelHeight;
        /// <summary>
        /// 轮毂高度
        /// </summary>
        public string WheelHeight
        {
            get { return _wheelHeight; }
            set { SetProperty(ref _wheelHeight, value); }
        }

        private string _wheelStyle;
        /// <summary>
        /// 轮毂样式
        /// </summary>
        public string WheelStyle
        {
            get { return _wheelStyle; }
            set { SetProperty(ref _wheelStyle, value); }
        }

        /// <summary>
        /// 确认按钮命令
        /// </summary>
        public DelegateCommand OkCommand { get; set; }
        /// <summary>
        /// 取消按钮命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        public WheelTypeSettingDialogViewModel()
        {
            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);

        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult());
        }
        /// <summary>
        /// 确认按钮
        /// </summary>
        private void Ok()
        {
            if (WheelType == null)
            {
                RequestClose?.Invoke(new DialogResult());
                return;
            }
            if (WheelType.Length < 8)
            {
                EventMessage.SystemMessageDisplay("轮型长度小于8个字符，请重新输入！", MessageType.Error);
                return;
            }
            //只允许数字、大写字母和下划线
            Regex regex = new Regex("[^0-9A-Z_]");
            if (regex.IsMatch(WheelType))
            {
                EventMessage.SystemMessageDisplay("轮型输入错误，请重新输入！", MessageType.Error);
                return;
            }
            if (WheelHeight == null)
            {
                EventMessage.SystemMessageDisplay("轮型高度未输入，请重新输入！", MessageType.Error);
                return;
            }
            if (!float.TryParse(WheelHeight, out float height))
            {
                EventMessage.SystemMessageDisplay("轮型高度错误，请重新输入！", MessageType.Error);
                return;
            }
            if (WheelStyle == null)
            {
                EventMessage.SystemMessageDisplay("轮型样式选择，请重新选择！", MessageType.Error);
                return;
            }

            var filteredData = TemplateDatas.Where(item => item.WheelType == WheelType).ToList();
            if (filteredData != null && filteredData.Count > 0)
            {
                EventMessage.SystemMessageDisplay("轮型重复，请重新输入！", MessageType.Error);
                return;
            }


            sys_bd_Templatedatamodel data = new sys_bd_Templatedatamodel
            {
                Index = int.Parse(Id),
                WheelType = WheelType,
                UnusedDays = 0,
                WheelHeight = float.Parse(WheelHeight),
                WheelStyle = WheelStyle,
                CreationTime = DateTime.Now.ToString("yy-MM-dd"),
                LastUsedTime = DateTime.Now
            };

            TemplateDatas.Add(data);
            //数据根据轮型还有轮毂样式排序
            var newDatas = TemplateDatas.OrderBy(x => x.WheelType).ThenBy(x => x.WheelStyle).ToList();
            //整理Index
            for (int i = 0; i < newDatas.Count; i++)
            {
                newDatas[i].Index = i + 1;
            }
            TemplateDatas.Clear();
            foreach (var item in newDatas)
            {
                TemplateDatas.Add(item);
            }
           
            //定义弹窗结果
            IDialogResult dialogResult = new DialogResult();
            //将新增数据添加到弹窗结果的Parameters
            dialogResult.Parameters.Add("set", "add_OK");
            //关闭弹窗并返回弹窗结果
            RequestClose?.Invoke(dialogResult);
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

        }
        /// <summary>
        /// 弹窗打开时
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("templateDatas"))
            {
                TemplateDatas = parameters.GetValue<ObservableCollection<sys_bd_Templatedatamodel>>("templateDatas");
                Id = (TemplateDatas.Count + 1).ToString();
            }
            //var sDB = new SqlAccess().SystemDataAccess;
            //var data = sDB.Queryable<sys_bd_Templatedatamodel>().Max(it => it.Index);
            //Id = (data + 1).ToString();
            //sDB.Close();
        }
    }
}
