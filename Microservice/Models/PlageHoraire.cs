using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Models
{
    public partial class PlageHoraire
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Date requise")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Heure de debut requise")]
        [Display(Name = "Heure de debut")]
        [DataType(DataType.Time)]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "Heure de fin requise")]
        [Display(Name = "Heure de fin")]
        [DataType(DataType.Time)]
        public TimeSpan HeureTer { get; set; }

        [NotMapped]
        public TimeSpan? Duree { get; set; }

        [NotMapped]
        [DataType(DataType.Time)]
        public TimeSpan? DebutPause { get; set; }

        [NotMapped]
        [DataType(DataType.Time)]
        public TimeSpan? FinPause { get; set; }

        [Required(ErrorMessage = "Acte medical requis")]
        [Display(Name = "Acte medical")]
        public long ActeMedicalId { get; set; }

        [Required(ErrorMessage = "Medecin requis")]
        [Display(Name = "Medecin")]
        public long MedecinId { get; set; }

        [Required(ErrorMessage = "Specialite requise")]
        [Display(Name = "Specialite")]
        public long SpecialiteId { get; set; }

        [Display(Name = "Acte medical")]
        public virtual ActeMedical? ActeMedical { get; set; } = null!;

        public virtual Medecin? Medecin { get; set; } = null!;

        public virtual Specialite? Specialite { get; set; } = null!;

        [Display(Name = "Rendez-vous")]
        public virtual RendezVous? RendezVous { get; set; } = null!;

        [Display(Name = "Specialite")]
        [NotMapped]
        public string? spe { get; set; } = null!;

        [Display(Name = "Acte medical")]
        [NotMapped]
        public string? acte { get; set; } = null!;

        public IEnumerable<ValidationResult> ValiderHeuresTravail(ValidationContext validationContext)
        {
            if (HeureTer < HeureDebut)
            {
                yield return new ValidationResult(
                    errorMessage: "Heure de debut apres heure de fin",
                    memberNames: new[] { "HeureDebut", "HeureTer" }
               );
            }
        }

        public IEnumerable<ValidationResult> ValiderHeuresPause(ValidationContext validationContext)
        {
            if (FinPause < DebutPause)
            {
                yield return new ValidationResult(
                    errorMessage: "Fin de pause apres debut de pause",
                    memberNames: new[] { "DebutPause", "FinPause" }
               );
            }
        }
    }
}
