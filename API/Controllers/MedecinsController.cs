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
    public class MedecinsController : ControllerBase
    {
        private readonly CabinetContext _context;

        public MedecinsController(CabinetContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medecin>>> GetMedecins()
        {
            return await _context.Medecins.Include(m => m.Specialites).Include(m => m.PlageHoraires).ToListAsync();
        }

        // GET: api/Medecins/5/specialites
        [HttpGet("{medecinId}/specialites")]
        public async Task<ActionResult<Medecin>> GetMedecinAndSpecialites(long medecinId)
        {
            return await _context.Medecins.Include(m => m.Specialites).FirstOrDefaultAsync(m => m.Id == medecinId);
        }

        // GET: api/Medecins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medecin>> GetMedecin(long id)
        {
            var medecin = await _context.Medecins.Include(m => m.Specialites).Include(m => m.PlageHoraires).FirstOrDefaultAsync(m => m.Id == id);

            if (medecin == null)
            {
                return NotFound();
            }

            return medecin;
        }

        // PUT: api/Medecins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedecin(long id, Medecin medecin)
        {
            if (id != medecin.Id)
            {
                return BadRequest();
            }

            var Medecin = _context.Medecins.AsNoTracking().First(p => p.Id == id);

            _context.Entry(medecin).State = EntityState.Modified;

            if (!String.IsNullOrWhiteSpace(medecin.NewPassword))
            {
                String salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                medecin.Password = BCrypt.Net.BCrypt.HashPassword(medecin.NewPassword, salt);
            }
            else
            {
                medecin.Password = Medecin.Password;
            }

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM medecin_specialite WHERE medecin_id = {0}", medecin.Id);
            medecin.Specialites.Clear();

            foreach (long specialiteId in medecin.SpecialitesId)
            {
                medecin.Specialites.Add(_context.Specialites.FirstOrDefault(s => s.Id == specialiteId));
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedecinExists(id))
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

        // POST: api/Medecins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Medecin>> PostMedecin(Medecin medecin)
        {
            String salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            medecin.Password = BCrypt.Net.BCrypt.HashPassword(medecin.Password, salt);
            foreach (long specialiteId in medecin.SpecialitesId)
            {
                medecin.Specialites.Add(_context.Specialites.FirstOrDefault(s => s.Id == specialiteId));
            }
            _context.Medecins.Add(medecin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedecin", new { id = medecin.Id }, medecin);
        }

        // POST: api/Medecins/login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<Medecin>> Login(Medecin user)
        {
            var medecin = await _context.Medecins.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (medecin == null)
            {
                return NotFound();
            }

            Debug.WriteLine("Email ok");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, medecin.Password))
            {
                return NotFound();
            }

            Debug.WriteLine("Password ok");

            return medecin;
        }

        // DELETE: api/Medecins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedecin(long id)
        {
            var medecin = await _context.Medecins.Include(m => m.Specialites).FirstOrDefaultAsync(m => m.Id == id);
            if (medecin == null)
            {
                return NotFound();
            }

            medecin.Specialites.Clear();

            _context.Medecins.Remove(medecin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedecinExists(long id)
        {
            return _context.Medecins.Any(e => e.Id == id);
        }
    }
}
