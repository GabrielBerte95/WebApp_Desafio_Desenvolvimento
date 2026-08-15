using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação da chamada
    /// </summary>
    public class ChamadoRequest
    {
        /// <summary>
        /// ID do Chamado
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Assunto do Chamado
        /// </summary>
        [Required(ErrorMessage = "O Assunto é obrigatório")]
        [StringLength(150, ErrorMessage = "O Assunto deve ter no máximo {1} caracteres")]
        public string assunto { get; set; }

        /// <summary>
        /// Solicitante do Chamado
        /// </summary>
        [Required(ErrorMessage = "O Solicitante é obrigatório")]
        [StringLength(100, ErrorMessage = "O Solicitante deve ter no máximo {1} caracteres")]
        public string solicitante { get; set; }

        /// <summary>
        /// ID do Departamento do Chamado
        /// </summary>
        [Required(ErrorMessage = "O Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
        public int idDepartamento { get; set; }

        /// <summary>
        /// Data de Abertura do Chamado
        /// </summary>
        public DateTime dataAbertura { get; set; }
    }
}
