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
        /// <summary>
        /// 主窗口
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            tbTranslation.Text = "";
            tbNumberSize.Text = "24";
            tbNumberNext.Text = tbGenerateFrom.Text = tbGenerateTo.Text = InitNumber;
            IsImageModified = IsTextModified = false;
            StatusGreen();
            stStatus.Text = "就绪";
            //IsGlobalInitDone = true;
            UpdateColorStatus();
        }
        private void wdMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 全局变量以及状态记录
        /// </summary>
        MemoryStream imageBuffer = new MemoryStream();

        bool IsImageModified = false;
        bool IsTextModified = false;

        string CurrentImagePath = "";
        string CurrentTextPath = "";
        const string InitNumber = "1";

        //bool IsGlobalInitDone = false;

        const string TextFileFilter = "Text File|*.txt|All Files|*";
        const string OpenImageFilter = "Image Files|*.jpg;*.png;*.bmp;*.gif|All Files|*";
        const string SaveImageFilter = "Bitmap File|*.bmp|All Files|*";

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

        /// <summary>
        /// 利用窗口边框的颜色来显示状态
        /// </summary>
        void UpdateColorStatus()
        {
            if (IsImageModified) bdrImage.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
            else bdrImage.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
            if (IsTextModified) bdrText.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
            else bdrText.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
            if (IsImageModified || IsTextModified) wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
            else wdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFF));
        }
        void StatusGreen()
        {
            bdrStatus.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00));
        }
        void StatusRed()
        {
            bdrStatus.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00));
        }

        /// <summary>
        /// 文件编辑部分
        /// </summary>
        private void imgShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int CurrentNumber = 0;
            try
            {
                Point p = e.GetPosition(imgShow);
                //tbTranslation.Text += string.Format("\r\nX: {0}\r\nY: {1}\r\n", p.X, p.Y);
                imageBuffer.Seek(0, SeekOrigin.Begin);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = imageBuffer;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                FormattedText txt = new FormattedText(
                    tbNumberNext.Text,
                    new CultureInfo("zh-cn"),
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    int.Parse(tbNumberSize.Text),
                    Brushes.Red);

                DrawingVisual drawingVisual = new DrawingVisual();

                double DrawX = p.X * bi.PixelWidth / imgShow.ActualWidth;
                double DrawY = p.Y * bi.PixelHeight / imgShow.ActualHeight;

                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bi, new Rect(0, 0, bi.PixelWidth, bi.PixelHeight));
                    drawingContext.DrawText(txt, new Point(DrawX, DrawY));
                }

                RenderTargetBitmap rtb = new RenderTargetBitmap(bi.PixelWidth, bi.PixelHeight, 0, 0, PixelFormats.Pbgra32);
                rtb.Render(drawingVisual);

                imgShow.Source = rtb;

                BmpBitmapEncoder bbe = new BmpBitmapEncoder();
                bbe.Frames.Add(BitmapFrame.Create(rtb));
                bbe.Save(imageBuffer);

                CurrentNumber = int.Parse(tbNumberNext.Text);
                tbTranslation.Text += string.Format("\r\n----------<{0,5:N0}>----------\r\n", CurrentNumber);
                ++CurrentNumber;
                tbNumberNext.Text = string.Format("{0}", CurrentNumber);
            }
            catch
            {
                StatusRed();
                stStatus.Text = string.Format("无法绘制编号");
                return;
            }

            sw.Stop();
            StatusGreen();
            stStatus.Text = string.Format("已绘制编号 \"{0}\" 用时 {1:N0} 毫秒", CurrentNumber - 1, sw.Elapsed.TotalMilliseconds);
            IsImageModified = true;
            UpdateColorStatus();
        }
        private void tbTranslation_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!IsGlobalInitDone) return;
            IsTextModified = true;
            UpdateColorStatus();
            tbTranslation.ScrollToEnd();
        }

        /// <summary>
        /// 文件操作部分
        /// </summary>
        bool IsSaveModifiedFile(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                FileName = "新建";
            string fmt = string.Format("文件 \"{0}\" 已修改，是否保存？", FileName);
            MessageBoxResult mbr = MessageBox.Show(fmt, "提示", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) return true;
            else return false;
        }

        void OpenImage()
        {
            Stopwatch sw = new Stopwatch();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = GetDirectory(CurrentImagePath);
            ofd.Filter = OpenImageFilter;
            if (ofd.ShowDialog() == true) CurrentImagePath = ofd.FileName;
            else return;

            sw.Start();

            try
            {
                imageBuffer.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(CurrentImagePath);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bi, new Rect(0, 0, bi.PixelWidth, bi.PixelHeight));
                }
                RenderTargetBitmap rtb = new RenderTargetBitmap(bi.PixelWidth, bi.PixelHeight, 0, 0, PixelFormats.Pbgra32);
                rtb.Render(drawingVisual);

                imgShow.Source = rtb;

                BmpBitmapEncoder bbe = new BmpBitmapEncoder();
                bbe.Frames.Add(BitmapFrame.Create(rtb));
                bbe.Save(imageBuffer);
            }
            catch
            {
                StatusRed();
                stStatus.Text = string.Format("无法打开图像 \"{0}\"", CurrentImagePath);
            }

            sw.Stop();
            StatusGreen();
            stStatus.Text = string.Format("已打开图像 \"{0}\" 用时 {1:N0} 毫秒", CurrentImagePath, sw.Elapsed.TotalMilliseconds);
            IsImageModified = false;
            UpdateColorStatus();
        }
        bool SaveImage()
        {
            if (!IsImageModified)
                return false;
            if (imageBuffer.Length == 0)
                return false;
            Stopwatch sw = new Stopwatch();

            //string sfn = CurrentImagePath;
            string sfn = null;

            if (string.IsNullOrWhiteSpace(sfn))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = CurrentImagePath;
                sfd.Filter = SaveImageFilter;
                if (sfd.ShowDialog() == true) sfn = sfd.FileName;
                else return false;
            }

            sw.Start();

            try
            {
                imageBuffer.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = imageBuffer;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bi, new Rect(0, 0, bi.PixelWidth, bi.PixelHeight));
                }
                RenderTargetBitmap rtb = new RenderTargetBitmap(bi.PixelWidth, bi.PixelHeight, 0, 0, PixelFormats.Pbgra32);
                rtb.Render(drawingVisual);

                imgShow.Source = rtb;

                BmpBitmapEncoder bbe = new BmpBitmapEncoder();
                bbe.Frames.Add(BitmapFrame.Create(rtb));
                bbe.Save(imageBuffer);

                FileStream fs = new FileStream(sfn, FileMode.Create, FileAccess.Write);
                imageBuffer.WriteTo(fs);
            }
            catch
            {
                StatusRed();
                stStatus.Text = string.Format("无法保存图像 \"{0}\"", sfn);
                return false;
            }

            sw.Stop();
            StatusGreen();
            stStatus.Text = string.Format("已保存图像 \"{0}\" 用时 {1:N0} 毫秒", sfn, sw.Elapsed.TotalMilliseconds);
            IsImageModified = false;
            UpdateColorStatus();

            return true;
        }
        private void btnOpenImage_Click(object sender, RoutedEventArgs e)
        {
            if (IsImageModified && IsSaveModifiedFile(CurrentImagePath) && SaveImage()) { }
            OpenImage();
        }
        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }

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
                StatusRed();
                stStatus.Text = string.Format("无法打开文本 \"{0}\"", CurrentTextPath);
            }

            sw.Stop();
            StatusGreen();
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
                StatusRed();
                stStatus.Text = string.Format("无法保存文本 \"{0}\"", sfn);
                return false;
            }

            sw.Stop();
            StatusGreen();
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

        /// <summary>
        /// 退出代码
        /// </summary>
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

        /// <summary>
        /// 限定四个文本框只接收数字
        /// </summary>
        bool IsNumeric(string str)
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
        private void tbNumberSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int size = int.Parse(tbNumberSize.Text);
                if (size <=0 || size > 240)
                {
                    tbNumberSize.Text = "24";
                }
            }
            catch
            {
                tbNumberSize.Text = "24";
            }
            //if (!IsNumeric(tbNumberSize.Text)) tbNumberSize.Text = "0";
        }
        private void tbNumberNext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsNumeric(tbNumberNext.Text)) tbNumberNext.Text = InitNumber;
        }
        private void tbGenerateFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsNumeric(tbGenerateFrom.Text)) tbGenerateFrom.Text = InitNumber;
        }
        private void tbGenerateTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsNumeric(tbGenerateTo.Text)) tbGenerateTo.Text = InitNumber;
        }
        private void btnGenerateNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int start = int.Parse(tbGenerateFrom.Text);
                int end = int.Parse(tbGenerateTo.Text);
                string tmp = "";
                for (int i = start; i <= end; ++i)
                {
                    tmp += string.Format("\r\n----------<{0,5:N0}>----------\r\n", i);
                }
                tbTranslation.Text += tmp;
                sw.Stop();
                StatusGreen();
                stStatus.Text = string.Format("共生成了 {0:N0} 个编号，用时 {1:N0} 毫秒", end - start + 1, sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                StatusRed();
                stStatus.Text = string.Format("编号生成失败");
            }
        }
    }
}
