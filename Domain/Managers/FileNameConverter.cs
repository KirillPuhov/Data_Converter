using System.IO;

namespace Domain.Managers
{
    public class FileNameConverter
    {
        public string Convert(string fileName, string newFormat)
        {
            FileInfo _fileInfo = new FileInfo(fileName);

            string _fileName = _fileInfo.FullName;
            string _ext = _fileInfo.Extension;

            _fileName = _fileName.Replace(_ext, newFormat);
            return _fileName;
        }
    }
}
