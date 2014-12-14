

using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;

using System.Diagnostics;

namespace mteditor
{
    /// <summary>
    /// GenerateNumber.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateNumber : Window
    {
        MainWindow pntWindow = null;
        public GenerateNumber()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(tbGenerateFrom, new DataObjectPastingEventHandler(tbGenerateFrom_Pasting));
            DataObject.AddPastingHandler(tbGenerateTo, new DataObjectPastingEventHandler(tbGenerateTo_Pasting));
        }
        public void SetCustomValues()
        {
            pntWindow = (this.Owner as MainWindow);
        }
        private void tbGenerateFrom_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateTo_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void tbGenerateFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrFrom, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrFrom, 0x00, 0xFF, 0xFF);
            }
        }

        private void tbGenerateTo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
            {
                Utilities.SetBorderColor(ref bdrTo, 0xFF, 0x00, 0x00);
            }
            else
            {
                Utilities.SetBorderColor(ref bdrTo, 0x00, 0xFF, 0xFF);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (isGenStrBusy)
            {
                isGenStrBusy = false;
                btnClose.Content = "关闭";
                pbProgress.Value = 0;
                stGenStatus.Text = "";
            }
            else
            {
                this.Close();
            }
        }

        private delegate void GenStrDele();
        private int start = 0;
        private int end = 0;
        private int i = 0;

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pbProgress.Value = 0;

                start = int.Parse(tbGenerateFrom.Text);
                end = int.Parse(tbGenerateTo.Text);

                //if ((end - start + 1) > 20000)
                //{
                //    stGenStatus.Text = Utilities.GeneLengthError;
                //    Utilities.SetBorderColor(ref bdrGenStatus, 0xFF, 0x00, 0x00);
                //    return;
                //}

                i = start - 1;
                btnClose.Content = "停止";
                isGenStrBusy = true;
                Utilities.SetBorderColor(ref bdrGenStatus, 0x00, 0xFF, 0xFF);
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new GenStrDele(GenStr));

            }
            catch
            {
                Utilities.SetBorderColor(ref bdrGenStatus, 0xFF, 0x00, 0x00);
                stGenStatus.Text = string.Format("编号生成失败");
            }
        }

        public bool isGenStrBusy = false;
        private void GenStr()
        {
            ++i;
            if (i <= end && isGenStrBusy)
            {
                pntWindow.TransBoxAppend(Utilities.addLine(i));
                double val = ((double)i - (double)start) / ((double)end - (double)start) * 100;
                pbProgress.Value = val;
                stGenStatus.Text = string.Format("{0,6:0.00}%", val);
                Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new GenStrDele(GenStr));
            }
            else
            {
                isGenStrBusy = false;
                btnClose.Content = "关闭";
            }
        }

        private void wdGen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void wdGen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).IsActivated = true;
        }
    }
}
