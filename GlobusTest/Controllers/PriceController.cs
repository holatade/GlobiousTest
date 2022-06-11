using GlobusTest.Settings;
using GlobusTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlobusTest.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly ILogger<PriceController> _logger;
        private readonly RapidApiSettings _rapidSettings;

        public PriceController(IOptions<RapidApiSettings> rapidSettings,ILogger<PriceController> logger)
        {
            _logger = logger;
            _rapidSettings = rapidSettings.Value;
        }

        /// <summary>
        /// Get Gold Live Price
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage<GoldRapidResponse>))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GoldPrice()
        {
            _logger.LogInformation($"Get gold live prics \n");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _rapidSettings.ApiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", _rapidSettings.ApiHost);
            var response = await client.GetAsync($"{_rapidSettings.BaseUrl}{_rapidSettings.GoldLivePrice}");
            string apiResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Gold Live price Response [Payload : {apiResponse}]\n");
            if (response.IsSuccessStatusCode)
            {
                var rapidResponse = JsonConvert.DeserializeObject<GoldRapidResponse>(apiResponse);
                return Ok(new ResponseMessage<GoldRapidResponse> { ResponseCode="00", ResponseDescription="Approved or completed Successfully",
                    Data = rapidResponse});
            }
            return BadRequest(new ResponseMessage { ResponseCode="99", ResponseDescription="Unspecified Error, please try again later"});
        }
    }
}
