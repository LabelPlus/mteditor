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
    /// GenerateNumber.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateNumber : Window
    {
        MainWindow pntWindow = null;
        public GenerateNumber()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(tbGenerateFrom, new DataObjectPastingEventHandler(tbGenerateFrom_Pasting));
            DataObject.AddPastingHandler(tbGenerateTo, new DataObjectPastingEventHandler(tbGenerateTo_Pasting));
        }
        public void SetCustomValues()
        {
            pntWindow = (this.Owner as MainWindow);
        }
        private void tbGenerateFrom_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateTo_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrFrom, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrFrom, 0x00, 0xFF, 0xFF);
            }
        }

        private void tbGenerateTo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrTo, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrTo, 0x00, 0xFF, 0xFF);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                int start = int.Parse(tbGenerateFrom.Text);
                int end = int.Parse(tbGenerateTo.Text);

                if ((end - start + 1) > 20000)
                {
                    stGenStatus.Text = Utilities.GeneLengthError;
                    Utilities.SetBorderColor(ref bdrGenStatus, 0xFF, 0x00, 0x00);
                    return;
                }

                string tmp = "";
                int count = 0;

                for (int i = start; i <= end; ++i, ++count)
                {
                    tmp += Utilities.addLine(i);
                }
                pntWindow.TransBoxAppend(tmp);

                sw.Stop();
                Utilities.SetBorderColor(ref bdrGenStatus, 0x00, 0xFF, 0xFF);
                stGenStatus.Text = string.Format("共生成了 {0:N0} 个编号，用时 {1:N0} 毫秒", count, sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Utilities.SetBorderColor(ref bdrGenStatus, 0xFF, 0x00, 0x00);
                stGenStatus.Text = string.Format("编号生成失败");
            }
        }

        private void wdGen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void wdGen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).IsActivated = true;
        }
    }
}
