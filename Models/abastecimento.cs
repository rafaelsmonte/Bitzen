using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testeBitzen.Models
{
    public class Abastecimento
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Quilometragem é obrigatoria")]
        [Range(1,int.MaxValue, ErrorMessage = "Quilometragem deve ser maior que zero")]
        public int KmDoAbastecimento { get; set; }

        [Required(ErrorMessage = "Valor é obrigatorio")]
        [Range(0.1,float.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public double ValorPago { get; set; }

        [Required(ErrorMessage = "Quantidade abastecida é obrigatoria")]
        [Range(0.1,float.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public double LitrosAbastecidos { get; set; }
        public DateTime DataDoAbastecimento {get; set;}

        [Required(ErrorMessage = "Posto é obrigatorio")]
        [MaxLength(100, ErrorMessage = "Posto deve conter entre 3 e 100 caracteres")]
        [MinLength(1, ErrorMessage = "Posto deve conter entre 3 e 100 caracteres")]
        public string Posto { get; set; }

        [Required(ErrorMessage = "Responsável é obrigatorio")]
        public int ResponsavelId { get; set; }

        public User Responsavel { get; set; }

        [Required(ErrorMessage = "Tipo do combustivel é obrigatorio")]
        public string TipoDoCombustivel { get; set; }




        [Required(ErrorMessage = "Veículo é obrigatorio")]
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

    }
}