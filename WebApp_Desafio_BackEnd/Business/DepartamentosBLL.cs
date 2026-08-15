using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.Business
{
    public class DepartamentosBLL
    {
        private DepartamentosDAL dal = new DepartamentosDAL();

        public IEnumerable<Departamento> ListarDepartamentos()
        {
            return dal.ListarDepartamentos();
        }

        public Departamento ObterDepartamento(int idDepartamento)
        {
            return dal.ObterDepartamento(idDepartamento);
        }

        public bool GravarDepartamento(int ID, string Descricao)
        {
            var departamento = new Departamento()
            {
                ID = ID,
                Descricao = Descricao
            };

            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(departamento, new ValidationContext(departamento), validationResults, true))
                throw new ArgumentException(string.Join(" ", validationResults.Select(r => r.ErrorMessage)));

            return dal.GravarDepartamento(ID, Descricao);
        }

        public bool ExcluirDepartamento(int idDepartamento)
        {
            return dal.ExcluirDepartamento(idDepartamento);
        }
    }
}
