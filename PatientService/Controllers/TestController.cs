using Microsoft.AspNetCore.Mvc;
using PatientService.Services;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly CircuitBreakerService _circuitBreakerService;

    public TestController(CircuitBreakerService circuitBreakerService)
    {
        _circuitBreakerService = circuitBreakerService;
    }

    [HttpGet("test-circuit-breaker")]
    public async Task<IActionResult> TestCircuitBreaker()
    {
        var response = await _circuitBreakerService.GetResponseWithCircuitBreaker();
        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
    }
}
