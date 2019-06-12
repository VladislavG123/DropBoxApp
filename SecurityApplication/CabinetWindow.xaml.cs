using Microsoft.Win32;
using SecurityApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SecurityApplication
{
    public partial class CabinetWindow : Window
    {
        private string _uri;

        public CabinetWindow()
        {
            InitializeComponent();
            uploadButton.IsEnabled = false;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChouseButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                _uri = fileDialog.FileName;
                uploadButton.IsEnabled = true;
            }
            catch (Exception)
            {}

        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = await new DropBoxService().ListRootFolder();
        }

        private async void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            await new DropBoxService().Upload(_uri, "file.txt");
             
        }
    }
}
