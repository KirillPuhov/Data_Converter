using Domain.Interfaces;
using Domain.Managers;
using System.Data;
using System.IO;

namespace Domain.Contollers
{
    public class TextConvertController
    {
        private readonly IDataConverter _dataConverter;
        private string _filePath;
        private Delimiters _delimiters;

        public TextConvertController(IDataConverter dataConverter, string filePath, Delimiters delimiters)
        {
            _dataConverter = dataConverter;
            _filePath = filePath;
            _delimiters = delimiters;
        }

        private DataTable ConvertToDataTable()
        {
            return _dataConverter.ConvertToDataTable(_filePath, _delimiters);
        }

        public bool CreateNewFile()
        {
            FileInfo _file = new FileInfo(_filePath);
            string _path = @"D:\NewFile.txt";
            

            if (_file.Extension.Equals(".txt"))
               _path = new FileNameConverter().Convert(_filePath, ".xlsx");

            if (_file.Extension.Equals(".xlsx") || _file.Extension.Equals(".xls"))
                _path = new FileNameConverter().Convert(_filePath, ".txt");

            return _dataConverter.CreateNewFile(ConvertToDataTable(), _path);
        }
    }
}
