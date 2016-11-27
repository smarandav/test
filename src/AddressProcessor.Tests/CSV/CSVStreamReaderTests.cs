using System;
using System.IO;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVStreamReaderTests
    {
        private const string FileName = "test_data/contacts.csv";
        private const string InvalidFileName = "test_data/invalidFile.csv";

        private ICSVStreamReader _csvStreamReader;

        [SetUp]
        public void SetUp()
        {
            _csvStreamReader = new CSVStreamReader();
        }

        [TearDown]
        public void TearDown()
        {
            _csvStreamReader.Dispose();
        }

        [Test]
        public void CSVStreamReader_ThrowsException_WhenInvalidSeparator()
        {
            Assert.Throws<ArgumentException>(() => new CSVStreamReader(null));
        }

        [Test]
        public void Open_ThrowsException_WhenStreamIsNotOpen()
        {
            string column1, column2;
            Assert.Throws<IOException>(() => _csvStreamReader.Read(out column1, out column2));
        }

        [Test]
        public void Read_ReadsFirstTwoColumnsOnlyFromFile()
        {
            _csvStreamReader.Open(FileName);
            string column1, column2;
            var result = _csvStreamReader.Read(out column1, out column2);

            result.Should().BeTrue();
            column1.Should().Be("Shelby Macias");
            column2.Should().Be("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England");
        }

        [Test]
        public void Read_RetunsNulls_WhenThreIsOneColumnOnly()
        {
            _csvStreamReader.Open(InvalidFileName);
            string column1, column2;
            var result = _csvStreamReader.Read(out column1, out column2);

            result.Should().BeFalse();
            column1.Should().BeNull();
            column2.Should().BeNull();
        }

        [Test]
        public void Read_ReadsEntireFile()
        {
            _csvStreamReader.Open(FileName);

            string column1, column2;

            int counter = 0;
            while (_csvStreamReader.Read(out column1, out column2))
            {
                counter++;
            }

            counter.Should().Be(229);
        }
    }
}
