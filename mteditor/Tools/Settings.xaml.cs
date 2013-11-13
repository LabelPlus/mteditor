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
using System.Windows.Shapes;

namespace mteditor
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {
        MainWindow pntWindow = null;
        public Settings()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(tbNumberSize, new DataObjectPastingEventHandler(tbNumberSize_Pasting));
            DataObject.AddPastingHandler(tbNumberNext, new DataObjectPastingEventHandler(tbNumberNext_Pasting));
        }
        public void SetCustomValues()
        {
            pntWindow = (this.Owner as MainWindow);
            tbNumberSize.Text = pntWindow.NumberSize.ToString();
            tbNumberNext.Text = pntWindow.NextNumber.ToString();
            cbResetNumber.IsChecked = pntWindow.AutoResetNumber;
        }
        private void tbNumberSize_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbNumberNext_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbNumberSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrNumSize, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrNumSize, 0x00, 0xFF, 0xFF);
            }
        }

        private void tbNumberNext_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrNumNext, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrNumNext, 0x00, 0xFF, 0xFF);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pntWindow.NumberSize = uint.Parse(tbNumberSize.Text);
                pntWindow.NextNumber = uint.Parse(tbNumberNext.Text);
                pntWindow.AutoResetNumber = cbResetNumber.IsChecked.Value;
                this.Close();
            }
            catch
            {
                this.Close();
            }
        }

        private void wdSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void wdSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).IsActivated = true;
        }
    }
}
