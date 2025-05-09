using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.ViewModels.Dialogs
{
    public class MatchResultDialogViewModel : BindableBase, IDialogAware
    {
        private ObservableCollection<MatchResultModel> _matchResultDatas;

        public ObservableCollection<MatchResultModel> MatchResultDatas
        {
            get { return _matchResultDatas; }
            set { _matchResultDatas = value; }
        }

        public MatchResultDialogViewModel()
        {
            MatchResultDatas = new ObservableCollection<MatchResultModel>();
            EventMessage.MessageHelper.GetEvent<MatchResultDatasDisplayEvent>().Subscribe(MatchResultDatasDisplay, ThreadOption.UIThread);
            //EventMessage.MessageHelper.GetEvent<MatchResultDatasDisplayEvent>().Subscribe(MatchResultDatasDisplay);
        }

        private void MatchResultDatasDisplay(List<MatchResultModel> list)
        {
            MatchResultDatas.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                MatchResultDatas.Add(list[i]);  
            }
        }

        public string Title => "匹配结果";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult());
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
