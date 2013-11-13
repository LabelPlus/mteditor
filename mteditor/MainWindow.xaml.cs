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
            NumberSize = 48;
            this.Width = 640;
            this.Height = 480;

            #region
            bdrImage.HorizontalAlignment = HorizontalAlignment.Left;
            bdrImage.VerticalAlignment = VerticalAlignment.Top;
            bdrImage.Margin = new Thickness(10, 10, 0, 0);

            bdrMenu.HorizontalAlignment = HorizontalAlignment.Right;
            bdrMenu.VerticalAlignment = VerticalAlignment.Top;
            bdrMenu.Margin = new Thickness(0, 10, 10, 0);

            bdrStatus.HorizontalAlignment = HorizontalAlignment.Right;
            bdrStatus.VerticalAlignment = VerticalAlignment.Bottom;
            bdrStatus.Margin = new Thickness(0, 0, 10, 10);

            bdrText.HorizontalAlignment = HorizontalAlignment.Right;
            bdrText.VerticalAlignment = VerticalAlignment.Top;
            bdrText.Margin = new Thickness(0, 36, 10, 0);

            #endregion

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

        private void wdMain_Activated(object sender, EventArgs e)
        {
            //IsActivated = true;
            //TransBoxAppend("Activated\n");
        }

        private void wdMain_Deactivated(object sender, EventArgs e)
        {
            IsActivated = false;
            //TransBoxAppend("Deactivated\n");
        }

        double portableWidth(double w)
        {
            if (w < 120) return 120;
            else return w;
        }
        private void wdMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double h = grid.ActualHeight - 20;
            double w = grid.ActualWidth - 20;

            double imgw = h * 7 / 10;
            w = w - imgw - 2;

            bdrImage.Height = h;
            bdrImage.Width = imgw;

            bdrMenu.Height = 24;
            bdrMenu.Width = portableWidth(w);

            bdrStatus.Height = 50;
            bdrStatus.Width = portableWidth(w);

            bdrText.Height = h - 24 - 2 - 50 - 2;
            bdrText.Width = portableWidth(w);
        }
    }
}
