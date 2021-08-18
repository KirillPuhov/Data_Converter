using System;
using System.Linq;
using System.Windows;
using Domain.Managers;
using System.Data;
using Domain.Factory;
using Domain.Factory.Interfaces;
using System.IO;

namespace AppUX
{
    public partial class MainWindow : Window
    {
        string _path;
        string _extension;

        public MainWindow()
        {
            InitializeComponent();
            DelimetersComboBox.ItemsSource = Enum.GetValues(typeof(Delimiters)).Cast<Delimiters>();
        }

        private void DaDplace_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] _files = (string[])e.Data.GetData(DataFormats.FileDrop);
                _path = _files[0];

                FileInfo _info = new FileInfo(_path);
                _extension = _info.Extension;

                Preview.Document.Blocks.Clear();
                if (_extension.Equals(".txt") || _extension.Equals(".xls") || _extension.Equals(".xlsx"))
                    Preview.AppendText(Environment.NewLine + _path);
                else
                    Preview.AppendText(Environment.NewLine + "invalid file extension");
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_path) && DelimetersComboBox.SelectedItem != null)
            {
                TxtConverterFactory _textConverterFactory = new TxtConverterFactory();
                IConverter converter = _textConverterFactory.CreateConverter();
                converter.Delimiters = (Delimiters)DelimetersComboBox.SelectedItem;
                converter.Convert(_extension, _path);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            _extension = null;

            if (Preview.Document.Blocks != null)
                Preview.Document.Blocks.Clear();

            if (DelimetersComboBox.SelectedItem != null)
                DelimetersComboBox.SelectedItem = null;
        }
    }
}
