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
    public class ActeMedicauxController : ControllerBase
    {
        private readonly CabinetContext _context;

        public ActeMedicauxController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/ActeMedicaux
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActeMedical>>> GetActeMedicaux()
        {
            return await _context.ActeMedicaux.Include(a => a.Specialite).ToListAsync();
        }

        // GET: api/ActeMedicaux/specialite/5
        [HttpGet("specialite/{specialiteId}")]
        public async Task<ActionResult<IEnumerable<ActeMedical>>> GetActeMedicaux(long specialiteId)
        {
            return await _context.ActeMedicaux.Where(a => a.SpecialiteId == specialiteId).ToListAsync();
        }

        // GET: api/ActeMedicaux/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActeMedical>> GetActeMedical(long id)
        {
            var acteMedical = await _context.ActeMedicaux.Include(a => a.Specialite).FirstOrDefaultAsync(a => a.Id == id);

            if (acteMedical == null)
            {
                return NotFound();
            }

            return acteMedical;
        }

        // PUT: api/ActeMedicaux/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActeMedical(long id, ActeMedical acteMedical)
        {
            if (id != acteMedical.Id)
            {
                return BadRequest();
            }

            _context.Entry(acteMedical).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActeMedicalExists(id))
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

        // POST: api/ActeMedicaux
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActeMedical>> PostActeMedical(ActeMedical acteMedical)
        {
            _context.ActeMedicaux.Add(acteMedical);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActeMedical", new { id = acteMedical.Id }, acteMedical);
        }

        // DELETE: api/ActeMedicaux/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActeMedical(long id)
        {
            var acteMedical = await _context.ActeMedicaux.FindAsync(id);
            if (acteMedical == null)
            {
                return NotFound();
            }

            _context.ActeMedicaux.Remove(acteMedical);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActeMedicalExists(long id)
        {
            return _context.ActeMedicaux.Any(e => e.Id == id);
        }
    }
}
