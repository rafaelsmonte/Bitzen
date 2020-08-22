using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria")]
        [MinLength(3, ErrorMessage = "Senha deve conter entre 6 e 20 caracteres")]
        [MaxLength(100, ErrorMessage = "Nome deve conter entre 6 e 20 caracteres")]
       

        public string Senha { get; set; }
       
    }
}