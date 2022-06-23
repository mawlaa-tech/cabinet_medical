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
    public class MedecinsController : ControllerBase
    {
        private readonly CabinetContext _context;

        public MedecinsController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/Medecins/search/searchString
        [HttpGet("search/{searchString}")]
        public async Task<ActionResult<IEnumerable<Medecin>>> GetMedecins(string searchString)
        {
            return await _context.Medecins.Include(m => m.Specialites).Include(m => m.PlageHoraires).Where(s => s.PrenomMedecin.Contains(searchString) ||
                s.NomMedecin.Contains(searchString) ||
                s.Email!.Contains(searchString)).ToListAsync();
        }
    }
}
