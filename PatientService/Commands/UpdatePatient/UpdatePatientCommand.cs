﻿using MediatR;

namespace PatientService.Commands.UpdatePatient
{
    public class UpdatePatientCommand : IRequest<Unit>
    {
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
