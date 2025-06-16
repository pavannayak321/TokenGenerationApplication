using Microsoft.AspNetCore.Http;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace TokenGenerationApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretsController : ControllerBase
    {
        private readonly SecretClient _secretClient;

        public SecretsController(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }





        [HttpPost("set")]
        public async Task<IActionResult> SetSecret(string key, string value)
        {
            KeyVaultSecret secret = await _secretClient.SetSecretAsync(key, value);
            return Ok(new { secret.Name, secret.Value });
        }




        [HttpGet("get")]
        public async Task<IActionResult> GetSecret(string key)
        {
            try
            {
                KeyVaultSecret secret = await _secretClient.GetSecretAsync(key);
                return Ok(new { secret.Name, secret.Value });
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                return NotFound($"Secret '{key}' not found.");
            }
        }
    }

}

