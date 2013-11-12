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
        bool IsActivated { get; set; }
        void UpdateColorStatus()
        {
            try
            {
                if (IsStatusGood) Utilities.SetBorderColor(ref bdrStatus, 0x00, 0xCC, 0x00);
                else Utilities.SetBorderColor(ref bdrStatus, 0xFF, 0x00, 0x00);
                if (IsImageModified) Utilities.SetBorderColor(ref bdrImage,0xFF, 0x66, 0x00);
                else Utilities.SetBorderColor(ref bdrImage,0x00, 0x00, 0xFF);
                if (IsTextModified) Utilities.SetBorderColor(ref bdrText,0xFF, 0x66, 0x00);
                else Utilities.SetBorderColor(ref bdrText,0x00, 0x00, 0xFF);
                if (IsImageModified || IsTextModified) wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x66, 0x00));
                else wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF));
            }
            catch { }
        }

        public uint NextNumber { get; set; }
        public uint NumberSize { get; set; }
    }
}
