using Domain.Managers;

namespace Domain.Factory.Interfaces
{
    public interface IConverter
    {
        Delimiters Delimiters { get; set; }

        bool Convert(string extension, string filePath);
    }
}
