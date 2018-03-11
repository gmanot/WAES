using System.Net.Http;
using System.Text;
using System.Threading;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using WAES.Shared;
using Xunit;

namespace WAES.WebApi.IntegrationTests
{
    public class WebApiIntegrationTest
    {
        private readonly TestServer _server;

        public WebApiIntegrationTest()
        {
            _server = TestServer.Create<Startup>();

        }

        [Fact]
        public void ExecuteLeftMethod_ValidRequest_Returns_OK()
        {
            const string url = "v1/diff/1/left";
            var byteArray = new byte[3];
            byteArray[0] = 100;
            byteArray[1] = 110;
            byteArray[2] = 98;

            var postData = new StringContent(JsonConvert.SerializeObject(new LeftRequest(){Base64 = byteArray}), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(url, postData, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }
        }

        [Fact]
        public void ExecuteLeftMethod_InValidRequest_Returns_ArgumentNullException()
        {
            const string url = "v1/diff/1/left";

            var postData = new StringContent(JsonConvert.SerializeObject(new LeftRequest() { Base64 = null }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(url, postData, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeFalse();

            }
        }

        [Fact]
        public void ExecuteRightMethod_ValidRequest_Returns_OK()
        {
            const string url = "v1/diff/1/right";
            var byteArray = new byte[3];
            byteArray[0] = 100;
            byteArray[1] = 110;
            byteArray[2] = 99;

            var postData = new StringContent(JsonConvert.SerializeObject(new RightRequest() { Base64 = byteArray }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(url, postData, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }
        }

        [Fact]
        public void ExecuteRightMethod_ValidRequest_Returns_()
        {
            const string url = "v1/diff/1/right";

            var postData = new StringContent(JsonConvert.SerializeObject(new RightRequest() { Base64 = null }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(url, postData, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeFalse();
            }
        }


        [Fact]
        public async void TestGetDiffMethod_ValidRequest_Returns_OK_Offset()
        {
            var urlLeft = "v1/diff/1/left";
            var byteArrayLeft = new byte[3];
            byteArrayLeft[0] = 100;
            byteArrayLeft[1] = 110;
            byteArrayLeft[2] = 98;

            var urlRight = "v1/diff/1/right";
            var byteArrayRight = new byte[3];
            byteArrayRight[0] = 100;
            byteArrayRight[1] = 110;
            byteArrayRight[2] = 99;

            var postDataLeft = new StringContent(JsonConvert.SerializeObject(new LeftRequest() { Base64 = byteArrayLeft }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlLeft, postDataLeft, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postDataRight = new StringContent(JsonConvert.SerializeObject(new RightRequest() { Base64 = byteArrayRight }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlRight, postDataRight, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            using (var response = _server.HttpClient.GetAsync("v1/diff/1").Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent.Should()
                    .BeEquivalentTo(
                        "{\"ByteArrayLenght\":3,\"OffsetIndexes\":[2],\"ResultStatusString\":\"Same size with offset\"}");
            }
        }

        [Fact]
        public async void TestGetDiffMethod_ValidRequest_Returns_OK_Equal()
        {
            var urlLeft = "v1/diff/1/left";
            var byteArrayLeft = new byte[3];
            byteArrayLeft[0] = 100;
            byteArrayLeft[1] = 110;
            byteArrayLeft[2] = 99;

            var urlRight = "v1/diff/1/right";
            var byteArrayRight = new byte[3];
            byteArrayRight[0] = 100;
            byteArrayRight[1] = 110;
            byteArrayRight[2] = 99;

            var postDataLeft = new StringContent(JsonConvert.SerializeObject(new LeftRequest() { Base64 = byteArrayLeft }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlLeft, postDataLeft, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postDataRight = new StringContent(JsonConvert.SerializeObject(new RightRequest() { Base64 = byteArrayRight }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlRight, postDataRight, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            using (var response = _server.HttpClient.GetAsync("v1/diff/1").Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent.Should()
                    .BeEquivalentTo(
                        "{\"ResultStatusString\":\"Equal\"}");
            }
        }

        [Fact]
        public async void TestGetDiffMethod_ValidRequest_Returns_OK_NotEqual()
        {
            var urlLeft = "v1/diff/1/left";
            var byteArrayLeft = new byte[4];
            byteArrayLeft[0] = 100;
            byteArrayLeft[1] = 110;
            byteArrayLeft[2] = 99;
            byteArrayLeft[3] = 22;

            var urlRight = "v1/diff/1/right";
            var byteArrayRight = new byte[3];
            byteArrayRight[0] = 100;
            byteArrayRight[1] = 110;
            byteArrayRight[2] = 99;

            var postDataLeft = new StringContent(JsonConvert.SerializeObject(new LeftRequest() { Base64 = byteArrayLeft }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlLeft, postDataLeft, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            var postDataRight = new StringContent(JsonConvert.SerializeObject(new RightRequest() { Base64 = byteArrayRight }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlRight, postDataRight, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            using (var response = _server.HttpClient.GetAsync("v1/diff/1").Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent.Should()
                    .BeEquivalentTo(
                        "{\"ResultStatusString\":\"Not Equal\"}");
            }
        }

        [Fact]
        public void TestGetDiffMethod_OnlyLeftValue_Returns_FalseSuccess()
        {
            var urlLeft = "v1/diff/1/left";
            var byteArrayLeft = new byte[3];
            byteArrayLeft[0] = 100;
            byteArrayLeft[1] = 110;
            byteArrayLeft[2] = 98;

            var postDataLeft = new StringContent(JsonConvert.SerializeObject(new LeftRequest() { Base64 = byteArrayLeft }), Encoding.UTF8, "application/json");
            using (var response = _server.HttpClient.PostAsync(urlLeft, postDataLeft, new CancellationTokenSource().Token).Result)
            {
                response.IsSuccessStatusCode.Should().BeTrue();
            }

            using (var response = _server.HttpClient.GetAsync("v1/diff/1").Result)
            {
                response.IsSuccessStatusCode.Should().BeFalse();

            }
        }
    }
}
