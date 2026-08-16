using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public abstract class BaseDAL
    {
        protected static string CONNECTION_STRING = $"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Dados\\DesafioDB.db\";Version=3;";

        /// <summary>
        /// Executa um comando de INSERT/UPDATE/DELETE e retorna o número de linhas afetadas.
        /// </summary>
        protected int ExecuteNonQuery(string commandText, Action<SQLiteCommand> configurarParametros = null)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = commandText;
                configurarParametros?.Invoke(dbCommand);

                dbConnection.Open();
                return dbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executa uma consulta escalar (ex.: SELECT COUNT(*)) e retorna o resultado como inteiro.
        /// </summary>
        protected int ExecuteScalarInt(string commandText, Action<SQLiteCommand> configurarParametros = null)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = commandText;
                configurarParametros?.Invoke(dbCommand);

                dbConnection.Open();
                return Convert.ToInt32(dbCommand.ExecuteScalar());
            }
        }

        /// <summary>
        /// Executa uma consulta e mapeia cada linha do resultado usando a função informada.
        /// </summary>
        protected IEnumerable<TResult> ExecuteReader<TResult>(string commandText, Func<SQLiteDataReader, TResult> map, Action<SQLiteCommand> configurarParametros = null)
        {
            var resultados = new List<TResult>();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = commandText;
                configurarParametros?.Invoke(dbCommand);

                dbConnection.Open();

                using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                        resultados.Add(map(dataReader));
                }
            }

            return resultados;
        }

        /// <summary>
        /// Executa uma consulta e mapeia a primeira linha do resultado, ou retorna o valor padrão
        /// informado caso não haja nenhuma linha.
        /// </summary>
        protected TResult ExecuteReaderSingle<TResult>(string commandText, Func<SQLiteDataReader, TResult> map, TResult valorPadrao, Action<SQLiteCommand> configurarParametros = null)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = commandText;
                configurarParametros?.Invoke(dbCommand);

                dbConnection.Open();

                using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                        return map(dataReader);
                }
            }

            return valorPadrao;
        }
    }
}
