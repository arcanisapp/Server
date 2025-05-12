using Microsoft.AspNetCore.Mvc;
using Server.Models.Dto.Account.Create;
using Server.Services;
using System.Text;

namespace Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Registration([FromBody] CreateAccountRequest request)
        {
            if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
                return BadRequest("Missing signature header");

            byte[] signatureBytes;
            try
            {
                signatureBytes = Convert.FromBase64String(signatureHeader!);
            }
            catch
            {
                return BadRequest("Invalid signature encoding");
            }
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            string rawJson = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var result = await accountService.CreateAccountAsync(signatureBytes, rawJson);

            return Ok("Account registered successfully.");
        }
    }
}
