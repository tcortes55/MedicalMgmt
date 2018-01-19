using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalMgmt.Models
{
    public class Physician
    {
        //http://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx
        [Key,ForeignKey("User")]
        public int PhysicianID { get; set; }

        [Required]
        [Display(Name="Especialidade")]
        [StringLength(30)]
        public string Expertise { get; set; }

        [Display(Name = "Formado por")]
        [StringLength(50)]
        public string GraduationUni { get; set; }

        [Display(Name = "Ano de graduação")]
        public DateTime? GraduationYear { get; set; }


        public virtual User User { get; set; }
        //public virtual Appointment Appointment { get; set; }
        //public virtual PrescriptedExam PrescriptedExam { get; set; }
        //public virtual PrescriptedMedicine PrescriptedMedicine { get; set; }
    }
}