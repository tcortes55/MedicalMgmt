﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class ProfileAssociation
    {
        [Key]
        public int ProfileAssociationID { get; set; }

        [Required]
        public int ProfileID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}