using MediatR;
using PatientService.DTOs;
using PatientService.Repositories;

namespace PatientService.Queries.GetPatients
{
    public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, IEnumerable<PatientDto>>
    {
        private readonly IPatientRepository _repository;

        public GetPatientsQueryHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PatientDto>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _repository.GetAllPatientsAsync();

            return patients.Select(patient => new PatientDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DOB = patient.DOB,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address
            }).ToList();
        }
    }
}
