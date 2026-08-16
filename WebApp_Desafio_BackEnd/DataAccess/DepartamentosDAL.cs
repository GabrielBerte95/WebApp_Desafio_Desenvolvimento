using System.Collections.Generic;
using System.Data.SQLite;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : BaseDAL
    {
        private const string SELECT_DEPARTAMENTOS = "SELECT ID, Descricao FROM departamentos ";

        private static Departamento MapDepartamento(SQLiteDataReader dataReader)
        {
            var departamento = new Departamento();

            if (!dataReader.IsDBNull(0))
                departamento.ID = dataReader.GetInt32(0);
            if (!dataReader.IsDBNull(1))
                departamento.Descricao = dataReader.GetString(1);

            return departamento;
        }

        public IEnumerable<Departamento> ListarDepartamentos()
        {
            return ExecuteReader(SELECT_DEPARTAMENTOS, MapDepartamento);
        }

        public Departamento ObterDepartamento(int idDepartamento)
        {
            return ExecuteReaderSingle(
                SELECT_DEPARTAMENTOS + "WHERE ID = @ID",
                MapDepartamento,
                Departamento.Empty,
                cmd => cmd.Parameters.AddWithValue("@ID", idDepartamento));
        }

        public bool GravarDepartamento(int ID, string Descricao)
        {
            string commandText = (ID == 0)
                ? "INSERT INTO departamentos (Descricao)" +
                  "VALUES (@Descricao)"
                : "UPDATE departamentos " +
                  "SET Descricao=@Descricao " +
                  "WHERE ID=@ID ";

            int regsAfetados = ExecuteNonQuery(commandText, cmd =>
            {
                cmd.Parameters.AddWithValue("@Descricao", Descricao);
                cmd.Parameters.AddWithValue("@ID", ID);
            });

            return (regsAfetados > 0);
        }

        public bool ExcluirDepartamento(int idDepartamento)
        {
            int regsAfetados = ExecuteNonQuery(
                "DELETE FROM departamentos WHERE ID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", idDepartamento));

            return (regsAfetados > 0);
        }
    }
}
