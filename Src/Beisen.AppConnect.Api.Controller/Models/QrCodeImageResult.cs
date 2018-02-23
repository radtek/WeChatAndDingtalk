using Newtonsoft.Json;

namespace Beisen.AppConnect.Api.Controller.Models
{
    public class QrCodeImageResult:ApiResult
    {
        [JsonProperty("qr_code")]
        public string QrCode { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
}
