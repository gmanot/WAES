using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using WAES.BusinessLogic;
using WAES.Shared;
using WAES.WebApi.SelfHost;
using Xunit;

namespace WAES.WebApi.UnitTests.Controllers
{
    public class DiffControllerTest
    {
        private readonly Mock<IByteDiffClient> _byteDiffClientMock;

        public DiffControllerTest()
        {
            _byteDiffClientMock = new Mock<IByteDiffClient>();
        }

        [Fact]
        public void LeftTest_InvalidRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var controler = GetControler();
            // Act
            try
            {
                controler.Left(1, new LeftRequest());
            }
            catch (ArgumentNullException e)
            {
                //Assert
                e.ParamName.Should().BeEquivalentTo(DebugAndErrorMessages.Base64);
            }
            
        }

        [Fact]
        public void LeftTest_ValidRequest_ReturnsHttpOkResult()
        {
            // Arrange
            var controler = GetControler();
            var bytes = new byte[5];
            bytes[0] = 103;
            bytes[1] = 111;
            bytes[2] = 114;
            bytes[3] = 97;
            bytes[4] = 110;
            // Act
            var httpActionResult = controler.Left(1, new LeftRequest() {Base64 = bytes});
            //Assert
            Assert.IsType<OkResult>(httpActionResult);
        }

        [Fact]
        public void RightTest_InvalidRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var controler = GetControler();
            // Act
            try
            {
                controler.Right(1, new RightRequest());
            }
            catch (ArgumentNullException e)
            {
                //Assert
                e.ParamName.Should().BeEquivalentTo(DebugAndErrorMessages.Base64);
            }

        }

        [Fact]
        public void RightTest_ValidRequest_ReturnsHttpOkResult()
        {
            // Arrange
            var controler = GetControler();
            var bytes = new byte[5];
            bytes[0] = 102;
            bytes[1] = 110;
            bytes[2] = 113;
            bytes[3] = 96;
            bytes[4] = 109;
            // Act
            var httpActionResult = controler.Right(1, new RightRequest() { Base64 = bytes });
            //Assert
            Assert.IsType<OkResult>(httpActionResult);
        }

        [Fact]
        public void DiffTest_InvalidRequest_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var controler = GetControler();
            _byteDiffClientMock.Setup(x => x.GetDiffResult(It.IsAny<int>())).Throws<ArgumentNullException>();
            
            try
            {
                // Act
                controler.Diff(1);
            }
            catch (IndexOutOfRangeException e)
            {
                // Assert
                e.Message.Should().BeEquivalentTo(DebugAndErrorMessages.NoValue);
            }
        }

        [Fact]
        public void DiffTest_ValidRequest_ReturnsHttpOkResult()
        {
            // Arrange
            var controler = GetControler();
            _byteDiffClientMock.Setup(x => x.GetDiffResult(It.IsAny<int>())).Returns(new DiffResult()
            {
                ByteArrayLenght = 5,
                ResultStatusString = ConstantStatusStrings.SameSizeWithOffset,
                OffsetIndexes = new List<int>(new int[]{2,3,7})
            });
            // Act
            var httpActionResult = controler.Diff(1);
            // Assert
            httpActionResult.Should().BeOfType<OkNegotiatedContentResult<DiffResultBase>>();
        }

        private DiffController GetControler()
        {
            return new DiffController(_byteDiffClientMock.Object);
        }
    }
}