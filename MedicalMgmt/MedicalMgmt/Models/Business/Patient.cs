﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome completo")]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de nascimento")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Telefone")]
        [Phone]
        public string Telephone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido!")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "RG")]
        [StringLength(20)]
        public string Rg { get; set; }

        [Display(Name = "CPF")]
        [StringLength(20)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Endereço")]
        [StringLength(300)]
        public string Address { get; set; }

        [Display(Name = "Alergias")]
        [StringLength(300)]
        public string Allergies { get; set; }

        [Display(Name = "Histórico familiar")]
        [StringLength(500)]
        public string FamilyMedicalHistory { get; set; }

        [Display(Name = "Uso de medicamentos")]
        [StringLength(300)]
        public string LongTermMedication { get; set; }

        [Required]
        [Display(Name = "Data de registro")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset RegisterDate { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Active { get; set; }


        //public virtual List<Appointment> Appointment { get; set; }
        public virtual List<PrescriptedMedicine> PrescriptedMedicine { get; set; }
        public virtual List<PrescriptedExam> PrescriptedExam { get; set; }
    }
}