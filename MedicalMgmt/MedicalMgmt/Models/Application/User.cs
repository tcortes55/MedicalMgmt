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

        [Display(Name="Usuário")]
        [Required(ErrorMessage="Campo obrigatório")]
        [StringLength(15)]
        public string Username { get; set; }

        [Display(Name = "Primeiro nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenomes")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(200)]
        public string FamilyNames { get; set; }

        [Display(Name = "Telefone")]
        [Phone]
        public string Telephone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "RG")]
        [StringLength(20)]
        public string Rg { get; set; }

        [Display(Name = "CPF")]
        [StringLength(20)]
        public string Cpf { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(300)]
        public string Address { get; set; }

        [Display(Name = "Data de registro")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset RegisterDate { get; set; }

        [Display(Name = "Status")]
        [Required]
        public int StatusID { get; set; }
    }
}