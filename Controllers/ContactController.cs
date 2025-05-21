using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController(IContactService contactService) : ControllerBase
    {
        [HttpPost("lookup")]
        public async Task<IActionResult> LookupUser()
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                    return BadRequest("Missing X-Signature header");

                var signatureBytes = Convert.FromBase64String(signatureHeader);

                using var ms = new MemoryStream();
                await Request.Body.CopyToAsync(ms);
                var rawData = ms.ToArray();

                var responce = await contactService.LookupUserRequestAsync(signatureBytes, rawData);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }

}
