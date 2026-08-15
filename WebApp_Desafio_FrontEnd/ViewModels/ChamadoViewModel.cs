using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace WebApp_Desafio_FrontEnd.ViewModels
{
    [DataContract]
    public class ChamadoViewModel
    {
        private CultureInfo ptBR = new CultureInfo("pt-BR");

        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O Assunto é obrigatório")]
        [StringLength(150, ErrorMessage = "O Assunto deve ter no máximo {1} caracteres")]
        [Display(Name = "Assunto")]
        [DataMember(Name = "Assunto")]
        public string Assunto { get; set; }

        [Required(ErrorMessage = "O Solicitante é obrigatório")]
        [StringLength(100, ErrorMessage = "O Solicitante deve ter no máximo {1} caracteres")]
        [Display(Name = "Solicitante")]
        [DataMember(Name = "Solicitante")]
        public string Solicitante { get; set; }

        [Required(ErrorMessage = "O Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um Departamento")]
        [Display(Name = "IdDepartamento")]
        [DataMember(Name = "IdDepartamento")]
        public int IdDepartamento { get; set; }

        [Display(Name = "Departamento")]
        [DataMember(Name = "Departamento")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "A Data de Abertura é obrigatória")]
        [Display(Name = "DataAbertura")]
        [DataMember(Name = "DataAbertura")]
        public DateTime DataAbertura { get; set; }

        [DataMember(Name = "DataAberturaWrapper")]
        public string DataAberturaWrapper
        {
            get
            {
                return DataAbertura.ToString("d", ptBR);
            }
            set
            {
                DataAbertura = DateTime.Parse(value, ptBR);
            }
        }
    }
}
