using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models.Business
{
    public class Appointment
    {
        [Key]
        public int ID { get; set; }
    }
}