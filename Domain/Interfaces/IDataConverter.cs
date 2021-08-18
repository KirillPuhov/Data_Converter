using Domain.Managers;
using System.Data;

namespace Domain.Interfaces
{
    public interface IDataConverter
    {
        DataTable ConvertToDataTable(string path, Delimiters delimiters);

        bool CreateNewFile(DataTable table, string path);
    }
}
