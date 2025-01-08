using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;

namespace WheelRecognitionSystem.ViewModels.Dialogs
{
    public class CameraSetDialogViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        /// 对话框标题
        /// </summary>
        public string Title => "相机设置";
        /// <summary>
        /// 调用这个事件，可关闭当前对话框并传递一个操作结果出去
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        private string _cameraName;
        /// <summary>
        ///  相机名称
        /// </summary>
        public string CameraName
        {
            get { return _cameraName; }
            set { SetProperty(ref _cameraName, value); }
        }

        private string _cameraLinkID;
        /// <summary>
        ///  相机名称
        /// </summary>
        public string CameraLinkID
        {
            get { return _cameraLinkID; }
            set { SetProperty(ref _cameraLinkID, value); }
        }

        private string _cameraExposure;
        /// <summary>
        ///  相机名称
        /// </summary>
        public string CameraExposure
        {
            get { return _cameraExposure; }
            set { SetProperty(ref _cameraExposure, value); }
        }

        /// <summary>
        /// 确认按钮命令
        /// </summary>
        public DelegateCommand OkCommand { get; set; }
        /// <summary>
        /// 取消按钮命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }
        public CameraSetDialogViewModel()
        {
            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);
        }
        private void Ok()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("LinkID", CameraLinkID);
            parameters.Add("Exposure", CameraExposure);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
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
            if (parameters.ContainsKey("Name"))
            {
                //保存一下
                CameraName = parameters.GetValue<string>("Name");
            }
            if (parameters.ContainsKey("LinkID"))
            {
                //保存一下
                CameraLinkID = parameters.GetValue<string>("LinkID");
            }
            if (parameters.ContainsKey("Exposure"))
            {
                //保存一下
                CameraExposure = parameters.GetValue<string>("Exposure");
            }




        }
    }
}
