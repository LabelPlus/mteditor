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
        const string InitNumber = "1";

        private void tbTranslation_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!IsGlobalInitDone) return;
            IsTextModified = true;
            UpdateColorStatus();
        }

        private void TransBoxAppend(string str)
        {
            tbTranslation.AppendText(str);
            tbTranslation.CaretIndex = tbTranslation.Text.Length;
            tbTranslation.ScrollToEnd();
        }
        /// <summary>
        /// 限定四个文本框只接收数字
        /// </summary>
        bool IsNumberic(string str)
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

        const string FormatError = "只能输入整数";
        const string NumberSizeError = "字体大小必须介于 1-240 之间";
        const string GeneLengthError = "一次最多只能生成 20,000 个标号";

        private void tbNumberSize_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbNumberNext_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateFrom_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateTo_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void TestFormat(ref TextCompositionEventArgs e)
        {
            if (!IsNumberic(e.Text))
            {
                stStatus.Text = FormatError;
                IsStatusGood = false;
                UpdateColorStatus();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }            
        }

        private void tbNumberSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TestFormat(ref e);
        }

        private void tbNumberNext_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TestFormat(ref e);
        }

        private void tbGenerateFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TestFormat(ref e);
        }

        private void tbGenerateTo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TestFormat(ref e);
        }

    }
}
