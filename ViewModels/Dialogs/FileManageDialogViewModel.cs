using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WheelRecognitionSystem.ViewModels.Dialogs
{
    public class FileManageDialogViewModel : BindableBase, IDialogAware, IDisposable
    {
        public string Title => "文件管理";

        private bool _disposed = false;

        private string _saveImageDays;
        /// <summary>
        ///  保存天数
        /// </summary>
        public string SaveImageDays
        {
            get { return _saveImageDays; }
            set
            {
                var filteredValue = new string(value.Where(c => char.IsDigit(c)).ToArray());
                SetProperty(ref _saveImageDays, filteredValue);
            }
        }

        private string _openTemplateFilePath;
        /// <summary>
        ///  软件路径
        /// </summary>
        public string OpenTemplateFilePath
        {
            get { return _openTemplateFilePath; }
            set
            {
                SetProperty(ref _openTemplateFilePath, value);
            }
        }
        private string _sQLManageSoftwarePath;
        /// <summary>
        ///  报表管理软件路径
        /// </summary>
        public string SQLManageSoftwarePath
        {
            get { return _sQLManageSoftwarePath; }
            set
            {
                SetProperty(ref _sQLManageSoftwarePath, value);
            }
        }
        private string _maintainQuantity;
        /// <summary>
        ///  常驻数量
        /// </summary>
        public string MaintainQuantity
        {
            get { return _maintainQuantity; }
            set
            {
                var filteredValue = new string(value.Where(c => char.IsDigit(c)).ToArray());
                SetProperty(ref _maintainQuantity, filteredValue);
            }
        }



        public event Action<IDialogResult> RequestClose;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<string> UpdatePathCommand { get; }




        public FileManageDialogViewModel()
        {
            ConfirmCommand = new DelegateCommand(OnConfirm);
            CancelCommand = new DelegateCommand(OnCancel);
            UpdatePathCommand = new DelegateCommand<string>(UpdatePath);
        }



        private async void UpdatePath(string obj)
        {
            string directory = @"E:\";
            if (!Directory.Exists(directory))
            {
                directory = @"D:\";
            }
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "请选择路径",
                    InitialDirectory = directory,
                    Filter = "文件(*.exe, *.EXE)|*.exe;*.EXE"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string value = openFileDialog.FileName;
                    if (obj == "模板制作")
                    {
                        OpenTemplateFilePath = value;
                    }
                    if (obj == "报表管理")
                    {
                        SQLManageSoftwarePath = value;
                    }


                }


            });
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("saveImageDays"))
            {
                SaveImageDays = parameters.GetValue<string>("saveImageDays");
            }
            if (parameters.ContainsKey("maintainQuantity"))
            {
                MaintainQuantity = parameters.GetValue<string>("maintainQuantity");
            }
            if (parameters.ContainsKey("TemplateSoftwarePath"))
            {
                OpenTemplateFilePath = parameters.GetValue<string>("TemplateSoftwarePath");
            }
            if (parameters.ContainsKey("SQLManageSoftwarePath"))
            {
                SQLManageSoftwarePath = parameters.GetValue<string>("SQLManageSoftwarePath");
            }

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
            parameters.Add("OpenTemplateFilePath", OpenTemplateFilePath);
            parameters.Add("SQLManageSoftwarePath", SQLManageSoftwarePath);
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


    }
}
