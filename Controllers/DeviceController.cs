using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDeviceService deviceService) : ControllerBase
    {
        [HttpPost("add")]
        [EnableRateLimiting("AddDevice")]
        public async Task<IActionResult> Add()
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                    return BadRequest("Missing X-Signature header");

                var signatureBytes = Convert.FromBase64String(signatureHeader);

                using var ms = new MemoryStream();
                await Request.Body.CopyToAsync(ms);
                var rawData = ms.ToArray();

                var result = await deviceService.AddDeviceRequestAsync(signatureBytes, rawData);

                if (!result)
                    return BadRequest("Invalid signature or data");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("confirm")]
        [EnableRateLimiting("AddDevice")]
        public async Task<IActionResult> Confirm()
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                    return BadRequest("Missing X-Signature header");

                var signatureBytes = Convert.FromBase64String(signatureHeader);

                using var ms = new MemoryStream();
                await Request.Body.CopyToAsync(ms);
                var rawData = ms.ToArray();

                var result = await deviceService.ConfirmDeviceRequestAsync(signatureBytes, rawData);

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
