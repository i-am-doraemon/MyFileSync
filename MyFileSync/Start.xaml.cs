using Microsoft.Win32;
using MyFileSync.src;
using System.IO;
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

namespace MyFileSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void CompareFolders(string folder1, string folder2)
        {
            CancellationTokenSource doCancel = new CancellationTokenSource();

            FileListingMonitor fileListingMonitor = new FileListingMonitor(doCancel);

            Progress<FileListUpReport> progress =
                new Progress<FileListUpReport>(fileListingMonitor.OnProgressChanged);

            fileListingMonitor.ContentRendered += async (s, e) =>
            {
                FileComparisonResult fileComparisonResult =
                                         await Files.CompareFolders(folder1,
                                                                    folder2, 
                                                                    SearchOption.AllDirectories, 
                                                                    progress,
                                                                    doCancel.Token);

                doListUpFiles.ItemsSource = fileComparisonResult.GetRecords();
            };

            fileListingMonitor.ShowDialog();
            doCancel.Dispose();
        }

        private void OnDoSelectFolders(object sender, RoutedEventArgs e)
        {
            FolderSelection folderSelection = new FolderSelection();

            bool? result = folderSelection.ShowDialog();
            if (result == true)
            {
                string folder1 = folderSelection.Folder1;
                string folder2 = folderSelection.Folder2;

                CompareFolders(folder1, folder2);
            }
        }
    }
}