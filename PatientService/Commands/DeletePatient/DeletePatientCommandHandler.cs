using MediatR;
using PatientService.Repositories;

namespace PatientService.Commands.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Unit>
    {
        private readonly IPatientRepository _repository;

        public DeletePatientCommandHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientByIdAsync(request.PatientId);
            if (patient == null)
            {
                // Handle the case when the patient is not found, possibly throw an exception
                return Unit.Value;
            }

            await _repository.DeletePatientAsync(request.PatientId);

            return Unit.Value;
        }
    }
}
