using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mteditor
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void wdAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void wdAbout_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).IsActivated = true;
        }
    }
}
