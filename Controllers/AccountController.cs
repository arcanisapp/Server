using Microsoft.AspNetCore.Mvc;
using Server.Models.Dto.Account.Create;
using Server.Services;
using System.IO.Compression;
using System.Text;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                    return BadRequest("Missing X-Signature header");

                var signatureBytes = Convert.FromBase64String(signatureHeader);

                // Распаковка GZip → JSON
                string rawJson;
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    rawJson = await reader.ReadToEndAsync();
                }

                var result = await accountService.CreateAccountAsync(signatureBytes, rawJson);

                if (!result)
                    return BadRequest("Invalid signature or data");

                return Ok("Account created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid request: {ex.Message}");
            }
        }
    }
}
