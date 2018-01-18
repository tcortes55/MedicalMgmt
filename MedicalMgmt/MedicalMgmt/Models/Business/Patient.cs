using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class Patient
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Primeiro nome")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sobrenomes")]
        [StringLength(200)]
        public string FamilyNames { get; set; }

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

        [Required]
        [Display(Name = "Data de registro")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset RegisterDate { get; set; }
    }
}