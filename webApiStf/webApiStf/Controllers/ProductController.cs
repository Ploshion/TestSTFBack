using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.Xml;

namespace webApiStf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly SettingsConexions _apiOptions;

        public ProductController(IOptions<SettingsConexions> apiOptions)
        {
            _apiOptions = apiOptions.Value;
        }

        [HttpGet("ByRef/{reference}")]
        public async Task<IActionResult> GetProduct(string reference)
        {
            using var httpClient = new HttpClient();

            var requestUri = new Uri($"{_apiOptions.BaseUrl}/api/catalog_system/pvt/products/productgetbyrefid/{reference}");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", _apiOptions.AppKey);
            httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", _apiOptions.AppToken);

            var response = await httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [HttpGet("ByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name, int top)
        {
            using var httpClient = new HttpClient();

            var requestUri = new Uri($"{_apiOptions.BaseUrl}/api/catalog_system/pub/products/search/?_from=1&_to={top}&{name}");

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", _apiOptions.AppKey);
            httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", _apiOptions.AppToken);

            var response = await httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}