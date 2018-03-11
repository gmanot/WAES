using System;
using System.Collections.Generic;
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

        [Fact]
        public void TestingGetDiffResult_ValidRequest_Returns()
        {
            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(true);
            _dataStorageClientMock.Setup(x => x.GetDiffResult(It.IsAny<int>()))
                .Returns(new DiffResultBase() { ResultStatusString = ConstantStatusStrings.Equal });
            var diffResultBase = client.GetDiffResult(1);
            diffResultBase.ResultStatusString.Should().BeEquivalentTo(ConstantStatusStrings.Equal);
        }

        [Fact]
        public void TestingGetDiffResult__Returns()
        {
            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(false);
            Assert.Throws<ArgumentNullException>(() => client.GetDiffResult(1));
        }

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
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(false);
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
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(true);
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

        [Fact]
        public void PerformDiff_ValidRequest_TestingNotEqualsCase()
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
                Left = "rest1",
                Right = "test"
            };

            var returnedId = 0;
            var returnDiffResultBase = new DiffResultBase()
            {
                ResultStatusString = ConstantStatusStrings.NotEqual
            };

            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(true);
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
            returnDiffResultBase.ResultStatusString.Should().BeEquivalentTo(ConstantStatusStrings.NotEqual);
        }

        [Fact]
        public void PerformDiff_ValidRequest_TestingEqualsSizeWithOffsetCase()
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
                Left = "khsg",
                Right = "test"
            };

            var ints = new[] { 0, 1, 3 };

            var returnedId = 0;
            var diffResult = new DiffResult()
            {
                ResultStatusString = ConstantStatusStrings.SameSizeWithOffset,
                ByteArrayLenght = 4,
                OffsetIndexes = new List<int>(ints)
            };

            var client = GetClient();
            _dataStorageClientMock.Setup(x => x.AreBothValuesPresent(It.IsAny<int>())).Returns(true);
            _dataStorageClientMock.Setup(x => x.GetBytesToCompareById(It.IsAny<int>())).Returns(bytesToCompare);
            _dataStorageClientMock.Setup(x => x.SaveDiffResult(It.IsAny<int>(), It.IsAny<DiffResult>()))
                .Callback<int, DiffResultBase>(
                    (x, y) =>
                    {
                        returnedId = x;
                        diffResult = (DiffResult)y;
                    });
            // Act
            client.Upsert(id, byteArray, side);
            // Assert
            returnedId.Should().Equals(id);
            diffResult.ResultStatusString.Should().BeEquivalentTo(ConstantStatusStrings.SameSizeWithOffset);
            diffResult.ByteArrayLenght.Equals(bytesToCompare.Left.Length);
            diffResult.OffsetIndexes.Should().Equal(new List<int>(ints));
        }

        private ByteDiffClient GetClient()
        {
            return new ByteDiffClient(_dataStorageClientMock.Object);
        }
    }
}