using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CoreHangfire.Repository.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        public DatabaseHelper(string hangfireConnectionString, string mySQLConnectionString)
        {
            this.HangfireConnectionString = hangfireConnectionString;

            this.MySQLConnectionString = mySQLConnectionString;
        }

        /// <summary>
        ///Hangfire連線字串
        /// </summary>
        public string HangfireConnectionString { get; }

        /// <summary>
        /// MySQL
        /// </summary>
        public string MySQLConnectionString { get; }

        public IDbConnection GetConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);

            return conn;
        }

        public IDbConnection GetMySQLConnection(string connectionString)
        {
            var conn = new MySqlConnection(connectionString);

            return conn;
        }
    }
}