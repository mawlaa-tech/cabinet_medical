using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Microservice.Models
{
    public partial class Patient: IValidatableObject
    {
        public Patient()
        {
            RendezVous = new List<RendezVous>();
            Role = "ROLE_PATIENT";
        }

        public long? Id { get; set; }

        [Required(ErrorMessage = "Date de naissance requise")]
        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime? DatNaisPat { get; set; }

        [Required(ErrorMessage = "Adresse mail requise")]
        [EmailAddress(ErrorMessage = "Format d'adresse mail invalide")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Nom requis")]
        [Display(Name = "Nom")]
        public string? NomPat { get; set; }

        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Nouveau mot de passe")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Numero de telephone requis")]
        [Phone(ErrorMessage = "Numero de telephone invalide")]
        [Display(Name = "Telephone")]
        public string? PhonePat { get; set; }

        [Required(ErrorMessage = "Prenom requis")]
        [Display(Name = "Prenom")]
        public string? PrenPat { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage = "Sexe requis")]
        [StringLength(5, ErrorMessage = "Sexe incorrecte")]
        [Display(Name = "Sexe")]
        public string? SexPat { get; set; }

        public virtual DossierFinancier? DossierFinancier { get; set; } = null!;

        public virtual DossierMedical? DossierMedical { get; set; } = null!;

        public virtual IList<RendezVous>? RendezVous { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == null)
            {
                if (Password == null)
                {
                    yield return new ValidationResult(
                        $"Mot de passe requis.",
                        new[] { nameof(Password) });
                }
                else if (Password.Length < 8)
                {
                    yield return new ValidationResult(
                        $"Mot de passe entre 8 et 30 requis",
                        new[] { nameof(Password) });
                }
                else if (Password.Length > 30)
                {
                    yield return new ValidationResult(
                        $"Mot de passe entre 8 et 30 requis",
                        new[] { nameof(Password) });
                }
            }
        }

        public IEnumerable<ValidationResult> ValidateNew(ValidationContext validationContext)
        {
            if (Id == null)
            {
                if (NewPassword == null)
                {
                    yield return new ValidationResult(
                        $"Mot de passe requis.",
                        new[] { nameof(NewPassword) });
                }
                else if (NewPassword.Length < 8)
                {
                    yield return new ValidationResult(
                        $"Mot de passe entre 8 et 30 requis",
                        new[] { nameof(NewPassword) });
                }
                else if (NewPassword.Length > 30)
                {
                    yield return new ValidationResult(
                        $"Mot de passe entre 8 et 30 requis",
                        new[] { nameof(NewPassword) });
                }
            }
        }
    }
}
