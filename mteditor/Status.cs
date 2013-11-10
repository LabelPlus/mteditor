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
        bool IsImageModified = false;
        bool IsTextModified = false;
        bool IsStatusGood = true;
        void UpdateColorStatus()
        {
            try
            {
                if (IsStatusGood) bdrStatus.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00));
                else bdrStatus.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00));
                if (IsImageModified) bdrImage.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
                else bdrImage.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
                if (IsTextModified) bdrText.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
                else bdrText.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
                if (IsImageModified || IsTextModified) wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
                else wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
            }
            catch { }
        }
    }
}
