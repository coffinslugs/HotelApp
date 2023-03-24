using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T,U>(string sqlStatement,
                                     U parameters,
                                     string connectionStringName,
                                     dynamic options = null)
        {
            CommandType commandType = CommandType.Text;

            if (options.IsStoredProcedure != null && options.IsStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                dynamic options = null)
        {
            CommandType commandType = CommandType.Text;

            if (options.IsStoredProcedure != null && options.IsStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
