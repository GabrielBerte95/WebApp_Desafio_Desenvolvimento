using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace WebApp_Desafio_FrontEnd.ViewModels
{
    [DataContract]
    public class DepartamentoViewModel
    {
        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "A Descricao é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A Descricao deve ter entre {2} e {1} caracteres")]
        [Display(Name = "Descricao")]
        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

    }
}
