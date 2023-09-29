using BMS_Test.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BMS_Test.MVVM.ViewModels
{
    public class VMUCBar : PropertyChangedNotifier
    {
        #region Command
        public RelayCommand CloseWindowCommand { get; set; }
        #endregion
        public VMUCBar()
        {
            //CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => { });
        }
    }
}
