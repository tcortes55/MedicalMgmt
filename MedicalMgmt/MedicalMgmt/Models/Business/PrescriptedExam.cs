using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalMgmt.Models
{
    public class PrescriptedExam
    {
        [Key]
        public int PrescriptedExamID { get; set; }

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
        public int ExamID { get; set; }
        //[ForeignKey("ExamID")]
        //public virtual Exam Exam { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ExecutionDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ResultDate { get; set; }

        [StringLength(500)]
        public string Result { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        //[ForeignKey("StatusID")]
        //public virtual Status Status { get; set; }
    }
}