using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Profile
    {
        [Key]
        public int ID { get; set; }

        [Display(Name="Nome")]
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}