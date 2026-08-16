using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : BaseDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd";

        private const string SELECT_CHAMADOS =
            "SELECT chamados.ID, " +
            "       Assunto, " +
            "       Solicitante, " +
            "       IdDepartamento, " +
            "       departamentos.Descricao AS Departamento, " +
            "       DataAbertura " +
            "FROM chamados " +
            "INNER JOIN departamentos " +
            "   ON chamados.IdDepartamento = departamentos.ID ";

        private static Chamado MapChamado(SQLiteDataReader dataReader)
        {
            var chamado = new Chamado();

            if (!dataReader.IsDBNull(0))
                chamado.ID = dataReader.GetInt32(0);
            if (!dataReader.IsDBNull(1))
                chamado.Assunto = dataReader.GetString(1);
            if (!dataReader.IsDBNull(2))
                chamado.Solicitante = dataReader.GetString(2);
            if (!dataReader.IsDBNull(3))
                chamado.IdDepartamento = dataReader.GetInt32(3);
            if (!dataReader.IsDBNull(4))
                chamado.Departamento = dataReader.GetString(4);
            if (!dataReader.IsDBNull(5))
                chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));

            return chamado;
        }

        public IEnumerable<Chamado> ListarChamados()
        {
            return ExecuteReader(SELECT_CHAMADOS, MapChamado);
        }

        public Chamado ObterChamado(int idChamado)
        {
            return ExecuteReaderSingle(
                SELECT_CHAMADOS + "WHERE chamados.ID = @ID",
                MapChamado,
                Chamado.Empty,
                cmd => cmd.Parameters.AddWithValue("@ID", idChamado));
        }

        public bool GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            string commandText = (ID == 0)
                ? "INSERT INTO chamados (Assunto,Solicitante,IdDepartamento,DataAbertura)" +
                  "VALUES (@Assunto,@Solicitante,@IdDepartamento,@DataAbertura)"
                : "UPDATE chamados " +
                  "SET Assunto=@Assunto, " +
                  "    Solicitante=@Solicitante, " +
                  "    IdDepartamento=@IdDepartamento, " +
                  "    DataAbertura=@DataAbertura " +
                  "WHERE ID=@ID ";

            int regsAfetados = ExecuteNonQuery(commandText, cmd =>
            {
                cmd.Parameters.AddWithValue("@Assunto", Assunto);
                cmd.Parameters.AddWithValue("@Solicitante", Solicitante);
                cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                cmd.Parameters.AddWithValue("@DataAbertura", DataAbertura.ToString(ANSI_DATE_FORMAT));
                cmd.Parameters.AddWithValue("@ID", ID);
            });

            return (regsAfetados > 0);
        }

        public bool ExcluirChamado(int idChamado)
        {
            int regsAfetados = ExecuteNonQuery(
                "DELETE FROM chamados WHERE ID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", idChamado));

            return (regsAfetados > 0);
        }

        public bool ExisteChamadoPorDepartamento(int idDepartamento)
        {
            int total = ExecuteScalarInt(
                "SELECT COUNT(*) FROM chamados WHERE IdDepartamento = @IdDepartamento",
                cmd => cmd.Parameters.AddWithValue("@IdDepartamento", idDepartamento));

            return total > 0;
        }
    }
}
