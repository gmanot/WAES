using System;
using System.Runtime.InteropServices;
using System.Text;
using Moq;
using WAES.DataStorage;
using WAES.Shared;
using Xunit;
using FluentAssertions;

namespace WAES.BusinessLogic.UnitTests
{
    public class ByteDiffClientTest
    {
        private readonly Mock<IDataStorageClient> _dataStorageClientMock;

        public ByteDiffClientTest()
        {
            _dataStorageClientMock = new Mock<IDataStorageClient>();
        }

        //[Fact]
        //public void TestingGetDiffResult_ValidRequest_Returns()
        //{
        //    var client = GetClient();
        //    _dataStorageClientMock.Setup(x => x.GetDiffResult(It.IsAny<int>()))
        //        .Returns(new DiffResultBase() {ResultStatusString = ConstantStatusStrings.Equal});
        //    var diffResultBase = client.GetDiffResult(1);
        //    diffResultBase.Should().BeNull();
        //}

        [Fact]
        public void Upsert_ValidRequest_TestingInteraction()
        {
            // Arrange
            const int id = 1;
            var byteArray = new byte[3];
            byteArray[0] = 100;
            byteArray[1] = 110;
            byteArray[2] = 98;
            const Side side = Side.Left;

            var decodedString = Encoding.UTF8.GetString(byteArray);

            var returnedId = 0;
            var returnedString = String.Empty;
            var returnedside = Side.Right;

            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.IsReadyForDiff(It.IsAny<int>())).Returns(false);
            _dataStorageClientMock.Setup(x => x.Upsert(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Side>()))
                .Callback<int, string, Side>(
                    (x, y, z) =>
                    {
                        returnedId = x;
                        returnedString = y;
                        returnedside = z;
                    });
            // Act
            client.Upsert(id, byteArray, side);
            // Assert
            returnedId.Should().Equals(id);
            returnedString.Should().BeEquivalentTo(decodedString);
            returnedside.Should().BeEquivalentTo(side);
        }

        [Fact]
        public void PerformDiff_ValidRequest_TestingEqualsCase()
        {
            // Arrange
            const int id = 1;
            var byteArray = new byte[3];
            byteArray[0] = 100;
            byteArray[1] = 110;
            byteArray[2] = 98;
            const Side side = Side.Left;

            var bytesToCompare = new BytesToCompare()
            {
                Left = "test",
                Right = "test"
            };

            var returnedId = 0;
            var returnDiffResultBase = new DiffResultBase()
            {
                ResultStatusString = ConstantStatusStrings.Equal
            };

            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.IsReadyForDiff(It.IsAny<int>())).Returns(true);
            _dataStorageClientMock.Setup(x => x.GetBytesToCompareById(It.IsAny<int>())).Returns(bytesToCompare);
            _dataStorageClientMock.Setup(x => x.SaveDiffResult(It.IsAny<int>(), It.IsAny<DiffResultBase>()))
                .Callback<int, DiffResultBase>(
                    (x, y) =>
                    {
                        returnedId = x;
                        returnDiffResultBase = y;
                    });
            // Act
            client.Upsert(id, byteArray, side);
            // Assert
            returnedId.Should().Equals(id);
            returnDiffResultBase.ResultStatusString.Should().BeEquivalentTo(ConstantStatusStrings.Equal);
        }

        private ByteDiffClient GetClient()
        {
            return new ByteDiffClient(_dataStorageClientMock.Object);
        }
    }
}