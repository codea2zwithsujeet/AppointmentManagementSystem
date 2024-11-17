using MediatR;
using PatientService.Repositories;

namespace PatientService.Commands.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Unit>
    {
        private readonly IPatientRepository _repository;

        public UpdatePatientCommandHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientByIdAsync(request.PatientId);
            if (patient == null)
            {
                // Handle the case when the patient is not found, possibly throw an exception
                return Unit.Value;
            }

            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.DOB = request.DOB;
            patient.Email = request.Email;
            patient.Phone = request.Phone;
            patient.Address = request.Address;

            await _repository.UpdatePatientAsync(patient);

            return Unit.Value;
        }
      
    }
}
