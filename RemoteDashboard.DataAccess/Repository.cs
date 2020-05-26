using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteDashboard.DataAccess.Interfaces;
using RemoteDashboard.Models.BaseTypes;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace RemoteDashboard.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly IDbConnection dbConnection;

        public Repository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }
        
        public int Add(T entity, string sqlQuery)
        {
           int rowsEffected = dbConnection.Execute(sqlQuery, entity);
            return rowsEffected;
        }

        public int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            throw new NotImplementedException();
        }
       
    }
}
