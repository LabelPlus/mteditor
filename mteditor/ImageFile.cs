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
        MemoryStream imageBuffer = new MemoryStream();
        const string OpenImageFilter = "图像文件|*.jpg;*.png;*.bmp;*.gif|所有文件|*";
        const string SaveImageFilter = "JPEG 图像|*.jpg|所有文件|*";
        string CurrentImagePath = "";
        string CurrentImageName = "";

        void OpenImage()
        {
            Stopwatch sw = new Stopwatch();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = GetDirectory(CurrentImagePath);
            ofd.FileName = CurrentImageName;
            ofd.Filter = OpenImageFilter;

            if (ofd.ShowDialog() == true)
            {
                CurrentImagePath = ofd.FileName;
                CurrentImageName = ofd.SafeFileName;
            }
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
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.Text = string.Format("无法打开图像 \"{0}\"", CurrentImagePath);
            }

            TransBoxAppend("\r\n>>> " + CurrentImageName + " <<<\r\n");

            sw.Stop();
            IsStatusGood = true;
            UpdateColorStatus();
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
                sfd.InitialDirectory = GetDirectory(CurrentImagePath);
                sfd.FileName = CurrentImageName;
                sfd.Filter = SaveImageFilter;
                if (sfd.ShowDialog() == true) sfn = sfd.FileName;
                else return false;
            }

            sw.Start();

            try
            {
                //BmpBitmapDecoder bbd = new BmpBitmapDecoder(imageBuffer, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);


                imageBuffer.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = imageBuffer;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                JpegBitmapEncoder jbe = new JpegBitmapEncoder();
                jbe.QualityLevel = 80;
                jbe.Frames.Add(BitmapFrame.Create(bi));
                //DrawingVisual drawingVisual = new DrawingVisual();
                //using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                //{
                //    drawingContext.DrawImage(bi, new Rect(0, 0, bi.PixelWidth, bi.PixelHeight));
                //}
                //RenderTargetBitmap rtb = new RenderTargetBitmap(bi.PixelWidth, bi.PixelHeight, 0, 0, PixelFormats.Pbgra32);
                //rtb.Render(drawingVisual);

                //imgShow.Source = rtb;

                //BmpBitmapEncoder bbe = new BmpBitmapEncoder();
                //bbe.Frames.Add(BitmapFrame.Create(rtb));
                //bbe.Save(imageBuffer);

                using(FileStream fs = new FileStream(sfn, FileMode.Create, FileAccess.Write)                )
                {
                    jbe.Save(fs);
                }
                //imageBuffer.WriteTo(fs);
            }
            catch
            {
                IsStatusGood = false;
                UpdateColorStatus();
                stStatus.Text = string.Format("无法保存图像 \"{0}\"", sfn);
                return false;
            }

            sw.Stop();
            IsStatusGood = true;
            UpdateColorStatus();
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

    }
}
