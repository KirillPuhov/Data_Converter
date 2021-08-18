using Domain.Contollers;
using Domain.Factory.Interfaces;
using Domain.Managers;

namespace Domain.Factory
{
    public class TxtConverter : IConverter
    {
        private Delimiters _delimiters;
        public Delimiters Delimiters
        {
            get { return _delimiters; }
            set
            {
                _delimiters = value;
            }
        }

        public bool Convert(string extension, string filePath)
        {
            switch (extension)
            {
                case ".txt":
                    return ConvertToXlsx(filePath, Delimiters);

                case ".xlsx":
                    return ConvertToTxt(filePath, Delimiters);

                case ".xls":
                    return ConvertToTxt(filePath, Delimiters);

                default:
                    return false;
            }

        }

        private bool ConvertToTxt(string filePath, Delimiters delimiters)
        {
            TextConvertController _convertController = new TextConvertController(new DataConverterToTxt(), filePath, delimiters);
            return _convertController.CreateNewFile();
        }

        private bool ConvertToXlsx(string filePath, Delimiters delimiters)
        {
            TextConvertController _convertController = new TextConvertController(new DataConverterToExcel(), filePath, delimiters);
            return _convertController.CreateNewFile();
        }
    }

    public class TxtConverterFactory : IConvertFactory
    {
        public IConverter CreateConverter()
        {
            return new TxtConverter();
        }
    }
}
