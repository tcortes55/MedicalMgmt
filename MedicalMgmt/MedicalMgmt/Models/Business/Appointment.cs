using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalMgmt.Models.Business
{
    public class Appointment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PhysicianID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int UserID { get; set; }

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
        public DateTime ActualStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ActualEndDate { get; set; }

        [Required]
        public int StatusID { get; set; }
    }
}