using System;

namespace AddressProcessing.CSV.Interfaces
{
    public interface ICSVStreamWriter : IDisposable
    {
        void Open(string fileName);
        void Write(params string[] columns);
    }
}