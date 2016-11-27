using System;
using System.IO;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    public class CSVStreamWriter : ICSVStreamWriter
    {
        private readonly string _separator;

        public CSVStreamWriter() : this("\t")
        {

        }
        public CSVStreamWriter(string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException($"The separator {separator} must have at least one character!");
            }
            _separator = separator;
        }
        private StreamWriter _writerStream;

        public void Open(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
        }

        public void Write(params string[] columns)
        {
            //consider using Generics to automatically serialize an object into CSV
            if (_writerStream == null || !_writerStream.BaseStream.CanWrite)
            {
                throw new IOException("Stream not initialized or can not be written. Please call Open method before trying to write.");
            }
            var outPut = string.Join(_separator, columns);

            _writerStream.WriteLine(outPut);
        }

        public void Dispose()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
                _writerStream = null;
            }
        }
    }
}