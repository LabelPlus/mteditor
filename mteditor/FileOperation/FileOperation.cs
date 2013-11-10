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
        string GetDirectory(string FullPath)
        {
            if (string.IsNullOrWhiteSpace(FullPath))
                return null;
            int idx = FullPath.LastIndexOf('\\');
            string ret = FullPath.Substring(0, idx);
            return ret;
        }

        bool IsSaveModifiedFile(string FilePath)
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                FilePath = "新建文本文件";
            string msg = string.Format("\"{0}\" 已修改，是否保存？", FilePath);
            MessageBoxResult mbr = MessageBox.Show(msg, "提示", MessageBoxButton.YesNo);
            return (mbr == MessageBoxResult.Yes);
        }
    }
}
