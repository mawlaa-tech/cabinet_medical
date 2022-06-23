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

namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly CabinetContext _context;

        public RendezVousController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/RendezVous/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<RendezVous>>> GetRendezVousPatient(long PatientId)
        {
            return await _context.RendezVous.Include(r => r.PlageHoraire).Include(r => r.PlageHoraire.ActeMedical).Include(r => r.PlageHoraire.Specialite).Include(r => r.PlageHoraire.Medecin).Where(r => r.PatientId == PatientId).ToListAsync();
        }

        // GET: api/RendezVous/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RendezVous>> GetRendezVous(long id)
        {
            var rendezVous = await _context.RendezVous.Include(r => r.PlageHoraire).Include(r => r.PlageHoraire.ActeMedical).Include(r => r.PlageHoraire.Specialite).Include(r => r.PlageHoraire.Medecin).FirstOrDefaultAsync(r => r.Id == id);

            if (rendezVous == null)
            {
                return NotFound();
            }

            return rendezVous;
        }

        // PUT: api/RendezVous/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRendezVous(long id, RendezVous rendezVous)
        {
            if (id != rendezVous.Id)
            {
                return BadRequest();
            }

            _context.Entry(rendezVous).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RendezVousExists(id))
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

        // POST: api/RendezVous
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RendezVous>> PostRendezVous(RendezVous rendezVous)
        {
            _context.RendezVous.Add(rendezVous);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRendezVous", new { id = rendezVous.Id }, rendezVous);
        }

        // DELETE: api/RendezVous/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRendezVous(long id)
        {
            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous == null)
            {
                return NotFound();
            }

            _context.RendezVous.Remove(rendezVous);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RendezVousExists(long id)
        {
            return _context.RendezVous.Any(e => e.Id == id);
        }
    }
}
