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

        // GET: api/PlageHoraires/search?page=1&spe=chirurgie&acte=cataracte
        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<PlageHoraire>>> Search([FromQuery] int? page, [FromQuery] string spe, [FromQuery] string acte)
        {
            Debug.WriteLine(page);
            Debug.WriteLine(spe);
            Debug.WriteLine(acte);
            if (String.IsNullOrWhiteSpace(spe))
            {
                spe = "";
            }
            if (String.IsNullOrWhiteSpace(acte))
            {
                acte = "";
            }
            int pageSize = 10;
            var count = await _context.PlageHoraires.CountAsync();
            page ??= 1;
            var plageHoraires = await _context.PlageHoraires
                .Include(p => p.Specialite).Where(p => p.Specialite.Nom.Contains(spe))
                .Include(p => p.ActeMedical).Where(p => p.ActeMedical.Libelle.Contains(acte))
                .Include(p => p.RendezVous)
                .Include(p => p.Medecin).Skip((page.Value - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<PlageHoraire>(plageHoraires, count, page ?? 1, pageSize);
        }
    }
}
