using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalMgmt.Models
{
    public class PrescriptedMedicine
    {
        [Key]
        public int PrescriptedMedicineID { get; set; }

        [Required]
        public int AppointmentID { get; set; }
        //[ForeignKey("AppointmentID")]
        //public virtual Appointment Appointment { get; set; }

        [Required]
        public int PhysicianID { get; set; }
        //[ForeignKey("PhysicianID")]
        //public virtual Physician Physician { get; set; }

        [Required]
        public int PatientID { get; set; }
        //[ForeignKey("PatientID")]
        //public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineID { get; set; }
        public virtual Medicine Medicine { get; set; }

        [Required]
        [StringLength(300)]
        public string Posology { get; set; }
    }
}