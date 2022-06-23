using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Models
{
    public partial class ActeMedical
    {
        public ActeMedical()
        {
            PlageHoraires = new List<PlageHoraire>();
        }

        public long Id { get; set; }

        [Required(ErrorMessage = "Libelle requis")]
        public string? Libelle { get; set; }
        public long? SpecialiteId { get; set; }

        [Required(ErrorMessage = "Specialite requise")]
        public virtual Specialite? Specialite { get; set; }

        public virtual IList<PlageHoraire> PlageHoraires { get; set; }

        public override string ToString()
        {
            return Libelle;
        }
    }
}
