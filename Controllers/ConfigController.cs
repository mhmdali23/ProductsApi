using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAppApi.Options;

namespace WebAppApi.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IOptionsMonitor<AttachmentOptions> attachmentOptions;

        public ConfigController(IConfiguration configuration , IOptionsMonitor<AttachmentOptions> attachmentOptions)
        {
            this.configuration = configuration;
            this.attachmentOptions = attachmentOptions;
            var val = attachmentOptions.CurrentValue;
        }


        [HttpGet]
        [Route("")]
        public IActionResult GetConfig()
        {
            Thread.Sleep(10000);

            var config = new
            {

                AllowedHosts = configuration["AllowedHosts"],
                SigningKey = configuration["SigningKey"],
                attachmentOptions = attachmentOptions.CurrentValue
            };

            return Ok(config);
        }
    }
}
