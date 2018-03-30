namespace MedicalMgmt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a3 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Appointments",
            //    c => new
            //        {
            //            AppointmentID = c.Int(nullable: false, identity: true),
            //            PhysicianID = c.Int(nullable: false),
            //            PatientID = c.Int(nullable: false),
            //            AppUserID = c.Int(nullable: false),
            //            RegistrationDate = c.DateTime(nullable: false),
            //            PlannedStartDate = c.DateTime(nullable: false),
            //            PlannedEndDate = c.DateTime(nullable: false),
            //            Anamnesis = c.String(maxLength: 500),
            //            PatientArrivingDate = c.DateTime(),
            //            ActualStartDate = c.DateTime(),
            //            ActualEndDate = c.DateTime(),
            //            StatusID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.AppointmentID)
            //    .ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: true)
            //    .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
            //    .ForeignKey("dbo.Physicians", t => t.PhysicianID, cascadeDelete: true)
            //    .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
            //    .Index(t => t.PhysicianID)
            //    .Index(t => t.PatientID)
            //    .Index(t => t.AppUserID)
            //    .Index(t => t.StatusID);
            
            //CreateTable(
            //    "dbo.AppUsers",
            //    c => new
            //        {
            //            AppUserID = c.Int(nullable: false, identity: true),
            //            AspNetUserId = c.String(nullable: false),
            //            Username = c.String(nullable: false, maxLength: 15),
            //            FullName = c.String(nullable: false, maxLength: 200),
            //            Telephone = c.String(nullable: false),
            //            Email = c.String(nullable: false, maxLength: 50),
            //            Rg = c.String(nullable: false, maxLength: 20),
            //            Cpf = c.String(nullable: false, maxLength: 20),
            //            Address = c.String(nullable: false, maxLength: 300),
            //            RegisterDate = c.DateTimeOffset(nullable: false, precision: 7),
            //            Active = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.AppUserID);
            
            //CreateTable(
            //    "dbo.Physicians",
            //    c => new
            //        {
            //            PhysicianID = c.Int(nullable: false),
            //            Expertise = c.String(nullable: false, maxLength: 30),
            //            GraduationUni = c.String(maxLength: 50),
            //            GraduationYear = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PhysicianID)
            //    .ForeignKey("dbo.AppUsers", t => t.PhysicianID)
            //    .Index(t => t.PhysicianID);
            
            //CreateTable(
            //    "dbo.PrescriptedExams",
            //    c => new
            //        {
            //            PrescriptedExamID = c.Int(nullable: false, identity: true),
            //            AppointmentID = c.Int(nullable: false),
            //            PhysicianID = c.Int(),
            //            PatientID = c.Int(),
            //            ExamID = c.Int(nullable: false),
            //            Details = c.String(maxLength: 500),
            //            ResultDate = c.DateTime(),
            //            Result = c.String(maxLength: 500),
            //            SendToPacient = c.Boolean(nullable: false),
            //            StatusID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PrescriptedExamID)
            //    .ForeignKey("dbo.Appointments", t => t.AppointmentID, cascadeDelete: true)
            //    .ForeignKey("dbo.Exams", t => t.ExamID, cascadeDelete: true)
            //    .ForeignKey("dbo.Patients", t => t.PatientID)
            //    .ForeignKey("dbo.Physicians", t => t.PhysicianID)
            //    .Index(t => t.AppointmentID)
            //    .Index(t => t.PhysicianID)
            //    .Index(t => t.PatientID)
            //    .Index(t => t.ExamID);
            
            //CreateTable(
            //    "dbo.Exams",
            //    c => new
            //        {
            //            ExamID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.ExamID);
            
            //CreateTable(
            //    "dbo.Patients",
            //    c => new
            //        {
            //            PatientID = c.Int(nullable: false, identity: true),
            //            FullName = c.String(nullable: false, maxLength: 200),
            //            BirthDate = c.DateTime(nullable: false),
            //            Telephone = c.String(nullable: false),
            //            Email = c.String(maxLength: 50),
            //            Rg = c.String(maxLength: 20),
            //            Cpf = c.String(maxLength: 20),
            //            Address = c.String(nullable: false, maxLength: 300),
            //            Allergies = c.String(maxLength: 300),
            //            FamilyMedicalHistory = c.String(maxLength: 500),
            //            LongTermMedication = c.String(maxLength: 300),
            //            RegisterDate = c.DateTimeOffset(nullable: false, precision: 7),
            //            Active = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PatientID);
            
            //CreateTable(
            //    "dbo.PrescriptedMedicines",
            //    c => new
            //        {
            //            PrescriptedMedicineID = c.Int(nullable: false, identity: true),
            //            AppointmentID = c.Int(nullable: false),
            //            PhysicianID = c.Int(),
            //            PatientID = c.Int(),
            //            MedicineID = c.Int(nullable: false),
            //            Posology = c.String(nullable: false, maxLength: 300),
            //        })
            //    .PrimaryKey(t => t.PrescriptedMedicineID)
            //    .ForeignKey("dbo.Appointments", t => t.AppointmentID, cascadeDelete: true)
            //    .ForeignKey("dbo.Medicines", t => t.MedicineID, cascadeDelete: true)
            //    .ForeignKey("dbo.Patients", t => t.PatientID)
            //    .ForeignKey("dbo.Physicians", t => t.PhysicianID)
            //    .Index(t => t.AppointmentID)
            //    .Index(t => t.PhysicianID)
            //    .Index(t => t.PatientID)
            //    .Index(t => t.MedicineID);
            
            //CreateTable(
            //    "dbo.Medicines",
            //    c => new
            //        {
            //            MedicineID = c.Int(nullable: false, identity: true),
            //            CommercialName = c.String(nullable: false, maxLength: 30),
            //            GenericName = c.String(nullable: false, maxLength: 30),
            //            Manufacturer = c.String(nullable: false, maxLength: 30),
            //            Active = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.MedicineID);
            
            //CreateTable(
            //    "dbo.Status",
            //    c => new
            //        {
            //            StatusID = c.Int(nullable: false, identity: true),
            //            StatusDescription = c.String(nullable: false, maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.StatusID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            //DropForeignKey("dbo.Appointments", "StatusID", "dbo.Status");
            //DropForeignKey("dbo.Appointments", "PhysicianID", "dbo.Physicians");
            //DropForeignKey("dbo.Appointments", "PatientID", "dbo.Patients");
            //DropForeignKey("dbo.Appointments", "AppUserID", "dbo.AppUsers");
            //DropForeignKey("dbo.PrescriptedExams", "PhysicianID", "dbo.Physicians");
            //DropForeignKey("dbo.PrescriptedExams", "PatientID", "dbo.Patients");
            //DropForeignKey("dbo.PrescriptedMedicines", "PhysicianID", "dbo.Physicians");
            //DropForeignKey("dbo.PrescriptedMedicines", "PatientID", "dbo.Patients");
            //DropForeignKey("dbo.PrescriptedMedicines", "MedicineID", "dbo.Medicines");
            //DropForeignKey("dbo.PrescriptedMedicines", "AppointmentID", "dbo.Appointments");
            //DropForeignKey("dbo.PrescriptedExams", "ExamID", "dbo.Exams");
            //DropForeignKey("dbo.PrescriptedExams", "AppointmentID", "dbo.Appointments");
            //DropForeignKey("dbo.Physicians", "PhysicianID", "dbo.AppUsers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            //DropIndex("dbo.PrescriptedMedicines", new[] { "MedicineID" });
            //DropIndex("dbo.PrescriptedMedicines", new[] { "PatientID" });
            //DropIndex("dbo.PrescriptedMedicines", new[] { "PhysicianID" });
            //DropIndex("dbo.PrescriptedMedicines", new[] { "AppointmentID" });
            //DropIndex("dbo.PrescriptedExams", new[] { "ExamID" });
            //DropIndex("dbo.PrescriptedExams", new[] { "PatientID" });
            //DropIndex("dbo.PrescriptedExams", new[] { "PhysicianID" });
            //DropIndex("dbo.PrescriptedExams", new[] { "AppointmentID" });
            //DropIndex("dbo.Physicians", new[] { "PhysicianID" });
            //DropIndex("dbo.Appointments", new[] { "StatusID" });
            //DropIndex("dbo.Appointments", new[] { "AppUserID" });
            //DropIndex("dbo.Appointments", new[] { "PatientID" });
            //DropIndex("dbo.Appointments", new[] { "PhysicianID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            //DropTable("dbo.Status");
            //DropTable("dbo.Medicines");
            //DropTable("dbo.PrescriptedMedicines");
            //DropTable("dbo.Patients");
            //DropTable("dbo.Exams");
            //DropTable("dbo.PrescriptedExams");
            //DropTable("dbo.Physicians");
            //DropTable("dbo.AppUsers");
            //DropTable("dbo.Appointments");
        }
    }
}
