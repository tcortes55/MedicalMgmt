using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalMgmt.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required]
        public int PhysicianID { get; set; }
        //[ForeignKey("PhysicianID")]
        //public virtual Physician Physician { get; set; }

        [Required]
        public int PatientID { get; set; }
        //[ForeignKey("PatientID")]
        //public virtual Patient Patient { get; set; }

        [Required]
        public int UserID { get; set; }
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PlannedStartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PlannedEndDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ActualStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ActualEndDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        //[ForeignKey("StatusID")]
        //public virtual Status Status { get; set; }


        //public virtual PrescriptedExam PrescriptedExam { get; set; }
        //public virtual PrescriptedMedicine PrescriptedMedicine { get; set; }
    }
}