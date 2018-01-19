using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name="Usuário")]
        [StringLength(15)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome completo")]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Telefone")]
        [Phone(ErrorMessage="Insira um telefone válido")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage="Insira um e-mail válido")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "RG")]
        [StringLength(20)]
        public string Rg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CPF")]
        [StringLength(20)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Endereço")]
        [StringLength(300)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Data de registro")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset RegisterDate { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Active { get; set; }


        //public virtual Appointment Appointment { get; set; }
        //public virtual Physician Physician { get; set; }
        public virtual List<Physician> Physician { get; set; }
    }
}