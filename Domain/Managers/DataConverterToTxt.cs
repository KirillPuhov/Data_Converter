using Domain.Interfaces;
using ExcelDataReader;
using System;
using System.Data;
using System.IO;

namespace Domain.Managers
{
    public class DataConverterToTxt : BaseManager, IDataConverter
    {
        private Delimiters _delimiter;

        public DataTable ConvertToDataTable(string path, Delimiters delimiters)
        {
            var _path = string.IsNullOrWhiteSpace(path) ? throw new Exception("Null or empty file path!\n") : path;
            _delimiter = delimiters;

            try
            {
                return TryConvert(_path);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        private DataTable TryConvert(string path)
        {
            var _table = new DataTable();

            FileStream _stream = File.Open(path, FileMode.Open, FileAccess.Read);

            IExcelDataReader _reader = ExcelReaderFactory.CreateReader(_stream);

            DataSet _db = _reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            var _result = ConvertToDataSetOfStrings(_db);
            DataTableCollection _tableCollection = _result.Tables;

            foreach (DataTable _data in _tableCollection)
            {
                _table = _data;
            }
            return _table;
        }

        private DataSet ConvertToDataSetOfStrings(DataSet _sourceDataSet)
        {
            var _result = _sourceDataSet.Clone();
            foreach (DataTable _table in _result.Tables)
            {
                foreach (DataColumn _column in _table.Columns)
                {
                    if (_column.DataType == typeof(int) || _column.DataType == typeof(double))
                        break;

                    if (_column.DataType == typeof(DateTime))
                        break;

                    if (_column.DataType != typeof(string))
                        _column.DataType = typeof(string);
                }
            }

            foreach (DataTable _table in _sourceDataSet.Tables)
            {
                var _targetTable = _result.Tables[_table.TableName];
                foreach (DataRow _row in _table.Rows)
                {
                    _targetTable.ImportRow(_row);
                }
            }
            return _result;
        }

        public bool CreateNewFile(DataTable table, string path)
        {
            try
            {
                switch (_delimiter)
                {
                    case Delimiters.Tabs:
                        return TryCreate(table, path, TABS);

                    case Delimiters.Semicolon:
                        return TryCreate(table, path, SEMICOLON);

                    case Delimiters.Space:
                        return TryCreate(table, path, SPACE);

                    default:
                        return TryCreate(table, path, TABS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }      
        }

        private bool TryCreate(DataTable table, string path, char delimiter)
        {
            string _headers = null;

            foreach (DataColumn item in table.Columns)
            {
                _headers += item.ColumnName + delimiter;
            }

            if (!File.Exists(path))
                File.WriteAllText(path, _headers);

            foreach (DataRow row in table.Rows)
            {
                string _row = "\n";

                foreach (var _word in row.ItemArray)
                {
                    _row += Convert.ToString(_word) + delimiter;
                }

                File.AppendAllText(path, _row);
            }

            return true;
        }
    }
}
