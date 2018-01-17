using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedicalMgmt.Models
{
    public class MedicalMgmtDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        //public DbSet<ProfileAssociation> ProfileAssociations { get; set; }
        public DbSet<Status> Statuses { get; set; }

        //public DbSet<Patient> Patients { get; set; }
    }
}