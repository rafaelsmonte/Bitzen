using System;
using System.ComponentModel.DataAnnotations;

namespace testeBitzen.Models
{
    public class Veiculo
    {
        static int ano = DateTime.Now.Year;
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Marca é obrigatoria")]
        [MaxLength(50, ErrorMessage = "Marca deve conter entre 1 e 50 caracteres")]
        [MinLength(1, ErrorMessage = "Marca deve conter entre 1 e 50 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatorio")]
        [MaxLength(100, ErrorMessage = "Modelo deve conter entre 3 e 100 caracteres")]
        [MinLength(3, ErrorMessage = "Modelo deve conter entre 3 e 100 caracteres")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Ano é obrigatorio")]
        [Range(1,2021, ErrorMessage = "Ano tem que ser entra 1 e 2021")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "Placa é obrigatoria")]
        [MaxLength(7, ErrorMessage = "Placa deve conter 7 digitos")]
        [MinLength(7, ErrorMessage = "Placa deve conter 7 digitos")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Tipo do veículo é obrigatorio")]
        public string TipoDoVeiculo { get; set; }

        [Required(ErrorMessage = "Tipo do combustivel é obrigatorio")]
        public string TipoDeCombustivel { get; set; }

        [Required(ErrorMessage = "Quilometragem é obrigatoria")]
        [Range(1,int.MaxValue, ErrorMessage = "Quilometragem deve ser maior que zero")]
        public int KM { get; set; }
        [Required(ErrorMessage = "Resposável é obrigatorio")]
        public int ResponsavelId { get; set; }
        public User Responsavel { get; set; }


    }
}