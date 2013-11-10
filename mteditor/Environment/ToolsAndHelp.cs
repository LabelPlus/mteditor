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
        private void miGeneNum_Click(object sender, RoutedEventArgs e)
        {
            var w = new GenerateNumber();
            w.Owner = this;
            w.SetCustomValues();
            w.Show();
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            var w = new Settings();
            w.Owner = this;
            w.SetCustomValues();
            w.Show();
        }

        private void miManual_Click(object sender, RoutedEventArgs e)
        {
            var w = new Manual();
            w.Owner = this;
            w.Show();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            var w = new About();
            w.Owner = this;
            w.Show();
        }
    }
}
