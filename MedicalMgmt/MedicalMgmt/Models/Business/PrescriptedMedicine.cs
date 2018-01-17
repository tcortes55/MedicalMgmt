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
        public int ID { get; set; }

        [Required]
        public int AppointmentID { get; set; }

        [Required]
        public int PhysicianID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int MedicineID { get; set; }

        [Required]
        [StringLength(300)]
        public string Posology { get; set; }
    }
}