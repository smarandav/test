using System;
using System.IO;
using AddressProcessing.CSV;
using FluentAssertions;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvStreamWriterTests
    {
        private const string FileName = "testFile.csv";
        private readonly string[] testColumns = { "column1", "columns2", "columns3" };

        private CSVStreamWriter _csvStreamWriter;

        [SetUp]
        public void SetUp()
        {
            _csvStreamWriter = new CSVStreamWriter();
        }

        [TearDown]
        public void TearDown()
        {
            _csvStreamWriter.Dispose();
            File.Delete(FileName);
        }

        [Test]
        public void CSVStreamReader_ThrowsException_WhenInvalidSeparator()
        {
            Assert.Throws<ArgumentException>(() => new CSVStreamReader(null));
        }

        [Test]
        public void Open_ThrowsException_WhenStreamIsNotOpen()
        {
            Assert.Throws<IOException>(() => _csvStreamWriter.Write(testColumns[0], testColumns[1]));
        }

        [Test]
        public void Write_WritesColumnsToFile()
        {
            try
            {
                _csvStreamWriter.Open(FileName);

                _csvStreamWriter.Write(testColumns);
            }
            finally
            {
                _csvStreamWriter.Dispose();
            }

            string[] columns = ReadColumnsFromFile(FileName, '\t');
            columns.Should().NotBeNull();
            columns[0].Should().Be(columns[0]);
            columns[1].Should().Be(columns[1]);
        }

        private string[] ReadColumnsFromFile(string fileName, char separator)
        {
            using (var fileReader = new StreamReader(fileName))
            {
                var line = fileReader.ReadLine();
                if (line != null)
                {
                    var columns = line.Split(separator);
                    return columns;
                }
            }

            return null;
        }
    }
}
