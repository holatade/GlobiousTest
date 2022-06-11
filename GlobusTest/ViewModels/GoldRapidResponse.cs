using Newtonsoft.Json;

namespace GlobusTest.ViewModels
{
    public class GoldRapidResponse
    {
        [JsonProperty("gold")]
        public decimal Gold { get; set; }
        [JsonProperty("silver")]
        public decimal Silver { get; set; }
    }
}