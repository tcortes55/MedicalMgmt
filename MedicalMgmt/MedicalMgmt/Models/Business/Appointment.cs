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
        //https://stackoverflow.com/questions/17127351/introducing-foreign-key-constraint-may-cause-cycles-or-multiple-cascade-paths
        [Key]
        public int AppointmentID { get; set; }

        [Required]
        [ForeignKey("Physician")]
        public int PhysicianID { get; set; }
        public virtual Physician Physician { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PlannedStartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PlannedEndDate { get; set; }

        [Display(Name = "Anamnese")]
        [StringLength(500)]
        public string Anamnesis { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PatientArrivingDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ActualStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ActualEndDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        //[ForeignKey("Status")]
        public int StatusID { get; set; }
        //public virtual Status Status { get; set; }


        public virtual List<PrescriptedMedicine> PrescriptedMedicine { get; set; }
        public virtual List<PrescriptedExam> PrescriptedExam { get; set; }
    }
}