using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
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

        private string _id;
        /// <summary>
        /// 轮型ID
        /// </summary>
        public string Id
        {
            get { return _id; }
            set {SetProperty(ref _id, value); }
        }

        private string _wheelType;
        /// <summary>
        /// 轮毂型号
        /// </summary>
        public string WheelType
        {
            get { return _wheelType; }
            set { SetProperty(ref _wheelType, value); }
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
            if(WheelType == null)
            {
                RequestClose?.Invoke(new DialogResult());
                return;
            }
            if (WheelType.Length < 8)
            {
                EventMessage.SystemMessageDisplay("轮型长度小于8个字符，请重新输入！", MessageType.Error);
                return;
            }
            //只允许数字、小写字母和下划线
            Regex regex = new Regex("[^0-9a-z_]");
            if(regex.IsMatch(WheelType))
            {
                EventMessage.SystemMessageDisplay("轮型输入错误，请重新输入！", MessageType.Error);
                return;
            }
            var sDB = new SqlAccess().SystemDataAccess;
            var datas = sDB.Queryable<TemplateDataModel>().ToList();
            var result = datas.FindIndex(x => x.WheelType == WheelType.Trim(' '));
            if (result >= 0)
            {
                EventMessage.SystemMessageDisplay("轮型重复，请重新输入！", MessageType.Error);
                return;
            }
            TemplateDataModel data = new TemplateDataModel
            {
                Index = int.Parse(Id),
                WheelType = WheelType,
                UnusedDays = 0,
                SortingEnable = false,
                CreationTime = DateTime.Now.ToString("yy-MM-dd")
            };

            datas.Add(data);
            //数据根据轮型排序
            var newDatas = datas.OrderBy(x => x.WheelType).ToList();
            //整理Index
            for (int i = 0; i < newDatas.Count; i++)
            {
                newDatas[i].Index = i + 1;
            }
            //修改数据库
            sDB.DbMaintenance.TruncateTable<TemplateDataModel>();
            sDB.Insertable(newDatas).ExecuteCommand();
            //获取排序后的新增数据
            TemplateDataModel d = newDatas.Find(x => x.WheelType == WheelType);
            //定义弹窗结果
            IDialogResult dialogResult = new DialogResult();
            //将新增数据添加到弹窗结果的Parameters
            dialogResult.Parameters.Add("set", d);
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
            var sDB = new SqlAccess().SystemDataAccess;
            var data = sDB.Queryable<TemplateDataModel>().Max(it => it.Index);
            Id = (data + 1).ToString();
        }
    }
}
