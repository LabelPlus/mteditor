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
        const string TextFileFilter = "文本文件|*.txt|所有文件|*";
        string CurrentTextPath = "";

        void OpenText()
        {
            Stopwatch sw = new Stopwatch();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = GetDirectory(CurrentTextPath);
            ofd.Filter = TextFileFilter;
            if (ofd.ShowDialog() == true) CurrentTextPath = ofd.FileName;
            else return;

            sw.Start();

            try
            {
                using (StreamReader srd = new StreamReader(CurrentTextPath, Encoding.Default, true))
                {
                    tbTranslation.Text = srd.ReadToEnd();
                }
            }
            catch
            {
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.Text = string.Format("无法打开文本 \"{0}\"", CurrentTextPath);
            }

            sw.Stop();
            IsStatusGood = true;
            UpdateColorStatus();
            stStatus.Text = string.Format("已打开文本 \"{0}\" 用时 {1:N0} 毫秒", CurrentTextPath, sw.Elapsed.TotalMilliseconds);
            IsTextModified = false;
            UpdateColorStatus();
        }
        bool SaveText()
        {
            Stopwatch sw = new Stopwatch();

            string sfn = CurrentTextPath;

            if (string.IsNullOrWhiteSpace(sfn))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = CurrentTextPath;
                sfd.Filter = TextFileFilter;
                if (sfd.ShowDialog() == true) sfn = sfd.FileName;
                else return false;
            }

            sw.Start();

            try
            {
                using (StreamWriter swt = new StreamWriter(File.Create(sfn), Encoding.Unicode))
                {
                    swt.Write(tbTranslation.Text);
                }

            }
            catch
            {
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.Text = string.Format("无法保存文本 \"{0}\"", sfn);
                return false;
            }

            sw.Stop();
            IsStatusGood = true;
            UpdateColorStatus();
            stStatus.Text = string.Format("已保存文本 \"{0}\" 用时 {1:N0} 毫秒", sfn, sw.Elapsed.TotalMilliseconds);
            IsTextModified = false;
            UpdateColorStatus();

            return true;
        }
        private void btnOpenText_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextModified && IsSaveModifiedFile(CurrentTextPath) && SaveText()) { }
            OpenText();
        }
        private void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveText();
        }
    }
}
