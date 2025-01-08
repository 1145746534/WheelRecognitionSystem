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
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.ViewModels.Dialogs
{
    public class ParameterSettingDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "参数设置";

        public event Action<IDialogResult> RequestClose;

        private int _wheelMinThresholdValue;
        /// <summary>
        /// 定位轮毂时的最小阈值
        /// </summary>
        public int WheelMinThresholdValue
        {
            get { return _wheelMinThresholdValue; }
            set{SetProperty(ref _wheelMinThresholdValue, value);}
        }

        private int _windowMaxThreshold;
        /// <summary>
        /// 制作模板时模板剔除部分最大阈值
        /// </summary>
        public int WindowMaxThreshold
        {
            get { return _windowMaxThreshold; }
            set { SetProperty(ref _windowMaxThreshold, value);}
        }

        private double _removeMixAreaValue;
        /// <summary>
        /// 制作模板时模板剔除部分的最小面积
        /// </summary>
        public double RemoveMixAreaValue
        {
            get { return _removeMixAreaValue; }
            set { SetProperty(ref _removeMixAreaValue, value);}
        }


        /// <summary>
        /// 确认按钮命令
        /// </summary>
        public DelegateCommand OkCommand { get; set; }
        /// <summary>
        /// 取消按钮命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        public ParameterSettingDialogViewModel()
        {
            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult());
        }

        private void Ok()
        {
            bool changed = false;
            if (WheelMinThresholdValue < 0 || WheelMinThresholdValue > 100)
            {
                EventMessage.SystemMessageDisplay("定位轮毂时的最小阈值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.WheelMinThreshold != WheelMinThresholdValue)
                {
                    SqlAccess.SystemDatasWrite("WheelMinThreshold", WheelMinThresholdValue.ToString());
                    SystemDatas.WheelMinThreshold = WheelMinThresholdValue;
                    changed = true;
                }
            }
            if (WindowMaxThreshold < 0 || WindowMaxThreshold > 200)
            {
                EventMessage.SystemMessageDisplay("轮毂窗口最大阈值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.WindowMaxThreshold != WindowMaxThreshold)
                {
                    SystemDatas.WindowMaxThreshold = WindowMaxThreshold;
                    SqlAccess.SystemDatasWrite("WindowMaxThreshold", WindowMaxThreshold.ToString());
                    changed = true;
                }
            }
            if (RemoveMixAreaValue < 10 || RemoveMixAreaValue > 500)
            {
                EventMessage.SystemMessageDisplay("轮毂窗口最小面积输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.RemoveMixArea != RemoveMixAreaValue)
                {
                    SqlAccess.SystemDatasWrite("RemoveMixArea", RemoveMixAreaValue.ToString());
                    SystemDatas.RemoveMixArea = RemoveMixAreaValue;
                    changed = true;
                }

            }
            if(changed)
            {
                EventMessage.MessageHelper.GetEvent<ParameterSettingChangedEvent>().Publish("");
                EventMessage.SystemMessageDisplay("修改成功！", Models.MessageType.Success);
            }
            RequestClose?.Invoke(new DialogResult());
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            WheelMinThresholdValue = SystemDatas.WheelMinThreshold;
            WindowMaxThreshold = SystemDatas.WindowMaxThreshold;
            RemoveMixAreaValue = SystemDatas.RemoveMixArea;
        }
    }
}
