﻿using System;
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

        private void tbTranslation_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!IsGlobalInitDone) return;
            IsTextModified = true;
            UpdateColorStatus();
        }

        public void TransBoxAppend(string str)
        {
            tbTranslation.AppendText(str);
            tbTranslation.CaretIndex = tbTranslation.Text.Length;
            tbTranslation.ScrollToEnd();
        }
    }
}
