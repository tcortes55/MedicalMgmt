using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Physician
    {
        public int UserID { get; set; }

        [Display(Name="Especialidade")]
        [Required]
        [StringLength(30)]
        public string Expertise { get; set; }

        [Display(Name = "GraduatedFrom")]
        [StringLength(30)]
        public string Expertise { get; set; }
    }
}