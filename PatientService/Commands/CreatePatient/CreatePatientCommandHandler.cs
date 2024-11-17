using MediatR;
using PatientService.Models;
using PatientService.Repositories;

namespace PatientService.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IPatientRepository _repository;

        public CreatePatientCommandHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {
                PatientId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DOB = request.DOB,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address
            };

            await _repository.AddPatientAsync(patient);

            return patient.PatientId;
        }
    }
}
