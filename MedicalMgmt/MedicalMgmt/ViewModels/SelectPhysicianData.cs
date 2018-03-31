using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedicalMgmt.Models;

namespace MedicalMgmt.ViewModels
{
    public class SelectPhysicianData
    {
        // Patient for which an appointment will be scheduled
        public Patient Patient { get; set; }

        // List of active physicians
        public PagedList.IPagedList<Physician> Physicians { get; set; }

        // Appointment that will be created
        public Appointment Appointment { get; set; }
    }
}
