

using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;

using System.Diagnostics;

namespace mteditor
{
    public partial class GenerateNumber : Window
    {
        delegate void GenStrDele();
        int start = 0;
        int end = 0;
        int i = 0;
        string gen_s;
        public bool isGenStrBusy = false;

        Stopwatch sw;

        void StartGen()
        {
            sw = new Stopwatch();

            gen_s = "";
            stGenStatus.Text = "";
            Utilities.SetBorderColor(ref bdrGenStatus, 0x00, 0xFF, 0xFF);
            btnClose.Content = "停止";
            isGenStrBusy = true;

            start = int.Parse(tbGenerateFrom.Text);
            end = int.Parse(tbGenerateTo.Text);
            i = start - 1;

            pbProgress.Value = 0;
            sw.Start();
        }
        void EndGen()
        {
            sw.Stop();

            isGenStrBusy = false;
            btnClose.Content = "关闭";

            if (cbContinue.IsChecked == true)
                tbGenerateFrom.Text = i.ToString();

            pntWindow.TransBoxAppend(gen_s);
            gen_s = "";
        }
        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (isGenStrBusy)
                isGenStrBusy = false;
            else
                this.Close();
        }
        void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isGenStrBusy) return;
                StartGen();
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new GenStrDele(GenStr));
            }
            catch
            {
                Utilities.SetBorderColor(ref bdrGenStatus, 0xFF, 0x00, 0x00);
                stGenStatus.Text = string.Format("编号生成失败");
                pbProgress.Value = 0;
            }
        }
        void GenStr()
        {
            if (++i <= end && isGenStrBusy)
            {
                gen_s += Utilities.addLine(i);
                double val = ((double)i - (double)start) / ((double)end - (double)start) * 100;
                pbProgress.Value = val;

                stGenStatus.Text = string.Format("{0,6:0.00}% {1,8:0.000}", val, sw.Elapsed.TotalSeconds);
                Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new GenStrDele(GenStr));
            }
            else
            {
                EndGen();
            }
        }
    }
}


namespace mteditor
{
    public partial class GenerateNumber : Window
    {
        private void wdGen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void wdGen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).IsActivated = true;
        }
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
                Utilities.SetBorderColor(ref bdrFrom, 0xFF, 0x00, 0x00);
            else
                Utilities.SetBorderColor(ref bdrFrom, 0x00, 0xFF, 0xFF);
        }

        private void tbGenerateTo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Utilities.IsGoodFormat(ref e))
                Utilities.SetBorderColor(ref bdrTo, 0xFF, 0x00, 0x00);
            else
                Utilities.SetBorderColor(ref bdrTo, 0x00, 0xFF, 0xFF);
        }
    }
}
