using MediatR;
using PatientService.DTOs;

namespace PatientService.Queries.GetPatient
{
    public class GetPatientQuery : IRequest<PatientDto>
    {
        public Guid PatientId { get; set; }
    }
}
