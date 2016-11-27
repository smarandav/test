using System.ComponentModel;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private const string FileName = "testFile.csv";
        private readonly string[] testColumns = { "column1", "columns2" };

        private CSVReaderWriter _csvReaderWriter;

        private Mock<ICSVStreamReader> _ICSVStreamReaderMock;
        private Mock<ICSVStreamWriter> _ICSVStreamWriterMock;

        [SetUp]
        public void SetUp()
        {
            _ICSVStreamReaderMock = new Mock<ICSVStreamReader>();
            _ICSVStreamWriterMock = new Mock<ICSVStreamWriter>();
            _csvReaderWriter = new CSVReaderWriter(_ICSVStreamReaderMock.Object, _ICSVStreamWriterMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _csvReaderWriter.Close();
        }

        [Test]
        public void Open_ThrowsException_WhenNotOpenedForReadOrWrite()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => _csvReaderWriter.Open(FileName, CSVReaderWriter.Mode.Read | CSVReaderWriter.Mode.Write));
        }

        [Test]
        public void Open_OpensStreamReader_WhenClassUsedForReading()
        {
            _csvReaderWriter.Open(FileName, CSVReaderWriter.Mode.Read);

            _ICSVStreamReaderMock.Verify(r => r.Open(FileName), Times.Once);
        }

        [Test]
        public void Open_OpensStreamWriter_WhenClassUsedForWriting()
        {
            _csvReaderWriter.Open(FileName, CSVReaderWriter.Mode.Write);

            _ICSVStreamWriterMock.Verify(r => r.Open(FileName), Times.Once);
        }

        [Test]
        public void Write_CallsWriter_WhenWritingToFile()
        {
            _csvReaderWriter.Open(FileName, CSVReaderWriter.Mode.Write);

            _csvReaderWriter.Write(testColumns);

            _ICSVStreamWriterMock.Verify(r => r.Write(testColumns), Times.Once);
        }

        [Test]
        public void Read_CallsRead_WhenReadingFromFile()
        {
            _csvReaderWriter.Open(FileName, CSVReaderWriter.Mode.Read);

            string column1, column2;
            _csvReaderWriter.Read(out column1, out column2);

            _ICSVStreamReaderMock.Verify(r => r.Read(out column1, out column2), Times.Once);
        }

        [Test]
        public void Close_ClosesStreamReader()
        {
            _csvReaderWriter.Close();

            _ICSVStreamReaderMock.Verify(r => r.Dispose(), Times.Once);
        }

        [Test]
        public void Close_ClosesStreamWriter()
        {
            _csvReaderWriter.Close();

            _ICSVStreamWriterMock.Verify(r => r.Dispose(), Times.Once);
        }

        [Test]
        public void Dispose_ClosesStreamReader()
        {
            _csvReaderWriter.Dispose();

            _ICSVStreamReaderMock.Verify(r => r.Dispose(), Times.Once);
        }

        [Test]
        public void Dispose_ClosesStreamWriter()
        {
            _csvReaderWriter.Dispose();

            _ICSVStreamWriterMock.Verify(r => r.Dispose(), Times.Once);
        }
    }
}
