using MahApps.Metro.Controls.Dialogs;
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
    internal class UpdateAiFileDialogViewModel : BindableBase, IDialogAware, IDisposable
    {
        private string _filePath_Hdl;
        /// <summary>
        ///  
        /// </summary>
        public string FilePath_Hdl
        {
            get { return _filePath_Hdl; }
            set
            {
                SetProperty(ref _filePath_Hdl, value);
            }
        }
        private string _filePath_Hdict;
        /// <summary>
        ///  
        /// </summary>
        public string FilePath_Hdict
        {
            get { return _filePath_Hdict; }
            set
            {
                SetProperty(ref _filePath_Hdict, value);
            }
        }


        public string Title => "大模型文件管理";
        private bool _disposed = false;


        public event Action<IDialogResult> RequestClose;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand ChooseFileCommand { get; }

        private readonly IDialogCoordinator _dialogCoordinator;
        public UpdateAiFileDialogViewModel(IDialogCoordinator dialogCoordinator)
        {
            this._dialogCoordinator = dialogCoordinator;
            ConfirmCommand = new DelegateCommand(OnConfirm);
            ChooseFileCommand = new DelegateCommand(ChooseFile);
        }

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

            if (FilePath_Hdict!=null && FilePath_Hdl!=null)
            {
                //parameters.Add("IsUpdate", true);
                parameters.Add("FilePath_Hdict", FilePath_Hdict);
                parameters.Add("FilePath_Hdl", FilePath_Hdl);
            }
                
                     
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        private async void ChooseFile()
        {
            bool isHave_hdl = false;
            bool isHave_hdict = false;
            FilePath_Hdl = null;
            FilePath_Hdict = null;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Title = "请选择要更新的大模型文件",
                    // InitialDirectory = initialDirectory,
                    Filter = "文件(*.hdl, *.hdict)|*.hdl;*.hdict"
                };

                // 如果有指定所有者窗口，设置Owner
                if (openFileDialog.ShowDialog() == true)
                {
                    List<string> strings = openFileDialog.FileNames.ToList();
                    if (strings.Count == 2)
                    {
                        for (int i = strings.Count - 1; i >= 0; i--)
                        {
                            FileInfo file = new FileInfo(strings[i]);
                            if (file.Name.EndsWith("hdl"))
                            {
                                FilePath_Hdl = strings[i];
                                isHave_hdl = true;
                            }
                            if (file.Name.EndsWith("hdict"))
                            {
                                FilePath_Hdict = strings[i];
                                isHave_hdict = true;
                            }

                        }
                        if (!isHave_hdict || !isHave_hdl)
                        {
                            FilePath_Hdl = null;
                            FilePath_Hdict = null;
                            //文件格式不对
                            this._dialogCoordinator.ShowMessageAsync(this, "错误提示", $"文件格式错误!文件格式应为hdl（1个），hdict（1个），请重新选择！");

                        }else
                        {
                            //显示
                        }
                    }
                    else
                    {
                        //文件数不对
                        this._dialogCoordinator.ShowMessageAsync(this, "错误提示", $"文件数量为：{strings.Count}!文件数应该为2个，请重新选择！");
                    }
                }


            });
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
