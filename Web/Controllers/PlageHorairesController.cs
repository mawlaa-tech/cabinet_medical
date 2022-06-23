using Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Web.ControllersAPI;
using Web.Models;

namespace Web.Controllers
{
    public class PlageHorairesController : Controller
    {
        private readonly ILogger<PlageHorairesController> _logger;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";

        public PlageHorairesController(ILogger<PlageHorairesController> logger)
        {
            _logger = logger;
        }

        // GET: PlageHoraires
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                var medecinId = HttpContext.Session.GetString(SessionKeyId);
                return View(await PlageHoraireAPI.Instance.GetPlageHorairesAsync(long.Parse(medecinId!)));
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: PlageHoraires/Details/5
        public async Task<IActionResult> Details(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {

                return View(await PlageHoraireAPI.Instance.GetPlageHoraireAsync(id));
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: PlageHoraires?specialiteId=5
        public async Task<ContentResult> GetActeMedicaux(long specialiteId)
        {
            Debug.WriteLine(specialiteId);
            var acteMedicaux = await PlageHoraireAPI.Instance.GetActeMedicauxAsync(specialiteId);
            return Content(acteMedicaux!, "application/json");
        }


        // GET: PlageHoraires/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                var medecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId)!);
                Debug.WriteLine(medecinId);
                ViewData["Specialites"] = new SelectList((await PlageHoraireAPI.Instance.GetSpecialitesAsync(medecinId))!.Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text"); ;
                return View();
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // POST: PlagesHoraires/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,HeureDebut,HeureTer,Duree,DebutPause,FinPause,ActeMedicalId,SpecialiteId")] PlageHoraire plageHoraire)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                plageHoraire.MedecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId)!);
                var medecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId)!);
                bool isValid = true;
                if (plageHoraire.Duree == TimeSpan.Zero)
                {
                    ModelState.AddModelError("Duree", "Duree requise");
                    isValid = false;
                }
                plageHoraire.Duree = plageHoraire.Duree!.Value.Divide(1440);
                if (isValid && ModelState.IsValid)
                {
                    if ((await PlageHoraireAPI.CheckDateAsync(plageHoraire)) != null)
                    {
                        ModelState.AddModelError("Date", "Date occupee");
                    }
                    else
                    {
                        await PlageHoraireAPI.AddPlageHoraire(plageHoraire);
                        return RedirectToAction(nameof(Index));
                    }

                }
                ViewData["Specialites"] = new SelectList(await PlageHoraireAPI.Instance.GetSpecialitesAsync(medecinId));
                return View(plageHoraire);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: PlageHoraires/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                var medecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId)!);
                var plageHoraire = await PlageHoraireAPI.Instance.GetPlageHoraireAsync(id);
                ViewData["Specialites"] = new SelectList((await PlageHoraireAPI.Instance.GetSpecialitesAsync(medecinId))!.Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");
                ViewData["ActeMedicaux"] = new SelectList((await PlageHoraireAPI.Instance.GetActeMedicauxAsyncNoJson(plageHoraire!.SpecialiteId))!.Select(a => new SelectListItem { Text = a.Libelle, Value = a.Id.ToString() }), "Value", "Text");
                return View(plageHoraire);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // POST: PlageHoraires/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Date,HeureDebut,HeureTer,ActeMedicalId,MedecinId,SpecialiteId")] PlageHoraire plageHoraire)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                plageHoraire.MedecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId));
                var medecinId = long.Parse(HttpContext.Session.GetString(SessionKeyId));
                if (ModelState.IsValid)
                {
                    if ((await PlageHoraireAPI.CheckTimeAsync(plageHoraire)) != null)
                    {
                        ModelState.AddModelError("HeureDebut", "Heures occupee partiellement ou entierement");
                        ModelState.AddModelError("HeureTer", "Heures occupee partiellement ou entierement");
                    }
                    else
                    {
                        await PlageHoraireAPI.UpdatePlageHoraire(plageHoraire);
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["Specialites"] = new SelectList((await PlageHoraireAPI.Instance.GetSpecialitesAsync(medecinId))!.Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");
                ViewData["ActeMedicaux"] = new SelectList((await PlageHoraireAPI.Instance.GetActeMedicauxAsyncNoJson(plageHoraire!.SpecialiteId))!.Select(a => new SelectListItem { Text = a.Libelle, Value = a.Id.ToString() }), "Value", "Text"); return View(plageHoraire);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // DELETE: PlageHoraires/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                return View(await PlageHoraireAPI.Instance.GetPlageHoraireAsync(id));
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                await PlageHoraireAPI.DeletePlageHoraire(id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}