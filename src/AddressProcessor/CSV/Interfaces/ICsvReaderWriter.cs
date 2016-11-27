using System;

namespace AddressProcessing.CSV.Interfaces
{
    public interface ICSVReaderWriter : IDisposable
    {
        void Close();
        void Open(string fileName, CSVReaderWriter.Mode mode);
        bool Read(out string column1, out string column2);
        void Write(params string[] columns);
    }
}