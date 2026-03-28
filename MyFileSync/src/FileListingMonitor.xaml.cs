using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyFileSync.src
{
    /// <summary>
    /// FileListingMonitor.xaml の相互作用ロジック
    /// </summary>
    public partial class FileListingMonitor : Window
    {
        private CancellationTokenSource _doCancel;

        public FileListingMonitor(CancellationTokenSource doCancel)
        {
            InitializeComponent();

            _doCancel = doCancel;
        }

        public void OnProgressChanged(FileListUpReport report)
        {
            doReport.Content = $"現在{report.Total}個あるファイル中、{report.Count}個目までのファイル（{report.Percent}%）を完了。。。";

            if (report.IsCompleted())
            {
                Close();
            }

            if (report.IsCancelled())
            {
                Close();
            }
        }

        private void OnDoCancel(object sender, RoutedEventArgs e)
        {
            _doCancel.Cancel();
        }
    }
}
