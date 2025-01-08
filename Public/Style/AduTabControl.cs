using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WheelRecognitionSystem.Public.Style
{
    public class AduTabControl : TabControl
    {
        void SelectionState()
        {
        }

        public AduTabControl()
        {
            Loaded += delegate { ElementBase.GoToState(this, "SelectionLoaded"); };
            SelectionChanged += delegate (object sender, SelectionChangedEventArgs e) { if (e.Source is AduTabControl) { SelectionState(); } };

        }

        static AduTabControl()
        {
            ElementBase.DefaultStyle<AduTabControl>(DefaultStyleKeyProperty);
        }
    }
}
