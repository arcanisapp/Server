using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Server.Crypto;
using Server.Services;
using Server.Services.Validation;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDeviceService deviceService) : ControllerBase
    {
        [HttpGet("add")]
        [EnableRateLimiting("Registration")]
        public async Task<IActionResult> AddDevice()
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                    return BadRequest("Missing X-Signature header");

                var signatureBytes = Convert.FromBase64String(signatureHeader);

                string rawJson;
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    rawJson = await reader.ReadToEndAsync();
                }
                var result = await deviceService.AddDeviceRequestAsync(signatureBytes, rawJson);

                if (!result)
                    return BadRequest("Invalid signature or data");

                return Ok();
            }   
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
