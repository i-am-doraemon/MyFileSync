using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyFileSync
{
    /// <summary>
    /// FolderSelection.xaml の相互作用ロジック
    /// </summary>
    public partial class FolderSelection : Window
    {
        public FolderSelection()
        {
            InitializeComponent();
        }

        public string Folder1
        {
            get
            {
                return doInputFolder1.Text;
            }
        }

        public string Folder2
        {
            get
            {
                return doInputFolder2.Text;
            }
        }

        private bool TryChooseFolder(string initialFolder, out string selectedFolder)
        {
            OpenFolderDialog dialog = new OpenFolderDialog()
            {
                Title = "フォルダ選択",
                InitialDirectory = initialFolder,
                Multiselect = false, 
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                selectedFolder = 
                            dialog.FolderName;
            }
            else
            {
                selectedFolder = string.Empty;
            }

            return result ?? false;
        }

        private void OnDoChooseFolder1(object sender, RoutedEventArgs e)
        {
            if (TryChooseFolder(AppContext.BaseDirectory, out string folder))
            {
                doInputFolder1.Text = folder;
            }
        }

        private void OnDoChooseFolder2(object sender, RoutedEventArgs e)
        {
            if (TryChooseFolder(AppContext.BaseDirectory, out string folder))
            {
                doInputFolder2.Text = folder;
            }
        }

        private void OnDoOK(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(doInputFolder1.Text))
            {
                MessageBox.Show($"左側のフォルダは存在しません。");
                return;
            }

            if (!Directory.Exists(doInputFolder2.Text))
            {
                MessageBox.Show($"右側のフォルダは存在しません。");
                return;
            }

            DialogResult = true;

            Close();
        }
    }
}
