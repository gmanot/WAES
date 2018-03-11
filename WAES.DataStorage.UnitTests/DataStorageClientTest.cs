using FluentAssertions;
using WAES.BusinessLogic;
using WAES.Shared;
using Xunit;

namespace WAES.DataStorage.UnitTests
{
    public class DataStorageClientTest
    {
        [Fact]
        public void UpsertTest_ValidRequest_TestsStorageInsertion()
        {
            // Arrange
            const int id = 1;
            const string data = "test";
            const Side side = Side.Left;

            var client = GetClient();

            // Act
            client.Upsert(id, data, side);
            client.Storage.TryGetValue(id, out var outBytesToCompare);
            // Assert
            client.Storage.Count.Should().BeGreaterThan(0);
            outBytesToCompare.Left.Equals(data);

        }

        [Fact]
        public void UpsertTest_ValidRequest_TestsUpdate()
        {
            // Arrange
            const int id = 1;
            const string data = "test";
            const Side side = Side.Left;

            var client = GetClient();
            client.Upsert(id, data, side);

            const int newId = 2;
            const string newData = "new value";

            // Act
            client.Upsert(newId, newData, side);
            client.Storage.TryGetValue(id, out var outBytesToCompare);
            // Assert
            client.Storage.Count.Should().BeGreaterThan(0);
            outBytesToCompare.Left.Equals(newData);

        }

        [Fact]
        public void IsReadyForDiff_ValidRequest_InsufficientDataInStorage_ReturnsFalse()
        {
            // Arrange
            const int id = 1;
            const string data = "test";
            const Side side = Side.Left;

            var client = GetClient();
            client.Upsert(id, data, side);
            // Act
            var isReadyForDiff = client.AreBothValuesPresent(1);
            // Assert
            isReadyForDiff.Should().BeFalse();
        }

        [Fact]
        public void IsReadyForDiff_ValidRequest_SufficientDataInStorage_ReturnsTrue()
        {
            // Arrange
            const int id = 1;
            const string data = "test";

            var client = GetClient();
            client.Upsert(id, data, Side.Left);
            client.Upsert(id, data, Side.Right);
            // Act
            var isReadyForDiff = client.AreBothValuesPresent(1);
            // Assert
            isReadyForDiff.Should().BeTrue();
        }

        [Fact]
        public void SaveDiffResult_ValidRequest_()
        {
            // Arrange
            const int id = 1;
            const string data = "test";
            const Side side = Side.Left;

            var client = GetClient();
            client.Upsert(id, data, side);
            var diffResultBase = new DiffResultBase() { ResultStatusString = ConstantStatusStrings.Equal };
            // Act
            client.SaveDiffResult(id, diffResultBase);
            client.Storage.TryGetValue(id, out var outBytesToCompare);
            // Assert
            outBytesToCompare.DiffResult.Should().BeEquivalentTo(diffResultBase);
        }

        [Fact]
        public void GetBytesToCompareById_ValidRequest_ReturnsBytesToCompare()
        {
            // Arrange
            const int id = 1;
            const string data = "test";
            const Side side = Side.Left;

            var client = GetClient();
            client.Upsert(id, data, side);
            // Act
            var bytesToCompareById = client.GetBytesToCompareById(id);
            // Assert
            bytesToCompareById.Left.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void GetDiffResult_ValidRequest_ReturnsDiffResultBase()
        {
            // Arrange
            const int id = 1;
            const string data = "test";

            var client = GetClient();
            client.Upsert(id, data, Side.Left);
            client.Upsert(id, data, Side.Right);
            client.SaveDiffResult(id, new DiffResultBase(){ ResultStatusString = ConstantStatusStrings.Equal});
            // Act
            var diffResultBase = client.GetDiffResult(id);
            // Assert
            diffResultBase.ResultStatusString.Should().BeEquivalentTo(ConstantStatusStrings.Equal);
        }

        private DataStorageClient GetClient()
        {
            return new DataStorageClient();
        }
    }
}