using System.ComponentModel.DataAnnotations;

namespace WebApp_Desafio_API.ViewModels
{
    public class DepartamentoRequest
    {
        public int id { get; set; }

        [Required(ErrorMessage = "A Descricao é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A Descricao deve ter entre {2} e {1} caracteres")]
        public string descricao { get; set; }
    }
}
