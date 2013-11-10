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
        private void imgShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int CurrentNumber = 0;
            try
            {
                int numbersize = int.Parse(tbNumberSize.Text);
                if(numbersize <= 0 || numbersize > 240)
                {
                    stStatus.Text = NumberSizeError;
                    IsStatusGood = false;
                    UpdateColorStatus();
                    return;
                }

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
                    numbersize,
                    Brushes.Red);

                DrawingVisual drawingVisual = new DrawingVisual();

                double DrawX = p.X * bi.PixelWidth / imgShow.ActualWidth - numbersize / 2;
                double DrawY = p.Y * bi.PixelHeight / imgShow.ActualHeight - numbersize / 2;

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
                tbTranslation.Text += addLine(CurrentNumber);
                tbTranslation.ScrollToEnd();

                ++CurrentNumber;
                tbNumberNext.Text = string.Format("{0}", CurrentNumber);
            }
            catch
            {
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.Text = string.Format("无法绘制标号");
                return;
            }

            sw.Stop();
            IsStatusGood = true;
            UpdateColorStatus();
            stStatus.Text = string.Format("已绘制标号 {0} 用时 {1:N0} 毫秒", CurrentNumber - 1, sw.Elapsed.TotalMilliseconds);
            IsImageModified = true;
            UpdateColorStatus();
        }
    }
}
