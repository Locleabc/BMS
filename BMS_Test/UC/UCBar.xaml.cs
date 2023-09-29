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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BMS_Test.UC
{
    /// <summary>
    /// Interaction logic for UCBar.xaml
    /// </summary>
    public partial class UCBar : UserControl
    {
        public VMUCBar ViewModel { get; set; }
        public UCBar()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new VMUCBar();
        }
    }
}
