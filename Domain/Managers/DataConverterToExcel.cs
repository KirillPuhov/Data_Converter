using Domain.Interfaces;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;

namespace Domain.Managers
{
    public class DataConverterToExcel : BaseManager, IDataConverter
    {
        public DataTable ConvertToDataTable(string path, Delimiters delimiters)
        {
            try
            {
                switch (delimiters)
                {
                    case Delimiters.Tabs:
                        return TryConvert(path, TABS);

                    case Delimiters.Semicolon:
                        return TryConvert(path, SEMICOLON);

                    case Delimiters.Space:
                        return TryConvert(path, SPACE);

                    default:
                        return TryConvert(path, TABS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        private DataTable TryConvert(string path, char delimiters)
        {
            string[] _txt = File.Exists(path) ? File.ReadAllLines(path) : throw new Exception("Null or empty file path!\n");

            string[] _headers = _txt[0].Split(new char[] { delimiters });

            DataRow _row;
            DataTable _table = new DataTable("table");

            foreach (string header in _headers)
            {
                _table.Columns.Add(header, typeof(string));
            }

            for (int i = 1; i < _txt.Length; i++)
            {
                _row = _table.NewRow();
                string[] _words = _txt[i].Split(new char[] { delimiters });
                _row.ItemArray = _words;
                _table.Rows.Add(_row);
            }

            return _table;
        }

        public bool CreateNewFile(DataTable table, string path)
        {
            try
            {
                return TryCreate(table, path);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        private bool TryCreate(DataTable table, string filePath)
        {
            FileInfo _file = new FileInfo(filePath);
            using (ExcelPackage _package = new ExcelPackage(_file))
            {
                ExcelWorksheet _worksheet = _package.Workbook.Worksheets.Add("DataTable");
                _worksheet.Cells["A1"].LoadFromDataTable(table, true);
                _package.Save();
            }

            return true;
        }
    }
}
