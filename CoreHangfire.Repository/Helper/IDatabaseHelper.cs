using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CoreHangfire.Repository.Helper
{
    public interface IDatabaseHelper
    {
        /// <summary>
        /// Hangfire連線字串
        /// </summary>
        string HangfireConnectionString { get; }

        /// <summary>
        /// MySQL
        /// </summary>
        string MySQLConnectionString { get; }

        IDbConnection GetConnection(string connectionString);

        IDbConnection GetMySQLConnection(string connectionString);
    }
}