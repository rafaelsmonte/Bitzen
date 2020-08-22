using System.ComponentModel.DataAnnotations;

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
        public string Senha { get; set; }

    }
}