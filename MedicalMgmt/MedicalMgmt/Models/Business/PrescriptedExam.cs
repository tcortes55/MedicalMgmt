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
        public int ID { get; set; }

        [Required]
        public int AppointmentID { get; set; }

        [Required]
        public int PhysicianID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        [StringLength(500)]
        public string Result { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }
    }
}