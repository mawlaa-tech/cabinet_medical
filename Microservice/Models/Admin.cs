using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Models
{
    public partial class Admin
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Adresse mail requise")]
        [EmailAddress(ErrorMessage = "Format d'adresse mail invalide")]
        public string? Email { get; set; }

        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Mot de passe requis")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
