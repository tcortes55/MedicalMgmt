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
        public int GraduationYear { get; set; }


        public virtual User User { get; set; }
        public virtual List<Appointment> Appointment { get; set; }
        public virtual List<PrescriptedMedicine> PrescriptedMedicine { get; set; }
        public virtual List<PrescriptedExam> PrescriptedExam { get; set; }
    }
}