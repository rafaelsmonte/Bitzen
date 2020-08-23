using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeBitzen.Data;

namespace testeBitzen.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatorio")]
        [MaxLength(100, ErrorMessage = "Nome deve conter entre 3 e 100 caracteres")]
        [MinLength(3, ErrorMessage = "Nome deve conter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatorio")]
        [CheckEmail(ErrorMessage = "Email inválido")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria")]
        [MinLength(3, ErrorMessage = "Senha deve conter entre 6 e 20 caracteres")]
        [MaxLength(20, ErrorMessage = "Nome deve conter entre 6 e 20 caracteres")]


        public string Senha { get; set; }



    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CheckEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (value == null)
                    return new ValidationResult(string.Format(this.ErrorMessage, 500));

                MailAddress m = new MailAddress(value.ToString());

                return ValidationResult.Success;
            }
            catch (FormatException)
            {
                return new ValidationResult(string.Format(this.ErrorMessage, 500));

            }
        }
    }

}


