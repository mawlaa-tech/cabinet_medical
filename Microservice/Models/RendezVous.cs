using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Models
{
    public partial class RendezVous
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Patient requis")]
        public long? PatientId { get; set; }

        [Required(ErrorMessage = "Plage horaire requise")]
        public long? PlageHoraireId { get; set; }

        public virtual Patient? Patient { get; set; } = null!;

        public virtual PlageHoraire? PlageHoraire { get; set; } = null!;
    }
}
