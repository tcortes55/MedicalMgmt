using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Medicine
    {
        [Key]
        public int ID { get; set; }

        [Display(Name="Nome Comercial")]
        [Required]
        [StringLength(30)]
        public string CommercialName { get; set; }

        [Display(Name = "Nome Genérico")]
        [Required]
        [StringLength(30)]
        public string GenericName { get; set; }

        [Display(Name = "Fabricante")]
        [Required]
        [StringLength(30)]
        public string Manufacturer { get; set; }

        [Display(Name = "Status")]
        [Required]
        public int StatusID { get; set; }
    }
}