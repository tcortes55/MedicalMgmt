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

        public const string PROFILE_PHYSICIAN = "MEDICO";

        //public const string[] schedule = {
        //                                     "08:00","08:20","08:40",
        //                                     "09:00","09:20","09:40",
        //                                     "10:00","10:20","10:40",
        //                                     "11:00","11:20","11:40",
        //                                     "13:00","13:20","13:40",
        //                                     "14:00","14:20","14:40",
        //                                     "15:00","15:20","15:40",
        //                                     "16:00","16:20","16:40",
        //                                     "17:00","17:20","17:40"
        //                                 };
    }
}