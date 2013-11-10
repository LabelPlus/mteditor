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
    static class Utilities
    {
        static bool IsNumberic(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (str.Length > 9)
                return false;
            foreach (var c in str)
            {
                if (!char.IsNumber(c))
                    return false;
            }
            return true;
        }

        public static string FormatError = "只能输入整数";
        public const string NumberSizeError = "字体大小必须介于 1-240 之间";
        public const string GeneLengthError = "一次最多只能生成 20,000 个标号";


        public static bool IsGoodFormat(ref TextCompositionEventArgs e)
        {
            if (!IsNumberic(e.Text))
            {
                e.Handled = true;
                return false;
            }
            else
            {
                e.Handled = false;
                return true;
            }
        }
        public static void SetBorderColor(ref Border bdr, byte r, byte g, byte b)
        {
            bdr.BorderBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public static string addLine(int number)
        {
            string tmp = string.Format("<{0:N0}>", number);
            int line = 40 - tmp.Length;
            int lineL = line / 2;
            int lineR = line - lineL;
            return "\r\n" + new string('-', lineL) + tmp + new string('-', lineR) + "\r\n";
        }
    }
}
