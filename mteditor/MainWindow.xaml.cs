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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //bool IsGlobalInitDone = false;
        public MainWindow()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            InitializeComponent();

            DataObject.AddPastingHandler(tbNumberSize, new DataObjectPastingEventHandler(tbNumberSize_Pasting));
            DataObject.AddPastingHandler(tbNumberNext, new DataObjectPastingEventHandler(tbNumberNext_Pasting));
            DataObject.AddPastingHandler(tbGenerateFrom, new DataObjectPastingEventHandler(tbGenerateFrom_Pasting));
            DataObject.AddPastingHandler(tbGenerateTo, new DataObjectPastingEventHandler(tbGenerateTo_Pasting));

            tbTranslation.Text = "";
            tbNumberSize.Text = "24";
            tbNumberNext.Text = tbGenerateFrom.Text = tbGenerateTo.Text = InitNumber;
            IsImageModified = IsTextModified = false;

            //IsGlobalInitDone = true;

            sw.Stop();
            stStatus.Text = string.Format("初始化完成，用时 {0:N0} 毫秒\n请打开图片并在图片上单击以生成标号", sw.Elapsed.TotalMilliseconds);

            IsStatusGood = true;
            UpdateColorStatus();
        }
        private void wdMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void wdMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsTextModified && IsSaveModifiedFile(CurrentTextPath) && !SaveText())
            { e.Cancel = true; return; }
            if (IsImageModified && IsSaveModifiedFile(CurrentImagePath) && !SaveImage())
            { e.Cancel = true; return; }
            e.Cancel = false;
        }
    }
}
