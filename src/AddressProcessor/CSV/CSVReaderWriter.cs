using System;
using System.ComponentModel;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.

Refactoring steps:

1) Created unit test for current functionality before re-factoring
2) Method: public bool Read(string column1, string column2) needs to be removed because the parameters will not change outside the method
3) The public methods were extracted into interface ICSVReaderWriter so that you can have multiple implementation of this,
	be able to mock it in unit tests, use dependency injection
4) Working with streams require implementing IDisposable, so I have implemented it
5) Re-factored method public void Write(params string[] columns) to optimize the string concatenation
6) Injected the Separator in constructor to allow a custom separator to be specified, 
	so that this class can be reused to read/write other files with different separators
7) Added validations to the stream variables before trying to read or write on them
8) Re-factored public bool Read(out string column1, out string column2) not to throw exception when there are less than 2 columns to read
9) Applied single responsibility principle and extracted the Read and Write functionalities into separate classes.
10) Created unit tests for new classes added 

Improvements to consider:

-In the read method from CSVStreamReader class consider using Generics to automatically create the object from the column values read
-In the write method from CSVStreamWriter class consider using Generics to automatically serialize an object into CSV
-Consider a wider re-factor to use async/await to improve throughput

    */

    public class CSVReaderWriter : ICSVReaderWriter
    {
        private readonly ICSVStreamReader _csvStreamReader;
        private readonly ICSVStreamWriter _csvStreamWriter;

        public CSVReaderWriter() : this(new CSVStreamReader(), new CSVStreamWriter())
        {//for backwards compatibility I have put a default constructor
        }

        public CSVReaderWriter(ICSVStreamReader csvStreamReader, ICSVStreamWriter csvStreamWriter)
        {
            _csvStreamReader = csvStreamReader;
            _csvStreamWriter = csvStreamWriter;
        }

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _csvStreamReader.Open(fileName);
            }
            else if (mode == Mode.Write)
            {
                _csvStreamWriter.Open(fileName);
            }
            else
            {
                throw new InvalidEnumArgumentException("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            _csvStreamWriter.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            //Compromise: For backwards compatibility I kept this method although this needs to be removed
            //because the parameters will not change outside the method
            return _csvStreamReader.Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            return _csvStreamReader.Read(out column1, out column2);
        }

        public void Close()
        {
            _csvStreamWriter?.Dispose();
            _csvStreamReader?.Dispose();
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
