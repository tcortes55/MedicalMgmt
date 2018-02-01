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
        public int StatusID { get; set; }

        [Required]
        [Display(Name="Descrição")]
        [StringLength(30)]
        public string StatusDescription { get; set; }


        public virtual List<Appointment> Appointment { get; set; }
        //public virtual List<PrescriptedExam> PrescriptedExam { get; set; }
    }
}