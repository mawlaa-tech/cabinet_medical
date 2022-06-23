#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microservice.DbContexts;
using Microservice.Models;
using System.Diagnostics;

namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly CabinetContext _context;

        public PatientsController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(long id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(long id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            var Patient = _context.Patients.AsNoTracking().First(p => p.Id == id);

            _context.Entry(patient).State = EntityState.Modified;

            if (!String.IsNullOrWhiteSpace(patient.NewPassword))
            {
                String salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                patient.Password = BCrypt.Net.BCrypt.HashPassword(patient.NewPassword, salt);
            }
            else
            {
                patient.Password = Patient.Password;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            String salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            patient.Password = BCrypt.Net.BCrypt.HashPassword(patient.Password, salt);
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        // POST: api/Patients/login
        [HttpPost("login")]
        public async Task<ActionResult<Patient>> Login(Patient user)
        {
            Debug.WriteLine(ObjectDumper.Dump(user));
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == user.Email);
            Debug.WriteLine(ObjectDumper.Dump(patient));

            if (patient == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, patient.Password))
            {
                return NotFound();
            }
            Debug.WriteLine("Password ok");

            return patient;
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
