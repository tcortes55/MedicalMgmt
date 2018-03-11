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
        [ForeignKey("Appointment")]
        public int AppointmentID { get; set; }
        public virtual Appointment Appointment { get; set; }

        //[Required]
        [ForeignKey("Physician")]
        public int? PhysicianID { get; set; }
        public virtual Physician Physician { get; set; }

        //[Required]
        [ForeignKey("Patient")]
        public int? PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey("Exam")]
        [Display(Name = "Exame")]
        public int ExamID { get; set; }
        public virtual Exam Exam { get; set; }

        [StringLength(500)]
        [Display(Name = "Detalhes")]
        public string Details { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? ResultDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Resultado")]
        public string Result { get; set; }

        [Required]
        [Display(Name = "Enviar para o paciente")]
        public bool SendToPacient { get; set; }

        [Required]
        //[ForeignKey("Status")]
        public int StatusID { get; set; }
        //public virtual Status Status { get; set; }
    }
}