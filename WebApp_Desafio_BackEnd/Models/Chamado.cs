using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_BackEnd.Models
{
    [Serializable]
    public class Chamado
    {
        public static readonly Chamado Empty;

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "O Assunto é obrigatório")]
        [StringLength(150, ErrorMessage = "O Assunto deve ter no máximo {1} caracteres")]
        public string Assunto { get; set; }

        [Required(ErrorMessage = "O Solicitante é obrigatório")]
        [StringLength(100, ErrorMessage = "O Solicitante deve ter no máximo {1} caracteres")]
        public string Solicitante { get; set; }

        [Required(ErrorMessage = "O Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
        public int IdDepartamento { get; set; }
        public string Departamento { get; set; }

        public DateTime DataAbertura { get; set; }
    }
}
