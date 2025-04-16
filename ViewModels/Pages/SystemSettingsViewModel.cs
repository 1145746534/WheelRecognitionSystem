using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WheelRecognitionSystem.DataAccess;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.ViewModels.Pages
{
    public class SystemSettingsViewModel : BindableBase, IDataErrorInfo
    {
        public string Title { get; set; } = "系统设置";

        public List<string> Errors = new List<string>();
        public string Error { get; set; }

        public string this[string propName]
        {
            get
            {
                PropertyInfo pi = this.GetType().GetProperty(propName);

                if(pi.IsDefined(typeof(RangeAttribute)))
                {
                    RangeAttribute ra = (RangeAttribute)pi.GetCustomAttributes(typeof(RangeAttribute), false)[0];
                    if (!ra.IsValid(pi.GetValue(this)))
                    {
                        int count = Errors.FindAll(x => x == ra.ErrorMessage).Count();
                        if (count == 0) Errors.Add(ra.ErrorMessage);
                        return ra.ErrorMessage;
                    }
                    else
                    {
                        Errors.Remove(ra.ErrorMessage);
                    }
                }
                //if(pi.IsDefined(typeof(RequiredAttribute)))
                //{
                //    RequiredAttribute ra = (RequiredAttribute)pi.GetCustomAttribute(typeof(RequiredAttribute));
                //    var ss = pi.GetValue(this);
                //    if (!ra.IsValid(pi.GetValue(this)))
                //    {
                //        int count = Errors.FindAll(x => x == ra.ErrorMessage).Count();
                //        if (count == 0) Errors.Add(ra.ErrorMessage);
                //        return ra.ErrorMessage;
                //    }
                //    else { Errors.Remove(ra.ErrorMessage); }
                //}
                return "";
            }
        }

        private int _wheelMinThreshold;
        //使用特性限制范围，并指定对应的错误信息
        [Range(0,200,ErrorMessage = "轮毂最小阈值必须大于等于0或小于等于200")]
        [Required(ErrorMessage = "轮毂最小阈值不能为空")]
        /// <summary>
        /// 轮毂最小阈值
        /// </summary>
        public int WheelMinThreshold
        {
            get { return _wheelMinThreshold; }
            set {SetProperty(ref _wheelMinThreshold, value); }
        }

        private int _windowMaxThreshold;
        [Range(1, 100, ErrorMessage = "轮毂窗口最大阈值必须大于等于1或小于等于100")]
        /// <summary>
        /// 轮毂窗口最大阈值
        /// </summary>
        public int WindowMaxThreshold
        {
            get { return _windowMaxThreshold; }
            set { SetProperty(ref _windowMaxThreshold, value); }
        }

        private double _removeMixArea;
        /// <summary>
        /// 剔除的轮毂窗口最小面积
        /// </summary>
        public double RemoveMixArea
        {
            get { return _removeMixArea; }
            set { SetProperty(ref _removeMixArea, value); }
        }

        private double _minSimilarity;
        /// <summary>
        /// 最小相似度
        /// </summary>
        public double MinSimilarity
        {
            get { return _minSimilarity; }
            set { SetProperty(ref _minSimilarity, value); }
        }

        private double _positioningGateRadius;
        /// <summary>
        /// 定位浇口区域最小半径
        /// </summary>
        public double PositioningGateRadius
        {
            get { return _positioningGateRadius; }
            set { SetProperty(ref _positioningGateRadius, value); }
        }

        private int _gateOutMinThreshold;
        /// <summary>
        /// 浇口区域轮毂最小阈值
        /// </summary>
        public int GateOutMinThreshold
        {
            get { return _gateOutMinThreshold; }
            set { SetProperty(ref _gateOutMinThreshold, value); }
        }

        private int _gateMinArea;
        /// <summary>
        /// 浇口最小面积
        /// </summary>
        public int GateMinArea
        {
            get { return _gateMinArea; }
            set { SetProperty(ref _gateMinArea, value); }
        }

        private double _gateMinRadius;
        /// <summary>
        /// 浇口最小半径
        /// </summary>
        public double GateMinRadius
        {
            get { return _gateMinRadius; }
            set { SetProperty(ref _gateMinRadius, value); }
        }

        private bool _gateDetectionSwitch;
        /// <summary>
        /// 浇口检测开关
        /// </summary>
        public bool GateDetectionSwitch
        {
            get { return _gateDetectionSwitch; }
            set 
            {
                SetProperty(ref _gateDetectionSwitch, value);
                SqlAccess.SystemDatasWrite("GateDetectionSwitch", value.ToString());
                SystemDatas.GateDetectionSwitch = value;
            }
        }

        private int _saveImageDays;
        /// <summary>
        /// 图片保存的天数
        /// </summary>
        public int SaveImageDays
        {
            get { return _saveImageDays; }
            set { SetProperty(ref _saveImageDays, value); }
        }

        private int _saveDataMonths;
        /// <summary>
        /// 数据保存的月数
        /// </summary>
        public int SaveDataMonths
        {
            get { return _saveDataMonths; }
            set { SetProperty(ref _saveDataMonths, value); }
        }

        private int _recognitionPauseSetting;
        /// <summary>
        /// 识别暂停
        /// </summary>
        public int RecognitionPauseSetting
        {
            get { return _recognitionPauseSetting; }
            set { SetProperty(ref _recognitionPauseSetting, value); }
        }

        private int _templateAdjustDays;
        /// <summary>
        /// 模板动态调整
        /// </summary>
        public int TemplateAdjustDays
        {
            get { return _templateAdjustDays; }
            set { SetProperty(ref _templateAdjustDays, value); }
        }

        private bool _confirmChangesButtonEnable = true;
        /// <summary>
        /// 确认修改按钮使能
        /// </summary>
        public bool ConfirmChangesButtonEnable
        {
            get { return _confirmChangesButtonEnable; }
            set {SetProperty(ref _confirmChangesButtonEnable, value); }
        }
        /// <summary>
        /// 确认修改按钮命令
        /// </summary>
        public DelegateCommand ConfirmChangesCommand {  get; set; }

        public SystemSettingsViewModel(IRegionManager regionManager)
        {
            WheelMinThreshold = SystemDatas.WheelMinThreshold;
            WindowMaxThreshold = SystemDatas.WindowMaxThreshold;
            RemoveMixArea = SystemDatas.RemoveMixArea;
            MinSimilarity = SystemDatas.MinSimilarity;
            PositioningGateRadius = SystemDatas.PositioningGateRadius;
            GateOutMinThreshold = SystemDatas.GateOutMinThreshold;
            GateMinArea = SystemDatas.GateMinArea;
            GateMinRadius = SystemDatas.GateMinRadius;
            SaveImageDays = SystemDatas.SaveImageDays;
            SaveDataMonths = SystemDatas.SaveDataMonths;
            var sDB = new SqlAccess().SystemDataAccess;
            var datas = sDB.Queryable<sys_bd_systemsettingsdatamodel>().First(x => x.Name == "RecognitionPauseSetting");
            SystemDatas.RecognitionPauseSetting = int.Parse(datas.Value);
            RecognitionPauseSetting = SystemDatas.RecognitionPauseSetting;
            TemplateAdjustDays = SystemDatas.TemplateAdjustDays;
            GateDetectionSwitch = SystemDatas.GateDetectionSwitch;
            ConfirmChangesCommand = new DelegateCommand(ConfirmChanges);

            EventMessage.MessageHelper.GetEvent<ParameterSettingChangedEvent>().Subscribe(ParameterSettingChanged);
        }

        private void ParameterSettingChanged(string obj)
        {
            WheelMinThreshold = SystemDatas.WheelMinThreshold;
            WindowMaxThreshold = SystemDatas.WindowMaxThreshold;
            RemoveMixArea = SystemDatas.RemoveMixArea;
        }

        private void ConfirmChanges()
        {
            bool changed = false;
            if (WheelMinThreshold < 0 || WheelMinThreshold > 100)
            {
                WheelMinThreshold = SystemDatas.WheelMinThreshold;
                EventMessage.SystemMessageDisplay("轮毂最小阈值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if (SystemDatas.WheelMinThreshold != WheelMinThreshold)
                {
                    SystemDatas.WheelMinThreshold = WheelMinThreshold;
                    SqlAccess.SystemDatasWrite("WheelMinThreshold", WheelMinThreshold.ToString());
                    changed = true;
                }
            }
            if (WindowMaxThreshold < 1 || WindowMaxThreshold > 100)
            {
                WindowMaxThreshold = SystemDatas.WindowMaxThreshold;
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
            if (RemoveMixArea < 10 || RemoveMixArea > 500)
            {
                RemoveMixArea = SystemDatas.RemoveMixArea;
                EventMessage.SystemMessageDisplay("轮毂窗口最小面积值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.RemoveMixArea != RemoveMixArea)
                {
                    SystemDatas.RemoveMixArea = RemoveMixArea;
                    SqlAccess.SystemDatasWrite("RemoveMixArea", RemoveMixArea.ToString());
                    changed = true;
                }
            }
            if (MinSimilarity < 0.6 || MinSimilarity > 1)
            {
                MinSimilarity = SystemDatas.MinSimilarity;
                EventMessage.SystemMessageDisplay("最小相似度值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.MinSimilarity != MinSimilarity)
                {
                    SystemDatas.MinSimilarity = MinSimilarity;
                    SqlAccess.SystemDatasWrite("MinSimilarity", MinSimilarity.ToString());
                    changed = true;
                }
            }
            if (PositioningGateRadius < 10 || PositioningGateRadius > 60)
            {
                PositioningGateRadius = SystemDatas.PositioningGateRadius;
                EventMessage.SystemMessageDisplay("浇口区域最小半径值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.PositioningGateRadius != PositioningGateRadius)
                {
                    SystemDatas.PositioningGateRadius = PositioningGateRadius;
                    SqlAccess.SystemDatasWrite("PositioningGateRadius", PositioningGateRadius.ToString());
                    changed = true;
                }
            }
            if (GateOutMinThreshold < 30 || GateOutMinThreshold > 200)
            {
                GateOutMinThreshold = SystemDatas.GateOutMinThreshold;
                EventMessage.SystemMessageDisplay("浇口区域轮毂最小阈值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.GateOutMinThreshold != GateOutMinThreshold)
                {
                    SystemDatas.GateOutMinThreshold = GateOutMinThreshold;
                    SqlAccess.SystemDatasWrite("GateOutMinThreshold", GateOutMinThreshold.ToString());
                    changed = true;
                }
            }
            if (GateMinArea <= 300 || GateMinArea >= 2000)
            {
                GateMinArea = SystemDatas.GateMinArea;
                EventMessage.SystemMessageDisplay("浇口最小面积值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.GateMinArea != GateMinArea)
                {
                    SystemDatas.GateMinArea = GateMinArea;
                    SqlAccess.SystemDatasWrite("GateMinArea", GateMinArea.ToString());
                    changed = true;
                }
            }
            if (GateMinRadius < 8 || GateMinRadius > 20)
            {
                GateMinRadius = SystemDatas.GateMinRadius;
                EventMessage.SystemMessageDisplay("浇口最小半径值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if (SystemDatas.GateMinRadius != GateMinRadius)
                {
                    SystemDatas.GateMinRadius = GateMinRadius;
                    SqlAccess.SystemDatasWrite("GateMinRadius", GateMinRadius.ToString());
                    changed = true;
                }
            }
            if (SaveImageDays < 0)
            {
                SaveImageDays = SystemDatas.SaveImageDays;
                EventMessage.SystemMessageDisplay("图片保存的天数值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.SaveImageDays != SaveImageDays)
                {
                    SystemDatas.SaveImageDays = SaveImageDays;
                    SqlAccess.SystemDatasWrite("SaveImageDays", SaveImageDays.ToString());
                    changed = true;
                }
            }
            if (SaveDataMonths < 0)
            {
                SaveDataMonths = SystemDatas.SaveDataMonths;
                EventMessage.SystemMessageDisplay("数据保存的月数值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.SaveDataMonths != SaveDataMonths)
                {
                    SystemDatas.SaveDataMonths = SaveDataMonths;
                    SqlAccess.SystemDatasWrite("SaveDataMonths", SaveDataMonths.ToString());
                    changed = true;
                }
            }
            if (RecognitionPauseSetting < 0)
            {
                RecognitionPauseSetting = SystemDatas.RecognitionPauseSetting;
                EventMessage.SystemMessageDisplay("识别暂停值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.RecognitionPauseSetting != RecognitionPauseSetting)
                {
                    SystemDatas.RecognitionPauseSetting = RecognitionPauseSetting;
                    SqlAccess.SystemDatasWrite("RecognitionPauseSetting", RecognitionPauseSetting.ToString());
                    EventMessage.MessageHelper.GetEvent<RecognitionPauseSettingEvent>().Publish(RecognitionPauseSetting.ToString());
                    changed = true;
                }
            }
            if (TemplateAdjustDays < 0)
            {
                TemplateAdjustDays = SystemDatas.TemplateAdjustDays;
                EventMessage.SystemMessageDisplay("模板动态调整值输入错误，请重新输入!", Models.MessageType.Error);
                return;
            }
            else
            {
                if(SystemDatas.TemplateAdjustDays != TemplateAdjustDays)
                {
                    SystemDatas.TemplateAdjustDays = TemplateAdjustDays;
                    SqlAccess.SystemDatasWrite("TemplateAdjustDays", TemplateAdjustDays.ToString());
                    changed = true;
                }
            }
            if (changed)
                EventMessage.SystemMessageDisplay("修改成功！", Models.MessageType.Success);
        }
    }
}
