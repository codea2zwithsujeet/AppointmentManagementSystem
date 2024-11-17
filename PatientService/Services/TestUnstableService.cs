namespace PatientService.Services
{
    public class TestUnstableService
    {
        public async Task<HttpResponseMessage> GetUnstableResponse()
        {
            // Simulate an unstable service by randomly failing
            if (new Random().Next(2) == 0) // 50% chance to fail
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
