using System;
using System.IO;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    public class CSVStreamReader : ICSVStreamReader
    {
        private readonly string _separator;
        private StreamReader _readerStream;

        public CSVStreamReader() : this("\t")
        {

        }
        public CSVStreamReader(string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException($"The separator {separator} must have at least one character!");
            }
            _separator = separator;
        }

        public void Open(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {fileName} was not found!");
            }
            _readerStream = fileInfo.OpenText();
        }

        public bool Read(out string column1, out string column2)
        {
            //Compromise: For backward compatibility I kept this method signature, although this can be re-factored to allow reading a variable number of columns
            //consider using Generics to automatically create the object from the column values read
            if (_readerStream == null || !_readerStream.BaseStream.CanRead)
            {
                throw new IOException("Stream not initialized or can not be read. Please call Open method before trying to read.");
            }
            column1 = null;
            column2 = null;
            var line = _readerStream.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                var columns = line.Split(new[] { _separator }, StringSplitOptions.None);

                if (columns.Length >= 2)
                {
                    column1 = columns[0];
                    column2 = columns[1];
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (_readerStream != null)
            {
                _readerStream.Dispose();
                _readerStream = null;
            }
        }
    }
}