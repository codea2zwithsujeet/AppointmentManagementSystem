using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientService.Services
{
    public class CircuitBreakerService
    {
        private readonly TestUnstableService _testUnstableService;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy =
            Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode) // Define the condition that causes the circuit to break
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3, // Break the circuit after 3 consecutive failures
                    durationOfBreak: TimeSpan.FromSeconds(30), // Keep the circuit open for 30 seconds before transitioning to half-open
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit broken due to {outcome.Result.StatusCode}, breaking for {breakDelay.TotalSeconds} seconds");
                    },
                    onReset: () => Console.WriteLine("Circuit reset"), // Invoked when the circuit transitions back to closed
                    onHalfOpen: () => Console.WriteLine("Circuit is half-open") // Invoked when the circuit transitions to half-open
                );

        public CircuitBreakerService(TestUnstableService testUnstableService)
        {
            _testUnstableService = testUnstableService;
        }

        public async Task<HttpResponseMessage> GetResponseWithCircuitBreaker()
        {
            // Execute the HTTP request within the circuit breaker policy
            return await _circuitBreakerPolicy.ExecuteAsync(() => _testUnstableService.GetUnstableResponse());
        }
    }
}
