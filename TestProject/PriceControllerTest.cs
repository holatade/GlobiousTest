using GlobusTest.Controllers;
using GlobusTest.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class PriceControllerTest
    {
        private readonly IOptions<RapidApiSettings> rapidApiSettings;
        private readonly PriceController _controller;
        private readonly Mock<ILogger<PriceController>> _logger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        public PriceControllerTest()
        {
            _logger = new Mock<ILogger<PriceController>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .Build();
            rapidApiSettings = Options.Create(configuration.GetSection("RapidApiSettings").Get<RapidApiSettings>());
            _controller = new PriceController(rapidApiSettings, _logger.Object);
        }

        [Fact]
        public void Customers_ShouldReturnOk()
        {
            //var clientHandlerStub = new DelegatingHandlerStub();
            //var client = new HttpClient(clientHandlerStub);
            //_httpClientFactory.Setup(_ => _.CreateClient("RapidClient")).Returns(client);
            //IHttpClientFactory factory = _httpClientFactory.Object;
            var actionResult = _controller.GoldPrice();
            var result = actionResult.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }
    }

    public class DelegatingHandlerStub : DelegatingHandler {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub() {
                _handlerFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc) {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            return _handlerFunc(request, cancellationToken);
        }
    }
}
