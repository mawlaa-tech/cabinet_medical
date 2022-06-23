#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microservice.Models;
using Web.ControllersAPI;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Web.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ILogger<PatientsController> _logger;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";

        public PatientsController(ILogger<PatientsController> logger)
        {
            _logger = logger;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                return View(await PatientAPI.Instance.GetPatientsAsync());
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                return View(await PatientAPI.Instance.GetPatientAsync(id));
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                ViewData["Sexes"] = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Homme", Value = "Homme"},
                    new SelectListItem { Text = "Femme", Value = "Femme"},
                }, "Value", "Text");
                return View();
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: Patients/SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Patients/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("Email,Password")] Patient patient)
        {
            bool isValid = true;
            if (String.IsNullOrWhiteSpace(patient.Password))
            {
                ModelState.AddModelError("Password", "Mot de passe requis");
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(patient.Email))
            {
                ModelState.AddModelError("Email", "Adresse mail requise");
                isValid = false;
            }

            var emailValidation = new EmailAddressAttribute();
            if (!emailValidation.IsValid(patient.Email))
            {
                ModelState.AddModelError("Email", "Format d'adresse mail invalide");
                isValid = false;
            }

            if (isValid)
            {
                var user = await PatientAPI.Instance.Login(patient);
                if (user != null)
                {
                    HttpContext.Session.SetString(SessionKeyId, (user.Id.ToString()));
                    HttpContext.Session.SetString(SessionKeyRole, user.Role);
                    return RedirectToAction("Index", "RendezVous");
                }
                else
                {
                    ModelState.AddModelError("Password", "Adresse mail ou mot de passe incorrect");
                    ModelState.AddModelError("Email", "Adresse mail ou mot de passe incorrect");
                }
            }

            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatNaisPat,NomPat,PrenPat,SexPat,Email,Password,PhonePat")] Patient patient)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                if (String.IsNullOrWhiteSpace(patient.Password))
                {
                    ModelState.AddModelError("Password", "Mot de passe requis");
                }

                if (ModelState.IsValid)
                {
                    await PatientAPI.AddPatientAsync(patient);
                    return RedirectToAction(nameof(Index));
                }

                ViewData["Sexes"] = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Homme", Value = "Homme"},
                    new SelectListItem { Text = "Femme", Value = "Femme"},
                }, "Value", "Text");
                return View(patient);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                Patient patient = await PatientAPI.Instance.GetPatientAsync(id);
                var selectList = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Homme", Value = "Homme"},
                    new SelectListItem { Text = "Femme", Value = "Femme"},
                }, "Value", "Text", patient.SexPat);
                foreach (SelectListItem item in selectList.Items)
                {
                    if (item.Value == patient.SexPat)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                ViewData["Sexes"] = selectList;
                return View(patient);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DatNaisPat,Email,NomPat,NewPassword,PhonePat,PrenPat,Role,SexPat")] Patient patient)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                if (id != patient.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await PatientAPI.UpdatePatientAsync(patient);
                    return RedirectToAction(nameof(Index));
                }

                var selectList = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Homme", Value = "Homme"},
                    new SelectListItem { Text = "Femme", Value = "Femme"},
                }, "Value", "Text", patient.SexPat);
                foreach (SelectListItem item in selectList.Items)
                {
                    if (item.Value == patient.SexPat)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                ViewData["Sexes"] = selectList;
                return View(patient);
            }
            return RedirectToAction("SignIn", "Medecins");
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_MEDECIN")
            {
                return View(await PatientAPI.Instance.GetPatientAsync(id));
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
                await PatientAPI.DeletePatientAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("signin", "medecins");
        }
    }
 
}
