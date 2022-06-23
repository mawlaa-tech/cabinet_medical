using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microservice.Models
{
    public partial class DossierMedical
    {
        public long Id { get; set; }
        public string? NumDossier { get; set; }
        public long PatientId { get; set; }

        public virtual Patient Patient { get; set; } = null!;
    }
}
