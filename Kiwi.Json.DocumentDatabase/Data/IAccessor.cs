using System.Data;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IAccessor
    {
        long Long(int index);
        string String(int index);
    }

    public class DataRowAccessor : IAccessor
    {
        private readonly DataRow _dataRow;

        public DataRowAccessor(DataRow dataRow)
        {
            _dataRow = dataRow;
        }

        public long Long(int index)
        {
            return (long)_dataRow[index];
        }

        public string String(int index)
        {
            return _dataRow[index].ToString();
        }
    }
}