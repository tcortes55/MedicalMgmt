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

        [Required]
        [Display(Name="Nome Comercial")]
        [StringLength(30)]
        public string CommercialName { get; set; }

        [Required]
        [Display(Name = "Nome Genérico")]
        [StringLength(30)]
        public string GenericName { get; set; }

        [Required]
        [Display(Name = "Fabricante")]
        [StringLength(30)]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Active { get; set; }


        //public virtual PrescriptedMedicine PrescriptedMedicine { get; set; }
    }
}