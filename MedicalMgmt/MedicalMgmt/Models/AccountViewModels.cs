using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalMgmt.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Perfil")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome completo")]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de nascimento")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Telefone")]
        [Phone(ErrorMessage = "Insira um telefone válido")]
        public string PhoneNumber { get; set; }

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
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
