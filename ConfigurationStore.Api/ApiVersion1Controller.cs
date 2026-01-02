using ConfigurationStore.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace ConfigurationStore.Api;

[Route("api/v1/")]
[ApiController]
public class ApiVersion1Controller : ControllerBase
{
    [Route("ping")]
    [HttpGet]
    [HttpPost]
    [ProducesResponseType(typeof(PingResponse), 200)]
    public async Task<IActionResult> Ping()
    {
        await Task.Yield();
        return Ok(new PingResponse());
    }
}