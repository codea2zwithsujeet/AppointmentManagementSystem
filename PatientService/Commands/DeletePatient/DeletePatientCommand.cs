using MediatR;

namespace PatientService.Commands.DeletePatient
{
    public class DeletePatientCommand : IRequest<Unit>
    {
        public Guid PatientId { get; set; }
    }
}
