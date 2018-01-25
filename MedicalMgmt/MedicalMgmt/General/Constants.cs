using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalMgmt.General
{
    public class Constants
    {
        public const int SS_AP_PLANNED = 1;
        public const int SS_AP_PATIENT_WAITING = 2;
        public const int SS_AP_ONGOING = 3;
        public const int SS_AP_FINISHED = 4;
        public const int SS_AP_CANCELED = 5;

        public const int SS_EX_REQUESTED = 6;
        public const int SS_EX_FINISHED = 7;
    }
}