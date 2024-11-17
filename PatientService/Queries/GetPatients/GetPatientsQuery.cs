using MediatR;
using PatientService.DTOs;

namespace PatientService.Queries.GetPatients
{
    public class GetPatientsQuery : IRequest<IEnumerable<PatientDto>>
    {
    }
}
