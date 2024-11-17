using MediatR;
using PatientService.DTOs;
using PatientService.Repositories;

namespace PatientService.Queries.GetPatient
{
    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, PatientDto>
    {
        private readonly IPatientRepository _repository;

        public GetPatientQueryHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientDto> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientByIdAsync(request.PatientId);
            if (patient == null)
            {
                return null; // Or handle it in another appropriate way
            }

            return new PatientDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DOB = patient.DOB,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address
            };
        }
    }
}
