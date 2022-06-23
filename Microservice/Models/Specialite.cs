using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microservice.Models
{
    public partial class Specialite
    {
        public Specialite()
        {
            ActeMedicaux = new List<ActeMedical>();
            PlageHoraires = new List<PlageHoraire>();
            Medecins = new List<Medecin>();
        }

        public long Id { get; set; }
        public string? Nom { get; set; }

        public virtual IList<ActeMedical> ActeMedicaux { get; set; }

        public virtual IList<PlageHoraire> PlageHoraires { get; set; }

        public virtual IList<Medecin> Medecins { get; set; }

        public override string ToString()
        {
            return Nom + " ";
        }
    }
}
