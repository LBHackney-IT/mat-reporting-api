using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MaTReportingAPI.Controllers.V1
{
    //TODO: should derive from ControllerBase since it's just Web API?
    public class BaseController  : Controller
    {
        public BaseController()
        {
            ConfigureJsonSerializer();
        }

        public static void ConfigureJsonSerializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

                return settings;
            };
        }
    }
}
