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
    public class DossierFinanciersController : ControllerBase
    {
        private readonly CabinetContext _context;

        public DossierFinanciersController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/DossierFinanciers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DossierFinancier>>> GetDossierFinanciers()
        {
            return await _context.DossierFinanciers.ToListAsync();
        }

        // GET: api/DossierFinanciers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DossierFinancier>> GetDossierFinancier(long id)
        {
            var dossierFinancier = await _context.DossierFinanciers.FindAsync(id);

            if (dossierFinancier == null)
            {
                return NotFound();
            }

            return dossierFinancier;
        }

        // PUT: api/DossierFinanciers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDossierFinancier(long id, DossierFinancier dossierFinancier)
        {
            if (id != dossierFinancier.Id)
            {
                return BadRequest();
            }

            _context.Entry(dossierFinancier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DossierFinancierExists(id))
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

        // POST: api/DossierFinanciers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DossierFinancier>> PostDossierFinancier(DossierFinancier dossierFinancier)
        {
            _context.DossierFinanciers.Add(dossierFinancier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDossierFinancier", new { id = dossierFinancier.Id }, dossierFinancier);
        }

        // DELETE: api/DossierFinanciers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossierFinancier(long id)
        {
            var dossierFinancier = await _context.DossierFinanciers.FindAsync(id);
            if (dossierFinancier == null)
            {
                return NotFound();
            }

            _context.DossierFinanciers.Remove(dossierFinancier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DossierFinancierExists(long id)
        {
            return _context.DossierFinanciers.Any(e => e.Id == id);
        }
    }
}
