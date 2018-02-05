using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedicalMgmt.Models
{
    public class MedicalMgmtDbContext : DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<MedicalMgmtDbContext>(null);
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        //public DbSet<ProfileAssociation> ProfileAssociations { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<PrescriptedMedicine> PrescriptedMedicines { get; set; }
        public DbSet<PrescriptedExam> PrescriptedExams { get; set; }
    }
}