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
    public class SpecialitesController : ControllerBase
    {
        private readonly CabinetContext _context;

        public SpecialitesController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/Specialites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialite>>> GetSpecialites()
        {
            return await _context.Specialites.Include(s => s.Medecins).Include(s => s.ActeMedicaux).ToListAsync();
        }

        // GET: api/Specialites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialite>> GetSpecialite(long id)
        {
            var specialite = await _context.Specialites.Include(s => s.Medecins).Include(s => s.ActeMedicaux).FirstOrDefaultAsync(s => s.Id == id);

            if (specialite == null)
            {
                return NotFound();
            }

            return specialite;
        }

        // PUT: api/Specialites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialite(long id, Specialite specialite)
        {
            if (id != specialite.Id)
            {
                return BadRequest();
            }

            _context.Entry(specialite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialiteExists(id))
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

        // POST: api/Specialites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Specialite>> PostSpecialite(Specialite specialite)
        {
            _context.Specialites.Add(specialite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecialite", new { id = specialite.Id }, specialite);
        }

        // DELETE: api/Specialites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialite(long id)
        {
            var specialite = await _context.Specialites.FindAsync(id);
            if (specialite == null)
            {
                return NotFound();
            }

            _context.Specialites.Remove(specialite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecialiteExists(long id)
        {
            return _context.Specialites.Any(e => e.Id == id);
        }
    }
}
