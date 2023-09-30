using BMS_Test.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BMS_Test.MVVM.Views
{
    /// <summary>
    /// Interaction logic for VMain.xaml
    /// </summary>
    
    public partial class VMain : Window
    {
        private VMMain vmMain;
        public VMain()
        {
            InitializeComponent();
            vmMain= new VMMain();
            this.DataContext= vmMain;
            this.Closed += Window_Closed;
            //vmMain = new VMMain();
            //DataContext = vmMain;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            vmMain.WindowClosed();
        }
    }
}
