using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Physician
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [Display(Name="Especialidade")]
        [StringLength(30)]
        public string Expertise { get; set; }

        [Display(Name = "Graduado por")]
        [StringLength(30)]
        public string GraduatedFrom { get; set; }
    }
}