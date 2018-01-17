using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Status
    {
        [Key]
        public int ID { get; set; }

        [Display(Name="Descrição")]
        [Required]
        [StringLength(30)]
        public string Description { get; set; }
    }
}