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
        [Key]
        public int PhysicianID { get; set; }

        //[Required]
        //[Key,ForeignKey("ID")]
        //public int UserID { get; set; }
        //public virtual User User { get; set; }

        //[Key]
        //public int ID { get; set; }

        ////https://stackoverflow.com/questions/10108160/entity-framework-code-first-setting-up-one-to-one-foreign-key-association-usin?noredirect=1&lq=1
        //[ForeignKey("ID")]
        //[Required]
        //public virtual User User { get; set; }



        [Required]
        [Display(Name="Especialidade")]
        [StringLength(30)]
        public string Expertise { get; set; }

        [Display(Name = "Formado por")]
        [StringLength(50)]
        public string GraduationUni { get; set; }

        [Display(Name = "Ano de graduação")]
        public DateTime? GraduationYear { get; set; }


        //public virtual Appointment Appointment { get; set; }
        //public virtual PrescriptedExam PrescriptedExam { get; set; }
        //public virtual PrescriptedMedicine PrescriptedMedicine { get; set; }
    }
}