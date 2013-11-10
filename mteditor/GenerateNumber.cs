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
    public partial class MainWindow
    {
        private string addLine(int number)
        {
            string tmp = string.Format("<{0:N0}>", number);
            int line = 40 - tmp.Length;
            int lineL = line / 2;
            int lineR = line - lineL;
            //tmp = tmp.PadLeft(lineL, '-');
            //tmp = tmp.PadRight(lineR, '-');
            return "\r\n" + new string('-', lineL) + tmp + new string('-', lineR) + "\r\n";
        }
        private void btnGenerateNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                int start = int.Parse(tbGenerateFrom.Text);
                int end = int.Parse(tbGenerateTo.Text);

                if( (end - start + 1) > 20000)
                {
                    stStatus.Text = GeneLengthError;
                    IsStatusGood = false;
                    UpdateColorStatus();
                    return;
                }

                string tmp = "";
                int count = 0;

                for (int i = start; i <= end; ++i, ++count)
                {
                    tmp += addLine(i);
                }
                tbTranslation.Text += tmp;
                tbTranslation.ScrollToEnd();

                sw.Stop();
                IsStatusGood = true;
                UpdateColorStatus();
                stStatus.Text = string.Format("共生成了 {0:N0} 个编号，用时 {1:N0} 毫秒", count, sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                stStatus.Text = string.Format("编号生成失败");
                IsStatusGood = false;
                UpdateColorStatus();
            }
        }
    }
}
