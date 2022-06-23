using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Models
{
    public partial class Medecin: IValidatableObject
    {
        public Medecin()
        {
            PlageHoraires = new List<PlageHoraire>();
            Specialites = new List<Specialite>();
            Role = "ROLE_MEDECIN";
        }
        [Key]
        public long? Id { get; set; }

        [Required(ErrorMessage = "Adresse mail requise")]
        [EmailAddress(ErrorMessage = "Format d'adresse mail invalide")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Nom requis")]
        [Display(Name = "Nom")]
        public string? NomMedecin { get; set; }

        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Nouveau mot de passe")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Prenom requis")]
        [Display(Name = "Prenom")]
        public string? PrenomMedecin { get; set; }
        public string? Role { get; set; }

        public virtual IList<PlageHoraire> PlageHoraires { get; set; }

        public virtual IList<Specialite> Specialites { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Specialites requises")]
        [Display(Name = "Specialites")]
        public IList<long>? SpecialitesId { get; set;}
      
        public string NomComplet 
        { 
            get { return  NomMedecin! + " " + PrenomMedecin!; }
        }

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
