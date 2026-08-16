using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.Business
{
    public class ChamadosBLL
    {
        private ChamadosDAL dal = new ChamadosDAL();

        public IEnumerable<Chamado> ListarChamados()
        {
            return dal.ListarChamados();
        }

        public Chamado ObterChamado(int idChamado)
        {
            return dal.ObterChamado(idChamado);
        }

        public bool GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            var chamado = new Chamado()
            {
                ID = ID,
                Assunto = Assunto,
                Solicitante = Solicitante,
                IdDepartamento = IdDepartamento,
                DataAbertura = DataAbertura
            };

            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(chamado, new ValidationContext(chamado), validationResults, true))
                throw new ArgumentException(string.Join(" ", validationResults.Select(r => r.ErrorMessage)));

            if (ID == 0 && DataAbertura.Date < DateTime.Today)
                throw new ArgumentException("A Data de Abertura não pode ser retroativa.");

            return dal.GravarChamado(ID, Assunto, Solicitante, IdDepartamento, DataAbertura);
        }

        public bool ExcluirChamado(int idChamado)
        {
            return dal.ExcluirChamado(idChamado);
        }
    }
}
