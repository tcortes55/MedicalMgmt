using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class ProfileAssociation
    {
        public int ProfileID { get; set; }
        public int UserID { get; set; }
    }
}