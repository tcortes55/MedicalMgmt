﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using MedicalMgmt.Models;

namespace MedicalMgmt.ViewModels
{
    public class SelectPhysicianData
    {
        public Patient Patient { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public Appointment Appointment { get; set; }
    }
}
