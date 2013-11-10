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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;

namespace mteditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IsImageModified = false;
            IsTextModified = false;
            IsStatusGood = true;
            NextNumber = 1;
            NumberSize = 24;
            UpdateColorStatus();
        }
        private void wdMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void wdMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsTextModified && IsSaveModifiedFile(CurrentTextPath) && !SaveText(false))
            { e.Cancel = true; return; }
            if (IsImageModified && IsSaveModifiedFile(CurrentImagePath) && !SaveImage(false))
            { e.Cancel = true; return; }
            e.Cancel = false;
        }
        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
