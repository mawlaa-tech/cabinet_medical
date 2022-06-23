#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microservice.DbContexts;
using Microservice.Models;
using Web.ControllersAPI;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Web.Controllers
{
    public class MedecinsController : Controller
    {
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";
        // GET: Medecins
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            Debug.WriteLine(searchString);
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                return View(await MedecinAPI.Instance.GetMedecinsAsync(searchString));
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // GET: Medecins/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                return View(await MedecinAPI.Instance.GetMedecinAsync(id));
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // GET: Medecins/SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Medecins/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("Email,Password")] Medecin medecin)
        {
            bool isValid = true;
            if (String.IsNullOrWhiteSpace(medecin.Password))
            {
                ModelState.AddModelError("Password", "Mot de passe requis");
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(medecin.Email))
            {
                ModelState.AddModelError("Email", "Adresse mail requise");
                isValid = false;
            }

            var emailValidation = new EmailAddressAttribute();
            if (!emailValidation.IsValid(medecin.Email))
            {
                ModelState.AddModelError("Email", "Format d'adresse mail invalide");
                isValid = false;
            }

            if (isValid)
            {
                var user = await MedecinAPI.Instance.Login(medecin);
                if (user != null)
                {
                    HttpContext.Session.SetString(SessionKeyId, (user.Id.ToString()));
                    Debug.WriteLine(HttpContext.Session.GetString(SessionKeyId));
                    HttpContext.Session.SetString(SessionKeyRole, user.Role);
                    return RedirectToAction("Index", "PlageHoraires");
                }
                else
                {
                    ModelState.AddModelError("Password", "Adresse mail ou mot de passe incorrect");
                    ModelState.AddModelError("Email", "Adresse mail ou mot de passe incorrect");
                }
            }

            return View();
        }

        // GET: Medecins/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                ViewData["Specialites"] = new SelectList((await MedecinAPI.Instance.GetSpecialitesAsync()).Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");
                return View();
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // POST: Medecins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,NomMedecin,Password,PrenomMedecin,Role,SpecialitesId")] Medecin medecin)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                if (ModelState.IsValid)
                {
                    await MedecinAPI.AddMedecinAsync(medecin);
                    TempData["success"] = "Medecin creé avec sucssé";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["Specialites"] = new SelectList((await MedecinAPI.Instance.GetSpecialitesAsync()).Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");

                return View(medecin);
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // GET: Medecins/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var medecin = await MedecinAPI.Instance.GetMedecinAsync(id);
                if (medecin == null)
                {
                    return NotFound();
                }

                medecin.SpecialitesId = medecin.Specialites.Select(s => s.Id).ToList();

                ViewData["Specialites"] = new SelectList((await MedecinAPI.Instance.GetSpecialitesAsync()).Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");
                return View(medecin);
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // POST: Medecins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Email,NomMedecin,NewPassword,SpecialitesId,PrenomMedecin,Role")] Medecin medecin)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                if (id != medecin.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await MedecinAPI.UpdateMedecinAsync(medecin);
                    TempData["success"] = "Medecin modifié avec succes";

                    return RedirectToAction(nameof(Index));
                }

                ViewData["Specialites"] = new SelectList((await MedecinAPI.Instance.GetSpecialitesAsync()).Select(s => new SelectListItem { Text = s.Nom, Value = s.Id.ToString() }), "Value", "Text");

                return View(medecin);
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // GET: Medecins/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var medecin = await MedecinAPI.Instance.GetMedecinAsync(id);
                return View(medecin);
            }
            return RedirectToAction("SignIn", "Admins");
        }

        // POST: Medecins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (HttpContext.Session.GetString(SessionKeyRole) == "ROLE_ADMIN")
            {
                await MedecinAPI.DeleteMedecinAsync(id);

                TempData["success"] = "Medecin supprimé avec sucssé";

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("SignIn", "Admins");
        }
    }
}
