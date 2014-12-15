

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Threading;

using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;

namespace mteditor
{
    public partial class MainWindow
    {
        delegate void draw_dele();
        MouseButtonEventArgs g_e;

        void draw()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {

                //if (NumberSize <= 0 || NumberSize > 240)
                //{
                //    stStatus.AppendText(Utilities.NumberSizeError;
                //    IsStatusGood = false;
                //    UpdateColorStatus();
                //    return;
                //}

                Point p = g_e.GetPosition(imgShow);
                imageBuffer.Seek(0, SeekOrigin.Begin);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = imageBuffer;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                FormattedText txt = new FormattedText(
                    NextNumber.ToString(),
                    new CultureInfo("zh-cn"),
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    NumberSize,
                    Brushes.Red);

                DrawingVisual drawingVisual = new DrawingVisual();

                double DrawX = p.X * bi.PixelWidth / imgShow.ActualWidth - NumberSize / 2;
                double DrawY = p.Y * bi.PixelHeight / imgShow.ActualHeight - NumberSize / 2;

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

                TransBoxAppend(Utilities.addLine((int)NextNumber++));
                //++NextNumber;
            }
            catch
            {
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.AppendText(string.Format("无法绘制标号\n"));
                stStatus.ScrollToEnd();
                stStatus.CaretIndex = stStatus.Text.Length;
                return;
            }

            sw.Stop();
            IsStatusGood = true;
            IsImageModified = true;
            UpdateColorStatus();
            stStatus.AppendText(string.Format("已绘制标号 {0} 用时 {1:N0} 毫秒\n", NextNumber - 1, sw.Elapsed.TotalMilliseconds));
            stStatus.ScrollToEnd();
            stStatus.CaretIndex = stStatus.Text.Length;
        }
        private void imgShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsActivated)
            {
                IsActivated = true;
                return;
            }

            g_e = e;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new draw_dele(this.draw));
        }
    }
}
