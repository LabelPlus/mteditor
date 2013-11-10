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
        /// <summary>
        /// 过滤文件名的路径名，请不要试图传入一个路径名
        /// </summary>
        /// <param name="FullPath">包含文件名的完整路径</param>
        /// <returns>去掉安全文件名的路径</returns>
        string GetDirectory(string FullPath)
        {
            if (string.IsNullOrWhiteSpace(FullPath))
                return null;
            int idx = FullPath.LastIndexOf('\\');
            string ret = FullPath.Substring(0, idx);
            return ret;
        }

        bool IsSaveModifiedFile(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                FileName = "新建文本";
            string fmt = string.Format("文件 \"{0}\" 已修改，是否保存？", FileName);
            MessageBoxResult mbr = MessageBox.Show(fmt, "提示", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) return true;
            else return false;
        }

    }
}