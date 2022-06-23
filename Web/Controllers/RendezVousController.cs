using Microservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Diagnostics;
using Web.ControllersAPI;

namespace Web.Controllers
{
    public class RendezVousController : Controller
    {
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";
        // GET: RendezVous
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                var patientId = HttpContext.Session.GetString(SessionKeyId);
                return View(await RendezVousAPI.Instance.GetRendezVousPatientAsync(long.Parse(patientId!)));
            }
            return RedirectToAction("SignIn", "Patients");
        }

        public async Task<IActionResult> Search(string? spe, string? acte, int? pageNumber, string? currentSpe, string? currentActe)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                if (spe != null || acte != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    spe = currentSpe;
                    acte = currentActe;
                }

                ViewData["currentSpe"] = spe;
                ViewData["currentActe"] = acte;

                PlageHoraire plageHoraire = new();
                plageHoraire.spe = spe;
                plageHoraire.acte = acte;
                return View(await RendezVousAPI.Instance.Search(plageHoraire, pageNumber));
            }
            return RedirectToAction("SignIn", "Patients");
        }

        // GET: RendezVous/Details/5
        public async Task<IActionResult> Details(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                return View(await RendezVousAPI.Instance.GetRendezVousAsync(id));
            }
            return RedirectToAction("SignIn", "Patients");
        }

        // GET: RendezVous/Create
        public async Task<IActionResult> Create(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                return View(await PlageHoraireAPI.Instance.GetPlageHoraireAsync(id));
            }
            return RedirectToAction("SignIn", "Patients");
        }

        // POST: RendezVous/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlageHoraireId, PatientId")] RendezVous rendezVous)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                if (ModelState.IsValid)
                {
                    await RendezVousAPI.AddRendezVous(rendezVous);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            return RedirectToAction("SignIn", "Patients");
        }

        // GET: RendezVous/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                return View(await RendezVousAPI.Instance.GetRendezVousAsync(id));
            }
            return RedirectToAction("SignIn", "Patients");
        }

        // POST: RendezVous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_PATIENT")
            {
                await RendezVousAPI.DeleteRendezVous(id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("SignIn", "Patients");
        }
    }
}
