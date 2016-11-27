using System;

namespace AddressProcessing.CSV.Interfaces
{
    public interface ICSVStreamReader : IDisposable
    {
        void Open(string fileName);
        bool Read(out string column1, out string column2);
    }
}