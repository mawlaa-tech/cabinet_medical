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
    public class DossierMedicauxController : ControllerBase
    {
        private readonly CabinetContext _context;

        public DossierMedicauxController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/DossierMedicaux
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DossierMedical>>> GetDossierMedicaux()
        {
            return await _context.DossierMedicaux.ToListAsync();
        }

        // GET: api/DossierMedicaux/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DossierMedical>> GetDossierMedical(long id)
        {
            var dossierMedical = await _context.DossierMedicaux.FindAsync(id);

            if (dossierMedical == null)
            {
                return NotFound();
            }

            return dossierMedical;
        }

        // PUT: api/DossierMedicaux/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDossierMedical(long id, DossierMedical dossierMedical)
        {
            if (id != dossierMedical.Id)
            {
                return BadRequest();
            }

            _context.Entry(dossierMedical).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DossierMedicalExists(id))
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

        // POST: api/DossierMedicaux
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DossierMedical>> PostDossierMedical(DossierMedical dossierMedical)
        {
            _context.DossierMedicaux.Add(dossierMedical);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDossierMedical", new { id = dossierMedical.Id }, dossierMedical);
        }

        // DELETE: api/DossierMedicaux/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossierMedical(long id)
        {
            var dossierMedical = await _context.DossierMedicaux.FindAsync(id);
            if (dossierMedical == null)
            {
                return NotFound();
            }

            _context.DossierMedicaux.Remove(dossierMedical);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DossierMedicalExists(long id)
        {
            return _context.DossierMedicaux.Any(e => e.Id == id);
        }
    }
}
