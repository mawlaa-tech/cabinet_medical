#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microservice.DbContexts;
using Microservice.Models;
using System.Diagnostics;

namespace Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlageHorairesController : ControllerBase
    {
        private readonly CabinetContext _context;

        public PlageHorairesController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/PlageHoraires/medecin/5
        [HttpGet("medecin/{id}")]
        public async Task<ActionResult<IEnumerable<PlageHoraire>>> GetPlageHoraires(long id)
        {
            return await _context.PlageHoraires.Include(p => p.ActeMedical).Include(p => p.Specialite).Where(p => p.MedecinId == id).ToListAsync();
        }

        // GET: api/PlageHoraires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlageHoraire>> GetPlageHoraire(long id)
        {
            var plageHoraire = await _context.PlageHoraires.Include(p => p.ActeMedical).Include(p => p.Specialite).Include(p => p.Medecin).FirstOrDefaultAsync(p => p.Id == id);

            if (plageHoraire == null)
            {
                return NotFound();
            }

            return plageHoraire;
        }

        // POST: api/PlageHoraires/check/date
        [HttpPost("check/date")]
        public async Task<ActionResult<PlageHoraire>> CheckDateOccupation(PlageHoraire PlageHoraire)
        {
            var plageHoraire = await _context.PlageHoraires.FirstOrDefaultAsync(p => p.MedecinId == PlageHoraire.MedecinId && p.Date == PlageHoraire.Date);

            if (plageHoraire == null)
            {
                return NotFound();
            }

            return plageHoraire;
        }

        // POST: api/PlageHoraires/check/time
        [HttpPost("check/time")]
        public async Task<ActionResult<PlageHoraire>> CheckTimeOccupation(PlageHoraire PlageHoraire)
        {
            var plageHoraire = await _context.PlageHoraires.FirstOrDefaultAsync(p => p.MedecinId == PlageHoraire.MedecinId && p.Date == PlageHoraire.Date && p.HeureDebut > PlageHoraire.HeureDebut && p.HeureDebut < PlageHoraire.HeureTer);

            Debug.WriteLine(ObjectDumper.Dump(plageHoraire));

            if (plageHoraire != null)
            {
                return plageHoraire;
            }

            Debug.WriteLine(ObjectDumper.Dump(plageHoraire));

            plageHoraire = await _context.PlageHoraires.FirstOrDefaultAsync(p => p.MedecinId == PlageHoraire.MedecinId && p.Date == PlageHoraire.Date && p.HeureTer > PlageHoraire.HeureDebut && p.HeureTer < PlageHoraire.HeureTer);

            if (plageHoraire != null)
            {
                return plageHoraire;
            }

            Debug.WriteLine(ObjectDumper.Dump(plageHoraire));

            plageHoraire = await _context.PlageHoraires.FirstOrDefaultAsync(p => p.MedecinId == PlageHoraire.MedecinId && p.Date == PlageHoraire.Date && p.HeureDebut < PlageHoraire.HeureDebut && p.HeureTer > PlageHoraire.HeureTer);

            if (plageHoraire != null)
            {
                return plageHoraire;
            }

            return NotFound();
        }

        // PUT: api/PlageHoraires/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlageHoraire(long id, PlageHoraire plageHoraire)
        {
            if (id != plageHoraire.Id)
            {
                return BadRequest();
            }

            _context.Entry(plageHoraire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlageHoraireExists(id))
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

        // POST: api/PlageHoraires
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlageHoraire>> PostPlageHoraire(PlageHoraire plageHoraire)
        {
            PlageHoraire plageHoraireFinal;
            using var transaction = _context.Database.BeginTransaction();

            if (!plageHoraire.DebutPause.HasValue || !plageHoraire.FinPause.HasValue)
            {
                for (TimeSpan? time = plageHoraire.HeureDebut; time < plageHoraire.HeureTer; time += plageHoraire.Duree.Value)
                {
                    plageHoraireFinal = new();

                    plageHoraireFinal.Date = plageHoraire.Date;

                    plageHoraireFinal.HeureDebut = time.Value;

                    plageHoraireFinal.HeureTer = time.Value + plageHoraire.Duree.Value;

                    if (plageHoraireFinal.HeureTer > plageHoraire.HeureTer)
                    {
                        plageHoraireFinal.HeureTer = plageHoraire.HeureTer;
                    }

                    plageHoraireFinal.ActeMedicalId = plageHoraire.ActeMedicalId;
                    plageHoraireFinal.SpecialiteId = plageHoraire.SpecialiteId;
                    plageHoraireFinal.MedecinId = plageHoraire.MedecinId;

                    _context.PlageHoraires.Add(plageHoraireFinal);

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                for (TimeSpan? time = plageHoraire.HeureDebut; time < plageHoraire.DebutPause; time += plageHoraire.Duree.Value)
                {
                    plageHoraireFinal = new();

                    plageHoraireFinal.Date = plageHoraire.Date;

                    plageHoraireFinal.HeureDebut = time.Value;

                    plageHoraireFinal.HeureTer = time.Value + plageHoraire.Duree.Value;

                    if (plageHoraireFinal.HeureTer > plageHoraire.DebutPause)
                    {
                        plageHoraireFinal.HeureTer = plageHoraire.DebutPause.Value;
                    }

                    plageHoraireFinal.ActeMedicalId = plageHoraire.ActeMedicalId;
                    plageHoraireFinal.SpecialiteId = plageHoraire.SpecialiteId;
                    plageHoraireFinal.MedecinId = plageHoraire.MedecinId;

                    _context.PlageHoraires.Add(plageHoraireFinal);

                    await _context.SaveChangesAsync();
                }

                for (TimeSpan? time = plageHoraire.FinPause; time < plageHoraire.HeureTer; time += plageHoraire.Duree.Value)
                {
                    plageHoraireFinal = new();

                    plageHoraireFinal.Date = plageHoraire.Date;

                    plageHoraireFinal.HeureDebut = time.Value;

                    plageHoraireFinal.HeureTer = time.Value + plageHoraire.Duree.Value;

                    if (plageHoraireFinal.HeureTer > plageHoraire.HeureTer)
                    {
                        plageHoraireFinal.HeureTer = plageHoraire.HeureTer;
                    }

                    plageHoraireFinal.ActeMedicalId = plageHoraire.ActeMedicalId;
                    plageHoraireFinal.SpecialiteId = plageHoraire.SpecialiteId;
                    plageHoraireFinal.MedecinId = plageHoraire.MedecinId;

                    _context.PlageHoraires.Add(plageHoraireFinal);

                    await _context.SaveChangesAsync();
                }
            }

            transaction.Commit();

            return NoContent();
        }

        // DELETE: api/PlageHoraires/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlageHoraire(long id)
        {
            var plageHoraire = await _context.PlageHoraires.FindAsync(id);
            if (plageHoraire == null)
            {
                return NotFound();
            }

            _context.PlageHoraires.Remove(plageHoraire);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlageHoraireExists(long id)
        {
            return _context.PlageHoraires.Any(e => e.Id == id);
        }
    }
}
