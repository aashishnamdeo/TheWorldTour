using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldTour.Controllers.Web.Services
{
    public class GeoCordService
    {
        private ILogger<GeoCordService> _logger;
        private IConfigurationRoot _config;

        public GeoCordService(ILogger<GeoCordService> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCordResult> GetGeoCord(string name)
        {
            var result = new GeoCordResult()
            {
                Success = false,
                Message = "Failed to get the Geo Data"
            };

            var apiKey = _config["Keys:BingKey"];
            var encodedName = WebUtility.UrlEncode(name);

            var url = $"http://dev.virtualearth.net/rest/v1/locations?q={encodedName}&key={apiKey}";

            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;
        }
    }
}
