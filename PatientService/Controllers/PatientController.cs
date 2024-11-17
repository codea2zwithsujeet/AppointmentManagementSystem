using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientService.Commands.CreatePatient;
using PatientService.Commands.DeletePatient;
using PatientService.Commands.UpdatePatient;
using PatientService.DTOs;
using PatientService.Queries.GetPatient;
using PatientService.Queries.GetPatients;
using System.Data;

namespace PatientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin1")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            var query = new GetPatientsQuery();
            var patients = await _mediator.Send(query);
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
        {
            var query = new GetPatientQuery { PatientId = id };
            var patient = await _mediator.Send(query);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePatient(CreatePatientCommand command)
        {
            var patientId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPatient), new { id = patientId }, patientId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, UpdatePatientDto dto)
        {
            var command = new UpdatePatientCommand
            {
                PatientId = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var command = new DeletePatientCommand { PatientId = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
