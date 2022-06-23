#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microservice.Models;
using Web.ControllersAPI;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Web.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ILogger<PatientsController> _logger;
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";

        public AdminsController(ILogger<PatientsController> logger)
        {
            _logger = logger;
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
        public async Task<IActionResult> SignIn([Bind("Email,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                var user = await AdminAPI.Instance.Login(admin);
                if (user != null)
                {
                    HttpContext.Session.SetString(SessionKeyId, (user.Id.ToString()));
                    HttpContext.Session.SetString(SessionKeyRole, user.Role);
                    return RedirectToAction("Index", "Medecins");
                }
                else
                {
                    ModelState.AddModelError("Password", "Adresse mail ou mot de passe incorrect");
                    ModelState.AddModelError("Email", "Adresse mail ou mot de passe incorrect");
                }
            }

            return View();
        }
    }
 
}
